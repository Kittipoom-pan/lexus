<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PagePreference.aspx.cs" Inherits="com.lexus.th.web.pages.PagePreference" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Preference</h3>
        </div>
    </div>

    <div class="row">
        <label class="col-md-2">Preference Enable</label>
        <%--<div class="col-md-5">
            <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
        </div>--%>
       <%-- <div class="col-md-2">
            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click"><span class="glyphicon glyphicon-search"></span><span> Search</span></asp:LinkButton>
        </div>--%>
    </div>

    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>
    <div class="col-md-12"></div>

    <div class="row">
        <div class="col-md-3">
            <asp:LinkButton ID="btnAddModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModal_Click"><span class="glyphicon glyphicon-plus"></span><span> New Question</span></asp:LinkButton>
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

    <!-- DataGrid Question -->
    <div class="row">
        <div class="col-lg-12">
            <div class="table-responsive table-hover">
                <asp:UpdatePanel runat="server" ID="upCustGrid">
                    <ContentTemplate>
                        <asp:GridView ID="gvCust" runat="server" AllowPaging="true"
                            CssClass="table table-bordered table-hover" DataKeyNames="question_id"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvCust_PageIndexChanging" 
                            OnSelectedIndexChanged="gvCust_SelectedIndexChanged" OnRowCommand="gvCust_RowCommand" OnRowDataBound="gvCust_RowDataBound" >
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select Answer" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnView" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-search'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnView_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEdit" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField ItemStyle-Width="10%" ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                <asp:BoundField HeaderText="QuestionID" DataField="question_id" />
                                <asp:BoundField HeaderText="Description" DataField="description" />
                                <asp:BoundField HeaderText="Active" DataField="is_active" />
                                <asp:BoundField HeaderText="Create Date" DataField="create_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="create_by" />
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

    <div class="row">
        <div class="col-md-3">
            <asp:LinkButton ID="btnAddAnswer" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddAnswer_Click"><span class="glyphicon glyphicon-plus"></span><span> New Answer</span></asp:LinkButton>
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

    <!-- DataGrid Answer -->
    <div class="row">
        <div class="col-lg-12">
            <div class="table-responsive table-hover">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <asp:GridView ID="gvAnswer" runat="server" AllowPaging="true"
                            CssClass="table table-bordered table-hover" DataKeyNames="answer_id, question_id"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            OnPageIndexChanging="gvAnswer_PageIndexChanging" OnRowCommand="gvAnswer_RowCommand" OnRowDataBound="gvAnswer_RowDataBound"
                            ShowHeaderWhenEmpty="true" >
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEditAnswer" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEditAnswer_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField ItemStyle-Width="10%" ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                <asp:BoundField HeaderText="QuestionID" DataField="question_id" />
                                <asp:BoundField HeaderText="AnswerID" DataField="answer_id" />
                                <asp:BoundField HeaderText="Description" DataField="description" />
                                <asp:BoundField HeaderText="Type" DataField="type" />
                                <asp:BoundField HeaderText="Picture" DataField="icon_image_url" />
                                <asp:BoundField HeaderText="Active" DataField="is_active" />
                                <asp:BoundField HeaderText="Create Date" DataField="create_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="create_by" />
                                <asp:BoundField HeaderText="Display Order" DataField="order" />
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

    <!-- Modal Confirm to delete Question -->
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
                    <h4 class="modal-title">Question</h4>
                  </div>
                  <div class="modal-body">
                      <div class="row">
                          <div class="col-md-12">
                              <div class="row">
                                  <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                  <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                          <label>Question EN<span style="color: red; font-weight: bold"> *</span></label>
                                          <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDescQ" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbDescQ" autocomplete="off"></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                          <label>Question TH<span style="color: red; font-weight: bold"> *</span></label>
                                          <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDescQTH" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbDescQTH" autocomplete="off"></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                          <label>Max Selected<span style="color: red; font-weight: bold"> *</span></label>
                                          <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbMaxSelected" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                          <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbMaxSelected" runat="server" ValidationGroup="Save" ValidationExpression="^[0-9\/\-,]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect number</span></asp:RegularExpressionValidator>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbMaxSelected" onkeypress="return IsNumberKey(event)" autocomplete="off"></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Active</label>
                                          <asp:DropDownList runat="server" CssClass="form-control" ID="dlActiveQ" >
                                             <asp:ListItem Text="Active" Value="1" />
                                             <asp:ListItem Text="Inactive" Value="0" />
                                          </asp:DropDownList>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Display Order<span style="color:red; font-weight:bold"> *</span></label>
                                          <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbOrderQ" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                          <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbOrderQ" runat="server" ValidationGroup="Save" ValidationExpression="^[0-9\/\-,]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect number</span></asp:RegularExpressionValidator>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbOrderQ" onkeypress="return IsNumberKey(event)" autocomplete="off"></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Require Answer</label>
                                          <asp:DropDownList runat="server" CssClass="form-control" ID="dlRequireAnswer" >
                                             <asp:ListItem Text="True" Value="1" />
                                             <asp:ListItem Text="False" Value="0" />
                                          </asp:DropDownList>
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

    <!-- Modal Add/Edit Answer -->
    <div id="mdAddAnswer" class="modal fade" data-backdrop="static" data-keyboard="false">
      <div class="modal-dialog">
        <asp:UpdatePanel ID="upModalAddAnswer" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- Modal content-->
                <div class="modal-content">
                  <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h4 class="modal-title">Answer</h4>
                  </div>
                  <div class="modal-body">
                      <div class="row">
                          <div class="col-md-12">
                              <div class="row">
                                  <asp:Label runat="server" ID="lbTypeAnswer" Visible="false"></asp:Label>
                                  <asp:Label runat="server" ID="lbModelId" Visible="false"></asp:Label>
                                  <asp:Label runat="server" ID="lbAnswerId" Visible="false"></asp:Label>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Question ID<span style="color:red; font-weight:bold"> *</span></label>
                                          <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="dlQuestion" runat="server" ValidationGroup="SaveAnswer"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                          <asp:DropDownList runat="server" CssClass="form-control" ID="dlQuestion" DataTextField="name_th" DataValueField="id" >
                                          </asp:DropDownList>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Type</label>
                                          <asp:DropDownList runat="server" CssClass="form-control" ID="dlOptional" >
                                             <asp:ListItem Text="Textbox" Value="1" />
                                             <asp:ListItem Text="Checkbox" Value="0" />
                                          </asp:DropDownList>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                          <label>AnswerEN<span style="color: red; font-weight: bold"> *</span></label>
                                          <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDesc" runat="server" ValidationGroup="SaveAnswer"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbDesc" autocomplete="off"></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                          <label>AnswerTH<span style="color: red; font-weight: bold"> *</span></label>
                                          <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbDescTH" runat="server" ValidationGroup="SaveAnswer"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbDescTH" autocomplete="off"></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Image</label><asp:Label ID="lbImg1" runat="server" ForeColor="Orange"></asp:Label>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbImg1" Enabled="false" ></asp:TextBox>
                                          <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" accept=".png, .jpg, .jpeg" ID="upImg1" />
                                          <asp:LinkButton ID="btnUpImg1" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                                          <asp:LinkButton ID="btnDelImg1" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Active</label>
                                          <asp:DropDownList runat="server" CssClass="form-control" ID="dlActive" >
                                             <asp:ListItem Text="Active" Value="1" />
                                             <asp:ListItem Text="Inactive" Value="0" />
                                          </asp:DropDownList>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Display Order<span style="color:red; font-weight:bold"> *</span></label>
                                          <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbOrder" runat="server" ValidationGroup="SaveAnswer"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                          <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbOrder" runat="server" ValidationGroup="SaveAnswer" ValidationExpression="^[0-9\/\-,]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect number</span></asp:RegularExpressionValidator>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbOrder" onkeypress="return IsNumberKey(event)" autocomplete="off"></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Optional Header</label>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbOptionalHeader" autocomplete="off"></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <label>Optional Text</label>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbOptionalText" autocomplete="off"></asp:TextBox>
                                      </div>
                                  </div>                                    
                            </div>
                        </div>
                    </div>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                      <asp:Button ID="btnSaveAnswer" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" OnClick="btnSaveAnswer_Click" ValidationGroup="SaveAnswer" />
                  </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpImg1" />
                <asp:PostBackTrigger ControlID="btnDelImg1" />
            </Triggers>
        </asp:UpdatePanel>
      </div>
    </div>

    <!-- Modal Confirm to delete Answer -->
    <div class="modal fade" id="mdDelConfirmAnswer" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalConfirmDeleteAnswer" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
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
                                <asp:Label runat="server" ID="lbDelAnswerModelId" Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lbDelAnswerId" Visible="false"></asp:Label>
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
                            <asp:Button ID="btnDeleteAnswer" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnDeleteAnswer_Click"/>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
