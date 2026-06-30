namespace Gut.Models
{
    public class Message
    {
        public User? Sender { get; set; }
        public string? Text { get; set; }
        public string? Time { get; set; }
        public string? Speak { get; set; }
        public string? Move { get; set; }
        public string? Implied { get; set; }
    }
}
