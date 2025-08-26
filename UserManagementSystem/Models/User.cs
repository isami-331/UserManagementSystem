using System;

namespace UserManagementSystem.Models
{
    /// <summary>
    /// ユーザーエンティティクラス
    /// </summary>
    public class User
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime SaveDate { get; set; }

        public User()
        {
            UserID = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            CreateDate = DateTime.Now;
            SaveDate = DateTime.Now;
        }
    }
}