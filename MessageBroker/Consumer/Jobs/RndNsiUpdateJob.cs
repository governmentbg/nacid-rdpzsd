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
    public class RndNsiUpdateJob : BaseConsumerJob
    {
        private readonly IServiceProvider serviceProvider;

        public RndNsiUpdateJob(
            IServiceProvider serviceProvider,
            RdpzsdMbConsumer rdpzsdMbConsumer
        ) : base(AppSettingsProvider.MessageBroker.RdpzsdConsumer.RndNsiUpdateExchange, rdpzsdMbConsumer)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override async Task HandleBody(byte[] body)
        {
            using var scope = serviceProvider.CreateScope();
            var rndNsiUpdateService = scope.ServiceProvider
                .GetRequiredService<RndNsiUpdateService>();
            var logService = scope.ServiceProvider
                .GetRequiredService<LogService>();
            var nsiForUpdate = JsonConvert.DeserializeObject<NationalStatisticalInstitute>(Encoding.UTF8.GetString(body));

            try
            {
                await rndNsiUpdateService.UpdateNsi(nsiForUpdate);
            }
            catch (Exception exception)
            {
                while (exception.InnerException != null)
                { exception = exception.InnerException; }

                var customText = $"НСИ с Id: {nsiForUpdate.Id} и Код: {nsiForUpdate.Code} не е въведена/редактирана през message broker-а";

                await logService.LogServerException(exception, null, LogType.MessageBrokerExceptionLog, null, customText);
            }
        }
    }
}
