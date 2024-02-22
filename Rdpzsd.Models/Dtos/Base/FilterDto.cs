namespace Rdpzsd.Models.Dtos.Base
{
	public class FilterDto
	{
		public int Limit { get; set; } = 30;
		public int Offset { get; set; } = 0;

		public bool? IsActive { get; set; }

		// If using pagination or nomenclature select this is false
		// When using excel export this is true
		public bool GetAllData { get; set; } = false;

		// This is used if you want to combine search by several fields
		// Also used from nomenclature selects
		public string TextFilter { get; set; }
	}
}
