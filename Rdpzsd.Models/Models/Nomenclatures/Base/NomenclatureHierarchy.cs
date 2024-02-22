using Rdpzsd.Models.Enums;

namespace Rdpzsd.Models.Models.Nomenclatures.Base
{
	public class NomenclatureHierarchy : NomenclatureCode
	{
		public int RootId { get; set; }
		public int? ParentId { get; set; }
		public Level Level { get; set; }
	}
}
