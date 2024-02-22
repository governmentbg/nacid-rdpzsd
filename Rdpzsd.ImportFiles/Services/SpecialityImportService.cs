using FileStorageNetCore.Api;
using Infrastructure.DomainValidation;
using Infrastructure.ExcelProcessor.Services;
using Infrastructure.Integrations.EmsIntegration;
using Infrastructure.User;
using Rdpzsd.Import.Services.Base;
using Rdpzsd.Models;
using Rdpzsd.Models.Models.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports.Files;

namespace Rdpzsd.Import.Services
{
    public class SpecialityImportService : RdpzsdImportService<SpecialityImport, SpecialityImportFile, SpecialityImportErrorFile, SpecialityImportHistory, SpecialityImportHistoryFile, SpecialityImportHistoryErrorFile>
	{
		public SpecialityImportService(
			RdpzsdDbContext context,
			UserContext userContext,
			DomainValidatorService domainValidatorService,
			BlobStorageService blobStorageService,
			EmsIntegrationService emsIntegrationService,
			ExcelProcessorService excelProcessorService
		) : base(context, userContext, domainValidatorService, blobStorageService, emsIntegrationService, excelProcessorService)
		{
		}
	}
}
