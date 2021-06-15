using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class LogManager
    {
        private static LogModel _log = null;
        public static LogModel ServiceLog
        {
            get
            {
                if (_log == null)
                {
                    _log = new LogModel();
                }

                return _log;
            }
        }
    }
}