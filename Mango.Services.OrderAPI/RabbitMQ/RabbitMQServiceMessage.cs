using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.OrderAPI.RabbitMQ
{
    public class RabbitMQServiceMessage : IRabbitMqServiceProducer
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;

        public RabbitMQServiceMessage()
        {
            _hostname = "localhost";
            _password = "guest";
            _username = "guest";
        }

        public async Task PublishMessageAsync(BaseMessageRabbitMq message, string queueName)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.QueueDeclare(queueName, false, false, false, null);
                var jsonMessage = JsonConvert.SerializeObject(message);
                var messageByte = Encoding.UTF8.GetBytes(jsonMessage);
                channel.BasicPublish(string.Empty, queueName, null, messageByte);
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password,
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                //log exception
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }
            CreateConnection();

            return _connection != null;
        }
    }
}
