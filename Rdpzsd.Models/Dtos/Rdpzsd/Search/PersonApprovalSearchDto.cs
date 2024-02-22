using Infrastructure.Integrations.EmsIntegration.Dtos;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Rdpzsd;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Search
{
    public class PersonApprovalSearchDto
    {
		public DateTime CreateDate { get; set; }
		public int CreateUserId { get; set; }
		public UserEmsDto CreateUser { get; set; }
        public int LotId { get; set; }
        public string Uan { get; set; }
        public LotState State { get; set; }
        [Skip]
		public Institution CreateInstitution { get; set; }
		public int? CreateInstitutionId { get; set; }
		[Skip]
		public Institution CreateSubordinate { get; set; }
		public int? CreateSubordinateId { get; set; }
		public string FullName { get; set; }
		public string FullNameAlt { get; set; }
        public Country BirthCountry{ get; set; }
        public DateTime BirthDate { get; set; }
        public string ForeignerBirthSettlement { get; set; }

        public PersonApprovalSearchDto(PersonLot personLot, List<UserEmsDto> emsUsers)
		{
			CreateDate = personLot.CreateDate;
			CreateUserId = personLot.CreateUserId;
			CreateUser = emsUsers.Single(e => e.Id == personLot.CreateUserId);
			LotId = personLot.Id;
			Uan = personLot.Uan;
			State = personLot.State;
			CreateInstitution = personLot.CreateInstitution;
			CreateInstitutionId = personLot.CreateInstitutionId;
			CreateSubordinate = personLot.CreateSubordinate;
			CreateSubordinateId = personLot.CreateSubordinateId;
			FullName = personLot.PersonBasic.FullName;
			FullNameAlt = personLot.PersonBasic.FullNameAlt;
			BirthCountry = personLot.PersonBasic.BirthCountry;
			BirthDate = personLot.PersonBasic.BirthDate;
			ForeignerBirthSettlement = personLot.PersonBasic.ForeignerBirthSettlement;
		}
	}

	public static class PersonApprovalSearchDtoExtensions
	{
		public static List<PersonApprovalSearchDto> ToPersonApprovalSearchDto(this List<PersonLot> personLots, List<UserEmsDto> emsUsers)
		{
			return personLots.Select(e => new PersonApprovalSearchDto(e, emsUsers)).ToList();
		}
	}
}
