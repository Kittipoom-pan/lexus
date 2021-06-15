<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageRoadsideAssistance.aspx.cs" Inherits="com.lexus.th.web.pages.PageRoadsideAssistance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Roadside Assistance</h3>
        </div>
    </div>

    <div class="row" style="width:800px;">
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Banner Picture</label>
                    <asp:Label ID="lbImg1" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg1" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg1" accept=".png, .jpg, .jpeg" />
                    <asp:LinkButton ID="btnUpImg1" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg1" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
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
                <div class="form-group">
                    <label>Contact Number</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbContactNum" autocomplete="off" ></asp:TextBox>
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
                <div class="form-group">
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSave_Click" ValidationGroup="Save"><span class="glyphicon glyphicon-phone"></span><span> Save</span></asp:LinkButton>
                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnCancel_Click" ValidationGroup="Cancel"><span class="glyphicon glyphicon-phone"></span><span> Cancel</span></asp:LinkButton>
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
