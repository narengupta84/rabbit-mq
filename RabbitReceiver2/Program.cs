using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://admin:AT63Ukv4@rabbitmq-125957-0.cloudclusters.net:10064");
factory.ClientProvidedName = "Rabbit Receiver2 App";

IConnection cnn = factory.CreateConnection();

IModel channel = cnn.CreateModel();


string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);
channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    Task.Delay(TimeSpan.FromSeconds(3)).Wait();//Added this line to test other receiver too
    var body = args.Body.ToArray();

    string messgae = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Message received from Rabbit MQ :{messgae}");

    //Send message to DB

    channel.BasicAck(args.DeliveryTag, false);
};

string consumerTag = channel.BasicConsume(queueName, false, consumer);
Console.ReadLine();
channel.BasicCancel(consumerTag);

channel.Close();
cnn.Close();