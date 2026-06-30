using Gut.Models;

namespace Gut.Interfaces
{
    public interface IWiFiService
    {
        List<Message> Receiver { get; set; }
        void SetUp();
        void Scan();
        Task<List<string>> Ping();
    }
}
