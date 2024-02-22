using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Interfaces
{
    public interface IMultiPart<TEntity, TLot, THistory> : ISinglePart<TEntity, TLot, THistory>
        where TLot : EntityVersion
        where TEntity : EntityVersion
    {
        int LotId { get; set; }
    }
}
