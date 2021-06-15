<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageInitialData.aspx.cs" Inherits="com.lexus.th.web.pages.PageInitialData" %>

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
            var dp = $('#<%=tbBirthdate.ClientID%>');
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
            var dp = $('#<%=tbRSDate.ClientID%>');
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
            var dp = $('#<%=tbSearchFrom.ClientID%>');
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
            var dp = $('#<%=tbSearchTo.ClientID%>');
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
        $(function () {
            $('#<%= chkSearchRSDate.ClientID  %>').click(function (e) {
                var tb1 = $('#<%= tbSearchFrom.ClientID %>');
                var tb2 = $('#<%= tbSearchTo.ClientID %>');
                if (this.checked) {
                    tb1.removeAttr('disabled');
                    tb2.removeAttr('disabled');
                }
                else {
                    tb1.attr('disabled', true);
                    tb2.attr('disabled', true);
                }
            });
        });

        function inputChange(e) {
            var formData = new FormData();
            //var files = e.target.files; 
            //var a = "a";
            formData.append('file', e[0]);
            //alert(formData);
            //for (var i = 0; i < e.length; i++) {
            //    fromData.append('file', e[i]);
            //}

            $.ajax({
                type: "POST",
                url: "UploadFileInitialCustomer.ashx",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
                success: function (response) {
                    response = JSON.parse(response)
                    if (response.status) {
                        var downloadLink = window.document.createElement('a');
                        downloadLink.href = response.path;
                        downloadLink.click();
                        document.body.removeChild(downloadLink);
                    }
                    else {
                        alert(response.message);
                    }
                }
            });
        }

        function isCharacterKeyPress(evt) {
            if ((event.charCode >= 48 && event.charCode <= 57) || (event.charCode >= 65 && event.charCode <= 90) || (event.charCode >= 97 && event.charCode <= 122))
                return true;
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" EnablePageMethods="true">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Initial Data</h3>
        </div>
    </div>

    <div class="row">
        <div class="col-md-2">
            <label>Search Initial</label>
        </div>
        <div class="col-md-6">
            <div class="col-md-12">
                <%--<asp:TextBox ID="txtSearch" CssClass="form-control" runat="server" onkeydown="return (event.keyCode!=13)"></asp:TextBox>--%>
                <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <div class="col-md-12">
                    <div class="form-check">
                        <label class="form-check-label">
                            <asp:CheckBox runat="server" ID="chkSearchCitizenId" /><span> Citizen ID</span>
                        </label>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="col-md-12">
                    <div class="form-check">
                        <label class="form-check-label">
                            <asp:CheckBox runat="server" ID="chkSearchFname" /><span> First Name</span>
                        </label>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="col-md-12">
                    <div class="form-check">
                        <label class="form-check-label">
                            <asp:CheckBox runat="server" ID="chkSearchEmail" /><span> Email</span>
                        </label>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="col-md-12">
                    <div class="form-check">
                        <label class="form-check-label">
                            <asp:CheckBox runat="server" ID="chkSearchVin" /><span> VIN</span>
                        </label>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="col-md-4">
                    <div class="form-check">
                        <label class="form-check-label">
                            <asp:CheckBox runat="server" ID="chkSearchRSDate" /><span> Delivery Date</span>
                            <%--                            <input type="checkbox" onclick="" /><span> Create Date</span>--%>
                        </label>
                    </div>
                </div>
                <div class="col-md-4">
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbSearchFrom" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect format</span></asp:RegularExpressionValidator>
                    <asp:TextBox ID="tbSearchFrom" CssClass="form-control" runat="server" placeholder="From" autocomplete="off"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbSearchTo" runat="server" ValidationGroup="Search" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect format</span></asp:RegularExpressionValidator>
                    <asp:TextBox ID="tbSearchTo" CssClass="form-control" runat="server" placeholder="To" autocomplete="off"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click" ValidationGroup="Search"><span class="glyphicon glyphicon-search"></span><span> Search</span></asp:LinkButton>
        </div>
    </div>

    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>

    <div class="row">
        <div class="col-md-6">
            <asp:LinkButton ID="btnAddModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModal_Click"><span class="glyphicon glyphicon-plus"></span><span> Initial Data</span></asp:LinkButton>
            <span class="upload-excel">
                <input type="file" id="uploadExcel" class="input-file"
                    accept="application/vnd.ms-excel"
                    onchange="inputChange(event.target.files)">
                <label for="uploadExcel" class="btn btn-success btn-sm">
                    <span class="glyphicon glyphicon-plus"></span>
                    <span>Upload Initial</span>
                </label>
            </span>
            <span class="upload-excel">
                <label class="btn btn-success btn-sm">
                    <span class="glyphicon glyphicon-save"></span>
                    <span>Download แบบฟอร์มตัวอย่าง initial data </span>
                    <input id="btnDownload" runat="server" type="button" class="input-file" onserverclick="btnDownload_ServerClick" />
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
                                <%--<asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEdit" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnDelete" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" CssClass="btn btn-danger btn-sm" OnClick="gvBtnDelete_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField HeaderText="Id" DataField="id" />
                                <asp:BoundField HeaderText="First Name" DataField="firstname" />
                                <asp:BoundField HeaderText="Last Name" DataField="lastname" />
                                <asp:BoundField HeaderText="Gender" DataField="gender" />
                                <asp:BoundField HeaderText="Birthdate" DataField="birthday" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Citizen Id" DataField="citizen_id" />
                                <asp:BoundField HeaderText="Email" DataField="email" />
                                <asp:BoundField HeaderText="Dealer" DataField="dealer" />
                                <asp:BoundField HeaderText="Vin" DataField="vin" />
                                <asp:BoundField HeaderText="Plate No" DataField="plate_no" />
                                <asp:BoundField HeaderText="Model" DataField="model" />
                                <asp:BoundField HeaderText="Delivery Date" DataField="rs_date" DataFormatString="{0:dd/MM/yyyy}" />
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

    <!-- Modal Add/Edit Customer -->
    <div id="mdAdd" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalAdd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal">&times;</button>--%>
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Initial Data</h4>
                        </div>
                        <div class="modal-body" onkeydown="return (event.keyCode!=13)">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                            <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                            <div class="form-group">
                                                <label>Title / คำนำหน้าชื่อ<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlTitle" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlTitle">
                                                    <asp:ListItem Text="Mr" Value="Mr" />
                                                    <asp:ListItem Text="Mrs" Value="Mrs" />
                                                    <asp:ListItem Text="Ms" Value="Ms" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Firstname / ชื่อจริง<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbFirstname" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbFirstname" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Lastname / นามสกุล<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbLastname" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbLastname" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Gender / เพศ<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlGener" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlGener">
                                                    <asp:ListItem Text="Male" Value="Male" />
                                                    <asp:ListItem Text="Female" Value="Female" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Birthdate / วันเกิด<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbBirthdate" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbBirthdate" runat="server" ValidationGroup="Save" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control input-append date" ID="tbBirthdate" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Citizen ID / บัตรประชาชน<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbSSN" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbSSN" onkeypress="return IsNumberKey(event)" MaxLength="13" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Email / อีเมล์<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbEmail" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbEmail" runat="server" ValidationGroup="Save" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect email format</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbEmail" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Dealer / ตัวแทนจำหน่าย<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlDealer" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlDealer" DataTextField="DEALER_NAME" DataValueField="DEALER_ID"></asp:DropDownList>
                                                <%--<asp:TextBox runat="server" CssClass="form-control" ID="tbDealer" autocomplete="off" ></asp:TextBox>--%>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Vin / หมายเลขตัวถัง<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbVin" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbVin" runat="server" ValidationGroup="Save" ValidationExpression="^[a-zA-Z0-9_.-]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect vin format</span></asp:RegularExpressionValidator>
                                                <asp:CustomValidator Display="Dynamic" ID="cvVin" runat="server" ValidationGroup="Save" OnServerValidate="cvVin_ServerValidate"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Duplicate Vin</span></asp:CustomValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbVin" MaxLength="17" autocomplete="off" style='text-transform:uppercase' onkeypress="return isCharacterKeyPress(event)"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>PlateNo / ทะเบียนรถ<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbPlateNo2" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbPlateNo1" autocomplete="off" MaxLength="3"></asp:TextBox>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbPlateNo2" onkeypress="return IsNumberKey(event)" autocomplete="off" MaxLength="4"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Model / รุ่น<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlModel" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlModel" DataTextField="MODEL" DataValueField="MODEL_ID"></asp:DropDownList>
                                                <%--<asp:TextBox runat="server" CssClass="form-control" ID="tbModel" autocomplete="off" ></asp:TextBox>--%>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Delivery Date / วันที่ส่งมอบ<span style="color: red; font-weight: bold">*</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbRSDate" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbRSDate" runat="server" ValidationGroup="Save" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect date format</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbRSDate" autocomplete="off"></asp:TextBox>
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
                                    <div class="form-group">
                                        <label>Reason / สาเหตุ<span style="color: red; font-weight: bold"> *</span></label>
                                        <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbReasonDelCust" runat="server" ValidationGroup="DelCust"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbReasonDelCust" TextMode="MultiLine" Rows="4" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <asp:Button ID="btnDelete" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnDelete_Click" ValidationGroup="DelCust" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
