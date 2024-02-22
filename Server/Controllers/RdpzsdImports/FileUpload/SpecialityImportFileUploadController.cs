using FilesStorageNetCore.FormDataHelpers;
using Infrastructure.DomainValidation;
using Infrastructure.FileUpload;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Import.Permissions;
using Rdpzsd.Import.Services;
using Rdpzsd.Import.Services.TxtParser;
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
    public class SpecialityImportFileUploadController : ControllerBase
    {
        private readonly SpecialityImportService specialityImportService;
        private readonly FileUploadService<SpecialityImportFile> fileUploadService;
        private readonly PermissionService permissionService;
        private readonly RdpzsdImportPermissionService rdpzsdImportPermissionService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly TxtParserService txtParserService;
        private readonly ActionWorkflowService<AwSpecialityImportDto> actionWorkflowService;

        public SpecialityImportFileUploadController(
            SpecialityImportService specialityImportService,
            FileUploadService<SpecialityImportFile> fileUploadService,
            PermissionService permissionService,
            RdpzsdImportPermissionService rdpzsdImportPermissionService,
            DomainValidatorService domainValidatorService,
            TxtParserService txtParserService,
            ActionWorkflowService<AwSpecialityImportDto> actionWorkflowService
        )
        {
            this.specialityImportService = specialityImportService;
            this.fileUploadService = fileUploadService;
            this.permissionService = permissionService;
            this.rdpzsdImportPermissionService = rdpzsdImportPermissionService;
            this.domainValidatorService = domainValidatorService;
            this.txtParserService = txtParserService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost, DisableRequestSizeLimit, DisableFormValueModelBinding]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue, MemoryBufferThreshold = int.MaxValue)]
        public async Task<ActionResult<SpecialityImportFile>> Upload()
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            permissionService.VerifyRsdUser();
            await rdpzsdImportPermissionService.VerifyUniqueSpecialityImportInstitution();

            var (fileStream, fileName, fileMimeType) = await fileUploadService.RecieveFile(Request.Body, Request.ContentType, FileUploadType.TxtFile, 15728640);
            txtParserService.ValidateTxtImportType(fileStream, ImportType.SpecialityImport);

            return Ok(await fileUploadService.SaveFile(fileStream, fileName, fileMimeType));
        }

        [HttpPost, DisableRequestSizeLimit, DisableFormValueModelBinding]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue, MemoryBufferThreshold = int.MaxValue)]
        [Route("{specialityImportId:int}")]
        public async Task<ActionResult<SpecialityImport>> ChangeUpload(int specialityImportId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            await actionWorkflowService.LogUpdateAction($"ид - {specialityImportId}");
            var specialityImport = await specialityImportService.Get(specialityImportId);
            rdpzsdImportPermissionService.VerifyEditPermissions(specialityImport.UserId);
            rdpzsdImportPermissionService.VerifyState(specialityImport.State, ImportState.Error, ImportState.Validated, ImportState.ValidationServerError, ImportState.RegistrationServerError);

            var (fileStream, fileName, fileMimeType) = await fileUploadService.RecieveFile(Request.Body, Request.ContentType, FileUploadType.TxtFile, 15728640);
            txtParserService.ValidateTxtImportType(fileStream, ImportType.SpecialityImport);
            var specialityImportFile = await fileUploadService.SaveFile(fileStream, fileName, fileMimeType);

            return Ok(await specialityImportService.ChangeImportFile(specialityImportFile, specialityImport));
        }
    }
}
