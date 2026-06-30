using Android.Content;
using Android.Net.Wifi;
using Android.Provider;
using Android.Telephony;
using Gut.Interfaces;
using Gut.Models;
using Gut.Platforms.Android.Broadcasts;
using static Android.Provider.CalendarContract;

[assembly: Dependency(typeof(Gut.Platforms.Android.Services.SMSService))]
namespace Gut.Platforms.Android.Services
{
    public class SMSService : ISMSService
    {
        public List<Message> Receiver { get; set; }

        //private SMSService _sMSService;

        public SMSService()
        {
            this.Receiver = new List<Message>();
        }

        public void Send(string destination, string text)
        {
            try
            {
                SmsManager sms = SmsManager.Default;
                IList<string>? parts = sms.DivideMessage(text);
                foreach (string part in parts)
                {
                    sms.SendTextMessage(destination, null, part, null, null);
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
                SMSBroadcast receiver = new SMSBroadcast();
                this.Receiver = receiver.Receiver;
                IntentFilter intentFilter = new IntentFilter(Telephony.Sms.Intents.SmsReceivedAction);
                Platform.AppContext.RegisterReceiver(receiver, intentFilter, ReceiverFlags.Exported);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public List<Message> NetworkActive()
        {
            try
            {
                List<Message> networks = new List<Message>();
                SubscriptionManager? subscription = (SubscriptionManager)Platform.AppContext.GetSystemService(Context.TelephonySubscriptionService);
                if (subscription != null)
                {
                    IList<SubscriptionInfo>? infos = subscription.ActiveSubscriptionInfoList;
                    if (infos != null)
                    {
                        foreach (SubscriptionInfo info in infos)
                        {
                            int id = info.SubscriptionId;
                            string name = info.CarrierName;
                            Message memo = new Message();
                            memo.Text = $"CARRIER {name} | SUBSCRIPTION: {id}";
                            memo.Implied = id.ToString();
                            networks.Add(memo);
                        }
                    }
                }
                return networks;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public Message NetworkCurrent()
        {
            try
            {
                TelephonyManager? telephony = (TelephonyManager)Platform.CurrentActivity.GetSystemService(Context.TelephonyService);
                int id = telephony.SubscriptionId;
                string name = telephony.SimCarrierIdName;
                Message memo = new Message();
                memo.Text = $"CARRIER {name} | SUBSCRIPTION: {id}";
                memo.Implied = id.ToString();
                return memo;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
