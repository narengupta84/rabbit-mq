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

byte[] messageBodyBytes = Encoding.UTF8.GetBytes("Hello World!");
channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

channel.Close();
cnn.Close();
