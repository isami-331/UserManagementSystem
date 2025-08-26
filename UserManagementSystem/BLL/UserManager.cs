using System;
using System.Collections.Generic;
using UserManagementSystem.Models;
using UserManagementSystem.DAL;

namespace UserManagementSystem.BLL
{
    /// <summary>
    /// ユーザー管理ビジネスロジッククラス
    /// </summary>
    public class UserManager
    {
        private UserRepository _userRepository;
        private LogManager _logManager;

        public UserManager()
        {
            _userRepository = new UserRepository();
            _logManager = new LogManager();
        }

        /// <summary>
        /// ユーザー認証
        /// </summary>
        /// <param name="userID">ユーザーID</param>
        /// <param name="password">パスワード</param>
        /// <returns>認証結果（true: 成功, false: 失敗）</returns>
        public bool AuthenticateUser(string userID, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(password))
                {
                    return false;
                }

                bool isAuthenticated = _userRepository.AuthenticateUser(userID, password);

                string operation = isAuthenticated ? "ログイン成功" : "ログイン失敗";
                string detail = isAuthenticated ? "システムにログインしました" : $"ログイン試行失敗: {userID}";
                _logManager.RecordLog(userID, operation, detail);

                return isAuthenticated;
            }
            catch (Exception ex)
            {
                _logManager.RecordLog(userID, "ログインエラー", $"エラー詳細: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 全ユーザー取得
        /// </summary>
        /// <param name="currentUserID">操作実行ユーザーID</param>
        /// <returns>ユーザーリスト</returns>
        public List<User> GetAllUsers(string currentUserID)
        {
            try
            {
                var users = _userRepository.GetAllUsers();
                _logManager.RecordLog(currentUserID, "ユーザー一覧表示", "ユーザー一覧画面を表示しました");
                return users;
            }
            catch (Exception ex)
            {
                _logManager.RecordLog(currentUserID, "ユーザー一覧表示エラー", $"エラー詳細: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// ユーザー削除
        /// </summary>
        /// <param name="userID">削除するユーザーID</param>
        /// <param name="currentUserID">操作実行ユーザーID</param>
        /// <returns>削除結果（true: 成功, false: 失敗）</returns>
        public bool DeleteUser(string userID, string currentUserID)
        {
            try
            {
                if (userID == currentUserID)
                {
                    _logManager.RecordLog(currentUserID, "ユーザー削除失敗", "自分自身は削除できません");
                    throw new InvalidOperationException("自分自身は削除できません。");
                }

                var targetUser = _userRepository.GetUserByID(userID);
                if (targetUser == null)
                {
                    _logManager.RecordLog(currentUserID, "ユーザー削除失敗", $"削除対象ユーザー不存在: {userID}");
                    throw new InvalidOperationException($"ユーザーID '{userID}' は存在しません。");
                }

                bool result = _userRepository.DeleteUser(userID);

                if (result)
                {
                    _logManager.RecordLog(currentUserID, "ユーザー削除", $"ユーザー削除: {userID} ({targetUser.UserName})");
                }
                else
                {
                    _logManager.RecordLog(currentUserID, "ユーザー削除失敗", $"削除処理失敗: {userID}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logManager.RecordLog(currentUserID, "ユーザー削除エラー", $"エラー詳細: {ex.Message}");
                throw;
            }
        }
    }
}