using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Base;

namespace Rdpzsd.Models.Models.Nomenclatures.Others
{
    public class AdmissionReasonCitizenship: EntityVersion
    {
        public int AdmissionReasonId { get; set; }
        public bool? ExcludeCountry { get; set; } = false;
        public int CountryId { get; set; }
        [Skip]
        public Country Country { get; set; }
    }
}
