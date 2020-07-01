using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.DataServices;
using ImTech.DataBase;
using ImTech.Model;
using System.Data;

namespace ImTech.DataServices
{
    public class LoginService
    {
        IDataBaseService dataBaseService;
        public LoginService(IDataBaseService databaseService)
        {
            dataBaseService = databaseService;
        }

        #region Public Methods
        public LoginModel ValidateUserLogin(LoginModel logOnModel)
        {
            if (logOnModel == null)
            {
                return new LoginModel()
                {
                    Success = false,
                    ErrorMessage = "Invalid User Login"
                };
            }
            List<Parameter> userParams = new List<Parameter>();

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);

            userParams.Add(new Parameter("@EmailId", logOnModel.Email));
            userParams.Add(new Parameter("@Password", logOnModel.Password));
            userParams.Add(new Parameter("@HashPassword", logOnModel.HashPassword));
            userParams.Add(p_IsError);
            userParams.Add(p_ErrorMsg);

            var dataSet = dataBaseService.GetDataSet(StoredProcedures.ValidateUserLogin, DBCommandType.Procedure, userParams.ToArray());
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                logOnModel = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapLogOnModel(r)).FirstOrDefault();
                logOnModel.Success = true;
            }
            else
            {
                logOnModel.Success = false;
                logOnModel.ErrorMessage = "Invalid User Login";
            }
            return logOnModel;
        }
        #endregion

        #region Private Method
        private LoginModel MapLogOnModel(DataRow row)
        {
            return new LoginModel()
            {
                Id = Convert.ToInt32(row["UserID"]),
                Email = row["EmailID"] != DBNull.Value ? Convert.ToString(row["EmailID"]) : string.Empty,
                Password = row["Password"] != DBNull.Value ? Convert.ToString(row["Password"]) : string.Empty,
                HashPassword = row["HashPassword"] != DBNull.Value ? Convert.ToString(row["HashPassword"]) : string.Empty,
                RoleId = row["RoleId"] != DBNull.Value ? Convert.ToInt16(row["RoleId"]) : (Int16)0
            };
        }
        #endregion

    }
}
