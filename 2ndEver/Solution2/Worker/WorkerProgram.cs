using System;
using System.Linq;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Worker
{
    class WorkerProgram
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("task_queue", true, false, false);

                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, eventArgs) =>
                {
                    var message = Encoding.UTF8.GetString(eventArgs.Body);
                    Console.WriteLine($"Received {message}");

                    var delay = message.Count(ch => ch == '.');
                    Thread.Sleep(TimeSpan.FromSeconds(delay));

                    channel.BasicAck(eventArgs.DeliveryTag, false);

                    Console.WriteLine("Done");
                };

                channel.BasicConsume("task_queue", false, consumer);

                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }

        }
    }
}
