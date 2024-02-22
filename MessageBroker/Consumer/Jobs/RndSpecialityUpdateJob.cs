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
    public class RndSpecialityUpdateJob : BaseConsumerJob
    {
        private readonly IServiceProvider serviceProvider;

        public RndSpecialityUpdateJob(
            IServiceProvider serviceProvider,
            RdpzsdMbConsumer rdpzsdMbConsumer
        ) : base(AppSettingsProvider.MessageBroker.RdpzsdConsumer.RndSpecialityUpdateExchange, rdpzsdMbConsumer)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override async Task HandleBody(byte[] body)
        {
            using var scope = serviceProvider.CreateScope();
            var rndSpecialityUpdateService = scope.ServiceProvider
                .GetRequiredService<RndSpecialityUpdateService>();
            var logService = scope.ServiceProvider
                .GetRequiredService<LogService>();
            var specialityForUpdate = JsonConvert.DeserializeObject<Speciality>(Encoding.UTF8.GetString(body));

            try
            {
                await rndSpecialityUpdateService.UpdateSpeciality(specialityForUpdate);
            }
            catch (Exception exception)
            {
                while (exception.InnerException != null)
                { exception = exception.InnerException; }

                var customText = $"Специалност с Id: {specialityForUpdate.Id} и Код: {specialityForUpdate.Code} не е въведена/редактирана през message broker-а";

                await logService.LogServerException(exception, null, LogType.MessageBrokerExceptionLog, null, customText);
            }
        }
    }
}
