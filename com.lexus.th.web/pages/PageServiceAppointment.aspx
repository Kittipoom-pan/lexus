<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageServiceAppointment.aspx.cs" Inherits="com.lexus.th.web.master.PageServiceAppointment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            var dp = $('#<%=tbcriRegisterDateFrom.ClientID%>');
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
        $(document).ready(function () {
            var dp = $('#<%=tbcriRegisterDateTo.ClientID%>');
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
        $(document).ready(function () {
            var dp = $('#<%=tbcriAppointmentDateFrom.ClientID%>');
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
        $(document).ready(function () {
            var dp = $('#<%=tbcriAppointmentDateTo.ClientID%>');
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
        $(document).ready(function () {
            var dp = $('#<%=tbConfirmDate.ClientID%>');
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
        $(document).ready(function () {
            var dp = $('#<%=tbCancelDate.ClientID%>');
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
        $(document).ready(function () {
            var dp = $('#<%=tbPickupDate.ClientID%>');
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
        function CheckValidateConfirm() {
            debugger;
            var chk = document.getElementById('<%= chkCancel.ClientID%>');
            var confirmDate = document.getElementById('<%= tbConfirmDate.ClientID%>')
            var confirmTime = document.getElementById('<%= tbConfirmTime.ClientID%>')
            if (chk.checked) {
                ValidatorEnable(document.getElementById('<%= vfConfirmDate.ClientID%>'), false);
                ValidatorEnable(document.getElementById('<%= vfConfirmTime.ClientID%>'), false);
            }
            else {
                ValidatorEnable(document.getElementById('<%= vfConfirmDate.ClientID%>'), true);
                ValidatorEnable(document.getElementById('<%= vfConfirmTime.ClientID%>'), true);
                if (confirmDate.value == '' || confirmTime.value == '') {
                    return false;
                }
            }
            return true;
        }

    </script>
    <style>
        .form-control-criteria {
            height: 30px !important;
        }

        .form-group-modal {
            margin-bottom: 5px !important;
        }

        .hr-line {
            color: #D3D3D3;
            border-style: solid;
            border-width: 0.5px;
            display: block;
            margin: auto;
            /*margin-top:3px;
            margin-bottom:0px;*/
        }

        .padding-grid {
            padding-left: 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header"><i class="fa fa-phone"></i>Service Appointment</h3>
        </div>
    </div>

    <div class="row">
        <%--      <label class="col-md-2">Service Appointment Search</label>
        <div class="col-md-5">
            <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server" onkeydown="return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>
        </div>
        --%>
        <div class="row">
            <div class="col-md-2">
                <div class="form-group">
                    <label>Dealer</label>
                    <asp:DropDownList runat="server" CssClass="form-control form-control-criteria" ID="dlcriDealer" DataValueField="DEALER_ID" DataTextField="DEALER_NAME"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label>Type Of Service</label>
                    <asp:DropDownList runat="server" CssClass="form-control form-control-criteria" ID="dlcriTypeOfService" DataValueField="ID" DataTextField="NAME_EN"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label>Appointment Code</label>
                    <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbcriAppointmentCode"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label>Name</label>
                    <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbcriName"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label>Surename</label>
                    <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbcriSurname"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label>Mobile Number</label>

                    <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbcriMobileNumber"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                <div class="form-group">
                    <label>Status</label>
                    <asp:DropDownList runat="server" CssClass="form-control form-control-criteria" ID="dlcriStatus">
                        <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Waiting to Confirm" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Confirm" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Cancel" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Completed" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label>Register Date</label>
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbcriRegisterDateFrom" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>

                    <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbcriRegisterDateFrom" autocomplete="off"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label>To</label>
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbcriRegisterDateTo" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>

                    <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbcriRegisterDateTo" autocomplete="off"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label>Appointment Date</label>
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbcriAppointmentDateFrom" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>

                    <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbcriAppointmentDateFrom" autocomplete="off"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label>To</label>
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbcriAppointmentDateTo" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>

                    <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbcriAppointmentDateTo" autocomplete="off"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <label>Pickup Service</label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="dlcriIsPickService" DataValueField="ID" DataTextField="NAME_EN">
                    <asp:ListItem Text="All" Value="-1" />
                    <asp:ListItem Text="Yes" Value="1" />
                    <asp:ListItem Text="No" Value="0" />
                </asp:DropDownList>
            </div>
        </div>

        <div class="row">
            <div class="col-md-10"></div>
            <div class="col-md-2">
                <div class="col-md-6">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click" ValidationGroup="Search"><span class="glyphicon glyphicon-search"></span><span> Search</span></asp:LinkButton>
                </div>
                <div class="col-md-6">
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnExport_Click"><span class="glyphicon glyphicon-save"></span><span> Export</span></asp:LinkButton>
                </div>
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

    <!-- DataGrid -->
    <div class="row">
        <div class="col-md-12 padding-grid">
            <div class="table-responsive table-hover">
                <asp:UpdatePanel runat="server" ID="upCustGrid">
                    <ContentTemplate>
                        <asp:GridView ID="gvCust" runat="server" AllowPaging="true"
                            CssClass="table table-bordered table-hover" DataKeyNames="ID"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvCust_PageIndexChanging"
                            OnSelectedIndexChanged="gvCust_SelectedIndexChanged" OnRowCommand="gvCust_RowCommand" OnRowDataBound="gvCust_RowDataBound">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEdit" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- "ID"
                                       "CODE"
                                       "SERVICE_APPOINTMENT_TYPE"
                                       "NAME"
                                       "SURNAME"
                                       "MOBILE_NUMBER"
                                       "PLATE_NUMBER"
                                       "MODEL_NAME"
                                       "TYPE_OF_SERVICE_ID"
                                	   "TYPE_OF_SERVICE_NAME
                                       "DEALER_ID"
                                	   "DEALER_NAME"
                                       "REMARK"
                                       "APPOINTMENTDATE"
                                       "APPOINTMENTTIME"
                                       "CONFIRM_DATE"
                                       "IS_CANCEL"
                                       "CANCEL_DATE"
                                       "CANCEL_REASON"
                                       "FOOTER_REMARK_DESCRIPTION"
                                       "CREATED_DATE"
                                       "CREATED_USER"
                                       "UPDATED_DATE"
                                       "UPDATED_USER"
                                       "DELETED_FLAG"
                                       "DELETE_DATE"
                                       "DELETE_USER"
                                       "VEHICLE_NO"
                                       "STATUS_ID"
                                       "MEMBER_ID"                   --%>
                                <asp:BoundField DataField="ID" HeaderText="No." />
                                <asp:BoundField DataField="CODE" HeaderText="Appointment Code" />
                                <asp:BoundField DataField="CREATED_DATE" HeaderText="Register Date" DataFormatString="{0:dd/MM/yyyy  HH:mm:ss}" />
                                <asp:BoundField DataField="MEMBER_ID" HeaderText="Member ID" />
                                <asp:BoundField DataField="NAME" HeaderText="Firstname" />
                                <asp:BoundField DataField="SURNAME" HeaderText="Surname" />
                                <asp:BoundField DataField="MOBILE_NUMBER" HeaderText="Mobile" />
                                <asp:BoundField DataField="PLATE_NUMBER" HeaderText="Plate No." />
                                <asp:BoundField DataField="MODEL_NAME" HeaderText="Car Model" />
                                <asp:BoundField DataField="Type_Of_Service" HeaderText="Type Of Service" />
                                <asp:BoundField DataField="DEALER_NAME" HeaderText="Dealer Name" />
                                <asp:BoundField DataField="DEALER_MOBILE" HeaderText="Dealer Mobile" />
                                <asp:BoundField DataField="APPOINTMENTDATE" HeaderText="Appointment Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="APPOINTMENTTIME" HeaderText="Appointment Time" />
                                <asp:BoundField DataField="UPDATED_DATE" HeaderText="Update Date" DataFormatString="{0:dd/MM/yyyy  HH:mm:ss}" />
                                <asp:BoundField DataField="CONFIRM_DATE" HeaderText="Confirm Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="CONFIRM_TIME" HeaderText="Confirm Time" />
                                <asp:BoundField DataField="CANCEL_DATE" HeaderText="Cancel Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="CANCEL_REASON" HeaderText="Cancel Reason" />
                                <asp:BoundField DataField="STATUS_NAME" HeaderText="Status" />
                                <asp:TemplateField HeaderText="Pickup Service">
                                    <ItemTemplate>
                                        <%# (Eval("is_pickup_service").ToString().ToLower().Equals("true") ? "Yes" : "No") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="pickup_location" HeaderText="Pickup Location" />
                                <asp:BoundField DataField="pickup_Address" HeaderText="Pickup Address" />
                                <asp:BoundField DataField="pickup_location_detail" HeaderText="Pickup Location Detail" />
                                <asp:BoundField DataField="pickup_date" HeaderText="Pickup Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="pickup_time" HeaderText="Pickup Time" />

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

    <!-- Modal Add/Edit -->
    <div id="mdAdd" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalAdd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <asp:HiddenField ID="oldStatusID" runat="server" />
                        <div class="modal-header">
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Service Appointment Detail</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                    <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                    <label>Appointment Code :</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:Label runat="server" ID="lbAppontmentCode"></asp:Label>
                                </div>
                            </div>

                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Register Date :</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:Label runat="server" ID="lbRegisterDate"></asp:Label>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Member ID :</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:Label runat="server" ID="lbMemberID"></asp:Label>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>First name :</label><span style="color: red; font-weight: bold"> *</span>
                                </div>
                                <div class="col-md-7">
                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbFirstname" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbFirstname"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Surname :</label><span style="color: red; font-weight: bold"> *</span>
                                </div>
                                <div class="col-md-7">
                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbSurname" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbSurname"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Mobile :</label><span style="color: red; font-weight: bold"> *</span>
                                </div>
                                <div class="col-md-7">
                                    <asp:RegularExpressionValidator Display="Dynamic" runat="server" ControlToValidate="tbMobile" SetFocusOnError="True" ValidationGroup="Save" ValidationExpression="^[0-9]{10,10}$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange">Number 10 characters.</span></asp:RegularExpressionValidator>

                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMobile" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbMobile"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Plate No. :</label><span style="color: red; font-weight: bold"> *</span>
                                </div>
                                <div class="col-md-7">
                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlPlateNo" InitialValue="select" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="dlPlateNo" DataTextField="PLATE_NO" DataValueField="PLATE_NO" AutoPostBack="true" OnSelectedIndexChanged="dlPlateNo_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:HiddenField runat="server" ID="hdVIN" />
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Car Model :</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:Label runat="server" ID="lbCarModel"></asp:Label>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <asp:UpdatePanel runat="server" ID="upRPTypeOfService" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="col-md-4">

                                            <label>Type Of Service :</label><span style="color: red; font-weight: bold"> *</span>


                                            <div class="col-md-12">
                                                <asp:Label runat="server" ID="lbTypeOfserviceRequired"><span style="display: inline;"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color: orange"></span><span style="color: orange">Required field</span></span></asp:Label>

                                            </div>
                                        </div>


                                        <div class="col-md-7" id="dvTypeOfService">
                                            <asp:Repeater ID="RPTypeOfService" runat="server" OnItemDataBound="RPTypeOfService_ItemDataBound" OnItemCommand="RPTypeOfService_ItemCommand">
                                                <ItemTemplate>
                                                    <div class="form-group">
                                                        <asp:Label ID="lbTypeOfServiceID" Visible="false" runat="server"></asp:Label>
                                                        <asp:Button ID="btnCheck" CommandName="check" CommandArgument='<%#  Eval("ID") %>' runat="server" Text="☐" Height="30" Width="30" Font-Size="20pt" BorderStyle="None" BorderColor="White" BackColor="White" />
                                                        <asp:CheckBox ID="chkTypeOfService" Style="display: none;" AutoPostBack="true" Enabled="false" runat="server" Height="30px" Width="30px" /><asp:Label ID="lbTypeOfServiceName" runat="server"></asp:Label>
                                                        <asp:DropDownList ID="dlTypeOfServiceDetail" CssClass="form-control" DataTextField="NAME_EN" DataValueField="ID" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Dealer :</label><span style="color: red; font-weight: bold"> *</span>
                                </div>
                                <div class="col-md-7">
                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlDealer" InitialValue="0" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="dlDealer" DataTextField="DEALER_NAME" DataValueField="DEALER_ID"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Remark :</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:Label runat="server" ID="lbRemark"></asp:Label>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Pickup Service :</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:Label runat="server" ID="lbIsPickUpService"></asp:Label>
                                </div>
                            </div>
                            <hr class="hr-line" />
                            <div class="col-md-12" ></div>
                             <div class="col-md-12" ></div>
                            <div class="col-md-12" ></div>
                             <div class="col-md-12" ></div>
                            <div class="col-md-12" style="padding-left:0px !important;">
                                <asp:Panel ID="pnPickup" runat="server">

                                    <div class="row form-group"></div>
                                    <div class="row form-group-modal" >
                                        <div class="col-md-4">
                                            <label>Pickup Location</label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbPickupLocation" runat="server" ValidationGroup="Save" ValidationExpression="\-?(90|[0-8]?[0-9]\.[0-9]{0,13})\,\-?(180|(1[0-7][0-9]|[0-9]{0,2})\.[0-9]{0,13})"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect latitude and longitude format!!.</span></asp:RegularExpressionValidator>

                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbPickupLocation" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row form-group-modal">
                                        <div class="col-md-4">
                                            <label>Pickup Address</label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbPickupAddress" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row form-group-modal">
                                        <div class="col-md-4">
                                            <label>Pickup Location Detail</label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbPickupLocationDetail" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row form-group-modal">
                                        <div class="col-md-4">
                                            <label>Pickup Date :</label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbPickupDate" runat="server" ValidationGroup="Save" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                                            <%--<asp:RequiredFieldValidator ID="vfPickupDate" Display="Dynamic" ControlToValidate="tbPickupDate" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>--%>

                                            <div class='input-group date'>
                                                <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbPickupDate" autocomplete="off"></asp:TextBox>
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="col-md-3" style="padding-left: 0px !important;">
                                            <asp:DropDownList runat="server" CssClass="form-control" ID="dlPickupTime" DataTextField="start_hour" DataValueField="id" AutoPostBack="true" OnSelectedIndexChanged="dlPlateNo_SelectedIndexChanged"></asp:DropDownList>

                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Appoint Date :</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:Label runat="server" ID="lbAppointDate"></asp:Label>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Confirm Date :</label><span style="color: red; font-weight: bold"> *</span>
                                </div>
                                <div class="col-md-4">
                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbConfirmDate" runat="server" ValidationGroup="Save" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="vfConfirmDate" Display="Dynamic" ControlToValidate="tbConfirmDate" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>

                                    <div class='input-group date'>

                                        <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbConfirmDate" autocomplete="off"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                                <div class="col-md-3" style="padding-left: 0px !important;">

                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbConfirmTime" runat="server" ValidationGroup="Save" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect time format HH:mm</span></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="vfConfirmTime" Display="Dynamic" ControlToValidate="tbConfirmTime" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>

                                    <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbConfirmTime" autocomplete="off"></asp:TextBox>


                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <asp:CheckBox runat="server" CssClass="" ID="chkCancel" AutoPostBack="true"></asp:CheckBox>
                                    <label>Cancel</label>
                                </div>
                                <div class="col-md-7">

                                    <div class='input-group date'>
                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbCancelDate" runat="server" ValidationGroup="Save" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbCancelDate" autocomplete="off"></asp:TextBox>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>

                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>
                                        <asp:Label runat="server" ID="lbCancelReason" Text="Cancel Reason :"></asp:Label></label>
                                    <asp:UpdatePanel runat="server" ID="upCancel" UpdateMode="Conditional">
                                        <ContentTemplate>

                                            <div class="col-md-12">
                                                <asp:Label runat="server" ID="lbRequiredCancel"><span style="display: inline;"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color: orange"></span><span style="color: orange">Required field Cancel Date and Cancel Reason</span></span></asp:Label>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbCancelReason" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Status :</label><span style="color: red; font-weight: bold"> *</span>
                                </div>
                                <div class="col-md-7">
                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlStatus" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="dlStatus">
                                        <asp:ListItem Text="Waiting to Confirm" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Confirm" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Cancel" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Completed" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Call Center Remark :</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbCallCenterRemark" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>

                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" Text="Close" OnClick="btnCancel_Click" />
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" OnClientClick="CheckValidateConfirm();" OnClick="btnSave_Click" ValidationGroup="Save" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
