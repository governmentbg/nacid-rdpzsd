using Infrastructure.Constants;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd;
using System;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search
{
    public class PersonExportDto
    {
        public string Uan { get; set; }
        public string Uin { get; set; }
        public string ForeignerNumber { get; set; }
        public string PersonIdn { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string FirstNameAlt { get; set; }
        public string MiddleNameAlt { get; set; }
        public string LastNameAlt { get; set; }
        public string OtherNameAlt { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public GenderType Gender { get; set; }
        public string BirthDate { get; set; }
        public string BirthCountryCode { get; set; }
        public string BirthSettlementCode { get; set; }
        public string BirthDistrictCode { get; set; }
        public string BirthMunicipalityCode { get; set; }
        public string ForeignerBirthSettlement { get; set; }
        public string ResidenceCountryCode { get; set; }
        public string ResidenceDistrictCode { get; set; }
        public string ResidenceMunicipalityCode { get; set; }
        public string ResidenceSettlementCode { get; set; }
        public string CitizenshipCode { get; set; }
        public string SecondCitizenshipCode { get; set; }
        public string PostCode { get; set; }
        public string ResidenceAddress { get; set; }
        public string SecondaryInfo { get; set; }
        public string GraduatedSpecialities { get; set; }

        public PersonExportDto(PersonLot personLot)
        {
            Uan = personLot.Uan;
            Uin = personLot.PersonBasic.Uin;
            PersonIdn = personLot.PersonBasic.IdnNumber;
            ForeignerNumber = personLot.PersonBasic.ForeignerNumber;
            BirthDate = personLot.PersonBasic.BirthDate.ToString("dd.MM.yyyy");
            FirstName = personLot.PersonBasic.FirstName;
            MiddleName = personLot.PersonBasic.MiddleName;
            LastName = personLot.PersonBasic.LastName;
            OtherNames = personLot.PersonBasic.OtherNames;
            FirstNameAlt = personLot.PersonBasic.FirstNameAlt;
            MiddleNameAlt = personLot.PersonBasic.MiddleNameAlt;
            LastNameAlt = personLot.PersonBasic.LastNameAlt;
            OtherNameAlt = personLot.PersonBasic.OtherNamesAlt;
            Gender = personLot.PersonBasic.Gender;
            Email = personLot.PersonBasic.Email;
            PhoneNumber = personLot.PersonBasic.PhoneNumber;
            BirthCountryCode = personLot.PersonBasic.BirthCountry?.Code;
            BirthSettlementCode = personLot.PersonBasic.BirthSettlement?.Code;
            BirthDistrictCode = personLot.PersonBasic.BirthDistrict?.Code;
            BirthMunicipalityCode = personLot.PersonBasic.BirthMunicipality?.Code;
            ResidenceCountryCode = personLot.PersonBasic.ResidenceCountry?.Code;
            ResidenceDistrictCode = personLot.PersonBasic.ResidenceDistrict?.Code;
            ResidenceMunicipalityCode = personLot.PersonBasic.ResidenceMunicipality?.Code;
            ResidenceSettlementCode = personLot.PersonBasic.ResidenceSettlement?.Code;
            ResidenceAddress = personLot.PersonBasic.ResidenceAddress;
            ForeignerBirthSettlement = personLot.PersonBasic.ForeignerBirthSettlement;
            PostCode = personLot.PersonBasic.PostCode;
            CitizenshipCode = personLot.PersonBasic.Citizenship?.Code;
            SecondCitizenshipCode = personLot.PersonBasic.SecondCitizenship?.Code;
            SecondaryInfo = personLot.PersonSecondary != null ? "Има информация" : "Липсва информация";
            GraduatedSpecialities = ConstructGraduatedSpecialities(personLot);
        }

        private string ConstructGraduatedSpecialities(PersonLot personLot)
        {
            string personGraduatedSpecialities = string.Empty;

            if (personLot.PersonStudents.Any() && personLot.PersonStudents.Any(e => e.StudentStatus.Alias == StudentStatusConstants.Graduated))
            {
                var personStudentGraduated = personLot.PersonStudents
                    .Where(e => e.StudentStatus.Alias == StudentStatusConstants.Graduated);

                personGraduatedSpecialities = string.Join(", ", personStudentGraduated.Select(e => e.InstitutionSpecialityId));
            }

            if (personLot.PersonDoctorals.Any() && personLot.PersonDoctorals.Any(e => e.StudentStatus.Alias == StudentStatusConstants.Graduated))
            {
                var personDoctoralGraduated = personLot.PersonDoctorals
                    .Where(e => e.StudentStatus.Alias == StudentStatusConstants.Graduated);

                personGraduatedSpecialities.Concat(string.Join(", ", personDoctoralGraduated.Select(e => e.InstitutionSpecialityId)));
            }

            return personGraduatedSpecialities;
        }
    }
    public static class PersonExportDtoExtensions
    {
        public static IQueryable<PersonExportDto> ToPersonExportDto(this IQueryable<PersonLot> personLots)
        {
            return personLots.Select(e => new PersonExportDto(e));
        }
    }
}
