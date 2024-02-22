using Infrastructure.User;
using Rdpzsd.Import.Search.Base;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports;

namespace Rdpzsd.Import.Services.Search
{
    public class SpecialityImportSearchService : BaseRdpzsdImportSearchService<SpecialityImport, SpecialityImportFilterDto>
    {
        public SpecialityImportSearchService(
            RdpzsdDbContext context,
            UserContext userContext

        ) : base(context, userContext)
        {
        }
    }
}
