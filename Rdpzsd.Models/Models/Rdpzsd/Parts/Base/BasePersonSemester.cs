using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Models.Models.Nomenclatures.StudentStatus;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.Base
{
    public abstract class BasePersonSemester : EntityVersion
    {
        public int PartId { get; set; }
        public int StudentStatusId { get; set; }
        [Skip]
        public StudentStatus StudentStatus { get; set; }
        public int StudentEventId { get; set; }
        [Skip]
        public StudentEvent StudentEvent { get; set; }

        public int? EducationFeeTypeId { get; set; }
        [Skip]
        public EducationFeeType EducationFeeType { get; set; }

        public int? RelocatedFromPartId { get; set; }

        public string Note { get; set; }

        public bool HasScholarship { get; set; }
        public bool UseHostel { get; set; }
        public bool UseHolidayBase { get; set; }
        public bool ParticipatedIntPrograms { get; set; }

        protected virtual void ValidateSemester(DomainValidatorService domainValidatorService)
        {
            if (StudentStatusId == 0)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidStudentStatus);
            }

            if (StudentEventId == 0)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidStudentEvent);
            }

            if (StudentStatus.Alias == StudentStatusConstants.Active && !EducationFeeTypeId.HasValue)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidEducationFeeType);
            }

            if (StudentEvent.Alias == StudentEventConstants.NextSemesterAfterRelocation && !RelocatedFromPartId.HasValue)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_InvalidRelocatedFromPart);
            }
        }
    }
}
