using Gut.Models;

namespace Gut.Services.Interfaces
{
    public interface IPerceptionService
    {
        void SetUpBluetooth();
        void ScanBluetooth();
        List<Message> ReceiverBluetooth();
        void SetUpWiFi();
        void ScanWiFi();
        List<Message> ReceiverWiFi();
        Task<List<string>> PingWiFi();
        void SetUpVPNClient();
        Task FileSave();
        Task<string> TokenPush();
        Task ConnectBluetooth(string device);
        void SMSSend(string destination, string text);
        void SMSScan();
        List<Message> ReceiverSMS();
        List<Message> SMSSIM();
        Message CurrentSIM();
        void PhoneCall();
        void PhoneScan();
        List<Message> ReceiverUpdate();
        Task AudioCall();
    }
}
