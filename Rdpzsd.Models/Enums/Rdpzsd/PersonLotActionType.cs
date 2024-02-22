using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Rdpzsd
{
    [Description("Тип на действието по партидата")]
    public enum PersonLotActionType
    {
        [Description("Създаване на партида")]
        Create = 1,

        [Description("Изпратена за вписване")]
        PendingApproval = 2,

        [Description("Върната за редакция")]
        CancelApproval = 3,

        [Description("Одобрение и вписване")]
        Approve = 4,

        [Description("Изтриване")]
        Erased = 5,

        [Description("Редакция на основни данни")]
        PersonBasicEdit = 6,

        [Description("Добавяне на средно образование")]
        PersonSecondaryAdd = 7,

        [Description("Редакция на средно образование")]
        PersonSecondaryEdit = 8,

        [Description("Добавяне на специалност")]
        PersonStudentAdd = 9,

        [Description("Редакция на специалност")]
        PersonStudentEdit = 10,

        [Description("Изтриване на специалност")]
        PersonStudentErase = 11,

        [Description("Добавяне на семестър към специалност")]
        PersonStudentSemesterAdd = 12,

        [Description("Редакция на семестър към специалност")]
        PersonStudentSemesterEdit = 13,

        [Description("Изтриване на семестър към специалност")]
        PersonStudentSemesterErase = 14,

        [Description("Добавяне на докторска програма")]
        PersonDoctoralAdd = 15,

        [Description("Редакция на докторска програма")]
        PersonDoctoralEdit = 16,

        [Description("Изтриване на докторска програма")]
        PersonDoctoralErase = 17,

        [Description("Добавяне на семестър към докторска програма")]
        PersonDoctoralSemesterAdd = 18,

        [Description("Редакция на семестър към докторска програма")]
        PersonDoctoralSemesterEdit = 19,

        [Description("Изтриване на семестър към докторска програма")]
        PersonDoctoralSemesterErase = 20,

        [Description("Създаване на партида, чрез .txt файл")]
        CreateTxt = 21,

        [Description("Редакция на основни данни, чрез .txt файл")]
        PersonBasicEditTxt = 22,

        [Description("Създаване на неверифицирано лице, чрез .txt файл")]
        CreateUnverifiedTxt = 23,

        [Description("Добавяне на протокол към специалност")]
        PersonStudentProtocolAdd = 24,

        [Description("Изтриване на протокол към специалност")]
        PersonStudentProtocolErase = 25,

        [Description("Редакция на протокол към специалност")]
        PersonStudentProtocolEdit = 26,

        [Description("Изпратен за стикер")]
        SendForSticker = 27,

        [Description("Изпратен за стикер с несъответствия")]
        SendForStickerDiscrepancy = 28,

        [Description("Върнат за преглед стикер")]
        ReturnedForEditSticker = 29,

        [Description("Стикер за печат")]
        StickerForPrint = 30,

        [Description("Получен стикер")]
        RecievedSticker = 31,

        [Description("Преиздаване на повреден или сгрешен стикер")]
        ReissueSticker = 32,

        [Description("Добавяне на диплома към специалност")]
        PersonStudentDiplomaAdd = 33,

        [Description("Редакция на диплома към специалност")]
        PersonStudentDiplomaEdit = 34,

        [Description("Невалидна диплома към специалност")]
        PersonStudentDiplomaInvalid = 35,

        [Description("Добавяне на дубликат на диплома към специалност")]
        PersonStudentDuplicateDiplomaAdd = 36,

        [Description("Редакция на дубликат на диплома към специалност")]
        PersonStudentDuplicateDiplomaEdit = 37,

        [Description("Изпратен за стикер на дубликат")]
        SendDuplicateForSticker = 38,

        [Description("Стикер за печат на дубликат")]
        StickerForPrintDuplicate = 39,

        [Description("Получен стикер на дубликат")]
        RecievedDuplicateSticker = 40,

        [Description("Невалиден дубликат на диплома към специалност")]
        PersonStudentDuplicateDiplomaInvalid = 41,

        [Description("Добавяне на семестър към специалност чрез .txt файл")]
        PersonStudentSemesterAddTxt = 42,

        [Description("Добавяне на специалност чрез .txt файл")]
        PersonStudentAddTxt = 43,

        [Description("Изтриване на семестър към специалност чрез .txt файл")]
        PersonStudentSemesterEraseTxt = 44,

        [Description("Изтриване на специалност чрез .txt файл")]
        PersonStudentEraseTxt = 45,

        [Description("Добавяне на семестър към докторска програма чрез .txt файл")]
        PersonDoctoralSemesterAddTxt = 46,

        [Description("Добавяне на докторска програма чрез .txt файл")]
        PersonDoctoralAddTxt = 47,

        [Description("Изтриване на семестър към докторска програма чрез .txt файл")]
        PersonDoctoralSemesterEraseTxt = 48,

        [Description("Изтриване на докторска програма чрез .txt файл")]
        PersonDoctoralEraseTxt = 49,

        [Description("Добавяне на снимка на физическо лице")]
        PersonImageAdd = 50,

        [Description("Редакция на снимка на физическо лице")]
        PersonImageEdit = 51
    }
}
