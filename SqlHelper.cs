using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace TOOL
{
    /// <summary>   
    /// 数据库执行控制类   
    /// </summary>   
    public class SqlHelper
    {
        private static string DBStr = "server=122.226.76.100,56782;database=EXPRESS_DATACENETER;User ID=sto_bi;Password=stobi#@!123s";


        #region 数据库执行操作   

        /* 执行更新删除插入数据库操作,成功则返回true　*/
        /// <summary>   
        /// 功能：执行更新删除插入数据库操作,成功则返回true   
        /// </summary>   
        /// <param name="strSql"></param>   
        /// <returns></returns>   
        public static bool ExecuteSql(string strSql)
        {
            SqlConnection conn = new SqlConnection(DBStr);
            conn.Open();  
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = strSql;
            try
            {
                cm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                TOOL.LogTool.WriteLog(ex.Message);
                return false;
            }
            finally
            {
                cm.Dispose();
                conn.Close();
            }


        }
        /* 执行查询数据库操作是否有记录　*/
        /// <summary>   
        /// 功能：执行查询数据库操作,返回是否有记录　   
        /// </summary>   
        /// <param name="strSql"></param>   
        public static bool SelectExist(string strSql)
        {
            SqlConnection conn = new SqlConnection(DBStr);
            conn.Open();
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = strSql;
            try
            {
                SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                TOOL.LogTool.WriteLog(ex.Message);
                return false;
            }
            finally
            {
                cm.Dispose();
                conn.Close();
            }

        }
       
        /// <summary>   
        /// 功能：执行数据库操作返回影响行数　   
        /// </summary>   
        /// <param name="strSql"></param>   
        public static int ExecuteNonQueryCount(string strSql)
        {
            SqlConnection conn = new SqlConnection(DBStr);
            conn.Open();
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = strSql;
            try
            {
                int count = cm.ExecuteNonQuery();
                return count;
            }
            catch (Exception ex)
            {
                TOOL.LogTool.WriteLog(ex.Message);
                return -1;
            }
            finally
            {
                cm.Dispose();
                conn.Close();
            }

        }

        public static SqlDataReader SelectSqlReader(string strSQL)
        {
            SqlConnection conn = new SqlConnection(DBStr);
            conn.Open();
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = strSQL;
            SqlDataReader reader = null;
            try
            {
                reader = cm.ExecuteReader();

            }
            catch (Exception e)
            {
                TOOL.LogTool.WriteLog(e.Message);
            }
            finally
            {
                cm.Dispose();
                conn.Close();
            }
            return reader;
        }
        /*执行查询操作*/
        /// <summary>   
        /// 功能：执行查询操作,返回数据集 
        /// </summary>   
        /// <param name="strSql"></param>   
        /// <param name="ds">返回数据集</param>   
        /// <returns></returns>   
        public static void ExecuteSelectCmmond(string strSQL,ref DataSet ds, string strTableName)
        {
            SqlConnection conn = new SqlConnection(DBStr);
            conn.Open();
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = strSQL;
            SqlDataAdapter da = new SqlDataAdapter(cm);
            try
            {
                da.Fill(ds, strTableName);
            }
            catch (Exception e)
            {
                TOOL.LogTool.WriteLog(e.Message);
            }
            finally
            {
                cm.Dispose();
                conn.Close();
            }
        }
        /// <summary>   
        /// 功能：执行查询操作,返回数据集 
        /// </summary>   
        /// <param name="strSql"></param>   
        /// <param name="ds">返回数据集</param>   
        /// <returns></returns>  
        public static DataSet ExecuteSelectCmmond(string strSQL)
        {
            SqlConnection conn = new SqlConnection(DBStr);
            conn.Open();
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = strSQL;
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception e)
            {
                TOOL.LogTool.WriteLog(e.Message);
            }
            finally
            {
                cm.Dispose();
                conn.Close();
            }
            return ds;
        }
        public static DataTable SqlTable(string strSQL)
        {
            SqlConnection conn = new SqlConnection(DBStr);
            conn.Open();
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = strSQL;
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
            }
            catch (Exception e)
            {
                TOOL.LogTool.WriteLog(e.Message);
            }
            finally
            {
                cm.Dispose();
                conn.Close();
            }
            return dt;
        }
        /* 查询表结构操作*/
        /// <summary>   
        ///功能： 查询表结构操作   
        /// </summary>   
        /// <param name="strSql"></param>   
        /// <param name="ds">返回数据集</param>   
        /// <returns></returns>   
        public static DataSet GetTableCol(string TableName)
        {
           
            string strSql = "select * from [" + TableName + "] where 1> 2 ";
            SqlConnection conn = new SqlConnection(DBStr);
            conn.Open();
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = strSql;
            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(cm);
            try
            {
                da.Fill(ds);
            }
            catch (Exception e)
            {
                TOOL.LogTool.WriteLog(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return ds;
        }
        public static bool AddDataTableToDB(DataTable source,string tableName)
        {
            SqlTransaction tran = null;//声明一个事务对象  
            try
            {
                using (SqlConnection conn = new SqlConnection(DBStr))
                {
                    conn.Open();//打开链接  
                    using (tran = conn.BeginTransaction())
                    {
                        using (SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran))
                        {
                            copy.DestinationTableName = tableName;  //指定服务器上目标表的名称  
                            copy.WriteToServer(source);                      //执行把DataTable中的数据写入DB  
                            tran.Commit();                                      //提交事务  
                            return true;                                        //返回True 执行成功！  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (null != tran)
                    tran.Rollback();
                //LogHelper.Add(ex);  
                return false;//返回False 执行失败！  
            }
        }


        #endregion


    }
}

