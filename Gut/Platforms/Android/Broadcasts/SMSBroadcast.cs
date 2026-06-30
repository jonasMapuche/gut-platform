using Android.Content;
using Android.Provider;
using Gut.Models;
using SmsMessage = Android.Telephony.SmsMessage;

namespace Gut.Platforms.Android.Broadcasts
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
//    [IntentFilter(new[] { Telephony.Sms.Intents.SmsReceivedAction })]
    public class SMSBroadcast : BroadcastReceiver
    {
        public List<Message> Receiver { get; set; }

        public SMSBroadcast()
        {
            this.Receiver = new List<Message>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action != Telephony.Sms.Intents.SmsReceivedAction)
                    return;
                SmsMessage[]? messages = Telephony.Sms.Intents.GetMessagesFromIntent(intent);
                foreach (SmsMessage message in messages)
                {
                    string sender = message.OriginatingAddress;
                    string body = message.MessageBody;
                    Message memo = new Message();
                    memo.Text = $"SENDER: {sender} | BODY: {body}";
                    memo.Implied = $"{sender}; {body}";
                    this.Receiver.Add(memo);
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
