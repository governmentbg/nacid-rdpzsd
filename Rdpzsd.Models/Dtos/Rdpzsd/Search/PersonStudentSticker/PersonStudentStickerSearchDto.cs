using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.StudentStatus;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentSticker
{
    public class PersonStudentStickerSearchDto : PersonSearchDto
    {
        public int PartId { get; set; }

        public int? DuplicateDiplomaId { get; set; } = null;
        public bool IsDuplicate { get; set; } = false;

        public int? StickerYear { get; set; }
        public StudentStickerState StickerState { get; set; }

        public Institution Subordinate { get; set; }
        public string Speciality { get; set; }
        public string SpecialityAlt { get; set; }

        public StudentStatus StudentStatus { get; set; }
        public StudentEvent StudentEvent { get; set; }

        public List<PersonStudentStickerNote> StickerNotes { get; set; } = new List<PersonStudentStickerNote>();

        public PersonStudentStickerSearchDto(PersonStudent personStudent) : base(personStudent)
        {
            PartId = personStudent.Id;

            var duplicateDiploma = personStudent.DuplicateDiplomas.FirstOrDefault(e => e.IsValid && (e.DuplicateStickerState != StudentStickerState.Recieved || e.File == null));
            if (duplicateDiploma != null)
            {
                StickerYear = duplicateDiploma.DuplicateStickerYear;
                StickerState = duplicateDiploma.DuplicateStickerState;
                DuplicateDiplomaId = duplicateDiploma.Id;
                IsDuplicate = true;
            }
            else
            {
                StickerYear = personStudent.StickerYear;
                StickerState = personStudent.StickerState;
                DuplicateDiplomaId = null;
                IsDuplicate = false;
            }

            Subordinate = personStudent.Subordinate;
            Speciality = personStudent.InstitutionSpeciality?.Speciality?.Name;
            SpecialityAlt = personStudent.InstitutionSpeciality?.Speciality?.NameAlt;

            StudentStatus = personStudent.StudentStatus;
            StudentEvent = personStudent.StudentEvent;

            StickerNotes = personStudent.StickerNotes;
        }
    }

    public static class PersonStudentStickerSearchDtoExtensions
    {
        public static IQueryable<PersonStudentStickerSearchDto> ToPersonStudentStickerSearchDto(this IQueryable<PersonStudent> personStudents)
        {
            return personStudents.Select(e => new PersonStudentStickerSearchDto(e));
        }
    }
}
