using StackExchange.Redis;

namespace NUnitSelfRunner.Listeners
{
    public class RedisQueueWriter : LinePrinter
    {
        private readonly ISubscriber subscriber;
        private readonly string queueName;

        public RedisQueueWriter(ISubscriber subscriber, string queueName)
        {
            this.subscriber = subscriber;
            this.queueName = queueName;
        }
        
        protected override void Print(string output)
        {
            subscriber.Publish(queueName, output);
        }
    }
}
