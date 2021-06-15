
using System.Runtime.Serialization;
using System.Web.Configuration;

namespace com.lexus.th.web.Deeplink
{
    [DataContract]
    public class DeeplinkIosInfo
    {
        [DataMember]
        public string iosBundleId { get; set; }
        [DataMember]
        public string iosAppStoreId { get; set; }

        public DeeplinkIosInfo()
        {
             iosBundleId = WebConfigurationManager.AppSettings["IosBundleId"];
             iosAppStoreId = WebConfigurationManager.AppSettings["iosAppStoreId"];
        }
    }
}