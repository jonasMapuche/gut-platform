using Gut.Models;

namespace Gut.Interfaces
{
    public interface IBluetoothService
    {
        void SetUp();
        void Scan();
        List<Message> Receiver { get; set; }
        void Connect(string address, string file_path);
    }
}
