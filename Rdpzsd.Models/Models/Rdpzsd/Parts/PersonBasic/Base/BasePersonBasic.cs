using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Base;
using Rdpzsd.Models.Models.Nomenclatures;
using Rdpzsd.Models.Models.Rdpzsd.Base;
using System;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts.Base
{
    public abstract class BasePersonBasic<TPartInfo, TPassportCopy, TPersonImage> : Part<TPartInfo>
        where TPartInfo : PartInfo
        where TPassportCopy : RdpzsdAttachedFile, new()
        where TPersonImage : RdpzsdAttachedFile, new()
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        // This is constructed in server by all names
        public string FullName { get; set; }

        public string FirstNameAlt { get; set; }
        public string MiddleNameAlt { get; set; }
        public string LastNameAlt { get; set; }
        public string OtherNamesAlt { get; set; }
        // This is constructed in server by all names
        public string FullNameAlt { get; set; }

        public string Uin { get; set; }
        public string ForeignerNumber { get; set; }
        public string IdnNumber { get; set; }
        public TPassportCopy PassportCopy { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PostCode { get; set; }

        public GenderType Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public int? BirthCountryId { get; set; }
        [Skip]
        public Country BirthCountry { get; set; }
        public int? BirthDistrictId { get; set; }
        [Skip]
        public District BirthDistrict { get; set; }
        public int? BirthMunicipalityId { get; set; }
        [Skip]
        public Municipality BirthMunicipality { get; set; }
        public int? BirthSettlementId { get; set; }
        [Skip]
        public Settlement BirthSettlement { get; set; }
        public string ForeignerBirthSettlement { get; set; }

        public int CitizenshipId { get; set; }
        [Skip]
        public Country Citizenship { get; set; }
        public int? SecondCitizenshipId { get; set; }
        [Skip]
        public Country SecondCitizenship { get; set; }

        public int? ResidenceCountryId { get; set; }
        [Skip]
        public Country ResidenceCountry { get; set; }
        public int? ResidenceDistrictId { get; set; }
        [Skip]
        public District ResidenceDistrict { get; set; }
        public int? ResidenceMunicipalityId { get; set; }
        [Skip]
        public Municipality ResidenceMunicipality { get; set; }
        public int? ResidenceSettlementId { get; set; }
        [Skip]
        public Settlement ResidenceSettlement { get; set; }
        public string ResidenceAddress { get; set; }

        public TPersonImage PersonImage { get; set; }
    }
}
