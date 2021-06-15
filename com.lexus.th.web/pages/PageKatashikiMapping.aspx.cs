using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.lexus.th.web.pages
{
    public partial class PageKatashikiMapping : System.Web.UI.Page
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
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();

                        //CarModelService srv = new CarModelService();
                        //dlModel.DataTextField = "MODEL";
                        //dlModel.DataValueField = "MODEL_ID";
                        //dlModel.DataSource = srv.GetModels("");
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
                KatashikiService srv = new KatashikiService();
                gvCust.DataSource = srv.GetKatashikiAndModel(txtSearch.Text);
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
                string katashiki_code = tbKatashikiCode.Text;
                string model_id = dlModel.SelectedValue;
                string user = Session["User"].ToString();

                KatashikiService srv = new KatashikiService();
                if (lbType.Text == "Add")
                {
                    srv.AddKatashikiModel(katashiki_code, model_id, user);
                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdateKatashikiModel(katashiki_code, model_id, user, lbId.Text);
                    BindGridData();
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

                KatashikiService srv = new KatashikiService();
                srv.DeleteKatashikiModel(id, user);

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

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdWarning').modal('hide');", true);
            upModalWarning.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdDelConfirm').modal('hide');", true);
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
                KatashikiService srv = new KatashikiService();
                dlModel.Items.Clear();
                dlModel.DataSource = srv.GetCarModels();
                dlModel.DataBind();

                tbKatashikiCode.Text = "";
                dlModel.SelectedIndex = -1;
                lbType.Text = "Add";

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

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvCust.DataKeys != null)
                    {
                        lbDelId.Text = gvCust.DataKeys[index].Values["id"].ToString();
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

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["id"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    KatashikiService srv = new KatashikiService();
                    dlModel.Items.Clear();
                    dlModel.DataSource = srv.GetCarModels();
                    dlModel.DataBind();

                    DataRow row = srv.GetKatashikiById(id);
                    if (row != null)
                    {
                        tbKatashikiCode.Text = row["katashiki_code"].ToString();
                        dlModel.SelectedItem.Text = row["MODEL"].ToString();

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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdUpload", "$('#mdUpload').modal();", true);
            upMdUpload.Update();
        }
        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (upload.HasFile)
                {
                    if (upload.PostedFile.ContentType == "text/plain")
                    {
                        string uploadFile = WebConfigurationManager.AppSettings["KatashikiPath"] + "\\" + upload.FileName;
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
                            //if (redeemCode.Count > 0)
                            //{
                            //    PrivilegeCodeService srv = new PrivilegeCodeService();
                            //    srv.UploadPrivilegeCode(dlPrivilege.SelectedValue, redeemCode, Session["User"].ToString());
                            //    BindGridData(dlPrivilege.SelectedValue);
                            //    ClientPopup(ModalType.Success, "Completed");
                            //}
                            //redeemCode = null;

                            //InsertKatashikiCode()
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

        //protected void btnDownload_ServerClick(object sender, EventArgs e)
        //{
        //    FileInfo file = new FileInfo(WebConfigurationManager.AppSettings["TemplatePath"] + "\\Katashiki_Template.xls");
        //    byte[] fileConent = File.ReadAllBytes(WebConfigurationManager.AppSettings["TemplatePath"] + "\\Katashiki_Template.xls");
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
        //    HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
        //    HttpContext.Current.Response.ContentType = "application/octet-stream";
        //    HttpContext.Current.Response.BinaryWrite(fileConent);
        //    HttpContext.Current.Response.End();
        //}

        protected void btnDownload_ServerClick(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(WebConfigurationManager.AppSettings["TemplatePath"] + "\\Katashiki_Template.xls");
            byte[] fileConent = File.ReadAllBytes(WebConfigurationManager.AppSettings["TemplatePath"] + "\\Katashiki_Template.xls");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(fileConent);
            HttpContext.Current.Response.End();
        }
    }
}