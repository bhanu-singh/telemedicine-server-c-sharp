using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class InternalDateTime
    {
        private DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public Int64 DateTimeInLong { get; set; }
        public InternalDateTime(Int64 timeInt)
        {
            DateTimeInLong = timeInt;
        }
        public InternalDateTime()
        {

        }
        private DateTime UtcDateTime
        {
            get
            {
                if (this.DateTimeInLong > 0)
                    return baseDate.AddSeconds(DateTimeInLong);
                else
                    return baseDate;
            }
            set { DateTimeInLong = (TimeZoneInfo.ConvertTimeToUtc(value).Second - baseDate.Second); }
        }
        public string GetDateTime(string format)
        {
            if (this.DateTimeInLong > 0)
            {
                return this.UtcDateTime.ToString(string.IsNullOrEmpty(format) ? "dd-MM-YYYY" : format);
            }
            else
            {
                return baseDate.ToString(string.IsNullOrEmpty(format) ? "dd-MM-YYYY" : format);
            }
        }
        public string GetDateTime(string format, string timezone)
        {
            if (this.DateTimeInLong > 0)
            {
                return //this.UtcDateTime.ToString(string.IsNullOrEmpty(format) ? "dd-MM-YYYY" : format);
                TimeZoneInfo.ConvertTimeFromUtc(this.UtcDateTime,
TimeZoneInfo.FindSystemTimeZoneById(timezone)).ToString(string.IsNullOrEmpty(format) ? "dd-MM-YYYY" : format);
            }
            else
            {
                return baseDate.ToString(string.IsNullOrEmpty(format) ? "dd-MM-YYYY" : format);
            }
        }
        public DateTime GetDateTime()
        {
            if (this.DateTimeInLong > 0)
                return baseDate.AddSeconds(DateTimeInLong);
            else
                return baseDate;
        }
    }
}
