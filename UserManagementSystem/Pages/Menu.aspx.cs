using System;
using System.Web.UI;
using UserManagementSystem.BLL;
using UserManagementSystem.DAL;

namespace UserManagementSystem.Pages
{
    /// <summary>
    /// メニューページ
    /// </summary>
    public partial class Menu : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // ログイン状態チェック
            if (!SessionManager.IsLoggedIn() || SessionManager.IsSessionTimeout())
            {
                SessionManager.Logout();
                Response.Redirect("~/Pages/Login.aspx");
                return;
            }

            if (!Page.IsPostBack)
            {
                InitializePage();
                RecordMenuAccess(); // メニューアクセスログを記録
            }
        }

        /// <summary>
        /// ページ初期化
        /// </summary>
        private void InitializePage()
        {
            try
            {
                // 現在のユーザー情報取得
                string currentUserID = SessionManager.GetLoginUserID();

                // UserName取得
                string userName = currentUserID;
                if (!string.IsNullOrEmpty(currentUserID))
                {
                    var userRepository = new UserRepository();
                    var user = userRepository.GetUserByID(currentUserID);
                    
                        userName = user.UserName;
                    
                }

                // ウェルカムメッセージ設定
                if (lblWelcome != null)
                {
                    lblWelcome.Text = $"ようこそ、{userName} さん";
                }
            }
            catch (Exception)
            {
                // 初期化エラーは何もしない（画面表示を優先）
            }
        }

        /// <summary>
        /// メニューアクセスログ記録
        /// </summary>
        private void RecordMenuAccess()
        {
            try
            {
                string currentUserID = SessionManager.GetLoginUserID();

                if (!string.IsNullOrEmpty(currentUserID))
                {
                    var logManager = new LogManager();
                    logManager.RecordLog(
                        currentUserID,
                        "メニュー表示",
                        "メニュー画面にアクセスしました"
                    );
                }
            }
            catch (Exception)
            {
                // ログ記録エラーは画面表示に影響させない
            }
        }

        /// <summary>
        /// ユーザー一覧ボタンクリック
        /// </summary>
        protected void btnUserList_Click(object sender, EventArgs e)
        {
          
            // 画面遷移
            Response.Redirect("~/Pages/UserList.aspx");
        }

        /// <summary>
        /// ユーザー登録ボタンクリック
        /// </summary>
        protected void btnUserRegister_Click(object sender, EventArgs e)
        {
            // 画面遷移
            Response.Redirect("~/Pages/UserRegister.aspx");
        }

        /// <summary>
        /// 操作ログボタンクリック
        /// </summary>
        protected void btnOperationLog_Click(object sender, EventArgs e)
        {
           
            // 画面遷移
            Response.Redirect("~/Pages/OperationLog.aspx");
        }

        /// <summary>
        /// ログアウトボタンクリック
        /// </summary>
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                string currentUserID = SessionManager.GetLoginUserID();

                // ログアウトログ記録
                if (!string.IsNullOrEmpty(currentUserID))
                {
                    var logManager = new LogManager();
                    logManager.RecordLog(currentUserID, "ログアウト", "システムからログアウトしました");
                }

                // セッション終了
                SessionManager.Logout();
                Response.Redirect("~/Pages/Login.aspx");
            }
            catch (Exception)
            {
                // エラーでもログアウト処理は実行
                SessionManager.Logout();
                Response.Redirect("~/Pages/Login.aspx");
            }
        }

        /// <summary>
        /// 画面遷移ログ記録
        /// </summary>
        /// <param name="operation">操作内容</param>
        /// <param name="detail">詳細</param>
        private void RecordNavigationLog(string operation, string detail)
        {
            try
            {
                string currentUserID = SessionManager.GetLoginUserID();

                if (!string.IsNullOrEmpty(currentUserID))
                {
                    var logManager = new LogManager();
                    logManager.RecordLog(currentUserID, operation, detail);
                }
            }
            catch (Exception)
            {
                // 画面遷移ログエラーは表示に影響させない
            }
        }
    }
}