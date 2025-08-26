using System;
using System.Web;

namespace UserManagementSystem.BLL
{
    /// <summary>
    /// セッション管理クラス
    /// </summary>
    public static class SessionManager
    {
        private const string USER_ID_KEY = "UserID";
        private const string LOGIN_TIME_KEY = "LoginTime";
        private const int SESSION_TIMEOUT_MINUTES = 30;

        /// <summary>
        /// ログインユーザー設定
        /// </summary>
        /// <param name="userID">ユーザーID</param>
        public static void SetLoginUser(string userID)
        {
            if (HttpContext.Current?.Session != null)
            {
                HttpContext.Current.Session[USER_ID_KEY] = userID;
                HttpContext.Current.Session[LOGIN_TIME_KEY] = DateTime.Now;
                HttpContext.Current.Session.Timeout = SESSION_TIMEOUT_MINUTES;
            }
        }

        /// <summary>
        /// ログインユーザーID取得
        /// </summary>
        /// <returns>ユーザーID</returns>
        public static string GetLoginUserID()
        {
            return HttpContext.Current?.Session?[USER_ID_KEY] as string;
        }

        /// <summary>
        /// ログイン状態チェック
        /// </summary>
        /// <returns>true: ログイン中, false: 未ログイン</returns>
        public static bool IsLoggedIn()
        {
            string userID = GetLoginUserID();
            return !string.IsNullOrEmpty(userID);
        }

        /// <summary>
        /// セッションタイムアウトチェック
        /// </summary>
        /// <returns>true: タイムアウト, false: 有効</returns>
        public static bool IsSessionTimeout()
        {
            if (HttpContext.Current?.Session?[LOGIN_TIME_KEY] is DateTime loginTime)
            {
                return DateTime.Now.Subtract(loginTime).TotalMinutes > SESSION_TIMEOUT_MINUTES;
            }
            return true;
        }

        /// <summary>
        /// ログアウト処理
        /// </summary>
        public static void Logout()
        {
            if (HttpContext.Current?.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }
        }
    }
}