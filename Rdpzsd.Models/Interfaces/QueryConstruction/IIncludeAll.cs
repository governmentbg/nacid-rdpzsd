using System.Linq;

namespace Rdpzsd.Models.Interfaces
{
	public interface IIncludeAll<TEntity>
		where TEntity : IEntityVersion
	{
		IQueryable<TEntity> IncludeAll(IQueryable<TEntity> query);
	}
}
