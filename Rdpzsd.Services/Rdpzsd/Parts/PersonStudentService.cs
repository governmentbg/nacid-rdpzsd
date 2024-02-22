using Infrastructure.DomainValidation;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Rdpzsd.Parts;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using Rdpzsd.Services.Rdpzsd.Parts.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Parts
{
    public class PersonStudentService : BaseMultiPartService<PersonStudent, PersonStudentInfo, PersonStudentHistory, PersonStudentHistoryInfo, PersonLot, PersonStudentFilterDto>
    {
        public PersonStudentService(
            RdpzsdDbContext context,
            DomainValidatorService domainValidatorService,
            UserContext userContext,
            PersonLotService personLotService,
            BaseHistoryPartService<PersonStudent, PersonStudentInfo, PersonStudentHistory, PersonStudentHistoryInfo, PersonLot> baseHistoryPartService
        ) : base(context, domainValidatorService, userContext, personLotService, baseHistoryPartService)
        {
        }

        public async override Task<List<PersonStudent>> GetAll(int lotId)
        {
            var parts = await new PersonStudent().IncludeAll(context.Set<PersonStudent>().AsQueryable())
                        .AsNoTracking()
                        .Where(e => e.LotId == lotId && e.State != PartState.Erased)
                        .OrderByDescending(e => e.Id)
                        .ToListAsync();

            foreach (var part in parts)
            {
                part.Semesters = part.Semesters
                    .OrderByDescending(e => e.Period.Year)
                    .ThenByDescending(e => e.Period.Semester)
                    .ThenByDescending(e => e.Id)
                    .ToList();
            }

            return parts;
        }

        public async override Task<PersonStudent> Get(int id)
        {
            var part = await new PersonStudent().IncludeAll(context.Set<PersonStudent>().AsQueryable())
                        .AsNoTracking()
                        .Include(e => e.PartInfo)
                        .SingleOrDefaultAsync(e => e.Id == id);

            part.Semesters = part.Semesters
                .OrderByDescending(e => e.Period.Year)
                .ThenByDescending(e => e.Period.Semester)
                .ThenByDescending(e => e.Id)
                .ToList();

            return part;
        }
    }
}
