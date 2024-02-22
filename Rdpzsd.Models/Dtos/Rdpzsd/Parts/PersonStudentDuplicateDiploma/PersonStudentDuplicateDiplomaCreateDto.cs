using Rdpzsd.Models.Dtos.Rdpzsd.Parts.PersonStudentSticker;
using Rdpzsd.Models.Models.Rdpzsd.Parts;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Parts
{
    public class PersonStudentDuplicateDiplomaCreateDto
    {
        public StickerDto StickerDto { get; set; }
        public PersonStudentDuplicateDiploma NewDuplicateDiploma { get; set; }
    }
}
