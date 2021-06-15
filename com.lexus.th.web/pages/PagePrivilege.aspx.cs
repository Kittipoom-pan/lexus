using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace com.lexus.th.web.master
{
    public partial class PagePrivilege : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }
        private enum ImageType { Img1, Img2 ,Img3}

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
                    else
                    {
                        ViewState["ImagePath"] = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePathPrivilege"];
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();
                    }
                }
                this.RegisterPostBackControl();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindGridData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvCust_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCust.PageIndex = e.NewPageIndex;
                BindGridData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void BindGridData()
        {
            try
            {
                PrivilegeService srv = new PrivilegeService();
                gvCust.DataSource = srv.GetPrivileges(txtSearch.Text);
                gvCust.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gvCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvCust.SelectedIndex;
                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[rowIndex].Values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(dlPrivilegeType.SelectedValue == "1" && dlExpiryType.SelectedValue == "2")
                {
                    DateTime myDate;
                    if (DateTime.TryParse(tbRedExpiry.Text, out myDate))
                    {
                        string time = myDate.ToString("HH:mm");
                        string hour = time.Substring(0, 2);
                        int hourInt = int.Parse(hour);
                        if (hourInt >= 24)
                        {
                            ClientPopup(ModalType.Warning, "กรุณากรอกรูปแบบเวลาให้ถูกต้อง");
                            return;
                        }
                    }
                    else
                    {
                        ClientPopup(ModalType.Warning, "กรุณากรอกรูปแบบเวลาให้ถูกต้อง");
                        return;
                    }
                }
               
                if (lbImg1.Text != "")
                {
                    DeleteImage(ImageType.Img1);
                }
                if (lbImg2.Text != "")
                {
                    DeleteImage(ImageType.Img2);
                }
                if (lbImg3.Text != "")
                {
                    DeleteImage(ImageType.Img3);
                }

                string title = tbTitle.Text;
                string desc = tbDesc.Text;
                //string period = tbPeriod.Text;
                string periodStart = tbPeriodStart.Text;
                string periodEnd = tbPeriodEnd.Text;
                string redCondition = tbRedCondition.Text;
                //string redPeriod = tbRedPeriod.Text;
                string redLocation = tbRedLocation.Text;
                //check format time
                
                string image = (tbImg1.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg1.Text) : "";
                string image_1 = (tbImg2.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg2.Text) : "";
               
                string dispStart = tbDispStart.Text;
                string dispEnd = tbDispEnd.Text;
                string user = Session["User"].ToString();
                string privilegeType = dlPrivilegeType.SelectedValue;

                string redeem_display_type = "";
                string redeem_display_image = "";
                //if (privilegeType=="3")
                {
                    redeem_display_type = dlRedeemDisplay.SelectedValue;
                    redeem_display_image = (tbImg3.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg3.Text) : "";
                    if (redeem_display_type == "4" && redeem_display_image == "")
                    {
                        ClientPopup(ModalType.Warning, "กรุณา Upload Picture Redeem Display");
                        return;
                    }
                }

                string redExpiry = tbRedExpiry.Text;

                redExpiry = (privilegeType == "3" ? "0" : redExpiry);
                string displayType = dlDisplayType.SelectedValue;
                //string period = WebUtility.GetDateFormat(Convert.ToDateTime("",)) + " - " + WebUtility.GetDateFormat(Convert.ToDateTime(period_end));
                string redMaxCount = tbMaxRedeemCnt.Text;

                string desc_th = tbDescTH.Text;
                string redCondition_th = tbRedConditionTH.Text;
                string redLocation_th = tbRedLocationTH.Text;
                string thk_message = tbThkMessage.Text;
                string thk_message_th = tbThkMessageTH.Text;
                string expiryType = dlExpiryType.SelectedValue;
                string is_show_remaining = cb_show_remaining.Checked == true ? "1" : "0";
                string order = tbOrder.Text;

                string period = "";
                string redPeriod = "";
                DateTime _periodStartDate, _periodEndDate;
                if (periodStart.Length > 0)
                {
                    _periodStartDate = DateTime.ParseExact(periodStart, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                    period += WebUtility.GetDateFormat(_periodStartDate);
                    redPeriod += WebUtility.GetDateFormat(_periodStartDate);
                }
                period += " - ";
                redPeriod += " - ";
                if (periodEnd.Length > 0)
                {
                    _periodEndDate = DateTime.ParseExact(periodEnd, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                    period += WebUtility.GetDateFormat(_periodEndDate);

                    redPeriod += WebUtility.GetDateFormat(_periodEndDate);
                }

                DateTime DisStart = DateTime.ParseExact(dispStart, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime DisEnd = DateTime.ParseExact(dispEnd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime PerStart = DateTime.ParseExact(periodStart, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime PerEnd = DateTime.ParseExact(periodEnd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                String redeem_display_html = tbRedeemDisplayHTML.Text;
                string redeem_display_height = tbRedeemDisplayHeight.Text;
                string redeem_display_width = tbRedeemDisplayWidth.Text;
                string period_type = dlPeriodType.SelectedValue;
                string period_start_in_week = dlPeriodStartInWeek.SelectedValue;
                string period_start_in_month = tbPeriodStartInMonth.Text;
                string customer_usage_with_car_total = cb_customer_usage_with_car_total.Checked ? "1" : "0";
                string customer_usage_per_period = tbCustomerUsagePerPeriod.Text;
                string period_quota = tbPeriodQuota.Text;
                string return_redeem_when_verify_expire = cb_return_redeem_when_verify_expire.Checked && cb_return_redeem_when_verify_expire.Visible ? "1" : "0";
                string repeat_redeem_when_verify_expire = cb_repeat_redeem_when_verify_expire.Checked && cb_repeat_redeem_when_verify_expire.Visible ? "1" : "0";

                if (DisEnd < DisStart)
                {
                    ClientPopup(ModalType.Warning, "Please Choose Display End > Display Start");
                }
                else if (PerEnd < PerStart)
                {
                    ClientPopup(ModalType.Warning, "Please Choose Period End > Period Start");
                }
                else
                {
                    PrivilegeService srv = new PrivilegeService();
                    if (lbType.Text == "Add")
                    {
                        srv.AddPrivilege(title, desc, period, periodStart, periodEnd, image, redCondition, redPeriod, redLocation, redExpiry, dispStart, dispEnd, 
                            privilegeType, user, redMaxCount, desc_th, redCondition_th, redLocation_th, thk_message, thk_message_th, expiryType, is_show_remaining, 
                            order, image_1, displayType, redeem_display_image, redeem_display_type, period_type, customer_usage_per_period, period_quota,
                            period_start_in_week, period_start_in_month, return_redeem_when_verify_expire, customer_usage_with_car_total, 
                            redeem_display_html, redeem_display_height, redeem_display_width, repeat_redeem_when_verify_expire);
                        BindGridData();

                        ClientPopup(ModalType.Success, "Completed");
                    }
                    else if (lbType.Text == "Edit")
                    {
                        srv.UpdatePrivilege(title, desc, period, periodStart, periodEnd, image, redCondition, redPeriod, redLocation, redExpiry, dispStart, dispEnd, 
                            privilegeType, user, lbId.Text, redMaxCount, desc_th, redCondition_th, redLocation_th, thk_message, thk_message_th, expiryType, is_show_remaining, 
                            order, image_1, displayType, redeem_display_image, redeem_display_type, period_type, customer_usage_per_period, period_quota,
                            period_start_in_week, period_start_in_month, return_redeem_when_verify_expire, customer_usage_with_car_total, 
                            redeem_display_html, redeem_display_height, redeem_display_width, repeat_redeem_when_verify_expire);
                        BindGridData();

                        ClientPopup(ModalType.Success, "Completed");
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
                ClientPopup(ModalType.Error, "Cannot save. Please try again later.");
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdDelConfirm').modal('hide');", true);
                upModelDel.Update();

                string id = lbDelId.Text;
                string user = Session["User"].ToString();

                PrivilegeService srv = new PrivilegeService();
                srv.DeletePrivilege(id, user);

                BindGridData();
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void ClientPopup(ModalType type, string message)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
            upModalAdd.Update();

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdWarning').modal('hide');", true);
            //upModalWarning.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal('hide');", true);
            upModelDel.Update();

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
            if (type == ModalType.Warning)
            {
                lbWarning.Text = message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdWarning", "$('#mdWarning').modal();", true);
                upModalWarning.Update();
            }
        }
        protected void btnAddModal_Click(object sender, EventArgs e)
        {
            try
            {
                tbTitle.Text = "";
                tbDesc.Text = "";
                //tbPeriod.Text = "";
                tbPeriodStart.Text = "";
                tbPeriodEnd.Text = "";
                tbRedCondition.Text = "";
                //tbRedPeriod.Text = "";
                tbRedLocation.Text = "";
                tbRedExpiry.Text = "0";
                tbImg1.Text = "";
                tbImg2.Text = "";
                tbImg3.Text = "";
                tbDispStart.Text = "";
                tbDispEnd.Text = "";
                lbType.Text = "Add";
                dlPrivilegeType.SelectedIndex = -1;
                dlRedeemDisplay.SelectedIndex = -1;
                dlDisplayType.SelectedIndex = -1;
                tbMaxRedeemCnt.Text = "1";
                tbDescTH.Text = "";
                tbRedConditionTH.Text = "";
                tbRedLocationTH.Text = "";
                tbThkMessage.Text = "";
                tbThkMessageTH.Text = "";
                dlExpiryType.SelectedIndex = -1;
                cb_show_remaining.Checked = false;
                tbOrder.Text = "";

                dlPeriodType.SelectedIndex = -1;
                dlPeriodStartInWeek.SelectedIndex = 0;
                tbPeriodStartInMonth.Text = "1";
                cb_customer_usage_with_car_total.Checked = false;
                tbCustomerUsagePerPeriod.Text = "1";
                tbPeriodQuota.Text = "1";
                cb_return_redeem_when_verify_expire.Checked = false;
                cb_repeat_redeem_when_verify_expire.Checked = false;
                tbRedeemDisplayHTML.Text = "";
                tbRedeemDisplayHeight.Text = "";
                tbRedeemDisplayWidth.Text = "";

                lbImg1.Text = "";
                pnRedeemType.Visible = true;
                dlDisplayType_SelectedIndexChanged(null, null);
                dlPrivilegeType_SelectedIndexChanged(null, null);
                dlRedeemDisplay_SelectedIndexChanged(null, null);
                dlPeriodType_SelectedIndexChanged(null, null);
                cb_customer_usage_with_car_total_CheckedChanged(null, null);
                RegisterScript();
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvCust_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    lbDelId.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvCust.DataKeys != null)
                    {
                        lbDelId.Text = gvCust.DataKeys[index].Values["ID"].ToString();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal();", true);
                        upModelDel.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void RegisterPostBackControl()
        {
            foreach (GridViewRow row in gvCust.Rows)
            {
                LinkButton lnkFull = row.FindControl("gvBtnEdit") as LinkButton;
                if (lnkFull != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
                }
            }
        }
        protected void gvCust_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton lnkFull = e.Row.FindControl("gvBtnEdit") as LinkButton;
            if (lnkFull != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
            }
        }
        protected void gvBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lbType.Text = "";
                lbId.Text = "";

                lbImg1.Text = "";
                lbImg2.Text = "";
                lbImg3.Text = "";
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    PrivilegeService srv = new PrivilegeService();
                    DataRow row = srv.GetPrivilegeById(id);
                    if (row != null)
                    {
                        tbTitle.Text = row["TITLE"].ToString();
                        tbDesc.Text = row["DESC"].ToString();
                        //tbPeriod.Text = row["PERIOD"].ToString();
                        tbPeriodStart.Text = (row["PERIOD_START"] == DBNull.Value) ? "" : Convert.ToDateTime(row["PERIOD_START"]).ToString("dd/MM/yyyy HH:mm");
                        tbPeriodEnd.Text = (row["PERIOD_END"] == DBNull.Value) ? "" : Convert.ToDateTime(row["PERIOD_END"]).ToString("dd/MM/yyyy HH:mm");
                        tbRedCondition.Text = row["RED_CONDITION"].ToString();
                        //tbRedPeriod.Text = row["RED_PERIOD"].ToString();
                        tbRedLocation.Text = row["RED_LOCATION"].ToString();
                        
                        tbImg1.Text = ImageService.Split1(row["IMAGE"].ToString());
                        tbImg2.Text = ImageService.Split1(row["IMAGE_1"].ToString());
                        tbImg3.Text = ImageService.Split1(row["redeem_display_image"].ToString());
                        tbDispStart.Text = (row["DISPLAY_START"] == DBNull.Value) ? "" : Convert.ToDateTime(row["DISPLAY_START"]).ToString("dd/MM/yyyy HH:mm");
                        tbDispEnd.Text = (row["DISPLAY_END"] == DBNull.Value) ? "" : Convert.ToDateTime(row["DISPLAY_END"]).ToString("dd/MM/yyyy HH:mm");
                        dlPrivilegeType.SelectedValue = row["PRIVILEDGE_TYPE"].ToString();
                        dlPrivilegeType_SelectedIndexChanged(null, null);
                        //if (dlPrivilegeType.SelectedValue == "3")
                        {
                            if (row["redeem_display_type"] != DBNull.Value)
                            {
                                dlRedeemDisplay.SelectedValue = row["redeem_display_type"].ToString();
                                tbImg3.Text = ImageService.Split1(row["redeem_display_image"].ToString());
                                tbRedeemDisplayHTML.Text = row["redeem_display_html"].ToString();
                                tbRedeemDisplayHeight.Text = row["redeem_display_height"].ToString();
                                tbRedeemDisplayWidth.Text = row["redeem_display_width"].ToString();
                            }
                            else
                            {
                                dlRedeemDisplay.SelectedIndex = -1;
                                tbImg3.Text = "";
                                tbRedeemDisplayHTML.Text = "";
                                tbRedeemDisplayHeight.Text = "";
                                tbRedeemDisplayWidth.Text = "";
                            }
                        }      
                    
                        dlRedeemDisplay_SelectedIndexChanged(null, null);
                        dlDisplayType.SelectedValue = row["display_type"].ToString();
                        dlDisplayType_SelectedIndexChanged(null, null);
                        tbMaxRedeemCnt.Text = row["REED_TIME"].ToString();
                        tbDescTH.Text = row["desc_th"].ToString();
                        tbRedConditionTH.Text = row["red_condition_th"].ToString();
                        tbRedLocationTH.Text = row["red_location_th"].ToString();
                        tbThkMessage.Text = row["thk_message"].ToString();
                        tbThkMessageTH.Text = row["thk_message_th"].ToString();
                        dlExpiryType.SelectedValue =  row["red_expiry_type"].ToString();
                        tbRedExpiry.Text = row["red_expiry_type"].ToString() == "2" ? row["RED_EXPIRY_TIME"].ToString() : row["RED_EXPIRY"].ToString();
                        cb_show_remaining.Checked = (int.Parse(row["is_show_remaining"].ToString()) == 1 ? true : false);
                        tbOrder.Text = row["order"].ToString();

                        dlPeriodType.SelectedValue = row["period_type"].ToString();
                        dlPeriodStartInWeek.SelectedValue = row["period_start_in_week"].ToString();
                        tbPeriodStartInMonth.Text = row["period_start_in_month"].ToString();
                        cb_customer_usage_with_car_total.Checked = row["customer_usage_with_car_total"].ToString() == "1";
                        tbCustomerUsagePerPeriod.Text = row["customer_usage_per_period"].ToString();
                        tbPeriodQuota.Text = row["period_quota"].ToString();
                        cb_return_redeem_when_verify_expire.Checked = row["return_redeem_when_verify_expire"].ToString() == "1";
                        cb_repeat_redeem_when_verify_expire.Checked = row["repeat_redeem_when_verify_expire"].ToString() == "1";
                        dlPeriodType_SelectedIndexChanged(null, null);
                        cb_customer_usage_with_car_total_CheckedChanged(null, null);

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                        upModalAdd.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void UploadImage_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            switch (btn.ID)
            {
                case "btnUpImg1":
                    UploadImage(ImageType.Img1);
                    break;
                case "btnUpImg2":
                    UploadImage(ImageType.Img2);
                    break;
                case "btnUpImg3":
                    UploadImage(ImageType.Img3);
                    break;
            }
        }
        protected void DeleteImage_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            switch (btn.ID)
            {
                case "btnDelImg1":
                    lbImg1.Text = ImageType.Img1.ToString();
                    //DeleteImage(ImageType.Img1);
                    break;
                case "btnDelImg2":
                    lbImg2.Text = ImageType.Img2.ToString();
                    //DeleteImage(ImageType.Img2);
                    break;
                case "btnDelImg3":
                    lbImg3.Text = ImageType.Img3.ToString();               
                    break;
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
            upModalAdd.Update();
        }
        private void UploadImage(ImageType type)
        {
            try
            {
                //string root = Server.MapPath("~");
                //string parent = Path.GetDirectoryName(root);
                //string grandParent = Path.GetDirectoryName(parent);
                //string imgDir = grandParent + "\\" + ViewState["ImagePath"].ToString().Replace("/", "\\") + "\\";
                //string imgDir = root + "\\" + ViewState["ImagePath"].ToString().Replace("/", "\\") + "\\";
                string imgDir = ViewState["ImagePath"] + "\\";
                FileUpload fileUpload = new FileUpload();
                fileUpload = upModalAdd.FindControl("up" + type.ToString()) as FileUpload;
                TextBox tbImage = upModalAdd.FindControl("tb" + type.ToString()) as TextBox;
                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;

                lbStatus.Text = "";

                if (fileUpload.HasFile)
                {
                   if ((fileUpload.PostedFile.ContentType == "image/jpeg") || (fileUpload.PostedFile.ContentType == "image/png"))
                   {
                       using (System.Drawing.Image img = System.Drawing.Image.FromStream(fileUpload.PostedFile.InputStream))
                       {
                            if (!Directory.Exists(imgDir))
                            {
                                Directory.CreateDirectory(imgDir);
                            }

                            if (tbImage.Text != "")
                            {
                                if (File.Exists(imgDir + tbImage.Text))
                                    File.Delete(imgDir + tbImage.Text);
                            }
                            fileUpload.SaveAs(imgDir + fileUpload.FileName);
                            tbImage.Text = fileUpload.FileName;
                            //if (img.Height == 764 && img.Width == 1500)
                            //{

                            //}
                            //else
                            //{
                            //    throw new Exception("Allow file size 1500 x 764 only!");
                            //}
                        }
                   }
                   else
                   {
                       throw new Exception("Allow file type JPG and PNG only!");
                   }
                }
                else
                {
                    throw new Exception("No file selected!");
                }

                RegisterScript();
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;
                lbStatus.Text = ex.Message;
                RegisterScript();
                upModalAdd.Update();
            }
        }
        private void DeleteImage(ImageType type)
        {
            try
            {
                //string root = Server.MapPath("~");
                ////string parent = Path.GetDirectoryName(root);
                ////string grandParent = Path.GetDirectoryName(parent);
                ////string imgDir = grandParent + "\\" + ViewState["ImagePath"].ToString().Replace("/", "\\") + "\\";
                //string imgDir = root + "\\" + ViewState["ImagePath"].ToString().Replace("/", "\\") + "\\";
                string imgDir = ViewState["ImagePath"] + "\\";

                TextBox tbImage = upModalAdd.FindControl("tb" + type.ToString()) as TextBox;
                
                if (tbImage.Text != "")
                {
                    if (File.Exists(imgDir + tbImage.Text))
                        File.Delete(imgDir + tbImage.Text);
                    tbImage.Text = "";
                }

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                //upModalAdd.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dlPrivilegeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dlPrivilegeType.SelectedValue == "1")
            {
                tbRedExpiry.Text = "0";
                pnRedeemType.Visible = true;
                dlExpiryType.SelectedIndex = -1;
                //pnRedeemDisplay.Visible = false;
                //dlRedeemDisplay.SelectedIndex = -1;
                //tbImg3.Text = "";
            }
            else if(dlPrivilegeType.SelectedValue == "3")
            {
                tbRedExpiry.Text = "0";
                pnRedeemType.Visible = false;
                //dlExpiryType.SelectedIndex = -1;
                //pnRedeemDisplay.Visible = true;
                //dlRedeemDisplay.SelectedIndex = -1;
                //tbImg3.Text = "";
            }

            //if (sender != null)
            {
                RegisterScript();
                upModalAdd.Update();
            }
        }
        protected void dlRedeemDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dlRedeemDisplay.SelectedValue == "4")
            {
                pnPictureRedeem.Visible = true;
                pnHTMLRedeem.Visible = true;
            }
            else
            {
                pnPictureRedeem.Visible = false;
                pnHTMLRedeem.Visible = false;
            }

            //if (sender != null)
            {
                RegisterScript();
                upModalAdd.Update();
            }
        }

        protected void dlPeriodType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnPeriodStartInWeek.Visible = dlPeriodType.SelectedValue == "2";
            pnPeriodStartInMonth.Visible = dlPeriodType.SelectedValue == "3" || dlPeriodType.SelectedValue == "6" || dlPeriodType.SelectedValue == "9";

            //if (sender != null)
            {
                RegisterScript();
                upModalAdd.Update();
            }
        }

        protected void cb_customer_usage_with_car_total_CheckedChanged(object sender, EventArgs e)
        {
            pnCustomerUsagePerPeriod.Visible = !cb_customer_usage_with_car_total.Checked;

            //if (sender != null)
            {
                RegisterScript();
                upModalAdd.Update();
            }
        }

        private void RegisterScript()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();" +
                "$('#ContentPlaceHolder1_tbDispStart')" +
                ".datetimepicker({defaultDate: new Date(),format: 'DD/MM/YYYY HH:mm'})" +
                ".on('changeDate', function(ev){$(this).blur();$(this).datepicker('hide');});" +
                "$('#ContentPlaceHolder1_tbDispEnd')" +
                 ".datetimepicker({defaultDate: new Date(),format: 'DD/MM/YYYY HH:mm'})" +
                ".on('changeDate', function(ev){$(this).blur();$(this).datepicker('hide');});" +
                "$('#ContentPlaceHolder1_tbPeriodStart')" +
                       ".datetimepicker({defaultDate: new Date(),format: 'DD/MM/YYYY HH:mm'})" +
                ".on('changeDate', function(ev){$(this).blur();$(this).datepicker('hide');});" +
                "$('#ContentPlaceHolder1_tbPeriodEnd')" +
                         ".datetimepicker({defaultDate: new Date(),format: 'DD/MM/YYYY HH:mm'})" +
                ".on('changeDate', function(ev){$(this).blur();$(this).datepicker('hide');});", true);
        }

        protected void dlDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_return_redeem_when_verify_expire.Visible = dlDisplayType.SelectedValue == "2";
            //cb_repeat_redeem_when_verify_expire.Visible = dlDisplayType.SelectedValue == "2";

            ////if (sender != null)
            //{
                RegisterScript();
                upModalAdd.Update();
            //}
        }
    }
}