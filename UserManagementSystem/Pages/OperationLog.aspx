<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperationLog.aspx.cs" Inherits="UserManagementSystem.Pages.OperationLog" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>操作ログ - ユーザー管理システム</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" href="../Styles/common.css" />
    <link rel="stylesheet" href="../Styles/userlist.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="userlist-container">
            <div class="userlist-header">
                <div class="userlist-title">操作ログ</div>
                <div class="userlist-actions">
                    <asp:Button ID="btnRefresh" runat="server" Text="最新" CssClass="action-button" OnClick="btnRefresh_Click" />
                </div>
            </div>
            
            <asp:GridView ID="gvOperationLog" runat="server" AutoGenerateColumns="false" CssClass="userlist-table">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" />
                    <asp:BoundField DataField="UserID" HeaderText="ユーザーID" />
                    <asp:BoundField DataField="Operation" HeaderText="操作内容" />
                    <asp:BoundField DataField="SaveDate" HeaderText="実行日時" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
                    <asp:BoundField DataField="Detail" HeaderText="詳細" />
                </Columns>
                <EmptyDataTemplate>
                    <div style="text-align: center; padding: 20px; color: #666;">
                        操作ログが記録されていません。
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
            
            <div class="back-button">
                <asp:Button ID="btnBack" runat="server" Text="もどる" CssClass="btn" OnClick="btnBack_Click" />
            </div>
        </div>
    </form>
</body>
</html>