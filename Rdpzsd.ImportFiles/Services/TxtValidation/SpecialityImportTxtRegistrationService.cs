using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.RdpzsdImport;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Base;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using Rdpzsd.Services.EntityServices;
using Rdpzsd.Services.Rdpzsd.Parts.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Import.Services.TxtValidation
{
    public class SpecialityImportTxtRegistrationService
    {
        private readonly RdpzsdDbContext context;
        private readonly NomenclatureDictionariesService nomenclatureDictionariesService;
        private readonly BaseHistoryPartService<PersonStudent, PersonStudentInfo, PersonStudentHistory, PersonStudentHistoryInfo, PersonLot> baseStudentHistoryPartService;
        private readonly BaseHistoryPartService<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralHistory, PersonDoctoralHistoryInfo, PersonLot> baseDoctoralHistoryPartService;
        private Dictionary<string, PersonLot> personUanLotDict;
        private Dictionary<int, InstitutionSpeciality> institutionSpecialityDict;
        private Dictionary<int, InstitutionSpeciality> doctoralProgrammeDict;

        public SpecialityImportTxtRegistrationService(
            RdpzsdDbContext context,
            NomenclatureDictionariesService nomenclatureDictionariesService,
            BaseHistoryPartService<PersonStudent, PersonStudentInfo, PersonStudentHistory, PersonStudentHistoryInfo, PersonLot> baseStudentHistoryPartService,
            BaseHistoryPartService<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralHistory, PersonDoctoralHistoryInfo, PersonLot> baseDoctoralHistoryPartService
        )
        {
            this.context = context;
            this.nomenclatureDictionariesService = nomenclatureDictionariesService;
            this.baseStudentHistoryPartService = baseStudentHistoryPartService;
            this.baseDoctoralHistoryPartService = baseDoctoralHistoryPartService;
        }

        public async Task Register(PersonStudentDoctoralTxtDto personStudentDoctoralTxtDto, int institutionId)
        {
            using var transaction = context.BeginTransaction();

            var txtPersonUans = personStudentDoctoralTxtDto.personStudentTxtDtos.Select(e => e.Uan)
                    .Concat(personStudentDoctoralTxtDto.personDoctoralTxtDtos.Select(e => e.Uan))
                .Distinct()
                .ToList();

            // If PersonSpeciality or PersonDoctoral is not found from actualPersonStudentsDict/actualPersonDoctoralsDict use this to get personLotId and create PersonSpeciality
            personUanLotDict = nomenclatureDictionariesService.GetPersonUanLotDict(txtPersonUans);

            // Get all institutionSpecialities by institutionId (so we can get InstitutionId and SubordinateId by institutionSpecialityId)
            (institutionSpecialityDict, doctoralProgrammeDict) = nomenclatureDictionariesService.GetInstitutionSpecialitiesDict(institutionId, false);

            if (personStudentDoctoralTxtDto.personStudentTxtDtos.Any())
            {
                // Use this to find PersonSpeciality to which to add/edit/erase semester
                var actualPersonStudentsDict = new PersonStudent().IncludeAll(context.Set<PersonStudent>().AsQueryable())
                    .Include(e => e.Lot)
                    .Include(e => e.PartInfo)
                    .Where(e => e.State != PartState.Erased && txtPersonUans.Contains(e.Lot.Uan))
                    .OrderByDescending(e => e.Id)
                    .AsEnumerable()
                    .GroupBy(e => new { e.Lot.Uan, e.InstitutionSpecialityId })
                    .ToDictionary(e => (e.Key.Uan, e.Key.InstitutionSpecialityId), e => e.First());

                var personStudentsForAdd = new List<PersonStudent>();
                var personStudentSemestersForAdd = new List<PersonStudentSemester>();

                foreach (var personStudentTxtDto in personStudentDoctoralTxtDto.personStudentTxtDtos)
                {
                    var actualPersonStudent = actualPersonStudentsDict.GetValueOrDefault((personStudentTxtDto.Uan, personStudentTxtDto.InstitutionSpecialityId));

                    if (actualPersonStudent != null)
                    {
                        var lastSemester = actualPersonStudent.Semesters
                                .OrderByDescending(e => e.Period.Year)
                                .ThenByDescending(e => e.Period.Semester)
                                .ThenByDescending(e => e.Id)
                                .FirstOrDefault();

                        if (personStudentTxtDto.ActionState == SpecialityImportAction.Add)
                        {
                            await CreateStudentHistoryAndUpdatePartInfo(actualPersonStudent, personStudentTxtDto, PersonLotActionType.PersonStudentSemesterAddTxt);
                            UpdatePersonStudentStatusEvent<PersonStudent, PersonStudentInfo, PersonStudentHistory, PersonStudentHistoryInfo, PersonStudentSemester, PersonStudentTxtDto>(actualPersonStudent, personStudentTxtDto);
                            personStudentSemestersForAdd.Add(personStudentTxtDto.ToPersonStudentSemester(actualPersonStudent.Id, lastSemester));
                        }
                        else if (personStudentTxtDto.ActionState == SpecialityImportAction.Edit)
                        {
                            // ToDo
                            continue;
                        }
                        else if (personStudentTxtDto.ActionState == SpecialityImportAction.Erase)
                        {
                            if (actualPersonStudent.Semesters.Count > 1)
                            {
                                if (personStudentTxtDto.PeriodId == lastSemester.PeriodId
                                    && personStudentTxtDto.StudentEventId == lastSemester.StudentEventId
                                    && personStudentTxtDto.Course == lastSemester.Course
                                    && personStudentTxtDto.StudentSemester == lastSemester.StudentSemester)
                                {
                                    await CreateStudentHistoryAndUpdatePartInfo(actualPersonStudent, personStudentTxtDto, PersonLotActionType.PersonStudentSemesterEraseTxt);

                                    var oldSemester = actualPersonStudent.Semesters
                                        .OrderByDescending(e => e.Period.Year)
                                        .ThenByDescending(e => e.Period.Semester)
                                        .ThenByDescending(e => e.Id)
                                        .FirstOrDefault(e => e.Id != lastSemester.Id);

                                    actualPersonStudent.StudentStatusId = oldSemester.StudentStatusId;
                                    actualPersonStudent.StudentEventId = oldSemester.StudentEventId;

                                    EntityService.Remove(lastSemester, context);
                                }
                            }
                            else
                            {
                                await CreateStudentHistoryAndUpdatePartInfo(actualPersonStudent, personStudentTxtDto, PersonLotActionType.PersonStudentEraseTxt);
                                actualPersonStudent.State = PartState.Erased;
                            }
                        }
                    }
                    else if (personStudentTxtDto.ActionState == SpecialityImportAction.Add)
                    {
                        var personLot = personUanLotDict[personStudentTxtDto.Uan];

                        var institution = institutionSpecialityDict.GetValueOrDefault(personStudentTxtDto.InstitutionSpecialityId).Institution;
                        await CreatePersonLotAction(personLot.Id, personStudentTxtDto, PersonLotActionType.PersonStudentAddTxt);

                        int? relocatedFromPartId = personStudentTxtDto.RelocatedFromInstitutionSpecialityId.HasValue
                            ? personLot.PersonStudents.FirstOrDefault(e => e.StudentEventId == 304
                                && e.InstitutionSpecialityId == personStudentTxtDto.RelocatedFromInstitutionSpecialityId
                                && e.State != PartState.Erased)?.Id
                            : null;

                        personStudentsForAdd.Add(personStudentTxtDto.ToPersonStudent(personLot.Id, institution.RootId, institution.ParentId.HasValue ? institution.Id : null, relocatedFromPartId));
                    }
                }

                context.PersonStudentSemesters.AddRange(personStudentSemestersForAdd);
                context.PersonStudents.AddRange(personStudentsForAdd);
            }

            if (personStudentDoctoralTxtDto.personDoctoralTxtDtos.Any())
            {
                // Use this to find PersonDoctoral to which to add/edit/erase semester
                var actualPersonDoctoralsDict = new PersonDoctoral().IncludeAll(context.Set<PersonDoctoral>().AsQueryable())
                    .Include(e => e.Lot)
                    .Include(e => e.PartInfo)
                    .Where(e => e.State != PartState.Erased && txtPersonUans.Contains(e.Lot.Uan))
                    .OrderByDescending(e => e.Id)
                    .AsEnumerable()
                    .GroupBy(e => new { e.Lot.Uan, e.InstitutionSpecialityId })
                    .ToDictionary(e => (e.Key.Uan, e.Key.InstitutionSpecialityId), e => e.First());

                var personDoctoralsForAdd = new List<PersonDoctoral>();
                var personDoctoralSemestersForAdd = new List<PersonDoctoralSemester>();

                foreach (var personDoctoralTxtDto in personStudentDoctoralTxtDto.personDoctoralTxtDtos)
                {
                    var actualPersonDoctoral = actualPersonDoctoralsDict.GetValueOrDefault((personDoctoralTxtDto.Uan, personDoctoralTxtDto.DoctoralProgrammeId));

                    if (actualPersonDoctoral != null)
                    {
                        var lastSemester = actualPersonDoctoral.Semesters
                                .OrderByDescending(e => e.ProtocolDate.Date)
                                .ThenByDescending(e => e.Id)
                                .FirstOrDefault();

                        if (personDoctoralTxtDto.ActionState == SpecialityImportAction.Add)
                        {
                            await CreateDoctoralHistoryAndUpdatePartInfo(actualPersonDoctoral, personDoctoralTxtDto, PersonLotActionType.PersonDoctoralSemesterAddTxt);
                            UpdatePersonStudentStatusEvent<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralHistory, PersonDoctoralHistoryInfo, PersonDoctoralSemester, PersonDoctoralTxtDto>(actualPersonDoctoral, personDoctoralTxtDto);
                            personDoctoralSemestersForAdd.Add(personDoctoralTxtDto.ToPersonDoctoralSemester(actualPersonDoctoral.Id, lastSemester));
                        }
                        else if (personDoctoralTxtDto.ActionState == SpecialityImportAction.Edit)
                        {
                            // ToDo
                            continue;
                        }
                        else if (personDoctoralTxtDto.ActionState == SpecialityImportAction.Erase)
                        {
                            if (actualPersonDoctoral.Semesters.Count > 1)
                            {
                                if (personDoctoralTxtDto.ProtocolNumber == lastSemester.ProtocolNumber
                                    && personDoctoralTxtDto.StudentEventId == lastSemester.StudentEventId
                                    && personDoctoralTxtDto.ProtocolDate.Date == lastSemester.ProtocolDate.Date
                                    && personDoctoralTxtDto.YearType == lastSemester.YearType)
                                {
                                    await CreateDoctoralHistoryAndUpdatePartInfo(actualPersonDoctoral, personDoctoralTxtDto, PersonLotActionType.PersonDoctoralSemesterEraseTxt);

                                    var oldSemester = actualPersonDoctoral.Semesters
                                        .OrderByDescending(e => e.ProtocolDate.Date)
                                        .ThenByDescending(e => e.Id)
                                        .FirstOrDefault(e => e.Id != lastSemester.Id);

                                    actualPersonDoctoral.StudentStatusId = oldSemester.StudentStatusId;
                                    actualPersonDoctoral.StudentEventId = oldSemester.StudentEventId;

                                    EntityService.Remove(lastSemester, context);
                                }
                            }
                            else
                            {
                                await CreateDoctoralHistoryAndUpdatePartInfo(actualPersonDoctoral, personDoctoralTxtDto, PersonLotActionType.PersonStudentEraseTxt);
                                actualPersonDoctoral.State = PartState.Erased;
                            }
                        }
                    }
                    else if (personDoctoralTxtDto.ActionState == SpecialityImportAction.Add)
                    {
                        var personLot = personUanLotDict[personDoctoralTxtDto.Uan];

                        var institution = doctoralProgrammeDict.GetValueOrDefault(personDoctoralTxtDto.DoctoralProgrammeId).Institution;
                        await CreatePersonLotAction(personLot.Id, personDoctoralTxtDto, PersonLotActionType.PersonDoctoralAddTxt);

                        int? relocatedFromPartId = personDoctoralTxtDto.RelocatedFromInstitutionSpecialityId.HasValue
                            ? personLot.PersonDoctorals.FirstOrDefault(e => e.StudentEventId == 304
                                && e.InstitutionSpecialityId == personDoctoralTxtDto.RelocatedFromInstitutionSpecialityId
                                && e.State != PartState.Erased)?.Id
                            : null;

                        personDoctoralsForAdd.Add(personDoctoralTxtDto.ToPersonDoctoral(personLot.Id, institution.RootId, institution.ParentId.HasValue ? institution.Id : null, relocatedFromPartId));
                    }
                }

                context.PersonDoctoralSemesters.AddRange(personDoctoralSemestersForAdd);
                context.PersonDoctorals.AddRange(personDoctoralsForAdd);
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        private async Task CreateStudentHistoryAndUpdatePartInfo(PersonStudent personStudent, PersonStudentTxtDto personStudentTxtDto, PersonLotActionType personLotActionType)
        {
            await baseStudentHistoryPartService.CreateHistory(personStudent, context);
            await BaseHistoryAndUpdatePartInfo<PersonStudent, PersonStudentInfo, PersonStudentHistory, PersonStudentHistoryInfo, PersonStudentTxtDto>(personStudent, personStudentTxtDto, personLotActionType);
        }

        private async Task CreateDoctoralHistoryAndUpdatePartInfo(PersonDoctoral personDoctoral, PersonDoctoralTxtDto personDoctoralTxtDto, PersonLotActionType personLotActionType)
        {
            await baseDoctoralHistoryPartService.CreateHistory(personDoctoral, context);
            await BaseHistoryAndUpdatePartInfo<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralHistory, PersonDoctoralHistoryInfo, PersonDoctoralTxtDto>(personDoctoral, personDoctoralTxtDto, personLotActionType);
        }

        private async Task BaseHistoryAndUpdatePartInfo<TPart, TPartInfo, THistory, THistoryInfo, TPersonStudentDoctoralTxtDto>(TPart personStudentDoctoral, TPersonStudentDoctoralTxtDto personStudentTxtDto, PersonLotActionType personLotActionType)
            where TPart : Part<TPartInfo>, IMultiPart<TPart, PersonLot, THistory>, new()
            where TPartInfo : PartInfo, new()
            where THistory : Part<THistoryInfo>, IHistoryPart<THistory>, new()
            where THistoryInfo : PartInfo, new()
            where TPersonStudentDoctoralTxtDto : BasePersonStudentDoctoralTxtDto
        {
            personStudentDoctoral.PartInfo.ActionDate = personStudentTxtDto.CreateDate;
            personStudentDoctoral.PartInfo.UserFullname = personStudentTxtDto.UserFullname;
            personStudentDoctoral.PartInfo.UserId = personStudentTxtDto.UserId;
            personStudentDoctoral.PartInfo.InstitutionId = personStudentTxtDto.CreateInstitutionId;
            personStudentDoctoral.PartInfo.SubordinateId = personStudentTxtDto.CreateSubordinateId;

            await CreatePersonLotAction(personStudentDoctoral.LotId, personStudentTxtDto, personLotActionType);
        }

        private async Task CreatePersonLotAction<TPersonStudentDoctoralTxtDto>(int lotId, TPersonStudentDoctoralTxtDto personStudentDoctoralTxtDto, PersonLotActionType personLotActionType)
            where TPersonStudentDoctoralTxtDto : BasePersonStudentDoctoralTxtDto
        {
            var newPersonLotAction = new PersonLotAction
            {
                ActionDate = personStudentDoctoralTxtDto.CreateDate,
                ActionType = personLotActionType,
                InstitutionId = personStudentDoctoralTxtDto.CreateInstitutionId,
                LotId = lotId,
                SubordinateId = personStudentDoctoralTxtDto.CreateSubordinateId,
                UserFullname = personStudentDoctoralTxtDto.UserFullname,
                UserId = personStudentDoctoralTxtDto.UserId
            };
            await context.PersonLotActions.AddAsync(newPersonLotAction);
        }

        private void UpdatePersonStudentStatusEvent<TPart, TPartInfo, THistory, THistoryInfo, TSemester, TPersonStudentDoctoralTxtDto>(TPart personStudentDoctoral, TPersonStudentDoctoralTxtDto personStudentDoctoralTxtDto)
            where TPart : BasePersonStudentDoctoral<TPartInfo, TSemester>, IMultiPart<TPart, PersonLot, THistory>, new()
            where TPartInfo : PartInfo, new()
            where THistory : Part<THistoryInfo>, IHistoryPart<THistory>, new()
            where THistoryInfo : PartInfo, new()
            where TSemester : BasePersonSemester, new()
            where TPersonStudentDoctoralTxtDto : BasePersonStudentDoctoralTxtDto
        {
            personStudentDoctoral.StudentStatusId = personStudentDoctoralTxtDto.StudentStatusId;
            personStudentDoctoral.StudentEventId = personStudentDoctoralTxtDto.StudentEventId;
        }
    }
}
