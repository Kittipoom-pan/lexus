using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ExportExcelService
    {
        public string ExportExcel()
        {
            string filePath = "";

            try
            {
                filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\ServiceAppointment.xlsx";
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                



            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "Export Excel");
                throw ex;
            }




            return filePath;
        }
    }
}