using Infrastructure.DomainValidation;
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
    public class PersonStudent : BasePersonStudentDoctoral<PersonStudentInfo, PersonStudentSemester>,
        IMultiPart<PersonStudent, PersonLot, PersonStudentHistory>
    {
		public string FacultyNumber { get; set; }
		[Skip]
        public PersonStudent PePart { get; set; }

        public int LotId { get; set; }
        [Skip]
        public PersonLot Lot { get; set; }

        #region Graduation
        public StudentStickerState StickerState { get; set; } = StudentStickerState.None;
        public int? StickerYear { get; set; }

		[SkipUpdate]
		public List<PersonStudentStickerNote> StickerNotes { get; set; } = new List<PersonStudentStickerNote>();
        [SkipUpdate]
        public PersonStudentDiploma Diploma { get; set; }
        [SkipUpdate]
		public List<PersonStudentDuplicateDiploma> DuplicateDiplomas { get; set; } = new List<PersonStudentDuplicateDiploma>();
		[SkipUpdate]
		public List<PersonStudentProtocol> Protocols { get; set; } = new List<PersonStudentProtocol>();
		#endregion

		[Skip]
        public List<PersonStudentHistory> Histories { get; set; } = new List<PersonStudentHistory>();

		public PersonStudentPeRecognitionDocument PeRecognitionDocument { get; set; }
		public IQueryable<PersonStudent> IncludeAll(IQueryable<PersonStudent> query)
        {
			return query
				 .Include(e => e.Institution)
				 .Include(e => e.Subordinate)
				 .Include(e => e.InstitutionSpeciality.Speciality.EducationalQualification)
				 .Include(e => e.InstitutionSpeciality.Speciality.ResearchArea)
				 .Include(e => e.InstitutionSpeciality.EducationalForm)
				 .Include(e => e.InstitutionSpeciality.InstitutionSpecialityJointSpecialities)
				 .ThenInclude(js => js.Institution)
				 .Include(e => e.InstitutionSpeciality.InstitutionSpecialityJointSpecialities)
				 .ThenInclude(js => js.InstitutionByParent)
				 .Include(e => e.AdmissionReason)
				 .Include(e => e.StudentEvent)
				 .Include(e => e.StudentStatus)
				 .Include(e => e.PePart.Institution)
				 .Include(e => e.PePart.Subordinate)
				 .Include(e => e.PePart.InstitutionSpeciality.Speciality.EducationalQualification)
				 .Include(e => e.PePart.InstitutionSpeciality.Speciality.ResearchArea)
				 .Include(e => e.PePart.InstitutionSpeciality.EducationalForm)
				 .Include(e => e.Protocols)
				 .Include(e => e.StickerNotes)
				 .Include(e => e.Diploma.File)
				 .Include(e => e.DuplicateDiplomas)
					.ThenInclude(s => s.File)
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
					.ThenInclude(s => s.SemesterRelocatedFile)
				.Include(e => e.Semesters)
					.ThenInclude(s => s.SemesterInstitution);
		}	

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
			ValidateProperties(domainValidatorService);
		}
    }

	public class PersonStudentConfiguration : IEntityTypeConfiguration<PersonStudent>
	{
		public void Configure(EntityTypeBuilder<PersonStudent> builder)
		{
			builder.HasMany(e => e.Histories)
				.WithOne()
				.HasForeignKey(e => e.PartId);

			builder.HasMany(e => e.Semesters)
				.WithOne()
				.HasForeignKey(e => e.PartId);

			builder.HasMany(e => e.Protocols)
				.WithOne()
				.HasForeignKey(e => e.PartId);

			builder.HasMany(e => e.DuplicateDiplomas)
				.WithOne()
				.HasForeignKey(e => e.PartId);

			builder.HasMany(e => e.StickerNotes)
				.WithOne()
				.HasForeignKey(e => e.PartId);

			builder.Property(e => e.FacultyNumber)
				.HasMaxLength(50);

			builder.Property(e => e.PeInstitutionName)
				.HasMaxLength(100);

			builder.Property(e => e.PeSubordinateName)
				.HasMaxLength(100);

			builder.Property(e => e.PeDiplomaNumber)
				.HasMaxLength(250);

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
