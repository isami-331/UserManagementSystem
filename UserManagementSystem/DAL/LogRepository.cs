using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using UserManagementSystem.Models;

namespace UserManagementSystem.DAL
{
    /// <summary>
    /// ログデータアクセスクラス
    /// </summary>
    public class LogRepository
    {
        /// <summary>
        /// 操作ログ挿入
        /// </summary>
        /// <param name="log">ログ情報</param>
        public void InsertLog(OperationLog log)
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string sql = @"INSERT INTO OperationLog (SaveDate, UserID, Operation, Detail) 
                                 VALUES (@SaveDate, @UserID, @Operation, @Detail)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@SaveDate", log.SaveDate);
                        command.Parameters.AddWithValue("@UserID", log.UserID);
                        command.Parameters.AddWithValue("@Operation", log.Operation);
                        command.Parameters.AddWithValue("@Detail", log.Detail ?? string.Empty);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // ログ記録エラーは無視
            }
        }

        /// <summary>
        /// 全ログ取得
        /// </summary>
        /// <returns>ログリスト</returns>
        public List<OperationLog> GetAllLogs()
        {
            var logs = new List<OperationLog>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string sql = @"SELECT TOP 100 ID, UserID, SaveDate, Operation, Detail 
                             FROM OperationLog ORDER BY SaveDate DESC";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logs.Add(new OperationLog
                        {
                            ID = (int)reader["ID"],
                            UserID = reader["UserID"].ToString(),
                            SaveDate = (DateTime)reader["SaveDate"],
                            Operation = reader["Operation"].ToString(),
                            Detail = reader["Detail"].ToString()
                        });
                    }
                }
            }

            return logs;
        }
    }
}