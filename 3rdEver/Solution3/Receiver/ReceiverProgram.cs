using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receiver
{
    class ReceiverProgram
    {
        static void Main(string[] args)
        {
            var fabric = new ConnectionFactory() {HostName = "localhost"};
            using (var connection = fabric.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("logs", ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queueName, "logs", "");
                Console.WriteLine("Waiting for logs");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, eventArgs) =>
                {
                    var message = Encoding.UTF8.GetString(eventArgs.Body);
                    Console.WriteLine($"received '{message}' from queue '{queueName}'");
                };

                channel.BasicConsume(queueName, true, consumer);

                Console.WriteLine("to close the app press enter");
                Console.ReadLine();
            }
        }
    }
}
