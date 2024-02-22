using Infrastructure.DomainValidation;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Services.EntityServices;
using System;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Parts.PersonStudentGraduation
{
    public class PersonStudentProtocolService
    {
        private readonly RdpzsdDbContext context;
        private readonly DomainValidatorService domainValidatorService;
        private readonly PersonStudentService personStudentService;
        private readonly PersonLotService personLotService;

        public PersonStudentProtocolService(
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            PersonStudentService personStudentService,
            PersonLotService personLotService
        )
        {
            this.context = context;
            this.domainValidatorService = domainValidatorService;
            this.personStudentService = personStudentService;
            this.personLotService = personLotService;
        }

        public async Task<PersonStudentProtocol> Get(int protocolId)
        {
            var protocol = await context.Set<PersonStudentProtocol>()
                        .AsNoTracking()
                        .SingleAsync(e => e.Id == protocolId);

            return protocol;
        }

        public async Task<PersonStudentProtocol> Create(PersonStudentProtocol protocol, PersonStudent actualPart)
        {
            protocol.ValidateProperties(context, domainValidatorService);

            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonStudentProtocolAdd, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, номер на протокол: {protocol?.ProtocolNumber}");

            EntityService.ClearSkipProperties(protocol);

            await context.PersonStudentProtocols.AddAsync(protocol);
            await context.SaveChangesAsync();

            return await Get(protocol.Id);
        }

        public async Task<PersonStudentProtocol> Update(PersonStudentProtocol protocol, PersonStudent actualPart)
        {
            protocol.ValidateProperties(context, domainValidatorService);

            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonStudentProtocolEdit, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, номер на протокол: {protocol?.ProtocolNumber}");

            var protocolForUpdate = await context.Set<PersonStudentProtocol>().AsQueryable()
                        .SingleAsync(e => e.Id == protocol.Id);

            EntityService.Update(protocolForUpdate, protocol, context);
            await context.SaveChangesAsync();

            return await Get(protocol.Id);
        }

        public async Task Delete(PersonStudentProtocol protocolForDelete, PersonStudent actualPart)
        {
            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonStudentProtocolErase, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, номер на протокол: {protocolForDelete?.ProtocolNumber}");

            EntityService.Remove(protocolForDelete, context);
            await context.SaveChangesAsync();
        }
    }
}
