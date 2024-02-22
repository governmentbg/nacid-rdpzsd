using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Nomenclature.School
{
    [Description("Вид на училището (чл. 35-36)")]
    public enum SchoolOwnershipType
    {
        [Description("Държавно")]
        State = 1,

        [Description("Общинско")]
        Municipal = 2,

        [Description("Частно")]
        Private = 3,

        [Description("Духовно")]
        TheologicalSchool = 4
    }
}
