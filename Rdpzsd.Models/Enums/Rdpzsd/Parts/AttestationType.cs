using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Rdpzsd.Parts
{
    [Description("Положителна или отрициателна атестация")]
    public enum AttestationType
    {
        [Description("Положителна")]
        Positive = 1,

        [Description("Отрицателна")]
        Negative = 2,
    }
}
