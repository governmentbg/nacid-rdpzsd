namespace Infrastructure.AppSettings.MessageBroker
{
    public class MessageBrokerSettings
    {
        public bool Enable { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public RdpzsdConsumer RdpzsdConsumer { get; set; }

        public int HeartbeatTimeout { get; set; }
        public int NetworkRecoveryInterval { get; set; }

        public bool SslEnabled { get; set; }
        public string SslServerName { get; set; }
        public string SslCertPath { get; set; }
        public string SslCertPassphrase { get; set; }
    }
}
