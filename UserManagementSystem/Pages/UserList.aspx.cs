using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserManagementSystem.BLL;
using UserManagementSystem.DAL;
using UserManagementSystem.Models;

namespace UserManagementSystem.Pages
{
    /// <summary>
    /// ユーザー一覧ページ
    /// </summary>
    public partial class UserList : Page
    {
        private UserManager _userManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            // ログイン状態チェック
            if (!SessionManager.IsLoggedIn() || SessionManager.IsSessionTimeout())
            {
                SessionManager.Logout();
                Response.Redirect("~/Pages/Login.aspx");
                return;
            }

            _userManager = new UserManager();

            if (!Page.IsPostBack)
            {
                LoadUserList();
            }
        }

        /// <summary>
        /// JavaScript用の現在のユーザーID取得メソッド
        /// </summary>
        /// <returns>現在のログインユーザーID</returns>
        protected string GetCurrentUserID()
        {
            try
            {
                return SessionManager.GetLoginUserID() ?? "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// ユーザー一覧読み込み
        /// </summary>
        private void LoadUserList()
        {
            try
            {
                string currentUserID = SessionManager.GetLoginUserID();
                List<User> users = _userManager.GetAllUsers(currentUserID);

                gvUserList.DataSource = users;
                gvUserList.DataBind();

                HideMessage();
            }
            catch (Exception ex)
            {
                ShowMessage($"ユーザー一覧の読み込みでエラーが発生しました: {ex.Message}", false);
            }
        }

        /// <summary>
        /// GridView 行データバインド（選択切り替え機能対応）
        /// </summary>
        protected void gvUserList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                User user = (User)e.Row.DataItem;
                string currentUserID = SessionManager.GetLoginUserID();

                // 行にdata-userid属性を追加
                e.Row.Attributes["data-userid"] = user.UserID;

                // 行全体をクリックで選択切り替え
                e.Row.Attributes["onclick"] = $"toggleRowSelection(this, '{user.UserID}');";

                // カーソルポインタを設定
                e.Row.Style["cursor"] = "pointer";

                // ホバー効果のためのクラス追加
                e.Row.CssClass = "selectable-row";

                // 自分自身またはadminユーザーの場合は視覚的に区別
                if (user.UserID == currentUserID)
                {
                    e.Row.CssClass += " current-user-row";
                }
                else if (user.UserID == "admin")
                {
                    e.Row.CssClass += " admin-user-row";
                }
                else
                {
                }
            }
        }

        /// <summary>
        /// 最新ボタンクリック
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUserList();
            hdnSelectedUserID.Value = ""; // 選択状態をクリア

            // JavaScript で選択解除
            string script = "clearSelection();";
            ClientScript.RegisterStartupScript(this.GetType(), "RefreshAndClear", script, true);

            ShowMessage("ユーザー一覧を更新しました。", true);
        }

        /// <summary>
        /// 新規ボタンクリック
        /// </summary>
        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/UserRegister.aspx");
        }

        /// <summary>
        /// 更新ボタンクリック
        /// </summary>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string selectedUserID = hdnSelectedUserID.Value;

                // 選択されたユーザーが存在するかチェック
                var userRepository = new UserRepository();
                User selectedUser = userRepository.GetUserByID(selectedUserID);

                // 選択されたユーザーIDをセッションに保存して、ユーザー登録画面に遷移
                Session["EditUserID"] = selectedUserID;
                Response.Redirect("~/Pages/UserRegister.aspx?mode=edit");
         
        }

        /// <summary>
        /// 削除ボタンクリック
        /// </summary>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string selectedUserID = hdnSelectedUserID.Value;
            string currentUserID = SessionManager.GetLoginUserID();
 
            DeleteUser(selectedUserID);
        }

        /// <summary>
        /// ユーザー削除
        /// </summary>
        /// <param name="userIDToDelete">削除するユーザーID</param>
        private void DeleteUser(string userIDToDelete)
        {
           
                string currentUserID = SessionManager.GetLoginUserID();
                bool result = _userManager.DeleteUser(userIDToDelete, currentUserID);

               
                    ShowMessage($"ユーザー '{userIDToDelete}' を削除しました。", true);
                    hdnSelectedUserID.Value = ""; // 選択状態をクリア

                    // JavaScript で選択解除s
                    string script = "clearSelection();";
                    ClientScript.RegisterStartupScript(this.GetType(), "ClearAfterDelete", script, true);

                    LoadUserList(); // 一覧を再読み込み
             
        }

        /// <summary>
        /// もどるボタンクリック
        /// </summary>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }

        /// <summary>
        /// メッセージ表示
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="isSuccess">成功メッセージかどうか</param>
        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;

            if (isSuccess)
            {
                alertDiv.Attributes["class"] = "success-message";
            }
            else
            {
                alertDiv.Attributes["class"] = "error-message";
            }

            pnlMessage.Visible = true;
        }

        /// <summary>
        /// メッセージ非表示
        /// </summary>
        private void HideMessage()
        {
            pnlMessage.Visible = false;
        }
    }
}