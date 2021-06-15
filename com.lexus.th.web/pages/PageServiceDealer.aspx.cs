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
    public partial class PageServiceDealer : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }
        DataTable dates;
        private string holiday = "Holiday";
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
                        gvDealer.DataSource = new System.Data.DataTable();
                        gvDealer.DataBind();

                        BindGeo();                       
                        BindStartEndTime();
                       
                        dlGeo.SelectedIndex = 1;
                        BindPinMapIcon();
                        dlIsPickServiceCriteria.SelectedIndex = -1;
                        dlPinMapIcon.SelectedIndex = -1;
                        CheckTab();
                        BindProvince();
                    }
                }
                else
                {
                    lbTab.Text = "";
                    CheckTab();
                    dates = (DataTable)ViewState[holiday];
                }
                this.RegisterPostBackControl();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void RegisterPostBackControl()
        {
            foreach (GridViewRow row in gvDealer.Rows)
            {
                LinkButton lnkFull = row.FindControl("gvBtnEdit") as LinkButton;
                if (lnkFull != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
                }
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
        protected void gvDealer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton lnkFull = e.Row.FindControl("gvBtnEdit") as LinkButton;
            if (lnkFull != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
            }
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;");
            }
        }

        private void BindProvince()
        {
            try
            {
                ProvinceService srv = new ProvinceService();
                List<ProvinceModel> datas = srv.GetProvince("EN", "");

                foreach (ProvinceModel p in datas)
                {
                    ListItem item = new ListItem(p.name, p.id.ToString());
                    dlProvince.Items.Add(item);
                }

                //dlProvince.DataSource = srv.GetProvince("EN", "");
                //dlProvince.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindProvinceByRegion(string regionid)
        {
            try
            {
                 ProvinceService srv = new ProvinceService();
                 dlProvince.DataSource = srv.GetProvinceByRegion("EN",Convert.ToInt32(regionid));
                 dlProvince.DataBind();
            }     
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindProvinceByRegion(dlGeo2.SelectedValue);
                upModalAdd.Update();

                //BindGridData(dlProvince.SelectedValue);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void gvDealer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    lbDelId.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvDealer.DataKeys != null)
                    {
                        lbDelId.Text = gvDealer.DataKeys[index].Values["ID"].ToString();
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
        protected void gvDealer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDealer.PageIndex = e.NewPageIndex;
                BindGridData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvDealer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvDealer.SelectedIndex;
                if (gvDealer.DataKeys != null)
                {
                    string id = gvDealer.DataKeys[rowIndex].Values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnAddModal_Click(object sender, EventArgs e)
        {
            try
            {
                tbDealerName.Text = "";
                tbDealerCode.Text = "";
                tbBranchName.Text = "";
                tbBranchCode.Text = "";
                tbAddress.Text = "";
                tbMobile.Text = "";
                tbOffice_hours.Text = "";
                tbOffice_hours2.Text = "";
                tbLat.Text = "";
                tbLng.Text = "";
                dlGeo2.SelectedIndex = 0;
                dlActive.SelectedIndex = 0;
                dlProvince.SelectedIndex = 0;
                tbDealerNameTH.Text = "";
                tbBranchNameTH.Text = "";
                tbAddressTH.Text = "";
                tbOffice_hoursTH.Text = "";
                tbOffice_hours2TH.Text = "";
                tbDealerDurationMin.Text = "2";
                tbDealerDurationMax.Text = "30";
                lbDuplicate.Text = "";
                tbDate.Text = "";
                tbCallCenterMobileBooking.Text = "";
                tbCallCenterEmailBooking.Text = "";
                dlPinMapIcon.SelectedIndex = -1;
                chkPickupService.Checked = false;

                SetWorkingAppointmentTimeToDefault();
                SetWorkingTestDriveTimeToDefault();
                BindNewHoliday();
                lbType.Text = "Add";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                upModalAdd.Update();
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
                string dealer_name = tbDealerName.Text;
                string dealer_code = tbDealerCode.Text;
                string branch_name = tbBranchName.Text;
                string branch_code = tbBranchCode.Text;
                string address = tbAddress.Text;
                string mobile = tbMobile.Text;
                string office_hours = tbOffice_hours.Text;
                string office_hours2 = tbOffice_hours2.Text;
                string lat = tbLat.Text;
                string lng = tbLng.Text;
                string geo_id = dlGeo2.SelectedValue;
                string active = dlActive.SelectedValue;
                string dealer_name_th = tbDealerNameTH.Text;
                string branch_name_th = tbBranchNameTH.Text;
                string address_th = tbAddressTH.Text;
                string office_hours_th = tbOffice_hoursTH.Text;
                string office_hours2_th = tbOffice_hours2TH.Text;
                string dealer_duration_min = tbDealerDurationMin.Text;
                string dealer_duration_max = tbDealerDurationMax.Text;
                string pin_map_icon_id = dlPinMapIcon.SelectedValue;
                string is_pickup_service = chkPickupService.Checked ? "1" : "0";
                string province_id = dlProvince.SelectedValue;

                DealerWorkingTimeModel appointMent = new DealerWorkingTimeModel()
                {
                    Dealer_ID = lbId.Text,
                    Service_Type = "ServiceAppointment",
                    CallCenterMobile = tbCallCenterMobile.Text,
                    CanncenterEmail = tbCallCenterEmail.Text,

                    Is_Monday = chkMon.Checked == true ? "1" : "0",
                    Is_Tuesday = chkTue.Checked == true ? "1" : "0",
                    Is_Wednesday = chkWed.Checked == true ? "1" : "0",
                    Is_Thursday = chkThu.Checked == true ? "1" : "0",
                    Is_Friday = chkFri.Checked == true ? "1" : "0",
                    Is_Satday = chkSat.Checked == true ? "1" : "0",
                    Is_Sunday = chkSun.Checked == true ? "1" : "0",

                    Mon_StartTime = FormatTimeSpan(dlStartTimeMon.SelectedValue),
                    Tues_StartTime = FormatTimeSpan(dlStartTimeTue.SelectedValue),
                    Wed_StartTime = FormatTimeSpan(dlStartTimeWed.SelectedValue),
                    Thur_StartTime = FormatTimeSpan(dlStartTimeThu.SelectedValue),
                    Fri_StartTime = FormatTimeSpan(dlStartTimeFri.SelectedValue),
                    Sat_StartTime = FormatTimeSpan(dlStartTimeSat.SelectedValue),
                    Sun_StartTime = FormatTimeSpan(dlStartTimeSun.SelectedValue),


                    Mon_EndTime = FormatTimeSpan(dlEndTimeMon.SelectedValue),
                    Tues_EndTime = FormatTimeSpan(dlEndTimeTue.SelectedValue),
                    Wed_EndTime = FormatTimeSpan(dlEndTimeWed.SelectedValue),
                    Thur_EndTime = FormatTimeSpan(dlEndTimeThu.SelectedValue),
                    Fri_EndTime = FormatTimeSpan(dlEndTimeFri.SelectedValue),
                    Sat_EndTime = FormatTimeSpan(dlEndTimeSat.SelectedValue),
                    Sun_EndTime = FormatTimeSpan(dlEndTimeSun.SelectedValue),

                };

                DealerWorkingTimeModel testDrive = new DealerWorkingTimeModel()
                {
                    Dealer_ID = lbId.Text,
                    Service_Type = "TestDrive",
                    CallCenterMobile = tbCallCenterMobileTestDrive.Text,
                    CanncenterEmail = tbCallCenterEmailTestDrive.Text,

                    Is_Monday = chkTdMon.Checked == true ? "1" : "0",
                    Is_Tuesday = chkTdTue.Checked == true ? "1" : "0",
                    Is_Wednesday = chkTdWed.Checked == true ? "1" : "0",
                    Is_Thursday = chkTdThu.Checked == true ? "1" : "0",
                    Is_Friday = chkTdFri.Checked == true ? "1" : "0",
                    Is_Satday = chkTdSat.Checked == true ? "1" : "0",
                    Is_Sunday = chkTdSun.Checked == true ? "1" : "0",

                    Mon_StartTime = FormatTimeSpan(dlStartTimeTdMon.SelectedValue),
                    Tues_StartTime = FormatTimeSpan(dlStartTimeTdTue.SelectedValue),
                    Wed_StartTime = FormatTimeSpan(dlStartTimeTdWed.SelectedValue),
                    Thur_StartTime = FormatTimeSpan(dlStartTimeTdThu.SelectedValue),
                    Fri_StartTime = FormatTimeSpan(dlStartTimeTdFri.SelectedValue),
                    Sat_StartTime = FormatTimeSpan(dlStartTimeTdSat.SelectedValue),
                    Sun_StartTime = FormatTimeSpan(dlStartTimeTdSun.SelectedValue),


                    Mon_EndTime = FormatTimeSpan(dlEndTimeTdMon.SelectedValue),
                    Tues_EndTime = FormatTimeSpan(dlEndTimeTdTue.SelectedValue),
                    Wed_EndTime = FormatTimeSpan(dlEndTimeTdWed.SelectedValue),
                    Thur_EndTime = FormatTimeSpan(dlEndTimeTdThu.SelectedValue),
                    Fri_EndTime = FormatTimeSpan(dlEndTimeTdFri.SelectedValue),
                    Sat_EndTime = FormatTimeSpan(dlEndTimeTdSat.SelectedValue),
                    Sun_EndTime = FormatTimeSpan(dlEndTimeTdSun.SelectedValue),

                };

                DealerWorkingTimeModel booking = new DealerWorkingTimeModel()
                {
                    Dealer_ID = lbId.Text,
                    Service_Type = "OnlineBooking",

                    CallCenterMobile = tbCallCenterMobileBooking.Text,
                    CanncenterEmail = tbCallCenterEmailBooking.Text,

                    Is_Monday = "1",
                    Is_Tuesday = "1",
                    Is_Wednesday = "1",
                    Is_Thursday = "1",
                    Is_Friday = "1",
                    Is_Satday = "1",
                    Is_Sunday = "1",

                    Mon_StartTime = FormatTimeSpan("0"),
                    Tues_StartTime = FormatTimeSpan("0"),
                    Wed_StartTime = FormatTimeSpan("0"),
                    Thur_StartTime = FormatTimeSpan("0"),
                    Fri_StartTime = FormatTimeSpan("0"),
                    Sat_StartTime = FormatTimeSpan("0"),
                    Sun_StartTime = FormatTimeSpan("0"),


                    Mon_EndTime = FormatTimeSpan("0"),
                    Tues_EndTime = FormatTimeSpan("0"),
                    Wed_EndTime = FormatTimeSpan("0"),
                    Thur_EndTime = FormatTimeSpan("0"),
                    Fri_EndTime = FormatTimeSpan("0"),
                    Sat_EndTime = FormatTimeSpan("0"),
                    Sun_EndTime = FormatTimeSpan("0"),

                };

                List<HolidayDateModel> hollidays = ((DataTable)ViewState[holiday]).AsEnumerable().Select(c => new HolidayDateModel()
                {

                    Holiday_ID = c.Field<int>("Holiday_ID"),
                    Dealer_ID = c.Field<int>("Dealer_ID"),
                    Holiday_Date = c.Field<DateTime>("Holiday_Date")

                }).ToList();


                string user = Session["User"].ToString();
                if (int.Parse(dealer_duration_min) >= int.Parse(dealer_duration_max))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
                    ClientPopup(ModalType.Warning, "Please Choose Duration Min less than Duration Max");
                    return;
                }
                DealerService srv = new DealerService();
                if (lbType.Text == "Add")
                {
                    srv.AddDealerService(geo_id, dealer_name, dealer_code, branch_name, branch_code, address, office_hours, office_hours2, mobile, active, user, lat, lng, dealer_name_th, branch_name_th, address_th, office_hours_th, office_hours2_th, dealer_duration_min, dealer_duration_max, pin_map_icon_id, is_pickup_service, appointMent, testDrive, hollidays, booking, province_id);
                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdateDealerService(geo_id, dealer_name, dealer_code, branch_name, branch_code, address, office_hours, office_hours2, mobile, active, user, lbId.Text, lat, lng, dealer_name_th, branch_name_th, address_th, office_hours_th, office_hours2_th, dealer_duration_min, dealer_duration_max, pin_map_icon_id, is_pickup_service, appointMent, testDrive, hollidays, booking, province_id);
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

                DealerService srv = new DealerService();
                srv.DeleteDealer(id, user);

                BindGridData();
                ClientPopup(ModalType.Success, "Completed");
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
        protected void gvBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lbType.Text = "";
                lbId.Text = "";
                lbDuplicate.Text = "";
                tbDate.Text = "";
                tbDealerDurationMin.Text = "";
                tbDealerDurationMax.Text = "";
                lbUpload.Text = "";
                chkPickupService.Checked = false;
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvDealer.DataKeys != null)
                {
                    string id = gvDealer.DataKeys[index].Values["ID"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    DealerService srv = new DealerService();
                    DataRow row = srv.GetDealerById(id);
                    if (row != null)
                    {
                        tbDealerName.Text = row["DEALER_NAME"].ToString();
                        tbDealerCode.Text = row["DEALER_CODE"].ToString();
                        tbBranchName.Text = row["BRANCH_NAME"].ToString();
                        tbBranchCode.Text = row["BRANCH_CODE"].ToString();
                        tbAddress.Text = row["DEALER_ADDRESS"].ToString();
                        tbMobile.Text = row["DEALER_MOBILE"].ToString();
                        tbOffice_hours.Text = row["DEALER_OFFICE_HOURS"].ToString();
                        tbOffice_hours2.Text = row["DEALER_OFFICE_HOURS2"].ToString();
                        tbLat.Text = row["latitude"].ToString();
                        tbLng.Text = row["longitude"].ToString();

                        dlGeo2.SelectedValue = row["GEO_ID"].ToString();
                        dlActive.SelectedValue = row["ACTIVE"].ToString();
                        dlPinMapIcon.SelectedIndex = row["pin_map_icon_id"] != DBNull.Value ? int.Parse(row["pin_map_icon_id"].ToString()) : 0;
                        chkPickupService.Checked =bool.Parse(row["is_pickup_service"].ToString());

                        BindProvinceByRegion(dlGeo2.SelectedValue);
                        var province_id = row["province_id"].ToString();
                        var a = dlProvince.Items.FindByValue(province_id);
                        if (a != null)
                        {
                            dlProvince.SelectedValue = row["province_id"].ToString();
                        }
                        else
                        {
                            dlProvince.Items.Insert(0, new ListItem()
                            {
                                Selected = true,
                                Text = "",
                                Value = ""
                            });
                        }
                        tbDealerNameTH.Text = row["dealer_name_th"].ToString();
                        tbBranchNameTH.Text = row["branch_name_th"].ToString();
                        tbAddressTH.Text = row["dealer_address_th"].ToString();
                        tbOffice_hoursTH.Text = row["DEALER_OFFICE_HOURS_TH"].ToString();
                        tbOffice_hours2TH.Text = row["DEALER_OFFICE_HOURS2_TH"].ToString();
                        tbDealerDurationMin.Text = row["DURATION_MIN"].ToString();
                        tbDealerDurationMax.Text = row["DURATION_MAX"].ToString();
                        #region Appointment Service

                        DataRow drAppointment = srv.GetWorkingTimeByIdandService(id, "ServiceAppointment");
                        //Appointment
                        if (drAppointment != null)
                        {
                            tbCallCenterMobile.Text = drAppointment["CallCenterMobile"].ToString();
                            tbCallCenterEmail.Text = drAppointment["CallCenterEmail"].ToString();

                            chkMon.Checked = drAppointment["Is_Monday"].ToString().ToLower().Equals("true");
                            chkTue.Checked = drAppointment["Is_Tuesday"].ToString().ToLower().Equals("true");
                            chkWed.Checked = drAppointment["Is_Wednesday"].ToString().ToLower().Equals("true");
                            chkThu.Checked = drAppointment["Is_Thursday"].ToString().ToLower().Equals("true");
                            chkFri.Checked = drAppointment["Is_Friday"].ToString().ToLower().Equals("true");
                            chkSat.Checked = drAppointment["Is_Satday"].ToString().ToLower().Equals("true");
                            chkSun.Checked = drAppointment["Is_Sunday"].ToString().ToLower().Equals("true");

                            dlStartTimeMon.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Mon_StartTime"]);
                            dlStartTimeTue.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Tues_StartTime"]);
                            dlStartTimeWed.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Wed_StartTime"]);
                            dlStartTimeThu.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Thur_StartTime"]);
                            dlStartTimeFri.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Fri_StartTime"]);
                            dlStartTimeSat.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Sat_StartTime"]);
                            dlStartTimeSun.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Sun_StartTime"]);

                            dlEndTimeMon.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Mon_EndTime"]);
                            dlEndTimeTue.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Tues_EndTime"]);
                            dlEndTimeWed.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Wed_EndTime"]);
                            dlEndTimeThu.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Thur_EndTime"]);
                            dlEndTimeFri.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Fri_EndTime"]);
                            dlEndTimeSat.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Sat_EndTime"]);
                            dlEndTimeSun.SelectedValue = GetValueFromTimeSpan((TimeSpan)drAppointment["Sun_EndTime"]);

                        }
                        else
                        {
                            SetWorkingAppointmentTimeToDefault();
                        }
                        #endregion

                        #region Test Drive
                        DataRow drTestDrive = srv.GetWorkingTimeByIdandService(id, "TestDrive");
                        //TestDrive
                        if (drTestDrive != null)
                        {
                            tbCallCenterMobileTestDrive.Text = drTestDrive["CallCenterMobile"].ToString();
                            tbCallCenterEmailTestDrive.Text = drTestDrive["CallCenterEmail"].ToString();

                            chkTdMon.Checked = drTestDrive["Is_Monday"].ToString().ToLower().Equals("true");
                            chkTdTue.Checked = drTestDrive["Is_Tuesday"].ToString().ToLower().Equals("true");
                            chkTdWed.Checked = drTestDrive["Is_Wednesday"].ToString().ToLower().Equals("true");
                            chkTdThu.Checked = drTestDrive["Is_Thursday"].ToString().ToLower().Equals("true");
                            chkTdFri.Checked = drTestDrive["Is_Friday"].ToString().ToLower().Equals("true");
                            chkTdSat.Checked = drTestDrive["Is_Satday"].ToString().ToLower().Equals("true");
                            chkTdSun.Checked = drTestDrive["Is_Sunday"].ToString().ToLower().Equals("true");

                            dlStartTimeTdMon.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Mon_StartTime"]);
                            dlStartTimeTdTue.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Tues_StartTime"]);
                            dlStartTimeTdWed.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Wed_StartTime"]);
                            dlStartTimeTdThu.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Thur_StartTime"]);
                            dlStartTimeTdFri.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Fri_StartTime"]);
                            dlStartTimeTdSat.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Sat_StartTime"]);
                            dlStartTimeTdSun.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Sun_StartTime"]);

                            dlEndTimeTdMon.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Mon_EndTime"]);
                            dlEndTimeTdTue.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Tues_EndTime"]);
                            dlEndTimeTdWed.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Wed_EndTime"]);
                            dlEndTimeTdThu.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Thur_EndTime"]);
                            dlEndTimeTdFri.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Fri_EndTime"]);
                            dlEndTimeTdSat.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Sat_EndTime"]);
                            dlEndTimeTdSun.SelectedValue = GetValueFromTimeSpan((TimeSpan)drTestDrive["Sun_EndTime"]);
                        }
                        else
                        {
                            SetWorkingTestDriveTimeToDefault();
                        }
                        #endregion

                        #region Holiday                        
                        GetHolidayDBToSession();
                        #endregion

                        #region Booking                        
                        DataRow drBooking = srv.GetWorkingTimeByIdandService(id, "OnlineBooking");
                        if (drBooking != null)
                        {
                            tbCallCenterMobileBooking.Text = drBooking["CallCenterMobile"].ToString();
                            tbCallCenterEmailBooking.Text = drBooking["CallCenterEmail"].ToString();
                        }
                        #endregion
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
        private void BindGridData()
        {
            try
            {
                DealerService srv = new DealerService();

                gvDealer.DataSource = srv.GetDealersService(dlGeo.SelectedValue, dlIsPickServiceCriteria.SelectedValue);
                gvDealer.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGeo()
        {
            try
            {
                DealerService srv = new DealerService();
                dlGeo.DataSource = srv.GetGeoSearchServiceDealer();
                dlGeo.DataBind();

                dlGeo2.DataSource = srv.GetGeo();
                dlGeo2.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindPinMapIcon()
        {
            try
            {
                DealerService srv = new DealerService();
                using (DataTable pin = srv.GetPinMapIcon()) 
                {
                    dlPinMapIcon.DataSource = pin;
                    dlPinMapIcon.DataBind();

                    dlPinMapIcon.DataSource = pin;
                    dlPinMapIcon.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        private void BindHoliday()
        {
            try
            {
                if (ViewState[holiday] != null)
                {
                    var dt = ((DataTable)ViewState[holiday]).AsEnumerable().OrderBy(c => c.Field<DateTime>("Holiday_Date"));
                    dates = dt.Any() ? dt.CopyToDataTable() : (DataTable)ViewState[holiday];
                    ViewState[holiday] = dates;
                    gvHoliday.DataSource = dates;
                    gvHoliday.DataBind();
                }
                else
                {
                    dates = new System.Data.DataTable();
                    dates.Columns.Add(new DataColumn("Holiday_ID", typeof(int)));
                    dates.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                    dates.Columns.Add(new DataColumn("Holiday_Date", typeof(DateTime)));
                    ViewState[holiday] = dates;
                    gvHoliday.DataSource = dates;
                    gvHoliday.DataBind();
                }
                upGvHoliday.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindNewHoliday()
        {
            try
            {

                dates = new System.Data.DataTable();
                dates.Columns.Add(new DataColumn("Holiday_ID", typeof(int)));
                dates.Columns.Add(new DataColumn("Dealer_ID", typeof(int)));
                dates.Columns.Add(new DataColumn("Holiday_Date", typeof(DateTime)));
                ViewState[holiday] = dates;
                BindHoliday();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GetHolidayDBToSession()
        {
            DealerService srv = new DealerService();
            DataTable dtHoliday = srv.GetHolidayByDealerId(lbId.Text);
            //TestDrive
            if (dtHoliday != null)
            {
                ViewState[holiday] = dtHoliday;
                BindHoliday();
            }
            else
            {
                BindNewHoliday();
            }
        }
        private void BindStartEndTime()
        {
            try
            {
                List<LookupModel> list = new List<LookupModel>();
                for (int i = 0; i < 24; i++)
                {
                    TimeSpan time = new TimeSpan(i, 0, 0);
                    list.Add(new LookupModel() { Text = string.Format("{0}:{1}", time.Hours.ToString("00"), time.Minutes.ToString("00")), Value = i.ToString() });
                }
                List<string> days = new List<string>() { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

                //Appoint
                BindDropDownlist(dlStartTimeMon, list);
                BindDropDownlist(dlStartTimeTue, list);
                BindDropDownlist(dlStartTimeWed, list);
                BindDropDownlist(dlStartTimeThu, list);
                BindDropDownlist(dlStartTimeFri, list);
                BindDropDownlist(dlStartTimeSat, list);
                BindDropDownlist(dlStartTimeSun, list);

                BindDropDownlist(dlEndTimeMon, list);
                BindDropDownlist(dlEndTimeTue, list);
                BindDropDownlist(dlEndTimeWed, list);
                BindDropDownlist(dlEndTimeThu, list);
                BindDropDownlist(dlEndTimeFri, list);
                BindDropDownlist(dlEndTimeSat, list);
                BindDropDownlist(dlEndTimeSun, list);

                //Test Drive
                BindDropDownlist(dlStartTimeTdMon, list);
                BindDropDownlist(dlStartTimeTdTue, list);
                BindDropDownlist(dlStartTimeTdWed, list);
                BindDropDownlist(dlStartTimeTdThu, list);
                BindDropDownlist(dlStartTimeTdFri, list);
                BindDropDownlist(dlStartTimeTdSat, list);
                BindDropDownlist(dlStartTimeTdSun, list);

                BindDropDownlist(dlEndTimeTdMon, list);
                BindDropDownlist(dlEndTimeTdTue, list);
                BindDropDownlist(dlEndTimeTdWed, list);
                BindDropDownlist(dlEndTimeTdThu, list);
                BindDropDownlist(dlEndTimeTdFri, list);
                BindDropDownlist(dlEndTimeTdSat, list);
                BindDropDownlist(dlEndTimeTdSun, list);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindDropDownlist(DropDownList dl, List<LookupModel> list)
        {
            dl.DataSource = list;
            dl.DataTextField = "Text";
            dl.DataValueField = "Value";
            dl.DataBind();
        }
        protected void lkTab_Click(object sender, EventArgs e)
        {
            LinkButton lk = sender as LinkButton;
            lbTab.Text = lk.ID;
            CheckTab();
        }
        protected void gvHoliday_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //LinkButton lnkFull = e.Row.FindControl("gvBtnEdit") as LinkButton;
            //if (lnkFull != null)
            //{
            //    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
            //}
        }
        protected void gvHoliday_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvHoliday.DataKeys != null)
                    {
                        var date = (DateTime)gvHoliday.DataKeys[index].Values["Holiday_Date"];
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal();", true);
                        DataTable dt = (DataTable)ViewState[holiday];
                        dt.Rows.Remove(dt.AsEnumerable().Where(c => c.Field<DateTime>("Holiday_Date") == date).FirstOrDefault());
                        ViewState[holiday] = dt;
                        BindHoliday();
                        upGvHoliday.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvHoliday_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvHoliday.PageIndex = e.NewPageIndex;

                gvHoliday.DataSource = dates;
                gvHoliday.DataBind();
                upGvHoliday.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvHoliday_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvHoliday.SelectedIndex;
                if (gvHoliday.DataKeys != null)
                {
                    string id = gvHoliday.DataKeys[rowIndex].Values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void CheckTab()
        {
            if (lbTab.Text.Equals("lkHoliday"))
            {
                GetHolidayDBToSession();

            }
        }
        private void SetWorkingAppointmentTimeToDefault()
        {
            tbCallCenterMobile.Text = "";
            tbCallCenterEmail.Text = "";

            chkMon.Checked = true;
            chkTue.Checked = true;
            chkWed.Checked = true;
            chkThu.Checked = true;
            chkFri.Checked = true;
            chkSat.Checked = true;
            chkSun.Checked = true;

            dlStartTimeMon.SelectedIndex = 0;
            dlStartTimeTue.SelectedIndex = 0;
            dlStartTimeWed.SelectedIndex = 0;
            dlStartTimeThu.SelectedIndex = 0;
            dlStartTimeFri.SelectedIndex = 0;
            dlStartTimeSat.SelectedIndex = 0;
            dlStartTimeSun.SelectedIndex = 0;
            dlEndTimeMon.SelectedIndex = 0;
            dlEndTimeTue.SelectedIndex = 0;
            dlEndTimeWed.SelectedIndex = 0;
            dlEndTimeThu.SelectedIndex = 0;
            dlEndTimeFri.SelectedIndex = 0;
            dlEndTimeSat.SelectedIndex = 0;
            dlEndTimeSun.SelectedIndex = 0;
        }
        private void SetWorkingTestDriveTimeToDefault()
        {
            tbCallCenterMobileTestDrive.Text = "";
            tbCallCenterEmailTestDrive.Text = "";

            chkTdMon.Checked = true;
            chkTdTue.Checked = true;
            chkTdWed.Checked = true;
            chkTdThu.Checked = true;
            chkTdFri.Checked = true;
            chkTdSat.Checked = true;
            chkTdSun.Checked = true;

            dlStartTimeTdMon.SelectedIndex = 0;
            dlStartTimeTdTue.SelectedIndex = 0;
            dlStartTimeTdWed.SelectedIndex = 0;
            dlStartTimeTdThu.SelectedIndex = 0;
            dlStartTimeTdFri.SelectedIndex = 0;
            dlStartTimeTdSat.SelectedIndex = 0;
            dlStartTimeTdSun.SelectedIndex = 0;
            dlEndTimeTdMon.SelectedIndex = 0;
            dlEndTimeTdTue.SelectedIndex = 0;
            dlEndTimeTdWed.SelectedIndex = 0;
            dlEndTimeTdThu.SelectedIndex = 0;
            dlEndTimeTdFri.SelectedIndex = 0;
            dlEndTimeTdSat.SelectedIndex = 0;
            dlEndTimeTdSun.SelectedIndex = 0;
        }
        private string GetValueFromTimeSpan(TimeSpan time)
        {
            return time.Hours.ToString();
        }
        private string FormatTimeSpan(string time)
        {
            return string.Format("{0}:{1}:{2}", (Convert.ToInt32(time)).ToString("00"), "00", "00");
        }
        protected void btnAddDate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date;

                lbDuplicate.Text = "";
                if (DateTime.TryParseExact(tbDate.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out date))
                {
                    DateTime? newDate = date;
                    bool isDuplicate = ((DataTable)ViewState[holiday]).AsEnumerable().Where(row => row.Field<DateTime>("Holiday_Date") != null && row.Field<DateTime>("Holiday_Date") == newDate).Any();
                    if (!isDuplicate)
                    {
                        DataRow dr = dates.NewRow();
                        dr["Holiday_ID"] = 0;
                        dr["Dealer_ID"] = string.IsNullOrEmpty(lbId.Text) ? 0 : Convert.ToInt32(lbId.Text);
                        dr["Holiday_Date"] = date;
                        dates.Rows.Add(dr);

                    }
                    else
                    {
                        lbDuplicate.Text = "Duplicate Date!!";

                    }
                }
                else
                {
                    lbDuplicate.Text = "";

                }
                tbDate.Text = "";
                ViewState[holiday] = dates;
                BindHoliday();

                //upModalAdd.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }

        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                GetHolidayDBToSession();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "show", "ShowAddHoliday(true);", true);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }

        }
        protected void btnDownload_ServerClick(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(WebConfigurationManager.AppSettings["TemplatePath"] + "\\HolidayTemplate.xls");
            byte[] fileConent = File.ReadAllBytes(WebConfigurationManager.AppSettings["TemplatePath"] + "\\HolidayTemplate.xls");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(fileConent);
            HttpContext.Current.Response.End();

        }
        protected void tbDate_TextChanged(object sender, EventArgs e)
        {
            lbDuplicate.Text = "";
        }
    }
}