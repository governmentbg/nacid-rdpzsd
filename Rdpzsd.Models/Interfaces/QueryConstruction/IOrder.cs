using System.Linq;

namespace Rdpzsd.Models.Interfaces.QueryConstruction
{
    public interface IOrder<TEntity>
        where TEntity: IEntityVersion
    {
        IQueryable<TEntity> OrderBuilder(IQueryable<TEntity> query);
    }
}
