using System.Collections.Generic;

namespace Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser
{
    public class PersonStudentDoctoralTxtDto
    {
        public List<PersonStudentTxtDto> personStudentTxtDtos { get; set; } = new List<PersonStudentTxtDto>();
        public List<PersonDoctoralTxtDto> personDoctoralTxtDtos { get; set; } = new List<PersonDoctoralTxtDto>();
    }
}
