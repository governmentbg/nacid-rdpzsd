using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Models.Nomenclatures
{
	public class InstitutionSpeciality : EntityVersion, IIncludeAll<InstitutionSpeciality>
	{
		public int InstitutionId { get; set; }
		[Skip]
		public Institution Institution { get; set; }

		public int SpecialityId { get; set; }
		[Skip]
		public Speciality Speciality { get; set; }
		public int? EducationalFormId { get; set; }
		[Skip]
		public EducationalForm EducationalForm { get; set; }
		public int? NsiRegionId { get; set; }
		[Skip]
		public NationalStatisticalInstitute NsiRegion { get; set; }
		public int? NationalStatisticalInstituteId { get; set; }
		[Skip]
		public NationalStatisticalInstitute NationalStatisticalInstitute { get; set; }
		public decimal? Duration { get; set; }
		public bool IsAccredited { get; set; }
		public bool IsForCadets { get; set; }
		// It will be false if IsActive = false or PartState = Erased
		public bool IsActive { get; set; }
		public bool? IsJointSpeciality { get; set; }

		public List<InstitutionSpecialityLanguage> OrganizationSpecialityLanguages { get; set; } = new List<InstitutionSpecialityLanguage>();
		public List<InstitutionSpecialityJointSpeciality> InstitutionSpecialityJointSpecialities { get; set; } = new List<InstitutionSpecialityJointSpeciality>();

		public IQueryable<InstitutionSpeciality> IncludeAll(IQueryable<InstitutionSpeciality> query)
		{
			return query
				.Include(e => e.OrganizationSpecialityLanguages)
					.ThenInclude(s => s.Language)
				.Include(e => e.InstitutionSpecialityJointSpecialities)
					.ThenInclude(e => e.Institution)
				.Include(e => e.InstitutionSpecialityJointSpecialities)
					.ThenInclude(e => e.InstitutionByParent)
				.Include(e => e.Institution.Parent)
				.Include(e => e.EducationalForm)
				.Include(e => e.NationalStatisticalInstitute)
				.Include(e => e.NsiRegion)
				.Include(e => e.Speciality.ResearchArea)
				.Include(e => e.Speciality.EducationalQualification);
		}
	}
}
