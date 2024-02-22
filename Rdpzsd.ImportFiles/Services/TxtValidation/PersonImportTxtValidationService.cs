using Infrastructure.Constants;
using Infrastructure.Extensions;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser;
using Rdpzsd.Models.Dtos.RdpzsdImports.TxtValidationErrorCodes;
using Rdpzsd.Models.Enums.Rdpzsd;
using Rdpzsd.Models.Models.Rdpzsd.Parts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdpzsd.Import.Services.TxtValidation
{
    public class PersonImportTxtValidationService
    {
        private readonly NomenclatureDictionariesService nomenclatureDictionariesService;
        private Dictionary<string, string> personUinUanDict;
        private Dictionary<string, string> personForeignerNumUanDict;
        private Dictionary<string, PersonBasic> personIdentifierUanDict;
        private Dictionary<string, string> personEmailDict;
        private Dictionary<string, string> personIdnEmailDict;
        private Tuple<Dictionary<string, string>, Dictionary<string, string>, Dictionary<string, PersonBasic>> personIdentifierDicts;
        private HashSet<(string BirthCountry, string BirthDate, string FirstName)> personIdnBirthPlaceHashSet;
        private HashSet<string> personUanHashSet;
        private HashSet<string> txtPersonUanHashSet;
        private HashSet<string> txtPersonUinHashSet;
        private HashSet<string> txtPersonForeignerNumberHashSet;
        private HashSet<string> txtPersonEmailHashSet;
        private HashSet<(string BirthCountry, string BirthDate, string FirstName)> txtPersonIdnBirthPlaceHashSet;
        private HashSet<string> countryCodesHashSet;
        private HashSet<string> settlementCodesHashSet;
        private HashSet<string> districtCodesHashSet;
        private HashSet<string> municipalityCodesHashSet;

        public PersonImportTxtValidationService(NomenclatureDictionariesService nomenclatureDictionariesService)
        {
            this.nomenclatureDictionariesService = nomenclatureDictionariesService;
        }

        private void LoadDictionaries()
        {
            personIdentifierDicts = nomenclatureDictionariesService.GetPersonIdentifierDicts();
            personEmailDict = nomenclatureDictionariesService.GetPersonEmailDict();
            personIdnEmailDict = nomenclatureDictionariesService.GetPersonIdnEmailDict();
            personIdnBirthPlaceHashSet = nomenclatureDictionariesService.GetPersonIdnBirthPlace();
            personUinUanDict = personIdentifierDicts.Item1;
            personForeignerNumUanDict = personIdentifierDicts.Item2;
            personIdentifierUanDict = personIdentifierDicts.Item3;
            personUanHashSet = nomenclatureDictionariesService.GetPersonUanHashSetAsync();
            countryCodesHashSet = nomenclatureDictionariesService.GetCountryCodes();
            settlementCodesHashSet = nomenclatureDictionariesService.GetSettlementCodes();
            districtCodesHashSet = nomenclatureDictionariesService.GetDistrictCodes();
            municipalityCodesHashSet = nomenclatureDictionariesService.GetMunicipalityCodes();
            txtPersonUanHashSet = new HashSet<string>();
            txtPersonUinHashSet = new HashSet<string>();
            txtPersonEmailHashSet = new HashSet<string>();
            txtPersonForeignerNumberHashSet = new HashSet<string>();
            txtPersonIdnBirthPlaceHashSet = new HashSet<(string, string, string)>();
        }

        public List<LineDto<TxtValidationErrorCode>> Validate(List<LineDto<TxtValidationErrorCode>> lineDtos)
        {
            LoadDictionaries();

            foreach (var line in lineDtos)
            {
                if (line.Columns.Count != 30)
                {
                    line.ErrorCodes.Add(TxtValidationErrorCode.WrongColumnCount);
                    continue;
                }

                line.ErrorCodes.AddRange(ValidateColumns(line.Columns));
            }

            return lineDtos;
        }

        private List<TxtValidationErrorCode> ValidateColumns(List<LineColumnDto> lineColumnDtos)
        {
            var txtValidationErrorCodes = new List<TxtValidationErrorCode>();

            var uanValue = lineColumnDtos.Single(e => e.Index == 0).Value;
            var isValidNotEmptyUan = ValidateUan(txtValidationErrorCodes, uanValue);

            var uinValue = lineColumnDtos.Single(e => e.Index == 1).Value;
            var foreignerNumberValue = lineColumnDtos.Single(e => e.Index == 2).Value;
            var idnNumberValue = lineColumnDtos.Single(e => e.Index == 3).Value;

            // Get these here to validate Person with IDN number without UAN
            var birthDateValue = lineColumnDtos.Single(e => e.Index == 4).Value;
            var firstNameCyrillicValue = lineColumnDtos.Single(e => e.Index == 5).Value;
            var birthCountryValue = lineColumnDtos.Single(e => e.Index == 16).Value;

            if (string.IsNullOrWhiteSpace(uinValue) && string.IsNullOrWhiteSpace(foreignerNumberValue) && string.IsNullOrWhiteSpace(idnNumberValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.MissingIdentificator);
            }

            var isValidNotEmptyUin = ValidateUin(txtValidationErrorCodes, uinValue);

            if (isValidNotEmptyUan && isValidNotEmptyUin)
            {
                var personUan = personUinUanDict.GetValueOrDefault(uinValue);

                if (personUan == null)
                {
                    personUinUanDict[uinValue] = uanValue;
                }
                else if (uanValue != personUan)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.UinExists);
                }
            }

            var isValidNotEmptyForeignerNum = ValidateForeignerNumber(txtValidationErrorCodes, foreignerNumberValue);

            if (isValidNotEmptyUan && isValidNotEmptyForeignerNum)
            {
                var personUan = personForeignerNumUanDict.GetValueOrDefault(foreignerNumberValue);

                if (personUan == null)
                {
                    personForeignerNumUanDict[foreignerNumberValue] = uanValue;
                }
                else if (uanValue != personUan)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ForeignerNumberExists);
                }
            }

            if (!string.IsNullOrWhiteSpace(idnNumberValue)
                && (idnNumberValue.Length < 6 || idnNumberValue.Length > 25 || !ValidatePropertiesStatic.IsValidIdnNumber(idnNumberValue)))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidIdnNumber);
            }
            else if (string.IsNullOrWhiteSpace(uanValue)
                  && string.IsNullOrWhiteSpace(uinValue)
                  && string.IsNullOrWhiteSpace(foreignerNumberValue))
            {
                ValidateIdnNumber(txtValidationErrorCodes, birthDateValue, firstNameCyrillicValue, birthCountryValue);
            }

            if (isValidNotEmptyUan && personIdentifierUanDict.ContainsKeyNullCheck(uanValue))
            {
                ValidateUanWithIdentificator(txtValidationErrorCodes, uanValue, uinValue, foreignerNumberValue, idnNumberValue, isValidNotEmptyUin, isValidNotEmptyForeignerNum);
            }

            var isValidIdnNumber = ValidatePersonIdnByBirthDateAndCountry(txtValidationErrorCodes, idnNumberValue, birthDateValue, firstNameCyrillicValue, birthCountryValue);

            var currentYear = DateTime.Now.Year;

            if (string.IsNullOrWhiteSpace(birthDateValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.BirthDateRequired);
            }
            else if (!ValidatePropertiesStatic.IsValidDateBetween(birthDateValue, currentYear - 80, currentYear - 14))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidBirthDate);
            }

            if (!string.IsNullOrWhiteSpace(uinValue)
                && !string.IsNullOrWhiteSpace(birthDateValue)
                && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.InvalidUin)
                && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.InvalidBirthDate))
            {
                if (!ValidatePropertiesStatic.IsValidBirthDateUin(birthDateValue, uinValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidBirthDateUin);
                }
            }

            var middleNameCyrillicValue = lineColumnDtos.Single(e => e.Index == 6).Value;
            var lastNameCyrillicValue = lineColumnDtos.Single(e => e.Index == 7).Value;
            var otherNamesCyrillicValue = lineColumnDtos.Single(e => e.Index == 8).Value;

            if (string.IsNullOrWhiteSpace(firstNameCyrillicValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.FirstNameRequired);
            }
            else
            {
                if (!ValidatePropertiesStatic.IsValidLength(firstNameCyrillicValue, minLength: 2, maxLength: 50))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.FirstNameLengthRestriction);
                }

                if (!ValidatePropertiesStatic.IsValidCyrillic(firstNameCyrillicValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.FirstNameCyrillic);
                }
            }

            if (!string.IsNullOrWhiteSpace(middleNameCyrillicValue))
            {
                if (!ValidatePropertiesStatic.IsValidLength(middleNameCyrillicValue, minLength: 2, maxLength: 50))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.MiddleNameLengthRestriction);
                }

                if (!ValidatePropertiesStatic.IsValidCyrillic(middleNameCyrillicValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.MiddleNameCyrillic);
                }
            }

            if (string.IsNullOrWhiteSpace(lastNameCyrillicValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.LastNameRequired);
            }
            else
            {
                if (!ValidatePropertiesStatic.IsValidLength(lastNameCyrillicValue, minLength: 2, maxLength: 50))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.LastNameLengthRestriction);
                }

                if (!ValidatePropertiesStatic.IsValidCyrillic(lastNameCyrillicValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.LastNameCyrillic);
                }
            }

            if (!string.IsNullOrWhiteSpace(otherNamesCyrillicValue))
            {
                if (!ValidatePropertiesStatic.IsValidLength(otherNamesCyrillicValue, minLength: 2, maxLength: 50))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.OtherNamesLengthRestriction);
                }

                if (!ValidatePropertiesStatic.IsValidCyrillic(otherNamesCyrillicValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.OtherNamesCyrillic);
                }
            }

            var firstNameLatinValue = lineColumnDtos.Single(e => e.Index == 9).Value;
            var middleNameLatinValue = lineColumnDtos.Single(e => e.Index == 10).Value;
            var lastNameLatinValue = lineColumnDtos.Single(e => e.Index == 11).Value;
            var otherNamesLatinValue = lineColumnDtos.Single(e => e.Index == 12).Value;

            if (string.IsNullOrWhiteSpace(firstNameLatinValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.FirstNameLatinRequired);
            }
            else
            {
                if (!ValidatePropertiesStatic.IsValidLength(firstNameLatinValue, minLength: 2, maxLength: 50))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.FirstNameLatinLengthRestriction);
                }

                if (!ValidatePropertiesStatic.IsValidLatin(firstNameLatinValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.FirstNameLatin);
                }
            }

            if (!string.IsNullOrWhiteSpace(middleNameLatinValue))
            {
                if (!ValidatePropertiesStatic.IsValidLength(middleNameLatinValue, minLength: 2, maxLength: 50))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.MiddleNameLatinLengthRestriction);
                }

                if (!ValidatePropertiesStatic.IsValidLatin(middleNameLatinValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.MiddleNameLatin);
                }
            }

            if (string.IsNullOrWhiteSpace(lastNameLatinValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.LastNameLatinRequired);
            }
            else
            {
                if (!ValidatePropertiesStatic.IsValidLength(lastNameLatinValue, minLength: 2, maxLength: 50))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.LastNameLatinLengthRestriction);
                }

                if (!ValidatePropertiesStatic.IsValidLatin(lastNameLatinValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.LastNameLatin);
                }
            }

            if (!string.IsNullOrWhiteSpace(otherNamesLatinValue))
            {
                if (!ValidatePropertiesStatic.IsValidLength(otherNamesLatinValue, minLength: 2, maxLength: 50))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.OtherNamesLatinLengthRestriction);
                }

                if (!ValidatePropertiesStatic.IsValidLatin(otherNamesLatinValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.OtherNamesLatin);
                }
            }

            if (!string.IsNullOrWhiteSpace(middleNameCyrillicValue)
                && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.MiddleNameLengthRestriction)
                && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.MiddleNameCyrillic)
                && string.IsNullOrWhiteSpace(middleNameLatinValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.RequiredMiddleNameLatin);
            }

            if (!string.IsNullOrWhiteSpace(middleNameLatinValue)
               && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.MiddleNameLatinLengthRestriction)
               && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.MiddleNameLatin)
               && string.IsNullOrWhiteSpace(middleNameCyrillicValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.RequiredMiddleNameCyrillic);
            }

            if (!string.IsNullOrWhiteSpace(otherNamesCyrillicValue)
               && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.OtherNamesLengthRestriction)
               && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.OtherNamesCyrillic)
               && string.IsNullOrWhiteSpace(otherNamesLatinValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.RequiredOtherNamesLatin);
            }

            if (!string.IsNullOrWhiteSpace(otherNamesLatinValue)
              && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.OtherNamesLatinLengthRestriction)
              && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.OtherNamesLatin)
              && string.IsNullOrWhiteSpace(otherNamesCyrillicValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.RequiredOtherNamesCyrillic);
            }

            var genderValue = lineColumnDtos.Single(e => e.Index == 13).Value;

            if (string.IsNullOrWhiteSpace(genderValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.GenderTypeRequired);
            }
            else if (!genderValue.TryParseEnumValue<GenderType>())
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidGenderType);
            }
            else if (!string.IsNullOrWhiteSpace(uinValue)
                && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.InvalidUin)
                && !ValidatePropertiesStatic.IsValidGenderTypeUin(genderValue, uinValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidGenderTypeByUin);
            }

            var emailValue = lineColumnDtos.Single(e => e.Index == 14).Value;

            if (string.IsNullOrWhiteSpace(emailValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.EmailRequired);
            }
            else
            {
                var emailValueToLower = emailValue.ToLower();

                if (!ValidatePropertiesStatic.IsValidEmail(emailValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidEmail);
                }
                else if (!ValidatePropertiesStatic.IsValidLength(emailValue, minLength: 8, maxLength: 50))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.EmailLengthRestriction);
                }

                else if (personIdnEmailDict.ContainsKeyNullCheck(emailValueToLower))
                {
                    if (!isValidNotEmptyUan || personIdnEmailDict.GetDictValueOrNull(emailValueToLower) != uanValue)
                    {
                        txtValidationErrorCodes.Add(TxtValidationErrorCode.EmailNotUnique);
                    }
                }

                else if (personEmailDict.ContainsValue(emailValueToLower))
                {
                    if (isValidNotEmptyUin
                        && ((personEmailDict.ContainsKeyNullCheck(uinValue) && personEmailDict[uinValue] != emailValueToLower) || !personEmailDict.ContainsKeyNullCheck(uinValue)))
                    {
                        txtValidationErrorCodes.Add(TxtValidationErrorCode.EmailNotUnique);
                    }
                    else if (isValidNotEmptyForeignerNum
                        && ((personEmailDict.ContainsKeyNullCheck(foreignerNumberValue) && personEmailDict[foreignerNumberValue] != emailValueToLower) || !personEmailDict.ContainsKeyNullCheck(foreignerNumberValue)))
                    {
                        txtValidationErrorCodes.Add(TxtValidationErrorCode.EmailNotUnique);
                    }
                    else if (isValidIdnNumber && personIdnEmailDict.ContainsValue(emailValueToLower))
                    {
                        txtValidationErrorCodes.Add(TxtValidationErrorCode.EmailNotUnique);
                    }
                }
                else if (txtPersonEmailHashSet.Contains(emailValueToLower))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.EmailNotUniqueInTxt);
                }
                else
                {
                    txtPersonEmailHashSet.Add(emailValueToLower);
                }
            }

            var phoneValue = lineColumnDtos.Single(e => e.Index == 15).Value;

            if (string.IsNullOrWhiteSpace(phoneValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.PhoneNumberRequired);
            }
            else if (!ValidatePropertiesStatic.IsValidPhoneNumber(phoneValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidPhoneNumber);
            }

            var birthSettlementValue = lineColumnDtos.Single(e => e.Index == 17).Value;
            var birthDistrictValue = lineColumnDtos.Single(e => e.Index == 18).Value;
            var birthMunicipalityValue = lineColumnDtos.Single(e => e.Index == 19).Value;

            if (string.IsNullOrWhiteSpace(birthCountryValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.BirthCountryRequired);
            }
            else
            {
                if (!ValidatePropertiesStatic.IsValidLength(birthCountryValue, minLength: 2, maxLength: 2))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.BirthCountryLengthRestriction);
                }
                else if (!countryCodesHashSet.Contains(birthCountryValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidBirthCountry);
                }
            }

            if (string.IsNullOrWhiteSpace(birthSettlementValue))
            {
                if (birthCountryValue == CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.BirthSettlementRequired);
                }
            }
            else
            {
                if (!ValidatePropertiesStatic.IsValidLength(birthSettlementValue, minLength: 5, maxLength: 5))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.BirthSettlementLengthRestriction);
                }
                else if (birthCountryValue == CountryConstants.BulgariaCode && !settlementCodesHashSet.Contains(birthSettlementValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidBirthSettlement);
                }
                else if (birthCountryValue != CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidBirthSettlementByCountry);
                }
            }

            if (!string.IsNullOrWhiteSpace(birthDistrictValue))
            {
                if (!ValidatePropertiesStatic.IsValidLength(birthDistrictValue, minLength: 3, maxLength: 3))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.BirthDistrictLengthRestriction);
                }
                else if (birthCountryValue == CountryConstants.BulgariaCode && !districtCodesHashSet.Contains(birthDistrictValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidBirthDistrict);
                }
                else if (birthCountryValue != CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidBirthDistrictByCountry);
                }
            }

            if (!string.IsNullOrWhiteSpace(birthMunicipalityValue))
            {
                if (!ValidatePropertiesStatic.IsValidLength(birthMunicipalityValue, minLength: 5, maxLength: 5))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.BirthMunicipalityLengthRestriction);
                }
                else if (birthCountryValue == CountryConstants.BulgariaCode && !municipalityCodesHashSet.Contains(birthMunicipalityValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidBirthMunicipality);
                }
                else if (birthCountryValue != CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidBirthMunicipalityByCountry);
                }
            }

            var foreignerBirthSettlementValue = lineColumnDtos.Single(e => e.Index == 20).Value;

            if (string.IsNullOrWhiteSpace(foreignerBirthSettlementValue))
            {
                if (!string.IsNullOrWhiteSpace(birthCountryValue) && birthCountryValue != CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ForeignerBirthSettlementRequired);
                }
            }
            else
            {
                if (!ValidatePropertiesStatic.IsValidLength(foreignerBirthSettlementValue, minLength: 3, maxLength: 50))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ForeignerBirthSettlementRestriction);
                }
                else if (!ValidatePropertiesStatic.IsValidForeignerBirthSettlement(foreignerBirthSettlementValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ForeignerBirthSettlementCyrillic);
                }
                else if (birthCountryValue == CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidForeignerBirthSettlementByCountry);
                }
            }

            var residenceCountryValue = lineColumnDtos.Single(e => e.Index == 21).Value;
            var residenceSettlementValue = lineColumnDtos.Single(e => e.Index == 22).Value;
            var residenceDistrictValue = lineColumnDtos.Single(e => e.Index == 23).Value;
            var residenceMunicipalityValue = lineColumnDtos.Single(e => e.Index == 24).Value;

            if (string.IsNullOrWhiteSpace(residenceCountryValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.ResidenceCountryRequired);
            }
            else
            {
                if (!ValidatePropertiesStatic.IsValidLength(residenceCountryValue, minLength: 2, maxLength: 2))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ResidenceCountryLengthRestriction);
                }
                else if (!countryCodesHashSet.Contains(residenceCountryValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidResidenceCountry);
                }
            }

            if (string.IsNullOrWhiteSpace(residenceSettlementValue))
            {
                if (residenceCountryValue == CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ResidenceSettlementRequired);
                }
            }
            else
            {
                if (!ValidatePropertiesStatic.IsValidLength(residenceSettlementValue, minLength: 5, maxLength: 5))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ResidenceSettlementLengthRestriction);
                }
                else if (residenceCountryValue == CountryConstants.BulgariaCode && !settlementCodesHashSet.Contains(residenceSettlementValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidResidenceSettlement);
                }
                else if (residenceCountryValue != CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidResidenceSettlementByCountry);
                }
            }

            if (!string.IsNullOrWhiteSpace(residenceDistrictValue))
            {
                if (!ValidatePropertiesStatic.IsValidLength(residenceDistrictValue, minLength: 3, maxLength: 3))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ResindeceDistrictLengthRestriction);
                }
                else if (residenceCountryValue == CountryConstants.BulgariaCode && !districtCodesHashSet.Contains(residenceDistrictValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidResidenceDistrict);
                }
                else if (residenceCountryValue != CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidResidenceDistrictByCountry);
                }
            }

            if (!string.IsNullOrWhiteSpace(residenceMunicipalityValue))
            {
                if (!ValidatePropertiesStatic.IsValidLength(residenceMunicipalityValue, minLength: 5, maxLength: 5))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ResidenceMunicipalityLengthRestriction);
                }
                else if (residenceCountryValue == CountryConstants.BulgariaCode && !municipalityCodesHashSet.Contains(residenceMunicipalityValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidResidenceMunicipality);
                }
                else if (residenceCountryValue != CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidResidenceMunicipalityByCountry);
                }
            }

            var postCodeValue = lineColumnDtos.Single(e => e.Index == 25).Value;

            if (!string.IsNullOrWhiteSpace(postCodeValue))
            {
                if (!string.IsNullOrWhiteSpace(residenceCountryValue) && residenceCountryValue != CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidPostCodeByCountry);
                }
                else if (!ValidatePropertiesStatic.IsValidLength(postCodeValue, minLength: 4, maxLength: 4))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.PostCodeLengthRestriction);
                }
                else if (!ValidatePropertiesStatic.NumbersOnly(postCodeValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidPostCode);
                }
            }

            var residenceAddressValue = lineColumnDtos.Single(e => e.Index == 26).Value;

            if (!string.IsNullOrWhiteSpace(residenceAddressValue))
            {
                if (!string.IsNullOrWhiteSpace(residenceCountryValue) && residenceCountryValue != CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidResindeceAddressByCountry);
                }
                else if (!ValidatePropertiesStatic.IsValidLength(residenceAddressValue, minLength: 8, maxLength: 255))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ResidenceAddressLengthRestriction);
                }
                else if (!ValidatePropertiesStatic.IsValidResidenceAddress(residenceAddressValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidResidenceAddress);
                }
            }

            var foreignerResidenceAddressValue = lineColumnDtos.Single(e => e.Index == 27).Value;

            if (!string.IsNullOrWhiteSpace(foreignerResidenceAddressValue))
            {
                if (!string.IsNullOrWhiteSpace(residenceCountryValue) && residenceCountryValue == CountryConstants.BulgariaCode)
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidForeignerResindeceAddressByCountry);
                }
                else if (!ValidatePropertiesStatic.IsValidLength(foreignerResidenceAddressValue, minLength: 8, maxLength: 255))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ForeignerResidenceAddressLengthRestriction);
                }
                else if (!ValidatePropertiesStatic.IsValidForeignerResidenceAddress(foreignerResidenceAddressValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidForeignerResidenceAddress);
                }
            }

            var citizenshipValue = lineColumnDtos.Single(e => e.Index == 28).Value;
            var secondCitizenshipValue = lineColumnDtos.Single(e => e.Index == 29).Value;

            if (string.IsNullOrWhiteSpace(citizenshipValue))
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.CitizenshipRequired);
            }
            else
            {
                if (citizenshipValue != CountryConstants.NoCitizenship && !ValidatePropertiesStatic.IsValidLength(citizenshipValue, minLength: 2, maxLength: 2))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.CitizenshipLengthRestriction);
                }
                else if (citizenshipValue != CountryConstants.NoCitizenship && !countryCodesHashSet.Contains(citizenshipValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidCitizenship);
                }
            }

            if (!string.IsNullOrWhiteSpace(secondCitizenshipValue))
            {
                if (secondCitizenshipValue != CountryConstants.NoCitizenship && !ValidatePropertiesStatic.IsValidLength(secondCitizenshipValue, minLength: 2, maxLength: 2))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.SecondCitizenshipLengthRestriction);
                }
                else if (secondCitizenshipValue != CountryConstants.NoCitizenship && !countryCodesHashSet.Contains(secondCitizenshipValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidSecondCitizenship);
                }
            }

            if (!string.IsNullOrWhiteSpace(citizenshipValue) && !string.IsNullOrWhiteSpace(secondCitizenshipValue)
                && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.CitizenshipLengthRestriction)
                && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.InvalidCitizenship)
                && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.SecondCitizenshipLengthRestriction)
                && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.InvalidSecondCitizenship)
                && citizenshipValue == secondCitizenshipValue)
            {
                txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidCitizenships);
            }

            return txtValidationErrorCodes;
        }

        private void ValidateUanWithIdentificator(List<TxtValidationErrorCode> txtValidationErrorCodes, string uanValue, string uinValue, string foreignerNumberValue, string idnNumberValue, bool isValidNotEmptyUin, bool isValidNotEmptyForeignerNum)
        {
            var identifiers = new Dictionary<string, string>
                {
                    { "UIN", uinValue },
                    { "ForeignerNumber", foreignerNumberValue },
                    { "IDN", idnNumberValue },
                };

            foreach (var identifier in identifiers.Where(e => !string.IsNullOrWhiteSpace(e.Value)))
            {
                switch (identifier.Key)
                {
                    case "UIN":
                        if (isValidNotEmptyUin && personIdentifierUanDict[uanValue].Uin == identifier.Value)
                        {
                            return;
                        }
                        break;
                    case "ForeignerNumber":
                        if (isValidNotEmptyForeignerNum && personIdentifierUanDict[uanValue].ForeignerNumber == identifier.Value)
                        {
                            return;
                        }
                        break;
                    case "IDN":
                        if (txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.InvalidIdnNumber) && personIdentifierUanDict[uanValue].IdnNumber == identifier.Value)
                        {
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }

            txtValidationErrorCodes.Add(TxtValidationErrorCode.UanWithIdentificatorNotFound);
        }

        private bool ValidatePersonIdnByBirthDateAndCountry(List<TxtValidationErrorCode> txtValidationErrorCodes, string idnNumberValue, string birthDateValue, string firstNameCyrillicValue, string birthCountryValue)
        {
            if (!string.IsNullOrWhiteSpace(idnNumberValue) && !txtValidationErrorCodes.Any(e => e == TxtValidationErrorCode.InvalidIdnNumber))
            {
                var firstNameCyrillicToLower = firstNameCyrillicValue.ToLower();

                if (!string.IsNullOrWhiteSpace(birthDateValue) || !string.IsNullOrWhiteSpace(birthCountryValue) || !string.IsNullOrWhiteSpace(firstNameCyrillicToLower))
                {
                    if (personIdnBirthPlaceHashSet.Any(e => e.BirthCountry == birthCountryValue && e.BirthDate == birthDateValue && e.FirstName.StartsWith(firstNameCyrillicToLower[0])))
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        private void ValidateIdnNumber(List<TxtValidationErrorCode> txtValidationErrorCodes, string birthDateValue, string firstNameCyrillicValue, string birthCountryValue)
        {
            var firstNameCyrillicToLower = firstNameCyrillicValue.ToLower();

            if (!string.IsNullOrWhiteSpace(birthDateValue) || !string.IsNullOrWhiteSpace(birthCountryValue) || !string.IsNullOrWhiteSpace(firstNameCyrillicToLower))
            {
                if (personIdnBirthPlaceHashSet.Any(e => e.BirthCountry == birthCountryValue && e.BirthDate == birthDateValue && e.FirstName.StartsWith(firstNameCyrillicToLower[0])))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.IdnBirthPlaceExists);
                }
                else if (txtPersonIdnBirthPlaceHashSet.Any(e => e.BirthCountry == birthCountryValue && e.BirthDate == birthDateValue && e.FirstName.StartsWith(firstNameCyrillicToLower[0])))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.IdnBirthPlaceNotUniqueInTxt);
                }
                else
                {
                    txtPersonIdnBirthPlaceHashSet.Add(new Tuple<string, string, string>(birthCountryValue, birthDateValue, firstNameCyrillicToLower).ToValueTuple());
                }
            }
        }

        private bool ValidateForeignerNumber(List<TxtValidationErrorCode> txtValidationErrorCodes, string foreignerNumberValue)
        {
            if (!string.IsNullOrWhiteSpace(foreignerNumberValue))
            {
                if (foreignerNumberValue.Length != 10 || !ValidatePropertiesStatic.IsValidForeignerNumberCheckSum(foreignerNumberValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidForeignerNumber);
                }
                else if (txtPersonForeignerNumberHashSet.Contains(foreignerNumberValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.ForeignerNumberNotUniqueInTxt);
                }
                else
                {
                    txtPersonForeignerNumberHashSet.Add(foreignerNumberValue);
                    return true;
                }
            }

            return false;
        }

        private bool ValidateUin(List<TxtValidationErrorCode> txtValidationErrorCodes, string uinValue)
        {
            if (!string.IsNullOrWhiteSpace(uinValue))
            {
                if (uinValue.Length != 10 || !ValidatePropertiesStatic.IsValidUinCheckSum(uinValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidUin);
                }
                else if (txtPersonUinHashSet.Contains(uinValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.UinNotUniqueInTxt);
                }
                else
                {
                    txtPersonUinHashSet.Add(uinValue);
                    return true;
                }
            }

            return false;
        }

        private bool ValidateUan(List<TxtValidationErrorCode> txtValidationErrorCodes, string uanValue)
        {
            if (!string.IsNullOrWhiteSpace(uanValue))
            {
                if (!ValidatePropertiesStatic.IsValidUan(uanValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.InvalidUan);
                }
                else if (!ValidatePropertiesStatic.IsValidStringHashSet(uanValue.ToUpper(), personUanHashSet))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.UanNotFound);
                }
                else if (txtPersonUanHashSet.Contains(uanValue))
                {
                    txtValidationErrorCodes.Add(TxtValidationErrorCode.UanNotUniqueInTxt);
                }
                else
                {
                    txtPersonUanHashSet.Add(uanValue);
                    return true;
                }
            }

            return false;
        }
    }
}
