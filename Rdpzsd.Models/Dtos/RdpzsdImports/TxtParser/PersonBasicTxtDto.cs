using Rdpzsd.Models.Dtos.RdpzsdImports.TxtValidationErrorCodes;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser
{
    public class PersonBasicTxtDto
    {
        public string Uan { get; set; }
        public string Uin { get; set; }
        public string ForeignerNumber { get; set; }
        public string IdnNumber { get; set; }
        public string BirthDate { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }

        public string FirstNameAlt { get; set; }
        public string MiddleNameAlt { get; set; }
        public string LastNameAlt { get; set; }
        public string OtherNamesAlt { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string BirthCountryCode { get; set; }
        public string BirthSettlementCode { get; set; }
        public string BirthDistrictCode { get; set; }
        public string BirthMunicipalityCode { get; set; }
        public string ForeignerBirthSettlement { get; set; }

        public string ResidenceCountryCode { get; set; }
        public string ResidenceSettlementCode { get; set; }
        public string ResidenceDistrictCode { get; set; }
        public string ResidenceMunicipalityCode { get; set; }
        public string PostCode { get; set; }
        public string ResidenceAddress { get; set; }
        public string ForeignerResidenceAddress { get; set; }

        public string CitizenshipCode { get; set; }
        public string SecondCitizenshipCode { get; set; }

        public int ErrorRow { get; set; }
        public string ErrorCodes { get; set; }

        public PersonBasicTxtDto(LineDto<TxtValidationErrorCode> lineDto)
        {
            Uan = lineDto.Columns.SingleOrDefault(e => e.Index == 0)?.Value;
            Uin = lineDto.Columns.SingleOrDefault(e => e.Index == 1)?.Value;
            ForeignerNumber = lineDto.Columns.SingleOrDefault(e => e.Index == 2)?.Value;
            IdnNumber = lineDto.Columns.SingleOrDefault(e => e.Index == 3)?.Value;
            BirthDate = lineDto.Columns.SingleOrDefault(e => e.Index == 4)?.Value;
            FirstName = lineDto.Columns.SingleOrDefault(e => e.Index == 5)?.Value;
            MiddleName = lineDto.Columns.SingleOrDefault(e => e.Index == 6)?.Value;
            LastName = lineDto.Columns.SingleOrDefault(e => e.Index == 7)?.Value;
            OtherNames = lineDto.Columns.SingleOrDefault(e => e.Index == 8)?.Value;
            FirstNameAlt = lineDto.Columns.SingleOrDefault(e => e.Index == 9)?.Value;
            MiddleNameAlt = lineDto.Columns.SingleOrDefault(e => e.Index == 10)?.Value;
            LastNameAlt = lineDto.Columns.SingleOrDefault(e => e.Index == 11)?.Value;
            OtherNamesAlt = lineDto.Columns.SingleOrDefault(e => e.Index == 12)?.Value;
            Gender = lineDto.Columns.SingleOrDefault(e => e.Index == 13)?.Value;
            Email = lineDto.Columns.SingleOrDefault(e => e.Index == 14)?.Value;
            PhoneNumber = lineDto.Columns.SingleOrDefault(e => e.Index == 15)?.Value;
            BirthCountryCode = lineDto.Columns.SingleOrDefault(e => e.Index == 16)?.Value;
            BirthSettlementCode = lineDto.Columns.SingleOrDefault(e => e.Index == 17)?.Value;
            BirthDistrictCode = lineDto.Columns.SingleOrDefault(e => e.Index == 18)?.Value;
            BirthMunicipalityCode = lineDto.Columns.SingleOrDefault(e => e.Index == 19)?.Value;
            ForeignerBirthSettlement = lineDto.Columns.SingleOrDefault(e => e.Index == 20)?.Value;
            ResidenceCountryCode = lineDto.Columns.SingleOrDefault(e => e.Index == 21)?.Value;
            ResidenceSettlementCode = lineDto.Columns.SingleOrDefault(e => e.Index == 22)?.Value;
            ResidenceDistrictCode = lineDto.Columns.SingleOrDefault(e => e.Index == 23)?.Value;
            ResidenceMunicipalityCode = lineDto.Columns.SingleOrDefault(e => e.Index == 24)?.Value;
            PostCode = lineDto.Columns.SingleOrDefault(e => e.Index == 25)?.Value;
            ResidenceAddress = lineDto.Columns.SingleOrDefault(e => e.Index == 26)?.Value;
            ForeignerResidenceAddress = lineDto.Columns.SingleOrDefault(e => e.Index == 27)?.Value;
            CitizenshipCode = lineDto.Columns.SingleOrDefault(e => e.Index == 28)?.Value;
            SecondCitizenshipCode = lineDto.Columns.SingleOrDefault(e => e.Index == 29)?.Value;

            ErrorRow = lineDto.RowIndex;
            ErrorCodes = $"{string.Join("; ", lineDto.ErrorCodes.Select(e => (int)e))}";
        }
    }

    public static class PersonBasicTxtDtoExtensions
    {
        public static List<PersonBasicTxtDto> ToPersonBasicTxtDto(this List<LineDto<TxtValidationErrorCode>> lineDtos)
        {
            return lineDtos.Select(e => new PersonBasicTxtDto(e)).ToList();
        }
    }
}
