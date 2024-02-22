using Infrastructure.Integrations.EmsIntegration;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Search;
using Rdpzsd.Models.Enums.Rdpzsd;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Search
{
    public class PersonSearchApprovalService
    {
		private readonly RdpzsdDbContext context;
		private readonly UserContext userContext;
		private readonly EmsIntegrationService emsIntegrationService;

		public PersonSearchApprovalService(
			RdpzsdDbContext context,
			UserContext userContext,
			EmsIntegrationService emsIntegrationService
		)
		{
			this.context = context;
			this.userContext = userContext;
			this.emsIntegrationService = emsIntegrationService;
		}

		private IQueryable<int> GetInitialQuery(PersonApprovalFilterDto filter)
		{
			if (filter == null)
			{
				filter = new PersonApprovalFilterDto();
			}

			var query = context.PersonLots
				.AsNoTracking()
				.OrderByDescending(e => e.State == LotState.PendingApproval)
				.ThenByDescending(e => e.Id)
				.AsQueryable();

			var queryIds = filter.WhereBuilder(query, userContext, context).Select(e => e.Id);

			return queryIds;
		}

		public async Task<SearchResultDto<PersonApprovalSearchDto>> GetAll(PersonApprovalFilterDto filter)
		{
			var queryIds = GetInitialQuery(filter);

			var loadIds = await queryIds
				.Take(filter.Limit)
				.ToListAsync();

			var result = await context.PersonLots
				.AsNoTracking()
				.Include(e => e.PersonBasic.BirthCountry)
				.Include(e => e.CreateInstitution)
				.Include(e => e.CreateSubordinate)
				.Where(e => loadIds.Contains(e.Id))
				.OrderByDescending(e => e.State == LotState.PendingApproval)
				.ThenByDescending(e => e.Id)
				.ToListAsync();

			var userIds = result
				.Select(e => e.CreateUserId)
				.Distinct()
				.ToList();

			var emsUsers = await emsIntegrationService.GetUsersInfo(userIds);
			var searchResult = new SearchResultDto<PersonApprovalSearchDto>
			{
				Result = result.ToPersonApprovalSearchDto(emsUsers)
			};

			return searchResult;
		}

		public async virtual Task<int> GetCount(PersonApprovalFilterDto filter)
		{
			var queryIds = GetInitialQuery(filter);

			return await queryIds.CountAsync();
		}
	}
}
