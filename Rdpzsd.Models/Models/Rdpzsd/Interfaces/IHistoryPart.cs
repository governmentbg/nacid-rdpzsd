using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Interfaces
{
    public interface IHistoryPart<TEntity> : IIncludeAll<TEntity>
        where TEntity : EntityVersion
    {
        int PartId { get; set; }
    }
}
