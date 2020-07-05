using System;
using RabbitMQ.Client;
using System.Text;
using projetoAlan.Models;
using Newtonsoft.Json;

namespace projetoAlan.Rabbitmq
{
    public class Sender
    {
        public void Send(Usuario usuario){
                var factory = new ConnectionFactory();
                factory.Uri = new Uri("amqp://bwcicctz:X3wu3LDr3rZV0SeHYhsvVrCv04Mb4jeO@chimpanzee.rmq.cloudamqp.com/bwcicctz".Replace("amqp://", "amqps://"));
                
                using(var connection = factory.CreateConnection())
                using(var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(usuario));

                    channel.BasicPublish(exchange: "",
                                        routingKey: "hello",
                                        basicProperties: null,
                                        body: body);

                }
        }
    }
}