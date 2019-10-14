using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    class ReceivingProgram
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory(){HostName = "localhost"};
            using(var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("hello", false, false, false);
                
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += MessageReceived;

                channel.BasicConsume("hello", true, consumer: consumer);

                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }

        private static void MessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received {message}");
        }
    }
}
