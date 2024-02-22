using FilesStorageNetCore.FormDataHelpers;
using FileStorageNetCore;
using FileStorageNetCore.Api;
using FileStorageNetCore.Models;
using Infrastructure.DomainValidation;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Services;
using System.Threading.Tasks;

namespace Server.Controllers.FileStorage
{
    [Route("api/FileStorage")]
    public class NacidRdpzsdFileStorage : FileStorageController
    {
        private readonly ImageFileService imageFileService;
        private readonly DomainValidatorService domainValidatorService;

        public NacidRdpzsdFileStorage(
            BlobStorageService service,
            ImageFileService imageFileService,
            DomainValidatorService domainValidatorService)
            : base(service)
        {
            this.imageFileService = imageFileService;
            this.domainValidatorService = domainValidatorService;
        }

        [HttpPost]
        [DisableFormValueModelBinding]
        public override async Task<AttachedFile> PostFile(int? dbId)
        {
            domainValidatorService.ThrowFunctionalityNotSupportedError();
            return await base.PostFile(dbId);
        }

        [HttpPost("Image")]
        public async Task<ActionResult<string>> GetNewImage([FromBody] AttachedFile file)
        {
            return Ok(await this.imageFileService.GetBase64ImageUrlAsync(file.Key, file.DbId));
        }
    }
}
