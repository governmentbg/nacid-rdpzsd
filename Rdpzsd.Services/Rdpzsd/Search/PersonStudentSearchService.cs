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
    public class PersonStudentSearchService : BasePersonStudentDoctoralSearchService<PersonStudent, PersonStudentInfo, PersonStudentSemester, PersonStudentHistory, PersonStudentSearchFilterDto>
	{
		public PersonStudentSearchService(
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
				.Include(e => e.PersonStudents)
					.ThenInclude(s => s.StudentStatus)
				.Include(e => e.PersonStudents)
					.ThenInclude(s => s.StudentEvent)
				.Include(e => e.PersonStudents)
					.ThenInclude(s => s.Institution)
				.Include(e => e.PersonStudents)
					.ThenInclude(s => s.Semesters)
						.ThenInclude(s => s.StudentStatus)
				.Include(e => e.PersonStudents)
					.ThenInclude(s => s.Semesters)
						.ThenInclude(s => s.StudentEvent)
				.Include(e => e.PersonStudents)
					.ThenInclude(s => s.InstitutionSpeciality.Speciality)
				.Include(e => e.PersonStudents)
					.ThenInclude(s => s.Semesters)
						.ThenInclude(m => m.Period)
				.Include(e => e.PersonStudents)
					.ThenInclude(s => s.Diploma);

			return query;
		}
	}
}
