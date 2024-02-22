using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Infrastructure.Constants
{
    public static class ValidatePropertiesStatic
    {
        public static bool IsValidCyrillic(string value)
        {
            return new Regex(@"^[А-Яа-я-\s]+$").IsMatch(value);
        }

        public static bool IsValidWithoutLatin(string value)
        {
            return new Regex(@"^[^A-Za-z]+$").IsMatch(value);
        }

        public static bool IsValidWithoutCyrillic(string value)
        {
            return new Regex(@"^[^А-Яа-я]+$").IsMatch(value);
        }

        public static bool IsValidLatin(string value)
        {
            return new Regex(@"^[A-Za-z-\s]+$").IsMatch(value);
        }

        public static bool IsValidUppercaseLatin(string value)
        {
            return new Regex(@"^[A-Z-\s]+$").IsMatch(value);
        }

        public static bool IsValidEmail(string value)
        {
            return new EmailAddressAttribute().IsValid(value);
        }
        public static bool IsValidPhoneNumber(string value)
        {
            return new Regex(@"^\+\d{1,12}$").IsMatch(value) || (value.Length > 8 && value.Length < 19 && new Regex(@"^[0-9-\s\+]+$").IsMatch(value));
        }

        public static bool NumbersOnly(string value)
        {
            return new Regex(@"^[0-9]+$").IsMatch(value);
        }

        public static bool IsValidBooleanNumber(string value)
        {
            return value == "0" || value == "1";
        }

        public static bool IsValidForeignerBirthSettlement(string value)
        {
            return new Regex(@"^[А-Яа-я\s]+$").IsMatch(value);
        }

        public static bool IsValidResidenceAddress(string value)
        {
            return new Regex(@"^[А-Яа-я0-9-№."",\s]+$").IsMatch(value);
        }

        public static bool IsValidForeignerResidenceAddress(string value)
        {
            return new Regex(@"^[A-Za-z0-9-#№."",\s]+$").IsMatch(value);
        }

        public static bool IsValidUinCheckSum(string value)
        {
            if (!value.All(char.IsDigit))
            {
                return false;
            }

            int[] weights = new int[] { 2, 4, 8, 5, 10, 9, 7, 3, 6 };
            int mod = 11;

            var sum = 0;
            var checkSum = int.Parse(value.Substring(9, 1));

            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(value.Substring(i, 1)) * weights[i];
            }

            var validChecksum = sum % mod;

            if (validChecksum >= 10)
            {
                validChecksum = 0;
            }

            return checkSum == validChecksum;
        }

        public static bool IsValidForeignerNumberCheckSum(string value)
        {
            if (!value.All(char.IsDigit) || value.Length != 10)
            {
                return false;
            }

            int[] weights = new int[] { 21, 19, 17, 13, 11, 9, 7, 3, 1 };
            int mod = 10;

            var sum = 0;
            var checkSum = int.Parse(value.Substring(9, 1));

            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(value.Substring(i, 1)) * weights[i];
            }

            var validChecksum = sum % mod;

            if (validChecksum >= 10)
            {
                validChecksum = 0;
            }

            return checkSum == validChecksum;
        }

        public static bool IsValidIdnNumber(string idnNumber)
        {
            return new Regex(@"^[A-Z0-9-\s]+$").IsMatch(idnNumber);
        }

        public static bool IsValidUan(string uan)
        {
            var isValidRegex = new Regex(@"^[a-zA-Z]{2}\d{5}$").IsMatch(uan);
            return uan.Length == 7 && isValidRegex;
        }

        public static bool IsValidDateBetween(string value, int? fromYear = null, int? toYear = null)
        {
            DateTime dt;
            var isValidDate = DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            bool isValidBetweenYears = true;

            if (isValidDate)
            {
                if (fromYear.HasValue && dt.Year < fromYear)
                {
                    isValidBetweenYears = false;
                }

                if (toYear.HasValue && dt.Year > toYear)
                {
                    isValidBetweenYears = false;
                }

                return isValidDate && isValidBetweenYears;
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidBirthDateUin(string date, string uin)
        {
            DateTime dt;
            DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

            var month = dt.Year > 1999 ? dt.Month + 40 : dt.Month;

            var dateCheck = $"{dt.ToString("yy")}{month:00}{dt.Day:00}";
            var uinCheck = uin[0..^4];

            return dateCheck == uinCheck;
        }

        public static bool IsValidGenderTypeUin(string value, string uin)
        {
            var uinGenderValue = int.Parse(uin[8].ToString());

            return (uinGenderValue % 2) + 1 == int.Parse(value);
        }

        public static bool IsValidStringHashSet(string value, HashSet<string> hashSet)
        {
            return hashSet.Contains(value);
        }

        public static bool IsValidLength(string value, int minLength, int maxLength)
        {
            if (value.Length < minLength || value.Length > maxLength)
            {
                return false;
            }

            return true;
        }
    }
}
