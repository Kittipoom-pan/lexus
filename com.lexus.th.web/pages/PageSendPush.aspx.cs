using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace com.lexus.th.web.master
{
    public partial class PageSendPush : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Session["User"] == null)
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }
                    else if (Session["Role"].ToString() != "2")
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }

                    //if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsProduction"] == "0")
                    //{
                    //    dlDestination.Items.Add(new ListItem { Text = "Test", Value = "Test" });
                    //}
                    //else if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsProduction"] == "1")
                    //{
                    //    dlDestination.Items.Add(new ListItem { Text = "All", Value = "All" });
                    //    dlDestination.Items.Add(new ListItem { Text = "Member", Value = "Member" });
                    //    dlDestination.Items.Add(new ListItem { Text = "Android", Value = "Android" });
                    //    dlDestination.Items.Add(new ListItem { Text = "iOS", Value = "iOS" });
                    //}
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        private void ClientPopup(ModalType type, string message)
        {
            if (type == ModalType.Success)
            {
                lbSuccess.Text = message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdSucess", "$('#mdSuccess').modal();", true);
                upModalSuccess.Update();
            }
            if (type == ModalType.Error)
            {
                lbErrMsg.Text = message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdError", "$('#mdError').modal();", true);
                upModalError.Update();
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            ServiceModel firebase = null;
            try
            {
                LogService ls = new LogService();
                APIService api = new APIService();

                string msgPush = tbMessage.Text;
                string memberID = tbMemberID.Text;
                string[] dataMember = memberID.Split(',');
                if (dataMember.Length > 0)
                {
                    for (var i = 0; i < dataMember.Length; i++)
                    {
                        string member = dataMember[i];
                        if (member != "")
                        {
                            //หา token
                            DataTable dt = api.GetDeviceToken(member);
                            if (dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.Rows[0];
                                string device_token = dr["DEVICE_TOKEN"].ToString();
                                string device_Type = dr["DEVICE_TYPE"].ToString();
                                //firebase = api.SendPush(device_token, msgPush, "SendPush", 1, device_Type, "", "", "", "","");
                            }
                            else
                            {
                                ClientPopup(ModalType.Success, "Not has token no");
                            }

                            if (firebase.Success) // Post Firebase
                            {
                                ClientPopup(ModalType.Success, "Completed");
                            }
                            else
                            {
                                throw new Exception("[Firebase failed] " + firebase.Message);
                            }
                        }
                    }
                }
                else
                {
                    DataTable dt = api.GetDeviceToken(memberID);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        //string device_token = dr["DEVICE_TOKEN"].ToString();
                        //string device_Type = dr["DEVICE_TYPE"].ToString();
                        //firebase = api.SendPush(device_token, msgPush, "SendPush", 1, device_Type, "", "", "", "", "");
                    }
                    else
                    {
                        ClientPopup(ModalType.Success, "Not has token no");
                    }

                    if (firebase.Success) // Post Firebase
                    {
                        ClientPopup(ModalType.Success, "Completed");
                    }
                    else
                    {
                        throw new Exception("[Firebase failed] " + firebase.Message);
                    }
                }

                string tokenAndroid = tbTokenAndroid.Text.Trim();
                string tokenIOS = tbTokenIOS.Text.Trim();
                string[] dataAndroid = tokenAndroid.Split(',');
                string[] dataIOS = tokenIOS.Split(',');
                if ((dataAndroid.Length > 0) || (dataIOS.Length > 0))
                {
                    int a = 0;
                    int ios = 0;
                    for (var i = 0; i < dataAndroid.Length; i++)
                    {
                        //string token = dataAndroid[i];
                        //firebase = api.SendPush(token, msgPush, "SendPush", 1, "android", "", "", "", "", "");
                        //a = i + 1;
                        //lb_android.Text = a.ToString();
                    }
                    for (var i = 0; i < dataIOS.Length; i++)
                    {
                        //string token = dataIOS[i];
                        //firebase = api.SendPush(token, msgPush, "SendPush", 1, "ios", "", "", "", "", "");
                        //ios = i + 1;
                        //lb_ios.Text = ios.ToString();
                    }
                    lb_android.Text = dataAndroid.Length.ToString();
                    lb_ios.Text = dataIOS.Length.ToString();
                }
                else
                {
                    if (tbTokenAndroid.Text != "")
                    {
                        //firebase = api.SendPush(tokenAndroid, msgPush, "SendPush", 1, "android", "", "", "", "", "");
                    }
                    if (tbTokenIOS.Text != "")
                    {
                        //firebase = api.SendPush(tokenIOS, msgPush, "SendPush", 1, "ios", "", "", "", "", "");
                    }
                }

                //log
                ls.InsertLogPush(msgPush, memberID, tokenAndroid, tokenIOS);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
    }
}