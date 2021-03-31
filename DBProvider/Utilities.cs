using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using EntityModel;

namespace DBProvider
{
    public static class Utilities
    {

        public static string toPersianNumber(this string input)
        {
            if (!String.IsNullOrEmpty(input) && input.Any(char.IsDigit))
            {
                string[] persian = new string[10] {"۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"};

                for (int j = 0; j < persian.Length; j++)
                    input = input.Replace(j.ToString(), persian[j]);

                return input;
            }
            else
                return input;
        }

        public static string toEnglishNumber(this string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
            {

                input = input.Replace(persian[j], j.ToString());
            }

            return input;
        }

        public static DateTime ToGeorgianDateTime(this string datetime)
        {
            try
            {
                string[] datetimearray = new string[3];
                if (datetime.Contains("/"))
                    datetimearray = datetime.toEnglishNumber().Split('/');
                else if (datetime.Contains("-"))
                    datetimearray = datetime.toEnglishNumber().Split('-');
                else if (datetime.Contains("."))
                    datetimearray = datetime.toEnglishNumber().Split('.');

                int year = Convert.ToInt32(datetimearray[0]);
                int month = Convert.ToInt32(datetimearray[1]);
                int day = Convert.ToInt32(datetimearray[2]);
                return new DateTime(year, month, day, new PersianCalendar());
            }
            catch
            {
                throw new Exception("Datetime is not in correct format!");
            }
        }

        private static PropertyInfo[] _PropertyInfos = null;
        /// <summary>
        /// jhjdfggfhdgfjhdgfkjhgfjhgdkfhgdfgdhfgkdhgfjh
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToPropertiesString(this Object c)
        {

            if (_PropertyInfos == null)
                _PropertyInfos = c.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(c, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }
        public static T DeepClone<T>(T _object)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, _object);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
        public static T DeepClone<T>(this Object t)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, t);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
        public static string ToPersianTime(this DateTime d)
        {
            System.Globalization.PersianCalendar shamsi = new System.Globalization.PersianCalendar();
            return string.Format("{0}/{1}/{2}", shamsi.GetYear(d).ToString("00"), shamsi.GetMonth(d).ToString("00"), shamsi.GetDayOfMonth(d).ToString("00"));
        }
        public static string ToUppercaseFirst(this String s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        public static string ToPassword(this String str)
        {
            str = MD5Of(sh1of(str));
            return str;
        }
        private static string sh1of(string text)
        {
            return HashOf<SHA1CryptoServiceProvider>(text, Encoding.Unicode);
        }
        private static string MD5Of(string text)
        {
            return HashOf<MD5CryptoServiceProvider>(text, Encoding.Unicode);
        }
        private static string HashOf<TP>(string text, Encoding enc)
         where TP : HashAlgorithm, new()
        {
            var buffer = enc.GetBytes(text);
            var provider = new TP();
            return BitConverter.ToString(provider.ComputeHash(buffer)).Replace("-", "");
        }  
        public static string GetContentBase64(this byte[] t,string contrnt)
        {
            var base64 = Convert.ToBase64String(t);
            return string.Format($"data:{contrnt};base64,{base64}");
        }
    }
}
