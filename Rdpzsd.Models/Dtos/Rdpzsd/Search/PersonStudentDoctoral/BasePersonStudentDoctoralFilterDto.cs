using Infrastructure.Constants;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Search;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Base;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search
{
    public class BasePersonStudentDoctoralFilterDto<TPart, TPartInfo, TSemester, THistory> : FilterDto, IWhere<TPart>
        where TPart : BasePersonStudentDoctoral<TPartInfo, TSemester>, IMultiPart<TPart, PersonLot, THistory>
        where TPartInfo : PartInfo
        where TSemester : BasePersonSemester, new()
        where THistory : EntityVersion
    {
        public PersonStudentStatusType? StudentStatus { get; set; }
        public string Uan { get; set; }
        public string Uin { get; set; }
        public string ForeignerNumber { get; set; }
        public string Idn { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? BirthCountryId { get; set; }
        public int? CitizenshipId { get; set; }
        public int? InstitutionId { get; set; }
        public int? InstitutionSpecialityId { get; set; }
        public int? EducationalQualificationId { get; set; }
        public int? EducationalFormId { get; set; }
        //Server only
        public bool HasJointSpecialities { get; set; } = false;

		public virtual IQueryable<TPart> WhereBuilder(IQueryable<TPart> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
        {
            query = ConstructSearchFilterType(query, userContext, rdpzsdDbContext);

            query = ConstructPeriodStatusSearch(query);

            if (!string.IsNullOrWhiteSpace(Uan))
            {
                query = query.Where(e => e.Lot.Uan == Uan.Trim().ToUpper());
            }

            if (!string.IsNullOrWhiteSpace(Uin))
            {
                query = query.Where(e => e.Lot.PersonBasic.Uin == Uin.Trim());
            }

            if (!string.IsNullOrWhiteSpace(ForeignerNumber))
            {
                query = query.Where(e => e.Lot.PersonBasic.ForeignerNumber == ForeignerNumber.Trim());
            }

            if (!string.IsNullOrWhiteSpace(Idn))
            {
                var idnNumber = Idn.Trim().ToLower();
                query = query.Where(e => e.Lot.PersonBasic.IdnNumber.Trim().ToLower() == idnNumber);
            }

            if (!string.IsNullOrWhiteSpace(FullName))
            {
                var fullName = FullName.Trim().ToLower();
                var splitedNames = FullName.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

                query = query
                    .Where(e => e.Lot.PersonBasic.FirstName.Trim().ToLower() == fullName
                       || e.Lot.PersonBasic.MiddleName.Trim().ToLower() == fullName
                       || e.Lot.PersonBasic.LastName.Trim().ToLower() == fullName
                       || e.Lot.PersonBasic.FirstNameAlt.Trim().ToLower() == fullName
                       || e.Lot.PersonBasic.MiddleNameAlt.Trim().ToLower() == fullName
                       || e.Lot.PersonBasic.LastNameAlt.Trim().ToLower() == fullName
                       || e.Lot.PersonBasic.OtherNames.Trim().ToLower() == fullName
                       || e.Lot.PersonBasic.OtherNamesAlt.Trim().ToLower() == fullName
                       || e.Lot.PersonBasic.FullName.Trim().ToLower() == fullName
                       || e.Lot.PersonBasic.FullNameAlt.Trim().ToLower() == fullName
                       || (splitedNames.Length == 2 && (e.Lot.PersonBasic.FirstName.Trim().ToLower() == splitedNames[0] &&
                            (e.Lot.PersonBasic.LastName.Trim().ToLower() == splitedNames[1]) || e.Lot.PersonBasic.MiddleName.Trim().ToLower() == splitedNames[1]))
                       || (splitedNames.Length == 3 && (e.Lot.PersonBasic.FirstName.Trim().ToLower() == splitedNames[0] &&
                            e.Lot.PersonBasic.MiddleName.Trim().ToLower() == splitedNames[1] && e.Lot.PersonBasic.LastName.Trim().ToLower() == splitedNames[2])));

            }

            if (!string.IsNullOrWhiteSpace(Email))
            {
                var email = $"%{Email.Trim().ToLower()}%";
                query = query.Where(e => EF.Functions.ILike(e.Lot.PersonBasic.Email.Trim().ToLower(), email));
            }

            if (BirthCountryId.HasValue)
            {
                query = query.Where(e => e.Lot.PersonBasic.BirthCountryId == BirthCountryId);
            }

            if (CitizenshipId.HasValue)
            {
                query = query.Where(e => e.Lot.PersonBasic.CitizenshipId == CitizenshipId || e.Lot.PersonBasic.SecondCitizenshipId == CitizenshipId);
            }

            if (InstitutionId.HasValue && !HasJointSpecialities)
            {
                query = query.Where(e => e.InstitutionId == InstitutionId);
            }

            if (InstitutionSpecialityId.HasValue)
            {
                query = query.Where(e => e.InstitutionSpecialityId == InstitutionSpecialityId);
            }
            else
            {
                if (EducationalQualificationId.HasValue)
                {
                    query = query.Where(e => e.InstitutionSpeciality.Speciality.EducationalQualificationId == EducationalQualificationId);
                }

                if (EducationalFormId.HasValue)
                {
                    query = query.Where(e => e.InstitutionSpeciality.EducationalFormId == EducationalFormId);
                }
            }

            return query;
        }

        public virtual IQueryable<TPart> ConstructPeriodStatusSearch(IQueryable<TPart> query)
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

            return query;
        }

        private IQueryable<TPart> ConstructSearchFilterType(IQueryable<TPart> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
        {
            query = query.Where(e => e.Lot.State == LotState.Actual && e.State != PartState.Erased);

            if (userContext.UserType == UserType.Rsd)
            {
                if(query is IQueryable<PersonStudent>)
				{
                    var institutionIds = rdpzsdDbContext.InstitutionSpecialities
                                                            .Include(e => e.Institution)
                                                            .Include(e => e.InstitutionSpecialityJointSpecialities)
                                                            .Where(e => e.InstitutionSpecialityJointSpecialities.Any(j => j.InstitutionId == userContext.Institution.Id))
                                                            .Select(e => e.Institution.RootId)
                                                            .ToList()
                                                            .Append(userContext.Institution.Id);

                    query = query.Where(e => institutionIds.Contains(e.InstitutionId));

                    var a = institutionIds.ToList();

                    if(institutionIds.Count() > 1)
					{
                        this.HasJointSpecialities = true;
                    }
				}
                else
				{
                    if (userContext.Institution.ChildInstitutions.Any())
                    {
                        query = query.Where(e => e.SubordinateId.HasValue && userContext.Institution.ChildInstitutions.Select(s => s.Id).ToList().Contains(e.SubordinateId.Value));
                    }
                    else
                    {
                        query = query.Where(e => e.InstitutionId == userContext.Institution.Id);
                    }
                }
            }

            return query;
        }
    }
}
