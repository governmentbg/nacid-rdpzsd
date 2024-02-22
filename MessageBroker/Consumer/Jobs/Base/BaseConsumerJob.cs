using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBroker.Consumer.Jobs.Base
{
    public abstract class BaseConsumerJob : BackgroundService
    {
		private IModel channel;
		private string queueName;
		private readonly RdpzsdMbConsumer rdpzsdMbConsumer;

		public BaseConsumerJob(
			string exchangeName,
			RdpzsdMbConsumer rdpzsdMbConsumer
		)
		{
			this.rdpzsdMbConsumer = rdpzsdMbConsumer;
			InitializeChannel(exchangeName);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new AsyncEventingBasicConsumer(channel);

			consumer.Received += async (model, bodyStream) => {
				var body = bodyStream.Body.ToArray();
				await HandleBody(body);
			};

			channel.BasicConsume(queue: queueName,
								autoAck: false,
								consumer: consumer);

			return Task.CompletedTask;
		}

		protected virtual async Task HandleBody(byte[] body)
		{
			await Task.CompletedTask;
		}

		private void InitializeChannel(string exchangeName)
		{
			channel = rdpzsdMbConsumer.connection.CreateModel();
			channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
			queueName = channel.QueueDeclare().QueueName;
			channel.QueueBind(queue: queueName,
							 exchange: exchangeName,
							 routingKey: "");
		}
	}
}
