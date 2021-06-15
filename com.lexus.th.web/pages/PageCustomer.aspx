<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageCustomer.aspx.cs" Inherits="com.lexus.th.web.master.PageCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var dp = $('#<%=tbDeliveryDate.ClientID%>');
            dp.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
                language: "en",
                
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        });
        $(document).ready(function () {
            var dp = $('#<%=tbRSDateInit.ClientID%>');
            dp.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
                language: "en",

            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        });
        $(document).ready(function () {
            var dp = $('#<%=tbBirthdate.ClientID%>');
            dp.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
                language: "en",

            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        });
        $(document).ready(function () {
            var dp = $('#<%=tbBirthdateApp.ClientID%>');
            dp.datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
                language: "en",

            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        });
        $(document).ready(function () {
            var dp = $('#<%=tbSearchFrom.ClientID%>');
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
            var dp = $('#<%=tbSearchTo.ClientID%>');
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
        $(function () {
            $('#<%= chkSearchCreateDate.ClientID  %>').click(function (e) {
                var tb1 = $('#<%= tbSearchFrom.ClientID %>');
                var tb2 = $('#<%= tbSearchTo.ClientID %>');
                if (this.checked) {
                    tb1.removeAttr('disabled');
                    tb2.removeAttr('disabled');
                }
                else {
                    tb1.attr('disabled', true);
                    tb2.attr('disabled', true);
                }
            });
        });
        function isCharacterKeyPress(evt) {
            if ((event.charCode >= 48 && event.charCode <= 57) || (event.charCode >= 65 && event.charCode <= 90) || (event.charCode >= 97 && event.charCode <= 122))
                return true;
            return false;
        }
    </script>
    <script>
        $('#myTab a:first').tab('show');
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" EnablePageMethods="true">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Customers</h3>
        </div>
    </div>

    <div class="row">
        <div class="col-md-2">
            <label>Customer Search</label>
        </div>
        <div class="col-md-6">
            <div class="col-md-12">
                <%--<asp:TextBox ID="txtSearch" CssClass="form-control" runat="server" onkeydown="return (event.keyCode!=13)"></asp:TextBox>--%>
                <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <div class="col-md-12">
                    <div class="form-check">
                        <label class="form-check-label">
                            <asp:CheckBox runat="server" ID="chkSearchMemberId" /><span> Member ID</span>
                        </label>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="col-md-12">
                    <div class="form-check">
                        <label class="form-check-label">
                            <asp:CheckBox runat="server" ID="chkSearchFname" /><span> First Name</span>
                        </label>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="col-md-12">
                    <div class="form-check">
                        <label class="form-check-label">
                            <asp:CheckBox runat="server" ID="chkSearchMobile" /><span> Mobile Phone</span>
                        </label>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="col-md-12">
                    <div class="form-check">
                        <label class="form-check-label">
                            <asp:CheckBox runat="server" ID="chkSearchVin" /><span> VIN</span>
                        </label>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="col-md-4">
                    <div class="form-check">
                        <label class="form-check-label">
                            <asp:CheckBox runat="server" ID="chkSearchCreateDate" /><span> Create Date</span>
                            <%--                            <input type="checkbox" onclick="" /><span> Create Date</span>--%>
                        </label>
                    </div>
                </div>
                <div class="col-md-4">
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbSearchFrom" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect format</span></asp:RegularExpressionValidator>
                    <asp:TextBox ID="tbSearchFrom" CssClass="form-control" runat="server" placeholder="From" autocomplete="off"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbSearchTo" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect format</span></asp:RegularExpressionValidator>
                    <asp:TextBox ID="tbSearchTo" CssClass="form-control" runat="server" placeholder="To" autocomplete="off"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click" ValidationGroup="Search"><span class="glyphicon glyphicon-search"></span><span> Search</span></asp:LinkButton>
        </div>
    </div>

    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>

    <div class="row">
        <div class="col-md-3">
            <asp:LinkButton ID="btnAddModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModal_Click"><span class="glyphicon glyphicon-plus"></span><span> Customer</span></asp:LinkButton>
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

    <!-- DataGrid App User -->
    <div class="row">
        <div class="col-md-2">
            <label>App User</label>
        </div>
        <div class="col-lg-12">
            <div class="table-responsive">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvAppUser" runat="server" AllowPaging="true"
                            CssClass="table table-bordered table-hover" DataKeyNames="ID,MEMBERID"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvAppUser_PageIndexChanging"
                            OnSelectedIndexChanged="gvAppUser_SelectedIndexChanged" OnRowCommand="gvAppUser_RowCommand" OnRowDataBound="gvAppUser_RowDataBound">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="Add Car">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnAddCar" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-plus'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnAddCar_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEditApp" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEditApp_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnDeleteApp" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" CssClass="btn btn-danger btn-sm" OnClick="gvBtnDeleteApp_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub Menu">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="gvDlSubMenu" runat="server" CssClass="btn btn-primary" Width="35px" AutoPostBack="true" OnSelectedIndexChanged="gvDlSubMenu_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>--%>
                                <asp:BoundField HeaderText="ID" DataField="ID" Visible="false" />
                                <asp:BoundField HeaderText="MemberID" DataField="MEMBERID" />
                                <asp:BoundField HeaderText="Title" DataField="TITLENAME" />
                                <asp:BoundField HeaderText="Firstname" DataField="FNAME" />
                                <asp:BoundField HeaderText="Lastname" DataField="LNAME" />
                                <asp:BoundField HeaderText="E-mail" DataField="EMAIL" />
                                <asp:BoundField HeaderText="Mobile" DataField="MOBILE" />
                                <asp:BoundField HeaderText="CitizenID" DataField="SSN" />
                                <asp:BoundField HeaderText="Privilege" DataField="PRIVILEGE_CNT" />
                                <asp:BoundField HeaderText="Expiry" DataField="EXPIRY" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Status" DataField="DEL_FLAG_DISP" />
                                <%-- <asp:BoundField HeaderText="Gender" DataField="GENDER" />
                                <asp:BoundField HeaderText="Birthdate" DataField="BIRTHDATE" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Age" DataField="AGE" />
                                <asp:BoundField HeaderText="Address 1" DataField="ADDRESS1" />
                                <asp:BoundField HeaderText="Address 2" DataField="ADDRESS2" />
                                <asp:BoundField HeaderText="Sub-District" DataField="SUBDISTRICT" />
                                <asp:BoundField HeaderText="District" DataField="DISTRICT" />
                                <asp:BoundField HeaderText="Province" DataField="PROVINCE" />
                                <asp:BoundField HeaderText="Postcode" DataField="POSTALCODE" />--%>
                                <asp:BoundField HeaderText="CreateDate" DataField="CREATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="CreateBy" DataField="CREATE_USER" />
                                <asp:BoundField HeaderText="UpdateDate" DataField="UPDATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="UpdateBy" DataField="UPDATE_USER" />
                                <asp:BoundField HeaderText="DEL_FLAG" DataField="DEL_FLAG" Visible="false" />
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

    <!-- DataGrid Car Owner -->
    <div class="row">
        <div class="col-md-2">
            <label>Car Owner</label>
        </div>
        <div class="col-lg-12">
            <div class="table-responsive table-hover">
                <asp:UpdatePanel runat="server" ID="upCustGrid">
                    <ContentTemplate>
                        <asp:GridView ID="gvCust" runat="server" AllowPaging="true"
                            CssClass="table table-bordered table-hover" DataKeyNames="ID,MEMBERID"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvCust_PageIndexChanging"
                            OnSelectedIndexChanged="gvCust_SelectedIndexChanged" OnRowCommand="gvCust_RowCommand" OnRowDataBound="gvCust_RowDataBound">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <%--<asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEdit" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnDelete" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" CssClass="btn btn-danger btn-sm" OnClick="gvBtnDelete_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--                                <asp:TemplateField HeaderText="Change Log">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnChgLog" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-search'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnChgLog_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%--<asp:ButtonField  ButtonType="Link" HeaderText="Edit" CommandName="Detail" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" Visible="true" ControlStyle-CssClass="btn btn-primary btn-sm"></asp:ButtonField>--%>
                                <%--<asp:ButtonField ButtonType="Link" HeaderText="Cars" CommandName="ViewCar" Text="<i aria-hidden='true' class='glyphicon glyphicon-search'></i>" Visible="true" ControlStyle-CssClass="btn btn-info btn-sm"></asp:ButtonField>--%>
                                <%--<asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>--%>
                                <asp:BoundField HeaderText="ID" DataField="ID" Visible="false" />
                                <asp:BoundField HeaderText="MemberID" DataField="MEMBERID" />
                                <asp:BoundField HeaderText="Title" DataField="TITLENAME" />
                                <asp:BoundField HeaderText="Firstname" DataField="FNAME" />
                                <asp:BoundField HeaderText="Lastname" DataField="LNAME" />
                                <asp:BoundField HeaderText="E-mail" DataField="EMAIL" />
                                <asp:BoundField HeaderText="Mobile" DataField="MOBILE" />
                                <asp:BoundField HeaderText="CitizenID" DataField="SSN" />

                                <%--<asp:BoundField HeaderText="Display Name" DataField="NICKNAME" />
                                <asp:BoundField HeaderText="Gender" DataField="GENDER" />
                                <asp:BoundField HeaderText="Birthdate" DataField="BIRTHDATE" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Age" DataField="AGE" />
                                <asp:BoundField HeaderText="Address 1" DataField="ADDRESS1" />
                                <asp:BoundField HeaderText="Address 2" DataField="ADDRESS2" />
                                <asp:BoundField HeaderText="Sub-District" DataField="SUBDISTRICT" />
                                <asp:BoundField HeaderText="District" DataField="DISTRICT" />
                                <asp:BoundField HeaderText="Province" DataField="PROVINCE" />
                                <asp:BoundField HeaderText="Postcode" DataField="POSTALCODE" />
                                <asp:BoundField HeaderText="Home Phone" DataField="HOME_NO" />--%>

                                <asp:BoundField HeaderText="Privilege" DataField="PRIVILEGE_CNT" />
                                <asp:BoundField HeaderText="Expiry" DataField="EXPIRY" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Status" DataField="DEL_FLAG_DISP" />
                                <asp:BoundField HeaderText="Create Date" DataField="CREATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="CREATE_USER" />
                                <asp:BoundField HeaderText="Update Date" DataField="UPDATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Update By" DataField="UPDATE_USER" />
                                <asp:BoundField HeaderText="DEL_FLAG" DataField="DEL_FLAG" Visible="false" />
                                <%--<asp:BoundField HeaderText="Profile Image" DataField="PROFILE_IMAGE" />--%>
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
    <div class="row">
        <div class="col-md-3">
            <asp:LinkButton ID="btnCarModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnCarModal_Click" Visible="false"><span aria-hidden="true" class="glyphicon glyphicon-plus"></span><span> Car</span></asp:LinkButton>
        </div>
    </div>

    <!-- DataGrid Cars -->
    <div class="row">
        <div class="col-md-2">
            <label>Car</label>
        </div>
        <div class="col-lg-12">
            <div class="table-responsive">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvCar" runat="server" AllowPaging="true"
                            CssClass="table table-bordered table-hover" DataKeyNames="CUS_ID"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnRowCommand="gvCar_RowCommand" OnPageIndexChanging="gvCar_PageIndexChanging" OnRowDataBound="gvCar_RowDataBound">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEditCar" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEditCar_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Disable">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnDeleteCar" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" CssClass="btn btn-danger btn-sm" OnClick="gvBtnDeleteCar_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Change Log">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnChgLogCar" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-search'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnChgLogCar_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>--%>
                                <asp:BoundField HeaderText="ID" DataField="CUS_ID" />
                                <asp:BoundField HeaderText="VIN" DataField="VIN" />
                                <asp:BoundField HeaderText="Plate No" DataField="PLATE_NO" />
                                <asp:BoundField HeaderText="Dealer" DataField="DEALER" />
                                <asp:BoundField HeaderText="Branch" DataField="BRANCH_NAME" />
                                <asp:BoundField HeaderText="RSDate" DataField="RS_Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Model" DataField="MODEL" />
                                <asp:BoundField HeaderText="Status" DataField="DEL_FLAG_DISP" />

                                <%--<asp:BoundField HeaderText="Member ID" DataField="MEMBERID" />
                                <asp:BoundField HeaderText="Color" DataField="BODYCLR_NAME" />
                                <asp:BoundField HeaderText="Inactive" DataField="INACTIVE" />
                                <asp:BoundField HeaderText="Inactive Reason" DataField="INACTIVE_REASON" />
                                <asp:BoundField HeaderText="Customer Type" DataField="CUS_TYPE"/>
                                <asp:BoundField HeaderText="Ownership Type" DataField="OWNERSHIP_TYPE"/>--%>

                                <asp:BoundField HeaderText="Create Date" DataField="CREATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="CREATE_USER" />
                                <asp:BoundField HeaderText="Update Date" DataField="UPDATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Update By" DataField="UPDATE_USER" />
                                <asp:BoundField HeaderText="DEL_FLAG" DataField="DEL_FLAG" Visible="false" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

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

    <!-- Modal Check Initial -->
    <div id="mdCheckInitial" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalCheckInitial" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Check Car Initial</h4>
                        </div>
                        <div class="modal-body" onkeydown="return (event.keyCode!=13)">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lbClickType" Visible="false"></asp:Label>
                                                <label>Search VIN Initial</label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbSearchInitial" runat="server" ValidationGroup="SearchInitial"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbSearchInitial" autocomplete="off" MaxLength="17"></asp:TextBox><br />
                                                <asp:LinkButton ID="btnSearchInitial" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSearchInitial_Click" ValidationGroup="SearchInitial"><span class="glyphicon glyphicon-search"></span><span> Search</span></asp:LinkButton>
                                                <asp:LinkButton ID="btnInsertCarInitial" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnInsertCarInitial_Click"><span class="glyphicon glyphicon-plus"></span><span> Insert</span></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Data Initial -->
    <div id="mdDataInitial" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalDataInitial" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Car Initial</h4>
                        </div>
                        <div class="modal-body" onkeydown="return (event.keyCode!=13)">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                
                                                <label>Vin / เลขตัวถังรถยนต์<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbVinInit" runat="server" ValidationGroup="Confirm"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbVinInit" onkeypress="return IsNumberKey(event)" MaxLength="13" autocomplete="off"></asp:TextBox>
                                               <%-- <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbVinInit" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbVinInit" runat="server" ValidationGroup="Save" ValidationExpression="^[a-zA-Z0-9_.-]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect vin format</span></asp:RegularExpressionValidator>
                                                <asp:CustomValidator Display="Dynamic" ID="cvVin" runat="server" ValidationGroup="Save" OnServerValidate="cvVin_ServerValidate"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Duplicate Vin</span></asp:CustomValidator>--%>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Plate No / ตัวถังรถยนต์<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbPlateNoInit" runat="server" ValidationGroup="Confirm"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbPlateNoInit" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Dealer / ดีลเลอร์<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlDealerInit" runat="server" ValidationGroup="Confirm"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlDealerInit" AutoPostBack="true" DataTextField="display_th" DataValueField="id" OnSelectedIndexChanged="dlDealerInit_SelectedIndexChanged"></asp:DropDownList>
                                                <%--<asp:TextBox runat="server" CssClass="form-control" ID="tbDealer" autocomplete="off" ></asp:TextBox>--%>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Branch / สาขา<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlBranchInit" runat="server" ValidationGroup="Confirm"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlBranchInit" DataTextField="branch_th" DataValueField="branch_code"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Delivery Date / วันส่งมอบรถ <span style="color: red; font-weight: bold">*</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbRSDateInit" runat="server" ValidationGroup="Confirm"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbRSDateInit" runat="server" ValidationGroup="Confirm" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control input-append date" ID="tbRSDateInit" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Model / รุ่นรถยนต์<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlModelInit" runat="server" ValidationGroup="Confirm"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlModelInit" DataTextField="MODEL" DataValueField="MODEL_ID"></asp:DropDownList>
                                                <%--<asp:TextBox runat="server" CssClass="form-control" ID="tbModel" autocomplete="off" ></asp:TextBox>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="btn btn-primary btn-sm" OnClick="btnConfirm_Click" ValidationGroup="Confirm" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Add/Edit Customer -->
    <div id="mdAdd" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalAdd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Customer</h4>
                        </div>
                        <div class="modal-body" style="height: 500px; overflow-y: auto;" role="document" onkeydown="return (event.keyCode!=13)">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Member ID / รหัสสมาชิก<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMemberId" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:CustomValidator Display="Dynamic" ID="cvMemberId" runat="server" ValidationGroup="Save" OnServerValidate="cvMemberId_ServerValidate"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Duplicate Member ID</span></asp:CustomValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbMemberId" MaxLength="6" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                       
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row">
                                         <ul class="nav nav-tabs" id="myTab" role="tablist">
                                            <li class="nav-item">
                                                <a class="nav-link active" id="carowner-tab" data-toggle="tab" href="#carowner" role="tab" aria-controls="carowner" >Car Owner</a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" id="appuser-tab" data-toggle="tab" href="#appuser" role="tab" aria-controls="appuser" >App User</a>
                                            </li>
                                        </ul>
                                        <div class="tab-content">
                                            <div class="tab-pane active" id="carowner" role="tabpanel" aria-labelledby="carowner-tab">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                                        <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                                        <label>Title / คำนำหน้าชื่อ<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlTitle" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="dlTitle">
                                                            <asp:ListItem Text="Mr" Value="Mr" />
                                                            <asp:ListItem Text="Mrs" Value="Mrs" />
                                                            <asp:ListItem Text="Ms" Value="Ms" />
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Firstname / ชื่อจริง<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbFirstname" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbFirstname" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Lastname / นามสกุล<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbLastname" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbLastname" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Gender / เพศ<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlGener" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="dlGener">
                                                            <asp:ListItem Text="Male" Value="M" />
                                                            <asp:ListItem Text="Female" Value="F" />
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Birthdate / วันเกิด<span style="color: red; font-weight: bold"> </span></label>
<%--                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbBirthdate" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>--%>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbBirthdate" runat="server" ValidationGroup="Save" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control input-append date" ID="tbBirthdate" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Citizen ID / บัตรประชาชน<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbSSN" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbSSN" onkeypress="return IsNumberKey(event)" MaxLength="13" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Email / อีเมล์<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbEmail" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbEmail" runat="server" ValidationGroup="Save" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect email format</span></asp:RegularExpressionValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbEmail" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Mobile Number / เบอร์มือถือ<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMobile" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbMobile" runat="server" ValidationGroup="Save" ValidationExpression="^[0-9]{10}$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> please enter 10 digit number</span></asp:RegularExpressionValidator>
                                                        <%--<asp:CustomValidator Display="Dynamic" ID="cvMobile" runat="server" ValidationGroup="Save" OnServerValidate="cvMobile_ServerValidate"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Duplicate Mobile Number</span></asp:CustomValidator>--%>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbMobile" onkeypress="return IsNumberKey(event)" MaxLength="10" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Address / ที่อยู่<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbAddr1" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbAddr1" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Sub-District / แขวง / ตำบล<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlSubDistict" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <%--<asp:TextBox runat="server" CssClass="form-control" ID="tbSubDistict" autocomplete="off" ></asp:TextBox>--%>
                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="dlSubDistict" AutoPostBack="true" DataTextField="SUBDISTRICT_NAME_TH" DataValueField="SUBDISTRICT_ID" OnSelectedIndexChanged="dlSubDistict_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>District / เขต / อำเภอ<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlDistict" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <%--<asp:TextBox runat="server" CssClass="form-control" ID="tbDistict" autocomplete="off" ></asp:TextBox>--%>
                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="dlDistict" AutoPostBack="true" DataTextField="DISTRICT_NAME_TH" DataValueField="DISTRICT_ID" OnSelectedIndexChanged="dlDistict_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Province / จังหวัด<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlProvince" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <%--<asp:TextBox runat="server" CssClass="form-control" ID="tbProvince" ></asp:TextBox>--%>
                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="dlProvince" AutoPostBack="true" DataTextField="PROVINCE_NAME_TH" DataValueField="PROVINCE_ID" OnSelectedIndexChanged="dlProvince_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Postal Code / รหัสไปรษณีย์<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbPostCode" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbPostCode" runat="server" ValidationGroup="Save" ValidationExpression="^[0-9]{5}$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> please enter 5 digit number</span></asp:RegularExpressionValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbPostCode" onkeypress="return IsNumberKey(event)" MaxLength="5" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="appuser" role="tabpanel" aria-labelledby="appuser-tab">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <asp:Button ID="btDuplicate" runat="server" Text="Duplicate Profile from Car Owner" CssClass="btn btn-primary btn-sm" OnClick="btDuplicate_Click" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <asp:Label runat="server" ID="lbTypeApp" Visible="false"></asp:Label>
                                                        <asp:Label runat="server" ID="lbIdApp" Visible="false"></asp:Label>
                                                        <label>Title / คำนำหน้าชื่อ<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlTitleApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="dlTitleApp">
                                                            <asp:ListItem Text="Mr" Value="Mr" />
                                                            <asp:ListItem Text="Mrs" Value="Mrs" />
                                                            <asp:ListItem Text="Ms" Value="Ms" />
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Firstname / ชื่อจริง<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbFirstnameApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbFirstnameApp" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Lastname / นามสกุล<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbLastnameApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbLastnameApp" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Gender / เพศ<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlGenerApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="dlGenerApp">
                                                            <asp:ListItem Text="Male" Value="M" />
                                                            <asp:ListItem Text="Female" Value="F" />
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Birthdate / วันเกิด<span style="color: red; font-weight: bold"> </span></label>
<%--                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbBirthdateApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>--%>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbBirthdateApp" runat="server" ValidationGroup="Save" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control input-append date" ID="tbBirthdateApp" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Citizen ID / บัตรประชาชน<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbSSNApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator Display="Dynamic" ID="cvSSNApp" runat="server" ValidationGroup="Save" OnServerValidate="cvSSNApp_ServerValidate"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Duplicate Citizen ID</span></asp:CustomValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbSSNApp" onkeypress="return IsNumberKey(event)" MaxLength="13" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Email / อีเมล์<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbEmailApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbEmailApp" runat="server" ValidationGroup="Save" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect email format</span></asp:RegularExpressionValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbEmailApp" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Mobile Number / เบอร์มือถือ<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMobileApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbMobileApp" runat="server" ValidationGroup="Save" ValidationExpression="^[0-9]{10}$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> please enter 10 digit number</span></asp:RegularExpressionValidator>
                                                        <asp:CustomValidator Display="Dynamic" ID="cvMobileApp" runat="server" ValidationGroup="Save" OnServerValidate="cvMobileApp_ServerValidate"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Duplicate Mobile Number</span></asp:CustomValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbMobileApp" onkeypress="return IsNumberKey(event)" MaxLength="10" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Address / ที่อยู่<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbAddr1App" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbAddr1App" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Sub-District / แขวง / ตำบล<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlSubDistictApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <%--<asp:TextBox runat="server" CssClass="form-control" ID="tbSubDistictApp" autocomplete="off"></asp:TextBox>--%>
                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="dlSubDistictApp" AutoPostBack="true" DataTextField="SUBDISTRICT_NAME_TH" DataValueField="SUBDISTRICT_ID" OnSelectedIndexChanged="dlSubDistictApp_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>District / เขต / อำเภอ<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlDistictApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <%--<asp:TextBox runat="server" CssClass="form-control" ID="tbDistictApp" autocomplete="off"></asp:TextBox>--%>
                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="dlDistictApp" AutoPostBack="true" DataTextField="DISTRICT_NAME_TH" DataValueField="DISTRICT_ID" OnSelectedIndexChanged="dlDistictApp_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Province / จังหวัด<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlProvinceApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <%--<asp:TextBox runat="server" CssClass="form-control" ID="tbProvince" ></asp:TextBox>--%>
                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="dlProvinceApp" AutoPostBack="true" DataTextField="PROVINCE_NAME_TH" DataValueField="PROVINCE_ID" OnSelectedIndexChanged="dlProvinceApp_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Postal Code / รหัสไปรษณีย์<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbPostCodeApp" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbPostCodeApp" runat="server" ValidationGroup="Save" ValidationExpression="^[0-9]{5}$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> please enter 5 digit number</span></asp:RegularExpressionValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbPostCodeApp" onkeypress="return IsNumberKey(event)" MaxLength="5" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-12" runat="server" id="divRemarkCust" visible="false">
                                            <div class="form-group">
                                                <label runat="server" id="lbRemarkCust">Change Log<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:CustomValidator Display="Dynamic" ID="cvRemarkCust" runat="server" ValidationGroup="Save" OnServerValidate="cvRemarkCust_ServerValidate"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Require field</span></asp:CustomValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbRemarkCust" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" OnClick="btnSave_Click" ValidationGroup="Save" />
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
                                    <div class="form-group">
                                        <label>Reason / สาเหตุ<span style="color: red; font-weight: bold"> *</span></label>
                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbReasonDelCust" runat="server" ValidationGroup="DelCust"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbReasonDelCust" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <asp:Button ID="btnDelete" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnDelete_Click" ValidationGroup="DelCust" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Add/Edit Car -->
    <div id="mdAddCar" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalAddCar" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title"><asp:Label runat="server" ID="lbHeadCar" Text="" ></asp:Label></h4>
                        </div>
                        <div class="modal-body" onkeydown="return (event.keyCode!=13)">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lbCarId" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lbMemberId" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lbCarType" Visible="false"></asp:Label>
                                                <label>VIN / เลขตัวถังรถยนต์<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbVin" runat="server" ValidationGroup="SaveCar"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbVin" runat="server" ValidationGroup="SaveCar" ValidationExpression="^[a-zA-Z0-9_.-]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect vin format</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbVin" MaxLength="17" autocomplete="off" style='text-transform:uppercase' onkeypress="return isCharacterKeyPress(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Plate No / หมายเลขทะเบียนรถยนต์</label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbPlateNo" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Dealer / ดีลเลอร์<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlDealer" runat="server" ValidationGroup="SaveCar"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlDealer" AutoPostBack="true" DataTextField="display_th" DataValueField="id" OnSelectedIndexChanged="dlDealer_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Branch / สาขา<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlBranch" runat="server" ValidationGroup="SaveCar"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlBranch" DataTextField="branch_th" DataValueField="branch_code"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Delivery Date / วันส่งมอบรถ<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDeliveryDate" runat="server" ValidationGroup="SaveCar"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbDeliveryDate" runat="server" ValidationGroup="SaveCar" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect format</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control input-append date" ID="tbDeliveryDate" autocomplete="off"></asp:TextBox>
                                                <%--<asp:TextBox ID="txtDeliveryDate" CssClass="form-control dateInput" runat="server" BackColor="White" AutoPostBack="true" OnTextChanged="txtDeliveryDate_TextChanged"></asp:TextBox>--%>
                                                <%--<asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbDeliveryDate" runat="server" ValidationGroup="SaveCar" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbDeliveryDate" autocomplete="off"></asp:TextBox>--%>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Model / รุ่นรถยนต์<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlModel" runat="server" ValidationGroup="SaveCar"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlModel" DataValueField="MODEL_ID" DataTextField="MODEL" AutoPostBack="true" OnSelectedIndexChanged="dlModel_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnChangeLog" runat="server" Visible="false">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label runat="server" id="lbRemarkCar">Change Log<span style="color: red; font-weight: bold"> *</span></label>
                                                    <asp:CustomValidator Display="Dynamic" ID="cvRemarkCar" runat="server" ValidationGroup="SaveCar" OnServerValidate="cvRemarkCar_ServerValidate"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Require field</span></asp:CustomValidator>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbRemarkCar" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button ID="btnCarSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" OnClick="btnCarSave_Click" ValidationGroup="SaveCar" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Confirm to delete Car -->
    <div class="modal fade" id="mdDelCarConfirm" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upDelCarConfirm" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
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
                                <asp:Label runat="server" ID="lbDelCarId" Visible="false"></asp:Label>
                                <div class="col-md-2">
                                    <img class="img-responsive" src="../images/warning-3-xxl.png" width="100" height="100">
                                </div>
                                <div class="col-md-10">
                                    <label>Do you want to delete a selected record?</label>
                                    <div class="form-group">
                                        <label>Reason / สาเหตุ<span style="color: red; font-weight: bold"> *</span></label>
                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbReasonDelCar" runat="server" ValidationGroup="DelCar"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbReasonDelCar" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <asp:Button ID="btnCarDelete" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnCarDelete_Click" ValidationGroup="DelCar" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Confirm to delete App User  -->
    <div class="modal fade" id="mdDelConfirmApp" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModelDelApp" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
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
                                <asp:Label runat="server" ID="lbDelIdApp" Visible="false"></asp:Label>
                                <div class="col-md-2">
                                    <img class="img-responsive" src="../images/warning-3-xxl.png" width="100" height="100">
                                </div>
                                <div class="col-md-10">
                                    <label>Do you want to delete a selected record?</label>
                                    <div class="form-group">
                                        <label>Reason / สาเหตุ<span style="color: red; font-weight: bold"> *</span></label>
                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbReasonDelApp" runat="server" ValidationGroup="DelApp"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbReasonDelApp" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <asp:Button ID="btnDeleteApp" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnDeleteApp_Click" ValidationGroup="DelApp" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Change Log Customer -->
    <div class="modal fade" id="mdChgLogCust" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upMdChgLogCust" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Changelog Customer</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <asp:Label runat="server" ID="Label1" Visible="false"></asp:Label>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <%--<label>Changelog</label>--%>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbChgLogCust" TextMode="MultiLine" Rows="10" ReadOnly="true" autocomplete="off"></asp:TextBox>
                                    </div>
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

    <!-- Modal Change Log Car -->
    <div class="modal fade" id="mdChgLogCar" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="UpMdChgLogCar" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Changelog Car</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <asp:Label runat="server" ID="Label2" Visible="false"></asp:Label>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <%--<label>Changelog</label>--%>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbChgLogCar" TextMode="MultiLine" Rows="10" ReadOnly="true" autocomplete="off"></asp:TextBox>
                                    </div>
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

    <!-- Modal Change Log App -->
    <div class="modal fade" id="mdChgLogApp" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upMdChgLogApp" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Changelog App User</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <asp:Label runat="server" ID="Label4" Visible="false"></asp:Label>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <%--<label>Changelog</label>--%>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbChgLogApp" TextMode="MultiLine" Rows="10" ReadOnly="true" autocomplete="off"></asp:TextBox>
                                    </div>
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

    <!-- Modal Exist Customer -->
    <div class="modal fade" id="mdExistCust" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upMdExistCust" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
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
                                <asp:Label runat="server" ID="Label3" Visible="false"></asp:Label>
                                <div class="col-md-2">
                                    <img class="img-responsive" src="../images/warning-3-xxl.png" width="100" height="100">
                                </div>
                                <div class="col-md-10">
                                    <label>ลูกค้าคนนี้มีข้อมูลอยู่ในระบบแล้ว กรุณาติดต่อ Call Center เพื่อเปลี่ยนแปลงรายการ</label>
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

    <!-- Modal Confirm to extend -->
    <div class="modal fade" id="mdExtendConfirm" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upMdExtendConfirm" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
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
                                <asp:Label runat="server" ID="lbExtendCustId" Visible="false"></asp:Label>
                                <div class="col-md-2">
                                    <img class="img-responsive" src="../images/warning-3-xxl.png" width="100" height="100">
                                </div>
                                <div class="col-md-10">
                                    <label>Do you want to re-new this customer</label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <asp:Button ID="btnExtend" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnExtend_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Adjust -->
    <div id="mdAdjust" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upMdAdjust" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Adjustment</h4>
                        </div>
                        <div class="modal-body" onkeydown="return (event.keyCode!=13)">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Member ID / รหัสสมาชิก</label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbAdjMemberId" onkeypress="return IsNumberKey(event)" MaxLength="5" ReadOnly="true" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Title / คำนำหน้าชื่อ</label>
                                                <%--<asp:TextBox runat="server" CssClass="form-control" ID="tbTitle"></asp:TextBox>--%>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlAdjTitle" Enabled="false">
                                                    <asp:ListItem Text="Mr" Value="Mr" />
                                                    <asp:ListItem Text="Mrs" Value="Mrs" />
                                                    <asp:ListItem Text="Ms" Value="Ms" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Firstname / ชื่อจริง</label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbAdjFName" ReadOnly="true" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Lastname / นามสกุล</label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbAdjLName" ReadOnly="true" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Point / คะแนน<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbAdjPoint" runat="server" ValidationGroup="SaveAdj"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbAdjPoint" onkeypress="return IsNumberKey(event)" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button ID="btnAdj" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" OnClick="btnAdj_Click" ValidationGroup="SaveAdj" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Privilege Redemption -->
    <div id="mdPrivilege" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upMdPrivilege" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Privilege Redemption</h4>
                        </div>
                        <div class="modal-body" onkeydown="return (event.keyCode!=13)">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lbDisplayType" Visible="false"></asp:Label>
                                                <label>Member ID / รหัสสมาชิก</label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbPrivilegeMemberId" onkeypress="return IsNumberKey(event)" MaxLength="5" ReadOnly="true" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Privilege<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlPrivilege" runat="server" ValidationGroup="SavePrivilege"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlPrivilege" DataTextField="PRIVILEGE_NAME" DataValueField="PRIVILEGE_ID" AutoPostBack="true" OnSelectedIndexChanged="dlPrivilege_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Privilege CNT / สิทธิ์ของลูกค้าที่เหลืออยู่</label>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbPrivilegeCNT" onkeypress="return IsNumberKey(event)" MaxLength="5" ReadOnly="true" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Redeem Code<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlRedeemCode" runat="server" ValidationGroup="SaveRedeemCode"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlRedeemCode" DataTextField="REDEEM_CODE" DataValueField="PRIVILEGE_CODE_ID">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <%--<div class="col-md-6">
                                        <div class="form-group">
                                            <label>Push Notification / ส่งการแจ้งเตือน</label><br />
                                            <asp:CheckBox runat="server" ID="chkNotification" />
                                        </div>
                                    </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button ID="btnPrivilege" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnPrivilege_Click" ValidationGroup="SavePrivilege" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
