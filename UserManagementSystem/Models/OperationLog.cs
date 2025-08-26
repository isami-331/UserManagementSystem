using System;

namespace UserManagementSystem.Models
{
    /// <summary>
    /// 操作ログエンティティクラス
    /// </summary>
    public class OperationLog
    {
        public int ID { get; set; }       
        public DateTime SaveDate { get; set; }  
        public string UserID { get; set; }
        public string Operation { get; set; }
        public string Detail { get; set; }
        public DateTime OperationDate
        {
            get { return SaveDate; }
            set { SaveDate = value; }
        }

        public OperationLog()
        {
            ID = 0;
            UserID = string.Empty;
            SaveDate = DateTime.Now;
            Operation = string.Empty;
            Detail = string.Empty;
        }
    }
}