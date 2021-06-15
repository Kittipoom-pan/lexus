<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageServiceDealer.aspx.cs" Inherits="com.lexus.th.web.pages.PageServiceDealer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var dp = $('#<%=tbDate.ClientID%>');
            dp.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
                language: "en"
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });

        });

        $("#other").show();
        function CheckTab(btn) {

            $("#other").hide()
            if (btn == 0) {
                $("#other").show();
            }
            else if (btn == 1) {
                $("#other").show();
            }
            else if (btn == 2) {

            }
            else if (btn == 3) {

            }
            else if (btn == 4) {

            }
            else if (btn == 5) {

            }
        }

<%--        function showBrowseDialog() {
            var fileuploadctrl = document.getElementById('<%= holidayUpload.ClientID %>');
            fileuploadctrl.click();
        }

        function upload() {

            var btn = document.getElementById('<%= hideButton.ClientID %>');
            btn.click();
        }--%>

        //function thisFileUpload() {
        //    document.getElementById("holidayUpload").click();
        //    //PageMethods.BindHoliday();
        //};


        ShowAddHoliday(true);
        function inputChange(e) {
            debugger;
            loading();
            var formData = new FormData();
            formData.append('file', e[0]);
            var lbUser = document.getElementById('lbUser');
            debugger;
            $.ajax({
                type: "POST",
                url: "UploadFileHolidayDate.ashx?user=" + lbUser.innerText,
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
                success: function (response) {
                    response = JSON.parse(response)
                    if (response.status) {
                        alert(response.message);
                        //$('.modal').modal('hide');
                        ShowAddHoliday(false);
                    }
                    else {
                        alert(response.message);
                    }
                    $("#uploadExcel").val(null);
                    removeLoading();
                },
                error: function (er) {

                }

            });

        }

        function loading() {
            // register on click event for buttons.          
            $.blockUI({
                css: {
                    theme: true,
                    baseZ: 2000,
                    backgroundColor: '#f00',
                    color: '#fff',
                    message: "Loading.."
                }
            });

        }
        function removeLoading() {
            $.unblockUI();
        }

        function ShowAddHoliday(isShow) {
            if (isShow) {
                $("#dvAddHolliday").show();
                $("#dvRefresh").hide()
                $("#dvSave").show();

            }
            else {
                $("#dvAddHolliday").hide();
                $("#dvRefresh").show()
                $("#dvSave").hide();
            }
        }

        function onLoadHolidayTemplate() {
            $.ajax({
                type: "POST",
                url: "PageAuthDealer.aspx/DownloadTemplate",
                data: '',
                headers: {
                    "cache-control": "no-cache",
                    "content-type": "application/json",
                    "charset": "utf-8"
                },
                dataType: "json",
                success: function (response) {

                },
                error: function (response) {

                }
            });
        }



    </script>

    <style>
        .label-dayHeader {
            font-size: 10pt;
            font-weight: 100;
            color: gray;
        }

        .label-day {
            font-size: 11pt;
            font-weight: bold;
        }

        .hr-line {
            color: dimgray;
            border-style: solid;
            border-width: 0.5px;
            display: block;
            margin: auto;
            /*margin-top:3px;
            margin-bottom:0px;*/
        }

        .bg-whitesmoke {
            background-color: whitesmoke;
            vertical-align: middle;
        }

        .form-control-ddl {
            height: 30px !important;
        }

        .form-group-day {
            margin-bottom: 40px;
            margin-top: 5px;
        }

        .upload-excel .input-file {
            width: 0;
            height: 0;
            opacity: 0;
            overflow: hidden;
            position: absolute;
            z-index: -1;
        }

        .upload-excel label {
            cursor: pointer;
            margin: 0;
        }

        .padding-holliday {
            padding-left: 5px !important;
            padding-right: 5px !important;
        }

        .margin-date-top {
            padding-top: 5px;
        }

        .btn-wide {
            width: 100%;
        }

        .lb-refreshdanger {
            color: red;
        }

        .chkPickup{
            padding-right:5px;
        }
        .padding-search
        {
            padding-top:25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Dealer Authorized Service Dealer</h3>
        </div>
    </div>

    <div class="row">
       
        <div class="col-md-4">
             <label>Regional</label>
            <asp:DropDownList runat="server" CssClass="form-control" ID="dlGeo" DataValueField="GEO_ID" DataTextField="GEO_NAME_EN">
            </asp:DropDownList>
        </div>
         
        <div class="col-md-2">
            <label>Pickup Service</label>
            <asp:DropDownList runat="server" CssClass="form-control" ID="dlIsPickServiceCriteria" DataValueField="ID" DataTextField="NAME_EN">
                 <asp:ListItem Text="All" Value="-1" />
                 <asp:ListItem Text="Yes" Value="1" />
                 <asp:ListItem Text="No" Value="0" />
            </asp:DropDownList>
        </div>
        <div class="col-md-2 padding-search">           
            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click"><span class="glyphicon glyphicon-search"></span><span> Search</span></asp:LinkButton>
        </div>
    </div>

    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>

    <div class="row">
        <div class="col-md-3">
            <asp:LinkButton ID="btnAddModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModal_Click"><span class="glyphicon glyphicon-plus"></span><span> Add Authorized Service Dealer</span></asp:LinkButton>
        </div>
    </div>

    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>

    <!-- DataGrid -->
    <div class="row">
        <div class="col-lg-12">
            <div class="table-responsive table-hover">
                <asp:UpdatePanel runat="server" ID="upCustGrid">
                    <ContentTemplate>
                        <asp:GridView ID="gvDealer" runat="server" AllowPaging="true"
                            CssClass="table table-bordered table-hover" DataKeyNames="ID" 
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvDealer_PageIndexChanging"
                            OnSelectedIndexChanged="gvDealer_SelectedIndexChanged" OnRowCommand="gvDealer_RowCommand" OnRowDataBound="gvDealer_RowDataBound">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEdit" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:ButtonField ButtonType="Link" HeaderText="Edit" CommandName="Detail" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" Visible="true" ControlStyle-CssClass="btn btn-primary btn-sm"></asp:ButtonField>--%>
                                <asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                <asp:BoundField HeaderText="DealerCode" DataField="DEALER_CODE" />
                                <asp:BoundField HeaderText="BranchCode" DataField="BRANCH_CODE" />
                                <asp:BoundField HeaderText="DealerNameTH" DataField="dealer_name_th" />
                                <asp:BoundField HeaderText="BranchNameTH" DataField="branch_name_th" />
                                <asp:BoundField HeaderText="Active" DataField="ACTIVE" />
                                <asp:BoundField HeaderText="Pin Map Icon" DataField="pin_map_icon" />
                                <%--<asp:BoundField HeaderText="Pickup Service" DataField="is_pickup_service" />--%>
                                <asp:TemplateField HeaderText="Pickup Service">
                                    <ItemTemplate>
                                        <%# (Eval("is_pickup_service").ToString().ToLower().Equals("true") ? "Yes" : "No") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Create Date" DataField="CREATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="CREATE_USER" />
                                <asp:BoundField HeaderText="Update Date" DataField="UPDATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Update By" DataField="UPDATE_USER" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>

    <!-- Modal Error -->
    <div class="modal fade" id="mdError" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalError" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Error</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-2">
                                    <img class="img-responsive" src="../images/close.png" width="100" height="100">
                                </div>
                                <div class="col-md-10">
                                    <asp:Label ID="lbErrMsg" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Success -->
    <div class="modal fade" id="mdSuccess" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalSuccess" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Information</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-2">
                                    <img class="img-responsive" src="../images/check.png" width="100" height="100">
                                </div>
                                <div class="col-md-10">
                                    <asp:Label ID="lbSuccess" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Warning -->
    <div class="modal fade" id="mdWarning" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalWarning" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Warning</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-2">
                                    <img class="img-responsive" src="../images/warning-3-xxl.png" width="100" height="100">
                                </div>
                                <div class="col-md-10">
                                    <asp:Label ID="lbWarning" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Confirm to delete -->
    <div class="modal fade" id="mdDelConfirm" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModelDel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Confirmation</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <asp:Label runat="server" ID="lbDelId" Visible="false"></asp:Label>
                                <div class="col-md-2">
                                    <img class="img-responsive" src="../images/warning-3-xxl.png" width="100" height="100">
                                </div>
                                <div class="col-md-10">
                                    <label>Do you want to delete a selected record?</label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <asp:Button ID="btnDelete" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnDelete_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Add/Edit -->
    <div id="mdAdd" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalAdd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Authorized Dealers</h4>
                        </div>
                        <div class="modal-body" role="document">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lbTab" Visible="false"></asp:Label>

                                                <label>Regional<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlGeo2" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlGeo2" DataValueField="GEO_ID" DataTextField="GEO_NAME_EN" OnSelectedIndexChanged="dlRegion_SelectedIndexChanged" AutoPostBack="true" >
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                         <div class="col-md-12">
                                            <div class="form-group">
                                                <label>Province<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlProvince" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlProvince" DataValueField="id" DataTextField="name" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Dealer Code<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDealerCode" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbDealerCode"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Branch Code</label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbBranchCode"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-6">

                                            <div class="form-group">
                                                <label>Active</label>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlActive">
                                                    <asp:ListItem Text="Active" Value="1" />
                                                    <asp:ListItem Text="Inactive" Value="0" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Pin Map Icon</label>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlPinMapIcon" DataValueField="ID" DataTextField="NAME_EN">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">                                                
                                                 <asp:CheckBox runat="server" CssClass="chkPickup" ID="chkPickupService" AutoPostBack="true"></asp:CheckBox><label>Can Pickup Service</label>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                        </div>

                                        <ul class="nav nav-tabs" id="myTab" role="tablist">
                                            <li class="nav-item">
                                                <asp:LinkButton class="nav-link" runat="server" ID="lkTH" OnClick="lkTab_Click" OnClientClick="CheckTab(0)" data-toggle="tab" href="#th" role="tab" aria-controls="th" aria-selected="true">TH</asp:LinkButton>

                                            </li>
                                            <li class="nav-item">
                                                <asp:LinkButton class="nav-link" runat="server" ID="lkEN" OnClick="lkTab_Click" OnClientClick="CheckTab(1)" data-toggle="tab" href="#en" role="tab" aria-controls="en" aria-selected="false">EN</asp:LinkButton>

                                            </li>
                                            <li class="nav-item">
                                                <asp:LinkButton class="nav-link" runat="server" ID="lkAppointment" OnClick="lkTab_Click" OnClientClick="CheckTab(2)" data-toggle="tab" href="#appointment" role="tab" aria-controls="appointment" aria-selected="false">Appointment</asp:LinkButton>

                                            </li>
                                            <li class="nav-item">
                                                <asp:LinkButton class="nav-link" runat="server" ID="lkTestDrive" OnClick="lkTab_Click" OnClientClick="CheckTab(3)" data-toggle="tab" href="#testdrive" role="tab" aria-controls="testdrive" aria-selected="false">Test Drive</asp:LinkButton>

                                            </li>
                                            <li class="nav-item">
                                                <asp:LinkButton class="nav-link" runat="server" ID="lkHoliday" OnClick="lkTab_Click" OnClientClick="CheckTab(4)" data-toggle="tab" href="#holiday" role="tab" aria-controls="holiday" aria-selected="false">Holiday</asp:LinkButton>

                                            </li>

                                            <li class="nav-item">
                                                <asp:LinkButton class="nav-link" runat="server" ID="lkBooking" OnClick="lkTab_Click" OnClientClick="CheckTab(5)" data-toggle="tab" href="#booking" role="tab" aria-controls="booking" aria-selected="false">Booking</asp:LinkButton>

                                            </li>
                                        </ul>
                                        <!-- Tab panes -->
                                        <div class="tab-content">
                                            <div class="tab-pane active" id="th" role="tabpanel" aria-labelledby="lkTH">
                                                <%--<asp:Panel runat="server" ID="pnTH">--%>
                                                <div class="col-md-12">
                                                    <div class="form-group"></div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Dealer Name TH<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDealerNameTH" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbDealerNameTH"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Branch Name TH</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbBranchNameTH"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Address TH</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbAddressTH" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Business Hours TH</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbOffice_hoursTH"></asp:TextBox><br />
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbOffice_hours2TH"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <%--</asp:Panel>--%>
                                            </div>
                                            <div class="tab-pane" id="en" role="tabpanel" aria-labelledby="lkEN">
                                                <%--<asp:Panel runat="server" ID="pnEN">--%>
                                                <div class="col-md-12">
                                                    <div class="form-group"></div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Dealer Name EN<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDealerName" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbDealerName"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Branch Name EN</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbBranchName"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Address EN</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbAddress" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Business Hours EN</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbOffice_hours"></asp:TextBox><br />
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbOffice_hours2"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <%--</asp:Panel>--%>
                                            </div>
                                            <div class="tab-pane" id="appointment" role="tabpanel" aria-labelledby="lkAppointment">
                                                <%--<asp:Panel runat="server" ID="pnAppointment">--%>
                                                <div class="col-md-12">
                                                    <div class="form-group"></div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Call Center Mobile<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbCallCenterMobile" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbCallCenterMobile" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Call Center Email<span style="color: red; font-weight: bold"> *</span></label>
                                                        <%--<asp:RegularExpressionValidator Display="Dynamic" runat="server" ControlToValidate="tbCallCenterEmail" SetFocusOnError="True" ValidationGroup="Save" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Invalid Email</span></asp:RegularExpressionValidator>--%>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbCallCenterEmail" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbCallCenterEmail" autocomplete="off" TextMode="MultiLine"></asp:TextBox>

                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Min Duration<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RangeValidator Display="Dynamic" ValidationGroup="Save" ControlToValidate="tbDealerDurationMin" MinimumValue="0" MaximumValue="999" runat="server" Type="Integer"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Please enter value between 0-999.</span></asp:RangeValidator>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDealerDurationMin" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" runat="server" ControlToValidate="tbDealerDurationMin" SetFocusOnError="True" ValidationGroup="Save" ValidationExpression="^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Invalid Number</span></asp:RegularExpressionValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbDealerDurationMin"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Max Duration<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RangeValidator Display="Dynamic" ValidationGroup="Save" ControlToValidate="tbDealerDurationMax" MinimumValue="0" MaximumValue="999" runat="server" Type="Integer"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Please enter value between 0-999.</span></asp:RangeValidator>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDealerDurationMax" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" runat="server" ControlToValidate="tbDealerDurationMax" SetFocusOnError="True" ValidationGroup="Save" ValidationExpression="^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Invalid Number</span></asp:RegularExpressionValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbDealerDurationMax"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-12"></div>
                                                </div>
                                                <div class="col-md-12">
                                                    <hr class="hr-line" />
                                                    <div class="col-md-12 bg-whitesmoke">

                                                        <div class="form-group form-group-day">

                                                            <div class="col-md-3">
                                                                <label class="label-dayHeader">Open</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-dayHeader">Working Day</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-dayHeader">Start Time</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-dayHeader">End Time</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <hr class="hr-line" />
                                                    <div class="col-md-12">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkMon"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">MonDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeMon"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeMon"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 bg-whitesmoke">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkTue"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">TuesDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeTue"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeTue"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkWed"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">WednesDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeWed"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeWed"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 bg-whitesmoke">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkThu"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">ThursDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeThu"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeThu"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkFri"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">FriDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeFri"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeFri"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 bg-whitesmoke">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkSat"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">SaturDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeSat"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeSat"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkSun"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">SunDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeSun"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeSun"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <hr class="hr-line" />

                                                </div>
                                                <%--</asp:Panel>--%>
                                            </div>
                                            <div class="tab-pane" id="testdrive" role="tabpanel" aria-labelledby="lkTestDrive">
                                                <%--<asp:Panel runat="server" ID="pnTestDrive">--%>
                                                <div class="col-md-12">
                                                    <div class="form-group"></div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Call Center Mobile (Test Drive)<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbCallCenterMobileTestDrive" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbCallCenterMobileTestDrive" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Call Center Email (Test Drive)<span style="color: red; font-weight: bold"> *</span></label>
                                                        <%--<asp:RegularExpressionValidator Display="Dynamic" runat="server" ControlToValidate="tbCallCenterEmailTestDrive" SetFocusOnError="True" ValidationGroup="Save" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Invalid Email</span></asp:RegularExpressionValidator>--%>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbCallCenterEmailTestDrive" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbCallCenterEmailTestDrive" autocomplete="off" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-12"></div>
                                                </div>
                                                <div class="col-md-12">
                                                    <hr class="hr-line" />
                                                    <div class="col-md-12 bg-whitesmoke">

                                                        <div class="form-group form-group-day">

                                                            <div class="col-md-3">
                                                                <label class="label-dayHeader">Open</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-dayHeader">Working Day</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-dayHeader">Start Time</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-dayHeader">End Time</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <hr class="hr-line" />
                                                    <div class="col-md-12">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkTdMon"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">MonDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeTdMon"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeTdMon"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 bg-whitesmoke">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkTdTue"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">TuesDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeTdTue"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeTdTue"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkTdWed"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">WednesDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeTdWed"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeTdWed"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 bg-whitesmoke">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkTdThu"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">ThursDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeTdThu"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeTdThu"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkTdFri"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">FriDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeTdFri"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeTdFri"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 bg-whitesmoke">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkTdSat"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">SaturDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeTdSat"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeTdSat"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">

                                                        <div class="form-group form-group-day">
                                                            <div class="col-md-3">
                                                                <asp:CheckBox runat="server" ID="chkTdSun"></asp:CheckBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="label-day">SunDay</label>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlStartTimeTdSun"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-ddl" ID="dlEndTimeTdSun"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <hr class="hr-line" />

                                                </div>
                                                <%--</asp:Panel>--%>
                                            </div>
                                            <div class="tab-pane" id="holiday" role="tabpanel" aria-labelledby="lkHoliday">
                                                <%--<asp:Panel runat="server" ID="pnHoliday">--%>

                                                <div class="col-md-12">
                                                    <div class="form-group"></div>
                                                </div>

                                                <div id="dvAddHolliday" class="col-md-7">
                                                    <div class="form-group">
                                                        <div class="col-md-2 margin-date-top">
                                                            <label>Date</label>
                                                        </div>
                                                        <div class="col-md-6 padding-holliday">
                                                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbDate" runat="server" ValidationGroup="Save" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>

                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbDate" autocomplete="off" OnTextChanged="tbDate_TextChanged"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-4 padding-holliday">
                                                            <asp:Button runat="server" CssClass="form-control" ID="btnAddDate" Text="Add" OnClick="btnAddDate_Click"></asp:Button>
                                                        </div>

                                                    </div>
                                                </div>
                                                <div id="dvRefresh" style="display: none;" class="col-md-7">
                                                    <div class="form-group">

                                                        <div class="col-md-12 padding-holliday">
                                                            <asp:Button runat="server" CssClass="btn btn-danger btn-wide" ID="btnRefresh" Text="Refresh Data Holiday" OnClick="btnRefresh_Click"></asp:Button>
                                                        </div>
                                                        <div class="col-md-12 padding-holliday">
                                                            <asp:Label runat="server" CssClass="lb-refreshdanger" ID="lbRefresh" Text="*** Please refresh for get new data."></asp:Label>
                                                        </div>

                                                    </div>
                                                </div>
                                                <%--    <asp:UpdatePanel runat="server" ID="upUpload" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <ContentTemplate>--%>
                                                <div class="col-md-5">
                                                    <div class="form-group">



                                                        <div class="col-md-6 padding-holliday">
                                                            <%--<input type="file" id="holidayUpload" style="display: none;" />--%>

                                                            <%--  <asp:FileUpload ID="holidayUpload" Style="display: none" runat="server" onchange="upload()" />
                                                                    <input type="button" id="UploadButton" class="btn btn-success btn-sm" value="Upload" onclick="showBrowseDialog()" />
                                                                    <asp:Button runat="server" ID="hideButton" type="button" Text="" Style="display: none;" OnClick="UploadButton_Click" />--%>
                                                            <span class="upload-excel">
                                                                <input type="file" id="uploadExcel" class="input-file"
                                                                    accept="application/vnd.ms-excel"
                                                                    onchange="inputChange(event.target.files)" />
                                                                <label for="uploadExcel" class="btn btn-success">
                                                                    <span class="glyphicon glyphicon-plus"></span>
                                                                    <span>Upload</span>
                                                                </label>
                                                            </span>

                                                        </div>
                                                        <div class="col-md-6 padding-holliday">
                                                            <span class="upload-excel">
                                                                <label class="btn btn-warning">
                                                                    <span class="glyphicon glyphicon-save"></span>
                                                                    <span>Template</span>
                                                                    <input id="btnDownload" runat="server" type="button" class="input-file" onserverclick="btnDownload_ServerClick" />
                                                                </label>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>

                                                <asp:UpdatePanel runat="server" ID="upGvHoliday" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <ContentTemplate>

                                                        <div class="col-md-7">
                                                            <div class="form-group">
                                                                <div class="col-md-12">
                                                                    <asp:Label runat="server" ID="lbDuplicate" ForeColor="#CC0000"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-5">
                                                            <div class="form-group">
                                                                <div class="col-md-12">
                                                                    <asp:Label runat="server" ID="lbUpload" ForeColor="#CC0000"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-lg-12">
                                                                <div class="table-responsive table-hover">

                                                                    <asp:GridView ID="gvHoliday" runat="server" AllowPaging="true"
                                                                        CssClass="table table-bordered table-hover" DataKeyNames="Holiday_Date"
                                                                        ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                                                                        ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvHoliday_PageIndexChanging"
                                                                        OnSelectedIndexChanged="gvHoliday_SelectedIndexChanged" OnRowCommand="gvHoliday_RowCommand" OnRowDataBound="gvHoliday_RowDataBound">
                                                                        <PagerStyle CssClass="pagination-ys" />
                                                                        <Columns>

                                                                            <asp:BoundField HeaderText="Holiday" DataField="Holiday_Date" DataFormatString="{0:dd MMM yyyy}" />
                                                                            <asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>

                                                                        </Columns>
                                                                    </asp:GridView>


                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="tab-pane" id="booking" role="tabpanel" aria-labelledby="lkBooking">
                                                <%--<asp:Panel runat="server" ID="pnBooking">--%>
                                                <div class="col-md-12">
                                                    <div class="form-group"></div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Call Center Mobile (Booking)<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbCallCenterMobileBooking" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbCallCenterMobileBooking" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Call Center Email<span style="color: red; font-weight: bold"> *</span></label>
                                                        <%--<asp:RegularExpressionValidator Display="Dynamic" runat="server" ControlToValidate="tbCallCenterEmail" SetFocusOnError="True" ValidationGroup="Save" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Invalid Email</span></asp:RegularExpressionValidator>--%>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbCallCenterEmailBooking" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbCallCenterEmailBooking" autocomplete="off" TextMode="MultiLine"></asp:TextBox>

                                                    </div>
                                                </div>
                                                
                                                <%--</asp:Panel>--%>
                                            </div>

                                            <div id="other">
                                                <div class="form-group">
                                                    <div class="col-md-12"></div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Latitude (กรอก 0 ถ้าไม่มี Location)<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbLat" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <%--<asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbLat" runat="server" ValidationGroup="Save" ValidationExpression="^(\+|-)?(?:90(?:(?:\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,6})?))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> format mismatch</span></asp:RegularExpressionValidator>--%>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbLat" onkeypress="return onlyNumbersWithDot(event)"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Tel</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbMobile"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Longitude (กรอก 0 ถ้าไม่มี Location)<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbLng" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <%--<asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbLng" runat="server" ValidationGroup="Save" ValidationExpression="^(\+|-)?(?:180(?:(?:\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,6})?))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> format mismatch</span></asp:RegularExpressionValidator>--%>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbLng" onkeypress="return onlyNumbersWithDot(event)"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="dvSave" class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" OnClick="btnSave_Click" ValidationGroup="Save" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnDownload" />
                    <asp:PostBackTrigger ControlID="lkHoliday"></asp:PostBackTrigger>
                </Triggers>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lkHoliday" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
