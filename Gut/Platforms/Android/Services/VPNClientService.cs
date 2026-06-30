using Android.Content;
using Android.Net;
using Gut.Interfaces;
using Gut.Platforms.Android.Broadcasts;

namespace Gut.Platforms.Android.Services
{
    public class VPNClientService : IVPNClientService
    {
        public void SetUp()
        {
            Intent intent = VpnService.Prepare(Platform.CurrentActivity);

            if (intent != null)
            {
                Platform.CurrentActivity.StartActivityForResult(intent, MainActivity.RequestVpnPermission);
            }
            else
            {
                Intent vpnIntent = new Intent(Platform.AppContext, typeof(VPNClientBroacast));
                Platform.AppContext.StartService(vpnIntent);
            }
        }
    }
}
