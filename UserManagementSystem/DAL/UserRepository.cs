using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using UserManagementSystem.Models;

namespace UserManagementSystem.DAL
{
    /// <summary>
    /// ユーザーデータアクセスクラス
    /// </summary>
    public class UserRepository
    {
        public UserRepository()
        {
        }

        /// <summary>
        /// ユーザー認証
        /// </summary>
        /// <param name="userID">ユーザーID</param>
        /// <param name="password">パスワード</param>
        /// <returns>認証結果</returns>
        public bool AuthenticateUser(string userID, string password)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string sql = "SELECT COUNT(*) FROM UserMaster WHERE UserID = @UserID AND Password = @Password";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// 全ユーザー取得
        /// </summary>
        /// <returns>ユーザーリスト</returns>
        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string sql = "SELECT UserID, UserName, CreateDate, SaveDate FROM UserMaster ORDER BY CreateDate DESC";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserID = reader["UserID"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            CreateDate = (DateTime)reader["CreateDate"],
                            SaveDate = (DateTime)reader["SaveDate"]
                        });
                    }
                }
            }

            return users;
        }

        /// <summary>
        /// ユーザーID による取得
        /// </summary>
        /// <param name="userID">ユーザーID</param>
        /// <returns>ユーザー情報</returns>
        public User GetUserByID(string userID)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string sql = "SELECT UserID, UserName, CreateDate, SaveDate FROM UserMaster WHERE UserID = @UserID";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader["UserID"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                CreateDate = (DateTime)reader["CreateDate"],
                                SaveDate = (DateTime)reader["SaveDate"]
                            };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// ユーザー削除
        /// </summary>
        /// <param name="userID">削除するユーザーID</param>
        /// <returns>削除結果</returns>
        public bool DeleteUser(string userID)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string sql = "DELETE FROM UserMaster WHERE UserID = @UserID";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);
                    int affected = command.ExecuteNonQuery();
                    return affected > 0;
                }
            }
        }
    }
}