using com.lexus.th.web.Deeplink;
using System.Runtime.Serialization;
using System.Web.Configuration;

namespace com.lexus.th.web
{

    [DataContract]
    public class DeeplinkRequest
    {
        [DataMember]
        public string domainUriPrefix { get; set; }

        [DataMember]
        public string link { get; set; }
        [DataMember]
        public DeeplinkAndroidInfo androidInfo { get; set; }

        [DataMember]
        public DeeplinkIosInfo iosInfo { get; set; }

        public DeeplinkRequest(string link)
        {
            domainUriPrefix = WebConfigurationManager.AppSettings["DomainUriPrefix"];
            this.link = link;
            androidInfo = new DeeplinkAndroidInfo();
            iosInfo = new DeeplinkIosInfo();
        }
    }
}