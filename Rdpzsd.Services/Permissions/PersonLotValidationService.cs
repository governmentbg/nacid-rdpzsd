using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Base;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Permissions
{
    public class PersonLotValidationService
    {
        private readonly RdpzsdDbContext context;
        private readonly DomainValidatorService domainValidatorService;
        private readonly UserContext userContext;
        private readonly PermissionService permissionService;

        public PersonLotValidationService(
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            UserContext userContext,
            PermissionService permissionService
        )
        {
            this.context = context;
            this.domainValidatorService = domainValidatorService;
            this.userContext = userContext;
            this.permissionService = permissionService;
        }

        public async Task VerifyLotState(int lotId, params LotState[] acceptedLotStates)
        {
            var personLot = await context.PersonLots
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == lotId);

            VerifyLotState(personLot.State, acceptedLotStates);
        }

        public void VerifyLotState(LotState lotState, params LotState[] acceptedLotStates)
        {
            if (!acceptedLotStates.Any(e => e == lotState))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.Lot_WrongLotState);
            }
        }

        public void VerifyPartState(PartState partState, params PartState[] acceptedPartStates)
        {
            if (!acceptedPartStates.Any(e => e == partState))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.Part_WrongPartState);
            }
        }

        public async Task VerifyLotStateRsdUser(int lotId, params LotState[] acceptedLotStates)
        {
            var personLot = await context.PersonLots
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == lotId);

            if (userContext.UserType != UserType.Rsd && personLot.CreateUserId != userContext.UserId)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
            }

            VerifyLotState(personLot.State, acceptedLotStates);
        }

        public async Task ValidateUniquePersonLot(string uin, string foreignerNumber, int? personLotId = null)
        {
            var personLots = context.PersonLots
                .AsNoTracking()
                .Include(e => e.PersonBasic)
                .Where(e => !personLotId.HasValue || e.Id != personLotId);

            if (!string.IsNullOrWhiteSpace(uin))
            {
                if (await personLots.AnyAsync(e => e.PersonBasic.Uin == uin))
                {
                    domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_UinExists);
                }
            }

            if (!string.IsNullOrWhiteSpace(foreignerNumber))
            {
                if (await personLots.AnyAsync(e => e.PersonBasic.ForeignerNumber == foreignerNumber))
                {
                    domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_UinExists);
                }
            }
        }

        public async Task ValidateUniquePersonEmail(string email, int? personLotId)
        {
            var emailsList = await context.PersonBasics
                .AsNoTracking()
                .Where(e => !personLotId.HasValue || e.Id != personLotId)
                .Select(e => e.Email.ToLower())
                .ToListAsync();

            var emailToLowerCase = email.ToLower();

            if (!string.IsNullOrWhiteSpace(email) && emailToLowerCase != EmailConstants.NoEmail.ToLower() && emailsList.Contains(emailToLowerCase))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_EmailNotUnique);
            }
        }

        public async Task VerifyPersonBasicEditPermissions(int lotId)
        {
            var personLot = await context.PersonLots
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == lotId);

            if (personLot.State != LotState.Actual && personLot.State != LotState.Erased && personLot.State != LotState.CancelApproval && personLot.State != LotState.MissingPassportCopy)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.Lot_WrongLotState); 
            }
            else if ((personLot.State == LotState.MissingPassportCopy || personLot.State == LotState.CancelApproval) && (userContext.UserType != UserType.Rsd || userContext.UserId != personLot.CreateUserId))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
            }
        }

        public async Task VerifyPersonLotActionsPermissions(int lotId)
        {
            var personLot = await context.PersonLots
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == lotId);

            if ((personLot.State != LotState.MissingPassportCopy || personLot.State == LotState.CancelApproval) && (userContext.UserType != UserType.Rsd || userContext.UserId != personLot.CreateUserId))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
            }
            else if (personLot.State != LotState.CancelApproval && personLot.State != LotState.MissingPassportCopy)
            {
                permissionService.VerifyInternalUser(PermissionConstants.RdpzsdRead);
            }
        }

        public void VerifyNotStudentOrDoctoral(PersonLot personLot)
        {
            if (personLot.PersonStudents.Any(e => e.State != PartState.Erased) || personLot.PersonDoctorals.Any(e => e.State != PartState.Erased))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonLot_CantEraseStudentDoctoral);
            }
        }

        public async Task VerifyHasStudentSecondaryAndLotState(int lotId, PreviousEducationType previousEducationType, LotState? lotState)
        {
            var personLot = await context.PersonLots
                    .AsNoTracking()
                    .Include(e => e.PersonSecondary)
                    .SingleAsync(e => e.Id == lotId);

            if (previousEducationType == PreviousEducationType.Secondary && personLot.PersonSecondary == null)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonSecondary_NoInformation);
            }

            if (lotState.HasValue)
            {
                VerifyLotState(personLot.State, lotState.Value);
            }
        }

        public void VerifyStudentDoctoralPartEditPermission<TStudentDoctoral, TPartInfo, TSemester>(TStudentDoctoral entity, bool enableEmsUser = false, bool onlyInstitutionValidation = false)
            where TStudentDoctoral : BasePersonStudentDoctoral<TPartInfo, TSemester>
            where TPartInfo : PartInfo
            where TSemester : BasePersonSemester, new()
        {
            if (userContext.UserType == UserType.Rsd)
            {
                int? institutionId = userContext.Institution?.Id;
                int? subordinateId = userContext.Institution?.ChildInstitutions.Count == 1 ? userContext.Institution.ChildInstitutions.First().Id : null;

                if(entity.InstitutionSpeciality.InstitutionSpecialityJointSpecialities == null || !entity.InstitutionSpeciality.InstitutionSpecialityJointSpecialities.Any())
				{
                    if (subordinateId.HasValue && !onlyInstitutionValidation)
                    {
                        if (entity.SubordinateId != subordinateId)
                        {
                            if (entity.SubordinateId != subordinateId)
                            {
                                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_NotEnoughPermissions);
                            }
                        }
                        else
                        {
                            if (entity.InstitutionId != institutionId)
                            {
                                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_NotEnoughPermissions);
                            }
                        }
                    }
                }
                else
				{
                    if(!entity.InstitutionSpeciality.InstitutionSpecialityJointSpecialities.Any(e => e.InstitutionId == institutionId) && entity.InstitutionId != institutionId)
					{
                        domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_NotEnoughPermissions);
                    }
                }
               

                switch (entity.InstitutionSpeciality.Speciality.EducationalQualification.Alias)
                {
                    case EducationalQualificationConstants.ProfessionalBachelor:
                        if (!userContext.Institution.HasBachelor)
                        {
                            domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_NoPermissionsForBachelor);
                        }
                        break;
                    case EducationalQualificationConstants.Bachelor:
                        if (!userContext.Institution.HasBachelor)
                        {
                            domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_NoPermissionsForBachelor);
                        }
                        break;
                    case EducationalQualificationConstants.MasterSecondary:
                        if (!userContext.Institution.HasMaster)
                        {
                            domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_NoPermissionsForMaster);
                        }
                        break;
                    case EducationalQualificationConstants.MasterHigh:
                        if (!userContext.Institution.HasMaster)
                        {
                            domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_NoPermissionsForMaster);
                        }
                        break;
                    case EducationalQualificationConstants.Doctor:
                        if (!userContext.Institution.HasDoctoral)
                        {
                            domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_NoPermissionsForDoctoral);
                        }
                        break;
                    default:
                        break;
                }
            }
            else if(!enableEmsUser)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_NotEnoughPermissions);
            }
        }

        public async Task VerifyNotSelectedAsRelocated<TSemester>(int partId, TSemester entity)
            where TSemester : BasePersonSemester, new()
        {
            if (entity.StudentEvent.Alias == StudentEventConstants.Relocation
                && await context.Set<TSemester>().AsNoTracking().AnyAsync(e => e.RelocatedFromPartId == partId))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_CantDeleteSelectedAsRelocated);
            }
        }

        public void VerifyInitialSpecialitySemester<TStudentDoctoral, TPartInfo, TSemester>(TStudentDoctoral entity)
            where TStudentDoctoral : BasePersonStudentDoctoral<TPartInfo, TSemester>
            where TPartInfo : PartInfo
            where TSemester : BasePersonSemester, IValidate, new()
        {
            if (!entity.Semesters.Any() || entity.Semesters.Count != 1)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_OneSemesterRequired);
            }

            var currentSemester = entity.Semesters.First();
            currentSemester.ValidateProperties(context, domainValidatorService);

            entity.StudentStatusId = currentSemester.StudentStatusId;
            entity.StudentEventId = currentSemester.StudentEventId;
        }

        public void VerifyPersonStudentNotGraduated<TStudentDoctoral, TPartInfo, TSemester>(TStudentDoctoral entity)
            where TStudentDoctoral : BasePersonStudentDoctoral<TPartInfo, TSemester>
            where TPartInfo : PartInfo
            where TSemester : BasePersonSemester, IValidate, new()
        {
            if (entity.StudentStatus?.Alias == StudentStatusConstants.Graduated)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_CannotEditGraduated);
            }
        }

        public void VerifyPersonStudentEvent(string currentStudentEvent, params string[] studentEvents)
        {
            if (!studentEvents.Any(e => e == currentStudentEvent))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_WrongStudentEvent);
            }
        }

        public void VerifyPersonStudentStatus(string currentStudentStatus, params string[] studentStatuses)
        {
            if (!studentStatuses.Any(e => e == currentStudentStatus))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_WrongStudentEvent);
            }
        }

        public void VerifyPersonStudentStatusOrEvent(string currentStudentStatus, string studentStatus, string currentStudentEvent, string studentEvent)
        {
            if (currentStudentStatus != studentStatus && currentStudentEvent != studentEvent)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_WrongStudentEvent);
            }
        }

        public void VerifyUniquePeriod(List<PersonStudentSemester> semesters, PersonStudentSemester currentSemester)
        {
            if (currentSemester.StudentStatus.Alias == StudentStatusConstants.Completed
                && semesters.Any(e => e.PeriodId == currentSemester.PeriodId && e.Id != currentSemester.Id && e.StudentStatus.Alias == StudentStatusConstants.Completed))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_SemesterNotUnique);
            }
        }

        public void VerifySemesterPeriodAfterLatest()
        {

        }

        public void VerifyAtLeastOneSemester<TSemester>(List<TSemester> semesters)
            where TSemester : BasePersonSemester, new()
        {
            if (semesters.Count < 2)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_MustHaveAtleastOneSemester);
            }
        }

        public void VerifyOnlyOneSemester<TSemester>(List<TSemester> semesters)
            where TSemester : BasePersonSemester, new()
        {
            if (semesters.Count > 1)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_CantDeleteWhenHasMoreThanOneSemester);
            }
        }

        public void VerifySendForApproval(PersonBasic personBasic)
        {
            if (string.IsNullOrWhiteSpace(personBasic.IdnNumber) || personBasic?.PassportCopy == null || string.IsNullOrWhiteSpace(personBasic?.PassportCopy?.Name))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_NoIdn);
            }
        }

        public void VerifyUniquePersonDiploma(PersonStudent personStudent)
        {
            if (personStudent.Diploma != null)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDiploma_NotUniqueDiploma);
            }
        }

        public void VerifyUniqueValidDuplicateDiploma(PersonStudent personStudent, int? currentDuplicateDiplomaId = null)
        {
            if (personStudent.Diploma == null 
                || personStudent.Diploma.IsValid 
                || personStudent.DuplicateDiplomas.Any(e => e.IsValid && (!currentDuplicateDiplomaId.HasValue || e.Id != currentDuplicateDiplomaId)))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDuplicateDiploma_NotUniqueValidDuplicateDiploma);
            }
        }

        public void ValidateDoctoralSemesterProtocolDate(PersonDoctoral personDoctoral, PersonDoctoralSemester newSemester)
        {
            var currentSemester = personDoctoral.Semesters
                .OrderBy(e => e.ProtocolDate.Date)
                .Last();

            if (currentSemester.ProtocolDate.Date > newSemester.ProtocolDate.Date)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonStudentDoctoral_ProtocolDateCannotBeLessThanPrevious);
            }
        }
    }
}
