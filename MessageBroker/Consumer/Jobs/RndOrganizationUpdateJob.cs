using Infrastructure.AppSettings;
using Logs.Enums;
using Logs.Services;
using MessageBroker.Consumer.Jobs.Base;
using MessageBroker.Consumer.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Rdpzsd.Models.Models.Nomenclatures;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Consumer.Jobs
{
    public class RndOrganizationUpdateJob : BaseConsumerJob
    {
        private readonly IServiceProvider serviceProvider;

		public RndOrganizationUpdateJob(
			IServiceProvider serviceProvider,
			RdpzsdMbConsumer rdpzsdMbConsumer
		) : base(AppSettingsProvider.MessageBroker.RdpzsdConsumer.RndOrganizationUpdateExchange, rdpzsdMbConsumer)
		{
			this.serviceProvider = serviceProvider;
		}

        protected override async Task HandleBody(byte[] body)
        {
            using var scope = serviceProvider.CreateScope();
            var rndOrganizationUpdateService = scope.ServiceProvider
                .GetRequiredService<RndOrganizationUpdateService>();
            var logService = scope.ServiceProvider
                .GetRequiredService<LogService>();

            var organizationForUpdate = JsonConvert.DeserializeObject<Institution>(Encoding.UTF8.GetString(body));

            try
            {
                await rndOrganizationUpdateService.UpdateOrganization(organizationForUpdate);
            }
            catch (Exception exception)
            {
                while (exception.InnerException != null)
                { exception = exception.InnerException; }

                var customText = $"Организация с Id: {organizationForUpdate.Id} и Наименование: {organizationForUpdate.Name} не е въведена/редактирана през message broker-а";

                await logService.LogServerException(exception, null, LogType.MessageBrokerExceptionLog, null, customText);
            }
        }
    }
}
