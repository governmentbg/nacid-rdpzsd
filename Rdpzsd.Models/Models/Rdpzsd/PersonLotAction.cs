using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Nomenclatures;
using System;

namespace Rdpzsd.Models.Models.Rdpzsd
{
    public class PersonLotAction : EntityVersion
    {
        public int LotId { get; set; }

        public int UserId { get; set; }
        public string UserFullname { get; set; }
        public DateTime ActionDate { get; set; }

        public int? InstitutionId { get; set; }
        [Skip]
        public Institution Institution { get; set; }
        public int? SubordinateId { get; set; }
        [Skip]
        public Institution Subordinate { get; set; }

        public PersonLotActionType ActionType { get; set; }

        public string Note { get; set; }
    }
}
