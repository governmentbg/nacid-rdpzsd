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
    public class StudentStickerReportController : ControllerBase
    {
        private readonly StudentStickerReportService studentStickerReportService;
        private readonly PermissionService permissionService;
        private readonly ActionWorkflowService<AwReportDto> actionWorkflowService;

        public StudentStickerReportController(
            StudentStickerReportService studentStickerReportService,
            PermissionService permissionService,
            ActionWorkflowService<AwReportDto> actionWorkflowService
        )
        {
            this.studentStickerReportService = studentStickerReportService;
            this.permissionService = permissionService;
            this.actionWorkflowService = actionWorkflowService;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<StudentStickerReportDto>>> GetStudentStickerReport([FromBody] StickerReportFilterDto filter)
        {
            await actionWorkflowService.LogCustomAction("Преглед на справка стикери по студенти");
            permissionService.VerifyInternalRsdUserPermissions(PermissionConstants.RdpzsdStickers);

            return Ok(await studentStickerReportService.GetStudentStickerReport(filter));
        }

        [HttpPost("Excel")]
        public async virtual Task<FileStreamResult> StudentStickerReportExcel([FromBody] StickerReportFilterDto filter)
        {
            await actionWorkflowService.LogCustomAction("Изтегляне на excel справка стикери по студенти");

            permissionService.VerifyInternalRsdUserPermissions(PermissionConstants.RdpzsdStickers);
            var excelStream = await studentStickerReportService.StudentStickerReportExcel(filter);

            return new FileStreamResult(excelStream, "application/vnd.ms-excel");
        }
    }
}
