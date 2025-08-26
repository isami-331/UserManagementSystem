using System;
using System.Data.SqlClient;
using System.Web.UI;
using UserManagementSystem.BLL;
using UserManagementSystem.DAL;
using UserManagementSystem.Models;

namespace UserManagementSystem.Pages
{
    /// <summary>
    /// ユーザー登録/更新ページ
    /// </summary>
    public partial class UserRegister : Page
    {
        private bool _isEditMode = false;
        private string _editUserID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
          

            // 編集モード判定
            _isEditMode = Request.QueryString["mode"] == "edit";

            if (_isEditMode)
            {
                _editUserID = Session["EditUserID"] as string;
                if (string.IsNullOrEmpty(_editUserID))
                {
                    Response.Redirect("~/Pages/UserList.aspx");
                    return;
                }
            }

            if (!Page.IsPostBack)
            {
                InitializePage();
                if (_isEditMode)
                {
                    LoadUserData(_editUserID);
                }
                else
                {
                    txtUserID.Focus();
                }
            }
        }

        /// <summary>
        /// ページ初期化
        /// </summary>
        private void InitializePage()
        {
            if (_isEditMode)
            {
                lblPageTitle.Text = "ユーザー更新画面";
                btnRegister.Text = "更新";
                txtUserID.ReadOnly = true; // 編集時はユーザーID変更不可
                txtUserID.CssClass += " readonly-input";
            }
            else
            {
                lblPageTitle.Text = "新規登録画面";
                btnRegister.Text = "登録";
                txtUserID.ReadOnly = false;
            }
        }

        /// <summary>
        /// ユーザーデータ読み込み（編集時）
        /// </summary>
        /// <param name="userID">読み込むユーザーID</param>
        private void LoadUserData(string userID)
        {
                var userRepository = new UserRepository();
                User user = userRepository.GetUserByID(userID);

                
                    txtUserID.Text = user.UserID;
                    txtUserName.Text = user.UserName;
                    txtPassword.Text = user.Password; 
                    txtUserName.Focus();
               
        }

        /// <summary>
        /// 登録/更新ボタンクリック
        /// </summary>
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                HideMessages();

                // 入力値取得・トリム
                string userID = txtUserID.Text.Trim();
                string userName = txtUserName.Text.Trim();
                string password = txtPassword.Text.Trim();

                // 入力値検証
                if (string.IsNullOrEmpty(userID))
                {
                    ShowError("ユーザーIDを入力してください。");
                    txtUserID.Focus();
                    return;
                }
                if (userID.Length > 20)
                {
                    ShowError("ユーザーIDは20文字以内で入力してください。");
                    txtUserID.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(userName))
                {
                    ShowError("名前を入力してください。");
                    txtUserName.Focus();
                    return;
                }
                if (userName.Length > 20)
                {
                    ShowError("名前は20文字以内で入力してください。");
                    txtUserName.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    ShowError("パスワードを入力してください。");
                    txtPassword.Focus();
                    return;
                }
                if (password.Length > 20)
                {
                    ShowError("パスワードは20文字以内で入力してください。");
                    txtPassword.Focus();
                    return;
                }


                bool result = false;
                string currentUserID = SessionManager.GetLoginUserID();
                var logManager = new LogManager();

                if (_isEditMode)
                {
                    // ユーザー更新処理
                    result = UpdateUser(userID, userName, password);

                    if (result)
                    {
                        logManager.RecordLog(currentUserID, "ユーザー更新", $"ユーザー '{userID}' ({userName}) の情報を更新しました");
                        ShowSuccess($"ユーザー '{userID}' を更新しました。");
                    }
                    else
                    {
                        ShowError("ユーザー更新に失敗しました。");
                    }
                }
                else
                {
                    // ユーザーID重複チェック（新規登録時のみ）
                    if (IsUserIDExists(userID))
                    {
                        ShowError($"ユーザーID '{userID}' は既に登録されています。");
                        txtUserID.Focus();
                        return;
                    }

                    // ユーザー登録処理
                    result = RegisterUser(userID, userName, password);

                    if (result)
                    {
                        logManager.RecordLog(currentUserID, "ユーザー登録", $"新規ユーザー '{userID}' ({userName}) を登録しました");
                        ShowSuccess($"ユーザー '{userID}' を登録しました。");
                        ClearForm();
                    }
                    else
                    {
                        ShowError("ユーザー登録に失敗しました。");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"システムエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// ユーザーID重複チェック
        /// </summary>
        private bool IsUserIDExists(string userID)
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string sql = "SELECT COUNT(*) FROM UserMaster WHERE UserID = @UserID";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception)
            {
                return true;
            }
        }

        /// <summary>
        /// ユーザー登録処理
        /// </summary>
        private bool RegisterUser(string userID, string userName, string password)
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string sql = @"INSERT INTO UserMaster (UserID, UserName, Password, CreateDate, SaveDate) 
                                 VALUES (@UserID, @UserName, @Password, @CreateDate, @SaveDate)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                        command.Parameters.AddWithValue("@SaveDate", DateTime.Now);

                        int affected = command.ExecuteNonQuery();
                        return affected > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// ユーザー更新処理
        /// </summary>
        private bool UpdateUser(string userID, string userName, string password)
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string sql = @"UPDATE UserMaster 
                                 SET UserName = @UserName, Password = @Password, SaveDate = @SaveDate
                                 WHERE UserID = @UserID";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@SaveDate", DateTime.Now);

                        int affected = command.ExecuteNonQuery();
                        return affected > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// ユーザー一覧に戻るボタンクリック
        /// </summary>
        protected void btnBackToList_Click(object sender, EventArgs e)
        {
            Session.Remove("EditUserID"); // セッションクリア
            Response.Redirect("~/Pages/UserList.aspx");
        }

        /// <summary>
        /// メニューに戻るボタンクリック
        /// </summary>
        protected void btnBackToMenu_Click(object sender, EventArgs e)
        {
            Session.Remove("EditUserID"); // セッションクリア
            Response.Redirect("~/Pages/Menu.aspx");
        }

        /// <summary>
        /// フォームクリア
        /// </summary>
        private void ClearForm()
        {
            txtUserID.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtUserID.Focus();
        }

        /// <summary>
        /// エラーメッセージ表示
        /// </summary>
        private void ShowError(string message)
        {
            lblError.Text = message;
            pnlError.Visible = true;
        }

        /// <summary>
        /// 成功メッセージ表示
        /// </summary>
        private void ShowSuccess(string message)
        {
            lblSuccess.Text = message;
            pnlSuccess.Visible = true;
        }

        /// <summary>
        /// メッセージ非表示
        /// </summary>
        private void HideMessages()
        {
            pnlError.Visible = false;
            pnlSuccess.Visible = false;
        }
    }
}