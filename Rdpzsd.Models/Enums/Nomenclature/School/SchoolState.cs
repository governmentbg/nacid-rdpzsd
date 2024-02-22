using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Nomenclature
{
    [Description("Статус на училището")]
    public enum SchoolState
    {
        [Description("Действащо")]
        Actual = 1,

        [Description("Преобразувано (закрито)")]
        ConvertedClosed = 2,

        [Description("Отписано")]
        WrittenOff = 3,

        [Description("Заличено")]
        Erased = 4,

        [Description("Закрито")]
        Closed = 5,

        [Description("Действащо (не провежда учебен процес през текущата година)")]
        ActualNoLearningProcess = 6,

        [Description("Преименувано")]
        Renamed = 7
    }
}
