﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace CMS
{
    /// <summary>
    /// Created By Mohammad Dayyan, @mdssoft, mds_soft@yahoo.com
    /// 1393/09/14
    /// </summary>
    [Serializable]
    public struct MDPersianDateTime :
        ISerializable, IFormattable,
        IComparable<MDPersianDateTime>, IComparable<DateTime>,
        IEquatable<MDPersianDateTime>, IEquatable<DateTime>
    {
        #region properties and fields

        private static PersianCalendar _persianCalendar;
        private static PersianCalendar PersianCalendar
        {
            get
            {
                if (_persianCalendar != null) return _persianCalendar;
                _persianCalendar = new PersianCalendar();
                return _persianCalendar;
            }
        }
        private readonly DateTime _dateTime;

        /// <summary>
        /// آیا اعداد در خروجی به صورت انگلیسی نمایش داده شوند؟
        /// </summary>
        public bool EnglishNumber { get; set; }

        /// <summary>
        /// سال شمسی
        /// </summary>
        public int Year
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return DateTime.MinValue.Year;
                return PersianCalendar.GetYear(_dateTime);
            }
        }

        /// <summary>
        /// ماه شمسی
        /// </summary>
        public int Month
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return DateTime.MinValue.Month;
                return PersianCalendar.GetMonth(_dateTime);
            }
        }

        /// <summary>
        /// نام فارسی ماه
        /// </summary>
        public string MonthName
        {
            get { return ((MDPersianDateTimeMonthEnum)Month).ToString(); }
        }

        /// <summary>
        /// روز ماه
        /// </summary>
        public int Day
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return DateTime.MinValue.Day;
                return PersianCalendar.GetDayOfMonth(_dateTime);
            }
        }

        /// <summary>
        /// روز هفته
        /// </summary>
        public DayOfWeek DayOfWeek
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return DateTime.MinValue.DayOfWeek;
                return PersianCalendar.GetDayOfWeek(_dateTime);
            }
        }

        /// <summary>
        /// روز هفته یا ایندکس شمسی
        /// <para />
        /// شنبه دارای ایندکس صفر است
        /// </summary>
        /// 

        public PersianDayOfWeek PersianDayOfWeek
        {
            get
            {
                var dayOfWeek = PersianCalendar.GetDayOfWeek(_dateTime);
                PersianDayOfWeek persianDayOfWeek;
                switch (dayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        persianDayOfWeek = PersianDayOfWeek.Sunday;
                        break;
                    case DayOfWeek.Monday:
                        persianDayOfWeek = PersianDayOfWeek.Monday;
                        break;
                    case DayOfWeek.Tuesday:
                        persianDayOfWeek = PersianDayOfWeek.Tuesday;
                        break;
                    case DayOfWeek.Wednesday:
                        persianDayOfWeek = PersianDayOfWeek.Wednesday;
                        break;
                    case DayOfWeek.Thursday:
                        persianDayOfWeek = PersianDayOfWeek.Thursday;
                        break;
                    case DayOfWeek.Friday:
                        persianDayOfWeek = PersianDayOfWeek.Friday;
                        break;
                    case DayOfWeek.Saturday:
                        persianDayOfWeek = PersianDayOfWeek.Saturday;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return persianDayOfWeek;
            }
        }

        /// <summary>
        /// ساعت
        /// </summary>
        public int Hour
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return 12;
                return PersianCalendar.GetHour(_dateTime);
            }
        }

        /// <summary>
        /// ساعت دو رقمی
        /// </summary>
        public int ShortHour
        {
            get
            {
                int shortHour;
                if (Hour > 12)
                    shortHour = Hour - 12;
                else
                    shortHour = Hour;
                return shortHour;
            }
        }

        /// <summary>
        /// دقیقه
        /// </summary>
        public int Minute
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return 0;
                return PersianCalendar.GetMinute(_dateTime);
            }
        }

        /// <summary>
        /// ثانیه
        /// </summary>
        public int Second
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return 0;
                return PersianCalendar.GetSecond(_dateTime);
            }
        }

        /// <summary>
        /// میلی ثانیه
        /// </summary>
        public int MiliSecond
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return 0;
                return (int)PersianCalendar.GetMilliseconds(_dateTime);
            }
        }

        /// <summary>
        /// تعداد روز در ماه
        /// </summary>
        public int GetMonthDays
        {
            get
            {
                int days;
                switch (Month)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        days = 31;
                        break;

                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        days = 30;
                        break;

                    case 12:
                        {
                            if (IsLeapYear) days = 30;
                            else days = 29;
                            break;
                        }

                    default:
                        throw new Exception("Month number is wrong !!!");
                }
                return days;
            }
        }

        /// <summary>
        /// هفته چندم سال
        /// </summary>
        public int GetWeekOfYear
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return 0;
                return PersianCalendar.GetWeekOfYear(_dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Saturday);
            }
        }

        /// <summary>
        /// هفته چندم ماه
        /// </summary>
        public int GetWeekOfMonth
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return 0;
                var MDPersianDateTime = AddDays(1 - Day);
                return GetWeekOfYear - MDPersianDateTime.GetWeekOfYear + 1;
            }
        }

        /// <summary>
        /// روز چندم ماه
        /// </summary>
        public int GetDayOfYear
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return 0;
                return PersianCalendar.GetDayOfYear(_dateTime);
            }
        }

        /// <summary>
        /// آیا سال کبیسه است؟
        /// </summary>
        public bool IsLeapYear
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return false;
                return PersianCalendar.IsLeapYear(Year);
            }
        }

        /// <summary>
        /// قبل از ظهر، بعد از ظهر
        /// </summary>
        private AmPmEnum PersianAmPm
        {
            get
            {
                return _dateTime.ToString("tt") == "PM" ? AmPmEnum.PM : AmPmEnum.AM;
            }
        }

        /// <summary>
        /// قبل از ظهر، بعد از ظهر به شکل مخفف . کوتاه
        /// </summary>
        public string GetPersianAmPm
        {
            get
            {
                var result = string.Empty;
                switch (PersianAmPm)
                {
                    case AmPmEnum.AM:
                        result = "ق.ظ";
                        break;

                    case AmPmEnum.PM:
                        result = "ب.ظ";
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// نام کامل ماه
        /// </summary>
        public string GetLongMonthName => GetPersianMonthNamePrivate(Month);

        /// <summary>
        /// سال دو رقمی
        /// </summary>
        public int GetShortYear
        {
            get { return Year % 100; }
        }

        /// <summary>
        /// نام کامل روز
        /// </summary>
        public string GetLongDayOfWeekName
        {
            get
            {
                string weekDayName = null;
                switch (DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        weekDayName = PersianWeekDaysStruct.یکشنبه.Value;
                        break;

                    case DayOfWeek.Monday:
                        weekDayName = PersianWeekDaysStruct.دوشنبه.Value;
                        break;

                    case DayOfWeek.Tuesday:
                        weekDayName = PersianWeekDaysStruct.سه_شنبه.Value;
                        break;

                    case DayOfWeek.Wednesday:
                        weekDayName = PersianWeekDaysStruct.چهارشنبه.Value;
                        break;

                    case DayOfWeek.Thursday:
                        weekDayName = PersianWeekDaysStruct.پنجشنبه.Value;
                        break;

                    case DayOfWeek.Friday:
                        weekDayName = PersianWeekDaysStruct.جمعه.Value;
                        break;

                    case DayOfWeek.Saturday:
                        weekDayName = PersianWeekDaysStruct.شنبه.Value;
                        break;
                }
                return weekDayName;
            }
        }

        /// <summary>
        /// نام یک حرفی روز، حرف اول روز
        /// </summary>
        public string GetShortDayOfWeekName
        {
            get
            {
                string weekDayName = null;
                switch (DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        weekDayName = "ی";
                        break;

                    case DayOfWeek.Monday:
                        weekDayName = "د";
                        break;

                    case DayOfWeek.Tuesday:
                        weekDayName = "س";
                        break;

                    case DayOfWeek.Wednesday:
                        weekDayName = "چ";
                        break;

                    case DayOfWeek.Thursday:
                        weekDayName = "پ";
                        break;

                    case DayOfWeek.Friday:
                        weekDayName = "ج";
                        break;

                    case DayOfWeek.Saturday:
                        weekDayName = "ش";
                        break;
                }

                return weekDayName;
            }
        }

        /// <summary>
        /// تاریخ و زمان همین الان
        /// </summary>
        public static MDPersianDateTime Now
        {
            get
            {
                return new MDPersianDateTime(DateTime.Now);
            }
        }

        /// <summary>
        /// تاریخ امروز
        /// </summary>
        public static MDPersianDateTime Today
        {
            get
            {
                return new MDPersianDateTime(DateTime.Today);
            }
        }

        /// <summary>
        /// زمان به فرمتی مشابه 
        /// <para />
        /// 13:47:40:530
        /// </summary>
        public string TimeOfDay
        {
            get
            {
                //if (_dateTime <= DateTime.MinValue) return null;
                var result = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", Hour, Minute, Second, MiliSecond);
                if (EnglishNumber) return result;
                return ToPersianNumber(result);
            }
        }

        /// <summary>
        /// زمان به فرمتی مشابه زیر 
        /// <para />
        /// ساعت 01:47:40:530 ب.ظ
        /// </summary>
        public string LongTimeOfDay
        {
            get
            {
                //if (_dateTime <= DateTime.MinValue) return null;
                var result = string.Format("ساعت {0:00}:{1:00}:{2:00}:{3:000} {4}", ShortHour, Minute, Second, MiliSecond, GetPersianAmPm);
                if (EnglishNumber) return result;
                return ToPersianNumber(result);
            }
        }

        /// <summary>
        /// زمان به فرمتی مشابه زیر
        /// <para />
        /// 01:47:40 ب.ظ
        /// </summary>
        public string ShortTimeOfDay
        {
            get
            {
                //if (_dateTime <= DateTime.MinValue) return null;
                var result = string.Format("{0:00}:{1:00}:{2:00} {3}", ShortHour, Minute, Second, GetPersianAmPm);
                if (EnglishNumber) return result;
                return ToPersianNumber(result);
            }
        }

        /// <summary>
        /// تاریخ بدون احتساب زمان
        /// </summary>
        public MDPersianDateTime Date
        {
            get
            {
                var MDPersianDateTime = new MDPersianDateTime(this.Year, this.Month, this.Day)
                {
                    EnglishNumber = this.EnglishNumber
                };
                return MDPersianDateTime;
            }
        }

        /// <summary>
        /// حداقل مقدار
        /// </summary>
        public static MDPersianDateTime MinValue
        {
            get { return new MDPersianDateTime(DateTime.MinValue); }
        }

        /// <summary>
        /// حداکثر مقدار
        /// </summary>
        public static MDPersianDateTime MaxValue
        {
            get
            {
                return new MDPersianDateTime(DateTime.MaxValue);
            }
        }

        #endregion

        #region ctor

        /// <summary>
        /// متد سازنده برای دی سریالایز شدن
        /// </summary>
        private MDPersianDateTime(SerializationInfo info, StreamingContext context)
            : this()
        {
            _dateTime = info.GetDateTime("DateTime");
            EnglishNumber = info.GetBoolean("EnglishNumber");
        }

        /// <summary>
        /// مقدار دهی اولیه با استفاده از دیت تایم میلادی
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <param name="englishNumber">آیا اعداد در خروجی های این آبجکت به صورت انگلیسی نمایش داده شوند یا فارسی؟</param>
        private MDPersianDateTime(DateTime dateTime, bool englishNumber)
            : this()
        {
            _dateTime = dateTime;
            EnglishNumber = englishNumber;
        }

        /// <summary>
        /// مقدار دهی اولیه با استفاده از دیت تایم میلادی
        /// </summary>
        public MDPersianDateTime(DateTime dateTime)
            : this()
        {
            _dateTime = dateTime;
        }

        /// <summary>
        /// مقدار دهی اولیه با استفاده از دیت تایم قابل نال میلادی
        /// </summary>
        public MDPersianDateTime(DateTime? nullableDateTime)
            : this()
        {
            if (!nullableDateTime.HasValue)
            {
                _dateTime = DateTime.MinValue;
                return;
            }
            _dateTime = nullableDateTime.Value;
        }

        /// <summary>
        /// مقدار دهی اولیه
        /// </summary>
        /// <param name="persianYear">سال شمسی</param>
        /// <param name="persianMonth">ماه شمسی</param>
        /// <param name="persianDay">روز شمسی</param>
        public MDPersianDateTime(int persianYear, int persianMonth, int persianDay)
            : this()
        {
            _dateTime = PersianCalendar.ToDateTime(persianYear, persianMonth, persianDay, 0, 0, 0, 0);
        }

        /// <summary>
        /// مقدار دهی اولیه
        /// </summary>
        /// <param name="persianYear">سال شمسی</param>
        /// <param name="persianMonth">ماه شمسی</param>
        /// <param name="persianDay">روز شمسی</param>
        /// <param name="hour">ساعت</param>
        /// <param name="minute">دقیقه</param>
        /// <param name="second">ثانیه</param>
        public MDPersianDateTime(int persianYear, int persianMonth, int persianDay, int hour, int minute, int second)
            : this()
        {
            _dateTime = PersianCalendar.ToDateTime(persianYear, persianMonth, persianDay, hour, minute, second, 0);
        }

        /// <summary>
        /// مقدار دهی اولیه
        /// </summary>
        /// <param name="persianYear">سال شمسی</param>
        /// <param name="persianMonth">ماه شمسی</param>
        /// <param name="persianDay">روز شمسی</param>
        /// <param name="hour">سال</param>
        /// <param name="minute">دقیقه</param>
        /// <param name="second">ثانیه</param>
        /// <param name="miliSecond">میلی ثانیه</param>
        public MDPersianDateTime(int persianYear, int persianMonth, int persianDay, int hour, int minute, int second, int miliSecond)
            : this()
        {
            _dateTime = PersianCalendar.ToDateTime(persianYear, persianMonth, persianDay, hour, minute, second, miliSecond);
        }

        #endregion

        #region Types

        private enum AmPmEnum
        {
            AM = 0,
            PM = 1,
            None = 2,
        }

        private enum MDPersianDateTimeMonthEnum
        {
            فروردین = 1,
            اردیبهشت = 2,
            خرداد = 3,
            تیر = 4,
            مرداد = 5,
            شهریور = 6,
            مهر = 7,
            آبان = 8,
            آذر = 9,
            دی = 10,
            بهمن = 11,
            اسفند = 12,
        }

        private struct PersianWeekDaysStruct
        {
            public static KeyValuePair<int, string> شنبه
            {
                get { return new KeyValuePair<int, string>((int)DayOfWeek.Saturday, "شنبه"); }
            }

            public static KeyValuePair<int, string> یکشنبه
            {
                get { return new KeyValuePair<int, string>((int)DayOfWeek.Sunday, "یکشنبه"); }
            }

            public static KeyValuePair<int, string> دوشنبه
            {
                get { return new KeyValuePair<int, string>((int)DayOfWeek.Monday, "دوشنبه"); }
            }

            public static KeyValuePair<int, string> سه_شنبه
            {
                get { return new KeyValuePair<int, string>((int)DayOfWeek.Tuesday, "سه شنبه"); }
            }

            public static KeyValuePair<int, string> چهارشنبه
            {
                get { return new KeyValuePair<int, string>((int)DayOfWeek.Thursday, "چهارشنبه"); }
            }

            public static KeyValuePair<int, string> پنجشنبه
            {
                get { return new KeyValuePair<int, string>((int)DayOfWeek.Wednesday, "پنج شنبه"); }
            }

            public static KeyValuePair<int, string> جمعه
            {
                get { return new KeyValuePair<int, string>((int)DayOfWeek.Friday, "جمعه"); }
            }
        }

        #endregion

        #region override

        /// <summary>
        /// تبدیل تاریخ به رشته با فرمت مشابه زیر
        /// <para />
        /// 1393/09/14   13:49:40
        /// </summary>
        public override string ToString()
        {
            return ToString("");
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MDPersianDateTime)) return false;
            var MDPersianDateTime = (MDPersianDateTime)obj;
            return _dateTime == MDPersianDateTime.ToDateTime();
        }

        public override int GetHashCode()
        {
            return _dateTime.GetHashCode();
        }

        /// <summary>
        /// مقایسه با تاریخ دیگر
        /// </summary>
        /// <returns>مقدار بازگشتی همانند مقدار بازگشتی متد کامپیر در دیت تایم دات نت است</returns>
        public int CompareTo(MDPersianDateTime otherMDPersianDateTime)
        {
            return _dateTime.CompareTo(otherMDPersianDateTime.ToDateTime());
        }

        /// <summary>
        /// مقایسه با تاریخ دیگر
        /// </summary>
        /// <returns>مقدار بازگشتی همانند مقدار بازگشتی متد کامپیر در دیت تایم دات نت است</returns>
        public int CompareTo(DateTime otherDateTime)
        {
            return _dateTime.CompareTo(otherDateTime);
        }

        #region operators

        /// <summary>
        /// تبدیل خودکار به دیت تایم میلادی
        /// </summary>
        public static implicit operator DateTime(MDPersianDateTime MDPersianDateTime)
        {
            return MDPersianDateTime.ToDateTime();
        }

        /// <summary>
        /// اپراتور برابر
        /// </summary>
        public static bool operator ==(MDPersianDateTime MDPersianDateTime1, MDPersianDateTime MDPersianDateTime2)
        {
            return MDPersianDateTime1.Equals(MDPersianDateTime2);
        }

        /// <summary>
        /// اپراتور نامساوی
        /// </summary>
        public static bool operator !=(MDPersianDateTime MDPersianDateTime1, MDPersianDateTime MDPersianDateTime2)
        {
            return !MDPersianDateTime1.Equals(MDPersianDateTime2);
        }

        /// <summary>
        /// اپراتور بزرگتری
        /// </summary>
        public static bool operator >(MDPersianDateTime MDPersianDateTime1, MDPersianDateTime MDPersianDateTime2)
        {
            return MDPersianDateTime1.ToDateTime() > MDPersianDateTime2.ToDateTime();
        }

        /// <summary>
        /// اپراتور کوچکتری
        /// </summary>
        public static bool operator <(MDPersianDateTime MDPersianDateTime1, MDPersianDateTime MDPersianDateTime2)
        {
            return MDPersianDateTime1.ToDateTime() < MDPersianDateTime2.ToDateTime();
        }

        /// <summary>
        /// اپراتور بزرگتر مساوی
        /// </summary>
        public static bool operator >=(MDPersianDateTime MDPersianDateTime1, MDPersianDateTime MDPersianDateTime2)
        {
            return MDPersianDateTime1.ToDateTime() >= MDPersianDateTime2.ToDateTime();
        }

        /// <summary>
        /// اپراتور کوچکتر مساوی
        /// </summary>
        public static bool operator <=(MDPersianDateTime MDPersianDateTime1, MDPersianDateTime MDPersianDateTime2)
        {
            return MDPersianDateTime1.ToDateTime() <= MDPersianDateTime2.ToDateTime();
        }

        /// <summary>
        /// اپراتور جمع تو زمان
        /// </summary>
        public static MDPersianDateTime operator +(MDPersianDateTime MDPersianDateTime1, TimeSpan timeSpanToAdd)
        {
            DateTime dateTime1 = MDPersianDateTime1;
            return new MDPersianDateTime(dateTime1.Add(timeSpanToAdd));
        }

        /// <summary>
        /// اپراتور کم کردن دو زمان از هم
        /// </summary>
        public static TimeSpan operator -(MDPersianDateTime MDPersianDateTime1, MDPersianDateTime MDPersianDateTime2)
        {
            DateTime dateTime1 = MDPersianDateTime1;
            DateTime dateTime2 = MDPersianDateTime2;
            return dateTime1 - dateTime2;
        }

        #endregion

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("DateTime", ToDateTime());
            info.AddValue("EnglishNumber", EnglishNumber);
        }

        #endregion

        #region IComparable

        public bool Equals(MDPersianDateTime other)
        {
            return Year == other.Year && Month == other.Month && Day == other.Day &&
                Hour == other.Hour && Minute == other.Minute && Second == other.Second && MiliSecond == other.MiliSecond;
        }

        public bool Equals(DateTime other)
        {
            return _dateTime == other;
        }

        #endregion

        #region Methods

        /// <summary>
        /// تاریخ شروع ماه رمضان 
        /// <para />
        /// چون ممکن است در یک سال شمسی دو شروع ماه رمضان داشته باشیم 
        /// <para />
        /// مقدار بازگشتی آرایه است که حداکثر دو آیتم دارد 
        /// </summary>
        public MDPersianDateTime[] GetStartDayOfRamadan(int hijriAdjustment)
        {
            var result = new List<MDPersianDateTime>();
            var hijriCalendar = new HijriCalendar { HijriAdjustment = hijriAdjustment };

            var currentHijriYear = hijriCalendar.GetYear(_dateTime);

            var startDayOfRamadan1 = new MDPersianDateTime(hijriCalendar.ToDateTime(currentHijriYear, 9, 1, 0, 0, 0, 0));
            result.Add(startDayOfRamadan1);

            var startDayOfRamadan2 = new MDPersianDateTime(hijriCalendar.ToDateTime(++currentHijriYear, 9, 1, 0, 0, 0, 0));
            if (startDayOfRamadan1.Year == startDayOfRamadan2.Year)
                result.Add(startDayOfRamadan2);

            return result.ToArray();
        }

        /// <summary>
        /// پارس کردن رشته و تبدیل به نوع MDPersianDateTime
        /// </summary>
        /// <param name="MDPersianDateTimeInString">متنی که باید پارس شود</param>
        /// <param name="dateSeperatorPattern">کاراکتری که جدا کننده تاریخ ها است</param>
        public static MDPersianDateTime Parse(string MDPersianDateTimeInString, string dateSeperatorPattern = @"\/|-")
        {
            //Convert persian and arabic digit to english to avoid throwing exception in Parse method
            MDPersianDateTimeInString = ExtensionsHelper.ConvertDigitsToLatin(MDPersianDateTimeInString);

            string month = "", year, day,
                hour = "0",
                minute = "0",
                second = "0",
                miliSecond = "0";
            var amPmEnum = AmPmEnum.None;
            var containMonthSeperator = Regex.IsMatch(MDPersianDateTimeInString, dateSeperatorPattern);

            MDPersianDateTimeInString = ToEnglishNumber(MDPersianDateTimeInString.Replace("&nbsp;", " ").Replace(" ", "-").Replace("\\", "-"));
            MDPersianDateTimeInString = Regex.Replace(MDPersianDateTimeInString, dateSeperatorPattern, "-");
            MDPersianDateTimeInString = MDPersianDateTimeInString.Replace("ك", "ک").Replace("ي", "ی");

            MDPersianDateTimeInString = string.Format("-{0}-", MDPersianDateTimeInString);

            // بدست آوردن ب.ظ یا ق.ظ
            if (MDPersianDateTimeInString.IndexOf("ق.ظ", StringComparison.InvariantCultureIgnoreCase) > -1)
                amPmEnum = AmPmEnum.AM;
            else if (MDPersianDateTimeInString.IndexOf("ب.ظ", StringComparison.InvariantCultureIgnoreCase) > -1)
                amPmEnum = AmPmEnum.PM;

            if (MDPersianDateTimeInString.IndexOf(":", StringComparison.InvariantCultureIgnoreCase) > -1) // رشته ورودی شامل ساعت نیز هست
            {
                MDPersianDateTimeInString = Regex.Replace(MDPersianDateTimeInString, @"-*:-*", ":");
                hour = Regex.Match(MDPersianDateTimeInString, @"(?<=-)\d{1,2}(?=:)", RegexOptions.IgnoreCase).Value;
                minute = Regex.Match(MDPersianDateTimeInString, @"(?<=-\d{1,2}:)\d{1,2}(?=:?)", RegexOptions.IgnoreCase).Value;
                if (MDPersianDateTimeInString.IndexOf(':') != MDPersianDateTimeInString.LastIndexOf(':'))
                {
                    second = Regex.Match(MDPersianDateTimeInString, @"(?<=-\d{1,2}:\d{1,2}:)\d{1,2}(?=(\d{1,2})?)", RegexOptions.IgnoreCase).Value;
                    miliSecond = Regex.Match(MDPersianDateTimeInString, @"(?<=-\d{1,2}:\d{1,2}:\d{1,2}:)\d{1,4}(?=(\d{1,2})?)", RegexOptions.IgnoreCase).Value;
                    if (string.IsNullOrEmpty(miliSecond)) miliSecond = "0";
                }
            }

            if (containMonthSeperator)
            {
                // بدست آوردن ماه
                month = Regex.Match(MDPersianDateTimeInString, @"(?<=\d{2,4}-)\d{1,2}(?=-\d{1,2}-\d{1,2}(?!-\d{1,2}:))", RegexOptions.IgnoreCase).Value;
                if (string.IsNullOrEmpty(month))
                    month = Regex.Match(MDPersianDateTimeInString, @"(?<=\d{2,4}-)\d{1,2}(?=-\d{1,2}[^:])", RegexOptions.IgnoreCase).Value;

                // بدست آوردن روز
                day = Regex.Match(MDPersianDateTimeInString, @"(?<=\d{2,4}-\d{1,2}-)\d{1,2}(?=-)", RegexOptions.IgnoreCase).Value;

                // بدست آوردن سال
                year = Regex.Match(MDPersianDateTimeInString, @"(?<=-)\d{2,4}(?=-\d{1,2}-\d{1,2})", RegexOptions.IgnoreCase).Value;
            }
            else
            {
                foreach (MDPersianDateTimeMonthEnum item in Enum.GetValues(typeof(MDPersianDateTimeMonthEnum)))
                {
                    var itemValueInString = item.ToString();
                    if (!MDPersianDateTimeInString.Contains(itemValueInString)) continue;
                    month = ((int)item).ToString();
                    break;
                }

                if (string.IsNullOrEmpty(month))
                    throw new Exception("عدد یا حرف ماه در رشته ورودی وجود ندارد");

                // بدست آوردن روز
                var dayMatch = Regex.Match(MDPersianDateTimeInString, @"(?<=-)\d{1,2}(?=-)", RegexOptions.IgnoreCase);
                if (dayMatch.Success)
                {
                    day = dayMatch.Value;
                    MDPersianDateTimeInString = Regex.Replace(MDPersianDateTimeInString, string.Format("(?<=-){0}(?=-)", day), "");
                }
                else
                    throw new Exception("عدد روز در رشته ورودی وجود ندارد");

                // بدست آوردن سال
                var yearMatch = Regex.Match(MDPersianDateTimeInString, @"(?<=-)\d{4}(?=-)", RegexOptions.IgnoreCase);
                if (yearMatch.Success)
                    year = yearMatch.Value;
                else
                {
                    yearMatch = Regex.Match(MDPersianDateTimeInString, @"(?<=-)\d{2,4}(?=-)", RegexOptions.IgnoreCase);
                    if (yearMatch.Success)
                        year = yearMatch.Value;
                    else
                        throw new Exception("عدد سال در رشته ورودی وجود ندارد");
                }
            }

            //if (year.Length <= 2 && year[0] == '9') year = string.Format("13{0}", year);
            //else if (year.Length <= 2) year = string.Format("14{0}", year);

            var numericYear = int.Parse(year);
            var numericMonth = int.Parse(month);
            var numericDay = int.Parse(day);
            var numericHour = int.Parse(hour);
            var numericMinute = int.Parse(minute);
            var numericSecond = int.Parse(second);
            var numericMiliSecond = int.Parse(miliSecond);

            if (numericYear < 100)
                numericYear += 1300;

            switch (amPmEnum)
            {
                case AmPmEnum.PM:
                    if (numericHour < 12)
                        numericHour = numericHour + 12;
                    break;
                case AmPmEnum.AM:
                case AmPmEnum.None:
                    break;
            }

            return new MDPersianDateTime(numericYear, numericMonth, numericDay, numericHour, numericMinute, numericSecond, numericMiliSecond);
        }

        /// <summary>
        /// پارس کردن یک رشته برای یافتن تاریخ شمسی
        /// </summary>
        public static bool TryParse(string MDPersianDateTimeInString, out MDPersianDateTime result, string dateSeperatorPattern = @"\/|-")
        {
            if (string.IsNullOrEmpty(MDPersianDateTimeInString))
            {
                result = MinValue;
                return false;
            }
            try
            {
                result = Parse(MDPersianDateTimeInString, dateSeperatorPattern);
                return true;
            }
            catch
            {
                result = MinValue;
                return false;
            }
        }

        /// <summary>
        /// پارس کردن عددی در فرمت تاریخ شمسی
        /// <para />
        /// همانند 13920305
        /// </summary>
        public static MDPersianDateTime Parse(int numericPersianDate)
        {
            if (numericPersianDate.ToString().Length != 8)
                throw new InvalidCastException("Numeric persian date must have a format like 13920101.");
            var year = numericPersianDate / 10000;
            var day = numericPersianDate % 100;
            var month = numericPersianDate / 100 % 100;
            return new MDPersianDateTime(year, month, day);
        }
        /// <summary>
        /// پارس کردن عددی در فرمت تاریخ شمسی
        /// <para />
        /// همانند 13920305
        /// </summary>
        public static bool TryParse(int numericPersianDate, out MDPersianDateTime result)
        {
            try
            {
                result = Parse(numericPersianDate);
                return true;
            }
            catch
            {
                result = MinValue;
                return false;
            }
        }

        /// <summary>
        /// پارس کردن عددی در فرمت تاریخ و زمان شمسی
        /// <para />
        /// همانند 13961223072132004
        /// </summary>
        public static MDPersianDateTime Parse(long numericMDPersianDateTime)
        {
            if (numericMDPersianDateTime.ToString().Length != 17)
                throw new InvalidCastException("Numeric persian date time must have a format like 1396122310223246.");
            var year = numericMDPersianDateTime / 10000000000000;
            var month = numericMDPersianDateTime / 100000000000 % 100;
            var day = numericMDPersianDateTime / 1000000000 % 100;
            var hour = numericMDPersianDateTime / 10000000 % 100;
            var minute = numericMDPersianDateTime / 100000 % 100;
            var second = numericMDPersianDateTime / 1000 % 100;
            var millisecond = numericMDPersianDateTime % 1000;
            return new MDPersianDateTime((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second, (int)millisecond);
        }
        /// <summary>
        /// پارس کردن عددی در فرمت تاریخ و زمان شمسی
        /// <para />
        /// همانند 13961223102232461
        /// </summary>
        public static bool TryParse(long numericMDPersianDateTime, out MDPersianDateTime result)
        {
            try
            {
                result = Parse(numericMDPersianDateTime);
                return true;
            }
            catch
            {
                result = MinValue;
                return false;
            }
        }

        private static readonly List<string> GregorianWeekDayNames = new List<string> { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
        private static readonly List<string> GregorianMonthNames = new List<string> { "january", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december" };
        private static readonly List<string> PmAm = new List<string> { "pm", "am" };

        /// <summary>
        /// فرمت های که پشتیبانی می شوند
        /// <para />
        /// yyyy: سال چهار رقمی
        /// <para />
        /// yy: سال دو رقمی
        /// <para />
        /// MMMM: نام فارسی ماه
        /// <para />
        /// MM: عدد دو رقمی ماه
        /// <para />
        /// M: عدد یک رقمی ماه
        /// <para />
        /// dddd: نام فارسی روز هفته
        /// <para />
        /// dd: عدد دو رقمی روز ماه
        /// <para />
        /// d: عدد یک رقمی روز ماه
        /// <para />
        /// HH: ساعت دو رقمی با فرمت 00 تا 24
        /// <para />
        /// H: ساعت یک رقمی با فرمت 0 تا 24
        /// <para />
        /// hh: ساعت دو رقمی با فرمت 00 تا 12
        /// <para />
        /// h: ساعت یک رقمی با فرمت 0 تا 12
        /// <para />
        /// mm: عدد دو رقمی دقیقه
        /// <para />
        /// m: عدد یک رقمی دقیقه
        /// <para />
        /// ss: ثانیه دو رقمی
        /// <para />
        /// s: ثانیه یک رقمی
        /// <para />
        /// fff: میلی ثانیه 3 رقمی
        /// <para />
        /// ff: میلی ثانیه 2 رقمی
        /// <para />
        /// f: میلی ثانیه یک رقمی
        /// <para />
        /// tt: ب.ظ یا ق.ظ
        /// <para />
        /// t: حرف اول از ب.ظ یا ق.ظ
        /// </summary>
        public string ToString(string format, IFormatProvider fp = null)
        {
            if (string.IsNullOrEmpty(format)) format = "yyyy/MM/dd   HH:mm:ss";
            var dateTimeString = format.Trim();

            dateTimeString = dateTimeString.Replace("yyyy", Year.ToString(CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("yy", GetShortYear.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("MMMM", MonthName);
            dateTimeString = dateTimeString.Replace("MM", Month.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("M", Month.ToString(CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("dddd", GetLongDayOfWeekName);
            dateTimeString = dateTimeString.Replace("dd", Day.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("d", Day.ToString(CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("HH", Hour.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("H", Hour.ToString(CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("hh", ShortHour.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("h", ShortHour.ToString(CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("mm", Minute.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("m", Minute.ToString(CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("ss", Second.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("s", Second.ToString(CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("fff", MiliSecond.ToString("000", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("ff", (MiliSecond / 10).ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("f", (MiliSecond / 100).ToString(CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("tt", GetPersianAmPm);
            dateTimeString = dateTimeString.Replace("t", GetPersianAmPm[0].ToString(CultureInfo.InvariantCulture));

            if (!EnglishNumber)
                dateTimeString = ToPersianNumber(dateTimeString);

            return dateTimeString;
        }

        /// <summary>
        /// بررسی میکند آیا تاریخ ورودی تاریخ میلادی است یا نه
        /// </summary>
        public static bool IsChristianDate(string inputString)
        {
            inputString = inputString.ToLower();
            bool result;

            foreach (var gregorianWeekDayName in GregorianWeekDayNames)
            {
                result = inputString.Contains(gregorianWeekDayName);
                if (result) return true;
            }

            foreach (var gregorianMonthName in GregorianMonthNames)
            {
                result = inputString.Contains(gregorianMonthName);
                if (result) return true;
            }

            foreach (var item in PmAm)
            {
                result = inputString.Contains(item);
                if (result) return true;
            }

            result = Regex.IsMatch(inputString, @"(1[8-9]|[2-9][0-9])\d{2}", RegexOptions.IgnoreCase);

            return result;
        }

        /// <summary>
        /// بررسی میکند آیا تاریخ ورودی مطابق  تاریخ اس کسو ال سرور می باشد یا نه
        /// </summary>
        public static bool IsSqlDateTime(DateTime dateTime)
        {
            var minSqlDateTimeValue = new DateTime(1753, 1, 1);
            return dateTime >= minSqlDateTimeValue;
        }

        /// <summary>
        /// تبدیل نام ماه شمسی به عدد معادل آن
        /// <para />
        /// به طور مثال آذر را به 9 تبدیل می کند
        /// </summary>
        public int GetMonthEnum(string longMonthName)
        {
            var monthEnum = (MDPersianDateTimeMonthEnum)Enum.Parse(typeof(MDPersianDateTimeMonthEnum), longMonthName);
            return (int)monthEnum;
        }

        /// <summary>
        /// نمایش تاریخ به فرمتی مشابه زیر
        /// <para />
        /// 1393/09/14
        /// </summary>
        public string ToShortDateString()
        {
            //if (_dateTime <= DateTime.MinValue) return null;
            var result = string.Format("{0:0000}/{1:00}/{2:00}", Year, Month, Day);
            if (EnglishNumber) return result;
            return ToPersianNumber(result);
        }

        /// <summary>
        /// نمایش تاریخ به فرمتی مشابه زیر
        /// <para />
        /// ج 14 آذر 93
        /// </summary>
        public string ToShortDate1String()
        {
            //if (_dateTime <= DateTime.MinValue) return null;
            var result = string.Format("{0} {1:00} {2} {3}", GetShortDayOfWeekName, Day, GetLongMonthName, GetShortYear);
            if (EnglishNumber) return result;
            return ToPersianNumber(result);
        }

        /// <summary>
        /// نمایش تاریخ به صورت عدد و در فرمتی مشابه زیر
        /// <para />
        /// 13930914
        /// </summary>
        public int ToShortDateInt()
        {
            var result = string.Format("{0:0000}{1:00}{2:00}", Year, Month, Day);
            return int.Parse(result);
        }

        /// <summary>
        /// نمایش تاریخ و ساعت تا دقت میلی ثانیه به صورت عدد
        /// <para />
        /// 1396122310324655
        /// </summary>
        public long ToLongDateTimeInt()
        {
            var result = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}{6:000}", Year, Month, Day, Hour, Minute, Second, MiliSecond);
            return long.Parse(result);
        }

        /// <summary>
        /// در این فرمت نمایش ساعت و دقیقه و ثانیه در کنار هم با حذف علامت : تبدیل به عدد می شوند و نمایش داده می شود
        /// <para />
        /// مثال: 123452 
        /// <para />
        /// که به معنای ساعت 12 و 34 دقیقه و 52 ثانیه می باشد
        /// </summary>
        public int ToTimeInt()
        {
            var result = string.Format("{0:00}{1:00}{2:00}", Hour, Minute, Second);
            return int.Parse(result);
        }

        /// <summary>
        /// در این فرمت نمایش ساعت و دقیقه در کنار هم با حذف علامت : تبدیل به عدد می شوند و نمایش داده می شود
        /// <para />
        /// مثال: 1234 
        /// <para />
        /// که به معنای ساعت 12 و 34 دقیقه می باشد
        /// </summary>
        public int ToTimeInt1()
        {
            var result = string.Format("{0:00}{1:00}", Hour, Minute);
            return int.Parse(result);
        }

        /// <summary>
        /// نمایش تاریخ به فرمتی مشابه زیر
        /// <para />
        /// جمعه، 14 آذر 1393
        /// </summary>
        public string ToLongDateString()
        {
            //if (_dateTime <= DateTime.MinValue) return null;
            var result = string.Format("{0}، {1:00} {2} {3:0000}", GetLongDayOfWeekName, Day, GetLongMonthName, Year);
            if (EnglishNumber) return result;
            return ToPersianNumber(result);
        }

        /// <summary>
        /// نمایش تاریخ و زمان به فرمتی مشابه زیر
        /// <para />
        /// جمعه، 14 آذر 1393 ساعت 13:50:27
        /// </summary>
        public string ToLongDateTimeString()
        {
            //if (_dateTime <= DateTime.MinValue) return null;
            var result = string.Format("{0}، {1:00} {2} {3:0000} ساعت {4:00}:{5:00}:{6:00}", GetLongDayOfWeekName, Day, GetLongMonthName, Year, Hour, Minute, Second);
            if (EnglishNumber) return result;
            return ToPersianNumber(result);
        }

        /// <summary>
        /// نمایش تاریخ و زمان به فرمتی مشابه زیر
        /// <para />
        /// جمعه، 14 آذر 1393 13:50
        /// </summary>
        public string ToShortDateTimeString()
        {
            //if (_dateTime <= DateTime.MinValue) return null;
            var result = string.Format("{0}، {1:00} {2} {3:0000} {4:00}:{5:00}", GetLongDayOfWeekName, Day, GetLongMonthName, Year, Hour, Minute);
            if (EnglishNumber) return result;
            return ToPersianNumber(result);
        }

        /// <summary>
        /// نمایش زمان به فرمتی مشابه زیر
        /// <para />
        /// 01:50 ب.ظ
        /// </summary>
        public string ToShortTimeString()
        {
            //if (_dateTime <= DateTime.MinValue) return null;
            var result = string.Format("{0:00}:{1:00} {2}", ShortHour, Minute, GetPersianAmPm);
            if (EnglishNumber) return result;
            return ToPersianNumber(result);
        }

        /// <summary>
        /// نمایش زمان به فرمتی مشابه زیر
        /// <para />
        /// 13:50:20
        /// </summary>
        public string ToLongTimeString()
        {
            //if (_dateTime <= DateTime.MinValue) return null;
            var result = string.Format("{0:00}:{1:00}:{2:00}", Hour, Minute, Second);
            if (EnglishNumber) return result;
            return ToPersianNumber(result);
        }

        /// <summary>
        /// نمایش زمان به فرمتی مشابه زیر
        /// <para />
        /// 1 دقیقه قبل
        /// </summary>
        public string ElapsedTime()
        {
            //if (_dateTime <= DateTime.MinValue) return null;

            var MDPersianDateTimeNow = new MDPersianDateTime(DateTime.Now);
            var timeSpan = MDPersianDateTimeNow - _dateTime;
            if (timeSpan.TotalDays > 90)
                return ToShortDateTimeString();

            var result = string.Empty;
            if (timeSpan.TotalDays > 30)
            {
                var month = timeSpan.TotalDays / 30;
                if (month > 0)
                    result = string.Format("{0:0} ماه قبل", month);
            }
            else if (timeSpan.TotalDays >= 1)
            {
                result = string.Format("{0:0} روز قبل", timeSpan.TotalDays);
            }
            else if (timeSpan.TotalHours >= 1)
            {
                result = string.Format("{0:0} ساعت قبل", timeSpan.TotalHours);
            }
            else
            {
                var minute = timeSpan.TotalMinutes;
                if (minute <= 1) minute = 1;
                result = string.Format("{0:0} دقیقه قبل", minute);
            }
            if (EnglishNumber) return result;
            return ToPersianNumber(result);
        }

        /// <summary>
        /// گرفتن فقط زمان 
        /// </summary>
	    public TimeSpan GetTime()
        {
            return new TimeSpan(0, _dateTime.Hour, _dateTime.Minute, _dateTime.Second, _dateTime.Millisecond);
        }

        /// <summary>
        /// تنظیم کردن زمان
        /// </summary>
	    public MDPersianDateTime SetTime(int hour, int minute, int second = 0, int miliSecond = 0)
        {
            return new MDPersianDateTime(Year, Month, Day, hour, minute, second, miliSecond);
        }

        /// <summary>
        /// تبدیل به تاریخ میلادی
        /// </summary>
        public DateTime ToDateTime()
        {
            return _dateTime;
        }

     
        /// <summary>
        /// کم کردن دو تاریخ از هم
        /// </summary>
        public TimeSpan Subtract(MDPersianDateTime MDPersianDateTime)
        {
            return _dateTime - MDPersianDateTime.ToDateTime();
        }

        /// <summary>
        /// تعداد ماه اختلافی با تاریخ دیگری را بر میگرداند
        /// </summary>
        /// <returns>تعداد ماه</returns>
        public int MonthDifference(DateTime dateTime)
        {
            return Math.Abs(dateTime.Month - _dateTime.Month + 12 * (dateTime.Year - _dateTime.Year));
        }

        /// <summary>
        /// اضافه کردن مدت زمانی به تاریخ
        /// </summary>
        public MDPersianDateTime Add(TimeSpan timeSpan)
        {
            return new MDPersianDateTime(_dateTime.Add(timeSpan), EnglishNumber);
        }

        /// <summary>
        /// اضافه کردن سال به تاریخ
        /// </summary>
        public MDPersianDateTime AddYears(int years)
        {
            return new MDPersianDateTime(PersianCalendar.AddYears(_dateTime, years), EnglishNumber);
        }

        /// <summary>
        /// اضافه کردن روز به تاریخ
        /// </summary>
        public MDPersianDateTime AddDays(int days)
        {
            return new MDPersianDateTime(PersianCalendar.AddDays(_dateTime, days), EnglishNumber);
        }

        /// <summary>
        /// اضافه کردن ماه به تاریخ
        /// </summary>
        public MDPersianDateTime AddMonths(int months)
        {
            return new MDPersianDateTime(PersianCalendar.AddMonths(_dateTime, months), EnglishNumber);
        }

        /// <summary>
        /// اضافه کردن ساعت به تاریخ
        /// </summary>
        public MDPersianDateTime AddHours(int hours)
        {
            return new MDPersianDateTime(_dateTime.AddHours(hours), EnglishNumber);
        }

        /// <summary>
        /// اضافه کردن دقیقه به تاریخ
        /// </summary>
        public MDPersianDateTime AddMinutes(int minuts)
        {
            return new MDPersianDateTime(_dateTime.AddMinutes(minuts), EnglishNumber);
        }

        /// <summary>
        /// اضافه کردن ثانیه به تاریخ
        /// </summary>
        public MDPersianDateTime AddSeconds(int seconds)
        {
            return new MDPersianDateTime(_dateTime.AddSeconds(seconds), EnglishNumber);
        }

        /// <summary>
        /// اضافه کردن میلی ثانیه به تاریخ
        /// </summary>
        public MDPersianDateTime AddMilliseconds(int miliseconds)
        {
            return new MDPersianDateTime(_dateTime.AddMilliseconds(miliseconds), EnglishNumber);
        }

        /// <summary>
        /// نام فارسی ماه بر اساس شماره ماه
        /// </summary>
        /// <returns>نام فارسی ماه</returns>
        public static string GetPersianMonthName(int monthNumber)
        {
            return GetPersianMonthNamePrivate(monthNumber);
        }

        private static string ToPersianNumber(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            input = input.Replace("ي", "ی").Replace("ك", "ک");

            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            return
                input
                    .Replace("0", "۰")
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

        private static string ToEnglishNumber(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            input = input.Replace("ي", "ی").Replace("ك", "ک");

            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            return input
                .Replace(",", "")
                .Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9");
        }

        private static string GetPersianMonthNamePrivate(int monthNumber)
        {
            var monthName = "";
            switch (monthNumber)
            {
                case 1:
                    monthName = "فروردین";
                    break;

                case 2:
                    monthName = "اردیبهشت";
                    break;

                case 3:
                    monthName = "خرداد";
                    break;

                case 4:
                    monthName = "تیر";
                    break;

                case 5:
                    monthName = "مرداد";
                    break;

                case 6:
                    monthName = "شهریور";
                    break;

                case 7:
                    monthName = "مهر";
                    break;

                case 8:
                    monthName = "آبان";
                    break;

                case 9:
                    monthName = "آذر";
                    break;

                case 10:
                    monthName = "دی";
                    break;

                case 11:
                    monthName = "بهمن";
                    break;

                case 12:
                    monthName = "اسفند";
                    break;
            }
            return monthName;
        }

        #endregion
    }
}