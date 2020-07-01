using ImTech.DataBase;
using ImTech.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.DataServices.Doctor
{
    public class DegreeService
    {
        IDataBaseService dataBaseService;
        public DegreeService(IDataBaseService databaseService)
        {
            dataBaseService = databaseService;
        }

        public IEnumerable<DegreeModel> GetDoctorDegrees(int doctorId, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@DoctorID", doctorId));
            param.Add(new Parameter("@TenantID", tenantId));
            var p_IsError = new Parameter("@IsError", ParameterDirection.Output);
            var p_ErrorMsg = new Parameter("@ErrorMsg", ParameterDirection.Output);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDoctorDegrees, DBCommandType.Procedure, param.ToArray());

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var degrees = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapDoctorDegree(r));
                return degrees;
            }
            return null;
        }

        DegreeModel MapDoctorDegree(DataRow row)
        {
            return new DegreeModel
            {
                Id = Convert.ToInt32(row["DegreeID"]),
                DegreeName = Convert.ToString(row["DegreeName"]),
                TenantID = Convert.ToInt32(row["TenantID"])
            };
        }
    }
}
