using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://admin:AT63Ukv4@rabbitmq-125957-0.cloudclusters.net:10064");
factory.ClientProvidedName = "Rabbit Sender App";

IConnection cnn = factory.CreateConnection();

IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

for (int i = 0; i <= 60; i++)
{
    Console.WriteLine($"Sending Message {i}");
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"Message #{i} sent to Rabbit MQ");
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
    Thread.Sleep(1000);
}

channel.Close();
cnn.Close();
