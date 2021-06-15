<%@ Page Title="" Language="C#" MasterPageFile="~/master/Site1.Master" AutoEventWireup="true" CodeBehind="zzzTest.aspx.cs" Inherits="com.lexus.th.web.pages.zzzTest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <h3 class="page-header">run push ที่ยังไม่ถึงเวลา</h3>
        </div>
    </div>

    <div class="row">
        <div class="col-md-2">
            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click"><span> Run</span></asp:LinkButton>
        </div>
    </div>
</asp:Content>
