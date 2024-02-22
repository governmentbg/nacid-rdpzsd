using Infrastructure.User;
using System.Linq;

namespace Rdpzsd.Models.Interfaces
{
	public interface IWhere<TEntity>
		where TEntity : IEntityVersion
	{
		IQueryable<TEntity> WhereBuilder(IQueryable<TEntity> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext);
	}
}
