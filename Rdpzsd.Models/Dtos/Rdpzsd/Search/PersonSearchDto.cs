using Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentDoctoral;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search
{
    public class PersonSearchDto
    {
        public int Id { get; set; }
        public string Uan { get; set; }
        public LotState State { get; set; }

        public string Uin { get; set; }
        public string ForeignerNumber { get; set; }
        public string PersonIdn { get; set; }

        public string FullName { get; set; }
        public string FullNameAlt { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public GenderType Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public Country BirthCountry { get; set; }
        public Settlement BirthSettlement { get; set; }
        public Country Citizenship { get; set; }
        public Country SecondCitizenship { get; set; }
        public bool HasSpeciality { get; set; }
        public bool HasSecondary { get; set; }
        public List<PersonSemesterSearchDto> PersonSemesters { get; set; } = new List<PersonSemesterSearchDto>();

        protected virtual void SetPersonStudentsDoctorals(PersonLot personLot)
        {
            if (personLot.PersonStudents.Any())
            {
                foreach (var personStudent in personLot.PersonStudents.Where(e => e.State != PartState.Erased))
                {
                    PersonSemesters.Add(new PersonSemesterSearchDto
                    {
                        InstitutionSpeciality = personStudent.InstitutionSpeciality,
                        StudentStatus = personStudent.StudentStatus,
                        Institution = personStudent.Institution
                    });
                }
            }
            else if (personLot.PersonDoctorals.Any())
            {
                foreach (var personDoctoral in personLot.PersonDoctorals.Where(e => e.State != PartState.Erased))
                {
                    PersonSemesters.Add(new PersonSemesterSearchDto
                    {
                        InstitutionSpeciality = personDoctoral.InstitutionSpeciality,
                        StudentStatus = personDoctoral.StudentStatus,
                        Institution = personDoctoral.Institution
                    });
                }
            }
        }

        public PersonSearchDto(PersonLot personLot)
        {
            var uin = !string.IsNullOrWhiteSpace(personLot.PersonBasic.Uin)
                ? $"{personLot.PersonBasic.Uin.Remove(10 - 4, 4)}****" : string.Empty;

            var foreignerNumber = !string.IsNullOrWhiteSpace(personLot.PersonBasic.ForeignerNumber)
                ? $"{personLot.PersonBasic.ForeignerNumber.Remove(10 - 4, 4)}****" : string.Empty;

            BirthDate = personLot.PersonBasic.BirthDate;
            ForeignerNumber = foreignerNumber;
            FullName = personLot.PersonBasic.FullName;
            FullNameAlt = personLot.PersonBasic.FullNameAlt;
            Gender = personLot.PersonBasic.Gender;
            PersonIdn = personLot.PersonBasic.IdnNumber;
            Uin = uin;
            Uan = personLot.Uan;
            Id = personLot.Id;
            State = personLot.State;
            Email = personLot.PersonBasic.Email;
            PhoneNumber = personLot.PersonBasic.PhoneNumber;
            BirthCountry = personLot.PersonBasic.BirthCountry;
            BirthSettlement = personLot.PersonBasic.BirthSettlement;
            Citizenship = personLot.PersonBasic.Citizenship;
            SecondCitizenship = personLot.PersonBasic.SecondCitizenship;
            HasSecondary = personLot.PersonSecondary != null;
            HasSpeciality = personLot.PersonStudents.Any() || personLot.PersonDoctorals.Any();
           
            SetPersonStudentsDoctorals(personLot);

        }

        public PersonSearchDto(PersonStudent personStudent)
        {
            var uin = !string.IsNullOrWhiteSpace(personStudent.Lot.PersonBasic.Uin)
                ? $"{personStudent.Lot.PersonBasic.Uin.Remove(10 - 4, 4)}****" : string.Empty;

            var foreignerNumber = !string.IsNullOrWhiteSpace(personStudent.Lot.PersonBasic.ForeignerNumber)
                ? $"{personStudent.Lot.PersonBasic.ForeignerNumber.Remove(10 - 4, 4)}****" : string.Empty;

            BirthDate = personStudent.Lot.PersonBasic.BirthDate;
            ForeignerNumber = foreignerNumber;
            FullName = personStudent.Lot.PersonBasic.FullName;
            FullNameAlt = personStudent.Lot.PersonBasic.FullNameAlt;
            Gender = personStudent.Lot.PersonBasic.Gender;
            PersonIdn = personStudent.Lot.PersonBasic.IdnNumber;
            Uin = uin;
            Uan = personStudent.Lot.Uan;
            Id = personStudent.LotId;
            State = personStudent.Lot.State;
            Email = personStudent.Lot.PersonBasic.Email;
            PhoneNumber = personStudent.Lot.PersonBasic.PhoneNumber;
            BirthCountry = personStudent.Lot.PersonBasic.BirthCountry;
            BirthSettlement = personStudent.Lot.PersonBasic.BirthSettlement;
            Citizenship = personStudent.Lot.PersonBasic.Citizenship;
            SecondCitizenship = personStudent.Lot.PersonBasic.SecondCitizenship;
            HasSecondary = true;
            HasSpeciality = true;
        }
    }

    public static class PersonSearchDtoExtensions
    {
        public static IQueryable<PersonSearchDto> ToPersonSearchDto(this IQueryable<PersonLot> personLots)
        {
            return personLots.Select(e => new PersonSearchDto(e));
        }
    }
}
