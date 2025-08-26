using System;
using System.Web.UI;
using UserManagementSystem.BLL;

namespace UserManagementSystem.Pages
{
    /// <summary>
    /// ログインページ
    /// </summary>
    public partial class Login : Page
    {
        private UserManager _userManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userManager = new UserManager();

            if (!Page.IsPostBack)
            {
                // 既にログイン済みの場合はメニューへリダイレクト
                if (SessionManager.IsLoggedIn())
                {
                    Response.Redirect("~/Pages/Menu.aspx");
                }

                // フォーカス設定
                txtUserID.Focus();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                HideError();

                // 入力値取得・トリム
                string userID = txtUserID.Text.Trim();
                string password = txtPassword.Text.Trim();

                // 入力値検証
                if (string.IsNullOrEmpty(userID))
                {
                    ShowError("ユーザーIDを入力してください。");
                    txtUserID.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    ShowError("パスワードを入力してください。");
                    txtPassword.Focus();
                    return;
                }

                // 認証処理
                bool isAuthenticated = _userManager.AuthenticateUser(userID, password);

                if (isAuthenticated)
                {
                    // ログイン成功
                    SessionManager.SetLoginUser(userID);

                    // リダイレクト先の決定
                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        Response.Redirect(returnUrl);
                    }
                    else
                    {
                        Response.Redirect("~/Pages/Menu.aspx");
                    }
                }
                else
                {
                    // ログイン失敗
                    ShowError("ユーザーIDまたはパスワードが間違っています。");
                    txtPassword.Text = "";
                    txtUserID.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowError($"システムエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// エラーメッセージ表示
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        private void ShowError(string message)
        {
            lblError.Text = message;
            pnlError.Visible = true;
        }

        /// <summary>
        /// エラーメッセージ非表示
        /// </summary>
        private void HideError()
        {
            pnlError.Visible = false;
        }
    }
}