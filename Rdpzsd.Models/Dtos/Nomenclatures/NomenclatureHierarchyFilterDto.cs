using Infrastructure.User;
using Rdpzsd.Models.Enums;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Nomenclatures
{
	public class NomenclatureHierarchyFilterDto<TEntity> : NomenclatureCodeFilterDto<TEntity>
		where TEntity : NomenclatureHierarchy
	{
		public Level? Level { get; set; }
		public bool ExcludeLevel { get; set; } = false;
		public int? RootId { get; set; }
		public int? ParentId { get; set; }

		public override IQueryable<TEntity> WhereBuilder(IQueryable<TEntity> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
		{
			if (Level.HasValue)
			{
				query = query.Where(e => ExcludeLevel ? e.Level != Level : e.Level == Level);
			}

			if (ParentId.HasValue)
			{
				query = query.Where(e => e.ParentId == ParentId);
			}

			if (RootId.HasValue)
			{
				query = query.Where(e => e.RootId == RootId);
			}

			return base.WhereBuilder(query, userContext, rdpzsdDbContext);
		}
	}
}
