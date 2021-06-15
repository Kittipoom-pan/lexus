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
    public partial class PageBookingCode : System.Web.UI.Page
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
                    else
                    {
                        BindBookingStatus();
                        BindGridData(dlBooking.SelectedValue);
                    }
                }
                this.RegisterPostBackControl();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void dlBookingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindBooking(dlBookingType.SelectedValue);
                BindGridData(dlBooking.SelectedValue);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void dlBooking_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindGridData(dlBooking.SelectedValue);
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
                BindGridData(dlBooking.SelectedValue);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        private void BindBookingStatus()
        {
            try
            {
                BookingCodeService srv = new BookingCodeService();
                dlBookingStatus.DataSource = srv.GetBookingCodeStatus();
                dlBookingStatus.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindBooking(string type)
        {
            try
            {
                BookingCodeService srv = new BookingCodeService();
                dlBooking.DataSource = srv.GetBookingByType(type);
                dlBooking.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGridData(string bookingId)
        {
            try
            {
                BookingCodeService srv = new BookingCodeService();
                gvCust.DataSource = srv.GetBookingCodeByBookingId(bookingId);
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
                if (!Page.IsValid)
                {
                    upModalAdd.Update();
                    return;
                }

                string user = Session["User"].ToString();
                string bookingCodeId = lbBookingCodeId.Text;
                string bookingId = dlBooking.SelectedValue;
                string code = tbBookingCode.Text;

                string status = dlBookingStatus.SelectedValue;

                BookingCodeService srv = new BookingCodeService();
                if (lbType.Text == "Add")
                {
                    srv.AddBookingCode(bookingId, code, user, status);
                    BindGridData(dlBooking.SelectedValue);
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdateBookingCode(bookingId, code, user, bookingCodeId, status);
                    BindGridData(dlBooking.SelectedValue);
                    ClientPopup(ModalType.Success, "Completed");
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
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

                BookingCodeService srv = new BookingCodeService();
                srv.DeleteBookingCode(id, user);

                BindGridData(dlBooking.SelectedValue);
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

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdWarning').modal('hide');", true);
            upModalWarning.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdDelConfirm').modal('hide');", true);
            upModelDel.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdUpload", "$('#mdUpload').modal('hide');", true);
            upMdUpload.Update();

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
                lbType.Text = "Add";
                lbBookingId.Text = dlBooking.SelectedValue;
                lbBookingCodeId.Text = "";

                tbBookingName.Text = dlBooking.SelectedItem.Text;
                tbBookingCode.Text = "";
                dlBookingStatus.SelectedIndex = -1;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
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
                    lbDelNo.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvCust.DataKeys != null)
                    {
                        lbDelId.Text = gvCust.DataKeys[index].Values["ID"].ToString();

                        BookingCodeService srv = new BookingCodeService();
                        DataRow row = srv.GetBookingCodeById(lbDelId.Text, dlBooking.SelectedValue);
                        if (row["status_id"].ToString() == "2")
                        {
                            ClientPopup(ModalType.Warning, "Can't delete used code.");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal();", true);
                            upModelDel.Update();
                        }
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

            
            DataRowView dr = (DataRowView)e.Row.DataItem;
            if (dr != null && !String.IsNullOrEmpty(dr["status_id"].ToString()))
            {
                if (dr["status_id"].ToString() == "2")
                {
                    LinkButton delFull = e.Row.FindControl("gvBtnDelete") as LinkButton;

                    if (delFull != null)
                    {
                        delFull.Visible = false;
                    }
                }
            }

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
                lbBookingCodeId.Text = "";



                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();

                    lbType.Text = "Edit";
                    lbBookingCodeId.Text = id;
                    lbBookingId.Text = dlBooking.SelectedValue;

                    BookingCodeService srv = new BookingCodeService();
                    DataRow row = srv.GetBookingCodeById(lbBookingCodeId.Text, lbBookingId.Text);
                    if (row != null)
                    {
                        tbBookingCode.Text = row["code"].ToString();
                        tbBookingName.Text = dlBooking.SelectedItem.Text;
                        dlBookingStatus.SelectedValue = row["status_id"].ToString();
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
        protected void cvDupBooking_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                BookingCodeService srv = new BookingCodeService();
                DataRow oldRow = srv.GetBookingCodeById(lbBookingCodeId.Text, dlBooking.SelectedValue);
                if (oldRow != null)
                {
                    string oldCode = oldRow["CODE"].ToString();
                    string curCode = tbBookingCode.Text;

                    if (oldCode == curCode)
                    {
                        args.IsValid = true;
                    }
                    else
                    {
                        args.IsValid = !srv.IsDupplicateBookingCode(dlBooking.SelectedValue, curCode);
                    }
                }
                else
                {
                    args.IsValid = !srv.IsDupplicateBookingCode(dlBooking.SelectedValue, tbBookingCode.Text);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdUpload", "$('#mdUpload').modal();", true);
            upMdUpload.Update();
        }
        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (dlBooking.SelectedIndex <= 0)
                {
                    throw new Exception("Please select Booking!");
                }
                if (upload.HasFile)
                {
                    if (upload.PostedFile.ContentType == "text/plain")
                    {
                        string uploadFile = System.Web.Configuration.WebConfigurationManager.AppSettings["UploadPath"] + "\\" + upload.FileName;
                        if (!Directory.Exists(Path.GetDirectoryName(uploadFile)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(uploadFile));
                        }
                        upload.SaveAs(uploadFile);

                        if (File.Exists(uploadFile))
                        {
                            BookingCodeService srv = new BookingCodeService();
                            List<string> codes = new List<string>();
                            using (StreamReader reader = new StreamReader(uploadFile, System.Text.Encoding.Default))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    codes.Add(line);
                                }
                            }
                            if (codes.Count > 0)
                            {

                                //Check Duplicate
                                List<string> dupCodes = new List<string>();
                                foreach (var code in codes)
                                {
                                    if (srv.IsDupplicateBookingCode(dlBooking.SelectedValue, code))
                                    {
                                        dupCodes.Add(code);
                                    }
                                }
                                if (dupCodes.Count > 0)
                                {
                                    ClientPopup(ModalType.Warning, "Duplicate Code :" + string.Join(",", dupCodes));
                                }
                                else
                                {

                                    srv.UploadBookingCode(dlBooking.SelectedValue, codes, Session["User"].ToString());
                                    BindGridData(dlBooking.SelectedValue);
                                    ClientPopup(ModalType.Success, "Completed");
                                }
                            }
                            codes = null;
                        }
                    }
                    else
                    {
                        throw new Exception("Allow file type Text only!");
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void btnUploadFile2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dlBooking.SelectedIndex < 0)
                {
                    throw new Exception("Please select Privilete!");
                }
                if (upload.HasFile)
                {
                    if (upload.PostedFile.ContentType == "text/plain")
                    {
                        string uploadFile = System.Web.Configuration.WebConfigurationManager.AppSettings["UploadPath"] + "\\" + upload.FileName;
                        if (!Directory.Exists(Path.GetDirectoryName(uploadFile)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(uploadFile));
                        }
                        upload.SaveAs(uploadFile);

                        if (File.Exists(uploadFile))
                        {
                            List<string> redeemCode = new List<string>();
                            using (StreamReader reader = new StreamReader(uploadFile, System.Text.Encoding.Default))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    redeemCode.Add(line);
                                }
                            }
                            if (redeemCode.Count > 0)
                            {
                                BookingCodeService srv = new BookingCodeService();
                                srv.UploadBookingCode(dlBooking.SelectedValue, redeemCode, Session["User"].ToString());
                                BindGridData(dlBooking.SelectedValue);
                                ClientPopup(ModalType.Success, "Completed");
                            }
                            redeemCode = null;
                        }
                    }
                    else
                    {
                        throw new Exception("Allow file type Text only!");
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
    }
}