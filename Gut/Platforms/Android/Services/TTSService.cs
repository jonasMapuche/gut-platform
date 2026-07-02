using Android.Runtime;
using Android.Speech.Tts;
using Gut.Interfaces;
using TextToSpeech = Android.Speech.Tts.TextToSpeech;

namespace Gut.Platforms.Android.Services
{
    public class TTSService : Java.Lang.Object, ITTSService, TextToSpeech.IOnInitListener
    {
        private TextToSpeech? _textToSpeech;
        private string? _text;

        public TTSService()
        {
            try
            {
                this._textToSpeech = new TextToSpeech(Platform.AppContext, this);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            try
            {
                if (status == OperationResult.Success)
                {
                    if (!string.IsNullOrEmpty(this._text))
                        this._textToSpeech.Speak(this._text, QueueMode.Flush, null, null);
                }
                else
                    throw new InvalidOperationException("Error operation!");
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public string FileText(string text, string file_path)
        {
            try
            {
                this._text = text;
                OperationResult result = OperationResult.Error;
                if (this._textToSpeech != null && this._textToSpeech.IsSpeaking == false)
                {
                    Dictionary<string, string> parameter = new Dictionary<string, string>();
                    parameter.Add(TextToSpeech.Engine.KeyParamUtteranceId, "fileSynthesis");
                    result = this._textToSpeech.SynthesizeToFile(text, parameter, file_path);
                }
                if (result == OperationResult.Success) return file_path;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
