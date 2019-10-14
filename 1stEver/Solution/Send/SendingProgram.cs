using System;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
    class SendingProgram
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory(){HostName = "localhost"};
            using(var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("hello", false, false, false);
                
                var message = "Hello world";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", "hello", body: body);
                Console.WriteLine($"Sent: {message}");
            }

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
