using DVLD.Model;
using DVLD.Model.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace DVLD.Application
{
    public static class InputValidator
    {
       private static readonly Regex EmailRegex  = new Regex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$",RegexOptions.Compiled) ;

        public enum enStringType
        {
            Email
        }
        public enum enCharType
        {
            Letters,
            Digits,
            LettersAndDigits,
            LettersWithSpace,
            DigitsWithDot
        }

        public static bool IsAllowedChar(char c, enCharType type = enCharType.Letters)
        {
            if (char.IsControl(c))
                return true;

            switch (type)
            {
                case enCharType.Letters:
                    return (char.IsLetter(c));

                case enCharType.Digits:
                    return (char.IsDigit(c));

                case enCharType.DigitsWithDot:
                    return (char.IsDigit(c) || c == '.');

                case enCharType.LettersAndDigits:
                    return (char.IsLetter(c) || char.IsDigit(c));

                case enCharType.LettersWithSpace:
                    return (char.IsLetter(c) || c == ' ');

                default:
                    return false;
            }
        }

        public static bool ValidateStringByType(string s, enStringType type, bool allowEmpty = false)
        {
            if (string.IsNullOrWhiteSpace(s))
                return allowEmpty;

            switch (type)
            {
                case enStringType.Email:
                    return EmailRegex.IsMatch(s.Trim());

                default:
                    return false;
            }
        }

    }
}
