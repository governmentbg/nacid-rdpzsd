using Infrastructure.User;
using Rdpzsd.Import.Search.Base;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.RdpzsdImports;
using Rdpzsd.Models.Models.RdpzsdImports;

namespace Rdpzsd.Import.Search
{
    public class PersonImportSearchService : BaseRdpzsdImportSearchService<PersonImport, PersonImportFilterDto>
    {
        public PersonImportSearchService(
            RdpzsdDbContext context,
            UserContext userContext

        ) : base(context, userContext)
        {
        }
    }
}
