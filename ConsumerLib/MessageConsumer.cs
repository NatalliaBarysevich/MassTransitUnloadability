using MassTransit;
using System;
using System.Threading.Tasks;

namespace ConsumerLib
{
    //Extern library, with will be loaded to domain from collectable assembly load context.
    //Other project in solution don't refer to it directly.

    public class Message
    {
        public string Text { get; set; }
    }

    public class MessageConsumer : IConsumer<Message>
    {
        public Task Consume(ConsumeContext<Message> context)
        {
            throw new NotImplementedException();
        }
    }
}
