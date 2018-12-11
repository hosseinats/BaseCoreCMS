using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CMS
{
    public static class CommonExtensions
    {
        public static int PageCount(this int totalCount, int pageSize)
        {

            if (pageSize <= 0 || totalCount <= 0)
            {
                new Exception("PageSize zirooo");
            }

            int totalPage = totalCount / pageSize;

            //add the last page, ugly
            if (totalCount % pageSize != 0) totalPage++;

            return totalPage;
        }

        public static string ToTitleInUrl(this string title)
        {

            var regexSpaces = new Regex(RegexPatterns.Spaces, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            title = regexSpaces.Replace(title, " ");

            var regexSymbol = new Regex(RegexPatterns.Symbols, RegexOptions.Multiline | RegexOptions.IgnoreCase);

            var titleArticle = regexSymbol.Replace(title, "-").Replace(" ", "-");




            //titleArticle = new Regex(@"(.)\1{2,}", RegexOptions.IgnoreCase).Replace(titleArticle, "$1");
            titleArticle = new Regex(@"(-)\1{1,}", RegexOptions.IgnoreCase).Replace(titleArticle, "$1");

            titleArticle = titleArticle.Trim().TrimEnd('-').TrimStart('-');

            return titleArticle;
        }


        public static PersianDateTime ToPersianDateTime(this DateTime datetime)
        {
            return new PersianDateTime(datetime);
        }

        public static PersianDateTime ToPersianDateTime(this int date)
        {
            return new PersianDateTime(date);
        }

        public static string ToPrice(this string i)
        {
            if (i.Length <= 0) return "0";

            string s1 = i;
            int div = s1.Length / 3;
            int mod = s1.Length % 3;
            string[] list = new string[div + (mod == 0 ? 0 : 1)];
            for (int j = (mod == 0 ? div - 1 : div); j >= 1; j--)
            {
                list[j] = s1.Substring((((mod == 0 ? j : j - 1)) * 3) + (mod), 3);
                s1 = s1.Substring(0, s1.Length - 3);
            }
            list[0] = s1.Substring(0, (mod == 0 ? 3 : mod));

            return string.Join(",",list);
        }

        /// <summary>
        /// converts one type to another
        /// Example:
        /// var age = "28";
        /// var intAge = age.To<int>();
        /// var doubleAge = intAge.To<double>();
        /// var decimalAge = doubleAge.To<decimal>();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T To<T>(this IConvertible value)
        {
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                        return default(T);

                    return (T)Convert.ChangeType(value, u);
                }
                else
                {
                    if (value == null || value.Equals(""))
                        return default(T);

                    return (T)Convert.ChangeType(value, t);
                }
            }

            catch
            {
                return default(T);
            }
        }

        public static T To<T>(this IConvertible value, IConvertible ifError)
        {
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                        return (T)ifError;

                    return (T)Convert.ChangeType(value, u);
                }
                else
                {
                    if (value == null || value.Equals(""))
                        return (T)(ifError.To<T>());

                    return (T)Convert.ChangeType(value, t);
                }
            }
            catch
            {

                return (T)ifError;
            }

        }

        /// <summary>
        /// c# version of "Between" clause of sql query.
        /// Example:
        /// DateTime today = DateTime.Now;
        /// var from = new DateTime(2012, 2, 1);
        /// var to = new DateTime(2012, 12, 20);
        ///
        /// bool isBetween = today.Between(from, to);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="from">Min</param>
        /// <param name="to">Max</param>
        /// <returns></returns>
        public static bool Between<T>(this T value, T from, T to) where T : IComparable<T>
        {
            return value.CompareTo(from) >= 0 && value.CompareTo(to) <= 0;
        }

        /// <summary>
        /// C# version of In clause of sql query.
        /// Example:
        /// string value = "net";
        ///    bool isIn = value.In("dot", "net", "languages"); //true
        ///    isIn = value.In("dot", "note", "languages"); //false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool In<T>(this T value, params T[] list)
        {
            return list.Contains(value);
        }


        /// <summary>
        /// Converts any type to another.
        /// Example:
        /// string a = "1234";
        /// int b = a.ChangeType<int>(); //Successful conversion to int (b=1234)
        /// string c = b.ChangeType<string>(); //Successful conversion to string (c="1234")
        /// string d = "foo";
        /// int e = d.ChangeType<int>(); //Exception System.InvalidCastException
        /// int f = d.ChangeType(0); //Successful conversion to int (f=0)
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <param name="source"></param>
        /// <param name="returnValueIfException"></param>
        /// <returns></returns>
        public static TU ChangeType<TU>(this object source, TU returnValueIfException)
        {
            try
            {
                return source.ChangeType<TU>();
            }
            catch
            {
                return returnValueIfException;
            }
        }

        /// <summary>
        /// Converts any type to another.
        /// Example:
        /// string a = "1234";
        /// int b = a.ChangeType<int>(); //Successful conversion to int (b=1234)
        /// string c = b.ChangeType<string>(); //Successful conversion to string (c="1234")
        /// string d = "foo";
        /// int e = d.ChangeType<int>(); //Exception System.InvalidCastException
        /// int f = d.ChangeType(0); //Successful conversion to int (f=0)
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TU ChangeType<TU>(this object source)
        {
            if (source is TU)
                return (TU)source;

            var destinationType = typeof(TU);
            if (destinationType.IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
                destinationType = new NullableConverter(destinationType).UnderlyingType;

            return (TU)Convert.ChangeType(source, destinationType);
        }


        /// <summary>
        /// get one , or more random get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IList<T> Shuffle<T>(this IList<T> list, int size = 1)
        {
            var r = new Random();
            var shuffledList =
                list.
                    Select(x => new { Number = r.Next(), Item = x }).
                    OrderBy(x => x.Number).
                    Select(x => x.Item).
                    Take(size); // Assume first @size items is fine

            return shuffledList.ToList();
        }
        public static T Shuffle<T>(this IList<T> list)
        {
            if (list == null) throw new ArgumentNullException("list");

            var r = new Random();
            var shuffledList =
                list.
                    Select(x => new { Number = r.Next(), Item = x }).
                    OrderBy(x => x.Number).
                    Select(x => x.Item).
                    Take(1); // Assume first @size items is fine

            return shuffledList.First();
        }

        /// <summary>
        ///  OrderBy Random
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IEnumerable<t> Randomize<t>(this IEnumerable<t> target)
        {
            Random r = new Random();

            return target.OrderBy(x => (r.Next()));
        }



        /// <summary>
        /// تبدیل اعداد انگلیسی به فارسی
        /// </summary>
        /// <param name="s">4547654</param>
        /// <returns></returns>
        public static string ToPersianNumbers(this object s)
        {
            return ToPersianNumbers(s.ToString());
        }
        public static string ToPersianNumbers(this string s)
        {
            return s.Replace("0", "۰")
                .Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹");
        }

        public static string ToEnglishNumbers(this string s)
        {
            return s.Replace((char)1632, '0')
                    .Replace((char)1776, '0')

                    .Replace((char)1633, '1')
                    .Replace((char)1777, '1')

                    .Replace((char)1634, '2')
                    .Replace((char)1778, '2')

                    .Replace((char)1635, '3')
                    .Replace((char)1779, '3')

                    .Replace((char)1636, '4')
                    .Replace((char)1780, '4')

                    .Replace((char)1637, '5')
                    .Replace((char)1781, '5')

                    .Replace((char)1638, '6')
                    .Replace((char)1782, '6')

                    .Replace((char)1639, '7')
                    .Replace((char)1783, '7')

                    .Replace((char)1640, '8')
                    .Replace((char)1784, '8')

                    .Replace((char)1641, '9')
                    .Replace((char)1785, '9');

        }
    }
}
