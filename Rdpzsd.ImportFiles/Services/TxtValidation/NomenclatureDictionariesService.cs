using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Nomenclature.AdmissionReason;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Import.Services.TxtValidation
{
    public class NomenclatureDictionariesService
    {
        private readonly RdpzsdDbContext context;

        public NomenclatureDictionariesService(RdpzsdDbContext context)
        {
            this.context = context;
        }

        public Dictionary<(string, DateTime), PersonLot> GetPersonLotStudentByUansDict(List<string> uans)
        {
            var personLotStudentDict = context.PersonLots
                .AsNoTracking()
                .Include(e => e.PersonBasic.Citizenship)
                .Include(e => e.PersonBasic.SecondCitizenship)
                .Include(e => e.PersonStudents)
                    .ThenInclude(s => s.InstitutionSpeciality.Speciality)
                .Include(e => e.PersonStudents)
                    .ThenInclude(s => s.StudentStatus)
                .Include(e => e.PersonStudents)
                    .ThenInclude(s => s.Semesters)
                        .ThenInclude(m => m.StudentStatus)
                .Include(e => e.PersonStudents)
                    .ThenInclude(s => s.Semesters)
                        .ThenInclude(m => m.StudentEvent)
                .Include(e => e.PersonStudents)
                    .ThenInclude(s => s.Semesters)
                        .ThenInclude(m => m.Period)
                .Include(e => e.PersonDoctorals)
                    .ThenInclude(s => s.StudentStatus)
                .Include(e => e.PersonDoctorals)
                    .ThenInclude(s => s.Semesters)
                        .ThenInclude(m => m.StudentStatus)
                .Where(e => uans.Contains(e.Uan) && e.State == LotState.Actual)
                .ToDictionary(e => (e.Uan, e.PersonBasic.BirthDate.Date), e => e);

            return personLotStudentDict;
        }

        public (Dictionary<int, InstitutionSpeciality>, Dictionary<int, InstitutionSpeciality>) GetInstitutionSpecialitiesDict(int institutionId, bool isSubordinate)
        {
            var allInstitutionSpecialityDict = context.InstitutionSpecialities
                .AsNoTracking()
                .Include(e => e.Institution)
                .Include(e => e.Speciality.EducationalQualification)
                .Include(e => e.InstitutionSpecialityJointSpecialities)
                .Where(e => e.IsActive
                    && (isSubordinate ? e.InstitutionId == institutionId : e.Institution.RootId == institutionId));

            var institutionSpecialityDict = allInstitutionSpecialityDict
                .Where(e => e.Speciality.EducationalQualification.Alias != EducationalQualificationConstants.Doctor)
                .ToDictionary(e => e.Id, e => e);

            var doctoralProgrammeDict = allInstitutionSpecialityDict
                .Where(e => e.Speciality.EducationalQualification.Alias == EducationalQualificationConstants.Doctor)
                .ToDictionary(e => e.Id, e => e);

            return (institutionSpecialityDict, doctoralProgrammeDict);
        }

        public (HashSet<int>, HashSet<int>) GetInstitutionSpecialitiesHashSet(int institutionId, bool isSubordinate)
        {
            var allInstitutionSpeciality = context.InstitutionSpecialities
                .AsNoTracking()
                .Include(e => e.Institution)
                .Include(e => e.Speciality.EducationalQualification)
                .Where(e => e.IsActive
                    && (isSubordinate ? e.InstitutionId == institutionId : e.Institution.RootId == institutionId));

            var institutionSpecialityHashSet = allInstitutionSpeciality
                .Where(e => e.Speciality.EducationalQualification.Alias != EducationalQualificationConstants.Doctor)
                .Select(e => e.Id)
                .ToHashSet();

            var doctoralProgrammeDict = allInstitutionSpeciality
                .Where(e => e.Speciality.EducationalQualification.Alias == EducationalQualificationConstants.Doctor)
                .Select(e => e.Id)
                .ToHashSet();

            return (institutionSpecialityHashSet, doctoralProgrammeDict);
        }

        public HashSet<(int, Semester)> GetPeriodHashSet()
        {
            var periodHashSet = context.Periods
                .AsNoTracking()
                .Where(e => e.IsActive)
                .Select(e => new Tuple<int, Semester>(e.Year, e.Semester).ToValueTuple())
                .ToHashSet();

            return periodHashSet;
        }

        public Dictionary<(int, Semester), int> GetPeriodIdDict()
        {
            var periodDict = context.Periods
                .AsNoTracking()
                .Where(e => e.IsActive)
                .ToDictionary(e => new Tuple<int, Semester>(e.Year, e.Semester).ToValueTuple(), e => e.Id);

            return periodDict;
        }

        public (HashSet<int>, HashSet<int>) GetStudentEventHashSet()
        {
            var eventHashSet = context.StudentEvents
                .AsNoTracking()
                .Include(e => e.StudentStatus)
                .Include(e => e.StudentEventQualifications)
                    .ThenInclude(s => s.EducationalQualification)
                .Where(e => e.IsActive && e.StudentStatus.Alias != StudentStatusConstants.Graduated);

            var studentEventHashSet = eventHashSet
                .Where(e => e.StudentEventQualifications
                        .Any(e => e.EducationalQualification.Alias == EducationalQualificationConstants.Bachelor
                            || e.EducationalQualification.Alias == EducationalQualificationConstants.ProfessionalBachelor
                            || e.EducationalQualification.Alias == EducationalQualificationConstants.MasterSecondary
                            || e.EducationalQualification.Alias == EducationalQualificationConstants.MasterHigh))
                .Select(e => e.Id)
                .ToHashSet();

            var doctoralEventHashSet = eventHashSet
                .Where(e => e.StudentEventQualifications
                        .Any(e => e.EducationalQualification.Alias == EducationalQualificationConstants.Doctor))
                .Select(e => e.Id)
                .ToHashSet();

            return (studentEventHashSet, doctoralEventHashSet);
        }

        public Dictionary<int, int> GetStudentStatusByEventDict()
        {
            var studentEventStatusDict = context.StudentEvents
                .AsNoTracking()
                .Where(e => e.IsActive
                    && e.StudentStatus.Alias != StudentStatusConstants.Graduated)
                .ToDictionary(e => e.Id, e => e.StudentStatusId);

            return studentEventStatusDict;
        }

        public (Dictionary<int, AdmissionReason>, Dictionary<int, AdmissionReason>) GetAdmissionReasonDict()
        {
            var admissionReasonDict = context.AdmissionReasons
                .AsNoTracking()
                .Include(e => e.AdmissionReasonCitizenships)
                .Include(e => e.AdmissionReasonEducationFees)
                .Where(e => e.IsActive);

            var studentAdmissionReasonDict = admissionReasonDict
                .Where(e => e.AdmissionReasonStudentType == AdmissionReasonStudentType.Students || e.AdmissionReasonStudentType == AdmissionReasonStudentType.StudentsAndDoctorals)
                .ToDictionary(e => e.Id, e => e);

            var doctoralAdmissionReasonDict = admissionReasonDict
                .Where(e => e.AdmissionReasonStudentType == AdmissionReasonStudentType.Doctorals || e.AdmissionReasonStudentType == AdmissionReasonStudentType.StudentsAndDoctorals)
                .ToDictionary(e => e.Id, e => e);

            return (studentAdmissionReasonDict, doctoralAdmissionReasonDict);
        }

        public HashSet<int> GetEducationalFeeHashSet()
        {
            var educationalFeeHashSet = context.EducationFeeTypes
                .AsNoTracking()
                .Where(e => e.IsActive)
                .Select(e => e.Id)
                .ToHashSet();

            return educationalFeeHashSet;
        }

        public HashSet<string> GetResearchAreaCodeHashSet()
        {
            var researchAreaCodeHashSet = context.ResearchAreas
                .AsNoTracking()
                .Where(e => e.IsActive && e.Level != Level.First)
                .Select(e => e.Code)
                .ToHashSet();

            return researchAreaCodeHashSet;
        }

        public Dictionary<string, int?> GetResearchAreaByCodeDict()
        {
            var researchAreaCodeDict = context.ResearchAreas
                .AsNoTracking()
                .Where(e => e.IsActive && e.Level != Level.First)
                .ToDictionary(e => e.Code, e => e?.Id);

            return researchAreaCodeDict;
        }

        public HashSet<(string, string, string)> GetPersonIdnBirthPlace()
        {
            var personBirthPlaceHashSet = context.PersonBasics
                .AsNoTracking()
                .Where(e => !string.IsNullOrWhiteSpace(e.IdnNumber))
                .Include(e => e.BirthCountry)
                .Select(e => new Tuple<string, string, string>(e.BirthCountry.Code, e.BirthDate.ToString("dd.MM.yyyy"), e.FirstName.ToLower()).ToValueTuple())
                .ToHashSet();

            return personBirthPlaceHashSet;
        }

        public HashSet<string> GetPersonUanHashSetAsync()
        {
            var personUanHashSet = context.PersonLots
                .AsNoTracking()
                .Select(e => e.Uan)
                .ToHashSet();

            return personUanHashSet;
        }

        public Dictionary<string, PersonLot> GetPersonUanLotDict(List<string> uans)
        {
            var personUanLotDict = context.PersonLots
                .AsNoTracking()
                .Include(e => e.PersonStudents)
                .Where(e => e.State != LotState.Erased && (!uans.Any() || uans.Contains(e.Uan)))
                .ToDictionary(e => e.Uan, e => e);

            return personUanLotDict;
        }

        public Tuple<Dictionary<string, string>, Dictionary<string, string>, Dictionary<string, PersonBasic>> GetPersonIdentifierDicts()
        {
            var personLot = context.PersonLots
                .AsNoTracking()
                .Include(e => e.PersonBasic);

            var personUinUanDict = personLot
                .Where(e => e.PersonBasic.Uin != null)
                .ToDictionary(e => e.PersonBasic.Uin, e => e.Uan);

            var personForeignerNumUanDict = personLot
                .Where(e => e.PersonBasic.ForeignerNumber != null)
                .ToDictionary(e => e.PersonBasic.ForeignerNumber, e => e.Uan);

            var personIdentifierUanDict = personLot
               .Where(e => e.PersonBasic.Uin != null || e.PersonBasic.ForeignerNumber != null)
               .ToDictionary(e => e.Uan, e => e.PersonBasic);

            return new Tuple<Dictionary<string, string>, Dictionary<string, string>, Dictionary<string, PersonBasic>>(personUinUanDict, personForeignerNumUanDict, personIdentifierUanDict);
        }

        public Dictionary<string, string> GetPersonIdnEmailDict()
        {
            var personIdnEmailDict = context.PersonBasics
                .AsNoTracking()
                .Include(e => e.Lot)
                .Where(e => !string.IsNullOrWhiteSpace(e.IdnNumber)
                          && string.IsNullOrWhiteSpace(e.Uin)
                          && string.IsNullOrWhiteSpace(e.ForeignerNumber)
                          && e.Email != EmailConstants.NoEmail)
                .ToDictionary(e => e.Email.ToLower(), e => e.Lot.Uan);

            return personIdnEmailDict;
        }

        public Dictionary<string, string> GetPersonEmailDict()
        {
            var personEmailDict = context.PersonBasics
                .AsNoTracking()
                .Where(e => e.Email != EmailConstants.NoEmail && (e.Uin != null || e.ForeignerNumber != null))
                .ToDictionary(e => e.Uin ?? e.ForeignerNumber, e => e.Email.ToLower());

            return personEmailDict;
        }

        public Dictionary<string, int?> GetCountryDictAsync()
        {
            var countriesDict = context.Countries
                .AsNoTracking()
                .ToDictionary(e => e.Code, e => e?.Id);

            return countriesDict;
        }

        public Dictionary<string, Settlement> GetSettlementDictAsync()
        {
            var settlementsDict = context.Settlements
                .AsNoTracking()
                .ToDictionary(e => e.Code, e => e);

            return settlementsDict;
        }

        public Dictionary<string, int?> GetDistrictDictAsync()
        {
            var districtsDict = context.Districts
                .AsNoTracking()
                .ToDictionary(e => e.Code, e => e?.Id);

            return districtsDict;
        }

        public Dictionary<string, int?> GetMunicipalityDictAsync()
        {
            var municipalitiesDict = context.Municipalities
                .AsNoTracking()
                .ToDictionary(e => e.Code, e => e?.Id);

            return municipalitiesDict;
        }

        public HashSet<string> GetCountryCodes()
        {
            var countryCodes = context.Countries
                .AsNoTracking()
                .Select(e => e.Code)
                .ToHashSet();

            return countryCodes;
        }

        public HashSet<string> GetSettlementCodes()
        {
            var settlementCodes = context.Settlements
                .AsNoTracking()
                .Select(e => e.Code)
                .ToHashSet();

            return settlementCodes;
        }

        public HashSet<string> GetDistrictCodes()
        {
            var districtCodes = context.Districts
                .AsNoTracking()
                .Select(e => e.Code)
                .ToHashSet();

            return districtCodes;
        }

        public HashSet<string> GetMunicipalityCodes()
        {
            var municipalityCodes = context.Municipalities
                .AsNoTracking()
                .Select(e => e.Code)
                .ToHashSet();

            return municipalityCodes;
        }

        public Dictionary<int, School> GetScoolDictByMigrationId()
        {
            var schoolsDict = context.Schools
                .AsNoTracking()
                .Where(e => e.MigrationId.HasValue)
                .ToDictionary(e => e.MigrationId.Value, e => e);

            return schoolsDict;
        }
    }
}
