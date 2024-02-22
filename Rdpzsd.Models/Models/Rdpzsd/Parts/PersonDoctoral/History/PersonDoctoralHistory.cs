using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using System;
using System.Linq;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.History
{
    public class PersonDoctoralHistory : BasePersonStudentDoctoral<PersonDoctoralHistoryInfo, PersonDoctoralSemesterHistory>, IHistoryPart<PersonDoctoralHistory>
	{
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public int PartId { get; set; }

		public PersonDoctoralPeRecognitionDocumentHistory PeRecognitionDocument { get; set; }
		public IQueryable<PersonDoctoralHistory> IncludeAll(IQueryable<PersonDoctoralHistory> query)
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

	public class PersonDoctoralHistoryConfiguration : IEntityTypeConfiguration<PersonDoctoralHistory>
	{
		public void Configure(EntityTypeBuilder<PersonDoctoralHistory> builder)
		{
			builder.HasMany(e => e.Semesters)
				.WithOne()
				.HasForeignKey(e => e.PartId);
		}
	}
}
