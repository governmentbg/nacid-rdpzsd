using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using System.Collections.Generic;

namespace Rdpzsd.Models.Models.Rdpzsd.Interfaces
{
    public interface ISinglePart<TEntity, TLot, THistory> : IIncludeAll<TEntity>, IValidate
        where TLot : EntityVersion
        where TEntity : EntityVersion
    {
        TLot Lot { get; set; }
        List<THistory> Histories { get; set; }
    }
}
