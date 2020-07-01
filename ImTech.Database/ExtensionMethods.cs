using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.DataBase
{
    public static class ExtensionMethods
    {
        public static DataTable ToDataTable(this SqlDataReader dataReader)
        {
            DataTable dataTable = new DataTable();

            if (dataReader.HasRows)
            {
                dataTable = dataReader.GetSchemaTable();

                while (dataReader.Read())
                {
                    DataRow dataRow = dataTable.NewRow();

                    foreach (DataRow schemaRow in dataTable.Rows)
                    {
                        var columnName = Convert.ToString(schemaRow["ColumnName"]);
                        dataRow[columnName] = dataReader[columnName];
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }
    }
}
