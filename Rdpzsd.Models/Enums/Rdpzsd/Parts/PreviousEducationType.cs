using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Rdpzsd.Parts
{
    [Description("Вид на предходното образование")]
    public enum PreviousEducationType
    {
        [Description("Средно образование")]
        Secondary = 1,

        [Description("Висше образование")]
        HighSchool = 2,

        [Description("Липсва")]
        Missing = 3
    }
}
