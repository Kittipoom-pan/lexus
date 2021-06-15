<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageKatashikiMapping.aspx.cs" Inherits="com.lexus.th.web.pages.PageKatashikiMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            function inputChange(e) {
            var formData = new FormData();
            formData.append('file', e[0]);

            $.ajax({
                type: "POST",
                url: "UploadFileKatashikiMapping.ashx",
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

            function onLoadKatashikiTemplate() {
                $.ajax({
                    type: "POST",
                    url: "PageKatashikiMapping.aspx/DownloadTemplate",
                    data: '',
                    headers: {
                        "cache-control": "no-cache",
                        "content-type": "application/json",
                        "charset": "utf-8"
                    },
                    dataType: "json",
                    success: function (response) {

                    },
                    error: function (response) {

                    }
                });
            }
    </script>
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Katashiki Mapping</h3>
        </div>
    </div>

    <div class="row">
        <label class="col-md-2">Katashiki Model</label>
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
        <div class="col-md-6">
            <asp:LinkButton ID="btnAddModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModal_Click"><span class="glyphicon glyphicon-plus"></span><span> Add Katashiki Model</span></asp:LinkButton>
            <%--<asp:LinkButton ID="btnUpload" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnUpload_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload Katashiki</span></asp:LinkButton>--%>
            <span class="upload-excel">
                <input type="file" id="uploadExcel" class="input-file"
                    accept="application/vnd.ms-excel"
                    onchange="inputChange(event.target.files)" />
                <label for="uploadExcel" class="btn btn-success btn-sm">
                    <span class="glyphicon glyphicon-plus"></span>
                    <span>Upload Excel</span>
                </label>
            </span>
            <span class="upload-excel">
                <label class="btn btn-success btn-sm">
                    <span class="glyphicon glyphicon-save"></span>
                    <span>Download แบบฟอร์มตัวอย่าง Katashiki</span>
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
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEdit" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:ButtonField ButtonType="Link" HeaderText="Edit" CommandName="Detail" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" Visible="true" ControlStyle-CssClass="btn btn-primary btn-sm"></asp:ButtonField>--%>
                                <asp:ButtonField ButtonType="Link" HeaderText="Disable" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                <asp:BoundField HeaderText="ID" DataField="id" />
                                <asp:BoundField HeaderText="Katashiki Code" DataField="katashiki_code" />
                                <asp:BoundField HeaderText="ModelName" DataField="MODEL" />
                                <%--<asp:BoundField HeaderText="Description" DataField="DESC" />
                                <asp:BoundField HeaderText="Image 1" DataField="IMAGES1" />
                                <asp:BoundField HeaderText="Image 2" DataField="IMAGES2" />
                                <asp:BoundField HeaderText="Image 3" DataField="IMAGES3" />
                                <asp:BoundField HeaderText="Image 4" DataField="IMAGES4" />
                                <asp:BoundField HeaderText="Image 5" DataField="IMAGES5" />--%>
                                <%--<asp:BoundField HeaderText="Display Start" DataField="DISPLAY_START" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Display End" DataField="DISPLAY_END" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create Date" DataField="CREATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="CREATE_USER" />
                                <asp:BoundField HeaderText="Update Date" DataField="UPDATE_DT" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Update By" DataField="UPDATE_USER" />--%>
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
                            <h4 class="modal-title">Katashiki Model</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                                <label>Katashiki Code<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbKatashikiCode" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbKatashikiCode" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Model Name<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlModel" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlModel" DataTextField="MODEL" DataValueField="MODEL_ID"></asp:DropDownList>

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

    <!-- Modal Upload -->
    <div id="mdUpload" class="modal fade" data-backdrop="static" data-keyboard="false">
      <div class="modal-dialog">
        <asp:UpdatePanel ID="upMdUpload" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- Modal content-->
                <div class="modal-content">
                  <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h4 class="modal-title">Upload Katashiki</h4>
                  </div>
                  <div class="modal-body">
                      <div class="row">
                          <div class="col-md-12">
                              <div class="row">
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Upload File</label>
                                          <asp:FileUpload runat="server" ID="upload" CssClass="btn btn-default btn-xs form-control" accept=".xlsx , .xls , .csv , .txt" />
                                      </div>
                                  </div>
                            </div>
                        </div>
                    </div>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                      <asp:Button ID="btnUploadFile" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnUploadFile_Click" />
                  </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUploadFile" />
            </Triggers>
        </asp:UpdatePanel>
      </div>
    </div>
</asp:Content>
