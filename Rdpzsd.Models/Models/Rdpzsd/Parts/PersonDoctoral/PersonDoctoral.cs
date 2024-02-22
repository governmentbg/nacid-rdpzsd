using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
	public class PersonDoctoral : BasePersonStudentDoctoral<PersonDoctoralInfo, PersonDoctoralSemester>,
		IMultiPart<PersonDoctoral, PersonLot, PersonDoctoralHistory>
	{
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		[Skip]
		public PersonStudent PePart { get; set; }

		public int LotId { get; set; }
		[Skip]
		public PersonLot Lot { get; set; }
		[Skip]
		public List<PersonDoctoralHistory> Histories { get; set; } = new List<PersonDoctoralHistory>();

		public PersonDoctoralPeRecognitionDocument PeRecognitionDocument { get; set; }
		public IQueryable<PersonDoctoral> IncludeAll(IQueryable<PersonDoctoral> query)
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
				 .Include(e => e.PePart.Institution)
				 .Include(e => e.PePart.Subordinate)
				 .Include(e => e.PePart.InstitutionSpeciality.Speciality.EducationalQualification)
				 .Include(e => e.PePart.InstitutionSpeciality.Speciality.ResearchArea)
				 .Include(e => e.PePart.InstitutionSpeciality.EducationalForm)
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

		public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
		{
            ValidateProperties(domainValidatorService);
		}
	}

	public class PersonDoctoralConfiguration : IEntityTypeConfiguration<PersonDoctoral>
	{
		public void Configure(EntityTypeBuilder<PersonDoctoral> builder)
		{
			builder.HasMany(e => e.Histories)
				.WithOne()
				.HasForeignKey(e => e.PartId);

			builder.HasMany(e => e.Semesters)
				.WithOne()
				.HasForeignKey(e => e.PartId);

			builder.Property(e => e.PeDiplomaNumber)
				.HasMaxLength(25);

			builder.Property(e => e.PeInstitutionName)
				.HasMaxLength(100);

			builder.Property(e => e.PeSubordinateName)
				.HasMaxLength(100);

			builder.Property(e => e.PeSpecialityName)
				.HasMaxLength(100);

			builder.Property(e => e.PeAcquiredSpeciality)
				.HasMaxLength(100);

			builder.Property(e => e.PeRecognizedSpeciality)
				.HasMaxLength(100);

			builder.Property(e => e.PeRecognitionNumber)
				.HasMaxLength(30);
		}
	}
}
