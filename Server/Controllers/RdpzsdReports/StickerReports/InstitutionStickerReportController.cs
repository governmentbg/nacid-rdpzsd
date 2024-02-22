using Infrastructure.Constants;
using Logs.Dtos;
using Logs.Services;
using Microsoft.AspNetCore.Mvc;
using Rdpzsd.Reports.StickerReports.Dtos;
using Rdpzsd.Reports.StickerReports.Services;
using Rdpzsd.Services.Permissions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers.RdpzsdReports.StickerReports
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstitutionStickerReportController : ControllerBase
    {
        private readonly InstitutionStickerReportService institutionStickerReportService;
        private readonly PermissionService permissionService;
        private readonly ActionWorkflowService<AwReportDto> actionWorkflowService;

        public InstitutionStickerReportController(
            InstitutionStickerReportService institutionStickerReportService,
            PermissionService permissionService,
            ActionWorkflowService<AwReportDto> actionWorkflowService
        )
        {
            this.institutionStickerReportService = institutionStickerReportService;
            this.permissionService = permissionService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost("Excel")]
        public async virtual Task<FileStreamResult> InstitutionStickerReportExcel([FromBody] StickerReportFilterDto filter)
        {
            await actionWorkflowService.LogCustomAction("Изтегляне на excel справка стикери по ВУ");
            permissionService.VerifyInternalRsdUserPermissions(PermissionConstants.RdpzsdStickers);
            var excelStream = await institutionStickerReportService.InstitutionStickerReportExcel(filter);

            return new FileStreamResult(excelStream, "application/vnd.ms-excel");
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<InstitutionStickerReportDto>>> GetInstitutionStickerReport([FromBody] StickerReportFilterDto filter)
        {
            await actionWorkflowService.LogCustomAction("Преглед на справка стикери по ВУ");

            permissionService.VerifyInternalRsdUserPermissions(PermissionConstants.RdpzsdStickers);

            return Ok(await institutionStickerReportService.GetInstitutionStickerReport(filter));
        }

    }
}
