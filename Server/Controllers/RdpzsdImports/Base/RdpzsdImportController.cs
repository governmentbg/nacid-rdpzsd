using Infrastructure.DomainValidation;
using Logs.Interfaces;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Import.Permissions;
using Rdpzsd.Import.Services.Base;
using Rdpzsd.Models.Enums.RdpzsdImportFile;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.RdpzsdImports.Base;
using Rdpzsd.Services.Permissions;
using System.IO;
using System.Threading.Tasks;

namespace Server.Controllers.RdpzsdImports.Base
{
    public abstract class RdpzsdImportController<TRdpzsdImport, TFile, TErrorFile, TImportHistory, TImportHistoryFile, TImportHistoryErrorFile, TRdpzsdImportService, TActionWorkflowOperation> : ControllerBase
        where TRdpzsdImport : RdpzsdImport<TFile, TErrorFile, TImportHistory, TImportHistoryFile, TImportHistoryErrorFile>, IIncludeAll<TRdpzsdImport>, new()
        where TFile : RdpzsdAttachedFile
        where TErrorFile : RdpzsdAttachedFile, new()
        where TImportHistory : RdpzsdImportHistory<TImportHistoryFile, TImportHistoryErrorFile>, new()
        where TImportHistoryFile : RdpzsdAttachedFile, new()
        where TImportHistoryErrorFile : RdpzsdAttachedFile, new()
        where TRdpzsdImportService : RdpzsdImportService<TRdpzsdImport, TFile, TErrorFile, TImportHistory, TImportHistoryFile, TImportHistoryErrorFile>
        where TActionWorkflowOperation : IActionWorkflowOperation, new()
    {
        protected readonly TRdpzsdImportService rdpzsdImportService;
        protected readonly PermissionService permissionService;
        protected readonly RdpzsdImportPermissionService rdpzsdImportPermissionService;
        protected readonly DomainValidatorService domainValidatorService;
        protected readonly ActionWorkflowService<TActionWorkflowOperation> actionWorkflowService;


        public RdpzsdImportController(
            TRdpzsdImportService rdpzsdImportService,
            PermissionService permissionService,
            RdpzsdImportPermissionService rdpzsdImportPermissionService,
            DomainValidatorService domainValidatorService,
            ActionWorkflowService<TActionWorkflowOperation> actionWorkflowService
            )
        {
            this.rdpzsdImportService = rdpzsdImportService;
            this.permissionService = permissionService;
            this.rdpzsdImportPermissionService = rdpzsdImportPermissionService;
            this.domainValidatorService = domainValidatorService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<TRdpzsdImport>> Get([FromRoute] int id)
        {
            await actionWorkflowService.LogGetAction($"ид - {id}");
            var rdpzsdImport = await rdpzsdImportService.Get(id);
            rdpzsdImportPermissionService.VerifyReadPermissions(rdpzsdImport.InstitutionId, rdpzsdImport.SubordinateId);

            return Ok(rdpzsdImport);
        }

        [HttpPost]
        [Route("Create")]
        public async virtual Task<ActionResult<TRdpzsdImport>> Post([FromBody] TRdpzsdImport rdpzsdImport)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogCreateAction($"ид - {rdpzsdImport.Id}");
            permissionService.VerifyRsdUser();

            return Ok(await rdpzsdImportService.Create(rdpzsdImport));
        }

        [HttpGet]
        [Route("{id:int}/SetForRegistration")]
        public async virtual Task<ActionResult<TRdpzsdImport>> SetForRegistration([FromRoute] int id)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            var rdpzsdImport = await rdpzsdImportService.GetWithoutIncludes(id);
            rdpzsdImportPermissionService.VerifyEditPermissions(rdpzsdImport.UserId);
            rdpzsdImportPermissionService.VerifyState(rdpzsdImport.State, ImportState.Validated);
            await rdpzsdImportService.ChangeState(rdpzsdImport, ImportState.WaitingRegistration, 
                rdpzsdImport.EntitiesCount, rdpzsdImport.EntitiesAcceptedCount,
                rdpzsdImport.FirstCriteriaCount, rdpzsdImport.FirstCriteriaAcceptedCount,
                rdpzsdImport.SecondCriteriaCount, rdpzsdImport.SecondCriteriaAcceptedCount);

            return Ok(await rdpzsdImportService.Get(rdpzsdImport.Id));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async virtual Task<ActionResult<TRdpzsdImport>> Delete([FromRoute] int id)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogDeleteAction($"ид - {id}");
            var rdpzsdImport = await rdpzsdImportService.GetWithoutIncludes(id);
            rdpzsdImportPermissionService.VerifyEditPermissions(rdpzsdImport.UserId);
            rdpzsdImportPermissionService.VerifyState(rdpzsdImport.State, ImportState.Error, ImportState.Validated, ImportState.ValidationServerError, ImportState.RegistrationServerError);

            return Ok(await rdpzsdImportService.DeleteImport(rdpzsdImport));
        }

        [HttpGet]
        [Route("{id:int}/DownloadImportFile")]
        public async Task<FileStreamResult> DownloadImportFile([FromRoute] int id)
        {
            var rdpzsdImport = await rdpzsdImportService.GetWithoutIncludes(id);
            await actionWorkflowService.LogCustomAction($"Изтегляне на импорт файл, ид - {id}");
            rdpzsdImportPermissionService.VerifyReadPermissions(rdpzsdImport.InstitutionId, rdpzsdImport.SubordinateId);

            var fileBytes = await rdpzsdImportService.GetFileBytes<TFile>(id);
            return new FileStreamResult(new MemoryStream(fileBytes), "text/plain");
        }

        [HttpGet]
        [Route("{id:int}/DownloadImportErrorFile")]
        public async Task<FileStreamResult> DownloadImportErrorFile([FromRoute] int id)
        {
            await actionWorkflowService.LogCustomAction($"Изтегляне на файл с грешки от импорт файл, ид - {id}");
            var rdpzsdImport = await rdpzsdImportService.GetWithoutIncludes(id);
            rdpzsdImportPermissionService.VerifyReadPermissions(rdpzsdImport.InstitutionId, rdpzsdImport.SubordinateId);

            var fileBytes = await rdpzsdImportService.GetFileBytes<TErrorFile>(id);
            return new FileStreamResult(new MemoryStream(fileBytes), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
