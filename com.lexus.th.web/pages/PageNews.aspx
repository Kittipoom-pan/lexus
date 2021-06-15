<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageNews.aspx.cs" Inherits="com.lexus.th.web.master.PageNews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var dp = $('#<%=tbDate.ClientID%>');
            dp.datetimepicker({
                defaultDate: new Date(),
                format: 'DD/MM/YYYY',
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        });
        $(document).ready(function () {
            var dp = $('#<%=tbDispStart.ClientID%>');
            dp.datetimepicker({
                defaultDate: new Date(),
                format: 'DD/MM/YYYY HH:mm',
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        });
        $(document).ready(function () {
            var dp = $('#<%=tbDispEnd.ClientID%>');
            dp.datetimepicker({
                defaultDate: new Date(),
                format: 'DD/MM/YYYY HH:mm',
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        });
        function bannerTab() {
            $('.nav-tabs #banner-tab').tab('show');
        }
        function bannerDetailTab() {
            $('.nav-tabs #bannerDetail-tab').tab('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">News</h3>
        </div>
    </div>

    <div class="row">
        <label class="col-md-2">News Search</label>
        <div class="col-md-5">
            <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server" onkeydown="return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click"><span class="glyphicon glyphicon-search"></span><span> Search</span></asp:LinkButton>
        </div>
    </div>

    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>

    <div class="row">
        <div class="col-md-3">
            <asp:LinkButton ID="btnAddModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModal_Click"><span class="glyphicon glyphicon-plus"></span><span> News</span></asp:LinkButton>
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
                                <%--<asp:ButtonField ButtonType="Link" HeaderText="Edit" CommandName="Detail" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" Visible="true" ControlStyle-CssClass="btn btn-primary btn-sm"></asp:ButtonField>--%>
                                <asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                <asp:BoundField HeaderText="Id" DataField="ID" />
                                <asp:BoundField HeaderText="Date" DataField="DATE" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Topic" DataField="TITLE" />
                                <%--<asp:BoundField HeaderText="Description" DataField="DESC" />
                                <asp:BoundField HeaderText="Image 1" DataField="IMAGES1" />
                                <asp:BoundField HeaderText="Image 2" DataField="IMAGES2" />
                                <asp:BoundField HeaderText="Image 3" DataField="IMAGES3" />
                                <asp:BoundField HeaderText="Image 4" DataField="IMAGES4" />
                                <asp:BoundField HeaderText="Image 5" DataField="IMAGES5" />--%>
                                <asp:BoundField HeaderText="Display Start" DataField="DISPLAY_START" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Display End" DataField="DISPLAY_END" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
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
                            <h4 class="modal-title">News</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                                <label>Topic</label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbTitle" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbTitle" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Date</label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDate" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbDate" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Display Start</label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDispStart" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbDispStart" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Display End</label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDispEnd" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbDispEnd" autocomplete="off"></asp:TextBox>
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
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <ul class="nav nav-tabs" id="myTab" role="tablist">
                                                    <li class="nav-item">
                                                        <a class="nav-link active" id="th-tab" data-toggle="tab" href="#th" role="tab" aria-controls="th" aria-selected="true">TH</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="en-tab" data-toggle="tab" href="#en" role="tab" aria-controls="en" aria-selected="false">EN</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="banner-tab" data-toggle="tab" href="#banner" role="tab" aria-controls="en" aria-selected="false">Banner</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="bannerDetail-tab" data-toggle="tab" href="#bannerDetail" role="tab" aria-controls="bannerDetail" aria-selected="false">Banner Detail</a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <!-- Tab panes -->
                                        <div class="tab-content">
                                            <div class="tab-pane active" id="th" role="tabpanel" aria-labelledby="th-tab">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label>Description TH</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbDescTH" TextMode="MultiLine" Rows="4" autocomplete="off" Width="100%"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="en" role="tabpanel" aria-labelledby="en-tab">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label>Description EN</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbDesc" TextMode="MultiLine" Rows="4" autocomplete="off" Width="100%"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="banner" role="tabpanel" aria-labelledby="banner-tab">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label>Image - News Banner</label><asp:Label ID="lbImgBanner1" runat="server" ForeColor="Orange"></asp:Label>
                                                        <asp:FileUpload runat="server" AllowMultiple="true" CssClass="btn btn-default btn-xs form-control" accept=".png, .jpg, .jpeg" ID="upImgBanner1" />
                                                        <asp:LinkButton ID="btnUpImgBanner1" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                                                    </div>
                                                </div>
                                                    

                                                <div class="col-md-12">
                                                    <div class="table-responsive table-hover">

                                                        <asp:GridView ID="gvBanner" runat="server" AllowPaging="true"
                                                            CssClass="table table-bordered table-hover" DataKeyNames="FileName"
                                                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                                                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvBanner_PageIndexChanging"
                                                            OnSelectedIndexChanged="gvBanner_SelectedIndexChanged" OnRowCommand="gvBanner_RowCommand" OnRowDataBound="gvBanner_RowDataBound">
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Image Name">
                                                                    <ItemTemplate>
                                                                        <span style="font-size: 9pt;"><%# Eval("FileName") %></span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                                            </Columns>
                                                        </asp:GridView>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="bannerDetail" role="tabpanel" aria-labelledby="bannerDetail-tab">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label>Image - News Banner Detail</label><asp:Label ID="lbImg1" runat="server" ForeColor="Orange"></asp:Label>
                                                        <asp:FileUpload runat="server" AllowMultiple="true" CssClass="btn btn-default btn-xs form-control" accept=".png, .jpg, .jpeg" ID="upImg1" />
                                                        <asp:LinkButton ID="btnUpImg1" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="table-responsive table-hover">
                                                        <%-- <asp:UpdatePanel runat="server" ID="upBannerDetail" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                    <ContentTemplate>--%>
                                                        <asp:GridView ID="gvBannerDetail" runat="server" AllowPaging="true"
                                                            CssClass="table table-bordered table-hover" DataKeyNames="FileName"
                                                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                                                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvBannerDetail_PageIndexChanging"
                                                            OnSelectedIndexChanged="gvBannerDetail_SelectedIndexChanged" OnRowCommand="gvBannerDetail_RowCommand" OnRowDataBound="gvBannerDetail_RowDataBound">
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Image Name">
                                                                    <ItemTemplate>
                                                                        <span style="font-size: 9pt;"><%# Eval("FileName") %></span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <%-- </ContentTemplate>

                                                </asp:UpdatePanel>--%>
                                                    </div>
                                                </div>
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
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnUpImgBanner1" />
                    <asp:PostBackTrigger ControlID="btnUpImg1" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
