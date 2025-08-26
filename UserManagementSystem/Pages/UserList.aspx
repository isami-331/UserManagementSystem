<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="UserManagementSystem.Pages.UserList" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>ユーザー一覧画面 - ユーザー管理システム</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" href="../Styles/common.css" />
    <link rel="stylesheet" href="../Styles/userlist.css" />
    <script type="text/javascript">
        // サーバーサイドから渡される現在のユーザーID
        var currentUserID = '<%= GetCurrentUserID() %>';

        // 行選択切り替え機能
        function toggleRowSelection(rowElement, userID) {
            var hiddenField = document.getElementById('<%= hdnSelectedUserID.ClientID %>');
            var currentSelectedID = hiddenField.value;

            if (currentSelectedID === userID) {
                // 既に選択されている場合は非選択にする
                rowElement.classList.remove('selected-row');
                hiddenField.value = '';
                updateButtonStates('');
            } else {
                // 他の行が選択されている場合は先に解除
                var allRows = document.querySelectorAll('.userlist-table tr[data-userid]');
                allRows.forEach(function (row) {
                    row.classList.remove('selected-row');
                });

                // 現在の行を選択
                rowElement.classList.add('selected-row');
                hiddenField.value = userID;
                updateButtonStates(userID);
            }
        }

        // ボタン状態更新
        function updateButtonStates(selectedUserID) {
            var updateBtn = document.getElementById('<%= btnUpdate.ClientID %>');
            var deleteBtn = document.getElementById('<%= btnDelete.ClientID %>');
            
            if (selectedUserID === '') {
                // 未選択時
                updateBtn.disabled = true;
                deleteBtn.disabled = true;
                updateBtn.style.opacity = '0.6';
                deleteBtn.style.opacity = '0.6';
            } else {
                // 選択時
                updateBtn.disabled = false;
                updateBtn.style.opacity = '1';
                
                // 自分自身またはadminの場合は削除不可
                if (selectedUserID === currentUserID) {
                    deleteBtn.disabled = true;
                    deleteBtn.style.opacity = '0.6';
                } else if (selectedUserID === 'admin') {
                    deleteBtn.disabled = true;
                    deleteBtn.style.opacity = '0.6';
                } else {
                    deleteBtn.disabled = false;
                    deleteBtn.style.opacity = '1';
                }
            }
        }

        // 選択解除（プログラム用）
        function clearSelection() {
            var allRows = document.querySelectorAll('.userlist-table tr[data-userid]');
            allRows.forEach(function(row) {
                row.classList.remove('selected-row');
            });
            
            document.getElementById('<%= hdnSelectedUserID.ClientID %>').value = '';
            updateButtonStates('');
        }

        // ページ読み込み時の初期化
        window.onload = function() {
            updateButtonStates('');
        };

        // 削除確認
        function confirmDelete() {
            var selectedUserID = document.getElementById('<%= hdnSelectedUserID.ClientID %>').value;
           
            return confirm('ユーザー「' + selectedUserID + '」を削除しますか？\n\n削除後は元に戻せません。');
        }

        // 更新確認
        function confirmUpdate() {
            var selectedUserID = document.getElementById('<%= hdnSelectedUserID.ClientID %>').value;
           
            return confirm('ユーザー「' + selectedUserID + '」の情報を更新しますか？');
        }

      
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="userlist-container">
            <div class="userlist-header">
                <div class="userlist-title">ユーザー一覧画面</div>
                <div class="userlist-actions">
                    <asp:Button ID="btnRefresh" runat="server" Text="最新" CssClass="action-button" OnClick="btnRefresh_Click" />
                    <asp:Button ID="btnAddUser" runat="server" Text="新規" CssClass="action-button" OnClick="btnAddUser_Click" />
                    <asp:Button ID="btnUpdate" runat="server" Text="更新" CssClass="action-button" OnClick="btnUpdate_Click" OnClientClick="return confirmUpdate();" />
                    <asp:Button ID="btnDelete" runat="server" Text="削除" CssClass="action-button action-button-delete" OnClick="btnDelete_Click" OnClientClick="return confirmDelete();" />
                </div>
            </div>
            
            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <div id="alertDiv" runat="server" class="error-message">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
            </asp:Panel>

            <!-- 選択されたユーザーIDを保持する隠しフィールド -->
            <asp:HiddenField ID="hdnSelectedUserID" runat="server" />
            
            <asp:GridView ID="gvUserList" runat="server" AutoGenerateColumns="false" CssClass="userlist-table selectable-table"
                         OnRowDataBound="gvUserList_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="UserID" HeaderText="ID" />
                    <asp:BoundField DataField="UserName" HeaderText="名前" />
                    <asp:BoundField DataField="CreateDate" HeaderText="登録日時" DataFormatString="{0:yyyy/MM/dd}" />
                </Columns>
                <EmptyDataTemplate>
                    <div style="text-align: center; padding: 20px; color: #666;">
                        ユーザーが登録されていません。
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