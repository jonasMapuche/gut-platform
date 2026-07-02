using Android.Content;
using Android.Media;
using Android.OS;
using Android.Telecom;
using Android.Telephony;
using Gut.Interfaces;
using Gut.Platforms.Android.Broadcasts;
using Java.IO;
using Message = Gut.Models.Message;
using Stream = Android.Media.Stream;
using Uri = Android.Net.Uri;

namespace Gut.Platforms.Android.Services
{
    public class PhoneService : IPhoneService
    {
        public List<Message> Receiver { get; set; }

        public PhoneService()
        {
            this.Receiver = new List<Message>();
        }

        public void Call(string number)
        {
            try
            {
                Uri? uri = Uri.Parse($"tel:{number}");
                Intent intent = new Intent(Intent.ActionCall, uri);
                intent.AddFlags(ActivityFlags.NewTask);
                Platform.AppContext.StartActivity(intent);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void Call(string numero, string caminhoAudio)
        {
            try
            {
                Context context = Platform.AppContext;
                TelecomManager? telecomManager = (TelecomManager)context.GetSystemService(Context.TelecomService);
                Uri? uri = Uri.FromParts(PhoneAccount.SchemeTel, numero, null);
                
                Bundle extras = new Bundle();
                PhoneAccountHandle callHandle = new PhoneAccountHandle(new ComponentName(context, Java.Lang.Class.FromType(typeof(PhoneConnectionService))), "MEU_ID_CONTA");

                extras.PutParcelable(TelecomManager.ExtraPhoneAccountHandle, callHandle);
                telecomManager.PlaceCall(uri, extras);

                InjectAudio(caminhoAudio);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void Scan()
        {
            try
            {
                PhoneBroadcast receiver = new PhoneBroadcast();
                this.Receiver = receiver.Receiver;
                IntentFilter filter = new IntentFilter(TelephonyManager.ActionPhoneStateChanged);
                Platform.AppContext.RegisterReceiver(receiver, filter);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private void InjectAudio(string filePath)
        {
            try
            {
                int sampleRate = 44100;
                ChannelIn channelIn = ChannelIn.Mono;
                Encoding encoding = Encoding.Pcm16bit;

                int bufferSize = AudioRecord.GetMinBufferSize(sampleRate, channelIn, encoding);
                var audioTrack = new AudioTrack(
                    Stream.VoiceCall,
                    sampleRate,
                    ChannelOut.Mono,
                    encoding,
                    bufferSize,
                    AudioTrackMode.Stream);

                audioTrack.Play();

                //File file = new File(filePath);
                FileInputStream fis = new FileInputStream(filePath);
                byte[] buffer = new byte[bufferSize];
                int bytesRead;

                try
                {
                    while ((bytesRead = fis.Read(buffer)) != -1)
                    {
                        audioTrack.Write(buffer, 0, bytesRead);
                    }
                }
                catch (Java.IO.IOException e) 
                { 
                    e.PrintStackTrace(); 
                }
                finally
                {
                    audioTrack.Stop();
                    audioTrack.Release();
                    fis.Close();
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private void IniciarInjecaoAudio()
        {
            int sampleRate = 16000;
            var channelConfig = ChannelOut.Mono;
            var audioFormat = Encoding.Pcm16bit;

            int bufferSize = AudioTrack.GetMinBufferSize(sampleRate, channelConfig, audioFormat);

            AudioTrack audioTrack = new AudioTrack(
                Stream.VoiceCall,
                sampleRate,
                channelConfig,
                audioFormat,
                bufferSize,
                AudioTrackMode.Stream
            );

            audioTrack.Play();

            System.Threading.Tasks.Task.Run(() =>
            {
                // Exemplo: byte[] audioData = File.ReadAllBytes("caminho_do_arquivo.pcm");
                byte[] audioData = new byte[bufferSize];
                int bytesWritten = 0;
                while (bytesWritten < audioData.Length)
                {
                    int result = audioTrack.Write(audioData, bytesWritten, audioData.Length - bytesWritten);
                    if (result < 0) break; 
                    bytesWritten += result;
                }
                audioTrack.Stop();
                audioTrack.Release();
            });
        }

        private AudioTrack _audioTrack;
        private bool _isPlaying;

        private void StartInjectingAudio(Context context, string audioFilePath)
        {
            AudioManager audioManager = (AudioManager)context.GetSystemService(Context.AudioService);
            audioManager.Mode = Mode.InCommunication;
            audioManager.SpeakerphoneOn = false;

            int sampleRate = 44100; 
            ChannelOut channelConfig = ChannelOut.Mono;
            Encoding audioFormat = Encoding.Pcm16bit;

            int bufferSize = AudioTrack.GetMinBufferSize(sampleRate, channelConfig, audioFormat);
            _audioTrack = new AudioTrack(
                Stream.VoiceCall,
                sampleRate,
                channelConfig,
                audioFormat,
                bufferSize,
                AudioTrackMode.Stream
            );

            _audioTrack.Play();
            _isPlaying = true;

            Thread audioThread = new Thread(() =>
            {
                byte[] buffer = new byte[bufferSize];
                using (FileInputStream fis = new FileInputStream(audioFilePath))
                {
                    while (_isPlaying)
                    {
                        int bytesRead = fis.Read(buffer, 0, buffer.Length);
                        if (bytesRead > 0)
                        {
                            _audioTrack.Write(buffer, 0, bytesRead);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            });
            audioThread.Start();
        }

        private void StopInjectingAudio()
        {
            _isPlaying = false;
            if (_audioTrack != null)
            {
                _audioTrack.Stop();
                _audioTrack.Release();
                _audioTrack = null;
            }
        }
    }
}
