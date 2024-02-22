using Rdpzsd.Models.Enums.RdpzsdImportFile;
using Rdpzsd.Models.Models.Base;
using System;

namespace Rdpzsd.Models.Models.RdpzsdImports.Base
{
    public abstract class RdpzsdImportHistory<TFile, TErrorFile> : EntityVersion
        where TFile : RdpzsdAttachedFile
        where TErrorFile : RdpzsdAttachedFile
    {
        public int RdpzsdImportId { get; set; }

        public ImportState State { get; set; }
        public DateTime CreateDate { get; set; }

        public TFile ImportFile { get; set; }
        public TErrorFile ErrorFile { get; set; }
    }
}
