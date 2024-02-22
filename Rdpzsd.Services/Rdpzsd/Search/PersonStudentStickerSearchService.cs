using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentSticker;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Search
{
    public class PersonStudentStickerSearchService
    {
        private readonly RdpzsdDbContext context;
        private readonly UserContext userContext;

        public PersonStudentStickerSearchService(
           RdpzsdDbContext context,
           UserContext userContext
        )
        {
            this.context = context;
            this.userContext = userContext;
        }

        private IQueryable<int> GetInitialIdQuery(PersonStudentStickerFilterDto filter)
        {
            if (filter == null)
            {
                filter = new PersonStudentStickerFilterDto();
            }

            var query = context.Set<PersonStudent>()
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .AsQueryable();

            var queryIds = filter
                .WhereBuilder(query, userContext, context)
                .Select(e => e.Id)
                .Distinct();

            return queryIds;
        }

        public async virtual Task<SearchResultDto<PersonStudentStickerSearchDto>> GetAll(PersonStudentStickerFilterDto filter)
        {
            var queryIds = GetInitialIdQuery(filter);

            var loadIds = await queryIds
                .Take(filter.Limit)
                .ToListAsync();

            var result = context.PersonStudents
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Where(e => loadIds.Contains(e.Id));

            result = ConstructIncludes(result);

            var searchResult = new SearchResultDto<PersonStudentStickerSearchDto>
            {
                Result = await result.ToPersonStudentStickerSearchDto().ToListAsync()
            };

            return searchResult;
        }

        public async Task<int> GetCount(PersonStudentStickerFilterDto filter)
        {
            var queryIds = GetInitialIdQuery(filter);

            return await queryIds.CountAsync();
        }

        private IQueryable<PersonStudent> ConstructIncludes(IQueryable<PersonStudent> query)
        {
            query = query
                .Include(e => e.Lot.PersonBasic.BirthCountry)
                .Include(e => e.Lot.PersonBasic.BirthSettlement)
                .Include(e => e.Lot.PersonBasic.Citizenship)
                .Include(e => e.Lot.PersonBasic.SecondCitizenship)
                .Include(e => e.Subordinate)
                .Include(e => e.InstitutionSpeciality.Speciality)
                .Include(e => e.StudentStatus)
                .Include(e => e.StudentEvent)
                .Include(e => e.Diploma)
                .Include(e => e.DuplicateDiplomas)
                    .ThenInclude(s => s.File)
                .Include(e => e.StickerNotes)
                    .ThenInclude(s => s.Institution);

            return query;
        }
    }
}
