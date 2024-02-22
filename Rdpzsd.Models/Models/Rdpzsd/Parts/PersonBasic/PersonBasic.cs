using Infrastructure.Constants;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rdpzsd.Models.Attributes;
using Rdpzsd.Models.Models.Rdpzsd.Interfaces;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Base;
using Rdpzsd.Models.Models.Rdpzsd.Parts.Collections;
using Rdpzsd.Models.Models.Rdpzsd.Parts.History;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rdpzsd.Models.Models.Rdpzsd.Parts
{
    public class PersonBasic : BasePersonBasic<PersonBasicInfo, PassportCopy, PersonImage>,
        ISinglePart<PersonBasic, PersonLot, PersonBasicHistory>
    {
        [Skip]
        public PersonLot Lot { get; set; }

        [Skip]
        public List<PersonBasicHistory> Histories { get; set; } = new List<PersonBasicHistory>();

        public void ValidateProperties(RdpzsdDbContext context, DomainValidatorService domainValidatorService)
        {
            Uin = Uin?.Trim();
            ForeignerNumber = ForeignerNumber?.Trim();
            FirstName = FirstName?.Trim();
            MiddleName = MiddleName?.Trim();
            LastName = LastName?.Trim();
            FirstNameAlt = FirstNameAlt?.Trim();
            MiddleNameAlt = MiddleNameAlt?.Trim();
            LastNameAlt = LastNameAlt?.Trim();
            OtherNames = OtherNames?.Trim();
            OtherNamesAlt = OtherNamesAlt?.Trim();
            Email = Email?.Trim();
            PhoneNumber = Regex.Replace(PhoneNumber?.Trim(), @"\s+", " ");
            FullName = FirstName + (!string.IsNullOrWhiteSpace(MiddleName) ? $" {MiddleName}" : string.Empty) + $" {LastName}"
                + (!string.IsNullOrWhiteSpace(OtherNames) ? $" {OtherNames}" : string.Empty);
            FullNameAlt = FirstNameAlt + (!string.IsNullOrWhiteSpace(MiddleNameAlt) ? $" {MiddleNameAlt}" : string.Empty) + $" {LastNameAlt}"
                + (!string.IsNullOrWhiteSpace(OtherNamesAlt) ? $" {OtherNamesAlt}" : string.Empty);
            PostCode = PostCode?.Trim();
            ResidenceAddress = ResidenceAddress?.Trim();

            if (string.IsNullOrWhiteSpace(Uin)
                && string.IsNullOrWhiteSpace(ForeignerNumber)
                && (string.IsNullOrWhiteSpace(IdnNumber) || PassportCopy == null))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_UinFnIdnRequired);
            }

            if ((!string.IsNullOrWhiteSpace(Uin) && Uin.Length != 10) || (!string.IsNullOrWhiteSpace(ForeignerNumber) && ForeignerNumber.Length != 10))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_UinFnLength);
            }

            if (!string.IsNullOrWhiteSpace(Uin))
            {
                if (!ValidatePropertiesStatic.IsValidUinCheckSum(Uin))
                {
                    domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_UinInvalid);
                }
            }

            if (!string.IsNullOrWhiteSpace(ForeignerNumber))
            {
                if (!ValidatePropertiesStatic.IsValidForeignerNumberCheckSum(ForeignerNumber))
                {
                    domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_ForeignerNumberInvalid);
                }
            }

            if (Id == 0 && (string.IsNullOrWhiteSpace(Email) || !ValidatePropertiesStatic.IsValidEmail(Email)))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_InvalidEmail);
            }

            if (Id > 0 && (string.IsNullOrWhiteSpace(Email) || (Email.ToLower() != EmailConstants.NoEmail.ToLower() && !ValidatePropertiesStatic.IsValidEmail(Email))))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_InvalidEmail);
            }

            if (string.IsNullOrWhiteSpace(PhoneNumber) || !ValidatePropertiesStatic.IsValidPhoneNumber(PhoneNumber))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_InvalidPhoneNumber);
            }

            if (string.IsNullOrWhiteSpace(FirstName)
                || string.IsNullOrWhiteSpace(LastName)
                || !ValidatePropertiesStatic.IsValidCyrillic(FirstName)
                || !ValidatePropertiesStatic.IsValidCyrillic(LastName))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_FirstLastNameRequiredCyrillic);
            }

            if (!string.IsNullOrWhiteSpace(MiddleName)
                && !ValidatePropertiesStatic.IsValidCyrillic(MiddleName))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_MiddleNameCyrillic);
            }

            if (!string.IsNullOrWhiteSpace(OtherNames)
                && !ValidatePropertiesStatic.IsValidCyrillic(OtherNames))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_OtherNamesCyrillic);
            }

            if (string.IsNullOrWhiteSpace(FirstNameAlt)
                || string.IsNullOrWhiteSpace(LastNameAlt)
                || !ValidatePropertiesStatic.IsValidLatin(FirstNameAlt)
                || !ValidatePropertiesStatic.IsValidLatin(LastNameAlt))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_FirstLastNameAltRequiredLatin);
            }

            if (!string.IsNullOrWhiteSpace(MiddleNameAlt)
                && !ValidatePropertiesStatic.IsValidLatin(MiddleNameAlt))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_MiddleNameAlt);
            }

            if (!string.IsNullOrWhiteSpace(OtherNamesAlt)
                && !ValidatePropertiesStatic.IsValidLatin(OtherNamesAlt))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_OtherNamesAltLatin);
            }

            if (!BirthCountryId.HasValue)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_BirthCountryRequired);
            }

            if (BirthCountry != null && BirthCountry.Code == CountryConstants.BulgariaCode
                && (!BirthDistrictId.HasValue || !BirthMunicipalityId.HasValue || !BirthSettlementId.HasValue))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_BirthSettlementRequired);
            }

            if (SecondCitizenshipId.HasValue && SecondCitizenshipId == CitizenshipId)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_SecondCitizenshipDifferentThanMain);
            }

            if (!ResidenceCountryId.HasValue)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_ResidenceCountryRequired);
            }

            if (ResidenceCountry != null && ResidenceCountry.Code == CountryConstants.BulgariaCode
                && (!ResidenceDistrictId.HasValue || !ResidenceMunicipalityId.HasValue || !ResidenceSettlementId.HasValue))
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_ResidenceSettlementRequired);
            }

            if (SecondCitizenshipId.HasValue && SecondCitizenshipId == CitizenshipId)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.PersonBasic_CitizenshipsMustBeDifferent);
            }
        }

        public IQueryable<PersonBasic> IncludeAll(IQueryable<PersonBasic> query)
        {
            return query
                .Include(e => e.BirthCountry)
                .Include(e => e.BirthDistrict)
                .Include(e => e.BirthMunicipality)
                .Include(e => e.BirthSettlement.District)
                .Include(e => e.BirthSettlement.Municipality)
                .Include(e => e.Citizenship)
                .Include(e => e.SecondCitizenship)
                .Include(e => e.ResidenceCountry)
                .Include(e => e.ResidenceDistrict)
                .Include(e => e.ResidenceMunicipality)
                .Include(e => e.ResidenceSettlement)
                .Include(e => e.PersonImage)
                .Include(e => e.PassportCopy);
        }
    }

    public class PersonBasicConfiguration : IEntityTypeConfiguration<PersonBasic>
    {
        public void Configure(EntityTypeBuilder<PersonBasic> builder)
        {
            builder.HasMany(e => e.Histories)
                .WithOne()
                .HasForeignKey(e => e.PartId);

            builder.Property(e => e.Uin)
                .HasMaxLength(10)
                .IsFixedLength()
                .IsRequired(false);

            builder.HasIndex(e => e.Uin)
                .IsUnique();

            builder.Property(e => e.ForeignerNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .IsRequired(false);

            builder.HasIndex(e => e.ForeignerNumber)
                .IsUnique();

            builder.Property(e => e.FirstName)
                .HasMaxLength(50);

            builder.Property(e => e.MiddleName)
                .HasMaxLength(50);

            builder.Property(e => e.LastName)
                .HasMaxLength(50);

            builder.Property(e => e.FirstNameAlt)
                .HasMaxLength(50);

            builder.Property(e => e.MiddleNameAlt)
                .HasMaxLength(50);

            builder.Property(e => e.LastNameAlt)
                .HasMaxLength(50);

            builder.Property(e => e.OtherNames)
                .HasMaxLength(100);

            builder.Property(e => e.OtherNamesAlt)
                .HasMaxLength(100);

            builder.Property(e => e.ForeignerBirthSettlement)
                .HasMaxLength(255);

            builder.HasIndex(e => e.Email)
                .IsUnique()
                .HasFilter("(email) NOT ILIKE 'NoEmail'");

            builder.Property(e => e.Email)
                .HasMaxLength(50);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(18);

            builder.Property(e => e.ResidenceAddress)
                .HasMaxLength(255);

            builder.Property(e => e.IdnNumber)
                .HasMaxLength(50);

            builder.Property(e => e.PostCode)
                .HasMaxLength(4);
        }
    }
}
