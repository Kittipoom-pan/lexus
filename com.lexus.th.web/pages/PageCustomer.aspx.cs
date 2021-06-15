using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace com.lexus.th.web.master
{
    public partial class PageCustomer : System.Web.UI.Page
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
                    else
                    {
                        //ViewState["User"] = "ADMIN";
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();
                        gvCar.DataSource = new System.Data.DataTable();
                        gvCar.DataBind();
                        gvAppUser.DataSource = new System.Data.DataTable();
                        gvAppUser.DataBind();

                        CustomerService srv = new CustomerService();
                        using (DataTable model = srv.GetModel())
                        {
                            dlModelInit.DataSource = model;
                            dlModelInit.DataBind();

                            dlModel.DataSource = model;
                            dlModel.DataBind();
                        }
                        using (DataTable dealer = srv.GetDealer())
                        {
                            dlDealerInit.DataSource = dealer;
                            dlDealerInit.DataBind();

                            dlDealer.DataSource = dealer;
                            dlDealer.DataBind();
                        }
                        using (DataTable branch = srv.GetBranch(dlDealerInit.SelectedValue))
                        {
                            dlBranchInit.DataSource = branch;
                            dlBranchInit.DataBind();
                        }
                        using (DataTable branch = srv.GetBranch(dlDealer.SelectedValue))
                        {
                            dlBranch.DataSource = branch;
                            dlBranch.DataBind();
                        }
                        using (DataTable province = srv.GetProvinces())
                        {
                            dlProvince.DataSource = province;
                            dlProvince.DataBind();

                            dlProvinceApp.DataSource = province;
                            dlProvinceApp.DataBind();
                        }
                        

                        BindControlCarMemberId();
                        BindControlCarReason();

                        //BindSellerID();

                        tbSearchFrom.Enabled = chkSearchCreateDate.Checked;
                        tbSearchTo.Enabled = chkSearchCreateDate.Checked;
                        //dlReason.Enabled = chkInActive.Checked;
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
                tbSearchFrom.Enabled = chkSearchCreateDate.Checked;
                tbSearchTo.Enabled = chkSearchCreateDate.Checked;
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
                CustomerService srv = new CustomerService();

                #region Old
                //using (DataTable dt = srv.GetCustomerSearch(txtSearch.Text,
                //    chkSearchMemberId.Checked,
                //    chkSearchFname.Checked,
                //    chkSearchMobile.Checked,
                //    chkSearchVin.Checked,
                //    chkSearchCreateDate.Checked,
                //    tbSearchFrom.Text,
                //    tbSearchTo.Text))
                //{
                //    gvCust.DataSource = dt;
                //    gvCust.DataBind();
                //    BindGridDataCar(dt.AsEnumerable().Select(p => p["MEMBERID"].ToString()).Distinct().ToList());
                //}
                #endregion

                DataTable dt2 = srv.GetAppSearch(txtSearch.Text,
                        chkSearchMemberId.Checked,
                        chkSearchFname.Checked,
                        chkSearchMobile.Checked,
                        chkSearchVin.Checked,
                        chkSearchCreateDate.Checked,
                        tbSearchFrom.Text,
                        tbSearchTo.Text,
                        Session["Role"].ToString());
                gvAppUser.DataSource = dt2;
                gvAppUser.DataBind();

                using (DataTable dt = srv.GetCustomerSearch(txtSearch.Text,
                    chkSearchMemberId.Checked,
                    chkSearchFname.Checked,
                    chkSearchMobile.Checked,
                    chkSearchVin.Checked,
                    chkSearchCreateDate.Checked,
                    tbSearchFrom.Text,
                    tbSearchTo.Text,
                    Session["Role"].ToString()))
                {
                    gvCust.DataSource = dt;
                    gvCust.DataBind();
                    BindGridDataCar(dt2.AsEnumerable().Select(p => p["MEMBERID"].ToString()).Distinct().ToList(), null);
                }

                //if (Session["Role"].ToString() == "3" || Session["Role"].ToString() == "4")
                //{
                //    DataTable seller = null;

                //    if (Session["Role"].ToString() == "3")
                //    {
                //        seller = srv.GetSeller(Session["Dealer"].ToString());
                //    }
                //    else if (Session["Role"].ToString() == "4")
                //    {
                //        seller = srv.GetSeller(null);
                //        var row = seller.AsEnumerable().Where(p => p["SELLERID"].ToString() == Session["Seller"].ToString());
                //        if (row.Any())
                //        {
                //            seller = row.CopyToDataTable();
                //        }
                //        else
                //        {
                //            seller.Rows.Clear();
                //        }
                //        row = null;
                //    }

                //    DataTable dt = srv.GetCustomerSearch(txtSearch.Text,
                //        chkSearchMemberId.Checked,
                //        chkSearchFname.Checked,
                //        chkSearchMobile.Checked,
                //        chkSearchVin.Checked,
                //        chkSearchCreateDate.Checked,
                //        tbSearchFrom.Text,
                //        tbSearchTo.Text,
                //        Session["Role"].ToString());
                //    gvCust.DataSource = dt;
                //    gvCust.DataBind();

                //    DataTable dt2 = srv.GetAppSearch(txtSearch.Text,
                //        chkSearchMemberId.Checked,
                //        chkSearchFname.Checked,
                //        chkSearchMobile.Checked,
                //        chkSearchVin.Checked,
                //        chkSearchCreateDate.Checked,
                //        tbSearchFrom.Text,
                //        tbSearchTo.Text,
                //        Session["Role"].ToString());
                //    gvAppUser.DataSource = dt2;
                //    gvAppUser.DataBind();


                //    bool isExistCustomer = false;
                //    FillterExistingCustomer(ref dt, ref isExistCustomer, seller);

                //    BindGridDataCar(dt.AsEnumerable().Select(p => p["MEMBERID"].ToString()).Distinct().ToList(), seller);

                //    if (isExistCustomer)
                //    {
                //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdExistCust", "$('#mdExistCust').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                //        upMdExistCust.Update();
                //    }

                //    dt = null;
                //    seller = null;
                //}
                //else
                //{
                //    DataTable dt2 = srv.GetAppSearch(txtSearch.Text,
                //        chkSearchMemberId.Checked,
                //        chkSearchFname.Checked,
                //        chkSearchMobile.Checked,
                //        chkSearchVin.Checked,
                //        chkSearchCreateDate.Checked,
                //        tbSearchFrom.Text,
                //        tbSearchTo.Text,
                //        Session["Role"].ToString());
                //    gvAppUser.DataSource = dt2;
                //    gvAppUser.DataBind();

                //    using (DataTable dt = srv.GetCustomerSearch(txtSearch.Text,
                //        chkSearchMemberId.Checked,
                //        chkSearchFname.Checked,
                //        chkSearchMobile.Checked,
                //        chkSearchVin.Checked,
                //        chkSearchCreateDate.Checked,
                //        tbSearchFrom.Text,
                //        tbSearchTo.Text,
                //        Session["Role"].ToString()))
                //    {
                //        gvCust.DataSource = dt;
                //        gvCust.DataBind();
                //        BindGridDataCar(dt2.AsEnumerable().Select(p => p["MEMBERID"].ToString()).Distinct().ToList(), null);
                //    }


                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGridDataCar(List<string> memberIdList, DataTable seller)
        {
            try
            {
                CustomerService srv = new CustomerService();
                if (seller == null)
                {
                    gvCar.DataSource = srv.GetCarsByMemberList(memberIdList);
                    gvCar.DataBind();
                }
                else
                {
                    DataTable dt = srv.GetCarsByMemberList(memberIdList);
                    var row = dt.AsEnumerable().Where(p2 =>
                        seller.AsEnumerable().Where(p => !string.IsNullOrEmpty(p["SELLERID"].ToString())).Select(p => p["SELLERID"].ToString()).Contains(p2["SELLERID"].ToString()));

                    if (row.Any())
                    {
                        dt = row.CopyToDataTable();
                    }
                    else
                    {
                        dt.Rows.Clear();
                    }

                    gvCar.DataSource = dt;
                    gvCar.DataBind();

                    dt = null;
                    row = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void BindGridDataCar2(string memberId)
        //{
        //    try
        //    {
        //        CustomerService srv = new CustomerService();
        //        gvCar.DataSource = srv.GetCarsByMemberList(memberId);
        //        gvCar.DataBind();
                //CustomerService srv = new CustomerService();
                //if (seller == null)
                //{
                    
                //}
                //else
                //{
                //    DataTable dt = srv.GetCarsByMemberList(memberIdList);
                //    var row = dt.AsEnumerable().Where(p2 =>
                //        seller.AsEnumerable().Where(p => !string.IsNullOrEmpty(p["SELLERID"].ToString())).Select(p => p["SELLERID"].ToString()).Contains(p2["SELLERID"].ToString()));

                //    if (row.Any())
                //    {
                //        dt = row.CopyToDataTable();
                //    }
                //    else
                //    {
                //        dt.Rows.Clear();
                //    }

                //    gvCar.DataSource = dt;
                //    gvCar.DataBind();

                //    dt = null;
                //    row = null;
                //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void BindGridDataAfterAddCustomer(string id)
        {
            try
            {
                CustomerService srv = new CustomerService();

                using (DataTable dt = srv.GetCustomerSearchById(id))
                {
                    gvCust.DataSource = dt;
                    gvCust.DataBind();

                    if (Session["Role"].ToString() == "3" || Session["Role"].ToString() == "4")
                    {
                        DataTable seller = null;

                        if (Session["Role"].ToString() == "3")
                        {
                            seller = srv.GetSeller(Session["Dealer"].ToString());
                        }
                        else if (Session["Role"].ToString() == "4")
                        {
                            seller = srv.GetSeller(null);
                            var row = seller.AsEnumerable().Where(p => p["SELLERID"].ToString() == Session["Seller"].ToString());
                            if (row.Any())
                            {
                                seller = row.CopyToDataTable();
                            }
                            else
                            {
                                seller.Rows.Clear();
                            }
                            row = null;
                        }

                        BindGridDataCar(dt.AsEnumerable().Select(p => p["MEMBERID"].ToString()).Distinct().ToList(), seller);
                        seller = null;
                    }
                    else
                    {
                        BindGridDataCar(dt.AsEnumerable().Select(p => p["MEMBERID"].ToString()).Distinct().ToList(), null);
                    }
                }

                //txtSearch.Text = id;
                //chkSearchMemberId.Checked = true;
                //chkSearchCreateDate.Checked = false;
                //chkSearchFname.Checked = false;
                //chkSearchMobile.Checked = false;
                //chkSearchVin.Checked = false;
                //BindGridData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindControlCarMemberId()
        {
            try
            {
                //CustomerService srv = new CustomerService();
                //dlMemberId.DataSource = srv.GetMembers();
                //dlMemberId.DataBind();
                //upModalCarAdd.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //private void BindControlCarColor(string modelId)
        //{
        //    try
        //    {
        //        CustomerService srv = new CustomerService();
        //        dlColor.DataSource = srv.GetCarColor(modelId);
        //        dlColor.DataBind();
        //        upModalCarAdd.Update();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private void BindControlCarReason()
        {
            try
            {
                //CustomerService srv = new CustomerService();
                //dlReason.DataSource = srv.GetCarReasons();
                //dlReason.DataBind();
                //upModalCarAdd.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //private void BindSellerID()
        //{
        //    try
        //    {
        //        string dealer = "";

        //        if (Session["Role"].ToString() == "3")
        //        {
        //            dealer = Session["Dealer"].ToString();
        //        }

        //        CustomerService srv = new CustomerService();
        //        using (DataTable seller = srv.GetSeller(dealer)) // If dealer get seller from dealer else get all seller
        //        {
        //            dlSellerID.DataSource = seller;
        //            dlSellerID.DataBind();

        //            //dlSellerIDCar.DataSource = seller;
        //            //dlSellerIDCar.DataBind();
        //        }

        //        if (Session["Role"].ToString() == "4")
        //        {
        //            dlSellerID.Enabled = false;
        //            //dlSellerIDCar.Enabled = false;

        //            DisplayDropdownByValue(dlSellerID, Session["Seller"].ToString());
        //            //DisplayDropdownByValue(dlSellerIDCar, Session["Seller"].ToString());
        //        }
        //        else
        //        {
        //            dlSellerID.Enabled = true;
        //            //dlSellerIDCar.Enabled = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private void FillterExistingCustomer(ref DataTable searchTable, ref bool isExistCustomer, DataTable seller)
        {
            try
            {
                int rowBefore = searchTable.Rows.Count;
                int rowAfter = 0;

                var row = searchTable.AsEnumerable().Where(p2 =>
                    seller.AsEnumerable().Where(p => !string.IsNullOrEmpty(p["SELLERID"].ToString())).Select(p => p["SELLERID"].ToString()).Contains(p2["SELLERID"].ToString()));

                if (row.Any())
                {
                    searchTable = row.CopyToDataTable();
                }
                else
                {
                    searchTable.Rows.Clear();
                }
                
                rowAfter = searchTable.Rows.Count;
                row = null;

                if (rowBefore > rowAfter)
                {
                    if (chkSearchFname.Checked || chkSearchCreateDate.Checked)
                    {
                        isExistCustomer = false;
                    }
                    else
                    {
                        isExistCustomer = true;
                    }
                }
                else
                {
                    isExistCustomer = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

        protected void cvVin_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (tbVin.Text.Length > 0)
                {
                    InitialService srv = new InitialService();
                    if (lbType.Text == "Add")
                    {
                        args.IsValid = !srv.IsExistsVin(tbVin.Text);
                    }
                    //else
                    //{
                    //    if (srv.GetLastMobile(lbId.Text) != tbVin.Text)
                    //    {
                    //        args.IsValid = !srv.IsExistsMobile(tbVin.Text);
                    //    }
                    //    else
                    //    {
                    //        args.IsValid = true;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime? BirthDate = null;
            DateTime? BirthDateApp = null;

            string message = "f";
            try
            {
                if (!Page.IsValid)
                {
                    upModalAdd.Update();
                    return;
                }

                #region car owner
                string firstName = tbFirstname.Text;
                string lastName = tbLastname.Text;
                string nickName = "";
                string gender = dlGener.SelectedValue;
                string age = "";
                string email = tbEmail.Text;
                string ssn = tbSSN.Text;
                string mobile = tbMobile.Text;
                string id = lbId.Text;
                string memberId = tbMemberId.Text;
                string title = dlTitle.SelectedValue;
                string birthdate = tbBirthdate.Text;
                string addr1 = tbAddr1.Text;
                string addr2 = "";
                string subDistrict = dlSubDistict.SelectedItem.ToString().Trim();
                string district = dlDistict.SelectedItem.ToString().Trim();
                string province = dlProvince.SelectedItem.ToString().Trim();
                string postcode = tbPostCode.Text;
                string homeNumber = "";
                string sellerId = "";
                #endregion

                #region app user
                string firstNameApp = tbFirstnameApp.Text;
                string lastNameApp = tbLastnameApp.Text;
                string nickNameApp = "";
                string genderApp = dlGenerApp.SelectedValue;
                string ageApp = "";
                string emailApp = tbEmailApp.Text;
                string ssnApp = tbSSNApp.Text;
                string mobileApp = tbMobileApp.Text;
                string idApp = lbIdApp.Text;
                string memberIdApp = tbMemberId.Text;
                string titleApp = dlTitleApp.SelectedValue;
                string birthdateApp = tbBirthdateApp.Text;
                string addr1App = tbAddr1App.Text;
                string addr2App = "";
                string subDistrictApp = dlSubDistictApp.SelectedItem.ToString().Trim();
                string districtApp = dlDistictApp.SelectedItem.ToString().Trim();
                string provinceApp = dlProvinceApp.SelectedItem.ToString().Trim();
                string postcodeApp = tbPostCodeApp.Text;
                string homeNumberApp = "";
                string sellerIdApp = "";
                #endregion
            
                if (!string.IsNullOrEmpty(birthdate))
                {
                    BirthDate = DateTime.ParseExact(birthdate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(birthdate);
                }
                if (!string.IsNullOrEmpty(birthdateApp))
                {
                    BirthDateApp = DateTime.ParseExact(birthdateApp, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(birthdateApp);
                }
                DateTime LowDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime("01/01/1900");
                DateTime DateNow = DateTime.Now;

                CustomerService srv = new CustomerService();
                if (lbType.Text == "Add")
                {
                    if (BirthDate > DateNow)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
                        ClientPopup(ModalType.Warning, "Please Choose Birthday Date Car Owner < Today");
                    }
                    else if (BirthDate < LowDate)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
                        ClientPopup(ModalType.Warning, "Please Choose Birthday Date Car Owner > 01/01/1900");
                    }
                    else
                    {

                        #region car owner
                        string idInsert = srv.AddCustomer(firstName, lastName, nickName, gender, age, email, ssn, mobile, memberId, Session["User"].ToString(), title, birthdate,
                        addr1, addr2, subDistrict, district, province, postcode, homeNumber, sellerId);
                        //BindGridDataAfterAddCustomer(idInsert);
                        #endregion
                    }

                    if (BirthDateApp > DateNow)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
                        ClientPopup(ModalType.Warning, "Please Choose Birthday Date App User < Today");
                    }
                    else if (BirthDateApp < LowDate)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
                        ClientPopup(ModalType.Warning, "Please Choose Birthday Date App User > 01/01/1900");
                    }
                    else
                    {

                        #region app user
                        string idInsert2 = srv.AddCustomerApp(firstNameApp, lastNameApp, nickNameApp, genderApp, ageApp, emailApp, ssnApp, mobileApp, memberIdApp, Session["User"].ToString(), titleApp, birthdateApp,
                        addr1App, addr2App, subDistrictApp, districtApp, provinceApp, postcodeApp, homeNumberApp, sellerIdApp);
                        //BindGridDataAfterAddCustomer(idInsert2);
                        #endregion


                        if (lbCarType.Text == "Insert")
                        {
                            string dealer = dlDealer.SelectedValue;
                            string modelId = dlModel.SelectedValue;
                            string plateNo = tbPlateNo.Text;
                            string vin = tbVin.Text;
                            string colorId = "";
                            string user = Session["User"].ToString();
                            string cusType = "";
                            string ownerShip = "";
                            string rsDate = tbDeliveryDate.Text;

                            srv.AddCar(memberId, dealer, modelId, plateNo, vin, user, colorId, cusType, ownerShip, rsDate, sellerId);
                            InitialService srs = new InitialService();
                            srs.AddInitial(titleApp, firstNameApp, lastNameApp, genderApp, birthdateApp, ssnApp, emailApp, dealer, vin, plateNo, modelId, rsDate, Session["User"].ToString());
                        }
                        else
                        {
                            string dealer = dlDealerInit.SelectedValue;
                            string modelId = dlModelInit.SelectedValue;
                            string plateNo = tbPlateNoInit.Text;
                            string vin = tbVinInit.Text;
                            string colorId = "";
                            string user = Session["User"].ToString();
                            string cusType = "";
                            string ownerShip = "";
                            string rsDate = tbRSDateInit.Text;

                            srv.AddCar(memberId, dealer, modelId, plateNo, vin, user, colorId, cusType, ownerShip, rsDate, sellerId);

                        }
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
                        ClientPopup(ModalType.Success, "Completed");
                    }
                    BindGridData();
                    
                }
                else if (lbType.Text == "Edit")
                {
                    if (BirthDate > DateNow)
                    {
                        ClientPopup(ModalType.Warning, "Please Choose Birthday Date Car Owner < Today");
                    }
                    else if (BirthDate < LowDate)
                    {
                        ClientPopup(ModalType.Warning, "Please Choose Birthday Date Car Owner > 01/01/1900");
                    }
                    else
                    {
                        #region car owner
                        srv.UpdateCustomer(firstName, lastName, nickName, gender, age, email, ssn, mobile, id, memberId, Session["User"].ToString(), title, birthdate,
                        addr1, addr2, subDistrict, district, province, postcode, homeNumber, tbRemarkCust.Text, sellerId);
                        #endregion
                    }

                    if (BirthDateApp > DateNow)
                    {
                        ClientPopup(ModalType.Warning, "Please Choose Birthday Date App User < Today");
                    }
                    else if (BirthDateApp < LowDate)
                    {
                        ClientPopup(ModalType.Warning, "Please Choose Birthday Date App User > 01/01/1900");
                    }
                    else
                    {
                        #region app user
                        srv.UpdateCustomerApp(firstNameApp, lastNameApp, nickNameApp, genderApp, ageApp, emailApp, ssnApp, mobileApp, idApp, memberIdApp, Session["User"].ToString(), titleApp, birthdateApp,
                        addr1App, addr2App, subDistrictApp, districtApp, provinceApp, postcodeApp, homeNumberApp, tbRemarkCust.Text, sellerIdApp);
                        #endregion
                    }

                    BindGridData();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
                    ClientPopup(ModalType.Success, "Completed");
                }

                BindControlCarMemberId();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, message + ex.Message);
                ClientPopup(ModalType.Error, "Save have error. Please try again later.");
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbDelId.Text != "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdDelConfirm').modal('hide');", true);
                    upModelDel.Update();

                    string id = lbDelId.Text;
                    string user = Session["User"].ToString();

                    CustomerService srv = new CustomerService();
                    srv.DeleteCustomer(id, tbReasonDelCust.Text, user);

                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
                else
                {
                    BindGridData();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void ClientPopup(ModalType type, string message)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
            //upModalAdd.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddCar", "$('#mdAddCar').modal('hide');", true);
            upModalAddCar.Update();

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdCheckInitial", "$('#mdCheckInitial').modal('hide');", true);
            //upModalCheckInitial.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDataInitial", "$('#mdDataInitial').modal('hide');", true);
            upModalDataInitial.Update();

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdWarning", "$('#mdWarning').modal('hide');", true);
            //upModalWarning.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal('hide');", true);
            upModelDel.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmApp", "$('#mdDelConfirmApp').modal('hide');", true);
            upModelDelApp.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdChgLogCust", "$('#mdChgLogCust').modal('hide');", true);
            upModelDel.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdChgLogCar", "$('#mdChgLogCar').modal('hide');", true);
            upModelDel.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdExistCust", "$('#mdExistCust').modal('hide');", true);
            upMdExistCust.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdExtendConfirm", "$('#mdExtendConfirm').modal('hide');", true);
            upMdExtendConfirm.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdjust", "$('#mdAdjust').modal('hide');", true);
            upMdAdjust.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdPrivilege", "$('#mdPrivilege').modal('hide');", true);
            upMdPrivilege.Update();

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

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdSucess", "$('#mdSuccess').modal('hide');", true);
                upModalSuccess.Update();
            }
            if (type == ModalType.Warning)
            {
                lbWarning.Text = message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdWarning", "$('#mdWarning').modal();", true);
                upModalWarning.Update();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdSucess", "$('#mdSuccess').modal('hide');", true);
                upModalSuccess.Update();
            }
        }

        protected void btnAddModal_Click(object sender, EventArgs e)
        {
            try
            {
                //tbMemberId.Text = "";
                //tbMemberId.Enabled = true;
                //tbFirstname.Text = "";
                //tbLastname.Text = "";
                //tbEmail.Text = "";
                //tbMobile.Text = "";
                //tbSSN.Text = "";
                //dlGener.SelectedIndex = -1;
                //lbType.Text = "Add";
                //dlTitle.SelectedIndex = -1;
                //tbBirthdate.Text = "";
                //tbAddr1.Text = "";
                //dlSubDistict.SelectedIndex = -1;
                //dlDistict.SelectedIndex = -1;
                //dlProvince.SelectedIndex = -1;
                //tbPostCode.Text = "";

                //if (Session["Role"].ToString() != "4")
                //{
                //    dlSellerID.SelectedIndex = -1;
                //}
                tbVin.Text = "";
                tbPlateNo.Text = "";
                tbDeliveryDate.Text = "";
                dlDealer.SelectedIndex = -1;
                dlBranch.SelectedIndex = -1;
                dlModel.SelectedIndex = -1;

                lbClickType.Text = "Customer";
                lbCarType.Text = "Insert";
                tbSearchInitial.Text = "";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdCheckInitial", "$('#mdCheckInitial').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                upModalCheckInitial.Update();
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
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                        upModelDel.Update();
                    }
                }
                //if (e.CommandName == "ViewCar")
                //{
                //    //tbCarMemberId.Text = "";

                //    int index = Convert.ToInt32(e.CommandArgument);
                //    if (gvCust.DataKeys != null)
                //    {
                //        //tbCarMemberId.Text = gvCust.DataKeys[index].Values["MEMBERID"].ToString();
                //        //BindGridDataCar(tbCarMemberId.Text);
                //    }
                //}
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                lbDelId.Text = "";
                tbReasonDelCust.Text = "";

                if (gvCust.DataKeys != null)
                {
                    lbDelId.Text = gvCust.DataKeys[index].Values["ID"].ToString();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                    upModelDel.Update();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvCust_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //LinkButton lnkFull = e.Row.FindControl("gvBtnEdit") as LinkButton;
                //if (lnkFull != null)
                //{
                //    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
                //}

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    string delFlag = Convert.ToString(drv["DEL_FLAG"]);

                    //LinkButton _btnEditCust = (LinkButton)e.Row.FindControl("gvBtnEdit");
                    //LinkButton _btnDelete = (LinkButton)e.Row.FindControl("gvBtnDelete");
                    //LinkButton _btnChgLog = (LinkButton)e.Row.FindControl("gvBtnChgLog");

                    if (delFlag == "Y")
                    {
                        
                        //_btnEditCust.Visible = false;
                        //_btnDelete.Visible = false;
                        //_btnChgLog.Visible = true;

                        foreach (TableCell cell in e.Row.Cells)
                        {
                            cell.Font.Italic = true;
                            cell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#888");
                        }
                    }

                    //if (Session["Role"].ToString() == "4")
                    //{
                    //    _btnEditCust.Visible = false;
                    //    _btnDelete.Visible = false;
                    //}

                   
                    
                }
            }
            catch
            {
            }
        }
        protected void gvBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerService srv = new CustomerService();
                using (DataTable district = srv.GetDistrict(dlProvince.SelectedValue))
                {
                    dlDistict.DataSource = district;
                    dlDistict.DataBind();
                }
                using (DataTable district = srv.GetDistrict(dlProvinceApp.SelectedValue))
                {
                    dlDistictApp.DataSource = district;
                    dlDistictApp.DataBind();
                }
                using (DataTable subdistrict = srv.GetSubDistrict(dlProvince.SelectedValue, dlDistict.SelectedValue))
                {
                    dlSubDistict.DataSource = subdistrict;
                    dlSubDistict.DataBind();
                }
                using (DataTable subdistrict = srv.GetSubDistrict(dlProvinceApp.SelectedValue, dlDistictApp.SelectedValue))
                {
                    dlSubDistictApp.DataSource = subdistrict;
                    dlSubDistictApp.DataBind();
                }

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                lbId.Text = "";

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    DataRow row = srv.GetCustomerById(id);
                    if (row != null)
                    {

                        tbFirstname.Text = row["FNAME"].ToString();
                        tbLastname.Text = row["LNAME"].ToString();
                        tbEmail.Text = row["EMAIL"].ToString();
                        tbMobile.Text = row["MOBILE"].ToString();
                        tbSSN.Text = row["SSN"].ToString();
                        dlGener.SelectedValue = row["GENDER"].ToString();
                        dlTitle.SelectedValue = row["TITLENAME"].ToString();
                        tbBirthdate.Text = (row["BIRTHDATE"] == DBNull.Value) ? "" : Convert.ToDateTime(row["BIRTHDATE"]).ToString("dd/MM/yyyy");
                        tbAddr1.Text = row["ADDRESS1"].ToString();
                        DisplayDropdownByText(dlSubDistict, row["SUBDISTRICT"].ToString());
                        DisplayDropdownByText(dlDistict, row["DISTRICT"].ToString());
                        DisplayDropdownByText(dlProvince, row["PROVINCE"].ToString());
                        tbPostCode.Text = srv.GetPostcode(dlSubDistict.SelectedValue); ;

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                        upModalAdd.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void gvCar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    lbDelCarId.Text = "";
                    
                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvCust.DataKeys != null)
                    {
                        lbDelCarId.Text = gvCar.DataKeys[index].Values["CUS_ID"].ToString();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelCarConfirm", "$('#mdDelCarConfirm').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                        upDelCarConfirm.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnCarModal_Click(object sender, EventArgs e)
        {
            try
            {
                pnChangeLog.Visible = false;
                lbCarId.Text = "";
                lbCarType.Text = "Add";

                tbVin.Text = "";
                tbPlateNo.Text = "";
                dlDealer.SelectedIndex = -1;
                dlBranch.SelectedIndex = -1;
                dlModel.SelectedIndex = -1;
                tbDeliveryDate.Text = "";
                tbRemarkCar.Text = "";
                tbRemarkCar.Visible = false;
                lbRemarkCar.Visible = false;

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddCar", "$('#mdAddCar').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                //upModalAddCar.Update();

                //if (!string.IsNullOrEmpty(tbCarMemberId.Text))
                //{
                //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddCar", "$('#mdAddCar').modal();", true);
                //    upModalAdd.Update();
                //}
                //else
                //{
                //    ClientPopup(ModalType.Error, "No customer selected");
                //}
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnCarSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    upModalAddCar.Update();
                    return;
                }

                string memberId = lbMemberId.Text;

                string dealer = dlDealer.SelectedValue;
                string modelId = dlModel.SelectedValue;
                string plateNo = tbPlateNo.Text;
                string vin = tbVin.Text;
                string colorId = "";
                string user = Session["User"].ToString();
                //bool isInactive = chkInActive.Checked;
                //string inactiveReason = dlReason.SelectedValue;
                string cusType = "";
                string ownerShip = "";
                string rsDate = tbDeliveryDate.Text;
                string sellerId = "";

                //InitialService inv = new InitialService();
                 
                //if (inv.IsExistsVin(tbVin.Text))
                //{
                //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
                //    ClientPopup(ModalType.Warning, "Already VIN in Initial data");
                //    return;
                //}
                
                CustomerService srv = new CustomerService();
                if (lbCarType.Text == "Add")
                {
                    DataRow row = srv.CheckVinInitial(vin);
                    if (row != null)
                    {
                        ClientPopup(ModalType.Warning, "Found VIN in Initial");
                    }
                    else
                    {
                        DataRow row2 = srv.CheckVinInCar(tbVin.Text);
                        if (row2 != null)
                        {
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddCar", "$('#mdAddCar').modal();", true);
                            //upModalCarAdd.Update();
                            ClientPopup(ModalType.Warning, "Found VIN in System");
                        }
                        else
                        {
                            string titleApp = string.Empty;
                            string firstNameApp = string.Empty;
                            string lastNameApp = string.Empty;
                            string genderApp = string.Empty;
                            string birthdateApp = string.Empty;
                            string ssnApp = string.Empty;
                            string emailApp = string.Empty;

                            DataRow rowApp = srv.GetAppUserByMemberID(lbMemberId.Text);
                            if (rowApp != null)
                            {
                                titleApp = rowApp["TITLENAME"].ToString();
                                firstNameApp = rowApp["FNAME"].ToString();
                                lastNameApp = rowApp["LNAME"].ToString();
                                genderApp = rowApp["GENDER"].ToString();
                                birthdateApp = (rowApp["BIRTHDATE"] == DBNull.Value) ? "" : Convert.ToDateTime(rowApp["BIRTHDATE"]).ToString("dd/MM/yyyy");
                                ssnApp = rowApp["SSN"].ToString();
                                emailApp = rowApp["EMAIL"].ToString();
                            }
                            srv.AddCar(memberId, dealer, modelId, plateNo, vin, user, colorId, cusType, ownerShip, rsDate, sellerId);

                            InitialService srs = new InitialService();
                            srs.AddInitial(titleApp, firstNameApp, lastNameApp, genderApp, birthdateApp, ssnApp, emailApp, dealer, vin, plateNo, modelId, rsDate, Session["User"].ToString());

                            ClientPopup(ModalType.Success, "Completed");
                        }
                    }
                }
                else if (lbCarType.Text == "Edit")
                {
                    srv.EditCar(memberId, dealer, modelId, plateNo, vin, user, colorId, lbCarId.Text, tbRemarkCar.Text, cusType, ownerShip, rsDate, sellerId);
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbCarType.Text == "Insert")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddCar", "$('#mdAddCar').modal('hide');", true);
                    divRemarkCust.Visible = false;
                    tbRemarkCust.Text = "";
                    tbRemarkCust.Visible = false;
                    lbRemarkCust.Visible = false;

                    tbMemberId.Text = "";
                    tbMemberId.Enabled = true;
                    tbFirstname.Text = "";
                    tbLastname.Text = "";
                    tbEmail.Text = "";
                    tbMobile.Text = "";
                    tbSSN.Text = "";
                    dlGener.SelectedIndex = -1;
                    lbType.Text = "Add";
                    dlTitle.SelectedIndex = -1;
                    tbBirthdate.Text = "";
                    tbAddr1.Text = "";
                    dlSubDistict.SelectedIndex = -1;
                    dlDistict.SelectedIndex = -1;
                    dlProvince.SelectedIndex = -1;
                    tbPostCode.Text = "";

                    tbFirstnameApp.Text = "";
                    tbLastnameApp.Text = "";
                    tbEmailApp.Text = "";
                    tbMobileApp.Text = "";
                    tbSSNApp.Text = "";
                    dlGenerApp.SelectedIndex = -1;
                    dlTitleApp.SelectedIndex = -1;
                    tbBirthdateApp.Text = "";
                    tbAddr1App.Text = "";
                    dlSubDistictApp.SelectedIndex = -1;
                    dlDistictApp.SelectedIndex = -1;
                    dlProvinceApp.SelectedIndex = -1;
                    tbPostCodeApp.Text = "";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                    upModalAdd.Update();
                }

                BindGridData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void dlModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (dlModel.SelectedIndex > -1)
                {
                    //BindControlCarColor(dlModel.SelectedValue);
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
            foreach (GridViewRow row in gvCust.Rows)
            {
                LinkButton lnkFull = row.FindControl("gvBtnAddCar") as LinkButton;
                if (lnkFull != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
                }
            }
            foreach (GridViewRow row in gvCar.Rows)
            {
                LinkButton lnkFull = row.FindControl("gvBtnEditCar") as LinkButton;
                if (lnkFull != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
                }
            }
            foreach (GridViewRow row in gvAppUser.Rows)
            {
                LinkButton lnkFull = row.FindControl("gvBtnEditApp") as LinkButton;
                if (lnkFull != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
                }
            }
        }
        protected void cvMemberId_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (tbMemberId.Text.Length > 0)
                {
                    CustomerService srv = new CustomerService();
                    if (lbType.Text == "Add")
                    {
                        args.IsValid = !srv.IsExistsMemberId(tbMemberId.Text);
                    }
                    else
                    {
                        if (srv.GetLastMemberId(lbIdApp.Text) != tbMemberId.Text)
                        {
                            args.IsValid = !srv.IsExistsMemberId(tbMemberId.Text);
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void cvMobile_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (tbMobile.Text.Length > 0)
                {
                    CustomerService srv = new CustomerService();
                    if (lbType.Text == "Add")
                    {
                        args.IsValid = !srv.IsExistsMobile(tbMobile.Text);
                    }
                    else
                    {
                        if (srv.GetLastMobile(lbId.Text) != tbMobile.Text)
                        {
                            args.IsValid = !srv.IsExistsMobile(tbMobile.Text);
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void cvReason_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                //if (chkInActive.Checked)
                //{
                //    if (dlReason.SelectedValue == "0")
                //    {
                //        args.IsValid = false;
                //    }
                //    else
                //    {
                //        args.IsValid = true;
                //    }
                //}
                //else
                //{
                //    args.IsValid = true;
                //}
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBtnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                pnChangeLog.Visible = true;
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                lbHeadCar.Text = "Edit Car";
                lbCarId.Text = "";
                lbCarType.Text = "";

                if (gvCar.DataKeys != null)
                {
                    string id = gvCar.DataKeys[index].Values["CUS_ID"].ToString();
                    

                    CustomerService srv = new CustomerService();
                    DataRow row = srv.GetCarById(id).AsEnumerable().FirstOrDefault();
                    if (row != null)
                    {
                        lbCarType.Text = "Edit";
                        lbCarId.Text = row["CUS_ID"].ToString();
                        lbMemberId.Text = row["MEMBERID"].ToString();

                        tbVin.Text = row["VIN"].ToString();
                        tbPlateNo.Text = row["PLATE_NO"].ToString();
                        dlDealer.DataSource = srv.GetDealer();
                        dlDealer.DataBind();
                        DisplayDropdownByValue(dlDealer, row["DEALER"].ToString());
                        dlBranch.DataSource = srv.GetBranch(dlDealer.SelectedValue);
                        dlBranch.DataBind();
                        DisplayDropdownByValue(dlBranch, row["branch_code"].ToString());
                        dlModel.DataSource = srv.GetModel();
                        dlModel.DataBind();
                        DisplayDropdownByValue(dlModel, row["MODEL_ID"].ToString());
                        
                        tbDeliveryDate.Text = (row["RS_Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["RS_Date"]).ToString("dd/MM/yyyy");

                        //BindControlCarColor(row["MODEL_ID"].ToString());
                        //DisplayDropdownByValue(dlColor, row["BODYCLR_CD"].ToString());

                        //chkInActive.Checked = (row["INACTIVE_FLAG"].ToString() == "Y") ? true : false;
                        //DisplayDropdownByValue(dlReason, row["INACTIVE_REASON_ID"].ToString());
                        //dlReason.Enabled = chkInActive.Checked;

                        //dlCustType.SelectedValue = row["CUS_TYPE"].ToString();
                        //dlOwner.SelectedValue = row["OWNERSHIP_TYPE"].ToString();

                        tbRemarkCar.Text = row["REMARK"].ToString();
                        tbRemarkCar.Visible = true;
                        lbRemarkCar.Visible = true;

                        //DisplayDropdownByValue(dlSellerIDCar, row["SELLERID"].ToString());

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddCar", "$('#mdAddCar').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                        upModalAddCar.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnCarDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdDelCarConfirm').modal('hide');", true);
                upDelCarConfirm.Update();

                string user = Session["User"].ToString();

                CustomerService srv = new CustomerService();
                srv.DeleteCar(lbDelCarId.Text, tbReasonDelCar.Text, user);

                BindGridData();
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void DisplayDropdownByValue(DropDownList dropDownList, string value)
        {
            try
            {
                if (dropDownList.Items.FindByValue(value) != null)
                {
                    dropDownList.Text = value;
                }
                else
                {
                    dropDownList.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void DisplayDropdownByText(DropDownList dropDownList, string value)
        {
            try
            {
                if (dropDownList.Items.FindByText(value) != null)
                {
                    dropDownList.Text = value;
                }
                else
                {
                    dropDownList.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void cvDupVin_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                #region Old
                /*
                if (!chkInActive.Checked)
                {
                    CustomerService srv = new CustomerService();

                    DataRow oldRow = srv.GetCarById(lbCarId.Text).AsEnumerable().FirstOrDefault();
                    if (oldRow != null)
                    {
                        string oldMem = oldRow["MEMBERID"].ToString();
                        //string curMem = dlMemberId.SelectedValue;
                        string curMem = tbCarMemberId.Text;
                        string oldFlag = oldRow["INACTIVE_FLAG"].ToString();
                        string curFlag = chkInActive.Checked ? "Y" : "N";

                        if (oldMem == curMem)
                        {
                            if (oldFlag == curFlag)
                            {
                                args.IsValid = true;
                            }
                            else
                            {
                                args.IsValid = !srv.IsDuplicateActiveCar(curMem);
                            }
                        }
                        else
                        {
                            args.IsValid = !srv.IsDuplicateActiveCar(curMem);
                        }
                    }
                    else
                    {
                        args.IsValid = !srv.IsDuplicateActiveCar(tbCarMemberId.Text);
                    }
                }
                else
                {
                    args.IsValid = true;
                }
                */
                #endregion

                #region Old
                /*
                if (!chkInActive.Checked)
                {
                    CustomerService srv = new CustomerService();
                    DataRow oldRow = srv.GetCarById(lbCarId.Text).AsEnumerable().FirstOrDefault();
                    if (oldRow != null)
                    {
                        string oldVin = oldRow["VIN"].ToString();
                        string curVin = tbVin.Text;
                        string oldFlag = oldRow["INACTIVE_FLAG"].ToString();
                        string curFlag = chkInActive.Checked ? "Y" : "N";

                        if (oldVin == curVin)
                        {
                            if (oldFlag == curFlag)
                            {
                                args.IsValid = true;
                            }
                            else
                            {
                                args.IsValid = !srv.IsDuplicateActiveVIN(curVin);
                            }
                        }
                        else
                        {
                            args.IsValid = !srv.IsDuplicateActiveVIN(curVin);
                        }
                    }
                    else
                    {
                        args.IsValid = !srv.IsDuplicateActiveVIN(tbVin.Text);
                    }
                }
                */
                #endregion

                CustomerService srv = new CustomerService();
                DataRow oldRow = srv.GetCarById(lbCarId.Text).AsEnumerable().FirstOrDefault();
                if (oldRow != null)
                {
                    string oldVin = oldRow["VIN"].ToString();
                    string curVin = tbVin.Text;

                    if (oldVin == curVin)
                    {
                        args.IsValid = true;
                    }
                    else
                    {
                        args.IsValid = !srv.IsDuplicateActiveVIN2(curVin);
                    }
                }
                else
                {
                    args.IsValid = !srv.IsDuplicateActiveVIN2(tbVin.Text);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void chkInActive_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //dlReason.Enabled = chkInActive.Checked;
                //upModalCarAdd.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvCar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCar.PageIndex = e.NewPageIndex;
                BindGridData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBtnAddCar_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                string id = gvAppUser.DataKeys[index].Values["ID"].ToString();
                lbIdApp.Text = id;

                lbHeadCar.Text = "Insert Car";
                lbClickType.Text = "Car";
                lbCarId.Text = "";
                lbCarType.Text = "Add";
                tbVin.Text = "";
                dlDealer.SelectedIndex = -1;
                tbPlateNo.Text = "";
                dlModel.SelectedIndex = -1;

                tbRemarkCar.Text = "";
                tbRemarkCar.Visible = false;
                lbRemarkCar.Visible = false;
                tbDeliveryDate.Text = "";

                if (Session["Role"].ToString() != "4")
                {
                    //dlSellerIDCar.SelectedIndex = -1;
                }

                if (gvAppUser.DataKeys != null)
                {
                    lbMemberId.Text = gvAppUser.DataKeys[index].Values["MEMBERID"].ToString();
                }
                tbSearchInitial.Text = "";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdCheckInitial", "$('#mdCheckInitial').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                upModalCheckInitial.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        //protected void cvRemarkCust_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    try
        //    {
        //        if (lbType.Text == "Edit")
        //        {
        //            if (tbRemarkCust.Text.Length == 0)
        //            {
        //                args.IsValid = false;
        //            }
        //            else
        //            {
        //                args.IsValid = true;
        //            }
        //        }
        //        else
        //        {
        //            args.IsValid = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ClientPopup(ModalType.Error, ex.Message);
        //    }
        //}
        protected void cvRemarkCar_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (lbCarType.Text == "Edit")
                {
                    if (tbRemarkCar.Text.Length == 0)
                    {
                        args.IsValid = false;
                    }
                    else
                    {
                        args.IsValid = true;
                    }
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void cvRemarkCust_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (lbType.Text == "Edit")
                {
                    if (tbRemarkCust.Text.Length == 0)
                    {
                        args.IsValid = false;
                    }
                    else
                    {
                        args.IsValid = true;
                    }
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBtnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                lbDelCarId.Text = "";
                tbReasonDelCar.Text = "";

                if (gvCar.DataKeys != null)
                {
                    lbDelCarId.Text = gvCar.DataKeys[index].Values["CUS_ID"].ToString();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelCarConfirm", "$('#mdDelCarConfirm').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                    upDelCarConfirm.Update();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvCar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                LinkButton lnkFull = e.Row.FindControl("gvBtnEditCar") as LinkButton;
                if (lnkFull != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    string delFlag = Convert.ToString(drv["DEL_FLAG"]);

                    LinkButton _btnEditCust = (LinkButton)e.Row.FindControl("gvBtnEditCar");
                    LinkButton _btnDelete = (LinkButton)e.Row.FindControl("gvBtnDeleteCar");
                    //LinkButton _btnChgLog = (LinkButton)e.Row.FindControl("gvBtnChgLogCar");

                    if (delFlag == "Y")
                    {
                        _btnEditCust.Visible = false;
                        _btnDelete.Visible = false;
                        //_btnChgLog.Visible = true;

                        foreach (TableCell cell in e.Row.Cells)
                        {
                            cell.Font.Italic = true;
                            cell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#888");
                        }
                    }

                    if (Session["Role"].ToString() == "4")
                    {
                        _btnEditCust.Visible = false;
                        _btnDelete.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void gvBtnChgLog_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                tbChgLogCust.Text = "";
                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();

                    CustomerService srv = new CustomerService();
                    DataRow row = srv.GetCustomerById(id);

                    if (row != null)
                    {
                        string remark = row["REMARK"].ToString();
                        foreach (string line in remark.Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries))
                        {
                            tbChgLogCust.Text += line + Environment.NewLine;
                        }
                    }

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdChgLogCust", "$('#mdChgLogCust').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                    upMdChgLogCust.Update();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBtnChgLogCar_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                tbChgLogCar.Text = "";
                if (gvCar.DataKeys != null)
                {
                    string id = gvCar.DataKeys[index].Values["CUS_ID"].ToString();

                    CustomerService srv = new CustomerService();
                    DataRow row = srv.GetCarById(id).AsEnumerable().FirstOrDefault();

                    if (row != null)
                    {
                        string remark = row["REMARK"].ToString();
                        foreach (string line in remark.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            tbChgLogCar.Text += line + Environment.NewLine;
                        }
                    }

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdChgLogCar", "$('#mdChgLogCar').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                    UpMdChgLogCar.Update();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void gvDlSubMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvAppUser.DataKeys != null)
                {
                    string id = gvAppUser.DataKeys[index].Values["ID"].ToString();
                    string member_id = gvAppUser.DataKeys[index].Values["MEMBERID"].ToString();
                    DropDownList _dl = (DropDownList)gvRow.FindControl("gvDlSubMenu");

                    if (_dl.SelectedValue == "1") // Renew Customer
                    {
                        CustomerService srv = new CustomerService();
                        DataRow row = srv.CheckDeliveryDate(id);
                        if (row != null)
                        {
                            lbExtendCustId.Text = id;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdExtendConfirm", "$('#mdExtendConfirm').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                            upMdExtendConfirm.Update();
                        }
                        else
                        {
                            ClientPopup(ModalType.Warning, "Cannot Renew Customer");
                        }
                    }
                    else if (_dl.SelectedValue == "2") // Adjustment
                    {
                        tbAdjMemberId.Text = "";
                        dlAdjTitle.SelectedIndex = -1;
                        tbAdjFName.Text = "";
                        tbAdjLName.Text = "";
                        tbAdjPoint.Text = "";

                        CustomerService srv = new CustomerService();
                        DataRow row = srv.GetAppUserByMemberID(member_id);
                        if (row != null)
                        {
                            tbAdjMemberId.Text = row["MEMBERID"].ToString();
                            tbAdjFName.Text = row["FNAME"].ToString();
                            tbAdjLName.Text = row["LNAME"].ToString();
                            tbAdjPoint.Text = row["PRIVILEGE_CNT"].ToString();
                            DisplayDropdownByText(dlAdjTitle, row["TITLENAME"].ToString());
                        }

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdjust", "$('#mdAdjust').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                        upMdAdjust.Update();
                    }
                    else if (_dl.SelectedValue == "3") // Privilege Redemption
                    {
                        tbPrivilegeMemberId.Text = "";
                        dlPrivilege.SelectedIndex = -1;
                        tbPrivilegeCNT.Text = "";
                        dlRedeemCode.SelectedIndex = -1;

                        CustomerService srv = new CustomerService();
                        DataRow row = srv.GetAppUserById(id);
                        if (row != null)
                        {
                            //lbDisplayType.Text = row["display_type"].ToString();
                            tbPrivilegeMemberId.Text = row["MEMBERID"].ToString();
                            tbPrivilegeCNT.Text = row["PRIVILEGE_CNT"].ToString();

                            dlPrivilege.DataSource = srv.GetPrivilegesActive();
                            dlPrivilege.DataBind();
                            dlPrivilege.SelectedIndex = -1;

                            dlRedeemCode.DataSource = srv.GetPrivilegesRedeemCode(dlPrivilege.SelectedValue);
                            dlRedeemCode.DataBind();
                            dlRedeemCode.SelectedIndex = -1;
                        }

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdPrivilege", "$('#mdPrivilege').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                        upMdPrivilege.Update();
                    }
                    else if (_dl.SelectedValue == "4") // Change Log
                    {
                        tbChgLogCust.Text = "";
                        CustomerService srv = new CustomerService();
                        DataRow row = srv.GetAppUserById(id);

                        if (row != null)
                        {
                            string remark = row["REMARK"].ToString();
                            foreach (string line in remark.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                tbChgLogCust.Text += line + Environment.NewLine;
                            }
                        }

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdChgLogCust", "$('#mdChgLogCust').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                        upMdChgLogCust.Update();
                    }

                    // Clear item select change
                    _dl.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void dlPrivilege_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (dlPrivilege.SelectedIndex > -1)
                {
                    dlRedeemCode.SelectedIndex = -1;

                    CustomerService srv = new CustomerService();
                    dlRedeemCode.DataSource = srv.GetPrivilegesRedeemCode(dlPrivilege.SelectedValue);
                    dlRedeemCode.DataBind();
                    dlRedeemCode.SelectedIndex = -1;
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdPrivilege", "$('#mdPrivilege').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                upMdPrivilege.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnExtend_Click(object sender, EventArgs e)
        {
            try
            {
                new CustomerService().ExtendCustomer(lbExtendCustId.Text);
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnAdj_Click(object sender, EventArgs e)
        {
            try
            {
                new CustomerService().AdjustCustomer(tbAdjMemberId.Text, tbAdjPoint.Text);
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnPrivilege_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerService srv = new CustomerService();
                lbDisplayType.Text = srv.GetDisplayType(dlPrivilege.SelectedValue);

                string privilege_code_id = dlRedeemCode.SelectedValue;

                if ((tbPrivilegeCNT.Text == "0") && (lbDisplayType.Text == "2"))
                {
                    ClientPopup(ModalType.Warning, "No Privilege CNT");
                }
                else
                {
                    srv.RedeemPrivilegeCode(privilege_code_id, tbPrivilegeMemberId.Text, dlPrivilege.SelectedValue);

                    ClientPopup(ModalType.Success, "Completed");
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void gvAppUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAppUser.PageIndex = e.NewPageIndex;
                BindGridData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvAppUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvAppUser.SelectedIndex;
                if (gvAppUser.DataKeys != null)
                {
                    string id = gvAppUser.DataKeys[rowIndex].Values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvAppUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    lbDelIdApp.Text = "";
                    lbDelId.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvAppUser.DataKeys != null)
                    {
                        lbDelIdApp.Text = gvAppUser.DataKeys[index].Values["ID"].ToString();
                        lbDelId.Text = gvAppUser.DataKeys[index].Values["MEMBERID"].ToString();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmApp", "$('#mdDelConfirmApp').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                        upModelDelApp.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvAppUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                LinkButton lnkFull = e.Row.FindControl("gvBtnEditApp") as LinkButton;
                if (lnkFull != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
                }
                lnkFull = e.Row.FindControl("gvBtnAddCar") as LinkButton;
                if (lnkFull != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    string delFlag = Convert.ToString(drv["DEL_FLAG"]);

                    LinkButton _btnAddCar = (LinkButton)e.Row.FindControl("gvBtnAddCar");
                    LinkButton _btnEditApp = (LinkButton)e.Row.FindControl("gvBtnEditApp");
                    LinkButton _btnDeleteApp = (LinkButton)e.Row.FindControl("gvBtnDeleteApp");
                    DropDownList _dlSubMenu = (DropDownList)e.Row.FindControl("gvDlSubMenu");
                    //LinkButton _btnChgLog = (LinkButton)e.Row.FindControl("gvBtnChgLogCar");

                    if (delFlag == "Y")
                    {
                        _btnAddCar.Visible = false;
                        _btnEditApp.Visible = false;
                        _btnDeleteApp.Visible = false;
                        _dlSubMenu.Visible = false;
                        //_btnChgLog.Visible = true;

                        foreach (TableCell cell in e.Row.Cells)
                        {
                            cell.Font.Italic = true;
                            cell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#888");
                        }
                    }

                    if (Session["Role"].ToString() == "4")
                    {
                        _btnEditApp.Visible = false;
                        _btnDeleteApp.Visible = false;
                    }

                    DropDownList _dl = (DropDownList)e.Row.FindControl("gvDlSubMenu");
                    _dl.Items.Add(new ListItem { Text = "", Value = "" });
                    _dl.Items.Add(new ListItem { Text = "Renew Customer", Value = "1" });
                    _dl.Items.Add(new ListItem { Text = "Adjustment", Value = "2" });
                    _dl.Items.Add(new ListItem { Text = "Redemption", Value = "3" });
                    _dl.Items.Add(new ListItem { Text = "Change Log", Value = "4" });
                    //if (Session["Role"].ToString() == "2")
                    //{

                    //}
                    //else
                    //{
                    //    _dl.Items.Add(new ListItem { Text = "", Value = "" });
                    //    _dl.Items.Add(new ListItem { Text = "Change Log", Value = "4" });
                    //}
                }
            }
            catch
            {
            }
        }
        protected void gvBtnEditApp_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                lbId.Text = "";
                lbIdApp.Text = "";

                if (gvAppUser.DataKeys != null)
                {
                    string id = gvAppUser.DataKeys[index].Values["ID"].ToString();
                    lbType.Text = "Edit";
                    lbIdApp.Text = id;

                    CustomerService srv = new CustomerService();
                    DataRow row = srv.GetAppUserById(id);
                    if (row != null)
                    {
                        using (DataTable privince = srv.GetProvinces())
                        {
                            dlProvince.DataSource = privince;
                            dlProvince.DataBind();

                            dlProvinceApp.DataSource = privince;
                            dlProvinceApp.DataBind();
                        }

                        string _memberId = row["MEMBERID"].ToString().Trim();
                        tbMemberId.Text = _memberId;

                        if (Session["Role"].ToString() == "2") // Admin can edit memberid
                        {
                            tbMemberId.Enabled = true;
                        }
                        else
                        {
                            tbMemberId.Enabled = false;
                        }

                        tbFirstnameApp.Text = row["FNAME"].ToString();
                        tbLastnameApp.Text = row["LNAME"].ToString();
                        tbEmailApp.Text = row["EMAIL"].ToString();
                        tbMobileApp.Text = row["MOBILE"].ToString();
                        tbSSNApp.Text = row["SSN"].ToString();
                        dlGenerApp.SelectedValue = row["GENDER"].ToString();
                        dlTitleApp.SelectedValue = row["TITLENAME"].ToString();
                        tbBirthdateApp.Text = (row["BIRTHDATE"] == DBNull.Value) ? "" : Convert.ToDateTime(row["BIRTHDATE"]).ToString("dd/MM/yyyy");
                        tbAddr1App.Text = row["ADDRESS1"].ToString();

                        dlProvinceApp.SelectedItem.Text = row["PROVINCE"].ToString();
                        //DisplayDropdownByText(dlProvinceApp, row["PROVINCE"].ToString().Trim());
                        dlDistictApp.DataSource = srv.GetDistrict(dlProvinceApp.SelectedValue);
                        dlDistictApp.DataBind();
                        dlDistictApp.SelectedItem.Text = row["DISTRICT"].ToString();
                        //DisplayDropdownByText(dlDistictApp, row["DISTRICT"].ToString().Trim());
                        dlSubDistictApp.DataSource = srv.GetSubDistrict(dlProvinceApp.SelectedValue, dlDistictApp.SelectedValue);
                        dlSubDistictApp.DataBind();
                        dlSubDistictApp.SelectedItem.Text = row["SUBDISTRICT"].ToString();
                        //DisplayDropdownByText(dlSubDistictApp, row["SUBDISTRICT"].ToString().Trim());
                        tbPostCodeApp.Text = row["POSTALCODE"].ToString();

                        tbRemarkCar.Text = row["REMARK"].ToString();
                        tbRemarkCar.Visible = true;
                        lbRemarkCar.Visible = true;

                        DataRow rowCarOwner = srv.GetCustomerById(_memberId);
                        if (rowCarOwner != null)
                        {
                            lbId.Text = rowCarOwner["ID"].ToString();
                            tbFirstname.Text = rowCarOwner["FNAME"].ToString();
                            tbLastname.Text = rowCarOwner["LNAME"].ToString();
                            tbEmail.Text = rowCarOwner["EMAIL"].ToString();
                            tbMobile.Text = rowCarOwner["MOBILE"].ToString();
                            tbSSN.Text = rowCarOwner["SSN"].ToString();
                            dlGener.SelectedValue = rowCarOwner["GENDER"].ToString();
                            dlTitle.SelectedValue = rowCarOwner["TITLENAME"].ToString();
                            tbBirthdate.Text = (rowCarOwner["BIRTHDATE"] == DBNull.Value) ? "" : Convert.ToDateTime(rowCarOwner["BIRTHDATE"]).ToString("dd/MM/yyyy");
                            tbAddr1.Text = rowCarOwner["ADDRESS1"].ToString();

                            dlProvince.SelectedItem.Text = rowCarOwner["PROVINCE"].ToString();
                            //DisplayDropdownByText(dlProvince, rowCarOwner["PROVINCE"].ToString().Trim());
                            dlDistict.DataSource = srv.GetDistrict(dlProvince.SelectedValue);
                            dlDistict.DataBind();
                            dlDistict.SelectedItem.Text = rowCarOwner["DISTRICT"].ToString();
                            //DisplayDropdownByText(dlDistict, rowCarOwner["DISTRICT"].ToString().Trim());
                            dlSubDistict.DataSource = srv.GetSubDistrict(dlProvince.SelectedValue, dlDistict.SelectedValue);
                            dlSubDistict.DataBind();
                            dlSubDistict.SelectedItem.Text = rowCarOwner["SUBDISTRICT"].ToString();
                            //DisplayDropdownByText(dlSubDistict, rowCarOwner["SUBDISTRICT"].ToString().Trim());

                            tbPostCode.Text = rowCarOwner["POSTALCODE"].ToString();
                        }
                    }
                    
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);

                    divRemarkCust.Visible = true;
                    tbRemarkCust.Text = "";
                    tbRemarkCust.Visible = true;
                    lbRemarkCust.Visible = true;
                    upModalAdd.Update();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBtnDeleteApp_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                lbDelIdApp.Text = "";
                tbReasonDelApp.Text = "";
                lbDelId.Text = "";

                if (gvAppUser.DataKeys != null)
                {
                    lbDelIdApp.Text = gvAppUser.DataKeys[index].Values["ID"].ToString();
                    
                }
                if (gvCust.DataKeys != null)
                {
                    lbDelId.Text = gvCust.DataKeys[index].Values["ID"].ToString();
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmApp", "$('#mdDelConfirmApp').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                upModelDelApp.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        //protected void btnSaveApp_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!Page.IsValid)
        //        {
        //            upModalAddApp.Update();
        //            return;
        //        }

        //        string firstName = tbFirstnameApp.Text;
        //        string lastName = tbLastnameApp.Text;
        //        string nickName = "";
        //        string gender = dlGenerApp.SelectedValue;
        //        string age = "";
        //        string email = tbEmailApp.Text;
        //        string ssn = tbSSNApp.Text;
        //        string mobile = tbMobileApp.Text;
        //        string id = lbIdApp.Text;
        //        string memberId = tbMemberIdApp.Text;
        //        string title = dlTitleApp.SelectedValue;
        //        string birthdate = tbBirthdateApp.Text;
        //        string addr1 = tbAddr1App.Text;
        //        string addr2 = "";
        //        string subDistrict = dlSubDistictApp.SelectedItem.ToString();
        //        string district = dlDistictApp.SelectedItem.ToString();
        //        string province = dlProvinceApp.SelectedItem.ToString();
        //        string postcode = tbPostCodeApp.Text;
        //        string homeNumber = "";
        //        string sellerId = dlSellerIDApp.SelectedValue;

        //        CustomerService srv = new CustomerService();
        //        if (lbTypeApp.Text == "Add")
        //        {
        //            string idInsert = srv.AddCustomerApp(firstName, lastName, nickName, gender, age, email, ssn, mobile, memberId, Session["User"].ToString(), title, birthdate,
        //                addr1, addr2, subDistrict, district, province, postcode, homeNumber, sellerId);
        //            //BindGridData();

        //            BindGridDataAfterAddCustomer(idInsert);

        //            ClientPopup(ModalType.Success, "Completed");
        //        }
        //        else if (lbTypeApp.Text == "Edit")
        //        {
        //            srv.UpdateCustomerApp(firstName, lastName, nickName, gender, age, email, ssn, mobile, id, memberId, Session["User"].ToString(), title, birthdate,
        //                addr1, addr2, subDistrict, district, province, postcode, homeNumber, "", sellerId);
        //            BindGridData();
        //            ClientPopup(ModalType.Success, "Completed");
        //        }

        //        BindControlCarMemberId();
        //    }
        //    catch (Exception ex)
        //    {
        //        //ClientPopup(ModalType.Error, ex.Message);
        //    }
        //}
        protected void btnDeleteApp_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbDelIdApp.Text != "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddApp", "$('#mdDelConfirmApp').modal('hide');", true);
                    upModelDelApp.Update();

                    string id = lbDelIdApp.Text;
                    string user = Session["User"].ToString();

                    CustomerService srv = new CustomerService();
                    srv.DeleteCustomerApp(id, tbReasonDelApp.Text, user);

                    BindGridData();

                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbDelId.Text != "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddApp", "$('#mdDelConfirmApp').modal('hide');", true);
                    upModelDelApp.Update();

                    string id = lbDelId.Text;
                    string user = Session["User"].ToString();

                    CustomerService srv = new CustomerService();
                    srv.DeleteCustomer(id, tbReasonDelCust.Text, user);

                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
                else
                {
                    BindGridData();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        //protected void cvMemberIdApp_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    try
        //    {
        //        if (tbMemberIdApp.Text.Length > 0)
        //        {
        //            CustomerService srv = new CustomerService();
        //            if (lbTypeApp.Text == "Add")
        //            {
        //                args.IsValid = !srv.IsExistsMemberId(tbMemberIdApp.Text);
        //            }
        //            else
        //            {
        //                if (srv.GetLastMemberId(lbIdApp.Text) != tbMemberIdApp.Text)
        //                {
        //                    args.IsValid = !srv.IsExistsMemberId(tbMemberIdApp.Text);
        //                }
        //                else
        //                {
        //                    args.IsValid = true;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ClientPopup(ModalType.Error, ex.Message);
        //    }
        //}
        protected void cvMobileApp_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (tbMobileApp.Text.Length > 0)
                {
                    CustomerService srv = new CustomerService();
                    if (lbType.Text == "Add")
                    {
                        args.IsValid = !srv.IsExistsMobile(tbMobileApp.Text);
                    }
                    else
                    {
                        if (srv.GetLastMobileApp(lbIdApp.Text) != tbMobileApp.Text)
                        {
                            args.IsValid = !srv.IsExistsMobile(tbMobileApp.Text);
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        //protected void cvRemarkApp_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    try
        //    {
        //        if (lbTypeApp.Text == "Edit")
        //        {
        //            if (tbRemarkApp.Text.Length == 0)
        //            {
        //                args.IsValid = false;
        //            }
        //            else
        //            {
        //                args.IsValid = true;
        //            }
        //        }
        //        else
        //        {
        //            args.IsValid = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ClientPopup(ModalType.Error, ex.Message);
        //    }
        //}
        protected void cvReasonApp_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                //if (chkInActive.Checked)
                //{
                //    if (dlReason.SelectedValue == "0")
                //    {
                //        args.IsValid = false;
                //    }
                //    else
                //    {
                //        args.IsValid = true;
                //    }
                //}
                //else
                //{
                //    args.IsValid = true;
                //}
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void dlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomerService srv = new CustomerService();
            dlSubDistict.Items.Clear();
            dlDistict.DataSource = srv.GetDistrict(dlProvince.SelectedValue);
            dlDistict.DataBind();

            dlDistict_SelectedIndexChanged(null, null);
            dlSubDistict_SelectedIndexChanged(null, null);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAdd.Update();
        }

        protected void dlDistict_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomerService srv = new CustomerService();
            dlSubDistict.DataSource = srv.GetSubDistrict(dlProvince.SelectedValue, dlDistict.SelectedValue);
            dlSubDistict.DataBind();

            dlSubDistict_SelectedIndexChanged(null, null);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAdd.Update();
        }

        protected void dlProvinceApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomerService srv = new CustomerService();
            dlSubDistictApp.Items.Clear();
            dlDistictApp.DataSource = srv.GetDistrict(dlProvinceApp.SelectedValue);
            dlDistictApp.DataBind();

            dlDistictApp_SelectedIndexChanged(null, null);
            dlSubDistictApp_SelectedIndexChanged(null, null);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAdd.Update();
            
        }

        protected void dlDistictApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomerService srv = new CustomerService();
            dlSubDistictApp.DataSource = srv.GetSubDistrict(dlProvinceApp.SelectedValue, dlDistictApp.SelectedValue);
            dlSubDistictApp.DataBind();

            dlSubDistictApp_SelectedIndexChanged(null, null);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAdd.Update();
        }

        protected void btnSearchInitial_Click(object sender, EventArgs e)
        {
            string vin_search = tbSearchInitial.Text;
            CustomerService srv = new CustomerService();
            DataRow row = srv.CheckVinInitial(vin_search);
            if (row != null)
            {
                using (DataTable dealer = srv.GetDealer())
                {
                    dlDealerInit.DataSource = dealer;
                    dlDealerInit.DataBind();
                }
                using (DataTable model = srv.GetModel())
                {
                    dlModelInit.DataSource = model;
                    dlModelInit.DataBind();
                }
                tbVinInit.Enabled = false;
                tbPlateNoInit.Enabled = false;
                dlDealerInit.Enabled = false;
                dlBranchInit.Enabled = false;
                tbRSDateInit.Enabled = false;
                dlModelInit.Enabled = false;

                tbVinInit.Text = row["vin"].ToString();
                tbPlateNoInit.Text = row["plate_no"].ToString();
                DisplayDropdownByValue(dlDealerInit, row["dealer"].ToString());
                dlBranchInit.DataSource = srv.GetBranch(dlDealerInit.SelectedValue);
                dlBranchInit.DataBind();
                DisplayDropdownByValue(dlBranchInit, row["branch"].ToString());
                tbRSDateInit.Text = (row["rs_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["rs_date"]).ToString("dd/MM/yyyy");
                dlModelInit.SelectedValue = row["model"].ToString();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdCheckInitial", "$('#mdCheckInitial').modal('hide');", true);
                //upModalCheckInitial.Update();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDataInitial", "$('#mdDataInitial').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                upModalDataInitial.Update();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdCheckInitial", "$('#mdCheckInitial').modal('hide');", true);
                ClientPopup(ModalType.Warning, "Not Found VIN in Initial");
            }
        }

        protected void btnInsertCarInitial_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdCheckInitial", "$('#mdCheckInitial').modal('hide');", true);
            //upModalCheckInitial.Update();

            //lbClickType.Text = "Car";
            //lbCarType.Text = "Insert";
            lbHeadCar.Text = "Insert Car";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdCheckInitial", "$('#mdCheckInitial').modal('hide');", true);
            //upModalCheckInitial.Update();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddCar", "$('#mdAddCar').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAddCar.Update();
           
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                upModalAddCar.Update();
                return;
            }

            string memberId = lbMemberId.Text;
            lbCarType.Text = "Add";
            string dealer = dlDealerInit.SelectedValue;
            string modelId = dlModelInit.SelectedValue;
            string plateNo = tbPlateNoInit.Text;
            string vin = tbVinInit.Text;
            string colorId = "";
            string user = Session["User"].ToString();
            //bool isInactive = chkInActive.Checked;
            //string inactiveReason = dlReason.SelectedValue;
            string cusType = "";
            string ownerShip = "";
            string rsDate = tbRSDateInit.Text;
            string sellerId = "";

            CustomerService srv = new CustomerService();
            DataRow row2 = srv.CheckVinInCar(vin);
            if (row2 != null)
            {
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddCar", "$('#mdAddCar').modal();", true);
                //upModalCarAdd.Update();
                ClientPopup(ModalType.Warning, "Found VIN in System");
            }
            else
            {
                ////upModalDataInitial.Update();
                if (lbClickType.Text == "Customer")
                {
                    
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDataInitial", "$('#mdDataInitial').modal('hide');", true);
                    divRemarkCust.Visible = false;
                    tbRemarkCust.Text = "";
                    tbRemarkCust.Visible = false;
                    lbRemarkCust.Visible = false;

                    tbMemberId.Text = "";
                    tbMemberId.Enabled = true;
                    tbFirstname.Text = "";
                    tbLastname.Text = "";
                    tbEmail.Text = "";
                    tbMobile.Text = "";
                    tbSSN.Text = "";
                    dlGener.SelectedIndex = -1;
                    lbType.Text = "Add";
                    dlTitle.SelectedIndex = -1;
                    tbBirthdate.Text = "";
                    tbAddr1.Text = "";
                    dlProvince.SelectedIndex = -1;
                    dlProvince_SelectedIndexChanged(null, null);
                    dlDistict.SelectedIndex = -1;
                    dlDistict_SelectedIndexChanged(null, null);
                    dlSubDistict.SelectedIndex = -1;
                    dlSubDistict_SelectedIndexChanged(null, null);
                    //tbPostCode.Text = "";

                    tbFirstnameApp.Text = "";
                    tbLastnameApp.Text = "";
                    tbEmailApp.Text = "";
                    tbMobileApp.Text = "";
                    tbSSNApp.Text = "";
                    dlGenerApp.SelectedIndex = -1;
                    dlTitleApp.SelectedIndex = -1;
                    tbBirthdateApp.Text = "";
                    tbAddr1App.Text = "";
                    dlProvinceApp.SelectedIndex = -1;
                    dlProvinceApp_SelectedIndexChanged(null, null);
                    dlDistictApp.SelectedIndex = -1;
                    dlDistictApp_SelectedIndexChanged(null, null);
                    dlSubDistictApp.SelectedIndex = -1;
                    dlProvinceApp.SelectedIndex = -1;
                    dlSubDistictApp_SelectedIndexChanged(null, null);
                    //tbPostCodeApp.Text = "";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                    upModalAdd.Update();
                }
                else
                {
                    srv.AddCar(memberId, dealer, modelId, plateNo, vin, user, colorId, cusType, ownerShip, rsDate, sellerId);
                    ClientPopup(ModalType.Success, "Completed");
                }
                
            }

            //tbMemberId.Text = "";
            ////tbMemberId.Enabled = true;
            //tbFirstname.Text = "";
            //tbLastname.Text = "";
            //tbEmail.Text = "";
            //tbMobile.Text = "";
            //tbSSN.Text = "";
            //dlGener.SelectedIndex = -1;
            //lbType.Text = "Add";
            //dlTitle.SelectedIndex = -1;
            //tbBirthdate.Text = "";
            //tbAddr1.Text = "";
            //dlSubDistict.SelectedIndex = -1;
            //dlDistict.SelectedIndex = -1;
            //dlProvince.SelectedIndex = -1;
            //tbPostCode.Text = "";

            //DataRow row = srv.CheckVinInCar(tbVinInit.Text);
            //if (row != null)
            //{
            //    ClientPopup(ModalType.Warning, "Found VIN in System");
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDataInitial", "$('#mdDataInitial').modal('hide');", true);
            //    //upModalDataInitial.Update();
            //    if (lbClickType.Text == "Customer")
            //    {
            //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
            //        upModalAdd.Update();
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddCar", "$('#mdAddCar').modal();", true);
            //        upModalAddCar.Update();
            //    }
            //}
        }

        protected void dlDealerInit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomerService srv = new CustomerService();
            dlBranchInit.DataSource = srv.GetBranch(dlDealerInit.SelectedValue);
            dlBranchInit.DataBind();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDataInitial", "$('#mdDataInitial').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalDataInitial.Update();
        }

        protected void dlDealer_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomerService srv = new CustomerService();
            dlBranch.DataSource = srv.GetBranch(dlDealer.SelectedValue);
            dlBranch.DataBind();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddCar", "$('#mdAddCar').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAddCar.Update();
        }

        protected void btDuplicate_Click(object sender, EventArgs e)
        {
            CustomerService srv = new CustomerService();
            dlTitleApp.SelectedValue = dlTitle.SelectedValue;
            tbFirstnameApp.Text = tbFirstname.Text;
            tbLastnameApp.Text = tbLastname.Text;
            dlGenerApp.SelectedValue = dlGener.SelectedValue;
            tbBirthdateApp.Text = tbBirthdate.Text;
            tbSSNApp.Text = tbSSN.Text;
            tbEmailApp.Text = tbEmail.Text;
            tbMobileApp.Text = tbMobile.Text;
            tbAddr1App.Text = tbAddr1.Text;
            
            
            dlProvinceApp.SelectedValue = dlProvince.SelectedValue;
            using (DataTable district = srv.GetDistrict(dlProvinceApp.SelectedValue))
            {
                dlDistictApp.DataSource = district;
                dlDistictApp.DataBind();
            }
            dlDistictApp.SelectedValue = dlDistict.SelectedValue;
            using (DataTable subdistrict = srv.GetSubDistrict(dlProvinceApp.SelectedValue, dlDistictApp.SelectedValue))
            {
                dlSubDistictApp.DataSource = subdistrict;
                dlSubDistictApp.DataBind();
            }
            dlSubDistictApp.SelectedValue = dlSubDistict.SelectedValue;
            tbPostCodeApp.Text = tbPostCode.Text;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAdd.Update();
        }

        protected void cvSSNApp_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (tbSSNApp.Text.Length > 0)
            {
                CustomerService srv = new CustomerService();
                if (lbType.Text == "Add")
                {
                    args.IsValid = !srv.IsExistsCitizen(tbSSNApp.Text);
                }
                else
                {
                    if (srv.GetLastCitizenApp(lbIdApp.Text) != tbSSNApp.Text)
                    {
                        args.IsValid = !srv.IsExistsCitizen(tbSSNApp.Text);
                    }
                    else
                    {
                        args.IsValid = true;
                    }
                }
            }
        }

        protected void dlSubDistict_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomerService srv = new CustomerService();
            string postcode = srv.GetPostcode(dlSubDistict.SelectedValue);
            tbPostCode.Text = postcode;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAdd.Update();
        }

        protected void dlSubDistictApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomerService srv = new CustomerService();
            string postcode = srv.GetPostcode(dlSubDistictApp.SelectedValue);
            tbPostCodeApp.Text = postcode;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbRSDateInit').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbBirthdateApp').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDeliveryDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAdd.Update();
        }
    }
}