using Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser;
using System.Collections.Generic;

namespace Rdpzsd.Import.Services.TxtValidation.SpecialitySemesterTxtValidation
{
    public class EditSpecialitySemesterTxtValidationService
    {
        public void EditSemesterValidation(List<LineColumnDto> lineColumnDtos, List<TxtSpecValidationErrorCode> txtValidationErrorCodes)
        {
            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.EditNotImplemented);
        }

        public void EditSpecialityValidation(List<LineColumnDto> lineColumnDtos, List<TxtSpecValidationErrorCode> txtValidationErrorCodes)
        {
            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.EditNotImplemented);
        }

        public void EditDoctoralSemesterValidation(List<LineColumnDto> lineColumnDtos, List<TxtSpecValidationErrorCode> txtValidationErrorCodes)
        {
            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.EditNotImplemented);
        }

        public void EditDoctoralProgrammeValidation(List<LineColumnDto> lineColumnDtos, List<TxtSpecValidationErrorCode> txtValidationErrorCodes)
        {
            txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.EditNotImplemented);
        }
    }
}
