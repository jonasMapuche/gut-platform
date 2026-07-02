using Android.Telecom;
using Gut.Models;

namespace Gut.Interfaces
{
    public interface IPhoneService
    {
        void Call(string number);
        void Call(string numero, string caminhoAudio);
        void Scan();
        List<Message> Receiver { get; set; }
    }
}
