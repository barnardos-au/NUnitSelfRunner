using System;
using StackExchange.Redis;

namespace RedisChannelSubscriber
{
    class Program
    {
        private static readonly ConnectionMultiplexer Redis = ConnectionMultiplexer.Connect("localhost:6379");
        private const string QueueName = "test-logs";
        
        static void Main(string[] args)
        {
            Console.WriteLine($"Subscribed to Redis channel:{QueueName}. Press any key to end...");
            var sub = Redis.GetSubscriber().Subscribe(QueueName);
            sub.OnMessage(msg =>
            {
                Console.WriteLine(msg.Message);
            });
            Console.ReadKey();        
        }
    }
}