using Rdpzsd.Models.Models.Nomenclatures.Base;

namespace Rdpzsd.Models.Models.Nomenclatures
{
	public class NationalStatisticalInstitute : NomenclatureHierarchy
	{
		public string OldCode { get; set; }
		public bool? ForDoctors { get; set; }
	}
}
