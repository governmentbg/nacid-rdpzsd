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
    public class PersonStudentSemesterService
    {
        private readonly RdpzsdDbContext context;
        private readonly DomainValidatorService domainValidatorService;
        private readonly PersonStudentService personStudentService;
        private readonly PersonLotService personLotService;

        public PersonStudentSemesterService(
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

        public async virtual Task<PersonStudentSemester> Get(int semesterId)
        {
            var semester = await new PersonStudentSemester().IncludeAll(context.Set<PersonStudentSemester>().AsQueryable())
                        .AsNoTracking()
                        .SingleAsync(e => e.Id == semesterId);

            return semester;
        }

        public async virtual Task<PersonStudentSemester> Create(PersonStudentSemester semester, PersonStudent actualPart)
        {
            semester.ValidateProperties(context, domainValidatorService);

            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonStudentSemesterAdd, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, {semester?.Period?.Year}/{semester?.Period?.Year + 1} {(semester?.Period?.Semester == Models.Enums.Semester.First ? "зимен" : "летен")} семестър");

            UpdateEntityStatusEvent(semester, actualPart, true);

            EntityService.ClearSkipProperties(semester);
            await context.AddAsync(semester);
            await context.SaveChangesAsync();

            return await Get(semester.Id);
        }

        public async virtual Task<PersonStudentSemester> Update(PersonStudentSemester semester, PersonStudent actualPart)
        {
            semester.ValidateProperties(context, domainValidatorService);

            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonStudentSemesterEdit, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, {semester?.Period?.Year}/{semester?.Period?.Year + 1} {(semester?.Period?.Semester == Models.Enums.Semester.First ? "зимен" : "летен")} семестър");

            UpdateEntityStatusEvent(semester, actualPart, false);

            var semesterForUpdate = await new PersonStudentSemester().IncludeAll(context.Set<PersonStudentSemester>().AsQueryable())
                        .SingleAsync(e => e.Id == semester.Id);

            EntityService.Update(semesterForUpdate, semester, context);
            await context.SaveChangesAsync();

            return await Get(semester.Id);
        }

        public async virtual Task Delete(PersonStudentSemester semesterForDelete, PersonStudent actualPart)
        {
            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonStudentSemesterErase, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, {semesterForDelete?.Period?.Year}/{semesterForDelete?.Period?.Year + 1} {(semesterForDelete?.Period?.Semester == Models.Enums.Semester.First ? "зимен" : "летен")} семестър");

            var lastSemester = actualPart.Semesters
                .OrderByDescending(e => e.Period.Year)
                .ThenByDescending(e => e.Period.Semester)
                .ThenByDescending(e => e.Id)
                .Where(e => e.Id != semesterForDelete.Id)
                .First();

            context.Entry(actualPart).State = EntityState.Modified;
            actualPart.StudentStatusId = lastSemester.StudentStatusId;
            actualPart.StudentEventId = lastSemester.StudentEventId;

            EntityService.Remove(semesterForDelete, context);
            await context.SaveChangesAsync();
        }

        private void UpdateEntityStatusEvent(PersonStudentSemester semester, PersonStudent actualPart, bool forceUpdate)
        {
            var lastSemester = actualPart.Semesters
                .OrderByDescending(e => e.Period.Year)
                .ThenByDescending(e => e.Period.Semester)
                .ThenByDescending(e => e.Id)
                .Where(e => e.Id != semester.Id)
                .FirstOrDefault();

            if (forceUpdate 
                || lastSemester == null 
                || semester.Period.Year > lastSemester.Period.Year 
                || (semester.Period.Year == lastSemester.Period.Year && semester.Period.Semester > lastSemester.Period.Semester)
                || (semester.Period.Year == lastSemester.Period.Year && semester.Period.Semester == lastSemester.Period.Semester && semester.Id > lastSemester.Id))
            {
                context.Entry(actualPart).State = EntityState.Modified;
                actualPart.StudentStatusId = semester.StudentStatusId;
                actualPart.StudentEventId = semester.StudentEventId;
            }
        }
    }
}
