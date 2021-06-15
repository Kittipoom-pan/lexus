<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageNotification.aspx.cs" Inherits="com.lexus.th.web.master.PageNotification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Notification</h3>
        </div>
    </div>

    <div class="row" style="width:800px;">
        <div class="col-md-12">
            <div class="col-md-12">
                <label>Destination<span style="color:red; font-weight:bold"> *</span></label>
                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlDestination" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                <asp:DropDownList runat="server" ID="dlDestination" ValidationGroup="Send" Width="150px" Height="34px" AutoPostBack="true" OnSelectedIndexChanged="dlDestination_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlSubDestination" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                <asp:DropDownList runat="server" ID="dlSubDestination" Visible="false" ValidationGroup="Send" Width="150px" Height="34px" AutoPostBack="true" OnSelectedIndexChanged="dlSubDestination_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>

        <asp:Panel ID="pnMemberID" runat="server" Visible="false">
            <div class="col-md-12">
                <div class="col-md-12">
                    <label>Member ID<span style="color: red; font-weight: bold"> *</span></label>
                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMemberID" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                    <asp:TextBox ID="tbMemberID" CssClass="form-control" runat="server" ValidationGroup="Send" autocomplete="off"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnMemberGroup" runat="server" Visible="false">
            <div class="col-md-12">
                <div class="col-md-12">
                    <label>Member Group<span style="color: red; font-weight: bold"> *</span></label>
                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMemberID" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                    <%--<asp:TextBox ID="TextBox1" CssClass="form-control" runat="server" ValidationGroup="Send" autocomplete="off"></asp:TextBox>--%>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnDeviceID" runat="server" Visible="false">
            <div class="col-md-12">
                <div class="col-md-12">
                    <label>Device ID<span style="color: red; font-weight: bold"> *</span></label>
                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDeviceID" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                    <asp:TextBox ID="tbDeviceID" CssClass="form-control" runat="server" ValidationGroup="Send" autocomplete="off"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnDeviceGroup" runat="server" Visible="false">
            <div class="col-md-12">
                <div class="col-md-12">
                    <label>Device Group<span style="color: red; font-weight: bold"> *</span></label>
                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMemberID" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                    <%--<asp:TextBox ID="TextBox1" CssClass="form-control" runat="server" ValidationGroup="Send" autocomplete="off"></asp:TextBox>--%>
                </div>
            </div>
        </asp:Panel>
        <div class="col-md-12">
            <div class="col-md-12">
                <label>Title<span style="color:red; font-weight:bold"> *</span></label>
                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbTitle" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                <asp:TextBox ID="tbTitle" CssClass="form-control" runat="server" ValidationGroup="Send" autocomplete="off"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-12">
            <div class="col-md-12">
                <label>Message<span style="color:red; font-weight:bold"> *</span></label>
                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMessage" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                <asp:TextBox ID="tbMessage" TextMode="MultiLine" Rows="10" CssClass="form-control" runat="server" ValidationGroup="Send" autocomplete="off" MaxLength="255"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-12">
            <div class="col-md-12">
                <div class="form-group">
                    <asp:CheckBox ID="cb_link" runat="server" Text="Link" />
                    <asp:DropDownList runat="server" ID="dlLink" Width="150px" Height="34px">
                        <asp:ListItem Text="Privilege" Value="Privilege"></asp:ListItem>
                        <asp:ListItem Text="Event" Value="Event"></asp:ListItem>
                        <asp:ListItem Text="News" Value="News"></asp:ListItem>
                        <asp:ListItem Text="Article" Value="Article"></asp:ListItem>
                        <asp:ListItem Text="Ext Link" Value="Ext Link"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="tbLinkURL"  runat="server" autocomplete="off" Width="250px" Height="34px"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <asp:CheckBox ID="cb_schedule" runat="server" Text="Schedule" />
                    <label>Date</label>
                    <asp:TextBox ID="tbDate" runat="server" autocomplete="off" Width="150px" Height="34px"></asp:TextBox>
                    <label>Time</label>
                    <asp:TextBox ID="tbTime" runat="server" autocomplete="off" Width="50px" Height="34px"></asp:TextBox>
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

        <div class="col-md-12">
            <div class="col-md-6">
                <asp:LinkButton ID="btnSend" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSend_Click" ValidationGroup="Send"><span class="glyphicon glyphicon-phone"></span><span> Send</span></asp:LinkButton>
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
                                    <img class="img-responsive" src="../images/check.png"  width="100" height="100">
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
                                    <img class="img-responsive" src="../images/warning-3-xxl.png"  width="100" height="100">
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

</asp:Content>