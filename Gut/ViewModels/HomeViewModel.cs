using Android.Content.Res;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gut.Models;
using Gut.Services;
using Gut.Services.Interfaces;
using Gut.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gut.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<Message>? messages;

        [ObservableProperty]
        private byte[]? bytes;
        public ICommand ScanCommand { get; set; }
        public ICommand SetUpCommand { get; set; }
        public ICommand BotCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand ListCommand { get; set; }
        public ICommand SendCommand { get; set; }
        public ICommand SetUpWiFiCommand { get; set; }
        public ICommand ScanWiFiCommand { get; set; }
        public ICommand UpdateWiFiCommand { get; set; }
        public ICommand NetCommand { get; set; }
        public ICommand VPNCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand TokenCommand { get; set; }
        public ICommand SMSCommand { get; set; }
        public ICommand SIMCommand { get; set; }
        public ICommand ScanSMSCommand { get; set; }
        public ICommand UpdateSMSCommand { get; set; }
        public ICommand CurrentSIMCommand { get; set; }
        public ICommand CameraCommand { get; set; }


        IPerceptionService _perceptionService;

        private bool _isCopy;

        public bool IsCopy
        {
            get => _isCopy;
            set
            {
                if (_isCopy != value)
                {
                    _isCopy = value;
                }
            }
        }

        public HomeViewModel(PerceptionService perceptionService)
        {
            try
            {
                ScanCommand = new AsyncRelayCommand(OnScanCommand);
                SetUpCommand = new AsyncRelayCommand(OnSetUpCommand);
                BotCommand = new AsyncRelayCommand<object>(OnBotCommand);
                UpdateCommand = new AsyncRelayCommand(OnUpdateCommand);
                ListCommand = new AsyncRelayCommand(OnListCommand);
                SendCommand = new AsyncRelayCommand(OnSendCommand);
                SetUpWiFiCommand = new AsyncRelayCommand(OnSetUpWiFiCommand);
                ScanWiFiCommand = new AsyncRelayCommand(OnScanWiFiCommand);
                UpdateWiFiCommand = new AsyncRelayCommand(OnUpdateWiFiCommand);
                NetCommand = new AsyncRelayCommand(OnNetCommand);
                VPNCommand = new AsyncRelayCommand(OnVPNCommand);
                ExitCommand = new AsyncRelayCommand(OnExitCommand);
                TokenCommand = new AsyncRelayCommand(OnTokenCommand);
                SMSCommand = new AsyncRelayCommand(OnSMSCommand);
                SIMCommand = new AsyncRelayCommand(OnSIMCommand);
                ScanSMSCommand = new AsyncRelayCommand(OnScanSMSCommand);
                UpdateSMSCommand = new AsyncRelayCommand(OnUpdateSMSCommand);
                CurrentSIMCommand = new AsyncRelayCommand(OnCurrentSIMCommand);
                CameraCommand = new AsyncRelayCommand(OnCameraCommand);

                Messages = new ObservableCollection<Message>();
                this._perceptionService = perceptionService;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnCameraCommand()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnCurrentSIMCommand()
        {
            try
            {
                Message memo = this._perceptionService.CurrentSIM();
                MessageSMS(memo);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnUpdateSMSCommand()
        {
            try
            {
                List<Message> memos = this._perceptionService.ReceiverSMS();
                UpdateSMS(memos);
                this._perceptionService.SMSScan();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnScanSMSCommand()
        {
            try
            {
                this._perceptionService.SMSScan();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnSIMCommand()
        {
            try
            {
                List<Message> memos = this._perceptionService.SMSSIM();
                UpdateSMS(memos);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnSMSCommand()
        {
            try
            {
                this._perceptionService.SMSSend("31983983590", "Olá!");
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnTokenCommand()
        {
            try
            {
                string token = await this._perceptionService.TokenPush();
                Text(token);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnExitCommand()
        {
            try
            {
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnVPNCommand()
        {
            try
            {
                this._perceptionService.SetUpVPNClient();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnNetCommand()
        {
            try
            {
                List<string> ping = await this._perceptionService.PingWiFi();
                UpdateNetwork(ping);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnUpdateWiFiCommand()
        {
            try
            {
                UpdateWiFi();
                this._perceptionService.ScanWiFi();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnScanWiFiCommand()
        {
            try
            {
                this._perceptionService.ScanWiFi();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnSetUpWiFiCommand()
        {
            try
            {
                this._perceptionService.SetUpWiFi();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnSendCommand()
        {
            try
            {
                await this._perceptionService.FileSave();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnListCommand()
        {
            try
            {
                List();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnUpdateCommand()
        {
            try
            {
                UpdateBluetooth();
                this._perceptionService.ScanBluetooth();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnBotCommand(object? arg)
        {
            try
            {
                string variable = arg as string;
                if (arg == null) 
                    return;
                if (!this._isCopy)
                {
                    this._perceptionService.ConnectBluetooth(variable);
                }
                else 
                {
                    await Clipboard.Default.SetTextAsync(variable);
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnSetUpCommand()
        {
            try
            {
                this._perceptionService.SetUpBluetooth();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task OnScanCommand()
        {
            try
            {
                this._perceptionService.ScanBluetooth();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private void List()
        {
            try
            {
                Message text = new Message();
                text.Sender = null;
                text.Text = "Olá";
                Messages.Add(text);
                Messages.Add(text);
                User user = new()
                {
                    Name = "Deutsch",
                    Image = "",
                    Color = Color.FromArgb("#FFE0EC")
                };
                text = new Message();
                text.Sender = user;
                text.Text = "Oi";
                Messages.Add(text);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private void Text(string token)
        {
            try
            {
                Message text = new Message();
                User user = new()
                {
                    Name = "Deutsch",
                    Image = "",
                    Color = Color.FromArgb("#FFE0EC")
                };
                text.Sender = user;
                text.Text = token;
                text.Implied = token;
                Messages.Add(text);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void UpdateBluetooth()
        {
            try
            {
                List<Message> listaDoServico = this._perceptionService.ReceiverBluetooth();
                //Messages.Clear();
                foreach (Message item in listaDoServico)
                {
                    Message text = new Message();
                    User user = new()
                    {
                        Name = "Deutsch",
                        Image = "",
                        Color = Color.FromArgb("#FFE0EC")
                    };
                    text.Sender = user;
                    text.Text = item.Text;
                    text.Implied = item.Implied;
                    Messages.Add(text);
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void UpdateWiFi()
        {
            try
            {
                List<Message> listaDoServico = this._perceptionService.ReceiverWiFi();
                //Messages.Clear();
                foreach (Message item in listaDoServico)
                {
                    Message text = new Message();
                    User user = new()
                    {
                        Name = "Deutsch",
                        Image = "",
                        Color = Color.FromArgb("#FFE0EC")
                    };
                    text.Sender = user;
                    text.Text = item.Text;
                    text.Implied = item.Implied;
                    Messages.Add(text);
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void UpdateNetwork(List<string> lista_servico)
        {
            try
            {
                List<string> listaDoServico = lista_servico;
                Messages.Clear();
                foreach (string item in listaDoServico)
                {
                    Message text = new Message();
                    User user = new()
                    {
                        Name = "Deutsch",
                        Image = "",
                        Color = Color.FromArgb("#FFE0EC")
                    };
                    text.Sender = user;
                    text.Text = item;
                    text.Implied = item;
                    Messages.Add(text);
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void UpdateSMS(List<Message> memos)
        {
            try
            {
                foreach (Message item in memos)
                {
                    Message text = new Message();
                    User user = new()
                    {
                        Name = "Deutsch",
                        Image = "",
                        Color = Color.FromArgb("#FFE0EC")
                    };
                    text.Sender = user;
                    text.Text = item.Text;
                    text.Implied = item.Implied;
                    Messages.Add(text);
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void MessageSMS(Message memo)
        {
            try
            {
                Message text = new Message();
                User user = new()
                {
                    Name = "Deutsch",
                    Image = "",
                    Color = Color.FromArgb("#FFE0EC")
                };
                text.Sender = user;
                text.Text = memo.Text;
                text.Implied = memo.Implied;
                Messages.Add(text);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
