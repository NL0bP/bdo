using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using NLog;

namespace BDCommon.Database
{
    public class MysqlDB
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        static MysqlConfig Auth;

        private static object _lock = new object();
        public static bool Init(MysqlConfig auth)
        {
            bool AuthResult = false;

            using (MySqlConnection mysqlCon = new MySqlConnection(auth.ToString()))
            {
                try
                {
                    mysqlCon.Open();
                    mysqlCon.Close();
                    AuthResult = true;
                }
                catch
                {
                    mysqlCon.Close();
                    AuthResult = false;
                }
            }
            if (AuthResult)
            {
                Auth = auth;
                return true;
            }
            return false;
        }


        public static bool Query(MysqlQuery query)
        {
            lock (_lock)
            {
                MySqlConnection Connection = GetConnection(query);
                bool Result = false;
                try
                {
                    Connection.Open();
                    MySqlCommand Command = query.GetMysqlCommand(Connection);
                    int t = Command.ExecuteNonQuery();
                    Result = !(t == -1 || t == 0);
                }
                catch (MySqlException ex)
                {
                    Log.WarnException("MySql", ex);
                    Result = false;
                }
                finally
                {
                    Connection.Close();
                }
                return Result;
            }
        }


        public static object Result(MysqlQuery query)
        {
            lock (_lock)
            {
                MySqlConnection Connection = GetConnection(query);
                object Result = null;
                try
                {
                    Connection.Open();
                    MySqlCommand Command = query.GetMysqlCommand(Connection);
                    Result = Command.ExecuteScalar();
                }
                catch (MySqlException ex)
                {
                    Log.WarnException("MySql", ex);
                    Result = null;
                }
                finally
                {
                    Connection.Close();
                }
                return Result;
            }
        }

        public static List<Dictionary<string, object>> SelectAll(MysqlQuery query)
        {
            var allValues = new List<Dictionary<string, object>>();
            lock (_lock)
            {
                MySqlConnection Connection = GetConnection(query);
                try
                {
                    Connection.Open();
                    MySqlCommand command = query.GetMysqlCommand(Connection);
                    MySqlDataReader dataReader = command.ExecuteReader();
                    DataTable schemaTable = null;
                    while (dataReader.Read())
                        allValues.Add(GetValues(dataReader, schemaTable));
                    dataReader.Close();
                }
                catch (MySqlException ex)
                {
                    Log.WarnException("MySql", ex);
                }
                finally
                {
                    Connection.Close();
                }
            }

            return allValues;
        }
        private static Dictionary<string, object> GetValues(MySqlDataReader dataReader, DataTable schemaTable = null)
        {
            var values = new Dictionary<string, object>();
            if (schemaTable == null)
                schemaTable = dataReader.GetSchemaTable();
            for (int i = 0; i < schemaTable.Rows.Count; i++)
            {
                DataRow row = schemaTable.Rows[i];
                if ((Type)row[(int)SchemaTableRows.DataType] == typeof(Boolean))
                    values.Add((string)row[(int)SchemaTableRows.ColumnName], dataReader.GetByte(i));
                else
                    values.Add((string)row[(int)SchemaTableRows.ColumnName],
                               (bool)row[(int)SchemaTableRows.AllowDbNull] && dataReader.IsDBNull(i) ? null : Convert.ChangeType(dataReader.GetValue(i), (Type)row[(int)SchemaTableRows.DataType]));
            }
            return values;
        }

        private static MySqlConnection GetConnection(MysqlQuery query)
        {
            MySqlConnection connection = new MySqlConnection(Auth.ToString());

            return connection;
        }

        private enum SchemaTableRows
        {
            ColumnName = 0,
            ColumnOrdinal = 1,
            ColumnSize = 2,
            NumericPrecision = 3,
            NumericScale = 4,
            IsUnique = 5,
            IsKey = 6,
            BaseCatalogName = 7,
            BaseColumnName = 8,
            BaseSchemaName = 9,
            BaseTableName = 10,
            DataType = 11,
            AllowDbNull = 12,
            ProviderType = 13,
            IsAliased = 14,
            IsExpression = 15,
            IsIdentity = 16,
            IsAutoIncrement = 17,
            IsRowVersion = 18,
            IsHidden = 19,
            IsLong = 20,
            IsReadOnly = 21,
        }
    }

    public class MysqlConfig
    {
        string Host, Database, User, Password;

        public MysqlConfig(string Host, string Database, string User, string Password)
        {
            this.Host = Host;
            this.Database = Database;
            this.User = User;
            this.Password = Password;
        }

        public override string ToString()
        {
            return "Data Source=" + Host + ";Database=" + Database + ";User ID=" + User + ";Password=" + Password + ";Max Pool Size=1000;Connection Lifetime=5";
        }
    }

    public class MysqlQuery
    {
        Dictionary<string, object> DbValues;
        Dictionary<string, MySqlDbType> DbValuesType;

        string SqlQuery;

        public MysqlQuery(string Sql)
        {
            DbValues = new Dictionary<string, object>();
            DbValuesType = new Dictionary<string, MySqlDbType>();
            SqlQuery = Sql;
        }

        public void AddValue(string Key, object Value, MySqlDbType Type)
        {
            DbValues.Add(Key, Value);
            DbValuesType.Add(Key, Type);
        }

        public MySqlCommand GetMysqlCommand(MySqlConnection Connection)
        {
            MySqlCommand Command = new MySqlCommand(SqlQuery, Connection);
            foreach (KeyValuePair<string, object> vl in DbValues)
            {
                Command.Parameters.Add("@" + vl.Key, DbValuesType[vl.Key]).Value = vl.Value;
            }
            return Command;
        }
    }
}
