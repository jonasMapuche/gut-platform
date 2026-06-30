using Android.Content;
using Android.Net.Wifi;
using Android.Provider;
using Gut.Interfaces;
using Gut.Models;
using Gut.Platforms.Android.Broadcasts;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using ProtocolType = System.Net.Sockets.ProtocolType;

namespace Gut.Platforms.Android.Services
{
    public class WiFiService : IWiFiService
    {
        public List<Message> Receiver { get; set; }

        private WifiManager _wifiManager;

        public WiFiService()
        {
            this.Receiver = new List<Message>();
        }

        public void SetUp()
        {
            try
            {
                WifiManager wifiManager = (WifiManager)Platform.AppContext.GetSystemService(Context.WifiService);
                this._wifiManager = wifiManager;

                if (this._wifiManager != null && !this._wifiManager.IsWifiEnabled)
                {
                    Intent intent = new Intent(Settings.ActionWifiSettings);
                    intent.SetFlags(ActivityFlags.NewTask);
                    Platform.AppContext.StartActivity(intent);
                }
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
                WiFiBroadcast receiver = new WiFiBroadcast();
                this.Receiver = receiver.Receiver;
                IntentFilter filter = new IntentFilter(WifiManager.ScanResultsAvailableAction);
                Platform.AppContext.RegisterReceiver(receiver, filter);
                this._wifiManager.StartScan();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<List<string>> Ping()
        {
            try
            {
                List<string> server_active = new List<string>();
                string subnet_base = "192.168.0.";
                List<Task> tarefa_ping = new List<Task>();
                for (int i = 1; i < 255; i++)
                {
                    string ip_address = subnet_base + i;
                    tarefa_ping.Add(PingCheck(ip_address, server_active));
                }
                await Task.WhenAll(tarefa_ping);
                return server_active;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private async Task PingCheck(string ip, List<string> active)
        {
            try
            {
                using (Ping ping_sender = new Ping())
                {
                    PingReply reply = await ping_sender.SendPingAsync(ip, 20000);
                    if (reply != null && reply.Status == IPStatus.Success)
                    {
                        lock (active)
                        {
                            active.Add(ip);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<bool> IsPortOpenAsync(string host, int port, int timeoutMilliseconds = 2000)
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    Task result = socket.ConnectAsync(host, port);
                    if (await Task.WhenAny(result, Task.Delay(timeoutMilliseconds)) == result)
                    {
                        return socket.Connected;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
