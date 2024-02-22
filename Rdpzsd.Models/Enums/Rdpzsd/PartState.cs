using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Rdpzsd
{
    [Description("Статус на партовете")]
    public enum PartState
    {
        [Description("Актуално състояние")]
        Actual = 1,

        [Description("Изтрит")]
        Erased = 2
    }
}
