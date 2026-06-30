using Android.Content;
using Android.Net.Wifi;
using Gut.Models;

namespace Gut.Platforms.Android.Broadcasts
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class WiFiBroadcast : BroadcastReceiver
    {
        public List<Message> Receiver { get; set; }

        public WiFiBroadcast()
        {
            this.Receiver = new List<Message>();
        }

        public override void OnReceive(Context? context, Intent? intent)
        {
            try
            {
                WifiManager wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
                IList<ScanResult>? results = wifiManager.ScanResults;
                if (results != null)
                {
                    foreach (ScanResult result in results)
                    {
                        string appliance = $"SSID: {result.Ssid} | BSSID: {result.Bssid} | RSSI: {result.Level} dBm";
                        Message memo = new Message();
                        memo.Text = appliance;
                        memo.Implied = result.Bssid;
                        Receiver.Add(memo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
