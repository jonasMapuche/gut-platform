using Gut.Models;

namespace Gut.Interfaces
{
    public interface ISMSService
    {
        List<Message> Receiver { get; set; }
        void Send(string destino, string text);
        List<Message> NetworkActive();
        Message NetworkCurrent();
        void Scan();
    }
}
