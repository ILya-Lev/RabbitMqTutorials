using System;
using System.Text;
using RabbitMQ.Client;

namespace EmitLog
{
    class EmitLogProgramm
    {
        static void Main(string[] args)
        {
            const string exchangeName = "logs";

            var fabric = new ConnectionFactory(){HostName = "localhost"};
            using (var connection = fabric.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);

                channel.BasicPublish(exchangeName, "", false, null, body);

                Console.WriteLine($"sending '{message}' using exchange '{exchangeName}'");
            }

            Console.WriteLine("To exit press enter");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return args?.Length > 0 ? string.Join(" ", args) : "info: Hello World!";
        }
    }
}
