using System.Runtime.Serialization;
using System.Web.Configuration;

namespace com.lexus.th.web.Deeplink
{

    [DataContract]
    public class DeeplinkAndroidInfo
    {
        [DataMember]
        public string androidPackageName { get; set; }
        [DataMember]
        public string androidFallbackLink { get; set; }
        public DeeplinkAndroidInfo()
        {
            androidPackageName = WebConfigurationManager.AppSettings["AndroidPackageName"];
            androidFallbackLink = WebConfigurationManager.AppSettings["androidFallbackLink"];
        }
    }
}