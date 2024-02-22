using Infrastructure.AppSettings.Jobs;
using Infrastructure.AppSettings.MessageBroker;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Infrastructure.AppSettings
{
	public static class AppSettingsProvider
	{
		public static string MainDbConnectionString { get; private set; }
		public static string LogDbConnectionString { get; private set; }
		public static bool ShouldUsePostgreSql { get; private set; }
		public static List<FileStorageSettings> FileStorages { get; private set; } = new List<FileStorageSettings>();
        public static ValidationTxtJobSettings ValidationTxtJob { get; set; }
		public static RegistrationTxtJobSettings RegistrationTxtJob { get; set; }
        public static bool EnableFullFunctionality { get; set; }
        public static bool EnableRsoIntegration { get; set; }
        public static ProxyPathsSettings ProxyPaths { get; private set; }
        public static RsoIntegrationSettings RsoIntegration { get; set; }
        public static MessageBrokerSettings MessageBroker { get; set; }

        public static void AddAppSettings(IConfiguration configuration)
		{
			if (configuration.GetSection("mainDbConnectionString").Exists())
			{
				MainDbConnectionString = configuration.GetSection("mainDbConnectionString").Get<string>();
			}

			if (configuration.GetSection("logDbConnectionString").Exists())
			{
				LogDbConnectionString = configuration.GetSection("logDbConnectionString").Get<string>();
			}

			if (configuration.GetSection("shouldUsePostgreSql").Exists())
			{
				ShouldUsePostgreSql= configuration.GetSection("shouldUsePostgreSql").Get<bool>();
			}

			if (configuration.GetSection("fileStorages").Exists())
			{
				FileStorages = configuration.GetSection("fileStorages").Get<List<FileStorageSettings>>();
			}

			if (configuration.GetSection("validationTxtJob").Exists())
			{
				ValidationTxtJob = configuration.GetSection("validationTxtJob").Get<ValidationTxtJobSettings>();
			}

			if (configuration.GetSection("registrationTxtJob").Exists())
			{
				RegistrationTxtJob = configuration.GetSection("registrationTxtJob").Get<RegistrationTxtJobSettings>();
			}

			if (configuration.GetSection("enableFullFunctionality").Exists())
			{
				EnableFullFunctionality = configuration.GetSection("enableFullFunctionality").Get<bool>();
			}

			if (configuration.GetSection("enableRsoIntegration").Exists())
			{
				EnableRsoIntegration = configuration.GetSection("enableRsoIntegration").Get<bool>();
			}

			if (configuration.GetSection("proxyPaths").Exists())
			{
				ProxyPaths = configuration.GetSection("proxyPaths").Get<ProxyPathsSettings>();
			}

			if (configuration.GetSection("rsoIntegration").Exists())
			{
				RsoIntegration = configuration.GetSection("rsoIntegration").Get<RsoIntegrationSettings>();
			}

			if (configuration.GetSection("messageBroker").Exists())
			{
				MessageBroker = configuration.GetSection("messageBroker").Get<MessageBrokerSettings>();
			}
		}
	}
}
