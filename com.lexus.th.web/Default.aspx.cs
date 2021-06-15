using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.lexus.th.web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lbError.Text = "";
            try
            {
                string username = tbUsername.Text;
                string password = tbPassword.Text;
                // Role (1) Super Admin, (2) Admin, (3) Dealer, (4) Sales
                LoginService srv = new LoginService();
                LoginService.UserInfo user = srv.GetUserInfo(username, password);
                if (user != null)
                {
                    if (user.Role == "4" && string.IsNullOrEmpty(user.SellerId))
                    {
                        Session["User"] = null;
                        Session["Seller"] = null;
                        Session["Role"] = null;
                        Session["Dealer"] = null;

                        lbError.Text = "Seller ID cannot be empty!";
                    }
                    else
                    {
                        Session["User"] = user.UserName;
                        Session["Seller"] = user.SellerId;
                        Session["Role"] = user.Role;
                        Session["Dealer"] = user.Dealer;
                        Response.Redirect("~/Lexus.aspx", false);
                    }
                }
                else
                {
                    lbError.Text = "Incorrect User Name or Password!";
                }
            }
            catch (Exception ex)
            {
                lbError.Text = "Login Fail";
            }
        }
    }
}