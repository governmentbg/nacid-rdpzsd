namespace Infrastructure.Integrations.RndIntegration.Dtos
{
	public class BaseInstitutionDto
	{
		// OrganizationLotId
		public int Id { get; set; }
		public int CommitId { get; set; }
		public int? LotNumber { get; set; }

		public int State { get; set; }
		public string Uin { get; set; }
		public string Name { get; set; }
		public string NameAlt { get; set; }
		public string ShortName { get; set; }
		public string ShortNameAlt { get; set; }

		public int? AcadUniId { get; set; }
		public int? AcadFacultyId { get; set; }

		public int? ParentId { get; set; }
	}
}
