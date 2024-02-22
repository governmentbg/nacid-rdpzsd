using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Nomenclatures.Others
{
    public class AdmissionReasonEducationFee : EntityVersion
    {
        public int AdmissionReasonId { get; set; }
        [Skip]
        public AdmissionReason AdmissionReason { get; set; }
        public int EducationFeeTypeId { get; set; }
        [Skip]
        public EducationFeeType EducationFeeType { get; set; }
    }
}

