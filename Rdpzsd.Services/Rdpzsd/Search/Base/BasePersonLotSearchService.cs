using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Dtos.Rdpzsd.Search;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Search.Base
{
    public class BasePersonLotSearchService<TFilter>
        where TFilter : FilterDto, IWhere<PersonLot>, new()
    {
        protected readonly RdpzsdDbContext context;
        protected readonly UserContext userContext;
        protected readonly ExcelProcessorService excelProcessorService;

        public BasePersonLotSearchService(
           RdpzsdDbContext context,
           UserContext userContext,
           ExcelProcessorService excelProcessorService
        )
        {
            this.context = context;
            this.userContext = userContext;
            this.excelProcessorService = excelProcessorService;
        }

        protected virtual IQueryable<int> GetInitialQuery(TFilter filter)
        {
            if (filter == null)
            {
                filter = new TFilter();
            }

            var query = context.PersonLots
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .AsQueryable();

            var queryIds = filter.WhereBuilder(query, userContext, context).Select(e => e.Id);

            return queryIds;
        }

        public async virtual Task<SearchResultDto<PersonSearchDto>> GetAll(TFilter filter)
        {
            var queryIds = GetInitialQuery(filter);

            var loadIds = await queryIds
                .Take(filter.Limit)
                .ToListAsync();

            var result = await context.PersonLots
                .AsNoTracking()
                .Include(e => e.PersonBasic.BirthCountry)
                .Include(e => e.PersonBasic.BirthSettlement)
                .Include(e => e.PersonBasic.Citizenship)
                .Include(e => e.PersonBasic.SecondCitizenship)
                .Include(e => e.PersonDoctorals)
                .ThenInclude(e => e.Institution)
                .Include(e => e.PersonDoctorals)
                .ThenInclude(e => e.StudentStatus)
                .Include(e => e.PersonDoctorals)
                .ThenInclude(e => e.InstitutionSpeciality.Speciality)
                .Include(e => e.PersonStudents)
                .ThenInclude(e => e.Institution)
                .Include(e => e.PersonStudents)
                .ThenInclude(e => e.StudentStatus)
                .Include(e => e.PersonStudents)
                .ThenInclude(e => e.InstitutionSpeciality.Speciality)
                .Include(e => e.PersonSecondary)
                .OrderByDescending(e => e.Id)
                .Where(e => loadIds.Contains(e.Id))
                .ToPersonSearchDto()
                .ToListAsync();

            var searchResult = new SearchResultDto<PersonSearchDto>
            {
                Result = result
            };

            return searchResult;
        }

        public async virtual Task<int> GetCount(TFilter filter)
        {
            var queryIds = GetInitialQuery(filter);

            return await queryIds.CountAsync();
        }

        public async virtual Task<SearchResultDto<PersonExportDto>> GetAllExport(TFilter filter)
        {
            var queryIds = await GetInitialQuery(filter).ToListAsync();

            var result = await context.PersonLots
                .AsNoTracking()
                .Include(e => e.PersonBasic.BirthCountry)
                .Include(e => e.PersonBasic.BirthSettlement)
                .Include(e => e.PersonBasic.BirthMunicipality)
                .Include(e => e.PersonBasic.BirthDistrict)
                .Include(e => e.PersonBasic.ResidenceCountry)
                .Include(e => e.PersonBasic.ResidenceSettlement)
                .Include(e => e.PersonBasic.ResidenceMunicipality)
                .Include(e => e.PersonBasic.ResidenceDistrict)
                .Include(e => e.PersonBasic.Citizenship)
                .Include(e => e.PersonBasic.SecondCitizenship)
                .Include(e => e.PersonDoctorals)
                .ThenInclude(e => e.StudentStatus)
                .Include(e => e.PersonStudents)
                .ThenInclude(e => e.StudentStatus)
                .Include(e => e.PersonSecondary)
                .OrderByDescending(e => e.Id)
                .Where(e => queryIds.Contains(e.Id))
                .ToPersonExportDto()
                .ToListAsync();

            var searchResult = new SearchResultDto<PersonExportDto>
            {
                Result = result
            };

            return searchResult;
        }

        public async virtual Task<MemoryStream> ExportExcel(TFilter filter)
        {
            filter.GetAllData = true;
            await GetAllExport(filter);

            return new MemoryStream();
        }
    }
}
