using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hermina_ABRTL.clsLogict
{
    public class clsDBSQLConnection : IDisposable
    {
        public bool QueryCommand(
           String ConnetionString,
           String SqlQuery,
           ref SqlParameter[] dbParam,
           out DataSet myDataSet,
           out string strError)
        {
            bool blnResult = true;
            myDataSet = new DataSet();
            strError = "";
            try
            {
                using (SqlConnection _sqlConn = new SqlConnection(ConnetionString))
                {
                    string _sql = SqlQuery;
                    using (SqlCommand _sqlCmd = new SqlCommand(_sql, _sqlConn))
                    {
                        _sqlCmd.CommandType = CommandType.Text;
                        _sqlCmd.Parameters.AddRange(dbParam);
                        _sqlConn.Open();
                        using (SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_sqlCmd))
                        {
                            _sqlAdapter.Fill(myDataSet);
                        }
                        _sqlConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                blnResult = false;
            }
            return blnResult;
        }


        public bool QueryCommand(
            String ConnetionString,
            String SqlQuery,
            out DataSet myDataSet,
            out string strError)
        {
            bool blnResult = true;
            myDataSet = new DataSet();
            strError = "";
            try
            {
                using (SqlConnection _sqlConn = new SqlConnection(ConnetionString))
                {
                    string _sql = SqlQuery;
                    using (SqlCommand _sqlCmd = new SqlCommand(_sql, _sqlConn))
                    {
                        _sqlCmd.CommandType = CommandType.Text;
                        _sqlConn.Open();
                        using (SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_sqlCmd))
                        {
                            _sqlAdapter.Fill(myDataSet);
                        }
                        _sqlConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                blnResult = false;
            }
            return blnResult;
        }

        public bool QueryCommand(
            String ConnetionString,
            String SqlQuery,
            out string strError)
        {
            bool blnResult = true;
            strError = "";
            try
            {
                using (SqlConnection _sqlConn = new SqlConnection(ConnetionString))
                {
                    string _sql = SqlQuery;
                    using (SqlCommand _sqlCmd = new SqlCommand(_sql, _sqlConn))
                    {
                        _sqlCmd.CommandType = CommandType.Text;
                        _sqlConn.Open();
                        _sqlCmd.ExecuteNonQuery();
                        _sqlConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                blnResult = false;
            }
            return blnResult;
        }

        #region DisposeObject
        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                //free any other management object here.
            }
            disposed = true;
        }
        #endregion
    }
}