using System;
using System.Windows;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace OracleCodeGenerator
{
    public class DBHelper
    {
        /// <summary>
        /// 声明连接
        /// </summary>
        protected static OracleConnection Connection;

        /// <summary>
        /// 返回Connection
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="port">端口</param>
        /// <param name="sid">服务名称</param>
        /// <param name="user">用户</param>
        /// <param name="pwd">密码</param>
        /// <returns>OleDbConnection</returns>
        private static OracleConnection ConnForOracle(string ip, string port, string sid, string user, string pwd)
        {
            string connStr;
            connStr = "Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = " + ip + ")(PORT = " + port + "))) (CONNECT_DATA = (SERVICE_NAME = " + sid + ")));User ID=" + user + ";Password=" + pwd + ";";
            Connection = new OracleConnection(connStr);
            return Connection;
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="port">端口</param>
        /// <param name="sid">服务名称</param>
        /// <param name="user">用户</param>
        /// <param name="pwd">密码</param>
        public static bool OpenConnection(string ip, string port, string sid, string user, string pwd)
        {
            ConnForOracle(ip, port, sid, user, pwd);
            try
            {
                //不为空 并且 是关闭或者断了的情况下，才连接
                if (Connection != null && (Connection.State == System.Data.ConnectionState.Closed || Connection.State == System.Data.ConnectionState.Broken))
                {
                    Connection.Open();
                    ReturnOwner = Select("SELECT OWNER, TABLE_NAME FROM ALL_TAB_COMMENTS ORDER BY OWNER, TABLE_NAME");
                }
                MessageBox.Show(Connection.State.ToString());
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public static void CloseConnection()
        {
            //不为空 并且 是打开状态下才关闭
            if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        private static DataTable Select(string sql)
        {
            OracleCommand cmd = new OracleCommand(sql, Connection);
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="owner">拥有者</param>
        public static void GetTableName(string owner)
        {
            ReturnTableName = Select("SELECT OWNER, TABLE_NAME, TABLE_TYPE, COMMENTS FROM ALL_TAB_COMMENTS WHERE OWNER='" + owner + "' ORDER BY OWNER, TABLE_TYPE, TABLE_NAME");
        }
        /// <summary>
        /// 获取表内容
        /// </summary>
        /// <param name="owner">拥有者</param>
        /// <param name="name">表名</param>
        public static void GetTableContent(string owner, string name)
        {
            ReturnTableContent = Select("SELECT COLUMN_NAME, DATA_TYPE, NULLABLE, DATA_LENGTH,(SELECT COMMENTS FROM ALL_COL_COMMENTS WHERE TABLE_NAME = '" + name + "' AND OWNER = '" + owner + "' AND COLUMN_NAME = a.COLUMN_NAME)COMMENTS FROM ALL_TAB_COLUMNS a WHERE TABLE_NAME = '" + name + "' AND OWNER = '" + owner + "' ORDER BY NULLABLE, COLUMN_NAME ");
        }
        #region 字段
        /// <summary>
        /// 要返回拥有者
        /// </summary>
        private static DataTable returnOwner;
        public static DataTable ReturnOwner
        {
            get
            {
                return returnOwner;
            }

            set
            {
                returnOwner = value;

            }
        }

        /// <summary>
        /// 返回表名
        /// </summary>
        private static DataTable returnTableName;
        public static DataTable ReturnTableName
        {
            get
            {
                return returnTableName;
            }

            set
            {
                returnTableName = value;
            }
        }
        /// <summary>
        /// 返回表内容
        /// </summary>
        private static DataTable returnTableContent;

        public static DataTable ReturnTableContent
        {
            get
            {
                return returnTableContent;
            }

            set
            {
                returnTableContent = value;
            }
        }
        #endregion
    }
}
