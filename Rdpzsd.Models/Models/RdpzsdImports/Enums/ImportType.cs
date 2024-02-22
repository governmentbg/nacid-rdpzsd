using System.ComponentModel;

namespace Rdpzsd.Models.Models.RdpzsdImports.Enums
{
    [Description("Тип на тхт импорт")]
    public enum ImportType
    {
        [Description("Физически данни")]
        PersonImport = 1,

        [Description("Семестриална информация")]
        SpecialityImport = 2,
    }
}
