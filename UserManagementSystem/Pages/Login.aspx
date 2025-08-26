<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UserManagementSystem.Pages.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>ログイン画面 - ユーザー管理システム</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" href="../Styles/common.css" />
    <link rel="stylesheet" href="../Styles/login.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="login-title">ログイン画面</div>
            
            <div class="login-form-group">
                <label for="txtUserID">ユーザーID</label>
                <asp:TextBox ID="txtUserID" runat="server" CssClass="login-input" placeholder="" />
            </div>
            
            <div class="login-form-group">
                <label for="txtPassword">パスワード</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="login-input" placeholder="" />
            </div>
            
            <asp:Button ID="btnLogin" runat="server" Text="ログイン" CssClass="login-button" OnClick="btnLogin_Click" />
            <div style="text-align:center; margin-top: 16px;">
                <a href="UserRegister.aspx" style="color: #d32f2f; font-weight: bold; text-decoration: underline;">
                    新規登録はこちら
                </a>
            </div>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="login-error">
                    <asp:Label ID="lblError" runat="server" />
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>