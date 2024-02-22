using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Nomenclatures;
using System;

namespace Rdpzsd.Models.Models.Rdpzsd.Base
{
    public abstract class PartInfo : EntityVersion
    {
        public int UserId { get; set; }
        public string UserFullname { get; set; }
        public DateTime ActionDate { get; set; }

        public int? InstitutionId { get; set; }
        [Skip]
        public Institution Institution { get; set; }
        public int? SubordinateId { get; set; }
        [Skip]
        public Institution Subordinate { get; set; }
    }
}
