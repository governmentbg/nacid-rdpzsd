using System.ComponentModel;

namespace Rdpzsd.Models.Enums
{
	[Description("Тип на институцията от РНД/РВУ")]
	public enum OrganizationType
	{
		[Description("Институт към НО")]
		InstitutionScientificOrganization = 1,

		[Description("Музей")]
		Museum = 2,

		[Description("Фирма")]
		Firm = 3,

		[Description("Център")]
		Center = 4,

		[Description("Лаборатория")]
		Laboratory = 5,

		[Description("Фондация")]
		Foundation = 6,

		[Description("Университет")]
		University = 7,

		[Description("Факултет")]
		Faculty = 8,

		[Description("Агенция")]
		Аgency = 10,

		[Description("Академия (без ВУ)")]
		AgencyWithoutUniversity = 11,

		[Description("Асоциация")]
		Association = 12,

		[Description("Библиотека")]
		Library = 13,

		[Description("Болница (акредитирана)")]
		HospitalAccredited = 14,

		[Description("Ботаническа градина")]
		BotanicalGarden = 15,

		[Description("Галерия")]
		Gallery = 16,

		[Description("Подразделение, болница")]
		SubdivisionHospital = 17,

		[Description("Клъстер")]
		Cluster = 18,

		[Description("Сдружение")]
		Association2 = 20,

		[Description("Сектор (НИС към ВУ)")]
		Sector = 21,

		[Description("Станция")]
		Station = 22,

		[Description("Съюз")]
		Union = 23,

		[Description("Фонд")]
		Fund = 24,

		[Description("Научна организация")]
		ScientificOrganization = 25,

		[Description("Болница (не акредитирана)")]
		HospitalNotAccredited = 26,

		[Description("Катедра")]
		Chair = 27,

		[Description("Департамент")]
		Department = 28,

		[Description("Самостоятелен колеж")]
		IndependentCollege = 29,

		[Description("Институт към ВУ")]
		InstitutionUniversity = 30,

		[Description("Колеж към ВУ")]
		CollegeUniversity = 31,

		[Description("Филиал")]
		Affiliate = 32,

		[Description("Специализирано висше училище")]
		SpecializedUniversity = 33,

		[Description("Център към ВУ")]
		CenterUniversity = 34
	}
}
