using Infrastructure.AppSettings;
using Logs.Enums;
using Logs.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rdpzsd.Import.Services;
using Rdpzsd.Import.Services.TxtParser;
using Rdpzsd.Import.Services.TxtProcessing;
using Rdpzsd.Import.Services.TxtValidation;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtValidationErrorCodes;
using Rdpzsd.Models.Enums.RdpzsdImportFile;
using Rdpzsd.Models.Models.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports.Files;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rdpzsd.Import.Jobs
{
    public class ValidationTxtJob : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly IServiceProvider serviceProvider;

        public ValidationTxtJob(
            IServiceProvider serviceProvider
        )
        {
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(AppSettingsProvider.ValidationTxtJob.JobInterval));

            return Task.CompletedTask;
        }

        public async void DoWork(object state)
        {
            using var scope = serviceProvider.CreateScope();
            var logService = scope.ServiceProvider
                .GetRequiredService<LogService>();
            var txtParserService = scope.ServiceProvider
                .GetRequiredService<TxtParserService>();
            var txtExcelService = scope.ServiceProvider
                .GetRequiredService<TxtExcelService>();
            var personImportService = scope.ServiceProvider
                .GetRequiredService<PersonImportService>();
            var specialityImportService = scope.ServiceProvider
                .GetRequiredService<SpecialityImportService>();

            var personImport = await personImportService.GetFirstByState(ImportState.Draft);
            SpecialityImport specialityImport = null;

            if (personImport == null)
            {
                specialityImport = await specialityImportService.GetFirstByState(ImportState.Draft);
            }

            if (personImport != null && personImport.ImportFile != null)
            {
                var personImportTxtValidationService = scope.ServiceProvider
                    .GetRequiredService<PersonImportTxtValidationService>();

                try
                {
                    await personImportService.ChangeState(personImport, ImportState.InProgress, null, null, null, null, null, null);

                    var fileByteArray = await personImportService.GetFileBytes<PersonImportFile>(personImport.Id);
                    using MemoryStream fileStream = new MemoryStream(fileByteArray);
                    string parsedString = txtParserService.ParseStreamToStringTxt(fileStream);

                    var linesDto = txtParserService.ConstructLineDtoList<TxtValidationErrorCode>(parsedString, LineDtoType.NaturalPerson);

                    personImportTxtValidationService.Validate(linesDto);

                    var linesDtoCount = linesDto.Count();
                    var linesDtoUinFnCount = linesDto.Count(e => !string.IsNullOrWhiteSpace(e.Columns[1]?.Value) || !string.IsNullOrWhiteSpace(e.Columns[2]?.Value));
                    var linesDtoIdnCount = linesDto.Count(e => string.IsNullOrWhiteSpace(e.Columns[1]?.Value) 
                        && string.IsNullOrWhiteSpace(e.Columns[2]?.Value));

                    if (linesDto.Any(e => e.ErrorCodes.Any()))
                    {
                        var errorLinesDto = linesDto.Where(e => e.ErrorCodes.Any()).ToList();
                        var errorLinesUinFnDtoCount = errorLinesDto.Count(e => !string.IsNullOrWhiteSpace(e.Columns[1]?.Value) || !string.IsNullOrWhiteSpace(e.Columns[2]?.Value));
                        var errorLinesIdnDtoCount = errorLinesDto.Count(e => string.IsNullOrWhiteSpace(e.Columns[1]?.Value)
                            && string.IsNullOrWhiteSpace(e.Columns[2]?.Value));
                        var errorFileStream = txtExcelService.GeneratePersonImportErrorExcel(errorLinesDto);
                        await personImportService.CreateErrorFile(errorFileStream, personImport, linesDtoCount, linesDtoCount - errorLinesDto.Count, linesDtoUinFnCount, linesDtoUinFnCount - errorLinesUinFnDtoCount, linesDtoIdnCount, linesDtoIdnCount - errorLinesIdnDtoCount);
                    }
                    else
                    {
                        await personImportService.ChangeState(personImport, ImportState.WaitingRegistration, linesDtoCount, linesDtoCount, linesDtoUinFnCount, linesDtoUinFnCount, linesDtoIdnCount, linesDtoIdnCount);
                    }
                }
                catch (Exception exception)
                {
                    while (exception.InnerException != null)
                    { exception = exception.InnerException; }

                    await logService.LogServerException(exception, personImport.UserId, LogType.JobExceptionLog);
                    await personImportService.ChangeState(personImport, ImportState.ValidationServerError, null, null, null, null, null, null);
                }
            }
            else if (specialityImport != null && specialityImport.ImportFile != null)
            {
                var specialityImportTxtValidationService = scope.ServiceProvider
                    .GetRequiredService<SpecialityImportTxtValidationService>();

                try
                {
                    await specialityImportService.ChangeState(specialityImport, ImportState.InProgress, null, null, null, null, null, null);

                    var fileByteArray = await specialityImportService.GetFileBytes<SpecialityImportFile>(specialityImport.Id);
                    using MemoryStream fileStream = new MemoryStream(fileByteArray);
                    string parsedString = txtParserService.ParseStreamToStringTxt(fileStream);

                    var linesDto = txtParserService.ConstructLineDtoList<TxtSpecValidationErrorCode>(parsedString, LineDtoType.Speciality);

                    specialityImportTxtValidationService.Validate(linesDto, specialityImport.SubordinateId ?? specialityImport.InstitutionId, specialityImport.SubordinateId.HasValue);

                    var linesDtoCount = linesDto.Count();
                    var linesDtoStudentsCount = linesDto.Count(e => e.LineType == LineDtoType.Speciality);
                    var linesDtoDoctoralsCount = linesDto.Count(e => e.LineType == LineDtoType.DoctoralProgramme);

                    if (linesDto.Any(e => e.ErrorCodes.Any()))
                    {
                        var errorLinesDto = linesDto.Where(e => e.ErrorCodes.Any()).ToList();
                        var errorLinesStudentsDtoCount = errorLinesDto.Count(e => e.LineType == LineDtoType.Speciality);
                        var errorLinesDoctoralsDtoCount = errorLinesDto.Count(e => e.LineType == LineDtoType.DoctoralProgramme);
                        var errorFileStream = txtExcelService.GenerateSpecialityImportErrorExcel(errorLinesDto);
                        await specialityImportService.CreateErrorFile(errorFileStream, specialityImport, linesDtoCount, linesDtoCount - errorLinesDto.Count, linesDtoStudentsCount, linesDtoStudentsCount - errorLinesStudentsDtoCount, linesDtoDoctoralsCount, linesDtoDoctoralsCount - errorLinesDoctoralsDtoCount);
                    }
                    else
                    {
                        await specialityImportService.ChangeState(specialityImport, ImportState.WaitingRegistration, linesDtoCount, linesDtoCount, linesDtoStudentsCount, linesDtoStudentsCount, linesDtoDoctoralsCount, linesDtoDoctoralsCount);
                    }
                }
                catch (Exception exception)
                {
                    while (exception.InnerException != null)
                    { exception = exception.InnerException; }

                    await logService.LogServerException(exception, specialityImport.UserId, LogType.JobExceptionLog);
                    await specialityImportService.ChangeState(specialityImport, ImportState.ValidationServerError, null, null, null, null, null, null);
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
