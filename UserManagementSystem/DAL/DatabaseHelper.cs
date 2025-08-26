using System;
using System.Configuration;
using System.Data.SqlClient;

namespace UserManagementSystem.DAL
{
    /// <summary>
    /// データベースヘルパークラス
    /// </summary>
    public static class DatabaseHelper
    {
        /// <summary>
        /// 接続文字列取得
        /// </summary>
        /// <returns>接続文字列</returns>
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["UserManagementDB"].ConnectionString;
        }

        /// <summary>
        /// SQL接続取得
        /// </summary>
        /// <returns>SQL接続オブジェクト</returns>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}