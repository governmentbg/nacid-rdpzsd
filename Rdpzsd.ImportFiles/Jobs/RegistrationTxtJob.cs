using Infrastructure.AppSettings;
using Logs.Enums;
using Logs.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rdpzsd.Import.Services;
using Rdpzsd.Import.Services.TxtParser;
using Rdpzsd.Import.Services.TxtValidation;
using Rdpzsd.Models.Enums.RdpzsdImportFile;
using Rdpzsd.Models.Models.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports.Files;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rdpzsd.Import.Jobs
{
    public class RegistrationTxtJob : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly IServiceProvider serviceProvider;

        public RegistrationTxtJob(
            IServiceProvider serviceProvider
        )
        {
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(AppSettingsProvider.RegistrationTxtJob.JobInterval));

            return Task.CompletedTask;
        }

        public async void DoWork(object state)
        {
            using var scope = serviceProvider.CreateScope();
            var logService = scope.ServiceProvider
                .GetRequiredService<LogService>();
            var txtParserService = scope.ServiceProvider
                .GetRequiredService<TxtParserService>();
            var personImportService = scope.ServiceProvider
                .GetRequiredService<PersonImportService>();
            var specialityImportService = scope.ServiceProvider
                .GetRequiredService<SpecialityImportService>();

            PersonImport personImport = null;
            SpecialityImport specialityImport = null;

            if (!await personImportService.HasEntityWithState(ImportState.InProgressRegistration))
            {
                personImport = await personImportService.GetFirstByState(ImportState.WaitingRegistration);
            }
            
            if (personImport == null && !await specialityImportService.HasEntityWithState(ImportState.InProgressRegistration))
            {
              specialityImport = await specialityImportService.GetFirstByState(ImportState.WaitingRegistration);
            }

            if (personImport != null && personImport.ImportFile != null)
            {
                var personImportTxtRegistrationService = scope.ServiceProvider
                    .GetRequiredService<PersonImportTxtRegistrationService>();

                try
                {
                    await personImportService.ChangeState(personImport, ImportState.InProgressRegistration,
                        personImport.EntitiesCount, personImport.EntitiesAcceptedCount,
                        personImport.FirstCriteriaCount, personImport.FirstCriteriaAcceptedCount,
                        personImport.SecondCriteriaCount, personImport.SecondCriteriaAcceptedCount);

                    var fileByteArray = await personImportService.GetFileBytes<PersonImportFile>(personImport.Id);
                    using MemoryStream fileStream = new MemoryStream(fileByteArray);
                    string parsedString = txtParserService.ParseStreamToStringTxt(fileStream);

                    var finishDate = DateTime.Now;
                    var personLotList = txtParserService.ParseToPersonLotList(parsedString, finishDate, personImport.UserId, personImport.UserFullname, personImport.InstitutionId, personImport.SubordinateId);
                    await personImportTxtRegistrationService.Register(personLotList, finishDate, personImport.Id);
                    await personImportService.ChangeState(personImport, ImportState.Registered,
                        personImport.EntitiesCount, personImport.EntitiesAcceptedCount,
                        personImport.FirstCriteriaCount, personImport.FirstCriteriaAcceptedCount,
                        personImport.SecondCriteriaCount, personImport.SecondCriteriaAcceptedCount, finishDate);
                }
                catch (Exception exception)
                {
                    while (exception.InnerException != null)
                    { exception = exception.InnerException; }

                    await logService.LogServerException(exception, personImport.UserId, LogType.JobExceptionLog);
                    await personImportService.ChangeState(personImport, ImportState.RegistrationServerError,
                        personImport.EntitiesCount, personImport.EntitiesAcceptedCount,
                        personImport.FirstCriteriaCount, personImport.FirstCriteriaAcceptedCount,
                        personImport.SecondCriteriaCount, personImport.SecondCriteriaAcceptedCount);
                }
            }
            else if (specialityImport != null && specialityImport.ImportFile != null)
            {
                var specialityImportTxtRegistrationService = scope.ServiceProvider
                    .GetRequiredService<SpecialityImportTxtRegistrationService>();

                try
                {
                    await specialityImportService.ChangeState(specialityImport, ImportState.InProgressRegistration, 
                        specialityImport.EntitiesCount, specialityImport.EntitiesAcceptedCount,
                        specialityImport.FirstCriteriaCount, specialityImport.FirstCriteriaAcceptedCount,
                        specialityImport.SecondCriteriaCount, specialityImport.SecondCriteriaAcceptedCount);
                    var fileByteArray = await specialityImportService.GetFileBytes<SpecialityImportFile>(specialityImport.Id);
                    using MemoryStream fileStream = new MemoryStream(fileByteArray);
                    string parsedString = txtParserService.ParseStreamToStringTxt(fileStream);

                    var finishDate = DateTime.Now;
                    var personStudentTxtDtoList = txtParserService.ParseToPersonStudentDoctoralTxtDtoList(parsedString, finishDate, specialityImport.UserId, specialityImport.UserFullname, specialityImport.InstitutionId, specialityImport.SubordinateId);
                    await specialityImportTxtRegistrationService.Register(personStudentTxtDtoList, specialityImport.InstitutionId);
                    await specialityImportService.ChangeState(specialityImport, ImportState.Registered,
                        specialityImport.EntitiesCount, specialityImport.EntitiesAcceptedCount,
                        specialityImport.FirstCriteriaCount, specialityImport.FirstCriteriaAcceptedCount,
                        specialityImport.SecondCriteriaCount, specialityImport.SecondCriteriaAcceptedCount, finishDate);
                }
                catch (Exception exception)
                {
                    while (exception.InnerException != null)
                    { exception = exception.InnerException; }

                    await logService.LogServerException(exception, specialityImport.UserId, LogType.JobExceptionLog);
                    await specialityImportService.ChangeState(specialityImport, ImportState.RegistrationServerError, 
                        specialityImport.EntitiesCount, specialityImport.EntitiesAcceptedCount,
                        specialityImport.FirstCriteriaCount, specialityImport.FirstCriteriaAcceptedCount,
                        specialityImport.SecondCriteriaCount, specialityImport.SecondCriteriaAcceptedCount);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
