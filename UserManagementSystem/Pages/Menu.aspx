<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="UserManagementSystem.Pages.Menu" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>メニュー画面 - ユーザー管理システム</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" href="../Styles/common.css" />
    <link rel="stylesheet" href="../Styles/menu.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="menu-container">
            <div class="menu-title">メニュー画面</div>
            
            <!-- ウェルカムメッセージ -->
            <div class="welcome-message">
                <asp:Label ID="lblWelcome" runat="server" />
            </div>
            
            <div class="menu-items">
                <div class="menu-item">
                    <asp:LinkButton ID="lbtnUserList" runat="server" CssClass="menu-box" OnClick="btnUserList_Click">
                        <div>ユーザー一覧</div>
                    </asp:LinkButton>
                </div>
                
                <div class="menu-item">
                    <asp:LinkButton ID="lbtnUserRegister" runat="server" CssClass="menu-box" OnClick="btnUserRegister_Click">
                        <div>ユーザー登録</div>
                    </asp:LinkButton>
                </div>
                
                <div class="menu-item">
                    <asp:LinkButton ID="lbtnOperationLog" runat="server" CssClass="menu-box" OnClick="btnOperationLog_Click">
                        <div>操作ログ</div>
                    </asp:LinkButton>
                </div>
            </div>
            
            <div class="logout-section">
                <asp:Button ID="btnLogout" runat="server" Text="ログアウト" CssClass="logout-button" OnClick="btnLogout_Click" />
            </div>
        </div>
    </form>
</body>
</html>3