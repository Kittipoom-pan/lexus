using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class PrivilegeModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public string period { get; set; }
        public string period_m { get; set; }
        public string period_start { get; set; }
        public string period_end { get; set; }
        public string image { get; set; }
        public string image_1 { get; set; }
        public RedeemModel redeem { get; set; }
        public int privilege_type { get; set; }
        public List<string> images { get; set; }
        public bool is_out_of_stock { get; set; }
        public bool is_expire { get; set; }
        public bool already_redeem { get; set; }
        public int display_type { get; set; }
        public int all_stock { get; set; }
        public bool is_show_stock { get; set; }
        public string thk_message { get; set; }

        public int period_type { get; set; }
        public int period_start_in_week { get; set; }
        public int period_start_in_month { get; set; }
        public int customer_usage_per_period { get; set; }
        public int customer_remain_in_period { get; set; }
        public int customer_usage_in_period { get; set; }
        public int customer_special_quota { get; set; }
        //public int overall_quota { get; set; }
        //public int overall_usage { get; set; }
        public int period_quota { get; set; }
        //public int period_usage { get; set; }
        public bool is_show_your_remaining { get; set; }
        public PrivilegePeriodModel privilege_period { get; set; }
        public string ribbon_message { get; set; }
        public string redeem_status { get; set; }
        public int message_code { get; set; }
        public int privilege_cnt { get; set; }
    }

    public class PrivilegePeriodModel
    {
        public DateTime period_start { get; set; }
        public DateTime period_end { get; set; }

        public PrivilegePeriodModel(int period_type, int period_start_in_week, int period_start_in_month, DateTime privilege_start, DateTime privilege_end)
        {
            DateTime DateNow = DateTime.Now;
            PeriodType pType = (PeriodType)period_type;
            if (pType == PeriodType.DAILY || pType == PeriodType.DAILY_LIMIT_BY_MONTH || pType == PeriodType.ONCE_PER_CAMPAIGN_LIMIT_BY_DAILY)
            {
                period_start = DateNow.Date;
                period_end = DateNow.Date.AddDays(1).AddSeconds(-1);

                period_start = privilege_start > period_start ? privilege_start : period_start;
                period_end = privilege_end < period_end ? privilege_end : period_end;
            }
            else if (pType == PeriodType.WEEKLY)
            {
                int currentDayOfWeek = (int)DateNow.DayOfWeek;
                int dateDiff = currentDayOfWeek - period_start_in_week;
                DateTime NewPeriod;
                if (dateDiff < 0)
                    NewPeriod = DateNow.Date.AddDays(-dateDiff);
                else
                    NewPeriod = DateNow.Date.AddDays(-dateDiff).AddDays(7);

                period_start = NewPeriod.AddDays(-7);
                period_end = NewPeriod.AddSeconds(-1);

                period_start = privilege_start > period_start ? privilege_start : period_start;
                period_end = privilege_end < period_end ? privilege_end : period_end;
            }
            else if (pType == PeriodType.MONTHLY || pType == PeriodType.MONTHLY_ACCUM ||
                     pType == PeriodType.ONCE_PER_CAMPAIGN_LIMIT_BY_MONTH || pType == PeriodType.ONCE_PER_CAMPAIGN_ACCUM ||
                     pType == PeriodType.DAILY_LIMIT_BY_MONTH)// || (pType == PeriodType.MONTHLY_LIMIT_DAILY_AND_MONTH && PeriodNo == 2))
            {
                DateTime NewPeriod;

                if (DateNow.Day < period_start_in_month)
                {
                    DateTime previousPeriod = DateNow.Date.AddMonths(-1);
                    NewPeriod = new DateTime(previousPeriod.Year, previousPeriod.Month, period_start_in_month);
                    period_start = NewPeriod;
                    period_end = NewPeriod.AddMonths(1).AddSeconds(-1);
                }
                else
                {
                    DateTime nextPeriod = DateNow.Date.AddMonths(1);
                    NewPeriod = new DateTime(nextPeriod.Year, nextPeriod.Month, period_start_in_month);
                    period_start = NewPeriod.AddMonths(-1);
                    period_end = NewPeriod.AddSeconds(-1);
                }

                period_start = privilege_start > period_start ? privilege_start : period_start;
                period_end = privilege_end < period_end ? privilege_end : period_end;
            }
            else
            {
                period_start = privilege_start;
                period_end = privilege_end;
            }
        }

        public enum PeriodType
        {
            UNLIMIT = 0,
            DAILY = 1,
            WEEKLY = 2,
            MONTHLY = 3,
            YEARLY = 4,
            ONCE_PER_CAMPAIGN = 5,
            ONCE_PER_CAMPAIGN_LIMIT_BY_MONTH = 6,
            MONTHLY_ACCUM = 7,
            ONCE_PER_CAMPAIGN_ACCUM = 8,
            DAILY_LIMIT_BY_MONTH = 9,
            //MONTHLY_LIMIT_DAILY_AND_MONTH = 10,
            ONCE_PER_CAMPAIGN_LIMIT_BY_DAILY = 11
        }
    }

}