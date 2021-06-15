<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageReportCustomer.aspx.cs" Inherits="com.lexus.th.web.master.PageReportCustomer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var dp = $('#<%=tbStartDate.ClientID%>');
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
            var dp = $('#<%=tbEndDate.ClientID%>');
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
            <h3 class="page-header">Register Report</h3>
        </div>
    </div>

    <div class="row" style="width:800px;">
<%--        <div class="col-md-12">
            <div class="col-md-6">
                <label>Event</label>
                <asp:DropDownList runat="server" ID="dlEvent" CssClass="form-control" DataValueField="ID" DataTextField="TITLE"></asp:DropDownList>
            </div>
        </div>--%>

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
                <label>Create Date Start</label>
                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbStartDate" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                <asp:TextBox ID="tbStartDate" CssClass="form-control" runat="server" onkeydown="return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <label>Create Date End</label>
                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbEndDate" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                <asp:TextBox ID="tbEndDate" CssClass="form-control" runat="server" onkeydown="return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>
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
                                <asp:BoundField HeaderText="Create Date" DataField="Create Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Title" DataField="Title" />
                                <asp:BoundField HeaderText="Name" DataField="Name" />
                                <asp:BoundField HeaderText="Family Name" DataField="Family Name" />
                                <asp:BoundField HeaderText="Member ID" DataField="Member ID" />
                                <asp:BoundField HeaderText="Gender" DataField="Gender" />
                                <asp:BoundField HeaderText="Tel." DataField="Tel." />
                                <asp:BoundField HeaderText="E-mail" DataField="E-mail" />
                                <asp:BoundField HeaderText="Redeem" DataField="Redeem" />
                                <asp:BoundField HeaderText="Address" DataField="Address" />
                                <asp:BoundField HeaderText="Province" DataField="Province" />
                                <asp:BoundField HeaderText="District" DataField="DISTRICT" />
                                <asp:BoundField HeaderText="Sub District" DataField="SUBDISTRICT" />
                                <asp:BoundField HeaderText="Postcode" DataField="Postcode" />
                                <asp:BoundField HeaderText="Birthdate" DataField="Birthdate" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Car No." DataField="Car No." />
                                <asp:BoundField HeaderText="Vin" DataField="Vin" />
                                <asp:BoundField HeaderText="Rigister No." DataField="Rigister No." />
                                <asp:BoundField HeaderText="Model" DataField="MODEL" />
                                <asp:BoundField HeaderText="Color" DataField="Color" />
                                <asp:BoundField HeaderText="Dealer" DataField="Dealer" />
                                <asp:BoundField HeaderText="R/S Date" DataField="R/S Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Member Type" DataField="register_type" />
                                <asp:BoundField HeaderText="Last Login" DataField="LastLogin" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
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