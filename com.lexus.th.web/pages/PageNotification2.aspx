<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageNotification2.aspx.cs" Inherits="com.lexus.th.web.pages.PageNotification2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .upload-excel .input-file {
            width: 0;
            height: 0;
            opacity: 0;
            overflow: hidden;
            position: absolute;
            z-index: -1;
        }

        .upload-excel label {
            cursor: pointer;
            margin: 0;
        }

    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var dp = $('#<%=tbDate.ClientID%>');
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

        function btnSave() {
            let status = false;
            if ($('#ContentPlaceHolder1_dlDestination').val() != "All") {
                if ($('#ContentPlaceHolder1_tbMemberID').val() != "" && $('#ContentPlaceHolder1_tbTitle').val() != "" && $('#ContentPlaceHolder1_tbMessage').val() != "") {
                    console.log("1");
                    $('#ContentPlaceHolder1_btnSave').attr('disabled', 'disabled')
                    status = true;
                }
            }
            else if ($('#ContentPlaceHolder1_tbTitle').val() != "" && $('#ContentPlaceHolder1_tbMessage').val() != "") {
                console.log("2");
                $('#ContentPlaceHolder1_btnSave').attr('disabled', 'disabled')
                status = true;
            }

            if (status) {
                $('#ContentPlaceHolder1_btnSaveServer').click();
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Message History</h3>
        </div>
    </div>

    <div class="row">
        <label class="col-md-2">Search</label>
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
        <div class="col-md-12">
            <asp:LinkButton ID="btnAddModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModal_Click">
                <span class="glyphicon glyphicon-plus"></span><span> Push Notification</span></asp:LinkButton>

            <span class="upload-excel">
                <label class="btn btn-success btn-sm">
                    <span class="glyphicon glyphicon-save"></span>
                    <span>Download แบบฟอร์มตัวอย่าง Group Member ID</span>
                    <input id="btnDownloadMember" runat="server" type="button" class="input-file" onserverclick="btnDownloadMember_ServerClick" />
                </label>
            </span>
            <span class="upload-excel">
                <label class="btn btn-success btn-sm">
                    <span class="glyphicon glyphicon-save"></span>
                    <span>Download แบบฟอร์มตัวอย่าง Group Device</span>
                    <input id="btnDownloadDevice" runat="server" type="button" class="input-file" onserverclick="btnDownloadDevice_ServerClick" />
                </label>
            </span>
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
                            CssClass="table table-bordered table-hover" DataKeyNames="id"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvCust_PageIndexChanging"
                            OnSelectedIndexChanged="gvCust_SelectedIndexChanged" OnRowCommand="gvCust_RowCommand" OnRowDataBound="gvCust_RowDataBound">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEdit" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-search'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnDelete" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" CssClass="btn btn-danger btn-sm" OnClick="gvBtnDeleteApp_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:ButtonField ButtonType="Link" HeaderText="Edit" CommandName="Detail" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" Visible="true" ControlStyle-CssClass="btn btn-primary btn-sm"></asp:ButtonField>--%>
                                <%--<asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>--%>
                                <asp:BoundField HeaderText="Id" DataField="id" />
                                <asp:BoundField HeaderText="Topic" DataField="title" />
                                <asp:BoundField HeaderText="Destination" DataField="destination" />
                                <asp:BoundField HeaderText="Type" DataField="sub_destination" />
                                <asp:BoundField HeaderText="Send Date/Time" DataField="send_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Status" DataField="status" />
                                <asp:BoundField HeaderText="Create Date" DataField="create_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="create_by" />
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
                            <h4 class="modal-title">Notification</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                                <label>Destination<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlDestination" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" ID="dlDestination" Width="150px" Height="34px" AutoPostBack="true" OnSelectedIndexChanged="dlDestination_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlSubDestination" runat="server" ValidationGroup="Send"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" ID="dlSubDestination" Visible="false" Width="150px" Height="34px" AutoPostBack="true" OnSelectedIndexChanged="dlSubDestination_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnMemberID" runat="server" Visible="false">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label>Member ID</label>
                                                    <asp:TextBox ID="tbMemberID" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pnMemberGroup" runat="server" Visible="false">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label>Member Group</label>
                                                    <asp:Label ID="lbFile1" runat="server" ForeColor="Orange"></asp:Label>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbFile1" Enabled="false" ></asp:TextBox>
                                                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upFile1" />
                                                    <asp:LinkButton ID="btnUpFile1" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadFile_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnDelFile1" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteFile_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pnDeviceID" runat="server" Visible="false">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label>Device ID</label>
                                                    <asp:TextBox ID="tbDeviceID" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pnDeviceGroup" runat="server" Visible="false">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label>Device Group</label>
                                                    <asp:Label ID="lbFile2" runat="server" ForeColor="Orange"></asp:Label>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbFile2" Enabled="false" ></asp:TextBox>
                                                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upFile2" />
                                                    <asp:LinkButton ID="btnUpFile2" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadFile_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnDelFile2" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteFile_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label>Title<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbTitle" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="tbTitle" CssClass="form-control" runat="server" MaxLength="150" autocomplete="off"  ></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label>Message<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMessage" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="tbMessage" TextMode="MultiLine" Rows="10" CssClass="form-control" runat="server" MaxLength="255" autocomplete="off" ></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:CheckBox ID="cb_link" runat="server" Text="Link" AutoPostBack="true" OnCheckedChanged="cb_link_CheckedChanged" />
                                                <asp:DropDownList runat="server" ID="dlLink" Width="150px" Height="34px" AutoPostBack="true" OnSelectedIndexChanged="dlLink_SelectedIndexChanged">
                                                    <asp:ListItem Text="None" Value="none"></asp:ListItem>
                                                    <asp:ListItem Text="Privilege" Value="Privilege"></asp:ListItem>
                                                    <asp:ListItem Text="Event" Value="Event"></asp:ListItem>
                                                    <asp:ListItem Text="News" Value="News"></asp:ListItem>
                                                    <asp:ListItem Text="Article" Value="Article"></asp:ListItem>
                                                    <asp:ListItem Text="Ext Link" Value="Ext_Link"></asp:ListItem>
                                                    <asp:ListItem Text="Main Privilege" Value="Main_Privilege"></asp:ListItem>
                                                    <asp:ListItem Text="Main News Event" Value="Main_News_Event"></asp:ListItem>
                                                    <asp:ListItem Text="Upcoming Event" Value="Upcoming_Event"></asp:ListItem>
                                                    <asp:ListItem Text="News List" Value="News_List"></asp:ListItem>
                                                    <asp:ListItem Text="Main Dealer" Value="Main_Dealer"></asp:ListItem>
                                                    <asp:ListItem Text="Auth Dealer" Value="Auth_Dealer"></asp:ListItem>
                                                    <asp:ListItem Text="Service Dealer" Value="Service_Dealer"></asp:ListItem>
                                                    <asp:ListItem Text="Main Service" Value="Main_Service"></asp:ListItem>
                                                    <asp:ListItem Text="Main More" Value="Main_More"></asp:ListItem>
                                                    <asp:ListItem Text="Main Article" Value="Main_Article"></asp:ListItem>
                                                    <asp:ListItem Text="Appointment Landing" Value="Appointment_Landing"></asp:ListItem>
                                                    <asp:ListItem Text="Test Drive Landing" Value="Test_Drive_Landing"></asp:ListItem>
                                                    <asp:ListItem Text="Service Reminder" Value="Service_Reminder"></asp:ListItem>
                                                    <asp:ListItem Text="Main Online Booking" Value="Main_Online_Booking"></asp:ListItem>
                                                </asp:DropDownList>
                                                
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:Panel ID="pnLinkURL" runat="server" Visible="false">
                                                    <asp:TextBox ID="tbLinkURL" runat="server" autocomplete="off" Width="250px" Height="34px"></asp:TextBox>
                                                </asp:Panel>
                                                <asp:Panel ID="pnLinkAction" runat="server" Visible="false">
                                                    <asp:DropDownList runat="server" CssClass="form-control" ID="dlAction" DataValueField="ID" DataTextField="TITLE">
                                                    </asp:DropDownList>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:CheckBox ID="cb_schedule" runat="server" Text="Schedule" AutoPostBack="true" OnCheckedChanged="cb_schedule_CheckedChanged" />
                                                <label>Date</label>
                                                <asp:TextBox ID="tbDate" runat="server" autocomplete="off" Width="150px" Height="34px"></asp:TextBox>
                                                <label>Time</label>
                                                <asp:TextBox ID="tbTime" runat="server" autocomplete="off" Width="50px" Height="34px" onkeypress="return IsNumberKeyTime(event)"></asp:TextBox>
                                                (00:00)
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <%--<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" ValidationGroup="Save" OnClick="btnSave_Click" />--%>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" ValidationGroup="Save" OnClientClick="btnSave();" />
                            <asp:Button ID="btnSaveServer" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" ValidationGroup="Save" style="opacity: 0; display: none;" OnClick="btnSave_Click" /> 
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnUpFile1" />
                    <asp:PostBackTrigger ControlID="btnDelFile1" />
                    <asp:PostBackTrigger ControlID="btnUpFile2" />
                    <asp:PostBackTrigger ControlID="btnDelFile2" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
