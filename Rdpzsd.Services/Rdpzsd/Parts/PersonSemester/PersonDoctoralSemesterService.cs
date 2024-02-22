using Infrastructure.DomainValidation;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Services.EntityServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Parts.PersonSemester
{
    public class PersonDoctoralSemesterService
    {
        private readonly RdpzsdDbContext context;
        private readonly DomainValidatorService domainValidatorService;
        private readonly PersonDoctoralService personDoctoralService;
        private readonly PersonLotService personLotService;

        public PersonDoctoralSemesterService(
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            PersonDoctoralService personDoctoralService,
            PersonLotService personLotService
        )
        {
            this.context = context;
            this.domainValidatorService = domainValidatorService;
            this.personDoctoralService = personDoctoralService;
            this.personLotService = personLotService;
        }

        public async virtual Task<PersonDoctoralSemester> Get(int semesterId)
        {
            var semester = await new PersonDoctoralSemester().IncludeAll(context.Set<PersonDoctoralSemester>().AsQueryable())
                        .AsNoTracking()
                        .SingleAsync(e => e.Id == semesterId);

            return semester;
        }

        public async virtual Task<PersonDoctoralSemester> Create(PersonDoctoralSemester semester, PersonDoctoral actualPart)
        {
            semester.ValidateProperties(context, domainValidatorService);

            var createDate = DateTime.Now;

            await personDoctoralService.CreateHistory(actualPart);
            personDoctoralService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonDoctoralSemesterAdd, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, {semester?.ProtocolDate.ToString("dd.MM.yyyy")} - {semester?.ProtocolNumber}");

            UpdateEntityStatusEvent(semester, actualPart, true);

            EntityService.ClearSkipProperties(semester);
            await context.AddAsync(semester);
            await context.SaveChangesAsync();

            return await Get(semester.Id);
        }

        public async virtual Task<PersonDoctoralSemester> Update(PersonDoctoralSemester semester, PersonDoctoral actualPart)
        {
            semester.ValidateProperties(context, domainValidatorService);

            var createDate = DateTime.Now;

            await personDoctoralService.CreateHistory(actualPart);
            personDoctoralService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonDoctoralSemesterEdit, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, {semester?.ProtocolDate.ToString("dd.MM.yyyy")} - {semester?.ProtocolNumber}");

            UpdateEntityStatusEvent(semester, actualPart, false);

            var semesterForUpdate = await new PersonDoctoralSemester().IncludeAll(context.Set<PersonDoctoralSemester>().AsQueryable())
                        .SingleAsync(e => e.Id == semester.Id);

            EntityService.Update(semesterForUpdate, semester, context);
            await context.SaveChangesAsync();

            return await Get(semester.Id);
        }

        public async virtual Task Delete(PersonDoctoralSemester semesterForDelete, PersonDoctoral actualPart)
        {
            var createDate = DateTime.Now;

            await personDoctoralService.CreateHistory(actualPart);
            personDoctoralService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonDoctoralSemesterErase, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, {semesterForDelete?.ProtocolDate.ToString("dd.MM.yyyy")} - {semesterForDelete?.ProtocolNumber}");

            var lastSemester = actualPart.Semesters
                .OrderByDescending(e => e.ProtocolDate.Date)
                .ThenByDescending(e => e.Id)
                .Where(e => e.Id != semesterForDelete.Id)
                .First();

            context.Entry(actualPart).State = EntityState.Modified;
            actualPart.StudentStatusId = lastSemester.StudentStatusId;
            actualPart.StudentEventId = lastSemester.StudentEventId;

            EntityService.Remove(semesterForDelete, context);
            await context.SaveChangesAsync();
        }

        private void UpdateEntityStatusEvent(PersonDoctoralSemester semester, PersonDoctoral actualPart, bool forceUpdate)
        {
            var lastSemester = actualPart.Semesters
                .OrderByDescending(e => e.ProtocolDate.Date)
                .ThenByDescending(e => e.Id)
                .Where(e => e.Id != semester.Id)
                .FirstOrDefault();

            if (forceUpdate
                || lastSemester == null 
                || semester.ProtocolDate.Date > lastSemester.ProtocolDate.Date
                || (semester.ProtocolDate.Date == lastSemester.ProtocolDate.Date && semester.Id > lastSemester.Id))
            {
                context.Entry(actualPart).State = EntityState.Modified;
                actualPart.StudentStatusId = semester.StudentStatusId;
                actualPart.StudentEventId = semester.StudentEventId;
            }
        }
    }
}
