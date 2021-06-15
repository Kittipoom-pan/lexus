<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageServiceTestDrive.aspx.cs" Inherits="com.lexus.th.web.master.PageServiceTestDrive" %>

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

        }); $(document).ready(function () {
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
            <h3 class="page-header"><i class="fa fa-car"></i>Test Drive</h3>
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
                    <asp:DropDownList runat="server" CssClass="form-control form-control-criteria" ID="dlcriDealer" DataValueField="dealer_id" DataTextField="display_th"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label>Preferred Model</label>
                    <asp:DropDownList runat="server" CssClass="form-control form-control-criteria" ID="dlcriPreferredModel" DataValueField="MODEL_ID" DataTextField="MODEL"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <label>Test Drive Code</label>
                    <asp:TextBox runat="server" CssClass="form-control form-control-criteria" ID="tbcriTestDriveCode"></asp:TextBox>
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

                                <asp:BoundField DataField="ID" HeaderText="No." />
                                <asp:BoundField DataField="CODE" HeaderText="Test Drive Code" />
                                <asp:BoundField DataField="CREATED_DATE" HeaderText="Register Date" DataFormatString="{0:dd/MM/yyyy  HH:mm:ss}" />
                                <asp:BoundField DataField="MEMBER_ID" HeaderText="Member ID" />
                                <asp:BoundField DataField="FIRSTNAME" HeaderText="Firstname" />
                                <asp:BoundField DataField="SURNAME" HeaderText="Surname" />
                                <asp:BoundField DataField="MOBILE_NUMBER" HeaderText="Mobile" />
                                <asp:BoundField DataField="EMAIL" HeaderText="Email" />
                                <asp:BoundField DataField="PREFERRED_MODEL" HeaderText="Preferred Model" />
                                <asp:BoundField DataField="DEALER_NAME" HeaderText="Dealer Name" />
                                <asp:BoundField DataField="DEALER_MOBILE" HeaderText="Dealer Mobile" />
                                <asp:BoundField DataField="PURCHASE_PLAN" HeaderText="Purchase Plan" />
                                <asp:BoundField DataField="CONFIRMED_DATE" HeaderText="Confirm Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="CONFIRMED_TIME" HeaderText="Confirm Time" />
                                <asp:BoundField DataField="UPDATED_DATE" HeaderText="Update Date" DataFormatString="{0:dd/MM/yyyy  HH:mm:ss}" />
                                <asp:BoundField DataField="CANCEL_DATE" HeaderText="Cancel Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="CANCEL_REASON" HeaderText="Cancel Reason" />
                                <asp:BoundField DataField="STATUS_NAME" HeaderText="Status" />

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
                    <asp:HiddenField ID="oldStatusID" runat="server" />
                    <div class="modal-content">
                        <div class="modal-header">
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Test Drive Detail</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                    <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                    <label>Test Drive Code :</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:Label runat="server" ID="lbTestDriveCode"></asp:Label>
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
                                    <label>Email :</label><span style="color: red; font-weight: bold"> *</span>
                                </div>
                                <div class="col-md-7">
                                    <asp:RegularExpressionValidator Display="Dynamic" runat="server" ControlToValidate="tbEmail" SetFocusOnError="True" ValidationGroup="Save" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Invalid Email</span></asp:RegularExpressionValidator>
                                                        
                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbEmail" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbEmail"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Preferred Model :</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlPreferredModel" InitialValue="0" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="dlPreferredModel" DataTextField="MODEL" DataValueField="MODEL_ID"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Dealer :</label><span style="color: red; font-weight: bold"> *</span><asp:Label id="lbNotSet" runat="server" ForeColor="red" Font-Bold="True" Text="(Not Set)"/>
                                </div>
                                <div class="col-md-7">
                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlDealer" InitialValue="0" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
<%--                                    <asp:DropDownList runat="server" CssClass="form-control" ID="dlDealer" DataTextField="DEALER_NAME" DataValueField="DEALER_ID"></asp:DropDownList>--%>
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="dlDealer" DataValueField="dealer_id" DataTextField="display_th">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Purchase Plan :</label><span style="color: red; font-weight: bold"> *</span>
                                </div>
                                <div class="col-md-7">
                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlPerchasePlan" InitialValue="0" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="dlPerchasePlan" DataTextField="NAME_EN" DataValueField="ID"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group-modal">
                                <div class="col-md-4">
                                    <label>Confirm Date :</label><span style="color: red; font-weight: bold"> *</span>
                                </div>
                                <div class="col-md-4">
                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbConfirmDate" runat="server" ValidationGroup="Save" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator Display="Dynamic" ID="vfConfirmDate" ControlToValidate="tbConfirmDate" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>

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
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" OnClick="btnClose_Click"/>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" OnClientClick="CheckValidateConfirm();" OnClick="btnSave_Click" ValidationGroup="Save" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
