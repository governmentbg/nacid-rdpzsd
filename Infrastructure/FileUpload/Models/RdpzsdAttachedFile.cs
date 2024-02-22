using System;

namespace Rdpzsd.Models.Models.Base
{
    public abstract class RdpzsdAttachedFile : EntityVersion
    {
        public Guid Key { get; set; }
        public string Hash { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public int DbId { get; set; }
    }
}
