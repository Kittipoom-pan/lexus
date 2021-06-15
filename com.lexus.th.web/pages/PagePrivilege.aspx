<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PagePrivilege.aspx.cs" Inherits="com.lexus.th.web.master.PagePrivilege" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
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
            var dp = $('#<%=tbPeriodStart.ClientID%>');
            dp.datetimepicker({
                defaultDate: new Date(),
                format: 'DD/MM/YYYY HH:mm',
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        });
        $(document).ready(function () {
            var dp = $('#<%=tbPeriodEnd.ClientID%>');
            dp.datetimepicker({
                defaultDate: new Date(),
                format: 'DD/MM/YYYY HH:mm',
            }).on('changeDate', function (ev) {
                $(this).blur();
                $(this).datepicker('hide');
            });
        });
    </script>
    <style>
        /* width */
        ::-webkit-scrollbar {
            width: 10px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            background: #f1f1f1;
        }

        /* Handle */
        ::-webkit-scrollbar-thumb {
            background: #888;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #555;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Privileges</h3>
        </div>
    </div>

    <div class="row">
        <label class="col-md-2">Privileges Search</label>
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
            <asp:LinkButton ID="btnAddModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModal_Click"><span class="glyphicon glyphicon-plus"></span><span> Privilege</span></asp:LinkButton>
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

                                <%--<asp:BoundField HeaderText="Description" DataField="DESC" />--%>
                                <asp:BoundField HeaderText="Privilege Type" DataField="display_type_name" />
                                <asp:BoundField HeaderText="Topic" DataField="TITLE" />
                                <%--<asp:BoundField HeaderText="Period" DataField="PERIOD" />--%>
                                <asp:BoundField HeaderText="Period Start" DataField="PERIOD_START" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Period End" DataField="PERIOD_END" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <%--<asp:BoundField HeaderText="Redeem Condition" DataField="RED_CONDITION" />
                                <asp:BoundField HeaderText="Redeem Period" DataField="RED_PERIOD" />
                                <asp:BoundField HeaderText="Redeem Location" DataField="RED_LOCATION" />
                                <asp:BoundField HeaderText="Redeem Expiry (minute)" DataField="RED_EXPIRY" />
                                <asp:BoundField HeaderText="Max Redeem per Account" DataField="REED_TIME" />
                                <asp:BoundField HeaderText="Image" DataField="IMAGE" />--%>
                                <asp:BoundField HeaderText="Display Start" DataField="DISPLAY_START" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Display End" DataField="DISPLAY_END" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Redeem Type" DataField="PRIVILEDGE_TYPE_NAME" />
                                <asp:BoundField HeaderText="Create Date" DataField="CREATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="CREATE_USER" />
                                <asp:BoundField HeaderText="Update Date" DataField="UPDATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Update By" DataField="UPDATE_USER" />
                                <asp:BoundField HeaderText="Display Order" DataField="order" />
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
                            <h4 class="modal-title">Privilege</h4>
                        </div>
                        <div class="modal-body" role="document">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Privilege Type</label>
                                                <asp:DropDownList runat="server" ID="dlDisplayType" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlDisplayType_SelectedIndexChanged">
                                                    <asp:ListItem Text="Elite Privilege" Value="1" />
                                                    <asp:ListItem Text="Luxury Privilege" Value="2" />
                                                    <asp:ListItem Text="Lifestyle Privilege" Value="3" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                                <label>Topic<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbTitle" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbTitle" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Display Start<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDispStart" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbDispStart" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Display End<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDispEnd" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbDispEnd" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Period Start<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbPeriodStart" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbPeriodStart" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Period End<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbPeriodEnd" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbPeriodEnd" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>

                                        <%--Update in phase 2.1.0 Period setting--%>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Period Type<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:DropDownList runat="server" ID="dlPeriodType" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlPeriodType_SelectedIndexChanged">
                                                    <%--<asp:ListItem Text="UNLIMIT" Value="0" />--%>
                                                    <asp:ListItem Text="DAILY" Value="1" />
                                                    <asp:ListItem Text="WEEKLY" Value="2" />
                                                    <asp:ListItem Text="MONTHLY" Value="3" />
                                                    <asp:ListItem Text="ONCE_PER_CAMPAIGN" Value="5" />
                                                    <asp:ListItem Text="ONCE_PER_CAMPAIGN_LIMIT_BY_MONTH" Value="6" />
                                                    <asp:ListItem Text="DAILY_LIMIT_BY_MONTH" Value="9" />
                                                    <asp:ListItem Text="ONCE_PER_CAMPAIGN_LIMIT_BY_DAILY" Value="11" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnPeriodStartInWeek" runat="server" Visible="false">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Period start in week<span style="color: red; font-weight: bold"> *</span></label>
                                                    <asp:DropDownList runat="server" ID="dlPeriodStartInWeek" CssClass="form-control" AutoPostBack="true">
                                                        <asp:ListItem Text="Monday" Value="1" />
                                                        <asp:ListItem Text="Tuesday" Value="2" />
                                                        <asp:ListItem Text="Wednesday" Value="3" />
                                                        <asp:ListItem Text="Thursday" Value="4" />
                                                        <asp:ListItem Text="Friday" Value="5" />
                                                        <asp:ListItem Text="Saturday" Value="6" />
                                                        <asp:ListItem Text="Sunday" Value="0" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pnPeriodStartInMonth" runat="server" Visible="false">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Period start in month (1 - 28)<span style="color: red; font-weight: bold"> *</span></label>
                                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbPeriodStartInMonth" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbPeriodStartInMonth" autocomplete="off" Text="1"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <div class="col-md-6">
                                            <div class="form-group" style="height: 29px; margin-top: 30px;">
                                                <asp:CheckBox ID="cb_customer_usage_with_car_total" runat="server" Text=" Check Customer usage with car total" Checked="false" AutoPostBack="true" OnCheckedChanged="cb_customer_usage_with_car_total_CheckedChanged" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Period Quota<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbPeriodQuota" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbPeriodQuota" autocomplete="off" Text="1"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group" style="height: 59px;">
                                                <asp:Panel ID="pnCustomerUsagePerPeriod" runat="server">
                                                    <label>Customer usage per period<span style="color: red; font-weight: bold"> *</span></label>
                                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbCustomerUsagePerPeriod" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbCustomerUsagePerPeriod" autocomplete="off" Text="1"></asp:TextBox>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <%--Update in phase 2.1.0 Period setting--%>

                                        <ul class="nav nav-tabs" id="myTab" role="tablist">
                                            <li class="nav-item">
                                                <a class="nav-link active" id="th-tab" data-toggle="tab" href="#th" role="tab" aria-controls="th" aria-selected="true">TH</a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" id="en-tab" data-toggle="tab" href="#en" role="tab" aria-controls="en" aria-selected="false">EN</a>
                                            </li>
                                        </ul>
                                        <div class="tab-content">
                                            <div class="tab-pane active" id="th" role="tabpanel" aria-labelledby="th-tab">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Description TH</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbDescTH" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Redeem Condition TH</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbRedConditionTH" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Redeem Location TH</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbRedLocationTH" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Thankyou Message TH</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbThkMessageTH" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="tab-pane" id="en" role="tabpanel" aria-labelledby="en-tab">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Description EN</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbDesc" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Redeem Condition EN</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbRedCondition" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Redeem Location EN</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbRedLocation" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Thankyou Message EN</label>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbThkMessage" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6" style="display: none;">
                                            <div class="form-group">
                                                <label>Max Redeem Count</label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMaxRedeemCnt" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbMaxRedeemCnt" onkeypress="return IsNumberKey(event)" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Redeem Type</label>
                                                <asp:DropDownList runat="server" ID="dlPrivilegeType" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlPrivilegeType_SelectedIndexChanged">
                                                    <asp:ListItem Text="Shop Verify" Value="1" />
                                                    <asp:ListItem Text="Redeem Code" Value="3" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnRedeemDisplay" runat="server">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Redeem Display</label>
                                                    <asp:DropDownList runat="server" ID="dlRedeemDisplay" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlRedeemDisplay_SelectedIndexChanged">
                                                        <asp:ListItem Text="Redeem Code" Value="1" />
                                                        <asp:ListItem Text="QR Code" Value="2" />
                                                        <asp:ListItem Text="Barcode" Value="3" />
                                                        <asp:ListItem Text="HTML" Value="4" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pnPictureRedeem" runat="server" Visible="false">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Picture Redeem Display</label><asp:Label ID="lbImg3" runat="server" ForeColor="Orange"></asp:Label>
                                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbImg3" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg3" Enabled="false"></asp:TextBox>
                                                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" accept=".png, .jpg, .jpeg" ID="upImg3" />
                                                    <asp:LinkButton ID="btnUpImg3" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnDelImg3" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="pnHTMLRedeem" runat="server" Visible="false">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>HTML Text</label>
                                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbRedeemDisplayHTML" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbRedeemDisplayHTML" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Redeem Display Height</label>
                                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbRedeemDisplayHeight" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbRedeemDisplayHeight" onkeypress="return IsNumberKey(event)" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Redeem Display Width</label>
                                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbRedeemDisplaywidth" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbRedeemDisplayWidth" onkeypress="return IsNumberKey(event)" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Image - Privilege Banner</label><asp:Label ID="lbImg1" runat="server" ForeColor="Orange"></asp:Label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbImg1" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbImg1" Enabled="false"></asp:TextBox>
                                                <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" accept=".png, .jpg, .jpeg" ID="upImg1" />
                                                <asp:LinkButton ID="btnUpImg1" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                                                <asp:LinkButton ID="btnDelImg1" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                                            </div>
                                        </div>


                                        <asp:Panel ID="pnRedeemType" runat="server" Visible="false">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Redeem Expiry (format Time = 00:00)</label>
                                                    <asp:DropDownList runat="server" ID="dlExpiryType" CssClass="form-control">
                                                        <asp:ListItem Text="Minute" Value="1" />
                                                        <asp:ListItem Text="Time" Value="2" />
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbRedExpiry" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbRedExpiry" onkeypress="return IsNumberKeyTime(event)" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Image - Privilege Detail</label><asp:Label ID="lbImg2" runat="server" ForeColor="Orange"></asp:Label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbImg2" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbImg2" Enabled="false"></asp:TextBox>
                                                <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" accept=".png, .jpg, .jpeg" ID="upImg2" />
                                                <asp:LinkButton ID="btnUpImg2" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                                                <asp:LinkButton ID="btnDelImg2" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Display Order<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbOrder" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbOrder" runat="server" ValidationGroup="Save" ValidationExpression="^[0-9\/\-,]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect number</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbOrder" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:CheckBox ID="cb_show_remaining" runat="server" Text="Show Privilege Remaining" style="display: block;"/>
                                                <asp:CheckBox ID="cb_return_redeem_when_verify_expire" runat="server" Text="คืนสิทธิ์ Luxury เมื่อไม่มีการ verify (Privilege CNT)" style="display: block;"/>
                                                <asp:CheckBox ID="cb_repeat_redeem_when_verify_expire" runat="server" Text="คืนสิทธิ์การใช้ privilege นี้ เมื่อไม่มีการ verify (Customer Usage)" style="display: block;"/>
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
                    <asp:PostBackTrigger ControlID="btnUpImg1" />
                    <asp:PostBackTrigger ControlID="btnDelImg1" />
                    <asp:PostBackTrigger ControlID="btnUpImg2" />
                    <asp:PostBackTrigger ControlID="btnDelImg2" />
                    <asp:PostBackTrigger ControlID="btnUpImg3" />
                    <asp:PostBackTrigger ControlID="btnDelImg3" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
