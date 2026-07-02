using CommunityToolkit.Maui.Storage;
using Gut.Interfaces;
using Gut.Models;
using Gut.Services.Interfaces;
using Plugin.Firebase.CloudMessaging;

namespace Gut.Services
{
    public class PerceptionService : IPerceptionService
    {
        IBluetoothService _bluetoothService;
        IWiFiService _wiFiService;
        IVPNClientService _vPNClientService;
        ISMSService _sMSService;
        IPhoneService _phoneService;
        ITTSService _tTSService;

        public PerceptionService(IBluetoothService bluetoothService, IWiFiService wiFiService, IVPNClientService vPNClientService, ISMSService sMSService, IPhoneService phoneService, ITTSService tTSService)
        {
            try
            {
                Permission();
                this._bluetoothService = bluetoothService;
                this._wiFiService = wiFiService;
                this._vPNClientService = vPNClientService;
                this._sMSService = sMSService;
                this._phoneService = phoneService;
                this._tTSService = tTSService;
                MountPath();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task<PermissionStatus> Permission()
        {
            try
            {
                PermissionStatus statusPhone = await Permissions.CheckStatusAsync<Permissions.Phone>();
                if (statusPhone != PermissionStatus.Granted)
                    await Permissions.RequestAsync<Permissions.Phone>();
                PermissionStatus statusSMS = await Permissions.CheckStatusAsync<Permissions.Sms>();
                if (statusSMS != PermissionStatus.Granted)
                    await Permissions.RequestAsync<Permissions.Sms>();
                PermissionStatus statusBluetooth = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
                if (statusBluetooth != PermissionStatus.Granted)
                    await Permissions.RequestAsync<Permissions.Bluetooth>();
                PermissionStatus statusStorare = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (statusStorare != PermissionStatus.Granted)
                    await Permissions.RequestAsync<Permissions.StorageWrite>();
                PermissionStatus statusRead = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                if (statusRead != PermissionStatus.Granted)
                    statusRead = await Permissions.RequestAsync<Permissions.StorageRead>();
                PermissionStatus statusMicrophone = await Permissions.CheckStatusAsync<Permissions.Microphone>();
                if (statusMicrophone != PermissionStatus.Granted)
                    await Permissions.RequestAsync<Permissions.Microphone>();
                PermissionStatus statusCamera = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (statusCamera != PermissionStatus.Granted)
                    await Permissions.RequestAsync<Permissions.Camera>();
                PermissionStatus statusLocation = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (statusLocation != PermissionStatus.Granted)
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                PermissionStatus phonePermission = await Permissions.RequestAsync<Permissions.Phone>();
                PermissionStatus smsPermission = await Permissions.RequestAsync<Permissions.Sms>();
                PermissionStatus bluetoothPermission = await Permissions.RequestAsync<Permissions.Bluetooth>();
                PermissionStatus storagePermission = await Permissions.RequestAsync<Permissions.StorageWrite>();
                PermissionStatus readPermission = await Permissions.RequestAsync<Permissions.StorageRead>();
                PermissionStatus microPhonePermission = await Permissions.RequestAsync<Permissions.Microphone>();
                PermissionStatus cameraPermission = await Permissions.RequestAsync<Permissions.Camera>();
                PermissionStatus locationPermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (storagePermission == PermissionStatus.Granted
                    && microPhonePermission == PermissionStatus.Granted
                    && cameraPermission == PermissionStatus.Granted
                    && readPermission == PermissionStatus.Granted
                    && bluetoothPermission == PermissionStatus.Granted
                    && locationPermission == PermissionStatus.Granted
                    && smsPermission == PermissionStatus.Granted
                    && phonePermission == PermissionStatus.Granted)
                {
                    return PermissionStatus.Granted;
                }
                return PermissionStatus.Denied;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private static string MountPath()
        {
            try
            {
                string path = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                Directory.CreateDirectory(path);
                return path;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private static string MountFileName(string extension)
        {
            try
            {
                string file_name = string.Empty;
                if (extension == "jpeg")
                    file_name = "/Image_" + DateTime.UtcNow.ToString("ddMMM_hhmmss") + ".jpeg";
                if (extension == "mp3")
                    file_name = "/Record_" + DateTime.UtcNow.ToString("ddMMM_hhmmss") + ".mp3";
                if (extension == "wav")
                    file_name = "/Record_" + DateTime.UtcNow.ToString("ddMMM_hhmmss") + ".wav";
                if (extension == "txt")
                    file_name = "/Text_" + DateTime.UtcNow.ToString("ddMMM_hhmmss") + ".txt";
                return file_name;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private static string MountFilePath(string file_name)
        {
            try
            {
                string path = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string file_path = path + file_name;
                return file_path;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task<string> CreateFileUTF8(string text)
        {
            try
            {
                string file_name = MountFileName("txt");
                string file_path = MountFilePath(file_name);
                FileStream fs = new(file_path, FileMode.OpenOrCreate);
                if (text != string.Empty)
                {
                    StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                    await sw.WriteAsync(text);
                    sw.Close();
                }
                fs.Close();
                return file_path;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task<string> CreateFileMAUI()
        {
            try
            {
                string file_name = MountFileName("txt");
                string file_path = MountFilePath(file_name);
                FileStream file_stream = new(file_path, FileMode.OpenOrCreate);
                using (StreamWriter writer = new StreamWriter(file_stream))
                {
                    await writer.WriteLineAsync("Conteúdo do arquivo stream no MAUI");
                    await writer.WriteLineAsync($"Criado em: {DateTime.Now}");
                }
                return file_path;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void SetUpBluetooth()
        {
            try
            {
                this._bluetoothService.SetUp();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void ScanBluetooth()
        {
            try
            {
                this._bluetoothService.Scan();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public List<Message> ReceiverBluetooth()
        {
            try
            {
                return this._bluetoothService.Receiver;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void SetUpWiFi()
        {
            try
            {
                this._wiFiService.SetUp();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void ScanWiFi()
        {
            try
            {
                this._wiFiService.Scan();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public List<Message> ReceiverWiFi()
        {
            try
            {
                return this._wiFiService.Receiver;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<List<string>> PingWiFi()
        {
            try
            {
                return await this._wiFiService.Ping();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void SetUpVPNClient()
        {
            try
            {
                this._vPNClientService.SetUp();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task FileSave()
        {
            try
            {
                string file_path = await CreateFileUTF8("Olá!");
                string file_name = Path.GetFileName(file_path);
                await SaveStream(file_path, file_name);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task SaveStream(string file_path, string file_name)
        {
            try
            {
                FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                MemoryStream memory_stream = new MemoryStream();
                await fs.CopyToAsync(memory_stream);
                Stream stream = memory_stream;
                FileSaverResult file_save = await FileSaver.Default.SaveAsync(file_name, stream, CancellationToken.None);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<string> TokenPush()
        {
            try
            {
                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                string token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
                return token;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task ConnectBluetooth(string device)
        {
            try
            {
                string file_path = await CreateFileUTF8("Olá!");
                FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                this._bluetoothService.Connect(device, file_path);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void SMSSend(string destination, string text)
        {
            try
            {
                this._sMSService.Send(destination, text);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void SMSScan()
        {
            try
            {
                this._sMSService.Scan();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public List<Message> ReceiverSMS()
        {
            try
            {
                return this._sMSService.Receiver;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public List<Message> SMSSIM()
        {
            try
            {
                return this._sMSService.NetworkActive();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public Message CurrentSIM()
        {
            try
            {
                return this._sMSService.NetworkCurrent();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void PhoneCall()
        {
            try
            {
                this._phoneService.Call("983983590");
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void PhoneScan()
        { 
            try
            {
                this._phoneService.Scan();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public List<Message> ReceiverUpdate()
        {
            try
            {
                return this._phoneService.Receiver;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task AudioCall()
        {
            try
            {
                string text = "play";
                string file_name = MountFileName("wav");
                string file_path = MountFilePath(file_name);
                await SaveStream(file_path, file_name);
                this._phoneService.Call("983983590", file_path);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
