using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Rdpzsd.Search
{
    [Description("Търсене на студенти/докторанти по статус")]
    public enum PersonStudentStatusType
    {
        [Description("действащ и прекъснал")]
        ActiveInterrupted = 1,

        [Description("в процес на дипломиране")]
        ProcessGraduation = 2,

        [Description("дипломиран")]
        Graduated = 3,

        [Description("дипломиран без диплома")]
        GraduatedWithoutDiploma = 4,

        [Description("действащ")]
        Active = 5,

        [Description("прекъснал")]
        Interrupted = 6,

        [Description("отписан")]
        WrittenOff = 7
    }
}
