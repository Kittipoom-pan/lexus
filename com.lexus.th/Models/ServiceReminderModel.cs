using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace com.lexus.th
{

    public class ServiceReminderBody
    {
        public ServiceReminderModel nextservicefollowup { get; set; }
    }
    public class ServiceReminderModel
    {
        public string vin { get; set; }
        public string maintenanceItem { get; set; }
        public string messageType { get; set; }
        public string recommendServiceinDate { get; set; }

        public (bool,string) ValidateBody()
        {
            if (string.IsNullOrEmpty(vin))
            {
                return (false, "vin");
            }else if (string.IsNullOrEmpty(maintenanceItem))
            {
                return (false, "maintenanceItem");
            }
            else if (string.IsNullOrEmpty(messageType))
            {
                return (false, "messageType");
            }
            else if (string.IsNullOrEmpty(recommendServiceinDate))
            {
                return (false, "recommendServiceinDate");
            }
            return (true, "");
        }

        public bool ValidateDateTime()
        {
            DateTime date;
            if (!DateTime.TryParseExact(recommendServiceinDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return false;
            }
            return true;
        }

        public string GetDateText()
        {
            DateTime date = DateTime.ParseExact(recommendServiceinDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            string dateText = date.ToString("dd MMMM yyyy", new CultureInfo("th-TH"));

            return dateText;
        }

        public (bool,string,int) ValidateMaxLength()
        {
            if (maintenanceItem.Length > 7)
            {
                return (false, "maintenanceItem", 7);
            }
            if (vin.Length > 32)
            {
                return (false, "vin", 32);
            }
            if (messageType.Length > 2)
            {
                return (false, "messageType", 2);
            }
            return (true, "", 0);
        }

        public (bool, string, string) ValidateDataType()
        {
            int number;
            bool maintenanceitem = Int32.TryParse(maintenanceItem, out number);
            bool messagetype = Int32.TryParse(messageType, out number);
            if (!maintenanceitem)
            {
                return (false, "maintenanceItem", "int");
            }
            if (!messagetype)
            {
                return (false, "messageType", "int");
            }
            return (true, "", "");
        }
    }

    public class ServiceReminderHeader
    {
        public string interfaceId { get; set; }
        public string fromSystem { get; set; }
        public string toSystem { get; set; }
        public string countryCode { get; set; }
        public string transmissionDate { get; set; }
        public string x_api_key { get; set; }

        
        public ServiceReminderHeader(HttpRequestHeaders headers)
        {
            if(headers.Contains("interfaceId"))
            {
                this.interfaceId = headers.GetValues("interfaceId").First().ToString();
            }
            if (headers.Contains("fromSystem"))
            {
                this.fromSystem = headers.GetValues("fromSystem").First().ToString();
            }
            if (headers.Contains("toSystem"))
            {
                this.toSystem = headers.GetValues("toSystem").First().ToString();
            }
            if (headers.Contains("countryCode"))
            {
                this.countryCode = headers.GetValues("countryCode").First().ToString();
            }
            if (headers.Contains("transmissionDate"))
            {
                this.transmissionDate = headers.GetValues("transmissionDate").First().ToString();
            }
            if (headers.Contains("x-api-key"))
            {
                this.x_api_key = headers.GetValues("x-api-key").First().ToString();
            }
        }
        public bool ValidateBadRequest()
        {
            if (string.IsNullOrEmpty(interfaceId) || string.IsNullOrEmpty(fromSystem) 
                || string.IsNullOrEmpty(toSystem) || string.IsNullOrEmpty(countryCode)
                || string.IsNullOrEmpty(transmissionDate) || string.IsNullOrEmpty(x_api_key))
            {
                return false;
            }

            return true;
        }

        public bool ValidateUnauthorized()
        {
            var x_key = WebConfigurationManager.AppSettings["service_reminder_key"];

            if (string.IsNullOrEmpty(x_api_key) || x_key != x_api_key)
            {
                return false;
            }

            return true;
        }
    }

    public class ServiceReminderResponse
    {
        public string statusCode { get; set; }
        public string message { get; set; }
        public ServiceReminderResponseData data { get; set; }

        public ServiceReminderResponse getMaxLength(string vin, string maintenanceItem, string param,int length)
        {
            this.statusCode = "200";
            this.message = "Success";
            this.data = new ServiceReminderResponseData()
            {
                vin = vin,
                MaintenanceItem = maintenanceItem,
                returnCode = "0006",
                returnMessage = param +" max length "+length+" exceeds"
            };
            return this;
        }

        public ServiceReminderResponse getNoBody(string vin, string maintenanceItem, string tagname)
        {
            this.statusCode = "200";
            this.message = "Success";
            this.data = new ServiceReminderResponseData()
            {
                vin = vin,
                MaintenanceItem = maintenanceItem,
                returnCode = "0005",
                returnMessage = tagname +" cannot be blank"
            };
            return this;
        }
        
        public ServiceReminderResponse getValidateDate(string vin, string maintenanceItem)
        {
            this.statusCode = "200";
            this.message = "Success";
            this.data = new ServiceReminderResponseData()
            {
                vin = vin,
                MaintenanceItem = maintenanceItem,
                returnCode = "0002",
                returnMessage = "Invalid request format"
            };
            return this;
        }

        public ServiceReminderResponse getUnauthorized(string vin, string maintenanceItem)
        {
            this.statusCode = "401";
            this.message = "Unauthorized access";
            this.data = new ServiceReminderResponseData()
            {
                vin = vin,
                MaintenanceItem = maintenanceItem,
                returnCode = "9999",
                returnMessage = "Internal application error"
            };
            return this;
        }

        public ServiceReminderResponse getHeaderNotFound(string vin, string maintenanceItem)
        {
            this.statusCode = "400";
            this.message = "Missing require parameter";
            this.data = new ServiceReminderResponseData()
            {
                vin = vin,
                MaintenanceItem = maintenanceItem,
                returnCode = "9999",
                returnMessage = "Internal application error"
            };
            return this;
        }

        public ServiceReminderResponse getRecordNotFound(string vin, string maintenanceItem)
        {
            this.statusCode = "200";
            this.message = "Success";
            this.data = new ServiceReminderResponseData()
            {
                vin = vin,
                MaintenanceItem = maintenanceItem,
                returnCode = "0001",
                returnMessage = "Record not found"
            };
            return this;
        }

        public ServiceReminderResponse getMessageTypeCharacter(string vin, string maintenanceItem, string param, string dataType)
        {
            this.statusCode = "200";
            this.message = "Success";
            this.data = new ServiceReminderResponseData()
            {
                vin = vin,
                MaintenanceItem = maintenanceItem,
                returnCode = "0004",
                returnMessage = "Data Type for "+param+" should be " +dataType
            };
            return this;
        }

        public ServiceReminderResponse getTokenNotFound(string vin, string maintenanceItem)
        {
            this.statusCode = "200";
            this.message = "Success";
            this.data = new ServiceReminderResponseData()
            {
                vin = vin,
                MaintenanceItem = maintenanceItem,
                returnCode = "0000",
                returnMessage = "Process completed successfully"
            };
            return this;
        }

        public ServiceReminderResponse getException(string vin, string maintenanceItem)
        {
            this.statusCode = "500";
            this.message = "Internal server error";
            this.data = new ServiceReminderResponseData()
            {
                vin = vin,
                MaintenanceItem = maintenanceItem,
                returnCode = "9999",
                returnMessage = "Internal application error"
            };
            return this;
        }
    }

    public class ServiceReminderResponseData
    {
        public string vin { get; set; }
        public string MaintenanceItem { get; set; }
        public string returnCode { get; set; }
        public string returnMessage { get; set; }
    }

    public class ServiceReminderMember
    {
        public string MemberId { get; set; }
        public string Plate_no { get; set; }
        public string Message { get; set; }
    }

    public class ServiceReminderMessage
    {
        public string title_msg { get; set; }
        public string detail_msg { get; set; }
        public string link_type { get; set; }
        public string schedule_date { get; set; }
        public string GetScheduleDate()
        {
            schedule_date = DateTime.Now.ToString("yyyy-MM-dd");

            return schedule_date;
        }
    }

    public class ServiceReminderDevice
    {
        public string device_token { get; set; }
        public string device_type { get; set; }
        public string reference_id { get; set; }
        public string link_type { get; set; }
        public string link_url { get; set; }
    }
}