using Infrastructure.DomainValidation;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Import.Permissions;
using Rdpzsd.Import.Services;
using Rdpzsd.Models.Models.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports.Files;
using Rdpzsd.Services.Permissions;
using Server.Controllers.RdpzsdImports.Base;
using System.Threading.Tasks;

namespace Server.Controllers.RdpzsdImports
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialityImportController : RdpzsdImportController<SpecialityImport, SpecialityImportFile, SpecialityImportErrorFile, SpecialityImportHistory, SpecialityImportHistoryFile, SpecialityImportHistoryErrorFile, SpecialityImportService, AwSpecialityImportDto>
    {
		public SpecialityImportController(
			SpecialityImportService specialityImportService,
			PermissionService permissionService,
			RdpzsdImportPermissionService rdpzsdImportPermissionService,
			DomainValidatorService domainValidatorService,
            ActionWorkflowService<AwSpecialityImportDto> actionWorkflowService
			)
			: base(specialityImportService, permissionService, rdpzsdImportPermissionService, domainValidatorService, actionWorkflowService)
		{
		}

        [HttpPost]
        [Route("Create")]
        public async override Task<ActionResult<SpecialityImport>> Post([FromBody] SpecialityImport rdpzsdImport)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCreateAction($"ид - {rdpzsdImport.Id}");
            permissionService.VerifyRsdUser();
            await rdpzsdImportPermissionService.VerifyUniqueSpecialityImportInstitution();

            return Ok(await rdpzsdImportService.Create(rdpzsdImport));
        }
    }
}
