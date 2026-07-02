using Android.App;
using Android.Telecom;
using Gut.Models;

namespace Gut.Platforms.Android.Services
{
    [Service(Permission = "android.permission.BIND_TELECOM_CONNECTION_SERVICE")]
    public class PhoneConnectionService : ConnectionService
    {
        public List<Message> Receiver { get; set; }

        public PhoneConnectionService()
        {
            this.Receiver = new List<Message>();
        }

        public override Connection OnCreateOutgoingConnection(PhoneAccountHandle connectionManagerPhone, ConnectionRequest request)
        {
            var connection = new PhoneConnection();
            connection.SetDialing();
            connection.ChangeRoute(CallAudioRoute.WiredHeadset);
            this.Receiver = connection.Receiver;
            //CallEndpoint availableEndpoints = connection.CurrentCallEndpoint;
            return connection;
        }

        public override Connection OnCreateIncomingConnection(PhoneAccountHandle connectionManagerPhoneAccount, ConnectionRequest request)
        {
            var connection = new PhoneConnection();
            connection.SetRinging();
            // Lógica para notificar chamada recebida
            return connection;
        }
    }

    public class PhoneConnection : Connection
    {
        public List<Message> Receiver { get; set; }

        public PhoneConnection()
        {
            this.Receiver = new List<Message>();
        }

        public override void OnAvailableCallEndpointsChanged(IList<CallEndpoint> availableEndpoints)
        {
            base.OnAvailableCallEndpointsChanged(availableEndpoints);

            foreach (CallEndpoint endpoint in availableEndpoints)
            {
                Message memo = new Message();
                memo.Text = endpoint.EndpointName;
                int kind = (int)endpoint.EndpointType;
                memo.Implied = kind.ToString();
                this.Receiver.Add(memo);
            }
        }

        public void ChangeRoute(CallAudioRoute route)
        {
            SetAudioRoute(route);
        }

        public override void OnShowIncomingCallUi()
        {
            base.OnShowIncomingCallUi();
        }
    }
}
