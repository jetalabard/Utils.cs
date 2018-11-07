using System;
using System.Text.RegularExpressions;
using Utils.Encryption;

namespace Utils
{
    public enum EncryptionMode
    {
        SHA_256,
        SHA_512
    }

    public static class StringExtensions
    {
        /// <summary>
        /// check if string is number
        /// </summary>
        /// <returns></returns>
        public static bool IsNumber(this string inputString)
        {
            int result;
            return int.TryParse(inputString, out result);
        }

        /// <summary>
        /// convert string to number
        /// </summary>
        /// <returns></returns>
        public static int ToNumber(this string inputString)
        {
            int result;
            if (!int.TryParse(inputString, out result))
            {
                throw new InvalidOperationException("string is not a number");
            }
            return result;
        }

        /// <summary>
        /// allows to check if string is a valid email
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static bool IsEmail(this string inputString)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(inputString);
                return addr.Address == inputString;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// transform string to hashed string whith sha 256
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string Encrypt(this string inputString, EncryptionMode mode = EncryptionMode.SHA_256)
        {
            string encrypt = string.Empty;
            if (mode == EncryptionMode.SHA_256)
            {
                encrypt = Sha.GenerateSHA256String(inputString);
            }
                
            if (mode == EncryptionMode.SHA_512)
            {
                encrypt = Sha.GenerateSHA512String(inputString);
            }
                
            return encrypt;
        }

        /// <summary>
        /// allows to check if string contains pattern string
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static bool IsMatch(this string inputString, string pattern)
        {
            return inputString.Contains(pattern);
        }

        /// <summary>
        /// allows to check if string contains regex
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static bool IsMatchRegex(this string inputString, Regex reg)
        {
            return reg.IsMatch(inputString);
        }

        /// <summary>
        /// allows to check if string contains regex string
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static bool IsMatchRegexString(this string inputString, string regexString)
        {
            return new Regex(regexString).IsMatch(inputString);
        }

        /// <summary>
        /// allows to split with string
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="regexString"></param>
        /// <returns></returns>
        public static string [] Split(this string inputString, string regexString)
        {
            return inputString.Split(new[] { regexString }, StringSplitOptions.None);
        }
    }
}
