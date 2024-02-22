using Infrastructure.AppSettings;
using RabbitMQ.Client;
using System;
using System.Security.Authentication;

namespace MessageBroker.Consumer
{
    public class RdpzsdMbConsumer
    {
		public readonly IConnection connection;

		public RdpzsdMbConsumer()
		{
			var factory = new ConnectionFactory
			{
				HostName = AppSettingsProvider.MessageBroker.Host,
				Port = AppSettingsProvider.MessageBroker.Port,
				UserName = AppSettingsProvider.MessageBroker.Username,
				Password = AppSettingsProvider.MessageBroker.Password,
				AutomaticRecoveryEnabled = true,
				RequestedHeartbeat = TimeSpan.FromSeconds(AppSettingsProvider.MessageBroker.HeartbeatTimeout),
				NetworkRecoveryInterval = TimeSpan.FromSeconds(AppSettingsProvider.MessageBroker.NetworkRecoveryInterval),
				DispatchConsumersAsync = true,
				Ssl = new SslOption
                {
					Enabled = AppSettingsProvider.MessageBroker.SslEnabled,
					ServerName = AppSettingsProvider.MessageBroker.SslServerName,
					CertPath = AppSettingsProvider.MessageBroker.SslCertPath,
					CertPassphrase = AppSettingsProvider.MessageBroker.SslCertPassphrase,
					Version = SslProtocols.Tls12
                }
			};

			connection = factory.CreateConnection(AppSettingsProvider.MessageBroker.RdpzsdConsumer.Name);
		}
	}
}
