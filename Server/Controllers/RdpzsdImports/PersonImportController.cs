using Infrastructure.DomainValidation;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Import.Permissions;
using Rdpzsd.Import.Services;
using Rdpzsd.Models.Enums.RdpzsdImportFile;
using Rdpzsd.Models.Models.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports.Files;
using Rdpzsd.Services.Permissions;
using Server.Controllers.RdpzsdImports.Base;
using System.Threading.Tasks;

namespace Server.Controllers.RdpzsdImports
{
	[ApiController]
	[Route("api/[controller]")]
	public class PersonImportController : RdpzsdImportController<PersonImport, PersonImportFile, PersonImportErrorFile, PersonImportHistory, PersonImportHistoryFile, PersonImportHistoryErrorFile, PersonImportService, AwPersonImportDto>
    {
		public PersonImportController(
			PersonImportService personImportService,
			PermissionService permissionService,
			RdpzsdImportPermissionService rdpzsdImportPermissionService,
			DomainValidatorService domainValidatorService,
			ActionWorkflowService<AwPersonImportDto> actionWorkflowService
			)
			: base(personImportService, permissionService, rdpzsdImportPermissionService, domainValidatorService, actionWorkflowService)
		{
		}

		[HttpGet("{personImportId:int}/UanExcel")]
		public async Task<FileStreamResult> ExportUanExcel([FromRoute] int personImportId)
		{
			var rdpzsdImport = await rdpzsdImportService.GetWithoutIncludes(personImportId);
			rdpzsdImportPermissionService.VerifyReadPermissions(rdpzsdImport.InstitutionId, rdpzsdImport.SubordinateId);
			rdpzsdImportPermissionService.VerifyState(rdpzsdImport.State, ImportState.Registered);

			var excelStream = await rdpzsdImportService.ExportUanExcel(personImportId);

			return new FileStreamResult(excelStream, "application/vnd.ms-excel");
		}
	}
}
