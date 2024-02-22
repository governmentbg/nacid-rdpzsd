using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Nomenclatures.Base;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures.Others
{
    public class EducationFeeType: Nomenclature, IIncludeAll<EducationFeeType>
    {
        public string OldCode { get; set; }

        public ICollection<AdmissionReasonEducationFee> AdmissionReasonEducationFees { get; set; }

        public EducationFeeType()
        {
            AdmissionReasonEducationFees = new List<AdmissionReasonEducationFee>();
        }

        public IQueryable<EducationFeeType> IncludeAll(IQueryable<EducationFeeType> query)
        {
            return query;
        }
    }
}
