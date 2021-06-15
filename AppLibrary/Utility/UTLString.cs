using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLibrary.Utility
{
    public class UTLString
    {
        public static StringBuilder GetWhereInString(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (list != null && list.Count > 0)
                {
                    list.Distinct().ToList().ForEach(str => sb.Append(",'" + str + "'"));
                    sb = new StringBuilder(sb.ToString().Substring(1));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sb;
        }
    }
}
