using Infrastructure.Extensions;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Import.Services.TxtValidation.SpecialitySemesterTxtValidation
{
    public class EraseSpecialitySemesterTxtValidationService
    {
        public void EraseSemesterValidation(PersonStudent personStudent, List<LineColumnDto> lineColumnDtos, List<TxtSpecValidationErrorCode> txtValidationErrorCodes)
        {
            var periodYear = int.Parse(lineColumnDtos.Single(e => e.Index == 3).Value);
            var periodSemester = lineColumnDtos.Single(e => e.Index == 4).Value.GetEnumValueString<Semester>();
            var studentEventId = int.Parse(lineColumnDtos.Single(e => e.Index == 5).Value);
            var courseType = lineColumnDtos.Single(e => e.Index == 6).Value.GetEnumValueString<CourseType>();
            var studentSemester = lineColumnDtos.Single(e => e.Index == 7).Value.GetEnumValueString<Semester>();

            var lastStudentSemester = personStudent.Semesters
                    .OrderByDescending(e => e.Period.Year)
                    .ThenByDescending(e => e.Period.Semester)
                    .ThenByDescending(e => e.StudentStatusId)
                    .ThenByDescending(e => e.Id)
                    .First();

            if (periodYear != lastStudentSemester.Period.Year
                || periodSemester != lastStudentSemester.Period.Semester
                || studentEventId != lastStudentSemester.StudentEventId
                || courseType != lastStudentSemester.Course
                || studentSemester != lastStudentSemester.StudentSemester)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.SemesterForEraseIsNotLastOrNotFound);
            }
        }

        public void EraseDoctoralSemesterValidation(PersonDoctoral personDoctoral, List<LineColumnDto> lineColumnDtos, List<TxtSpecValidationErrorCode> txtValidationErrorCodes)
        {
            var protocolNumber = lineColumnDtos.Single(e => e.Index == 3).Value;
            var protocolDate = lineColumnDtos.Single(e => e.Index == 4).Value.GetDateTimeString().Value;
            var studentEventId = int.Parse(lineColumnDtos.Single(e => e.Index == 5).Value);
            var doctoralYearType = lineColumnDtos.Single(e => e.Index == 6).Value.GetEnumValueString<YearType>();

            var lastDoctoralSemester = personDoctoral.Semesters
                    .OrderByDescending(e => e.ProtocolDate.Date)
                    .ThenByDescending(e => e.Id)
                    .First();

            if (protocolNumber != lastDoctoralSemester.ProtocolNumber
                || protocolDate.Date != lastDoctoralSemester.ProtocolDate.Date
                || studentEventId != lastDoctoralSemester.StudentEventId
                || doctoralYearType != lastDoctoralSemester.YearType)
            {
                txtValidationErrorCodes.Add(TxtSpecValidationErrorCode.DoctoralSemesterForEraseIsNotLastOrNotFound);
            }
        }
    }
}
