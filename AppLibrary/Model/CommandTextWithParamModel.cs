using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLibrary.Database
{
    public class CommandTextWithParamModel
    {
        public string CommandText { get; set; }
        public List<SqlParameter> Parameters { get; set; }
    }
}
