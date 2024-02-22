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
	public class PersonDoctoralService : BaseMultiPartService<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralHistory, PersonDoctoralHistoryInfo, PersonLot, PersonDoctoralFilterDto>
	{
		public PersonDoctoralService(
			RdpzsdDbContext context,
			DomainValidatorService domainValidatorService,
			UserContext userContext,
			PersonLotService personLotService,
            BaseHistoryPartService<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralHistory, PersonDoctoralHistoryInfo, PersonLot> baseHistoryPartService
        ) : base(context, domainValidatorService, userContext, personLotService, baseHistoryPartService)
		{
		}

        public async override Task<List<PersonDoctoral>> GetAll(int lotId)
        {
            var parts = await new PersonDoctoral().IncludeAll(context.Set<PersonDoctoral>().AsQueryable())
                        .AsNoTracking()
                        .Where(e => e.LotId == lotId && e.State != PartState.Erased)
                        .OrderByDescending(e => e.Id)
                        .ToListAsync();

            foreach (var part in parts)
            {
                part.Semesters = part.Semesters
                    .OrderByDescending(e => e.ProtocolDate.Date)
                    .ThenByDescending(e => e.Id)
                    .ToList();
            }

            return parts;
        }

        public async override Task<PersonDoctoral> Get(int id)
        {
            var part = await new PersonDoctoral().IncludeAll(context.Set<PersonDoctoral>().AsQueryable())
                        .AsNoTracking()
                        .Include(e => e.PartInfo)
                        .SingleOrDefaultAsync(e => e.Id == id);

            part.Semesters = part.Semesters
                .OrderByDescending(e => e.ProtocolDate.Date)
                .ThenByDescending(e => e.Id)
                .ToList();

            return part;
        }
    }
}
