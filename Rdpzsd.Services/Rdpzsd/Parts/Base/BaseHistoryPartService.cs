using Rdpzsd.Models;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Rdpzsd.Base;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Services.EntityServices;
using System;
using System.Threading.Tasks;

namespace Rdpzsd.Services.Rdpzsd.Parts.Base
{
    public class BaseHistoryPartService<TPart, TPartInfo, THistory, THistoryInfo, TLot>
        where TPart : Part<TPartInfo>, ISinglePart<TPart, TLot, THistory>, new()
        where TPartInfo : PartInfo, new()
        where THistory : Part<THistoryInfo>, IHistoryPart<THistory>, new()
        where THistoryInfo : PartInfo, new()
        where TLot : EntityVersion
    {
        public async Task CreateHistory(TPart actualPart, RdpzsdDbContext context)
        {
            var partHistory = Activator.CreateInstance(typeof(THistory)) as THistory;
            EntityService.CloneProperties(actualPart, partHistory);
            partHistory.PartId = actualPart.Id;
            await context.Set<THistory>().AddAsync(partHistory);
        }
    }
}
