namespace MufaApp.Models
{
    public class Email
    {
        public int EmailId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
