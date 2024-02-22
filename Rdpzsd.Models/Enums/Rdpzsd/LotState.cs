using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Rdpzsd
{
    [Description("Статус на партида")]
    public enum LotState
    {
        [Description("Актуално състояние")]
        Actual = 1,

        [Description("Изпратен за вписване")]
        PendingApproval = 2,

        [Description("Върнат за редакция")]
        CancelApproval = 3,

        [Description("Липсва документ за самоличност")]
        MissingPassportCopy = 4,

        [Description("Изтрит")]
        Erased = 5
    }
}
