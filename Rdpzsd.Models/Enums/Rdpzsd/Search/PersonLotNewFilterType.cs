using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Rdpzsd.Search
{
	[Description("Търсене по идентификационен номер или месторождение")]
	public enum PersonLotNewFilterType
	{
		[Description("Идентификационен номер")]
		IdentificationNumber = 1,

		[Description("Месторождение")]
		BirthPlace = 2
	}
}
