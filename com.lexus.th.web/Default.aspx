<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="com.lexus.th.web.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lexus</title>
    <link href="~/components/bootstrap-3.3.7-dist/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        body
        {
            background: url('images/product-page-gallery-pilot-sport-a-s-3-plus-Lexus.jpg') fixed;
            background-size: cover;
            padding: 0;
            margin: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SCLogin">
    </asp:ScriptManager>

        <div class="container" style="width:400px; margin-top:20px; margin-left:10px;">
            <div class="row">
		        <div class="col-md-12">
    		        <div class="panel panel-default" >
                        <div class="panel-heading">
                            <h3 class="panel-title">Please sign in V.2.0.0</h3>
                        </div>
			  	        <div class="panel-body">
			    	  	        <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbUsername" ></asp:TextBox>

			    		            <%--<input id="tbUsername" runat="server" class="form-control" placeholder="Username" name="tbUser" type="text">--%>
			    		        </div>
			    		        <div class="input-group" style="padding-top: 20px">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="tbPassword" TextMode="Password" ToolTip="password"  ></asp:TextBox>
                                    <%--<asp:TextBox runat="server" CssClass="form-control" ID="TextBox1" onclick="this.value='';" onblur="this.value='Password';" TextMode="Password" ToolTip="password"  ></asp:TextBox>--%>
			    			        <%--<input id="tbPassword" runat="server" class="form-control" placeholder="Password" name="tbPassword" type="password" value="">--%>
			    		        </div>                                  
                                <div class="form-group" style="padding-top: 20px">
                                    <asp:Button runat="server" ID="btnLogin" CssClass="btn btn-lg btn-success btn-block" OnClick="btnLogin_Click" Text="Login" />
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" ID="lbError" CssClass="text-danger"></asp:Label>
                                </div>
			            </div>
			        </div>
		        </div>
	        </div>
        </div>
    </form>
</body>
</html>
