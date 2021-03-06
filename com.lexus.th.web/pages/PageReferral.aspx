<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageReferral.aspx.cs" Inherits="com.lexus.th.web.master.PageReferral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function initailDateTime() {
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
            $(document).ready(function () {
                var dp = $('#<%=tbRegPeriondStart.ClientID%>');
                dp.datetimepicker({
                    defaultDate: new Date(),
                    format: 'DD/MM/YYYY HH:mm',
                }).on('changeDate', function (ev) {
                    $(this).blur();
                    $(this).datepicker('hide');
                });
            });
            $(document).ready(function () {
                var dp = $('#<%=tbRegPeriondEnd.ClientID%>');
                dp.datetimepicker({
                    defaultDate: new Date(),
                    format: 'DD/MM/YYYY HH:mm',
                }).on('changeDate', function (ev) {
                    $(this).blur();
                    $(this).datepicker('hide');
                });
            });
        }
        initailDateTime();
        function bannerTab() {
            $('.nav-tabs #banner-tab').tab('show');
        }
        function bannerDetailTab() {
            $('.nav-tabs #bannerDetail-tab').tab('show');
        }
        function defaultTab() {
            $('.nav-tabs #th-tab').tab('show');
        }
        function ShowHideDiv() {
            var chkYes = document.getElementById("<%=rdFollowByCar.ClientID %>");
            var tbByMember = document.getElementById("<%=tbByMember.ClientID %>");

            if (chkYes.checked) {

                tbByMember.disabled = true;
            }
            else {
                tbByMember.disabled = false;
            }
        }
    </script>
    <style>
        .chkPickup {
            padding-right: 5px;
        }

        .choice-text label {
            padding-left: 5px;
        }
        input[type="radio"] {
            margin: 10px 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Referral</h3>
        </div>
    </div>

    <div class="row">
        <label class="col-md-2">Referral Search</label>
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
            <asp:LinkButton ID="btnAddModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModal_Click"><span class="glyphicon glyphicon-plus"></span><span> Referral</span></asp:LinkButton>
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
                                <asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                <asp:BoundField HeaderText="Id" DataField="ID" />
                                <asp:BoundField HeaderText="Title EN" DataField="title_en" />
                                <asp:BoundField HeaderText="Title TH" DataField="title_th" />
                                <asp:BoundField HeaderText="Display Start" DataField="DISPLAY_START" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Display End" DataField="DISPLAY_END" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Register Period Start" DataField="reg_start" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Register Period End" DataField="reg_end" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <%# (Eval("is_active").ToString().ToLower().Equals("true") ? "<i class='fa fa-check' style='font-size:36px;color:#5cb85c'></i>" : "<i class='fa fa-close' style='font-size:36px;color:red'></i>") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Create Date" DataField="created_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="created_user" />
                                <asp:BoundField HeaderText="Update Date" DataField="updated_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Update By" DataField="updated_user" />

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
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
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
                            <h4 class="modal-title">Referral</h4>
                        </div>
                        <div class="modal-body">
                            <!-- Prefered Model -->
                            <asp:Button ID="btnDetail" runat="server" class="btn btn-default" Text="Referral Detail" OnClick="Menu_Click" />
                            &nbsp;&nbsp;<asp:Button ID="btnPreferred" runat="server" class="btn btn-default" Text="Preferred Model" OnClick="Menu_Click" />
                            <asp:Panel ID="pnPreferred" runat="server">
                                <div class="row" style="height: 500px; overflow: auto;">
                                    <br />
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">

                                                    <label>Preferred Model</label>
                                                    <asp:Label runat="server" ID="lbRequiredPreferredModel"><span style="display: inline;"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color: orange"></span><span style="color: orange">Preferred Model is Required</span></span></asp:Label>
                                                    <br />
                                                    <asp:CheckBox runat="server" ID="chkPreferredModelSelectAll"  CssClass="chkPickup" onclick="SelectUnSelectAll()"></asp:CheckBox><label>Select All</label>
                                                    <asp:CheckBoxList runat="server" ID="chkPreferredModel" CssClass="choice-text" onclick="CheckSelectAll()" DataTextField="model" DataValueField="id" ></asp:CheckBoxList>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <!-- Detail -->
                            <asp:Panel ID="pnDetail" runat="server">
                                <div class="row">
                                    <br />
                                    <div class="col-md-12">
                                        <div class="row">
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
                                                    <label>Register Period Start</label>
                                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbRegPeriondStart" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbRegPeriondStart" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Register Period End</label>
                                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbRegPeriondEnd" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbRegPeriondEnd" autocomplete="off"></asp:TextBox>
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
                                   <%--         <div class="col-md-6">
                                                <div class="form-group">

                                                    <label>Is Require Plate No</label>--%>
                                                    <asp:DropDownList runat="server" CssClass="form-control" ID="dlIsrequirePlateNo" Visible="false">
                                                        <asp:ListItem Text="Required" Value="1" />
                                                        <asp:ListItem Text="Not Required" Value="0" />
                                                    </asp:DropDownList>
                                      <%--          </div>
                                            </div>--%>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Following By</label>
                                                    <div>
                                                    <asp:RadioButton ID="rdFollowByCar" GroupName="Passport" Text="Following By Car" runat="server" onclick="ShowHideDiv()"/><br>
                                                    <asp:RadioButton ID="rdFollowByMember" GroupName="Passport" Text="Following By Member" runat="server" onclick="ShowHideDiv()" />
                                                        </div>
                                                    <div id="dvFollow" style="display: block">
                                                        Max Use :
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbByMember"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12">

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

                                            <!-- Tab panes -->
                                            <div class="tab-content">
                                                <div class="tab-pane active" id="th" role="tabpanel" aria-labelledby="th-tab">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                                            <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                                            <label>Title TH</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="TitleTH" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="TitleTH" TextMode="MultiLine" Rows="4" MaxLength="250" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Description TH</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDescTH" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbDescTH" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Condition TH</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbConditionTH" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbConditionTH" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Register Period TH</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbRegPeriodTH" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbRegPeriodTH" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Thank You Message TH</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbThankYouMsgTH" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbThankYouMsgTH" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Codemessage TH</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbCodemessageTH" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbCodemessageTH" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="tab-pane" id="en" role="tabpanel" aria-labelledby="en-tab">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Title EN</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="TitleEN" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="TitleEN" TextMode="MultiLine" Rows="4" MaxLength="250" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Description EN</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDesc" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbDesc" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Condition EN</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbCondition" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbCondition" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Register Period EN</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbRegPeriod" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbRegPeriod" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Thank You Message EN</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbThankYouMsgEN" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbThankYouMsgEN" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Codemessage EN</label>
                                                            <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbCodemessageEN" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbCodemessageEN" autocomplete="off"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="tab-pane" id="banner" role="tabpanel" aria-labelledby="banner-tab">
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <label>Image - Referral Banner</label><asp:Label ID="lbImgBanner1" runat="server" ForeColor="Orange"></asp:Label>
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
                                                            <label>Image - Referral Banner Detail</label><asp:Label ID="lbImg1" runat="server" ForeColor="Orange"></asp:Label>
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
                            </asp:Panel>
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

    <script lang="javascript" type="text/javascript">
        function SelectUnSelectAll()
        {
            debugger;
            var checkAll = true;
            if($('#<%=chkPreferredModelSelectAll.ClientID%>')[0].checked)
            {
                checkAll = true;
            }
            else
            {
                checkAll = false;
            }
            var chkBoxList = $('#<%=chkPreferredModel.ClientID%>');
            var chkBoxCount = chkBoxList.find("input:checkbox");
            for(var i=0; i< chkBoxCount.length; i++)
            {
                chkBoxCount[i].checked = checkAll;
            }
        }
        function CheckSelectAll()
        {            
            var chkBoxList = $('#<%=chkPreferredModel.ClientID%>');
            var chkBoxCount = chkBoxList.find("input:checkbox");
            var chkBoxCheckedCount = chkBoxList.find("input:checkbox:checked");

            if (chkBoxCount.length == chkBoxCheckedCount.length) {
                $('#<%=chkPreferredModelSelectAll.ClientID%>').prop('checked', true);
            }
            else {
                $('#<%=chkPreferredModelSelectAll.ClientID%>').prop('checked', false);
            }
        }     
    </script>

</asp:Content>
