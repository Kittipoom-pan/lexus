<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageSendPush.aspx.cs" Inherits="com.lexus.th.web.master.PageSendPush" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Send Push</h3>
        </div>
    </div>

    <div class="row" style="width:800px;">
        <div class="col-md-12">
            <div class="col-md-12">
                <label>MessageText<span style="color:red; font-weight:bold"> *</span></label>
                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMessage" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                <asp:TextBox ID="tbMessage" CssClass="form-control" runat="server" ValidationGroup="Send" autocomplete="off"></asp:TextBox>
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
            <div class="col-md-12">
                <label>Member ID</label>
                <%--<asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMessage" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>--%>
                <asp:TextBox ID="tbMemberID" TextMode="SingleLine" Rows="10" CssClass="form-control" runat="server" ValidationGroup="Send" autocomplete="off"></asp:TextBox>
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
            <div class="col-md-12">
                <label>Device Token Android</label>
                <asp:TextBox ID="tbTokenAndroid" TextMode="MultiLine" Rows="10" CssClass="form-control" runat="server" ValidationGroup="Send" autocomplete="off"></asp:TextBox>
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
            <div class="col-md-12">
                <label>Device Token IOS</label>
                <asp:TextBox ID="tbTokenIOS" TextMode="MultiLine" Rows="10" CssClass="form-control" runat="server" ValidationGroup="Send" autocomplete="off"></asp:TextBox>
            </div>
            <div class="col-md-12">
                <label>จำนวน push android : </label> <asp:Label ID="lb_android" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-md-12">
                <label>จำนวน push ios : </label> <asp:Label ID="lb_ios" runat="server" Text=""></asp:Label>
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
</asp:Content>
