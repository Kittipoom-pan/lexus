using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;


namespace AppLibrary.Database
{
    public class DBAccess : IDisposable
    {
        private IDbConnection connection;
        private string connectionString;
        private int commandTimeout;
        private static List<IDbDataParameter> paramterList;
        private static ConnectionType type;
        private int bulkCopyBatchSize;
        private int bulkCopyTimeout;
        private List<BulkCopyColumnMap> bulkCopyColumnMapList;

        public enum ConnectionType
        {
            SQLDB, OLEDB, Access, AS400, DB2
        }
        public class BulkCopyColumnMap
        {
            public int SourceColumnIndex { get; set; }
            public int DestinationColumnIndex { get; set; }
        }
        public IDbConnection Connection
        {
            set { connection = value; }
            get { return connection; }
        }
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }
        public int CommandTimeout
        {
            set { commandTimeout = value; }
            get { return commandTimeout; }
        }
        public List<IDbDataParameter> ParamterList
        {
            set { paramterList = value; }
            get { return paramterList; }
        }
        public ConnectionType Type
        {
            get { return type; }
            set { type = value; }
        }
        public int BulkCopyBatchSize
        {
            set { bulkCopyBatchSize = value; }
            get { return bulkCopyBatchSize; }
        }
        public int BulkCopyTimeout
        {
            set { bulkCopyTimeout = value; }
            get { return bulkCopyTimeout; }
        }
        public List<BulkCopyColumnMap> BulkCopyColumnMapList
        {
            get { return bulkCopyColumnMapList; }
            set { bulkCopyColumnMapList = value; }
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Dispose();
        }
        ~DBAccess()
        {
            Dispose();
        }
        #endregion
        public DBAccess()
        {
            try
            {
                ResetObject();
                switch (type)
                {
                    case ConnectionType.SQLDB:
                        connection = new SqlConnection(connectionString);
                        break;
                    default:
                        connection = new OleDbConnection(connectionString);
                        break;
                }
                OpenConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DBAccess(string connectionString, ConnectionType connectionType)
        {
            try
            {
                ResetObject();
                type = connectionType;
                switch (type)
                {
                    case ConnectionType.SQLDB:
                        connection = new SqlConnection(connectionString);
                        break;
                    default:
                        connection = new OleDbConnection(connectionString);
                        break;
                }
                OpenConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void OpenConnection()
        {
            try
            {
                if (connection != null && connection.State != ConnectionState.Open)
                    connection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CloseConnection()
        {
            try
            {
                if (connection != null && connection.State != ConnectionState.Open)
                    connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            try
            {
                CloseConnection();
                ResetObject();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ResetObject()
        {
            try
            {
                connection = null;
                connectionString = "";
                commandTimeout = 30;
                paramterList = new List<IDbDataParameter>();
                type = ConnectionType.SQLDB;
                bulkCopyBatchSize = 0;
                bulkCopyTimeout = 30;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetDataTableFromCommandText(string commandText)
        {
            DataTable dtRet = new DataTable();
            try
            {
                OpenConnection();
                if (type == ConnectionType.SQLDB)
                {
                    SqlCommand cm = new SqlCommand();
                    cm.CommandText = commandText;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.Text;
                    cm.Connection = (SqlConnection)connection;

                    if (paramterList != null && paramterList.Count > 0)
                    {
                        foreach (SqlParameter prm in paramterList)
                        {
                            cm.Parameters.Add(prm);
                        }
                    }

                    SqlDataAdapter adp = new SqlDataAdapter(cm);
                    adp.Fill(dtRet);
                    adp = null;
                }
                else
                {
                    OleDbCommand cm = new OleDbCommand();
                    cm.CommandText = commandText;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.Text;
                    cm.Connection = (OleDbConnection)connection;

                    if (paramterList != null && paramterList.Count > 0)
                    {
                        foreach (OleDbParameter prm in paramterList)
                        {
                            cm.Parameters.Add(prm);
                        }
                    }

                    OleDbDataAdapter adp = new OleDbDataAdapter(cm);
                    adp.Fill(dtRet);
                    adp = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return dtRet;
        }
        public DataTable GetDataTableFromStoreProcedure(string procedureName)
        {
            DataTable dtRet = new DataTable();
            try
            {
                OpenConnection();
                if (type == ConnectionType.SQLDB)
                {
                    SqlCommand cm = new SqlCommand();
                    cm.CommandText = procedureName;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.StoredProcedure;
                    if (paramterList != null)
                        foreach (SqlParameter prm in paramterList)
                            cm.Parameters.Add(prm);
                    cm.Connection = (SqlConnection)connection;

                    SqlDataAdapter adp = new SqlDataAdapter(cm);
                    adp.Fill(dtRet);
                    adp = null;
                }
                else
                {
                    OleDbCommand cm = new OleDbCommand();
                    cm.CommandText = procedureName;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.StoredProcedure;
                    if (paramterList != null)
                        foreach (OleDbParameter prm in paramterList)
                            cm.Parameters.Add(prm);
                    cm.Connection = (OleDbConnection)connection;

                    OleDbDataAdapter adp = new OleDbDataAdapter(cm);
                    adp.Fill(dtRet);
                    adp = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return dtRet;
        }
        public int ExecuteNonQueryFromCommandText(string commandText)
        {
            int status = -1;
            try
            {
                OpenConnection();
                if (type == ConnectionType.SQLDB)
                {
                    SqlTransaction transaction = ((SqlConnection)connection).BeginTransaction();
                    try
                    {
                        SqlCommand cm = new SqlCommand();
                        cm.CommandText = commandText;
                        cm.CommandTimeout = commandTimeout;
                        cm.CommandType = CommandType.Text;
                        cm.Connection = (SqlConnection)connection;
                        cm.Transaction = transaction;

                        if (paramterList != null && paramterList.Count > 0)
                        {
                            foreach (SqlParameter prm in paramterList)
                            {
                                cm.Parameters.Add(prm);
                            }
                        }

                        status = cm.ExecuteNonQuery();
                       
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        transaction = null;
                    }
                }
                else
                {
                    OleDbTransaction transaction = ((OleDbConnection)connection).BeginTransaction();
                    try
                    {
                        OleDbCommand cm = new OleDbCommand();
                        cm.CommandText = commandText;
                        cm.CommandTimeout = commandTimeout;
                        cm.CommandType = CommandType.Text;
                        cm.Connection = (OleDbConnection)connection;
                        cm.Transaction = transaction;

                        if (paramterList != null && paramterList.Count > 0)
                        {
                            foreach (OleDbParameter prm in paramterList)
                            {
                                cm.Parameters.Add(prm);
                            }
                        }

                        status = cm.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        transaction = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return status;
        }
        public int ExecuteNonQueryFromMultipleCommandText(List<string> commandTexts)
        {
            int status = -1;
            try
            {
                OpenConnection();
                if (type == ConnectionType.SQLDB)
                {
                    SqlTransaction transaction = ((SqlConnection)connection).BeginTransaction();
                    try
                    {
                        foreach (string cmd in commandTexts)
                        {
                            SqlCommand cm = new SqlCommand();
                            cm.CommandText = cmd;
                            cm.CommandTimeout = commandTimeout;
                            cm.CommandType = CommandType.Text;
                            cm.Connection = (SqlConnection)connection;
                            cm.Transaction = transaction;

                            if (paramterList != null && paramterList.Count > 0)
                            {
                                foreach (SqlParameter prm in paramterList)
                                {
                                    cm.Parameters.Add(prm);
                                }
                            }

                            status = cm.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        transaction = null;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return status;
        }
        public int ExecuteNonQueryFromMultipleCommandText(List<CommandTextWithParamModel> commands)
        {
            int status = -1;
            try
            {
                OpenConnection();
                if (type == ConnectionType.SQLDB)
                {
                    SqlTransaction transaction = ((SqlConnection)connection).BeginTransaction();
                    try
                    {
                        foreach (CommandTextWithParamModel cmd in commands)
                        {
                            SqlCommand cm = new SqlCommand();
                            cm.CommandText = cmd.CommandText;
                            cm.CommandTimeout = commandTimeout;
                            cm.CommandType = CommandType.Text;
                            cm.Connection = (SqlConnection)connection;
                            cm.Transaction = transaction;

                            if (cmd.Parameters != null && cmd.Parameters.Count > 0)
                            {
                                foreach (SqlParameter prm in cmd.Parameters)
                                {
                                    cm.Parameters.Add(prm);
                                }
                            }

                            status = cm.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        transaction = null;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return status;
        }
        public int ExecuteNonQueryFromCommandText(List<string> commandTextList)
        {
            int status = -1;
            try
            {
                bool _status = true;
                OpenConnection();
                if (type == ConnectionType.SQLDB)
                {
                    SqlTransaction transaction = ((SqlConnection)connection).BeginTransaction();
                    try
                    {
                        SqlCommand cm = new SqlCommand();
                        cm.CommandTimeout = commandTimeout;
                        cm.CommandType = CommandType.Text;
                        cm.Connection = (SqlConnection)connection;
                        cm.Transaction = transaction;
                        if (commandTextList != null)
                            foreach (string commandText in commandTextList)
                            {
                                cm.CommandText = commandText;
                                _status &= (cm.ExecuteNonQuery() > -1);
                            }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        transaction = null;
                    }
                }
                else
                {
                    OleDbTransaction transaction = ((OleDbConnection)connection).BeginTransaction();
                    try
                    {
                        OleDbCommand cm = new OleDbCommand();
                        cm.CommandTimeout = commandTimeout;
                        cm.CommandType = CommandType.Text;
                        cm.Connection = (OleDbConnection)connection;
                        cm.Transaction = transaction;
                        if (commandTextList != null)
                            foreach (string commandText in commandTextList)
                            {
                                cm.CommandText = commandText;
                                _status &= (cm.ExecuteNonQuery() > -1);
                            }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        transaction = null;
                    }
                }
                if (_status)
                    status = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return status;
        }
        public int ExecuteNonQueryFromStoreProcedure(string procedureName)
        {
            int status = -1;
            try
            {
                OpenConnection();
                if (type == ConnectionType.SQLDB)
                {
                    SqlTransaction transaction = ((SqlConnection)connection).BeginTransaction();
                    try
                    {
                        SqlCommand cm = new SqlCommand();
                        cm.CommandText = procedureName;
                        cm.CommandTimeout = commandTimeout;
                        cm.CommandType = CommandType.StoredProcedure;
                        if (paramterList != null)
                            foreach (SqlParameter prm in paramterList)
                                cm.Parameters.Add(prm);
                        cm.Connection = (SqlConnection)connection;
                        cm.Transaction = transaction;
                        status = cm.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        transaction = null;
                    }
                }
                else
                {
                    OleDbTransaction transaction = ((OleDbConnection)connection).BeginTransaction();
                    try
                    {
                        OleDbCommand cm = new OleDbCommand();
                        cm.CommandText = procedureName;
                        cm.CommandTimeout = commandTimeout;
                        cm.CommandType = CommandType.StoredProcedure;
                        if (paramterList != null)
                            foreach (OleDbParameter prm in paramterList)
                                cm.Parameters.Add(prm);
                        cm.Connection = (OleDbConnection)connection;
                        cm.Transaction = transaction;
                        status = cm.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        transaction = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return status;
        }
        public T ExecuteNonQueryFromStoreProcedure<T>(string procedureName, string paramOutputName, object dbType, int paramOutputSize)
        {
            T obj = default(T);
            try
            {
                if (paramterList == null)
                    paramterList = new List<IDbDataParameter>();

                paramterList.Add(CreateOutputParameter(paramOutputName, dbType, paramOutputSize));
                ExecuteNonQueryFromStoreProcedure(procedureName);
                obj = (T)paramterList[paramterList.Count - 1].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
        public IDataReader ExecuteReaderFromCommandText(string commandText)
        {
            IDataReader dataReader = null;
            try
            {
                OpenConnection();
                if (type == ConnectionType.SQLDB)
                {
                    SqlCommand cm = new SqlCommand();
                    cm.CommandText = commandText;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.Text;
                    cm.Connection = (SqlConnection)connection;

                    if (paramterList != null && paramterList.Count > 0)
                    {
                        foreach (SqlParameter prm in paramterList)
                        {
                            cm.Parameters.Add(prm);
                        }
                    }

                    dataReader = cm.ExecuteReader();
                }
                else
                {
                    OleDbCommand cm = new OleDbCommand();
                    cm.CommandText = commandText;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.Text;
                    cm.Connection = (OleDbConnection)connection;

                    if (paramterList != null && paramterList.Count > 0)
                    {
                        foreach (OleDbParameter prm in paramterList)
                        {
                            cm.Parameters.Add(prm);
                        }
                    }

                    dataReader = cm.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataReader;
        }
        public IDataReader ExecuteReaderFromStoreProcedure(string procedureName)
        {
            IDataReader dataReader = null;
            try
            {
                OpenConnection();
                if (type == ConnectionType.SQLDB)
                {
                    SqlCommand cm = new SqlCommand();
                    cm.CommandText = procedureName;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.StoredProcedure;
                    if (paramterList != null)
                        foreach (SqlParameter prm in paramterList)
                            cm.Parameters.Add(prm);
                    cm.Connection = (SqlConnection)connection;
                    dataReader = cm.ExecuteReader();
                }
                else
                {
                    OleDbCommand cm = new OleDbCommand();
                    cm.CommandText = procedureName;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.StoredProcedure;
                    if (paramterList != null)
                        foreach (OleDbParameter prm in paramterList)
                            cm.Parameters.Add(prm);
                    cm.Connection = (OleDbConnection)connection;
                    dataReader = cm.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataReader;
        }
        public T ExecuteScalarFromCommandText<T>(string commandText)
        {
            T obj = default(T);
            try
            {
                OpenConnection();
                if (type == ConnectionType.SQLDB)
                {
                    SqlCommand cm = new SqlCommand();
                    cm.CommandText = commandText;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.Text;
                    cm.Connection = (SqlConnection)connection;

                    if (paramterList != null && paramterList.Count > 0)
                    {
                        foreach (SqlParameter prm in paramterList)
                        {
                            cm.Parameters.Add(prm);
                        }
                    }

                    obj = (T)cm.ExecuteScalar();
                }
                else
                {
                    OleDbCommand cm = new OleDbCommand();
                    cm.CommandText = commandText;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.Text;
                    cm.Connection = (OleDbConnection)connection;

                    if (paramterList != null && paramterList.Count > 0)
                    {
                        foreach (OleDbParameter prm in paramterList)
                        {
                            cm.Parameters.Add(prm);
                        }
                    }

                    obj = (T)cm.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return obj;
        }
        public T ExecuteScalarFromStoreProcedure<T>(string procedureName)
        {
            T obj = default(T);
            try
            {
                OpenConnection();
                if (type == ConnectionType.SQLDB)
                {
                    SqlCommand cm = new SqlCommand();
                    cm.CommandText = procedureName;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.StoredProcedure;
                    if (paramterList != null)
                        foreach (SqlParameter prm in paramterList)
                            cm.Parameters.Add(prm);
                    cm.Connection = (SqlConnection)connection;
                    obj = (T)cm.ExecuteScalar();
                }
                else
                {
                    OleDbCommand cm = new OleDbCommand();
                    cm.CommandText = procedureName;
                    cm.CommandTimeout = commandTimeout;
                    cm.CommandType = CommandType.StoredProcedure;
                    if (paramterList != null)
                        foreach (OleDbParameter prm in paramterList)
                            cm.Parameters.Add(prm);
                    cm.Connection = (OleDbConnection)connection;
                    obj = (T)cm.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return obj;
        }
        public void BulkCopy(string destinationTableName, object objectData)
        {
            try
            {
                if (type != ConnectionType.SQLDB)
                    throw new Exception("BulkCopy can use from SQL provider only!");

                if (!(objectData is DataTable) && !(objectData is IDataReader) && !(objectData is DataRow[]))
                    throw new Exception("Out of Object Data provider!");

                SqlBulkCopy copy = new SqlBulkCopy((SqlConnection)connection);
                copy.BatchSize = bulkCopyBatchSize;
                copy.BulkCopyTimeout = bulkCopyTimeout;
                copy.DestinationTableName = destinationTableName;
                if (bulkCopyColumnMapList != null)
                    foreach (BulkCopyColumnMap map in bulkCopyColumnMapList)
                        copy.ColumnMappings.Add(map.SourceColumnIndex, map.DestinationColumnIndex);

                if (objectData is DataTable)
                    copy.WriteToServer((DataTable)objectData);
                if (objectData is IDataReader)
                    copy.WriteToServer((IDataReader)objectData);
                if (objectData is DataRow[])
                    copy.WriteToServer((DataRow[])objectData);

                copy.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
        public static IDbDataParameter CreateInputParameter(string parameterName, object parameterValue)
        {
            IDbDataParameter param = null;
            try
            {
                switch (type)
                {
                    case ConnectionType.SQLDB:
                        param = new SqlParameter(parameterName, parameterValue);
                        break;
                    default:
                        param = new OleDbParameter(parameterName, parameterValue);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return param;
        }
        public static IDbDataParameter CreateOutputParameter(string parameterName, object dbType, int paramOutputSize)
        {
            IDbDataParameter param = null;
            try
            {
                switch (type)
                {
                    case ConnectionType.SQLDB:
                        param = new SqlParameter(parameterName, (SqlDbType)dbType, paramOutputSize);
                        param.Direction = ParameterDirection.Output;
                        break;
                    default:
                        param = new OleDbParameter(parameterName, (OleDbType)dbType, paramOutputSize);
                        param.Direction = ParameterDirection.Output;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return param;
        }
        public static T GetParameterValue<T>(string parameterName)
        {
            T obj = default(T);
            try
            {
                obj = (T)paramterList.Find(f => f.ParameterName == parameterName).Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
    }
}