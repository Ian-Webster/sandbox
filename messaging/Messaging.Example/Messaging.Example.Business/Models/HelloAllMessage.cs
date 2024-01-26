namespace Messaging.Example.Business.Models
{
    public class HelloAllMessage: MessageBase
    {
        /*public override Guid MessageId { get; set; }

        public override string Message { get; set; }

        public override DateTime Created { get; set; }*/

        public HelloAllMessage() : base("Hello All")
        {
        }
    }
}
