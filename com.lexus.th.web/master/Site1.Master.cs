using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.lexus.th.web.master
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lbUser.Text = "";
                if (Session["User"] != null)
                {
                    lbUser.Text = Session["User"].ToString();
                    switch (Session["Role"].ToString())
                    {
                        case "1":
                            menu.Text = GetMenuSuperAdmin();
                            break;
                        case "2":
                            menu.Text = GetMenuAdmin();
                            break;
                        default:
                            menu.Text = GetMenuOther();
                            break;
                    }
                }
            }
        }
        private string GetMenuSuperAdmin()
        {
            return
            "<li>" +
            "<a href=\"" + ResolveUrl("~/pages/PageUserManagement.aspx") + "\"><i class=\"glyphicon glyphicon-user\"></i> User Management</a>" +
            "</li>";
        }
        private string GetMenuAdmin()
        {
            return
            // Master
            "<li>" +
            "<a href=\"\"><i class=\"glyphicon glyphicon-th-list\"></i> Customer Section<span class=\"fa arrow\"></span></a>" +
            "<ul class=\"nav nav-second-level\">" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageCustomer.aspx") + "\"><i class=\"glyphicon glyphicon-user\"></i> Customer Profile</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageInitialData.aspx") + "\"><i class=\"glyphicon glyphicon-user\"></i> Initial Data</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PagePreference.aspx") + "\"><i class=\"glyphicon glyphicon-user\"></i> Preference</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageSurvey.aspx") + "\"><i class=\"glyphicon glyphicon-user\"></i> Survey</a>" +
            "    </li>" +
            "</ul>" +
            "</li>" +

            "<li>" +
            "<a href=\"\"><i class=\"glyphicon glyphicon-th-list\"></i> Privilege Section<span class=\"fa arrow\"></span></a>" +
            "<ul class=\"nav nav-second-level\">" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PagePrivilege.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Privilege</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PagePrivilegeCode.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Privilege Code</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PagePrivilegeVerify.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Privilege Verify</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PagePrivilegeSpecialQuota.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Privilege Special Quota</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PagePrivilegeBlockQuota.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Privilege Block Quota</a>" +
            "    </li>" +
            "</ul>" +
            "</li>" +

            "<li>" +
            "<a href=\"\"><i class=\"glyphicon glyphicon-th-list\"></i> News & Event Section<span class=\"fa arrow\"></span></a>" +
            "<ul class=\"nav nav-second-level\">" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageNews.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> News</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageEvent.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Events</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageEventCode.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Events Code</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageEventBlockQuota.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Events Block Quota</a>" +
            "    </li>" +
            "</ul>" +
            "</li>" +

            "<li>" +
            "<a href=\"\"><i class=\"glyphicon glyphicon-th-list\"></i> Dealer Section<span class=\"fa arrow\"></span></a>" +
            "<ul class=\"nav nav-second-level\">" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageAuthDealer.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Authorized Dealers</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageServiceDealer.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Service Dealers</a>" +
            "    </li>" +
            "</ul>" +
            "</li>" +

            "<li>" +
            "<a href=\"\"><i class=\"glyphicon glyphicon-th-list\"></i> Service Section<span class=\"fa arrow\"></span></a>" +
            "<ul class=\"nav nav-second-level\">" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageRoadsideAssistance.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Roadside Assistance</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageServiceDealer.aspx") + "\"><i class=\"glyphicon glyphicon-th-list\"></i> Service Appointment</a>" +
            "           <ul class=\"nav nav-second-level\">" +
            "               <li>" +
            "                   <a href=\"" + ResolveUrl("~/pages/PageTypeOfService.aspx") + "\"><i class=\"glyphicon glyphicon-minus\"></i> Type of Service</a>" +
            "               </li>" +
            "               <li>" +
            "                   <a href=\"" + ResolveUrl("~/pages/PageTypeOfServiceDetail.aspx") + "\"><i class=\"glyphicon glyphicon-minus\"></i> Type of Service Detail</a>" +
            "               </li>" +
            "               <li>" +
            "                   <a href=\"" + ResolveUrl("~/pages/PageServiceAppointment.aspx") + "\"><i class=\"glyphicon glyphicon-minus\"></i> Service Appointment</a>" +
            "               </li>" +
            "           </ul>" +
            "    </li>" +
            "</ul>" +
            "</li>" +

            "<li>" +
            "<a href=\"\"><i class=\"glyphicon glyphicon-th-list\"></i> More Section<span class=\"fa arrow\"></span></a>" +
            "<ul class=\"nav nav-second-level\">" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageECatalogue.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> E-Catalogue</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PagePriceList.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Price List</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageCalculator.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Payment</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageTestDrive.aspx") + "\"><i class=\"glyphicon glyphicon-th-list\"></i> Test Drive</a>" +
            "           <ul class=\"nav nav-second-level\">" +
            "               <li>" +
            "                   <a href=\"" + ResolveUrl("~/pages/PageTestDrive.aspx") + "\"><i class=\"glyphicon glyphicon-minus\"></i> Test Drive</a>" +
            "               </li>" +
            "               <li>" +
            "                   <a href=\"" + ResolveUrl("~/pages/PagePurchasePlan.aspx") + "\"><i class=\"glyphicon glyphicon-minus\"></i> Purchase Plan</a>" +
            "               </li>" +
            "               <li>" +
            "                   <a href=\"" + ResolveUrl("~/pages/PageServiceTestDrive.aspx") + "\"><i class=\"glyphicon glyphicon-minus\"></i> Test Drive Service</a>" +
            "               </li>" +
            "           </ul>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("") + "\"><i class=\"glyphicon glyphicon-th-list\"></i> Online Booking</a>" +
            "           <ul class=\"nav nav-second-level\">" +
            "               <li>" +
            "                   <a href=\"" + ResolveUrl("~/pages/PageRepurchase.aspx") + "\"><i class=\"glyphicon glyphicon-minus\"></i> Repurchase</a>" +
            "               </li>" +
            "               <li>" +
            "                   <a href=\"" + ResolveUrl("~/pages/PageReferral.aspx") + "\"><i class=\"glyphicon glyphicon-minus\"></i> Referral</a>" +
            "               </li>" +
            "               <li>" +
            "                   <a href=\"" + ResolveUrl("~/pages/PageBooking.aspx") + "\"><i class=\"glyphicon glyphicon-minus\"></i> Booking</a>" +
            "               </li>" +
            "               <li>" +
            "                   <a href=\"" + ResolveUrl("~/pages/PageBookingCode.aspx") + "\"><i class=\"glyphicon glyphicon-minus\"></i> Booking Code</a>" +
            "               </li>" +
            "               <li>" +
            "                   <a href=\"" + ResolveUrl("~/pages/PageOnlineBooking.aspx") + "\"><i class=\"glyphicon glyphicon-minus\"></i> Booking Register</a>" +
            "               </li>" +
            "           </ul>" +
            "    </li>" +
            "</ul>" +
            "</li>" +

            "<li>" +
            "<a href=\"\"><i class=\"glyphicon glyphicon-th-list\"></i> Notification Section<span class=\"fa arrow\"></span></a>" +
            "<ul class=\"nav nav-second-level\">" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageNotification2.aspx") + "\"><i class=\"glyphicon glyphicon-phone\"></i> Notification</a>" +
            "    </li>" +
            "</ul>" +
            "</li>" +

            "<li>" +
            "<a href=\"\"><i class=\"glyphicon glyphicon-th-list\"></i> Configuration<span class=\"fa arrow\"></span></a>" +
            "<ul class=\"nav nav-second-level\">" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageBanner2.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Home Banner Control</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageUpcoming.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Upcoming Events & News Control</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageCar.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Car Model</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageKatashikiMapping.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Katashiki Mapping</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageConfig.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Config</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/ManageDeeplink.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Manage Deeplink</a>" +
            "    </li>" +

            "</ul>" +
            "</li>" +

            //"<li>" +
            //"<a href=\"\"><i class=\"glyphicon glyphicon-th-list\"></i> Other<span class=\"fa arrow\"></span></a>" +
            //"<ul class=\"nav nav-second-level\">" +
            //"    <li>" +
            //"        <a href=\"" + ResolveUrl("~/pages/PageDealer.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Service Appointment</a>" +
            //"    </li>" +

            //"    <li>" +
            //"        <a href=\"" + ResolveUrl("~/pages/PageSendPush.aspx") + "\"><i class=\"glyphicon glyphicon-phone\"></i> Send Push</a>" +
            //"    </li>" +
            //"</ul>" +
            //"</li>" +
            // Report
            "<li>" +
            "<a href=\"\"><i class=\"glyphicon glyphicon-list-alt\"></i> Report<span class=\"fa arrow\"></span></a>" +
            "<ul class=\"nav nav-second-level\">" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageReportCustomer.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Register Report</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageReportEvent.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Event Register Report</a>" +
            "    </li>" +
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/PageReportRedeem.aspx") + "\"><i class=\"glyphicon glyphicon-plus\"></i> Redeem Report</a>" +
            "    </li>" +
            "</ul>" +
            "</li>";
        }
        private string GetMenuOther()
        {
            return
            "    <li>" +
            "        <a href=\"" + ResolveUrl("~/pages/zzzTest.aspx") + "\"><i class=\"glyphicon glyphicon-user\"></i> ทดสอบ</a>" +
            "    </li>";
        }
    }
}