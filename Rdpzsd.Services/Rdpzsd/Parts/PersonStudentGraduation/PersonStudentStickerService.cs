using Infrastructure.Constants;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Rdpzsd.Parts.PersonStudentSticker;
using Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentSticker;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Parts.PersonStudentGraduation
{
    public class PersonStudentStickerService
    {
        private readonly RdpzsdDbContext context;
        private readonly UserContext userContext;
        private readonly PersonLotService personLotService;
        private readonly PersonSecondaryService personSecondaryService;

        public PersonStudentStickerService(
            RdpzsdDbContext context,
            UserContext userContext,
            PersonLotService personLotService,
            PersonSecondaryService personSecondaryService
        )
        {
            this.context = context;
            this.userContext = userContext;
            this.personLotService = personLotService;
            this.personSecondaryService = personSecondaryService;
        }

        public async Task<StudentStickerState> SendForSticker(StickerDto stickerDto, PersonStudent actualPart)
        {
            var stickerErrorDto = await ValidatePersonStudentStickerInfo(actualPart);

            context.Entry(actualPart).State = EntityState.Modified;
            actualPart.StickerYear = stickerDto.StickerYear;
            actualPart.StickerState = stickerErrorDto.HasError ? StudentStickerState.SendForStickerDiscrepancy : StudentStickerState.SendForSticker;

            var actionDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(stickerDto.Note))
            {
                var stickerNote = new PersonStudentStickerNote
                {
                    ActionDate = actionDate,
                    InstitutionId = userContext.UserType == UserType.Rsd ? userContext.Institution.Id : null,
                    SubordinateId = userContext.UserType == UserType.Rsd && userContext.Institution.ChildInstitutions.Count == 1
                    ? userContext.Institution.ChildInstitutions.First().Id
                    : null,
                    PartId = actualPart.Id,
                    Note = stickerDto.Note,
                    UserId = userContext.UserId.Value,
                    UserFullname = userContext.UserFullname
                };

                await context.PersonStudentStickerNotes.AddAsync(stickerNote);
            }

            var personLotActionType = stickerErrorDto.HasError ? PersonLotActionType.SendForStickerDiscrepancy : PersonLotActionType.SendForSticker;

            await personLotService
                .AddPersonLotAction(actualPart.LotId, actionDate, personLotActionType, $"специалност: {actualPart?.InstitutionSpeciality?.Speciality?.Code}");

            await context.SaveChangesAsync();

            return actualPart.StickerState;
        }

        public async Task<StudentStickerState> ReturnForEdit(StickerDto stickerDto, PersonStudent actualPart)
        {
            context.Entry(actualPart).State = EntityState.Modified;
            actualPart.StickerState = StudentStickerState.ReturnedForEdit;

            var actionDate = DateTime.Now;

            var stickerNote = new PersonStudentStickerNote
            {
                ActionDate = actionDate,
                InstitutionId = null,
                SubordinateId = null,
                PartId = actualPart.Id,
                Note = stickerDto.Note,
                UserId = userContext.UserId.Value,
                UserFullname = userContext.UserFullname
            };

            await context.PersonStudentStickerNotes.AddAsync(stickerNote);

            await personLotService
                .AddPersonLotAction(actualPart.LotId, actionDate, PersonLotActionType.ReturnedForEditSticker, $"специалност: {actualPart?.InstitutionSpeciality?.Speciality?.Code}");

            await context.SaveChangesAsync();

            return actualPart.StickerState;
        }

        public async Task MarkedForPrint(List<int> personStudentIds)
        {
            var parts = await new PersonStudent().IncludeAll(context.Set<PersonStudent>().AsQueryable())
                        .Include(e => e.PartInfo)
                        .Where(e => (e.StickerState == StudentStickerState.SendForSticker
                                || e.StickerState == StudentStickerState.ReissueSticker
                                || e.DuplicateDiplomas.Any(e => e.DuplicateStickerState == StudentStickerState.SendForSticker || e.DuplicateStickerState == StudentStickerState.ReissueSticker))
                            && personStudentIds.Contains(e.Id))
                        .ToListAsync();

            var actionDate = DateTime.Now;

            foreach (var part in parts)
            {
                if (part.StickerState == StudentStickerState.SendForSticker || part.StickerState == StudentStickerState.ReissueSticker)
                {
                    await personLotService
                        .AddPersonLotAction(part.LotId, actionDate, PersonLotActionType.StickerForPrint, $"специалност: {part?.InstitutionSpeciality?.Speciality?.Code}");

                    part.StickerState = StudentStickerState.StickerForPrint;
                    part.StudentEventId = StudentEventConstants.GraduatedWithoutDiplomaId;
                    part.StudentStatusId = StudentStatusConstants.GraduatedId;
                }
                else
                {
                    var duplicateDiploma = part.DuplicateDiplomas.FirstOrDefault(e => e.DuplicateStickerState == StudentStickerState.SendForSticker || e.DuplicateStickerState == StudentStickerState.ReissueSticker);

                    if (duplicateDiploma != null)
                    {
                        await personLotService
                            .AddPersonLotAction(part.LotId, actionDate, PersonLotActionType.StickerForPrintDuplicate, $"специалност: {part?.InstitutionSpeciality?.Speciality?.Code}, година на стикера: {duplicateDiploma.DuplicateStickerYear}");

                        duplicateDiploma.DuplicateStickerState = StudentStickerState.StickerForPrint;
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task MarkedRecieved(List<int> personStudentIds)
        {
            var parts = await new PersonStudent().IncludeAll(context.Set<PersonStudent>().AsQueryable())
                        .Include(e => e.PartInfo)
                        .Where(e => (e.StickerState == StudentStickerState.StickerForPrint
                                || e.DuplicateDiplomas.Any(e => e.DuplicateStickerState == StudentStickerState.StickerForPrint))
                            && personStudentIds.Contains(e.Id))
                        .ToListAsync();

            var actionDate = DateTime.Now;

            foreach (var part in parts)
            {
                if (part.StickerState == StudentStickerState.StickerForPrint)
                {
                    await personLotService
                        .AddPersonLotAction(part.LotId, actionDate, PersonLotActionType.RecievedSticker, $"специалност: {part?.InstitutionSpeciality?.Speciality?.Code}");

                    part.StickerState = StudentStickerState.Recieved;
                }
                else
                {
                    var duplicateDiploma = part.DuplicateDiplomas.FirstOrDefault(e => e.DuplicateStickerState == StudentStickerState.StickerForPrint);

                    if (duplicateDiploma != null)
                    {
                        await personLotService
                        .AddPersonLotAction(part.LotId, actionDate, PersonLotActionType.RecievedDuplicateSticker, $"специалност: {part?.InstitutionSpeciality?.Speciality?.Code}, година на стикера: {duplicateDiploma.DuplicateStickerYear}");

                        duplicateDiploma.DuplicateStickerState = StudentStickerState.Recieved;
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task<PersonStudentStickerSearchDto> ForPrint(PersonStudent actualPart)
        {
            var actionDate = DateTime.Now;

            await personLotService
                .AddPersonLotAction(actualPart.LotId, actionDate, PersonLotActionType.StickerForPrint, $"специалност: {actualPart?.InstitutionSpeciality?.Speciality?.Code}");

            context.Entry(actualPart).State = EntityState.Modified;
            actualPart.StickerState = StudentStickerState.StickerForPrint;
            actualPart.StudentEventId = StudentEventConstants.GraduatedWithoutDiplomaId;
            actualPart.StudentStatusId = StudentStatusConstants.GraduatedId;

            await context.SaveChangesAsync();

            return await ReturnStudentStickerSearchDtoById(actualPart.Id);
        }

        public async Task<StudentStickerState> ForPrintDuplicate(PersonStudentDuplicateDiploma duplicateDiploma, PersonStudent actualPart)
        {
            var actionDate = DateTime.Now;

            await personLotService
                .AddPersonLotAction(actualPart.LotId, actionDate, PersonLotActionType.StickerForPrintDuplicate, $"специалност: {actualPart?.InstitutionSpeciality?.Speciality?.Code}, година на стикера: {duplicateDiploma.DuplicateStickerYear}");

            context.Entry(duplicateDiploma).State = EntityState.Modified;
            duplicateDiploma.DuplicateStickerState = StudentStickerState.StickerForPrint;

            await context.SaveChangesAsync();

            return duplicateDiploma.DuplicateStickerState;
        }

        public async Task<StudentStickerState> Recieved(PersonStudent actualPart)
        {
            var actionDate = DateTime.Now;

            await personLotService
                .AddPersonLotAction(actualPart.LotId, actionDate, PersonLotActionType.RecievedSticker, $"специалност: {actualPart?.InstitutionSpeciality?.Speciality?.Code}");

            context.Entry(actualPart).State = EntityState.Modified;
            actualPart.StickerState = StudentStickerState.Recieved;

            await context.SaveChangesAsync();

            return actualPart.StickerState;
        }

        public async Task<StudentStickerState> RecievedDuplicate(PersonStudentDuplicateDiploma duplicateDiploma, PersonStudent actualPart)
        {
            var actionDate = DateTime.Now;

            await personLotService
                .AddPersonLotAction(actualPart.LotId, actionDate, PersonLotActionType.RecievedDuplicateSticker, $"специалност: {actualPart?.InstitutionSpeciality?.Speciality?.Code}, година на стикера: {duplicateDiploma.DuplicateStickerYear}");

            context.Entry(duplicateDiploma).State = EntityState.Modified;
            duplicateDiploma.DuplicateStickerState = StudentStickerState.Recieved;

            await context.SaveChangesAsync();

            return duplicateDiploma.DuplicateStickerState;
        }

        public async Task<StudentStickerState> ReissueSticker(StickerDto stickerDto, PersonStudent actualPart)
        {
            context.Entry(actualPart).State = EntityState.Modified;
            actualPart.StickerYear = stickerDto.StickerYear;
            actualPart.StickerState = StudentStickerState.ReissueSticker;

            var actionDate = DateTime.Now;

            var stickerNote = new PersonStudentStickerNote
            {
                ActionDate = actionDate,
                InstitutionId = userContext.UserType == UserType.Rsd ? userContext.Institution.Id : null,
                SubordinateId = userContext.UserType == UserType.Rsd && userContext.Institution.ChildInstitutions.Count == 1
                ? userContext.Institution.ChildInstitutions.First().Id
                : null,
                PartId = actualPart.Id,
                Note = stickerDto.Note,
                UserId = userContext.UserId.Value,
                UserFullname = userContext.UserFullname
            };

            await context.PersonStudentStickerNotes.AddAsync(stickerNote);

            await personLotService
                .AddPersonLotAction(actualPart.LotId, actionDate, PersonLotActionType.ReissueSticker, $"специалност: {actualPart?.InstitutionSpeciality?.Speciality?.Code}");

            await context.SaveChangesAsync();

            return actualPart.StickerState;
        }


        public async Task<StickerErrorDto> ValidatePersonStudentStickerInfo(PersonStudent actualPart)
        {
            var stickerErrorDto = new StickerErrorDto();

            var requiredSemesters = ConstuctRequiredSemesters(actualPart?.InstitutionSpeciality?.Duration);

            if (requiredSemesters == null)
            {
                stickerErrorDto.OtherErrors.Add(new CustomStickerErrorDto
                {
                    Error = "Липсва продължителност на обучение в специалността.",
                    ErrorAlt = "Speciality has no duration."
                });

                stickerErrorDto.OtherErrors.AddRange(await ConstructOtherErrors(actualPart));

                stickerErrorDto.HasError = true;

                return stickerErrorDto;
            }
            else
            {
                var missingStudentSemesters = ConstructMissingStudentSemesters(requiredSemesters, actualPart.Semesters);

                if (missingStudentSemesters.Any())
                {
                    stickerErrorDto.MissingStudentSemesters.AddRange(missingStudentSemesters);
                    stickerErrorDto.HasError = true;
                }

                var customStickerOtherErrors = await ConstructOtherErrors(actualPart);

                if (customStickerOtherErrors.Any())
                {
                    stickerErrorDto.OtherErrors.AddRange(customStickerOtherErrors);
                    stickerErrorDto.HasError = true;
                }
            }

            return stickerErrorDto;
        }

        private List<(CourseType, Semester)> ConstuctRequiredSemesters(decimal? specialityDuration)
        {
            var requiredSemesters = new List<(CourseType, Semester)>();

            if (specialityDuration.HasValue)
            {
                var specialityDurationRoundDown = Convert.ToInt32(Math.Floor(specialityDuration.Value));

                for (int i = 1; i < specialityDurationRoundDown + 1; i++)
                {
                    requiredSemesters.Add(((CourseType)i, Semester.First));
                    requiredSemesters.Add(((CourseType)i, Semester.Second));
                }

                if (specialityDuration.Value % 1 != 0)
                {
                    var specialityLastHalfYear = specialityDurationRoundDown + 1;
                    requiredSemesters.Add(((CourseType)specialityLastHalfYear, Semester.First));
                }

                return requiredSemesters;
            }
            else
            {
                return null;
            }
        }

        private List<MissingStudentSemesterDto> ConstructMissingStudentSemesters(List<(CourseType, Semester)> requiredSemesters, List<PersonStudentSemester> studentSemesters)
        {
            var missingStudentSemesters = new List<MissingStudentSemesterDto>();

            var interruptedPeriods = studentSemesters
                .Where(e => e.StudentStatus.Alias == StudentStatusConstants.Interrupted)
                .Select(e => e.PeriodId)
                .ToList();

            var activeSemesters = studentSemesters
                .Where(e => e.StudentStatus.Alias == StudentStatusConstants.Active && !interruptedPeriods.Contains(e.PeriodId));

            foreach (var requiredSemester in requiredSemesters)
            {
                if (!activeSemesters.Any(e => e.Course == requiredSemester.Item1 && e.StudentSemester == requiredSemester.Item2)
                    && !activeSemesters.Any(e => e.IndividualPlanCourse == requiredSemester.Item1 && e.IndividualPlanSemester == requiredSemester.Item2))
                {
                    missingStudentSemesters.Add(new MissingStudentSemesterDto
                    {
                        Course = requiredSemester.Item1,
                        StudentSemester = requiredSemester.Item2
                    });
                }
            }

            return missingStudentSemesters;
        }

        private async Task<List<CustomStickerErrorDto>> ConstructOtherErrors(PersonStudent actualPart)
        {
            var customStickerErrors = new List<CustomStickerErrorDto>();

            // Check relocation file
            if (actualPart.Semesters.Any(e => (e.StudentEvent.Alias == StudentEventConstants.NextSemesterAfterRelocation || actualPart.StudentEvent.Alias == StudentEventConstants.NextSemesterAfterRelocationAbroad)
                && e.SemesterRelocatedFile == null))
            {
                customStickerErrors.Add(new CustomStickerErrorDto
                {
                    Error = "Липсва документ за признаване след преместване.",
                    ErrorAlt = "Missing relocation document."
                });
            }

            // Check faculty number
            if (string.IsNullOrWhiteSpace(actualPart.FacultyNumber))
            {
                customStickerErrors.Add(new CustomStickerErrorDto
                {
                    Error = "Липсва факултетен номер.",
                    ErrorAlt = "Missing faculty number."
                });
            }

            if (actualPart.PeType == PreviousEducationType.HighSchool)
            {
                switch (actualPart.PeHighSchoolType)
                {
                    case PreviousHighSchoolEducationType.MissingInRegister:
                        var missingInRegisterError = ConstructPeMissingInRegisterErrors(actualPart);
                        if (missingInRegisterError != null)
                        {
                            customStickerErrors.Add(missingInRegisterError);
                        }
                        break;
                    case PreviousHighSchoolEducationType.Abroad:
                        var abroadError = ConstructPeAbroadErrors(actualPart);
                        if (abroadError != null)
                        {
                            customStickerErrors.Add(abroadError);
                        }
                        break;
                    case PreviousHighSchoolEducationType.ClosedInstitution:
                        var closedInstitutionError = ConstructPeClosedInstitutionErrors(actualPart);
                        if (closedInstitutionError != null)
                        {
                            customStickerErrors.Add(closedInstitutionError);
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (actualPart.PeType == PreviousEducationType.Secondary)
            {
                var secondaryError = await ConstructPeSecondaryErrors(actualPart.LotId);

                if (secondaryError != null)
                {
                    customStickerErrors.Add(secondaryError);
                }
            }
            else
            {
                customStickerErrors.Add(new CustomStickerErrorDto
                {
                    Error = "Липсва информация за предходното образование.",
                    ErrorAlt = "Missing information for previous education."
                });
            }

            return customStickerErrors;
        }

        private async Task<CustomStickerErrorDto> ConstructPeSecondaryErrors(int lotId)
        {
            var personSecondary = await personSecondaryService.Get(lotId);

            if (personSecondary == null || personSecondary.GraduationYear < 1960 || personSecondary.GraduationYear > DateTime.Now.Year
                || (!personSecondary.SchoolId.HasValue && string.IsNullOrWhiteSpace(personSecondary.MissingSchoolName) && string.IsNullOrWhiteSpace(personSecondary.ForeignSchoolName))
                || string.IsNullOrWhiteSpace(personSecondary.DiplomaNumber) || !personSecondary.DiplomaDate.HasValue
                || (personSecondary.Country.Code != CountryConstants.BulgariaCode && (personSecondary.PersonSecondaryRecognitionDocument == null || string.IsNullOrWhiteSpace(personSecondary.RecognitionNumber) || !personSecondary.RecognitionDate.HasValue)))
            {
                return new CustomStickerErrorDto
                {
                    Error = "Липсват задължителни полета за предходното средно образование.",
                    ErrorAlt = "Missing information for previous secondary education."
                };
            }
            else
            {
                return null;
            }
        }

        private CustomStickerErrorDto ConstructPeMissingInRegisterErrors(PersonStudent actualPart)
        {
            if (!actualPart.PeEducationalQualificationId.HasValue || !actualPart.PeInstitutionId.HasValue 
                || (!actualPart.PeInstitutionSpecialityId.HasValue && string.IsNullOrWhiteSpace(actualPart.PeSpecialityName))
                || !actualPart.PeResearchAreaId.HasValue || string.IsNullOrWhiteSpace(actualPart.PeDiplomaNumber) || !actualPart.PeDiplomaDate.HasValue)
            {
                return new CustomStickerErrorDto
                {
                    Error = "Липсват задължителни полета за предходното висше образование.",
                    ErrorAlt = "Missing information for previous education."
                };
            }
            else
            {
                return null;
            }
        }

        private CustomStickerErrorDto ConstructPeAbroadErrors(PersonStudent actualPart)
        {
            if (!actualPart.PeEducationalQualificationId.HasValue || !actualPart.PeAcquiredForeignEducationalQualificationId.HasValue
                || !actualPart.PeCountryId.HasValue || string.IsNullOrWhiteSpace(actualPart.PeInstitutionName)
                || string.IsNullOrWhiteSpace(actualPart.PeAcquiredSpeciality) || string.IsNullOrWhiteSpace(actualPart.PeRecognizedSpeciality)
                || !actualPart.PeResearchAreaId.HasValue || string.IsNullOrWhiteSpace(actualPart.PeDiplomaNumber) || !actualPart.PeDiplomaDate.HasValue
                || actualPart.PeRecognitionDocument == null || string.IsNullOrWhiteSpace(actualPart.PeRecognitionNumber) || !actualPart.PeRecognitionDate.HasValue)
            {
                return new CustomStickerErrorDto
                {
                    Error = "Липсват задължителни полета за предходното висше образование.",
                    ErrorAlt = "Missing information for previous education."
                };
            }
            else
            {
                return null;
            }
        }

        private CustomStickerErrorDto ConstructPeClosedInstitutionErrors(PersonStudent actualPart)
        {
            if (!actualPart.PeEducationalQualificationId.HasValue || string.IsNullOrWhiteSpace(actualPart.PeInstitutionName)
                   || string.IsNullOrWhiteSpace(actualPart.PeSpecialityName) || !actualPart.PeResearchAreaId.HasValue 
                   || string.IsNullOrWhiteSpace(actualPart.PeDiplomaNumber) || !actualPart.PeDiplomaDate.HasValue)
            {
                return new CustomStickerErrorDto
                {
                    Error = "Липсват задължителни полета за предходното висше образование.",
                    ErrorAlt = "Missing information for previous education."
                };
            }
            else
            {
                return null;
            }
        }

        private async Task<PersonStudentStickerSearchDto> ReturnStudentStickerSearchDtoById(int id)
        {
            var partResult = await context.PersonStudents
                        .AsNoTracking()
                        .Include(e => e.Lot.PersonBasic.BirthCountry)
                        .Include(e => e.Lot.PersonBasic.BirthSettlement)
                        .Include(e => e.Lot.PersonBasic.Citizenship)
                        .Include(e => e.Lot.PersonBasic.SecondCitizenship)
                        .Include(e => e.Subordinate)
                        .Include(e => e.InstitutionSpeciality.Speciality)
                        .Include(e => e.StudentStatus)
                        .Include(e => e.StudentEvent)
                        .Include(e => e.StickerNotes)
                            .ThenInclude(s => s.Institution)
                        .SingleOrDefaultAsync(e => e.Id == id);

            return new PersonStudentStickerSearchDto(partResult);
        }
    }
}
