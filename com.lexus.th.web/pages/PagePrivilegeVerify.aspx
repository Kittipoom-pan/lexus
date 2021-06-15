<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PagePrivilegeVerify.aspx.cs" Inherits="com.lexus.th.web.master.PagePrivilegeVerify" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Privilege Verify</h3>
        </div>
    </div>

    <div class="row">
        <label class="col-md-2">Privilege</label>
        <div class="col-md-5">
            <asp:DropDownList ID="dlPrivilege" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="PRIVILEGE_ID" DataTextField="PRIVILEGE_NAME" OnSelectedIndexChanged="dlPrivilege_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>

    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>

    <div class="row">
        <div class="col-md-3">
            <asp:LinkButton ID="btnAddModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModal_Click"><span class="glyphicon glyphicon-plus"></span><span> Privilege Verify</span></asp:LinkButton>
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
                            CssClass="table table-bordered table-hover" DataKeyNames="PRIVILEGE_ID,SHOP_NM"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvCust_PageIndexChanging" 
                            OnSelectedIndexChanged="gvCust_SelectedIndexChanged" OnRowCommand="gvCust_RowCommand" OnRowDataBound="gvCust_RowDataBound" >
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEdit" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:ButtonField ButtonType="Link" HeaderText="Edit" CommandName="Detail" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" Visible="true" ControlStyle-CssClass="btn btn-primary btn-sm"></asp:ButtonField>--%>
                                <asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                <asp:BoundField HeaderText="Privilege ID" DataField="PRIVILEGE_ID" />
                                <asp:BoundField HeaderText="Shop Name" DataField="SHOP_NM" />
                                <asp:BoundField HeaderText="Verify Code" DataField="VERIFY_CODE" />
                                <asp:BoundField HeaderText="Status" DataField="STATUS" />                                
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
                                <asp:Label runat="server" ID="lbDelShop" Visible="false"></asp:Label>
                                <div class="col-md-2">
                                    <img class="img-responsive" src="../images/warning-3-xxl.png"  width="100" height="100">
                                </div>
                                <div class="col-md-10">
                                    <label>Do you want to delete a selected record?</label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <asp:Button ID="btnDelete" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnDelete_Click"/>
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
                    <h4 class="modal-title">Privilege Verify</h4>
                  </div>
                  <div class="modal-body">
                      <div class="row">
                          <div class="col-md-12">
                              <div class="row">
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Privilege</label>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbPrivilegeName" Enabled="false" autocomplete="off" ></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Shop Name<span style="color:red; font-weight:bold"> *</span></label>
                                           <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbShopName" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                          <asp:CustomValidator Display="Dynamic" id="cvDupShopNM" runat="server" ValidationGroup="Save" OnServerValidate="cvDupShopNM_ServerValidate"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Duplicate Shop NM</span></asp:CustomValidator>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbShopName" autocomplete="off" ></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="lbPrivilegeId" Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="lbShopNameOld" Visible="false"></asp:Label>
                                        <label>Verify Code<span style="color:red; font-weight:bold"> *</span></label>
                                          <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbVerifyCode" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                          <asp:CustomValidator Display="Dynamic" id="cvDupVerify" runat="server" ValidationGroup="Save" OnServerValidate="cvDupVerify_ServerValidate"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Duplicate Verify Code</span></asp:CustomValidator>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbVerifyCode" autocomplete="off" ></asp:TextBox>
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

</asp:Content>
