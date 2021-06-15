<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageOnlineBooking.aspx.cs" Inherits="com.lexus.th.web.pages.PageOnlineBooking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Booking Register</h3>
        </div>
    </div>

    <div class="row">
        <label class="col-md-2">Booking Type</label>
        <div class="col-md-2">
            <asp:DropDownList runat="server" CssClass="form-control" ID="dlBookingType" AutoPostBack="true" OnSelectedIndexChanged="dlBookingType_SelectedIndexChanged">
                                                        <asp:ListItem Text="-- select --" Value="0" />
                                                        <asp:ListItem Text="Repurchase" Value="1" />
                                                        <asp:ListItem Text="Referral" Value="2" />
                                                        <asp:ListItem Text="Booking" Value="3" />
            </asp:DropDownList>
            
        </div>
        <label class="col-md-1">Booking</label>
        <div class="col-md-6">
            <asp:DropDownList ID="dlBooking" runat="server" CssClass="form-control" AutoPostBack="true" DataValueField="id" DataTextField="title_en" OnSelectedIndexChanged="dlBooking_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
        <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnExport_Click" ValidationGroup="Search"><span class="glyphicon glyphicon-download"></span><span> Save Report</span></asp:LinkButton>
        </div>
    </div>
    <br />
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
                            OnSelectedIndexChanged="gvCust_SelectedIndexChanged" OnRowCommand="gvCust_RowCommand" >
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEdit" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="ID" DataField="id" />
                                <asp:BoundField HeaderText="Name" DataField="name" />
                                <asp:BoundField HeaderText="Surname" DataField="surname" />
                                <asp:BoundField HeaderText="Contact Number" DataField="contact_number" />
                                <asp:BoundField HeaderText="Email" DataField="email" />   
                                <asp:BoundField HeaderText="Model" DataField="MODEL" />                                
                                <asp:BoundField HeaderText="Remark" DataField="remark" />                                                          
                                <asp:BoundField HeaderText="Create Date" DataField="CREATED_DATE" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="CREATED_USER" />
                                <asp:BoundField HeaderText="Update Date" DataField="UPDATED_DATE" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Update By" DataField="UPDATED_USER" />
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
                                <asp:Label runat="server" ID="lbDelNo" Visible="false"></asp:Label>
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
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Edit -->
    <div id="mdAdd" class="modal fade" data-backdrop="static" data-keyboard="false">
      <div class="modal-dialog">
        <asp:UpdatePanel ID="upModalAdd" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- Modal content-->
                <div class="modal-content">
                  <div class="modal-header">
                    <a class="close" data-dismiss="modal">&times;</a>
                    <h4 class="modal-title">Booking Code</h4>
                  </div>
                  <div class="modal-body">
                      <div class="row">
                          <div class="col-md-12">
                              <div class="row">
                         
                          <%--        
                                  <div class="col-md-6">
                                      <div class="form-group">
                                        <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                          <asp:Label runat="server" ID="lbBookingId" Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="lbBookingCodeId" Visible="false"></asp:Label>
                                        
                                        <label>Booking Code</label>
                                          <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbBookingCode" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                          <asp:CustomValidator Display="Dynamic" id="cvDupBooking" runat="server" ValidationGroup="Save" OnServerValidate="cvDupBooking_ServerValidate"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Duplicate Booking Code</span></asp:CustomValidator>
                                          <asp:TextBox runat="server" CssClass="form-control" ID="tbBookingCode" autocomplete="off"></asp:TextBox>
                                      </div>
                                  </div>--%>
                                          <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>

                                          <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Name</label>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbName" ></asp:TextBox>
                                                        </div>
                                                    </div>
                                          <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Surname</label>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbSurname"></asp:TextBox>
                                                        </div>
                                                    </div>
                                          <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>tbContactNumber</label>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbContactNumber"></asp:TextBox>
                                                        </div>
                                                    </div>
                                          <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label>Email</label>
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="tbEmail"></asp:TextBox>
                                                        </div>
                                                    </div>
                                       <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Dealer</label><asp:Label id="lbNotSet" runat="server" ForeColor="red" Font-Bold="True" Text="(Not Set)"/>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlDealer" DataValueField="dealer_id" DataTextField="display_th">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                          <div class="col-md-6">
                                               <div class="form-group">
                                                    <label>Plate Number</label>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbPlateNumber"></asp:TextBox>
                                               </div>
                                          </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Need to test drive</label>
                                               <asp:DropDownList runat="server" CssClass="form-control" ID="dlTestDrive">
                                                        <asp:ListItem Text="Need To Test Drive" Value="1" />
                                                        <asp:ListItem Text="Not Test Drive" Value="0" />
                                                    </asp:DropDownList>
                                            </div>
                                        </div>

                                            <div class="col-md-6">
                                               <div class="form-group">
                                                    <label>Remark</label>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbRemark"></asp:TextBox>
                                               </div>
                                          </div>
                                            <div class="col-md-6">
                                               <div class="form-group">
                                                    <label>Referral Name</label>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbReferralName"></asp:TextBox>
                                               </div>
                                          </div>
                                            <div class="col-md-6">
                                               <div class="form-group">
                                                    <label>Referral Surname</label>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbReferralSurname"></asp:TextBox>
                                               </div>
                                          </div>
                                            <div class="col-md-6">
                                               <div class="form-group">
                                                    <label>Referral Contact Number</label>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbReferralContactNumber"></asp:TextBox>
                                               </div>
                                          </div>
                                          <div class="col-md-6">
                                               <div class="form-group">
                                                    <label>Referral Email</label>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbReferralEmail"></asp:TextBox>
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