using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS
{
    public class RegexPatterns
    {

        /// <summary>
        /// <p></p>
        /// </summary>
        public const string HtmlTagEmpty = @"<([^>/][^>]*)>((&nbsp;|\n|\t|\r)*|\s*)</\1>";


        public const string Alpha = @"^[a-zA-Z]*$";
        public const string AlphaUpperCase = @"^[A-Z]*$";
        public const string AlphaLowerCase = @"^[a-z]*$";
        public const string AlphaNumeric = @"^[a-zA-Z0-9]*$";
        public const string AlphaNumericSpace = @"^[a-zA-Z0-9 ]*$";
        public const string AlphaNumericSpaceDash = @"^[a-zA-Z0-9 \-]*$";
        public const string AlphaNumericSpaceDashUnderscore = @"^[a-zA-Z0-9 \-_]*$";
        public const string AlphaNumericSpaceDashUnderscorePeriod = @"^[a-zA-Z0-9\. \-_]*$";
        public const string AlphaNumericWithoutSpace = @"((([a-z]|[A-Z]|[\u0600-\u06FF])\S[0-9]))|(([0-9]\S([a-z]|[A-Z]|[\u0600-\u06FF])))";

        public const string Number = @"([0-9])";
        public const string SocialSecurity = @"\d{3}[-]?\d{2}[-]?\d{4}";

        public const string Url = @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";
        public const string ZipCodeUs = @"\d{5}";
        public const string ZipCodeUsWithFour = @"\d{5}[-]\d{4}";
        public const string ZipCodeUsWithFourOptional = @"\d{5}([-]\d{4})?";
        public const string PhoneUs = @"\d{3}[-]?\d{3}[-]?\d{4}";

        public const string Email = @"^([A-Z|a-z|0-9](\.|_|-){0,1})+[A-Z|a-z|0-9]\@([A-Z|a-z|0-9|-])+((\.){0,1}[A-Z|a-z|0-9]){2}\.[a-z]{2,3}$";

        public const string Mobile = @"^[0]\d{0}[9]\d{1}\d{8}$";

        public const string Emoji = @"[\uD800-\uDBFF]|[\u2702-\u27B0]|[\uF680-\uF6C0]|[\u24C2-\uF251]";

        public const string Spaces = @"([\u0009,\u000A,\u000B,\u000C,\u000D,\u001C,\u001D,\u001E,\u001F,\u00A0,\u1680,\u180E,\u2000,\u2001,\u2002,\u2003,\u2004,\u2005,\u2006,\u2007,\u2008,\u2009,\u200A,\u200B,\u200C,\u202F,\u205F,\u3000,\uFEFF])|(\s)";

        public const string Symbols = @"[!""#$%&'()*+,\-،./،:؛;<=>؟?@[\\\]^_`–{|}~«»]";


    }
}
