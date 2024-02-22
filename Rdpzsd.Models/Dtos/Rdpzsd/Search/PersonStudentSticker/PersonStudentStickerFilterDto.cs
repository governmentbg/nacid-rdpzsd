using Infrastructure.Constants;
using Infrastructure.User;
using Infrastructure.User.Enums;
using Microsoft.EntityFrameworkCore;
using Rdpzsd.Models.Dtos.Base;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Enums.Rdpzsd.Parts;
using Rdpzsd.Models.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search.PersonStudentSticker
{
    public class PersonStudentStickerFilterDto : FilterDto, IWhere<PersonStudent>
    {
        public StudentStickerState? StickerState { get; set; }
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

        public IQueryable<PersonStudent> WhereBuilder(IQueryable<PersonStudent> query, UserContext userContext, RdpzsdDbContext rdpzsdDbContext)
        {
            query = ConstructSearchFilterType(query, userContext);

            if (StickerState.HasValue)
            {
                query = query.Where(e => (e.StickerState == StickerState && (e.Diploma == null || e.Diploma.IsValid)) 
                    || e.DuplicateDiplomas.Any(s => s.DuplicateStickerState == StickerState && s.IsValid));
            }

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

            if (InstitutionId.HasValue)
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

        private IQueryable<PersonStudent> ConstructSearchFilterType(IQueryable<PersonStudent> query, UserContext userContext)
        {
            query = query.Where(e => e.Lot.State == LotState.Actual && e.State != PartState.Erased);

            if (userContext.UserType == UserType.Rsd)
            {
                query = query.Where(e => e.StudentStatus.Alias == StudentStatusConstants.ProcessGraduation 
                    || e.StudentEvent.Alias == StudentEventConstants.GraduatedWithoutDiploma
                    || e.StickerState == StudentStickerState.ReturnedForEdit
                    || (e.StudentEvent.Alias == StudentEventConstants.GraduatedWithDiploma &&
                        e.DuplicateDiplomas.Any(s => s.IsValid && (s.DuplicateStickerState != StudentStickerState.Recieved || s.File == null))));

                if (userContext.Institution.ChildInstitutions.Any())
                {
                    query = query.Where(e => e.SubordinateId.HasValue && userContext.Institution.ChildInstitutions.Select(s => s.Id).ToList().Contains(e.SubordinateId.Value));
                }
                else
                {
                    query = query.Where(e => e.InstitutionId == userContext.Institution.Id);
                }
            } 
            else if (userContext.UserType == UserType.Ems && userContext.Permissions.Contains(PermissionConstants.RdpzsdStickers))
            {
                query = query.Where(e => (e.StickerState != StudentStickerState.None 
                        && e.StickerState != StudentStickerState.Recieved
                        && e.StickerState != StudentStickerState.ReturnedForEdit)
                    || e.DuplicateDiplomas.Any(s => s.IsValid 
                        && s.DuplicateStickerState != StudentStickerState.None
                        && s.DuplicateStickerState != StudentStickerState.Recieved
                        && s.DuplicateStickerState != StudentStickerState.ReturnedForEdit)
                    );
            }
            else
            {
                query = query.Where(e => false);
            }

            return query;
        }
    }
}
