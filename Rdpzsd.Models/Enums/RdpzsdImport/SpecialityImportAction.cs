using System.ComponentModel;

namespace Rdpzsd.Models.Enums.RdpzsdImport
{
    [Description("Действие за импортирания файл - семестриална информация")]
    public enum SpecialityImportAction
    {
        [Description("Добавяне")]
        Add = 1,

        [Description("Редакция")]
        Edit = 2,

        [Description("Изтриване")]
        Erase = 3
    }
}
