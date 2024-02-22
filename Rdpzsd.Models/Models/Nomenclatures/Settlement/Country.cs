using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures
{
    public class Country : NomenclatureCode, IIncludeAll<Country>
	{
        public bool EuCountry { get; set; }
        public bool EeaCountry { get; set; }

        public IQueryable<Country> IncludeAll(IQueryable<Country> query)
		{
			return query;
		}
	}
}
