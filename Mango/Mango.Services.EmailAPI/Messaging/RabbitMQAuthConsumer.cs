
using Mango.Services.EmailAPI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class RabbitMQAuthConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private IConnection _connection;
        private IModel _channel;
        public RabbitMQAuthConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
            InitializeRabbitMQConnectionFactory();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, eventArgument) =>
            {
                string content = Encoding.UTF8.GetString(eventArgument.Body.ToArray());
                string email = JsonConvert.DeserializeObject<string>(content);
                HandlerMessage(email).GetAwaiter().GetResult();

                _channel.BasicAck(eventArgument.DeliveryTag, false);
            };

            _channel.BasicConsume(_configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"), false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandlerMessage(string email)
        {
            await _emailService.RegisterUserEmailAndLog(email);
        }

        private void InitializeRabbitMQConnectionFactory()
        {
            string queueName = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue");
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queueName, false, false, false, null);
        }
    }
}
