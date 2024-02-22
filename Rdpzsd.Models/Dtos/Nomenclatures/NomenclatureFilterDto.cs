using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Interfaces.QueryConstruction;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures
{
    public class NomenclatureFilterDto<TEntity> : FilterDto, IWhere<TEntity>, IOrder<TEntity>
        where TEntity : Nomenclature
    {
        public string Name { get; set; }
        public string NameAlt { get; set; }

        public int? ExcludeId { get; set; }

		public string Alias { get; set; }
		public bool ExcludeAlias { get; set; } = true;

		public virtual IQueryable<TEntity> WhereBuilder(IQueryable<TEntity> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
		{
			if (!string.IsNullOrWhiteSpace(Name))
			{
				var textFilter = $"%{Name.Trim().ToLower()}%";
				query = query.Where(e => EF.Functions.ILike(e.Name.Trim().ToLower(), textFilter));
			}

			if (!string.IsNullOrWhiteSpace(NameAlt))
			{
				var textFilter = $"%{NameAlt.Trim().ToLower()}%";
				query = query.Where(e => EF.Functions.ILike(e.NameAlt.Trim().ToLower(), textFilter));
			}

			if (!string.IsNullOrWhiteSpace(Alias))
			{
				if (ExcludeAlias)
				{
					query = query.Where(e => e.Alias != Alias);
				}
				else
				{
					query = query.Where(e => e.Alias == Alias);
				}
			}

			if (IsActive.HasValue)
			{
				if (IsActive.Value)
				{
					query = query.Where(e => e.IsActive);
				}
				else
				{
					query = query.Where(e => !e.IsActive);
				}
			}

			if (ExcludeId.HasValue)
			{
				query = query.Where(e => e.Id != ExcludeId);
			}

			query = ConstructTextFilter(query);

			return query;
		}

		public virtual IQueryable<TEntity> ConstructTextFilter(IQueryable<TEntity> query)
		{
			if (!string.IsNullOrWhiteSpace(TextFilter))
			{
				var textFilter = $"{TextFilter.Trim().ToLower()}";
				query = query.Where(e => e.Name.Trim().ToLower().Contains(textFilter) || e.NameAlt.Trim().ToLower().Contains(textFilter));
			}

			return query;
		}

		public virtual IQueryable<TEntity> OrderBuilder(IQueryable<TEntity> query)
		{
			return query
				.OrderBy(e => e.ViewOrder)
				.ThenBy(e => e.Name)
				.ThenBy(e => e.Id);
		}
	}
}
