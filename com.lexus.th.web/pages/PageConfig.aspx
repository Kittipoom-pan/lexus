<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageConfig.aspx.cs" Inherits="com.lexus.th.web.pages.PageConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Config</h3>
        </div>
    </div>

    <div class="row" id="aaa">
        <%--<div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>OTP</label>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Timing</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbTiming" autocomplete="off" ></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Maximum Input</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbMaximumInput" autocomplete="off" ></asp:TextBox>
                </div>
            </div>
        </div>--%>
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Call Center Contact</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbCallCenterContact" autocomplete="off"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Call Center Email</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbCallCenterEmail" autocomplete="off" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Banner Speed (Second)</label>
                    <asp:TextBox runat="server"  TextMode="Number" CssClass="form-control" ID="tbBannerSpeed" autocomplete="off"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Push Notification Day</label>
                    <asp:Label ID="Label1" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" MaxLength="3" CssClass="form-control" ID="TxtPushDay"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Banner Picture</label>
                </div>
            </div>
        </div>
        <%--<div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Guest</label>
                    <asp:Label ID="lbImg1" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg1" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg1" />
                    <asp:LinkButton ID="btnUpImg1" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg1" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                </div>
            </div>
        </div>--%>
        <%--<div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Blank Banner</label>
                    <asp:Label ID="lbImg2" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg2" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg2" />
                    <asp:LinkButton ID="btnUpImg2" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg2" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                </div>
            </div>
        </div>--%>
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Authorized Dealer</label>
                    <asp:Label ID="lbImg3" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg3" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg3" />
                    <asp:LinkButton ID="btnUpImg3" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg3" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Authorized Service Dealer</label>
                    <asp:Label ID="lbImg4" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg4" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg4" />
                    <asp:LinkButton ID="btnUpImg4" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg4" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                </div>
            </div>
        </div>
        <%--<div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Blank Article</label>
                    <asp:Label ID="lbImg5" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg5" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg5" />
                    <asp:LinkButton ID="btnUpImg5" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg5" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                </div>
            </div>
        </div>--%>
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Upcoming</label>
                    <asp:Label ID="lbImg6" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg6" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg6" />
                    <asp:LinkButton ID="btnUpImg6" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg6" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>News</label>
                    <asp:Label ID="lbImg7" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg7" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg7" />
                    <asp:LinkButton ID="btnUpImg7" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg7" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                </div>
            </div>
        </div>
        <%--<div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Blank Upcoming</label>
                    <asp:Label ID="lbImg8" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg8" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg8" />
                    <asp:LinkButton ID="btnUpImg8" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg8" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                </div>
            </div>
        </div>--%>
        <%--<div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Blank News</label>
                    <asp:Label ID="lbImg9" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg9" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg9" />
                    <asp:LinkButton ID="btnUpImg9" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg9" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                </div>
            </div>
        </div>--%>
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Article Banner</label>
                    <asp:Label ID="lbImg10" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg10" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg10" />
                    <asp:LinkButton ID="btnUpImg10" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg10" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                </div>
            </div>
        </div>


        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Service Appointment Banner</label>
                    <asp:Label ID="lbImg11" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg11" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg11" />
                    <asp:LinkButton ID="btnUpImg11" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg11" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                </div>
            </div>
        </div>

        <div class="col-md-12">
            <div class="col-md-6">
                <div class="form-group">
                    <label>Online Booking Banner</label>
                    <asp:Label ID="lbImg12" runat="server" ForeColor="Orange"></asp:Label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg12" Enabled="false"></asp:TextBox>
                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upImg12" />
                    <asp:LinkButton ID="btnUpImg12" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                    <asp:LinkButton ID="btnDelImg12" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
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
</asp:Content>
