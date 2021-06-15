<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageReportRedeem.aspx.cs" Inherits="com.lexus.th.web.master.PageReportRedeem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var dp = $('#<%=tbRedeemStartDate.ClientID%>');
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
            var dp = $('#<%=tbRedeemEndDate.ClientID%>');
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
            var dp = $('#<%=tbVerStartDate.ClientID%>');
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
            var dp = $('#<%=tbVerEndDate.ClientID%>');
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Redeem Report</h3>
        </div>
    </div>

    <div class="row" style="width:800px;">
        <div class="col-md-12">
            <div class="col-md-6">
                <label>Privilege Name</label>
                <asp:DropDownList runat="server" ID="dlPrivilege" CssClass="form-control" DataValueField="ID" DataTextField="TITLE"></asp:DropDownList>
            </div>
            <div class="col-md-6">
                <label>Member ID</label>
                <asp:TextBox ID="tbMemberID" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
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

        <div class="col-md-12">
            <div class="col-md-6">
                <label>Redeem Date Start</label>
                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbRedeemStartDate" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                <asp:TextBox ID="tbRedeemStartDate" CssClass="form-control" runat="server" onkeydown="return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <label>Redeem Date End</label>
                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbRedeemEndDate" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                <asp:TextBox ID="tbRedeemEndDate" CssClass="form-control" runat="server" onkeydown="return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>
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
        
        <div class="col-md-12">
            <div class="col-md-6">
                <label>Verify Date Start</label>
                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbVerStartDate" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                <asp:TextBox ID="tbVerStartDate" CssClass="form-control" runat="server" onkeydown="return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <label>Verify Date End</label>
                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbVerEndDate" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                <asp:TextBox ID="tbVerEndDate" CssClass="form-control" runat="server" onkeydown="return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>
            </div>
        </div>

        <div class="col-md-12">
            <div class="col-md-6">
                <label>Customer Name</label>
                <asp:TextBox ID="tbFullName" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <label>Mobile</label>
                <asp:TextBox ID="tbMobile" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
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

        <div class="col-md-12">
            <div class="col-md-6">
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click" ValidationGroup="Search"><span class="glyphicon glyphicon-search"></span><span> Search</span></asp:LinkButton>
                <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnSave_Click" ValidationGroup="Search"><span class="glyphicon glyphicon-download"></span><span> Save Report</span></asp:LinkButton>
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
        <div class="col-lg-12">
            <div class="table-responsive table-hover">
                <asp:UpdatePanel runat="server" ID="upCustGrid">
                    <ContentTemplate>
                        <asp:GridView ID="gvCust" runat="server" AllowPaging="true"
                            CssClass="table table-bordered table-hover"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvCust_PageIndexChanging"  >
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>                                
                                <asp:BoundField HeaderText="No." DataField="No." />
                                <asp:BoundField HeaderText="Title" DataField="Title" />
                                <asp:BoundField HeaderText="Name" DataField="Name" />
                                <asp:BoundField HeaderText="Family Name" DataField="Family Name" />
                                <asp:BoundField HeaderText="Member ID" DataField="Member ID" />
                                <asp:BoundField HeaderText="Tel." DataField="Tel." />
                                <asp:BoundField HeaderText="E-mail" DataField="E-mail" />
                                <asp:BoundField HeaderText="Privilege Name" DataField="Privilege Name" />
                                <asp:BoundField HeaderText="Date Redeem" DataField="Date Redeem" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Redeem Code" DataField="Redeem Code" />
                                <asp:BoundField HeaderText="Date Verify" DataField="Date Verify" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="VIN Number" DataField="Vin" />
                                <asp:BoundField HeaderText="Redeem Balance" DataField="Redeem Balance" />
                                <asp:BoundField HeaderText="Model" DataField="Model" />
                                <asp:BoundField HeaderText="Color" DataField="Color" />
                                <asp:BoundField HeaderText="Shop Name" DataField="SHOP_NM" />
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
                                    <img class="img-responsive" src="../images/close.png"  width="100" height="100">
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
</asp:Content>