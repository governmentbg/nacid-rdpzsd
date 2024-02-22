using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Nomenclatures.Others
{
    public class AdmissionReasonEducationFeeHistory: EntityVersion
    {
        public int AdmissionReasonId { get; set; }
        public int EducationFeeTypeId { get; set; }
        public string EducationFeeTypeName { get; set; }
    }
}
