using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer.Model
{
    /// <summary>
    /// Connects to the queue manager, reads a encrypted json string, decrypt it and creates a new Record from it.
    /// The received records are been saved in a static List.
    /// </summary>
    public class RecordConsumerService
    {
        private static readonly ConnectionFactory factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        //key is saved here for simplicity - in real app we should use protected storage provider.
        private const string key = "5EJOeNGy00Mx6STCUQFBFQ==";


        public static readonly List<Record> Records;

        static RecordConsumerService()
        {
            Records = new List<Record>();
            new Thread(CheckMessages).Start();
        }

        public static void CheckMessages()
        {
            using (
                var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    while (true)
                    {
                        //set queue and message durable
                        channel.QueueDeclare(queue: "Records",
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        channel.CreateBasicProperties().Persistent = true;

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();

                            var recordString = Encryptor.DecryptString(key, Convert.ToBase64String(body));

                            var record = JsonSerializer.Deserialize<Record>(recordString);

                            Records.Add(record);

                            Console.WriteLine("Received {0}, {1}, {2}, {3}", record.name, record.age, record.profession,
                                record.date);

                            if (connection.IsOpen && channel.IsOpen)
                                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        };

                        channel.BasicConsume(queue: "Records",
                            consumer: consumer,
                            autoAck: false);
                    }
                }
            }
        }
    }
}