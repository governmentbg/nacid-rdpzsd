using Infrastructure.ExcelProcessor.Services;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models;
using Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentDoctoral;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using Rdpzsd.Services.Rdpzsd.Search.Base;
using System.Linq;

namespace Rdpzsd.Services.Rdpzsd.Search
{
    public class PersonDoctoralSearchService : BasePersonStudentDoctoralSearchService<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester, PersonDoctoralHistory, PersonDoctoralSearchFilterDto>
    {
		public PersonDoctoralSearchService(
			RdpzsdDbContext context,
			UserContext userContext,
			ExcelProcessorService excelProcessorService
		) : base(context, userContext, excelProcessorService)
		{
		}

		protected override IQueryable<PersonLot> ConstructIncludes(IQueryable<PersonLot> query)
		{
			query = query
				.Include(e => e.PersonBasic.BirthCountry)
				.Include(e => e.PersonBasic.BirthSettlement)
				.Include(e => e.PersonBasic.Citizenship)
				.Include(e => e.PersonBasic.SecondCitizenship)
				.Include(e => e.PersonDoctorals)
					.ThenInclude(s => s.StudentStatus)
				.Include(e => e.PersonDoctorals)
					.ThenInclude(s => s.StudentEvent)
				.Include(e => e.PersonDoctorals)
					.ThenInclude(s => s.Institution)
				.Include(e => e.PersonDoctorals)
					.ThenInclude(s => s.Semesters)
						.ThenInclude(s => s.StudentStatus)
				.Include(e => e.PersonDoctorals)
					.ThenInclude(s => s.Semesters)
						.ThenInclude(s => s.StudentEvent)
				.Include(e => e.PersonDoctorals)
					.ThenInclude(s => s.InstitutionSpeciality.Speciality);

			return query;
		}
	}
}
