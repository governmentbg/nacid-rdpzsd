using Infrastructure.DomainValidation;
using Infrastructure.Integrations.EmsIntegration;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Lot;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Services.EntityServices;
using Rdpzsd.Services.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd
{
    public class PersonLotService
    {
        private readonly RdpzsdDbContext context;
        private readonly UserContext userContext;
        private readonly PersonLotValidationService personLotValidationService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly EmsIntegrationService emsIntegrationService;

        public PersonLotService(
            RdpzsdDbContext context,
            UserContext userContext,
            PersonLotValidationService personLotValidationService,
            DomainValidatorService domainValidatorService,
            EmsIntegrationService emsIntegrationService
        )
        {
            this.context = context;
            this.userContext = userContext;
            this.personLotValidationService = personLotValidationService;
            this.domainValidatorService = domainValidatorService;
            this.emsIntegrationService = emsIntegrationService;
        }

        public async Task<PersonLotDto> GetLot(string uan)
        {
            var personLot = await context.PersonLots
                .AsNoTracking()
                .Include(e => e.PersonBasic)
                .Include(e => e.PersonBasic.Citizenship)
                .Include(e => e.PersonBasic.SecondCitizenship)
                .Include(e => e.PersonSecondary)
                .Include(e => e.PersonStudents)
                .Include(e => e.PersonDoctorals)
                .SingleAsync(e => e.Uan == uan);

            return new PersonLotDto
            {
                PersonLot = personLot,
                PersonBasicNames = personLot.PersonBasic.FullName,
                PersonBasicNamesAlt = personLot.PersonBasic.FullNameAlt,
                Citizenship = personLot.PersonBasic.Citizenship,
                SecondCitizenship = personLot.PersonBasic.SecondCitizenship,
                HasActualStudentOrDoctoral = personLot.PersonStudents.Any(e => e.State != PartState.Erased) || personLot.PersonDoctorals.Any(e => e.State != PartState.Erased),
                PersonSecondaryFromRso = personLot.PersonSecondary?.FromRso,
            };
        }

        public async Task<LotState> GetLotState(int lotId)
        {
            var personLot = await context.PersonLots
                .AsNoTracking()
                .SingleAsync(e => e.Id == lotId);

            return personLot.State;
        }

        public async Task<PersonLot> CreateLot(PersonBasic personBasic)
        {
            using var transaction = context.BeginTransaction();

            personBasic.ValidateProperties(context, domainValidatorService);

            var date = DateTime.Now;

            EntityService.ClearSkipProperties(personBasic);

            bool isForApproval = userContext.UserType == UserType.Rsd
                                && string.IsNullOrWhiteSpace(personBasic.Uin)
                                && string.IsNullOrWhiteSpace(personBasic.ForeignerNumber)
                                && !string.IsNullOrWhiteSpace(personBasic.IdnNumber)
                                && personBasic.PassportCopy != null;

            var personLot = new PersonLot
            {
                State = isForApproval ? LotState.PendingApproval : LotState.Actual,
                CreateDate = date,
                CreateUserId = userContext.UserId.Value,
                CreateInstitutionId = userContext.UserType == UserType.Rsd ? userContext.Institution.Id : null,
                CreateSubordinateId = userContext.UserType == UserType.Rsd && userContext.Institution.ChildInstitutions.Count == 1
                    ? userContext.Institution.ChildInstitutions.First().Id
                    : null,
                PersonLotActions = new List<PersonLotAction>
                {
                    new PersonLotAction
                    {
                        ActionDate = date,
                        InstitutionId = userContext.UserType == UserType.Rsd ? userContext.Institution.Id : null,
                        ActionType = isForApproval ? PersonLotActionType.PendingApproval : PersonLotActionType.Create,
                        SubordinateId = userContext.UserType == UserType.Rsd && userContext.Institution.ChildInstitutions.Count == 1
                            ? userContext.Institution.ChildInstitutions.First().Id
                            : null,
                        UserFullname = userContext.UserFullname,
                        UserId = userContext.UserId.Value
                    }
                }
            };

            personLot.GenerateUan(context);

            personBasic.State = PartState.Actual;
            personBasic.PartInfo = new PersonBasicInfo
            {
                ActionDate = date,
                InstitutionId = userContext.UserType == UserType.Rsd ? userContext.Institution.Id : null,
                SubordinateId = userContext.UserType == UserType.Rsd && userContext.Institution.ChildInstitutions.Count == 1
                            ? userContext.Institution.ChildInstitutions.First().Id
                            : null,
                UserFullname = userContext.UserFullname,
                UserId = userContext.UserId.Value
            };

            personLot.PersonBasic = personBasic;

            await context.PersonLots.AddAsync(personLot);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return personLot;
        }

        public async Task<LotState> CancelPendingApproval(int lotId, NoteDto noteDto)
        {
            var pendingPersonLot = await context.PersonLots
                .SingleAsync(e => e.Id == lotId && e.State == LotState.PendingApproval);

            pendingPersonLot.State = LotState.CancelApproval;

            await AddPersonLotAction(lotId, DateTime.Now, PersonLotActionType.CancelApproval, noteDto?.Note);

            await context.SaveChangesAsync();

            return pendingPersonLot.State;
        }

        public async Task<LotState> SendForApproval(int lotId)
        {
            var cancelApprovalPersonLot = await context.PersonLots
                .Include(e => e.PersonBasic.PassportCopy)
                .SingleAsync(e => e.Id == lotId && (e.State == LotState.CancelApproval || e.State == LotState.MissingPassportCopy));

            personLotValidationService.VerifySendForApproval(cancelApprovalPersonLot.PersonBasic);

            cancelApprovalPersonLot.State = LotState.PendingApproval;

            await AddPersonLotAction(lotId, DateTime.Now, PersonLotActionType.PendingApproval, null);

            await context.SaveChangesAsync();
            return cancelApprovalPersonLot.State;
        }

        public async Task<LotState> Approve(int lotId)
        {
            var pendingPersonLot = await context.PersonLots
                .SingleAsync(e => e.Id == lotId && e.State == LotState.PendingApproval);

            pendingPersonLot.State = LotState.Actual;

            await AddPersonLotAction(lotId, DateTime.Now, PersonLotActionType.Approve, null);

            await context.SaveChangesAsync();
            return pendingPersonLot.State;
        }

        public async Task<LotState> Erase(PersonLot personLot)
        {
            personLot.State = LotState.Erased;

            await AddPersonLotAction(personLot.Id, DateTime.Now, PersonLotActionType.Erased, null);

            await context.SaveChangesAsync();
            return personLot.State;
        }

        public async Task<List<PersonLotActionDto>> GetPersonLotActions(int lotId)
        {
            var personLotActions = await context.PersonLotActions
                .AsNoTracking()
                .Include(e => e.Institution)
                .Include(e => e.Subordinate)
                .Where(e => e.LotId == lotId)
                .OrderByDescending(e => e.ActionDate)
                .ToListAsync();

            var userIds = personLotActions
                .Select(e => e.UserId)
                .Distinct()
                .ToList();

            var emsUsers = await emsIntegrationService.GetUsersInfo(userIds);

            return personLotActions.ToPersonLotActionDto(emsUsers);
        }

        public async Task<PersonLotAction> GetLatestPersonLotActionByType(int lotId, PersonLotActionType actionType)
        {
            var personLotAction = await context.PersonLotActions
                .AsNoTracking()
                .Include(e => e.Institution)
                .Include(e => e.Subordinate)
                .Where(e => e.LotId == lotId && e.ActionType == actionType)
                .OrderByDescending(e => e.ActionDate)
                .FirstOrDefaultAsync();

            return personLotAction;
        }

        public async Task AddPersonLotAction(int lotId, DateTime actionDate, PersonLotActionType actionType, string note)
        {
            var newPersonLotAction = new PersonLotAction
            {
                ActionDate = actionDate,
                ActionType = actionType,
                InstitutionId = userContext.UserType == UserType.Rsd ? userContext.Institution.Id : null,
                SubordinateId = userContext.UserType == UserType.Rsd && userContext.Institution.ChildInstitutions.Count == 1
                    ? userContext.Institution.ChildInstitutions.First().Id
                    : null,
                LotId = lotId,
                Note = note,
                UserId = userContext.UserId.Value,
                UserFullname = userContext.UserFullname
            };

            await context.PersonLotActions.AddAsync(newPersonLotAction);
        }
    }
}
