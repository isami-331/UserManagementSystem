<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserRegister.aspx.cs" Inherits="UserManagementSystem.Pages.UserRegister" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>ユーザー登録- ユーザー管理システム</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" href="../Styles/common.css" />
    <link rel="stylesheet" href="../Styles/register.css" />
    <script type="text/javascript">
        // 入力値リアルタイム検証
        function validateInput() {
            var userID = document.getElementById('<%= txtUserID.ClientID %>').value.trim();
            var userName = document.getElementById('<%= txtUserName.ClientID %>').value.trim();
            var password = document.getElementById('<%= txtPassword.ClientID %>').value.trim();
            
            var registerBtn = document.getElementById('<%= btnRegister.ClientID %>');
            
            if (userID === '' || userName === '' || password === '') {
                registerBtn.disabled = true;
                registerBtn.style.opacity = '0.6';
            } else {
                registerBtn.disabled = false;
                registerBtn.style.opacity = '1';
            }
        }

        // ページ読み込み時の初期化
        window.onload = function() {
            // 入力フィールドにイベントリスナー追加
            document.getElementById('<%= txtUserID.ClientID %>').addEventListener('input', validateInput);
            document.getElementById('<%= txtUserName.ClientID %>').addEventListener('input', validateInput);
            document.getElementById('<%= txtPassword.ClientID %>').addEventListener('input', validateInput);
            
            // 初期検証実行
            validateInput();
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="register-container">
            <div class="register-title">
                <asp:Label ID="lblPageTitle" runat="server" />
            </div>
            
            <div class="register-form-group">
                <label for="txtUserID">ユーザーID</label>
                <asp:TextBox ID="txtUserID" runat="server" CssClass="register-input" placeholder="" MaxLength="20" />
            </div>
            
            <div class="register-form-group">
                <label for="txtUserName">名前</label>
                <asp:TextBox ID="txtUserName" runat="server" CssClass="register-input" placeholder="" MaxLength="20" />
            </div>
            
            <div class="register-form-group">
                <label for="txtPassword">パスワード</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="register-input" placeholder="" MaxLength="20" />
            </div>
            
            <asp:Button ID="btnRegister" runat="server" CssClass="register-button" OnClick="btnRegister_Click" />
            
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="register-error">
                    <asp:Label ID="lblError" runat="server" />
                </div>
            </asp:Panel>
            
            <asp:Panel ID="pnlSuccess" runat="server" Visible="false">
                <div class="register-success">
                    <asp:Label ID="lblSuccess" runat="server" />
                </div>
            </asp:Panel>

            <div style="text-align: center; margin-top: 20px; display: flex; gap: 10px; justify-content: center;">
                <asp:Button ID="btnBackToList" runat="server" Text="ユーザー一覧に戻る" CssClass="btn" OnClick="btnBackToList_Click" />
                <asp:Button ID="btnBackToMenu" runat="server" Text="メニューに戻る" CssClass="btn" OnClick="btnBackToMenu_Click" />
            </div>
        </div>
    </form>
</body>
</html>