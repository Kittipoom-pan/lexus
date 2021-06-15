using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace com.lexus.th.web
{
    public class WebUtility
    {
        public static void AlertMessage(Control control, string message)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "script", GetAlertMessage(message), true);
        }
        private static string GetAlertMessage(string message)
        {
            message = message.Replace("'", "");
            message = message.Replace("\"", "");
            message = message.Replace("\\", "");
            return "alert('" + message + "');";
        }
        public static string GetSQLTextValue(string value)
        {
            return value.Replace("'", "''");
        }
        public static string GetDateTimeFormat(DateTime dateTime)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string GetDateFormat(DateTime dateTime)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            return dateTime.ToString("d MMMM yyyy");
        }
        public static string GetTimeFormat(DateTime dateTime)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            return dateTime.ToString("HH:mm");
        }
       
    }
}