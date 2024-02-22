using FilesStorageNetCore.FormDataHelpers;
using Infrastructure.DomainValidation;
using Infrastructure.FileUpload;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Import.Permissions;
using Rdpzsd.Import.Services;
using Rdpzsd.Import.Services.TxtParser;
using Rdpzsd.Import.Services.TxtValidation;
using Rdpzsd.Models.Enums.RdpzsdImportFile;
using Rdpzsd.Models.Models.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports.Enums;
using Rdpzsd.Models.Models.RdpzsdImports.Files;
using Rdpzsd.Services.Permissions;
using System.Threading.Tasks;

namespace Server.Controllers.RdpzsdImports.FileUpload
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonImportFileUploadController : ControllerBase
    {
        private readonly PersonImportService personImportService;
        private readonly FileUploadService<PersonImportFile> fileUploadService;
        private readonly PermissionService permissionService;
        private readonly RdpzsdImportPermissionService rdpzsdImportPermissionService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly TxtParserService txtParserService;
        private readonly ActionWorkflowService<AwPersonImportDto> actionWorkflowService;

        public PersonImportFileUploadController(
            PersonImportService personImportService,
            FileUploadService<PersonImportFile> fileUploadService,
            PermissionService permissionService,
            RdpzsdImportPermissionService rdpzsdImportPermissionService,
            DomainValidatorService domainValidatorService,
            TxtParserService txtParserService,
            ActionWorkflowService<AwPersonImportDto> actionWorkflowService
        )
        {
            this.personImportService = personImportService;
            this.fileUploadService = fileUploadService;
            this.permissionService = permissionService;
            this.rdpzsdImportPermissionService = rdpzsdImportPermissionService;
            this.domainValidatorService = domainValidatorService;
            this.txtParserService = txtParserService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost, DisableRequestSizeLimit, DisableFormValueModelBinding]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue, MemoryBufferThreshold = int.MaxValue)]
        public async Task<ActionResult<PersonImportFile>> Upload()
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            permissionService.VerifyRsdUser();

            var (fileStream, fileName, fileMimeType) = await fileUploadService.RecieveFile(Request.Body, Request.ContentType, FileUploadType.TxtFile, 15728640);
            txtParserService.ValidateTxtImportType(fileStream, ImportType.PersonImport);

            return Ok(await fileUploadService.SaveFile(fileStream, fileName, fileMimeType));
        }

        [HttpPost, DisableRequestSizeLimit, DisableFormValueModelBinding]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue, MemoryBufferThreshold = int.MaxValue)]
        [Route("{personImportId:int}")]
        public async Task<ActionResult<PersonImport>> ChangeUpload(int personImportId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogUpdateAction($"ид - {personImportId}");
            var personImport = await personImportService.Get(personImportId);
            rdpzsdImportPermissionService.VerifyEditPermissions(personImport.UserId);
            rdpzsdImportPermissionService.VerifyState(personImport.State, ImportState.Error, ImportState.Validated, ImportState.ValidationServerError, ImportState.RegistrationServerError);

            var (fileStream, fileName, fileMimeType) = await fileUploadService.RecieveFile(Request.Body, Request.ContentType, FileUploadType.TxtFile, 15728640);
            txtParserService.ValidateTxtImportType(fileStream, ImportType.PersonImport);
            var personImportFile = await fileUploadService.SaveFile(fileStream, fileName, fileMimeType);

            return Ok(await personImportService.ChangeImportFile(personImportFile, personImport));
        }
    }
}
