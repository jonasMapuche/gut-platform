using Android.App;
using Android.Content;
using Android.Net;

namespace Gut.Platforms.Android.Broadcasts
{
    [Service(Label = "GUT", Permission = "android.permission.BIND_VPN_SERVICE", Exported = true)]
    [IntentFilter(new[] { ActionVpnService })]
    public class VPNClientBroacast : VpnService
    {
        public const string ActionVpnService = "android.net.VpnService";

        public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
        {
            Builder builder = new Builder(this);
            return StartCommandResult.Sticky;
        }
    }
}
