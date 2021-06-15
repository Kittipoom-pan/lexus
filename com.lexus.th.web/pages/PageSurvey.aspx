<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="PageSurvey.aspx.cs" Inherits="com.lexus.th.web.master.PageSurvey" %>

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
            var dp = $('#<%=tbStartDate.ClientID%>');
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
            var dp = $('#<%=tbEndDate.ClientID%>');
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
        
        function bannerTab() {
            $('.nav-tabs #banner-tab').tab('show');
        }
        function bannerDetailTab() {
            $('.nav-tabs #bannerDetail-tab').tab('show');
        }
        function CheckValidateConfirm() {
            debugger;
            var suggestion = document.getElementById('<%= dlAllowSuggestion.ClientID%>');
            var suggestion_th = document.getElementById('<%= tbSuggesttionTH.ClientID%>');
            var suggestion_en = document.getElementById('<%= tbSuggesttionEN.ClientID%>');

            if (suggestion == "0") {
                
                ValidatorEnable(document.getElementById('<%= rfSuggestionEN.ClientID%>'), false);ValidatorEnable(document.getElementById('<%= rfSuggestionTH.ClientID%>'), false);
            }
            else {
                
                ValidatorEnable(document.getElementById('<%= rfSuggestionEN.ClientID%>'), true);ValidatorEnable(document.getElementById('<%= rfSuggestionTH.ClientID%>'), true);
                if (suggestion_th.value == '' || suggestion_en.value == '') {
                    return false;
                }            
             

            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">Survey</h3>
        </div>
    </div>

    <div class="row">
        <label class="col-md-2">Survey Search</label>
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
            <asp:LinkButton ID="btnAddModal" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModal_Click"><span class="glyphicon glyphicon-plus"></span><span> Survey</span></asp:LinkButton>
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
                            CssClass="table table-bordered table-hover" DataKeyNames="ID"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvCust_PageIndexChanging"
                            OnSelectedIndexChanged="gvCust_SelectedIndexChanged" OnRowCommand="gvCust_RowCommand" OnRowDataBound="gvCust_RowDataBound">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select Question" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnView" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-search'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnView_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEdit" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                <asp:BoundField HeaderText="Id" DataField="ID" />
                                <asp:BoundField HeaderText="Survey TH" DataField="survey_th" />
                                <asp:BoundField HeaderText="Survey EN" DataField="survey_en" />
                                <asp:TemplateField HeaderText="Allow Postpone">
                                    <ItemTemplate>
                                        <%# (Eval("allow_postpone").ToString().ToLower().Equals("true") ? "<i class='fa fa-check' style='font-size:36px;color:#5cb85c'></i>" : "<i class='fa fa-close' style='font-size:36px;color:red'></i>") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Interval (Day)" DataField="interval" />
                                <asp:BoundField HeaderText="Times" DataField="times" />
                                <asp:TemplateField HeaderText="Show Times">
                                    <ItemTemplate>
                                        <%# (Eval("times").ToString().ToLower().Equals("0") ? "Away" : "First Time") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Start Date" DataField="start_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="End Date" DataField="end_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create Date" DataField="created_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="created_user" />
                                <asp:BoundField HeaderText="Update Date" DataField="updated_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Update By" DataField="updated_user" />
                                <asp:BoundField HeaderText="Display Order" DataField="ordinal" />

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
    <div class="row">

        <div class="col-md-3">
            <asp:LinkButton ID="btnAddModalQuestion" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddModalQuestion_Click"><span class="glyphicon glyphicon-plus"></span><span>Question</span></asp:LinkButton>
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
                <asp:UpdatePanel runat="server" ID="upQuestionGrid">

                    <ContentTemplate>
                        <div class="col-md-3">
                            <asp:Label runat="server" ID="lbShowEventId"></asp:Label>
                        </div>
                        <asp:GridView ID="gvQuestion" runat="server" AllowPaging="true"
                            CssClass="table table-bordered table-hover" DataKeyNames="ID"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvQuestion_PageIndexChanging"
                            OnSelectedIndexChanged="gvQuestion_SelectedIndexChanged" OnRowCommand="gvQuestion_RowCommand" OnRowDataBound="gvQuestion_RowDataBound">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select Answer" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnView" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-search'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvQuestionBtnView_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEdit" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvQuestionBtnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField ItemStyle-Width="10%" ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                <%--     <asp:BoundField HeaderText="EventID" DataField="event_id" />--%>


                                <asp:BoundField HeaderText="ID" DataField="id" />
                                <asp:BoundField HeaderText="Question TH" DataField="question_th" />
                                <asp:BoundField HeaderText="Question EN" DataField="question_en" />
                                <asp:TemplateField HeaderText="Question Type">
                                    <ItemTemplate>
                                        <%# (Eval("question_type").ToString().ToLower().Equals("1") ? "Rating" :
                                                Eval("question_type").ToString().ToLower().Equals("2")? "TextBox":                                            
                                                Eval("question_type").ToString().ToLower().Equals("3")? "Checkbox":
                                                Eval("question_type").ToString().ToLower().Equals("4")? "Radio":
                                                Eval("question_type").ToString().ToLower().Equals("5")? "None":"") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Allow Suggestion">
                                    <ItemTemplate>
                                        <%# (Eval("allow_suggestion").ToString().ToLower().Equals("true") ? "<i class='fa fa-check' style='font-size:36px;color:#5cb85c'></i>" : "<i class='fa fa-close' style='font-size:36px;color:red'></i>") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Allow Back">
                                    <ItemTemplate>
                                        <%# (Eval("allow_back").ToString().ToLower().Equals("true") ? "<i class='fa fa-check' style='font-size:36px;color:#5cb85c'></i>" : "<i class='fa fa-close' style='font-size:36px;color:red'></i>") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Depend On">
                                    <ItemTemplate>
                                        <%# (Eval("is_depend_on_other_questions").ToString().ToLower().Equals("true") ? "<i class='fa fa-check' style='font-size:36px;color:#5cb85c'></i>" : "<i class='fa fa-close' style='font-size:36px;color:red'></i>") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Require All">
                                    <ItemTemplate>
                                        <%# (Eval("is_require_all").ToString().ToLower().Equals("true") ? "<i class='fa fa-check' style='font-size:36px;color:#5cb85c'></i>" : "<i class='fa fa-close' style='font-size:36px;color:red'></i>") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Required">
                                    <ItemTemplate>
                                        <%# (Eval("is_required").ToString().ToLower().Equals("true") ? "<i class='fa fa-check' style='font-size:36px;color:#5cb85c'></i>" : "<i class='fa fa-close' style='font-size:36px;color:red'></i>") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Create Date" DataField="created_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="created_user" />
                                <asp:BoundField HeaderText="Display Order" DataField="ordinal" />
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

    <!-- DataGrid Answer -->
    <div class="row">
        <div class="col-lg-12">
            <div class="table-responsive table-hover">
                <asp:UpdatePanel runat="server" ID="upAnswer">
                    <ContentTemplate>

                        <div class="row">
                            <div class="col-md-3">

                                <asp:LinkButton ID="btnAddAnswer" Visible="false" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddAnswer_Click"><span class="glyphicon glyphicon-plus"></span><span>Answer</span></asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <asp:Label runat="server" ID="lbShowQuestionId"></asp:Label>
                        </div>
                        <div class="col-md-12"></div>
                        <div class="col-md-12"></div>
                        <div class="col-md-12"></div>
                        <div class="col-md-12"></div>
                        <div class="col-md-12"></div>
                        <div class="col-md-12"></div>
                        <div class="col-md-12"></div>
                        <div class="col-md-12"></div>

                        <asp:GridView ID="gvChoice" runat="server" AllowPaging="true"
                            CssClass="table table-bordered table-hover" DataKeyNames="ID, question_id"
                            ShowFooter="true" AutoGenerateColumns="false" PageSize="10" Width="100%" EmptyDataText="No data"
                            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvChoice_PageIndexChanging"
                            OnSelectedIndexChanged="gvChoice_SelectedIndexChanged" OnRowCommand="gvChoice_RowCommand" OnRowDataBound="gvChoice_RowDataBound">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvBtnEditAnswer" runat="server" Text="<i aria-hidden='true' class='glyphicon glyphicon-pencil'></i>" CssClass="btn btn-primary btn-sm" OnClick="gvBtnEditAnswer_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:ButtonField ItemStyle-Width="10%" ButtonType="Link" HeaderText="Delete" CommandName="Clear" Text="<i aria-hidden='true' class='glyphicon glyphicon-remove'></i>" Visible="true" ControlStyle-CssClass="btn btn-danger btn-sm"></asp:ButtonField>
                                <%-- <asp:BoundField HeaderText="QuestionID" DataField="question_id" />--%>
                                <asp:BoundField HeaderText="ID" DataField="ID" />
                                <asp:BoundField HeaderText="Choice TH" DataField="choice_th" />
                                <asp:BoundField HeaderText="Choice EN" DataField="choice_en" />
                                <asp:TemplateField HeaderText="Is Image">
                                    <ItemTemplate>
                                        <%# (Eval("is_image").ToString().ToLower().Equals("true") ? "<i class='fa fa-check' style='font-size:36px;color:#5cb85c'></i>" : "<i class='fa fa-close' style='font-size:36px;color:red'></i>") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Image Path" DataField="image_path" />
                                <asp:BoundField HeaderText="Create Date" DataField="created_date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                <asp:BoundField HeaderText="Create By" DataField="created_user" />
                                <asp:BoundField HeaderText="Display Order" DataField="ordinal" />
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

    <!-- Modal Confirm to delete Question -->
    <div class="modal fade" id="mdDelConfirmQuestion" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalConfirmDeleteQuestion" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
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
                                <asp:Label runat="server" ID="lbDelQuestionId" Visible="false"></asp:Label>
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
                            <asp:Button ID="btnDeleteQuestion" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnDeleteQuestion_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Modal Confirm to delete Question -->
    <div class="modal fade" id="mdDelConfirmChoice" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalConfirmDeleteChoice" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
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
                                <asp:Label runat="server" ID="lbDelChoiceId" Visible="false"></asp:Label>
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
                            <asp:Button ID="btnDeleteAnswer" runat="server" Text="OK" CssClass="btn btn-primary btn-sm" OnClick="btnDeleteAnswer_Click" />
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
                            <h4 class="modal-title">Survey</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">

                                                <label>Survey Type<span style="color: red; font-weight: bold"> *</span></label>
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
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbFile1" Enabled="false"></asp:TextBox>
                                                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upFile1" />
                                                    <asp:LinkButton ID="btnUpFile1" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadFile_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnDelFile1" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteFile_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnDownload1" runat="server" CssClass="btn btn-default btn-xs" OnClick="btnExport_Click"><span class="glyphicon glyphicon-download"></span><span> Export</span></asp:LinkButton>

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
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbFile2" Enabled="false"></asp:TextBox>
                                                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" ID="upFile2" />
                                                    <asp:LinkButton ID="btnUpFile2" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadFile_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnDelFile2" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteFile_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnDownload2" runat="server" CssClass="btn btn-default btn-xs" OnClick="btnExport_Click"><span class="glyphicon glyphicon-download"></span><span> Export</span></asp:LinkButton>

                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lbType" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lbId" Visible="false"></asp:Label>
                                                <label>Survey TH<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbSurveyTH" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbSurveyTH" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Survey EN<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbSurveyEN" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbSurveyEN" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Start Date</label>
                                                <%--<asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbStartDate" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>--%>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbStartDate" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>End Date</label>
                                                <%--<asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbEndDate" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>--%>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbEndDate" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Allow Postpone</label>
                                                <asp:DropDownList runat="server" ID="dlAllowPostpone" CssClass="form-control">
                                                    <asp:ListItem Text="Disable" Value="0" />
                                                    <asp:ListItem Text="Enable" Value="1" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Postpone Interval (Day)<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbInterval" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbInterval" runat="server" ValidationGroup="Save" ValidationExpression="^[0-9\/\-,]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect number</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbInterval" onkeypress="return IsNumberKey(event)" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Show Times<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:DropDownList runat="server" ID="dlTimes" CssClass="form-control">
                                                    <asp:ListItem Text="Away" Value="0" />
                                                    <asp:ListItem Text="First Time" Value="1" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Display Order<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbSurveyOrdinal" runat="server" ValidationGroup="Save"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbSurveyOrdinal" runat="server" ValidationGroup="Save" ValidationExpression="^[0-9\/\-,]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect number</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbSurveyOrdinal" onkeypress="return IsNumberKey(event)" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
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
                    <asp:PostBackTrigger ControlID="btnUpFile1" />
                    <asp:PostBackTrigger ControlID="btnDelFile1" />
                    <asp:PostBackTrigger ControlID="btnUpFile2" />
                    <asp:PostBackTrigger ControlID="btnDelFile2" />
                    <asp:PostBackTrigger ControlID="btnDownload1" />
                    <asp:PostBackTrigger ControlID="btnDownload2" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- Modal Question Add/Edit -->
    <div id="mdAddQuestion" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="overflow-y: auto; max-height: 650px;">
            <asp:UpdatePanel ID="upModalQuestion" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
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
                                        <asp:Label runat="server" ID="lbQuestionType" Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="lbQSurveyId" Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="lbQuestionId" Visible="false"></asp:Label>

                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Question TH<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbQuestionTH" runat="server" ValidationGroup="SaveQuestion"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbQuestionTH" MaxLength="200" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Question EN<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbQuestionEN" runat="server" ValidationGroup="SaveQuestion"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbQuestionEN" MaxLength="200" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Question Type</label>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlQuestionType">
                                                    <asp:ListItem Text="Rating" Value="1" />
                                                    <asp:ListItem Text="TextBox" Value="2" />
                                                    <asp:ListItem Text="Checkbox" Value="3" />
                                                    <asp:ListItem Text="Radio" Value="4" />
                                                    <asp:ListItem Text="None" Value="5" />

                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Allow Suggestion</label>
                                                <asp:DropDownList runat="server" ID="dlAllowSuggestion" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlAllowSuggestion_SelectedIndexChanged">
                                                    <asp:ListItem Text="Disable" Value="0" />
                                                    <asp:ListItem Text="Enable" Value="1" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-12" style="padding: 0 !important;">
                                            <asp:Panel runat="server" ID="pnSuggestion">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Suggestion TH<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator ID="rfSuggestionTH" Display="Dynamic" ControlToValidate="tbSuggesttionTH" runat="server" ValidationGroup="SaveQuestion"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbSuggesttionTH" MaxLength="200" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Suggestion EN<span style="color: red; font-weight: bold"> *</span></label>
                                                        <asp:RequiredFieldValidator ID="rfSuggestionEN" Display="Dynamic" ControlToValidate="tbSuggesttionEN" runat="server" ValidationGroup="SaveQuestion"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="tbSuggesttionEN" MaxLength="200" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Allow Back</label>
                                                <asp:DropDownList runat="server" ID="dlAllowBack" CssClass="form-control">
                                                    <asp:ListItem Text="Disable" Value="0" />
                                                    <asp:ListItem Text="Enable" Value="1" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Is Required</label>
                                                <asp:DropDownList runat="server" ID="dlRequire" CssClass="form-control">
                                                    <asp:ListItem Text="None" Value="0" />
                                                    <asp:ListItem Text="Required" Value="1" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Display Order<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbOrderQuestion" runat="server" ValidationGroup="SaveQuestion"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbOrderQuestion" runat="server" ValidationGroup="SaveQuestion" ValidationExpression="^[0-9\/\-,]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect number</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbOrderQuestion" onkeypress="return IsNumberKey(event)" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Depend On Other Question</label>
                                                <asp:DropDownList runat="server" ID="dlDependOn" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlDependOn_SelectedIndexChanged">
                                                    <asp:ListItem Text="No" Value="0" />
                                                    <asp:ListItem Text="Yes" Value="1" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-12" style="padding: 0 !important;">
                                            <asp:Panel runat="server" ID="pnDepend">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Is Require All</label>
                                                        <asp:DropDownList runat="server" ID="dlRequiredAll" CssClass="form-control">
                                                            <asp:ListItem Text="None" Value="0" />
                                                            <asp:ListItem Text="Required" Value="1" />
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <%--  <asp:UpdatePanel runat="server" ID="upRPTypeOfService" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>

                                                        <div class="col-md-12 table-bordered">
                                                            <div class="col-md-12">
                                                                <div class="col-md-12">
                                                                    <div class="form-group">
                                                                    </div>
                                                                </div>
                                                                <label>Question Rule</label>

                                                            </div>
                                                            <div class="col-md-8">
                                                                <div class="form-group">

                                                                    <asp:DropDownList runat="server" ID="dlPrevQuestion" CssClass="form-control" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">

                                                                    <asp:LinkButton ID="btnAddPrevQuestion" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAddPrevQuestion_Click"><span class="glyphicon glyphicon-plus"></span><span>Previous Question</span></asp:LinkButton>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="form-group">

                                                                    <asp:Repeater ID="RPDepend" runat="server" OnItemDataBound="RPDepend_ItemDataBound" OnItemCommand="RPDepend_ItemCommand">
                                                                        <ItemTemplate>
                                                                            <div class="col-md-12  table-bordered">

                                                                                <div class="form-group" style="padding-left: 20px !important;">
                                                                                    <asp:Label ID="lbQuestionRuleID" Visible="false" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lbRPQuestionID" Visible="false" runat="server"></asp:Label>
                                                                                    <div class="col-md-12">
                                                                                        <div class="form-group">
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-12">
                                                                                        <div class="form-group">

                                                                                            <asp:LinkButton ID="btnDelPrevQuestion" CommandName="delete" CommandArgument='<%#  Eval("ID") %>' runat="server" CssClass="btn btn-danger btn-sm"><span class="glyphicon glyphicon-plus"></span><span>Delete</span></asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                    <br />

                                                                                    <label>Previous Question ID : </label>
                                                                                    <asp:Label ID="lbPreviousQuestionID" runat="server"></asp:Label>
                                                                                    <br />
                                                                                    <asp:Label ID="lbQuestionType" Visible="false" runat="server"></asp:Label>
                                                                                    <label>Question Type : </label>
                                                                                    <asp:Label ID="lbQuestionTypeLabel" runat="server"></asp:Label>
                                                                                    <br />
                                                                                    <div class="col-md-6" style="padding-left: 0px !important;">
                                                                                        <div class="form-group">
                                                                                            <label>Question TH : </label>
                                                                                            <asp:Label ID="lbQuestionTH" runat="server"></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-6" style="padding-left: 0px !important;">
                                                                                        <div class="form-group">
                                                                                            <label>Question EN : </label>
                                                                                            <asp:Label ID="lbQuestionEN" runat="server"></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-12">
                                                                                        <div class="form-group">
                                                                                            <asp:CheckBoxList ID="chkQuestionRuleChoiceList" runat="server"></asp:CheckBoxList>
                                                                                            <asp:RadioButtonList ID="rdQuestionRuleCoiceList" runat="server"></asp:RadioButtonList>
                                                                                        </div>
                                                                                    </div>


                                                                                </div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="form-group">
                                                                </div>
                                                            </div>
                                                        </div>



                                                        <%-- </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                                    </div>
                                                </div>

                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button ID="btnSaveQuestion" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" OnClientClick="CheckValidateConfirm();" OnClick="btnSaveQuestion_Click" ValidationGroup="SaveQuestion" />
                        </div>
                    </div>
                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
    </div>
    <!-- Modal Add/Edit Answer -->
    <div id="mdAddChoice" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModalAddChoice" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <a class="close" data-dismiss="modal">&times;</a>
                            <h4 class="modal-title">Choice</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">

                                        <asp:Label runat="server" ID="lbAQuestionId" Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="lbQChoiceType" Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="lbChoiceId" Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="lbChoiceType" Visible="false"></asp:Label>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Choice TH<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbChoiceTH" runat="server" ValidationGroup="SaveAnswer"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbChoiceTH" MaxLength="200" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Choice EN<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbChoiceEN" runat="server" ValidationGroup="SaveAnswer"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbChoiceEN" MaxLength="200" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Display Order<span style="color: red; font-weight: bold"> *</span></label>
                                                <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="tbOrderChoice" runat="server" ValidationGroup="SaveAnswer"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Required field</span></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tbOrderChoice" runat="server" ValidationGroup="SaveAnswer" ValidationExpression="^[0-9\/\-,]*$"><span class="glyphicon glyphicon glyphicon-warning-sign" style="color:orange"></span><span style="color:orange"> Incorrect number</span></asp:RegularExpressionValidator>
                                                <asp:TextBox runat="server" CssClass="form-control" ID="tbOrderChoice" onkeypress="return IsNumberKey(event)" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label>Is Image</label>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="dlIsImage" AutoPostBack="true" OnSelectedIndexChanged="dlIsImage_SelectedIndexChanged">
                                                    <asp:ListItem Text="None" Value="0" />
                                                    <asp:ListItem Text="Image" Value="1" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Panel runat="server" ID="pnUpload" Visible="false">
                                                <div class="form-group">
                                                    <label>Image - Choice</label><asp:Label ID="lbImg1" runat="server" ForeColor="Orange"></asp:Label>
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbImg1" Enabled="false"></asp:TextBox>
                                                    <asp:FileUpload runat="server" CssClass="btn btn-default btn-xs form-control" accept=".png, .jpg, .jpeg" ID="upImg1" />
                                                    <asp:LinkButton ID="btnUpImg1" runat="server" CssClass="btn btn-default btn-xs" OnClick="UploadImage_Click"><span class="glyphicon glyphicon-upload"></span><span> Upload</span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnDelImg1" runat="server" CssClass="btn btn-default btn-xs" OnClick="DeleteImage_Click"><span class="glyphicon glyphicon-remove"></span><span> Remove</span></asp:LinkButton>
                                                </div>
                                            </asp:Panel>
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
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
