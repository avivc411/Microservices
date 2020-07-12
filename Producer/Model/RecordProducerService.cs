using System;
using System.Configuration;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Producer.Model
{
    /// <summary>
    /// Connects to the queue manager and send a Record as encrypted json string.
    /// </summary>
    public class RecordProducerService
    {
        private static readonly ConnectionFactory factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        //key is saved here for simplicity - in real app we should use protected storage provider.
        private const string key = "5EJOeNGy00Mx6STCUQFBFQ==";

        public static void SendRecord(Record record)
        {
            if (record == null)
                return;

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //set queue and message durable
            channel.QueueDeclare(queue: "Records",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.CreateBasicProperties().Persistent = true;

            var recordAsJson = JsonSerializer.Serialize(record);

            var body = Convert.FromBase64String(Encryptor.EncryptString(key, recordAsJson));

            channel.BasicPublish(exchange: "",
                routingKey: "Records",
                basicProperties: null,
                body: body);

            Console.WriteLine("Sent {0}, {1}, {2}, {3}", record.name, record.age, record.profession, record.date);
        }
    }
}