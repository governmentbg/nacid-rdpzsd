using System.ComponentModel;

namespace Rdpzsd.Models.Enums.Nomenclature.School
{
    [Description("Вид на училището (чл. 24-27/37-41)")]
    public enum SchoolType
    {
        [Description("Средно училище")]
        Secondary = 1,

        [Description("Професионална гимназия")]
        HighSchool = 2,

        [Description("Профилирана гимназия")]
        ProfiledHighSchool = 3,

        [Description("Духовно училище")]
        TheologicalSchool = 4,

        [Description("Обединено училище")]
        UnitedSchool = 5,

        [Description("Училище по изкуствата")]
        ArtSchool = 6,

        [Description("Училище по културата")]
        CultureSchool = 7,

        [Description("Спортно училище")]
        SportSchool = 8,

        [Description("Специализирано училище")]
        SpecializedSchool = 9
    }
}
