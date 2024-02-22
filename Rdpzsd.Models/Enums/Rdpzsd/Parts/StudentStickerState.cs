using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Rdpzsd.Parts
{
    [Description("Статус на стикера за студент")]
    public enum StudentStickerState
    {
        [Description("Няма изпратена заявка")]
        None = 1,

        [Description("Изпратен за стикер")]
        SendForSticker = 2,

        [Description("Изпратен за стикер с несъответствия")]
        SendForStickerDiscrepancy = 3,

        [Description("Върнат за преглед")]
        ReturnedForEdit = 4,

        [Description("Преиздаване на повреден/сгрешен стикер")]
        ReissueSticker = 5,

        [Description("За печат")]
        StickerForPrint = 6,

        [Description("Получен")]
        Recieved = 7
    }
}
