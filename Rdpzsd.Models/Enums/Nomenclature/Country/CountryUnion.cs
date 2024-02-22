using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Nomenclature.Country
{
    public enum CountryUnion
    {
		[Description("Държави членуващи в ЕС и ЕИП")]
		EuAndEea = 1,

		[Description("Държави нечленуващи в ЕС и ЕИП")]
		OtherCountries = 2,
	}
}
