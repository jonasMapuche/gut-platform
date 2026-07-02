using Android.Content;
using Android.Telephony;
using Gut.Models;

namespace Gut.Platforms.Android.Broadcasts
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    //[IntentFilter(new[] { TelephonyManager.ActionPhoneStateChanged })]
    public class PhoneBroadcast : BroadcastReceiver
    {
        public List<Message> Receiver { get; set; }

        public PhoneBroadcast()
        {
            this.Receiver = new List<Message>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action == TelephonyManager.ActionPhoneStateChanged)
                {
                    string? state = intent.GetStringExtra(TelephonyManager.ExtraState);
                    if (state == TelephonyManager.ExtraStateRinging)
                    {
                        string? phoneNumber = intent.GetStringExtra(TelephonyManager.ExtraIncomingNumber);
                        Message memo = new Message();
                        memo.Text = phoneNumber;
                        memo.Implied = phoneNumber;
                        Receiver.Add(memo);
                        try
                        {
                            Call(context);
                        }
                        catch (Exception ex)
                        {
                            throw new NotImplementedException(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        private void Call(Context context)
        {
            try
            {
                TelephonyManager telephonyManager = (TelephonyManager)context.GetSystemService(Context.TelephonyService);

                Java.Lang.Class serviceClass = Java.Lang.Class.FromType(typeof(TelephonyManager));
                Java.Lang.Reflect.Method method = serviceClass.GetDeclaredMethod("getITelephony");
                method.Accessible = true;
                Java.Lang.Object? iTelephony = method.Invoke(telephonyManager);

                Java.Lang.Reflect.Method telephonyInterface = iTelephony.Class.GetDeclaredMethod("answerRingingCall");
                telephonyInterface.Accessible = true;
                telephonyInterface.Invoke(iTelephony);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
