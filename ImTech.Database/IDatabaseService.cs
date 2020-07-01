using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace ImTech.DataBase
{
    public interface IDataBaseService : IDisposable
    {
        //IDbTransaction Transaction
        //{
        //    get;
        //}
        int ExecuteNonQuery(string scriptOrProcedure,
                            DBCommandType commandType = DBCommandType.Procedure,
                            params Parameter[] parameters);

        object ExecuteScalar(string scriptOrProcedure,
                            DBCommandType commandType = DBCommandType.Procedure,
                            params Parameter[] parameters);

        DataSet GetDataSet(string scriptOrProcedure,
                            DBCommandType commandType = DBCommandType.Procedure,
                            params Parameter[] parameters);

        DataTable GetDataTable(string scriptOrProcedure,
                            DBCommandType commandType = DBCommandType.Procedure,
                            params Parameter[] parameters);

        DataTable ExecuteReader(string scriptOrProcedure,
                            DBCommandType commandType,
                            params Parameter[] parameters);


    }
}
