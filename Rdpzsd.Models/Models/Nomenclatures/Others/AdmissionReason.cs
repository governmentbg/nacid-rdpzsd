using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Collections.Generic;
using System.Linq;
using Rdpzsd.Models.Extensions;
using Infrastructure.Constants;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Nomenclature.Country;
using Rdpzsd.Models.Enums.Nomenclature.AdmissionReason;

namespace Rdpzsd.Models.Models.Nomenclatures.Others
{
    public class AdmissionReason : Nomenclature, IIncludeAll<AdmissionReason>, IValidate
    {
        public string OldCode { get; set; }
        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }
        public string Description { get; set; }
        public AdmissionReasonStudentType? AdmissionReasonStudentType { get; set; }
        public CountryUnion? CountryUnion { get; set; }
        public ICollection<AdmissionReasonEducationFee> AdmissionReasonEducationFees { get; set; }
        public ICollection<AdmissionReasonHistory> AdmissionReasonHistories { get; set; }
        public ICollection<AdmissionReasonCitizenship> AdmissionReasonCitizenships { get; set; }

        public AdmissionReason()
        {
            AdmissionReasonEducationFees = new List<AdmissionReasonEducationFee>();
            AdmissionReasonHistories = new List<AdmissionReasonHistory>();
            AdmissionReasonCitizenships = new List<AdmissionReasonCitizenship>();
        }

        public IQueryable<AdmissionReason> IncludeAll(IQueryable<AdmissionReason> query)
        {
            return query
              .Include(ae => ae.AdmissionReasonEducationFees)
              .ThenInclude(e => e.EducationFeeType)
              .Include(e => e.AdmissionReasonHistories)
              .ThenInclude(e => e.AdmissionReasonEducationFeeHistories)
              .Include(e => e.AdmissionReasonCitizenships)
              .ThenInclude(e => e.Country);
        }

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
            Name = Name?.Trim();
            NameAlt = NameAlt?.Trim();
            ShortName = ShortName?.Trim();
            ShortNameAlt = ShortNameAlt?.Trim();
            Description = Description?.Trim();

            if (!string.IsNullOrWhiteSpace(Name)
                && !ValidatePropertiesStatic.IsValidWithoutLatin(Name))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.AdmissionReason_NameCyrilic);
            } 

            if (!string.IsNullOrWhiteSpace(ShortName) 
                && !ValidatePropertiesStatic.IsValidWithoutLatin(ShortName))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.AdmissionReason_ShortNameCyrilic);
            }

            if (!AdmissionReasonEducationFees.Any())
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.AdmissionReason_EducationFeeTypeRequired);
            }

            if (AdmissionReasonEducationFees.Select(e => e.EducationFeeTypeId).HasDuplicate())
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.AdmissionReason_EducationFeeTypeMustBeUnique);
            }

            if (!AdmissionReasonStudentType.HasValue)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.AdmissionReason_StudentTypeRequired);
            }
        }
    }
}
