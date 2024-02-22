using Rdpzsd.Models.Models.Base;
using System;
using System.Collections.Generic;

namespace Rdpzsd.Models.Models.Nomenclatures.Others
{
    public class AdmissionReasonHistory: EntityVersion
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int AdmissionReasonId { get; set; }
        public string Name { get; set; }
        public string NameAlt { get; set; }
        public string Description { get; set; }

        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }
        public bool IsActive { get; set; }
        public DateTime ChangeDate { get; set; }

        public ICollection<AdmissionReasonEducationFeeHistory> AdmissionReasonEducationFeeHistories { get; set; }

        public AdmissionReasonHistory()
        {
            AdmissionReasonEducationFeeHistories = new List<AdmissionReasonEducationFeeHistory>();
        }
    }
}
