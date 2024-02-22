using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonStudentHistory : BasePersonStudentDoctoral<PersonStudentHistoryInfo, PersonStudentSemesterHistory>, IHistoryPart<PersonStudentHistory>
    {
        public int PartId { get; set; }
		public string FacultyNumber { get; set; }

		#region Graduation
		public StudentStickerState StickerState { get; set; } = StudentStickerState.None;
		public int? StickerYear { get; set; }

		[SkipUpdate]
		public PersonStudentDiplomaHistory Diploma { get; set; }
		[SkipUpdate]
		public List<PersonStudentDuplicateDiplomaHistory> DuplicateDiplomas { get; set; } = new List<PersonStudentDuplicateDiplomaHistory>();
		[SkipUpdate]
		public List<PersonStudentProtocolHistory> Protocols { get; set; } = new List<PersonStudentProtocolHistory>();
		#endregion

		public PersonStudentPeRecognitionDocumentHistory PeRecognitionDocument { get; set; }
		public IQueryable<PersonStudentHistory> IncludeAll(IQueryable<PersonStudentHistory> query)
        {
			return query
				.Include(e => e.Institution)
				.Include(e => e.Subordinate)
				.Include(e => e.InstitutionSpeciality.Speciality.EducationalQualification)
				.Include(e => e.InstitutionSpeciality.Speciality.ResearchArea)
				.Include(e => e.InstitutionSpeciality.EducationalForm)
				.Include(e => e.AdmissionReason)
				.Include(e => e.StudentEvent)
				.Include(e => e.StudentStatus)
				.Include(e => e.PeResearchArea)
				.Include(e => e.PeEducationalQualification)
				.Include(e => e.PeAcquiredForeignEducationalQualification)
				.Include(e => e.PeInstitution)
				.Include(e => e.PeSubordinate)
				.Include(e => e.PeCountry)
				.Include(e => e.PeInstitutionSpeciality.EducationalForm)
				.Include(e => e.PeInstitutionSpeciality.Speciality.EducationalQualification)
				.Include(e => e.PeInstitutionSpeciality.Speciality.ResearchArea)
				.Include(e => e.PeRecognitionDocument)
				.Include(e => e.Protocols)
				.Include(e => e.Diploma.File)
				.Include(e => e.DuplicateDiplomas)
					.ThenInclude(s => s.File)
				.Include(e => e.Semesters)
					.ThenInclude(s => s.Period)
				.Include(e => e.Semesters)
					.ThenInclude(s => s.StudentEvent)
				.Include(e => e.Semesters)
					.ThenInclude(s => s.StudentStatus)
				.Include(e => e.Semesters)
					.ThenInclude(s => s.EducationFeeType)
				.Include(e => e.Semesters)
					.ThenInclude(s => s.RelocatedFromPart.StudentEvent)
				.Include(e => e.Semesters)
					.ThenInclude(s => s.RelocatedFromPart.Institution)
				.Include(e => e.Semesters)
					.ThenInclude(s => s.RelocatedFromPart.Subordinate)
				.Include(e => e.Semesters)
					.ThenInclude(s => s.RelocatedFromPart.InstitutionSpeciality.Speciality)
				.Include(e => e.Semesters)
					.ThenInclude(s => s.SemesterRelocatedFile);
		}
    }

	public class PersonStudentHistoryConfiguration : IEntityTypeConfiguration<PersonStudentHistory>
	{
		public void Configure(EntityTypeBuilder<PersonStudentHistory> builder)
		{
			builder.HasMany(e => e.Semesters)
				.WithOne()
				.HasForeignKey(e => e.PartId);

			builder.HasMany(e => e.Protocols)
				.WithOne()
				.HasForeignKey(e => e.PartId);

			builder.HasMany(e => e.DuplicateDiplomas)
				.WithOne()
				.HasForeignKey(e => e.PartId);

			builder.Property(e => e.FacultyNumber)
				.HasMaxLength(50);
		}
	}
}
