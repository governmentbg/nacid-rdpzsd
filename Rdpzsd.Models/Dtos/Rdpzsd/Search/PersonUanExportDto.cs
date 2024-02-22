using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Rdpzsd;
using System;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search
{
    public class PersonUanExportDto
    {
        public int LotId { get; set; }
        public string MigrationIdNumber { get; set; }
        public string MigrationUniId { get; set; }
        public string Uan { get; set; }
        public string Uin { get; set; }
        public string ForeignerNumber { get; set; }
        public string PersonIdn { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FirstNameAlt { get; set; }
        public string MiddleNameAlt { get; set; }
        public string LastNameAlt { get; set; }
        public string NameInitials { get; set; }
        public string ForeignerBirthSettlement { get; set; }
        public GenderType Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public Country BirthCountry { get; set; }

        public PersonUanExportDto(PersonLot personLot, int institutionId)
        {
            LotId = personLot.Id;
            MigrationIdNumber = personLot.PersonLotIdNumbers.FirstOrDefault(e => e.InstitutionLotId == institutionId)?.MigrationIdNumber.ToString();
            MigrationUniId = personLot.PersonLotIdNumbers.FirstOrDefault(e => e.InstitutionLotId == institutionId)?.MigrationUniId.ToString();
            Uan = personLot.Uan;
            Uin = personLot.PersonBasic.Uin;
            ForeignerNumber = personLot.PersonBasic.ForeignerNumber;
            PersonIdn = personLot.PersonBasic.IdnNumber;
            BirthDate = personLot.PersonBasic.BirthDate;
            FirstName = personLot.PersonBasic.FirstName;
            MiddleName = personLot.PersonBasic.MiddleName;
            LastName = personLot.PersonBasic.LastName;
            FirstNameAlt = personLot.PersonBasic.FirstNameAlt;
            MiddleNameAlt = personLot.PersonBasic.MiddleNameAlt;
            LastNameAlt = personLot.PersonBasic.LastNameAlt;
            ForeignerBirthSettlement = personLot.PersonBasic.ForeignerBirthSettlement;
            Gender = personLot.PersonBasic.Gender;
            BirthCountry = personLot.PersonBasic.BirthCountry;
            NameInitials = personLot.PersonBasic.FirstName.Substring(0, 1) + 
                (!string.IsNullOrWhiteSpace(personLot.PersonBasic.MiddleName) ? ". " + personLot.PersonBasic.MiddleName.Substring(0, 1) : "") + 
                (!string.IsNullOrWhiteSpace(personLot.PersonBasic.LastName) ? ". " + personLot.PersonBasic.LastName.Substring(0, 1) : "") + ".";
        }
    }

    public static class PersonUanExportDtoExtensions
    {
        public static IQueryable<PersonUanExportDto> ToPersonUanExportDto(this IQueryable<PersonLot> personCommits, int institutionId)
        {
            return personCommits.Select(e => new PersonUanExportDto(e, institutionId));
        }
    }
}
