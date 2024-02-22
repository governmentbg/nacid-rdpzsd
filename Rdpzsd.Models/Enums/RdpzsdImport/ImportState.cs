using System.ComponentModel;

namespace Rdpzsd.Models.Enums.RdpzsdImportFile
{
    [Description("Статус на импортирания TXT/JSON файл")]
    public enum ImportState
    {
        [Description("Очаква проверка")]
        Draft = 1,

        [Description("В проверка")]
        InProgress = 2,

        [Description("Грешка")]
        Error = 3,

        [Description("Проверен")]
        Validated = 4,

        [Description("Очаква вписване")]
        WaitingRegistration = 5,

        [Description("В процес на вписване")]
        InProgressRegistration = 6,

        [Description("Вписан")]
        Registered = 7,

        [Description("Изтрит")]
        Deleted = 8,

        [Description("Сървърна грешка - валидация")]
        ValidationServerError = 9,

        [Description("Сървърна грешка - вписване")]
        RegistrationServerError = 10
    }
}
