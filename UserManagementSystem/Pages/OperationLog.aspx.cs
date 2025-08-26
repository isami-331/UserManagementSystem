using System;
using System.Web.UI;
using UserManagementSystem.BLL;

namespace UserManagementSystem.Pages
{
    /// <summary>
    /// 操作ログページs
    /// </summary>
    public partial class OperationLog : Page
    {
        private LogManager _logManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            // ログイン状態チェック
            if (!SessionManager.IsLoggedIn() || SessionManager.IsSessionTimeout())
            {
                SessionManager.Logout();
                Response.Redirect("~/Pages/Login.aspx");
                return;
            }

            _logManager = new LogManager();

            if (!Page.IsPostBack)
            {
                LoadOperationLogs();
            }
        }

        /// <summary>
        /// 操作ログ一覧読み込み
        /// </summary>
        private void LoadOperationLogs()
        {
            
                string currentUserID = SessionManager.GetLoginUserID();
                var logs = _logManager.GetOperationLogs(currentUserID);

                gvOperationLog.DataSource = logs;
                gvOperationLog.DataBind();
          
        }

        /// <summary>
        /// 最新ボタンクリック
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOperationLogs();
        }

        /// <summary>
        /// もどるボタンクリック
        /// </summary>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }
    }
}