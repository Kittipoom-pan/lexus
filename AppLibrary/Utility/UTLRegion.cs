using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLibrary.Utility
{
    public class UTLCulture
    {
        public static void SetCurrentThreadCulture(string cultureName)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureName);
        }
    }
}
