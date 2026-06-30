using Android.Bluetooth;
using Android.Content;
using Gut.Models;

namespace Gut.Platforms.Android.Broadcasts
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class BluetoothBroadcast : BroadcastReceiver
    {
        public List<Message> Receiver { get; set; }

        public BluetoothBroadcast()
        {
            this.Receiver = new List<Message>();
        }

        public override void OnReceive(Context? context, Intent? intent)
        {
            try
            {
                string action = intent.Action;
                if (BluetoothDevice.ActionFound.Equals(action))
                {
                    BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                    string appliance = $"{device.Name} - {device.Address}";
                    Message memo = new Message();
                    memo.Text = appliance;
                    memo.Implied = device.Address;
                    Receiver.Add(memo);
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
