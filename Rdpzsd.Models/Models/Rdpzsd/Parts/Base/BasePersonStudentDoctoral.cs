using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Nomenclatures.Others;
using Rdpzsd.Models.Models.Nomenclatures.StudentStatus;
using Rdpzsd.Models.Models.Rdpzsd.Base;
using System;
using System.Collections.Generic;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.Base
{
    public abstract class BasePersonStudentDoctoral<TPartInfo, TSemester> : Part<TPartInfo>
        where TPartInfo : PartInfo
        where TSemester : BasePersonSemester, new()
	{
		public int InstitutionId { get; set; }
		[Skip]
		public Institution Institution { get; set; }

		public int? SubordinateId { get; set; }
		[Skip]
		public Institution Subordinate { get; set; }

		public int InstitutionSpecialityId { get; set; }
		[Skip]
		public InstitutionSpeciality InstitutionSpeciality { get; set; }
		public int StudentStatusId { get; set; }
		[Skip]
		public StudentStatus StudentStatus { get; set; }
		public int StudentEventId { get; set; }
		[Skip]
		public StudentEvent StudentEvent { get; set; }

		public int AdmissionReasonId { get; set; }
		[Skip]
		public AdmissionReason AdmissionReason { get; set; }

		[SkipUpdate]
		public List<TSemester> Semesters { get; set; } = new List<TSemester>();

		#region PreviousEducation
		public PreviousEducationType PeType { get; set; }
		public PreviousHighSchoolEducationType? PeHighSchoolType { get; set; }

		public string PeDiplomaNumber { get; set; }
		public DateTime? PeDiplomaDate { get; set; }
		public int? PeResearchAreaId { get; set; }
		[Skip]
		public ResearchArea PeResearchArea { get; set; }
		public int? PeEducationalQualificationId { get; set; }
		[Skip]
		public EducationalQualification PeEducationalQualification { get; set; }

		// FromRegister
		public int? PePartId { get; set; }

		// FromRegister, MissingInRegister
		public int? PeInstitutionId { get; set; }
		[Skip]
		public Institution PeInstitution { get; set; }
		public int? PeSubordinateId { get; set; }
		[Skip]
		public Institution PeSubordinate { get; set; }
		public int? PeInstitutionSpecialityId { get; set; }

		[Skip]
		public InstitutionSpeciality PeInstitutionSpeciality { get; set; }
        public bool? PeSpecialityMissingInRegister { get; set; }

        // Abroad, ClosedInstitution
        public string PeInstitutionName { get; set; }
		public string PeSubordinateName { get; set; }

		// ClosedInstitution
		public string PeSpecialityName { get; set; }

		// Abroad
		public int? PeCountryId { get; set; }
		[Skip]
		public Country PeCountry { get; set; }
		public string PeRecognizedSpeciality { get; set; }
		public string PeAcquiredSpeciality { get; set; }
		public int? PeAcquiredForeignEducationalQualificationId { get; set; }
		[Skip]
		public EducationalQualification PeAcquiredForeignEducationalQualification { get; set; }

		public string PeRecognitionNumber { get; set; }
		public DateTime? PeRecognitionDate { get; set; }
		#endregion

		protected void ValidateProperties(DomainValidatorService domainValidatorService)
        {
			if (PeType == PreviousEducationType.HighSchool)
			{
				switch (PeHighSchoolType)
				{
					case PreviousHighSchoolEducationType.FromRegister:
						ValidateFromRegister(domainValidatorService);
						break;
					case PreviousHighSchoolEducationType.MissingInRegister:
						ValidateMissingInRegister(domainValidatorService);
						break;
					case PreviousHighSchoolEducationType.Abroad:
						ValidateAbroad(domainValidatorService);
						break;
					case PreviousHighSchoolEducationType.ClosedInstitution:
						ValidateClosedInstitution(domainValidatorService);
						break;
					default:
						domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_MissingPreviousEducationType);
						break;
				}

				if (!(PeHighSchoolType == PreviousHighSchoolEducationType.FromRegister) && (!PeDiplomaDate.HasValue || PeDiplomaDate.Value.Year < 1960 || PeDiplomaDate.Value.Year > DateTime.Now.Year))
				{
					domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_InvalidDiplomaDate);
				}

				if (!PeResearchAreaId.HasValue || PeResearchAreaId == 0)
				{
					domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_InvalidResearchArea);
				}

				if (!PeEducationalQualificationId.HasValue || PeEducationalQualificationId == 0)
				{
					domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_InvalidEducationalQualification);
				}
			}
			else
            {
				PeAcquiredSpeciality = null;
				PeCountry = null;
				PeCountryId = null;
				PeDiplomaDate = null;
				PeDiplomaNumber = null;
				PeEducationalQualification = null;
				PeEducationalQualificationId = null;
				PeHighSchoolType = null;
				PeInstitution = null;
				PeInstitutionId = null;
				PeInstitutionName = null;
				PeSpecialityMissingInRegister = null;
				PeInstitutionSpeciality = null;
				PeInstitutionSpecialityId = null;
				PePartId = null;
				PeRecognizedSpeciality = null;
				PeResearchArea = null;
				PeResearchAreaId = null;
				PeSpecialityName = null;
				PeSubordinate = null;
				PeSubordinateId = null;
				PeSubordinateName = null;
            }
		}

		protected void ValidateFromRegister(DomainValidatorService domainValidatorService)
		{
			if (!PePartId.HasValue || !PeInstitutionId.HasValue || !PeInstitutionSpecialityId.HasValue)
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_NoPreviousEducation);
			}

			PeInstitutionName = null;
			PeSubordinateName = null;
			PeSpecialityName = null;
			PeSpecialityMissingInRegister = null;
			PeAcquiredSpeciality = null;
			PeRecognizedSpeciality = null;
			PeCountry = null;
			PeCountryId = null;
			PeRecognitionDate = null;
			PeRecognitionNumber = null;
			PeAcquiredForeignEducationalQualification = null;
			PeAcquiredForeignEducationalQualificationId = null;
		}

		protected void ValidateMissingInRegister(DomainValidatorService domainValidatorService)
		{
			if (!PeInstitutionId.HasValue)
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_NoInstitution);
			}

			if (!PeInstitutionSpecialityId.HasValue && string.IsNullOrWhiteSpace(PeSpecialityName))
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_NoSpeciality);
			}

			PeInstitutionName = null;
			PeSubordinateName = null;
			PeCountry = null;
			PeCountryId = null;
			PePartId = null;
			PeAcquiredSpeciality = null;
			PeRecognizedSpeciality = null;
			PeRecognitionDate = null;
			PeRecognitionNumber = null;
			PeAcquiredForeignEducationalQualification = null;
			PeAcquiredForeignEducationalQualificationId = null;
		}

		protected void ValidateAbroad(DomainValidatorService domainValidatorService)
		{
			if (string.IsNullOrWhiteSpace(PeInstitutionName))
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_NoInstitution);
			}

			if (string.IsNullOrWhiteSpace(PeRecognizedSpeciality))
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_NoRecognizedSpeciality);
			}

			if (string.IsNullOrWhiteSpace(PeAcquiredSpeciality))
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_NoAcquiredSpeciality);
			}

			if (!PeCountryId.HasValue)
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_NoCountryAbroad);
			}
			if(!PeAcquiredForeignEducationalQualificationId.HasValue || PeAcquiredForeignEducationalQualificationId == 0)
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_InvalidAcquiredEducationalQualification);
			}
			if(!PeRecognitionDate.HasValue)
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_InvalidRecognitionDate);
			}
			if (String.IsNullOrWhiteSpace(PeRecognitionNumber))
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_InvalidRecognitionNumber);
			}

			PePartId = null;
			PeInstitutionId = null;
			PeInstitution = null;
			PeSubordinateId = null;
			PeSubordinate = null;
			PeInstitutionSpecialityId = null;
			PeInstitutionSpeciality = null;
			PeSpecialityName = null;
			PeSpecialityMissingInRegister = null;
		}

		protected void ValidateClosedInstitution(DomainValidatorService domainValidatorService)
		{
			if (string.IsNullOrWhiteSpace(PeInstitutionName))
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_NoInstitution);
			}

			if (string.IsNullOrWhiteSpace(PeSpecialityName))
			{
				domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonPreviousEducation_NoSpeciality);
			}

			PePartId = null;
			PeInstitutionId = null;
			PeInstitution = null;
			PeSubordinateId = null;
			PeSubordinate = null;
			PeInstitutionSpecialityId = null;
			PeInstitutionSpeciality = null;
			PeCountry = null;
			PeCountryId = null;
			PeAcquiredSpeciality = null;
			PeRecognizedSpeciality = null;
			PeRecognitionDate = null;
			PeRecognitionNumber = null;
			PeAcquiredForeignEducationalQualification = null;
			PeAcquiredForeignEducationalQualificationId = null;
			PeSpecialityMissingInRegister = null;
		}
	}
}
