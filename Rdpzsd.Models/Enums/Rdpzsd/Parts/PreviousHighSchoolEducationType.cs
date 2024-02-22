using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Rdpzsd.Parts
{
    [Description("Вид на предходното висше образование")]
    public enum PreviousHighSchoolEducationType
    {
        [Description("От регистъра")]
        FromRegister = 1,

        [Description("Липсва информация в регистъра")]
        MissingInRegister = 2,

        [Description("В чужбина")]
        Abroad = 3,

        [Description("Закрито ВУ/НО в България")]
        ClosedInstitution = 4
    }

    [Description("Вид на предходното висше образование - тхт импорт")]
    public enum PreviousHighSchoolEducationTypeTxt
    {
        [Description("В България")]
        MissingInRegister = 1,

        [Description("В чужбина")]
        Abroad = 2
    }
}
