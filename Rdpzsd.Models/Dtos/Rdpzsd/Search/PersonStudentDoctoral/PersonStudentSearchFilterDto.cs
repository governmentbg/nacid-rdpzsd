using Infrastructure.Constants;
using Rdpzsd.Models.Enums.Rdpzsd.Search;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentDoctoral
{
    public class PersonStudentSearchFilterDto : BasePersonStudentDoctoralFilterDto<PersonStudent, PersonStudentInfo, PersonStudentSemester, PersonStudentHistory>
    {
        public int? PeriodId { get; set; }

        public override IQueryable<PersonStudent> ConstructPeriodStatusSearch(IQueryable<PersonStudent> query)
        {
            if (PeriodId.HasValue)
            {
                switch (StudentStatus)
                {
                    case PersonStudentStatusType.ActiveInterrupted:
                        query = query.Where(e => (e.StudentStatus.Alias == StudentStatusConstants.Active || e.StudentStatus.Alias == StudentStatusConstants.Interrupted) 
                            && e.Semesters.Any(s => (s.StudentStatus.Alias == StudentStatusConstants.Active
                                || s.StudentStatus.Alias == StudentStatusConstants.Interrupted)
                                    && s.PeriodId == PeriodId)
                        && !e.Semesters.Any(s => s.PeriodId > PeriodId));
                        break;
                    case PersonStudentStatusType.ProcessGraduation:
                        query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.ProcessGraduation
                                && e.Semesters.Any(s => s.StudentStatus.Alias == StudentStatusConstants.ProcessGraduation && s.PeriodId == PeriodId)
                        && !e.Semesters.Any(s => s.PeriodId > PeriodId));
                        break;
                    //case PersonStudentStatusType.Graduated:
                    //    query = query.Where(e => e.Semesters.Any(s => s.StudentStatus.Alias == StudentStatusConstants.Graduated && s.PeriodId == PeriodId)
                    //    && !e.Semesters.Any(s => s.PeriodId > PeriodId));
                    //    break;
                    case PersonStudentStatusType.Active:
                        query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.Active 
                            && e.Semesters.Any(s => s.StudentStatus.Alias == StudentStatusConstants.Active && s.PeriodId == PeriodId)
                            && !e.Semesters.Any(s => s.PeriodId > PeriodId));
                        break;
                    case PersonStudentStatusType.Interrupted:
                        query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.Interrupted
                            && e.Semesters.Any(s => s.StudentStatus.Alias == StudentStatusConstants.Interrupted && s.PeriodId == PeriodId)
                            && !e.Semesters.Any(s => s.PeriodId > PeriodId));
                        break;
                    case PersonStudentStatusType.WrittenOff:
                        query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.Completed
                                && e.Semesters.Any(s => s.StudentStatus.Alias == StudentStatusConstants.Completed && s.PeriodId == PeriodId)
                        && !e.Semesters.Any(s => s.PeriodId > PeriodId));
                        break;
                    default:
                        query = query.Where(e => e.Semesters.Any(s => s.PeriodId == PeriodId) && !e.Semesters.Any(s => s.PeriodId > PeriodId));
                        break;
                }
            }
            else
            {
                switch (StudentStatus)
                {
                    case PersonStudentStatusType.ActiveInterrupted:
                        query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.Active
                            || e.StudentStatus.Alias == StudentStatusConstants.Interrupted);
                        break;
                    case PersonStudentStatusType.ProcessGraduation:
                        query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.ProcessGraduation);
                        break;
                    case PersonStudentStatusType.Graduated:
                        query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.Graduated && e.StudentEvent.Alias == StudentEventConstants.GraduatedWithDiploma);
                        break;
                    case PersonStudentStatusType.GraduatedWithoutDiploma:
                        query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.Graduated && e.StudentEvent.Alias == StudentEventConstants.GraduatedWithoutDiploma);
                        break;
                    case PersonStudentStatusType.Active:
                        query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.Active);
                        break;
                    case PersonStudentStatusType.Interrupted:
                        query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.Interrupted);
                        break;
                    case PersonStudentStatusType.WrittenOff:
                        query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.Completed);
                        break;
                    default:
                        break;
                }
            }

            return query;
        }
    }
}