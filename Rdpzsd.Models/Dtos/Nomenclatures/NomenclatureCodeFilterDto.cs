using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures
{
	public class NomenclatureCodeFilterDto<TEntity> : NomenclatureFilterDto<TEntity>
		where TEntity : NomenclatureCode
	{
		public string Code { get; set; }

        public string ExcludeCode { get; set; }

        public override IQueryable<TEntity> WhereBuilder(IQueryable<TEntity> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
		{
			if (!string.IsNullOrWhiteSpace(Code))
			{
				var textFilter = $"%{Code.Trim().ToLower()}%";
				query = query.Where(e => EF.Functions.ILike(e.Code.Trim().ToLower(), textFilter));
			}

            if (!string.IsNullOrWhiteSpace(ExcludeCode))
            {
				query = query.Where(e => e.Code != ExcludeCode);
            }

			return base.WhereBuilder(query, userContext, rdpzsdDbContext);
		}

		public override IQueryable<TEntity> ConstructTextFilter(IQueryable<TEntity> query)
		{
			if (!string.IsNullOrWhiteSpace(TextFilter))
			{
				var textFilter = $"{TextFilter.Trim().ToLower()}";
				query = query.Where(e => (e.Code.Trim().ToLower() + " " + e.Name.Trim().ToLower()).Contains(textFilter) 
					|| (e.Code.Trim().ToLower() + " " + e.NameAlt.Trim().ToLower()).Contains(textFilter));
			}

			return query;
		}
	}
}
