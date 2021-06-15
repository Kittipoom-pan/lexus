using AppLibrary.WebHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace com.lexus.th.web
{
    public class TreasureDataController
    {
        public TreasureDataController()
        {

        }

        public void AddEvent(object request, string treasureDataType)
        {
            string key = System.Web.Configuration.WebConfigurationManager.AppSettings["TreasureDataKey"];
            string addEventPath = System.Web.Configuration.WebConfigurationManager.AppSettings["TreasureDataAddEventURL"];
            string dbName = System.Web.Configuration.WebConfigurationManager.AppSettings["TreasureDataDBName"];
            string tableName = "";
            if (treasureDataType == "ServiceAppointment")
            {
                tableName = System.Web.Configuration.WebConfigurationManager.AppSettings["TreasureDataTableName_appointment"];
            }
            else
            {
                tableName = System.Web.Configuration.WebConfigurationManager.AppSettings["TreasureDataTableName_testdrive"];
            }
            string url_path = string.Format(addEventPath, dbName, tableName);

            Uri url = new Uri(url_path);

            WebHelper webHelper = new WebHelper();

            string param = JsonConvert.SerializeObject(request);

            IDictionary<string, string> header = new Dictionary<string, string>();
            header.Add("X-TD-Write-Key", key);

            webHelper.Post(url, param, header, null, error => 
            {
                //LogManager.ServiceLog.WriteCustomLog("AddEvent TreasureData", error);
            });

        }

        public void AddEventTestDrive(int id)
        {
            ServiceTestDriveService service = new ServiceTestDriveService();
            TreasureDataAddEventTestDriveRequest request = service.GetTreasureDataTestDrive(id);
            AddEvent(request, "TestDrive");
        }

        public void AddEventAppointment(int id)
        {
            ServiceAppointmentService service = new ServiceAppointmentService();
            TreasureDataAddEventServiceAppointmentRequest request = service.GetTreasureDataAppointment(id);
            AddEvent(request, "ServiceAppointment");
        }

    }
}