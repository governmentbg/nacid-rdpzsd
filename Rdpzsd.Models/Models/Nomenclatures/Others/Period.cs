using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures.Others
{
    public class Period : Nomenclature, IIncludeAll<Period>, IValidate
    {
        public int Year { get; set; }
        public Semester Semester { get; set; }

        public IQueryable<Period> IncludeAll(IQueryable<Period> query)
        {
            return query;
        }

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
            if (Year < 2013 || Year > 2050 || (Semester != Semester.First && Semester != Semester.Second))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.Period_NotValidPeriod);
            }

            Name = $"{Year}/{Year + 1} {(Semester == Semester.First ? "зимен" : "летен")}";
            NameAlt = $"{Year}/{Year + 1} {(Semester == Semester.First ? "winter" : "summer")}";
        }
    }
}
