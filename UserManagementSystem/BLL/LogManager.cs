using System;
using System.Collections.Generic;
using UserManagementSystem.Models;
using UserManagementSystem.DAL;

namespace UserManagementSystem.BLL
{
    /// <summary>
    /// ログ管理ビジネスロジッククラス
    /// </summary>
    public class LogManager
    {
        private LogRepository _logRepository;

        public LogManager()
        {
            _logRepository = new LogRepository();
        }

        /// <summary>
        /// 操作ログ記録
        /// </summary>
        /// <param name="userID">ユーザーID</param>
        /// <param name="operation">操作内容</param>
        /// <param name="detail">詳細</param>
        public void RecordLog(string userID, string operation, string detail)
        {
            try
            {
                var log = new OperationLog
                {
                    UserID = userID ?? "system",
                    OperationDate = DateTime.Now,
                    Operation = operation,
                    Detail = detail
                };

                _logRepository.InsertLog(log);
            }
            catch (Exception)
            {
                // ログ記録エラーは無視（システム継続のため）
            }
        }

        /// <summary>
        /// 操作ログ一覧取得
        /// </summary>
        /// <param name="currentUserID">操作実行ユーザーID</param>
        /// <returns>ログリスト</returns>
        public List<OperationLog> GetOperationLogs(string currentUserID)
        {
           
                RecordLog(currentUserID, "操作ログ表示", "操作ログ画面を表示しました");
                return _logRepository.GetAllLogs();
            
          
        }
    }
}