using Infrastructure.DomainValidation;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Rdpzsd.Parts;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Services.EntityServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Parts.PersonStudentGraduation
{
    public class PersonStudentDuplicateDiplomaService
    {
        private readonly RdpzsdDbContext context;
        private readonly DomainValidatorService domainValidatorService;
        private readonly PersonStudentService personStudentService;
        private readonly PersonLotService personLotService;
        private readonly UserContext userContext;

        public PersonStudentDuplicateDiplomaService(
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            PersonStudentService personStudentService,
            PersonLotService personLotService,
            UserContext userContext
        )
        {
            this.context = context;
            this.domainValidatorService = domainValidatorService;
            this.personStudentService = personStudentService;
            this.personLotService = personLotService;
            this.userContext = userContext;
        }

        public async Task<PersonStudentDuplicateDiploma> Get(int duplicateDiplomaId)
        {
            var duplicateDiploma = await context.Set<PersonStudentDuplicateDiploma>()
                        .AsNoTracking()
                        .Include(e => e.File)
                        .SingleAsync(e => e.Id == duplicateDiplomaId);

            return duplicateDiploma;
        }

        public async Task<PersonStudentDuplicateDiploma> Create(PersonStudentDuplicateDiplomaCreateDto createDuplicateDiplomaDto, PersonStudent actualPart)
        {
            var duplicateDiploma = createDuplicateDiplomaDto.NewDuplicateDiploma;
            var stickerDto = createDuplicateDiplomaDto.StickerDto;

            duplicateDiploma.DuplicateStickerState = StudentStickerState.SendForSticker;

            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.SendDuplicateForSticker, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, година на стикера: {duplicateDiploma?.DuplicateStickerYear}");

            EntityService.ClearSkipProperties(duplicateDiploma);

            await context.PersonStudentDuplicateDiplomas.AddAsync(duplicateDiploma);

            var stickerNote = new PersonStudentStickerNote
            {
                ActionDate = createDate,
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

            await context.SaveChangesAsync();

            return await Get(duplicateDiploma.Id);
        }

        public async Task<PersonStudentDuplicateDiploma> Update(PersonStudentDuplicateDiploma duplicateDiploma, PersonStudent actualPart)
        {
            duplicateDiploma.ValidateProperties(context, domainValidatorService);

            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonStudentDuplicateDiplomaEdit, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, година на стикера: {duplicateDiploma?.DuplicateStickerYear}");

            var duplicateDiplomaForUpdate = await context.Set<PersonStudentDuplicateDiploma>().AsQueryable()
                        .Include(e => e.File)
                        .SingleAsync(e => e.Id == duplicateDiploma.Id && e.IsValid);

            EntityService.Update(duplicateDiplomaForUpdate, duplicateDiploma, context);
            await context.SaveChangesAsync();

            return await Get(duplicateDiploma.Id);
        }

        public async Task<PersonStudentDuplicateDiploma> Invalid(PersonStudentDuplicateDiploma duplicateDiplomaForUpdate, PersonStudent actualPart)
        {
            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);

            duplicateDiplomaForUpdate.IsValid = false;

            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonStudentDuplicateDiplomaInvalid, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, номер на диплома: {duplicateDiplomaForUpdate?.DuplicateDiplomaNumber}");

            await context.SaveChangesAsync();

            return duplicateDiplomaForUpdate;
        }
    }
}
