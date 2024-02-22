using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Rdpzsd;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Lot
{
    public class PersonLotDto
    {
        public PersonLot PersonLot { get; set; }
        public string PersonBasicNames { get; set; }
        public string PersonBasicNamesAlt { get; set; }
        public Country Citizenship { get; set; }
        public Country SecondCitizenship { get; set; }
        public bool HasActualStudentOrDoctoral { get; set; }
        public bool? PersonSecondaryFromRso { get; set; }
    }
}
