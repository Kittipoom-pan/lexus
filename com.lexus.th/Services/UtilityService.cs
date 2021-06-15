using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace com.lexus.th
{
    public class UtilityService
    {
        public static string GetDateTimeFormat(DateTime dateTime)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string GetDateTimeFormat2(DateTime dateTime)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            return dateTime.ToString("d MMMM yyyy hh:mm tt");
        }
        public static string GetDateFormat(DateTime dateTime)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            return dateTime.ToString("d MMMM yyyy");
        }

        public static string GetDateTimeFormatStyle2(DateTime dateTime)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string GetTimeFormat(DateTime dateTime)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            return dateTime.ToString("HH:mm");
        }
        public static string GetAPIVersion()
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["APIVersion"];
        }

        public static bool EmailIsValid(string email)
        {
            string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (Regex.IsMatch(email, expression))
            {
                if (Regex.Replace(email, expression, string.Empty).Length == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}