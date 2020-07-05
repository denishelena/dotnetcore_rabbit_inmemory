using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;


namespace projetoAlanC
{
    public class Usuario{
        public int Id { get; set; }
        public String Nome { get; set; }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://bwcicctz:X3wu3LDr3rZV0SeHYhsvVrCv04Mb4jeO@chimpanzee.rmq.cloudamqp.com/bwcicctz".Replace("amqp://", "amqps://"));
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var usuario = JsonConvert.DeserializeObject<Usuario>(message);
                    Console.WriteLine(" [x] Received {0}", usuario.Nome);
                };
                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
        }
    }
}
}