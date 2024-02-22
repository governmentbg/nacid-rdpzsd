using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Services.EntityServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Parts.PersonStudentGraduation
{
    public class PersonStudentDiplomaService
    {
        private readonly RdpzsdDbContext context;
        private readonly DomainValidatorService domainValidatorService;
        private readonly PersonStudentService personStudentService;
        private readonly PersonLotService personLotService;

        public PersonStudentDiplomaService(
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

        public async Task<PersonStudentDiploma> Get(int diplomaId)
        {
            var diploma = await context.Set<PersonStudentDiploma>()
                        .AsNoTracking()
                        .Include(e => e.File)
                        .SingleAsync(e => e.Id == diplomaId);

            return diploma;
        }

        public async Task<PersonStudentDiploma> Create(PersonStudentDiploma diploma, PersonStudent actualPart)
        {
            diploma.ValidateProperties(context, domainValidatorService);

            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonStudentDiplomaAdd, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, номер на диплома: {diploma?.DiplomaNumber}");

            EntityService.ClearSkipProperties(diploma);

            await context.PersonStudentDiplomas.AddAsync(diploma);

            context.Entry(actualPart).State = EntityState.Modified;
            actualPart.StudentEventId = StudentEventConstants.GraduatedWithDiplomaId;
            actualPart.StudentStatusId = StudentStatusConstants.GraduatedId;

            //Update Diploma Information From PE
            await UpdatePersonStudentDoctoralDiplomaInformation(diploma, actualPart);

            await context.SaveChangesAsync();

            return await Get(diploma.Id);
        }

        public async Task<PersonStudentDiploma> Update(PersonStudentDiploma diploma, PersonStudent actualPart)
        {
            diploma.ValidateProperties(context, domainValidatorService);

            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);
            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonStudentDiplomaEdit, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, номер на диплома: {diploma?.DiplomaNumber}");

            var diplomaForUpdate = await context.PersonStudentDiplomas
                        .Include(e => e.File)
                        .SingleAsync(e => e.Id == diploma.Id && e.IsValid);

            EntityService.Update(diplomaForUpdate, diploma, context);

            //Update Diploma Information From PE
            await UpdatePersonStudentDoctoralDiplomaInformation(diploma, actualPart);

            await context.SaveChangesAsync();

            return await Get(diploma.Id);
        }

        public async Task<PersonStudentDiploma> Invalid(PersonStudent actualPart)
        {
            var createDate = DateTime.Now;

            await personStudentService.CreateHistory(actualPart);
            personStudentService.UpdatePartInfo(actualPart, createDate);

            var diplomaForUpdate = await context.PersonStudentDiplomas
                        .Include(e => e.File)
                        .SingleAsync(e => e.Id == actualPart.Id && e.IsValid);
            diplomaForUpdate.IsValid = false;

            await personLotService
                .AddPersonLotAction(actualPart.LotId, createDate, PersonLotActionType.PersonStudentDiplomaInvalid, $"{actualPart?.InstitutionSpeciality?.Speciality?.Code}, номер на диплома: {diplomaForUpdate?.DiplomaNumber}");

            await context.SaveChangesAsync();

            return diplomaForUpdate;
        }
        private async Task UpdatePersonStudentDoctoralDiplomaInformation(PersonStudentDiploma diploma, PersonStudent actualPart)
		{
            var personStudentsDiplomasToUpdate = await context.PersonStudents.Where(s => s.PePartId == actualPart.Id && (s.PeHighSchoolType.HasValue && s.PeHighSchoolType == PreviousHighSchoolEducationType.FromRegister)).ToListAsync();

            var personDoctoralsDiplomasToUpdate = await context.PersonDoctorals.Where(s => s.PePartId == actualPart.Id && (s.PeHighSchoolType.HasValue && s.PeHighSchoolType == PreviousHighSchoolEducationType.FromRegister)).ToListAsync();

            if(personStudentsDiplomasToUpdate.Any())
			{
                personStudentsDiplomasToUpdate.ForEach(d =>
                {
                    d.PeDiplomaNumber = diploma.DiplomaNumber;
                    d.PeDiplomaDate = diploma.DiplomaDate;
                });
			}

            if(personDoctoralsDiplomasToUpdate.Any())
			{
                personDoctoralsDiplomasToUpdate.ForEach(d =>
                {
                    d.PeDiplomaNumber = diploma.DiplomaNumber;
                    d.PeDiplomaDate = diploma.DiplomaDate;
                });
            }
		}
    }
}
