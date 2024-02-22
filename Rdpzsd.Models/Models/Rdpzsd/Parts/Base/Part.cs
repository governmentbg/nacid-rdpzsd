using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Rdpzsd.Base
{
    public abstract class Part<TPartInfo> : EntityVersion
        where TPartInfo : PartInfo
    {
        public TPartInfo PartInfo { get; set; }

        public PartState State { get; set; }
    }
}
