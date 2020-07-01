using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace ImTech.DataBase
{
    public class DataBaseService : IDataBaseService
    {
        SqlConnection connection;
        SqlCommand command;
        SqlTransaction transaction;

        public DataBaseService()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionString);
        }

        private void BindCommand(string scriptOrProcedure, DBCommandType commandType = DBCommandType.Procedure)
        {
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = scriptOrProcedure;
            command.CommandType = commandType == DBCommandType.Procedure
                                        ? CommandType.StoredProcedure
                                        : CommandType.Text;
        }

        private void OpenConnection()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }
        private void CloseConnection()
        {
            if (connection != null)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();

                }
            }
        }

        private void BindParameters(params Parameter[] parameters)
        {
            for (int i = 0; i < parameters.Count(); i++)
            {
                if (parameters[i].Direction != 0)
                {
                    command.Parameters.Add(new SqlParameter(parameters[i].Name, parameters[i].Value));
                    command.Parameters[i].Direction = parameters[i].Direction;
                }
                else
                {
                    command.Parameters.Add(new SqlParameter(parameters[i].Name, parameters[i].Value));
                }
                if (parameters[i].Size != 0)
                {
                    command.Parameters[i].DbType = parameters[i].Type;
                    command.Parameters[i].Size = parameters[i].Size;
                }

            }
        }

        private SqlTransaction BeginTransaction()
        {
            if (transaction == null)
            {
                OpenConnection();
                transaction = connection.BeginTransaction();
            }

            return transaction;
        }

        private void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
                CloseConnection();
            }

        }

        private void RollBackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                CloseConnection();
            }
        }
        public int ExecuteNonQueryWithTransaction(string scriptOrProcedure, DBCommandType commandType = DBCommandType.Procedure, params Parameter[] parameters)
        {
            try
            {
                BindCommand(scriptOrProcedure, commandType);

                BindParameters(parameters);

                OpenConnection();

                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int ExecuteNonQuery(string scriptOrProcedure, DBCommandType commandType = DBCommandType.Procedure, params Parameter[] parameters)
        {
            try
            {
                BindCommand(scriptOrProcedure, commandType);

                BindParameters(parameters);

                OpenConnection();

                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        public object ExecuteScalar(string scriptOrProcedure, DBCommandType commandType = DBCommandType.Procedure, params Parameter[] parameters)
        {
            try
            {
                BindCommand(scriptOrProcedure, commandType);

                BindParameters(parameters);

                OpenConnection();

                return command.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataSet GetDataSet(string scriptOrProcedure, DBCommandType commandType = DBCommandType.Procedure, params Parameter[] parameters)
        {
            try
            {
                DataSet dataSet = new DataSet();

                BindCommand(scriptOrProcedure, commandType);

                BindParameters(parameters);

                var dataAdapter = new SqlDataAdapter(command);

                dataAdapter.Fill(dataSet);

                return dataSet;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable GetDataTable(string scriptOrProcedure, DBCommandType commandType = DBCommandType.Procedure, params Parameter[] parameters)
        {
            try
            {
                DataTable dataTable = new DataTable();

                BindCommand(scriptOrProcedure, commandType);

                BindParameters(parameters);

                var dataAdapter = new SqlDataAdapter(command);

                dataAdapter.Fill(dataTable);

                return dataTable;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable ExecuteReader(string scriptOrProcedure, DBCommandType commandType = DBCommandType.Procedure, params Parameter[] parameters)
        {
            try
            {
                return GetDataTable(scriptOrProcedure, commandType, parameters);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        public void Dispose()
        {
            CloseConnection();
            connection.Dispose();
            command.Dispose();
        }
    }
}
