using ImTech.DataBase;
using ImTech.DataServices;
using ImTech.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.DataServices
{
    public class UserService
    {
        IDataBaseService dataBaseService;
        public UserService(IDataBaseService databaseService)
        {
            dataBaseService = databaseService;
        }

        #region private Method
        IEnumerable<UserModel> Map(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows.Cast<DataRow>())
            {
                yield return Map(row);
            }
        }
        UserModel Map(DataRow row)
        {
            return new UserModel
            {
                Id = Convert.ToInt32(row["UserId"]),
                Email = Convert.ToString(row["EmailID"]),
                Password = Convert.ToString(row["Password"]),

                //from base model
                TenantID = Convert.ToInt32(row["TenantID"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                CreatedOn = new InternalDateTime(Convert.ToInt64(row["CreatedOn"])),
                CreatedByEntity = Convert.ToInt32(row["CreatedByEntity"]),
                ModifiedBy = Convert.ToInt32(row["ModifiedBy"]),
                ModifiedOn = new InternalDateTime(Convert.ToInt64(row["ModifiedOn"])),
                ModifiedByEntity = Convert.ToInt32(row["ModifiedByEntity"]),
            };
        }

        #endregion

        #region public methods
        public UserModel GetUser(int id, int tenantId)
        {
            UserModel userModel = new UserModel();
            List<Parameter> userParams = new List<Parameter>();

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);

            userParams.Add(new Parameter("@UserID", id));
            userParams.Add(new Parameter("@TenantID", tenantId));
            userParams.Add(p_IsError);
            userParams.Add(p_ErrorMsg);

            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetUserOrUserList, DBCommandType.Procedure, userParams.ToArray());
            if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
            {
                userModel = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapUser(r)).FirstOrDefault();
                userModel.Success = true;
            }
            else
            {
                userModel.Success = false;
                userModel.ErrorMessage = "No able to retreive user";
            }
            return userModel;
        }

        public UserModel GetUserByPhoneNo(string phoneNo, int tenantId)
        {
            UserModel userModel = new UserModel();
            List<Parameter> userParams = new List<Parameter>();

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);

            userParams.Add(new Parameter("@PhoneNo", phoneNo));
            userParams.Add(new Parameter("@TenantID", tenantId));
            userParams.Add(p_IsError);
            userParams.Add(p_ErrorMsg);

            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetUsersByPhoneNo, DBCommandType.Procedure, userParams.ToArray());
            if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
            {
                userModel = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapUser(r)).FirstOrDefault();
                userModel.Success = true;
            }
            else
            {
                userModel.Success = false;
                userModel.ErrorMessage = "No able to retreive user";
            }
            return userModel;
        }

        public UserModel RegisterUser(UserModel userModel)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@EmailID", userModel.Email));
            param.Add(new Parameter("@Password", userModel.Password));
            param.Add(new Parameter("@HashPassword", userModel.HashPassword));
            param.Add(new Parameter("@PhoneNumber", userModel.PhoneNumber));
            param.Add(new Parameter("@TenantID", userModel.TenantID));
            param.Add(new Parameter("@CreatedByEntity", userModel.CreatedByEntity));
            param.Add(new Parameter("@FirstName", userModel.FirstName));
            param.Add(new Parameter("@LastName", userModel.LastName));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.RegisterUser, DBCommandType.Procedure, param.ToArray());
            int Id;
            if (int.TryParse(r.ToString(), out Id))
            {
                userModel.Success = true;
                userModel.Id = Id;
            }
            else
            {
                userModel.Success = false;
                userModel.ErrorMessage = "Error in insert or update user";
            }
            return userModel;
        }

        public UserLogOnModel ValidateUserLogOn(UserLogOnModel logOnModel)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Email", logOnModel.Email));
            param.Add(new Parameter("@Password", logOnModel.Password));
            param.Add(new Parameter("@TenantId", logOnModel.TenantID));
            param.Add(new Parameter("@CreatedByEntity", logOnModel.CreatedByEntity));

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.ValidateLoginDetail, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt16(r);
            if (Id > 0)
            {
                logOnModel.Success = true;
                logOnModel.Id = Id;
            }
            else if (Id == -3)
            {
                logOnModel.Success = false;
                logOnModel.ErrorMessage = "Phone Number not verified login failed";
            }
            else if (Id == -2)
            {
                logOnModel.Success = false;
                logOnModel.ErrorMessage = "Invalid login.";
            }
            else if (Id == -4)
            {
                logOnModel.Success = false;
                logOnModel.ErrorMessage = "Invalid Email ID or User is not registered with this password.";
            }
            return logOnModel;
        }

        public bool UpdateDeviceId(UserLogOnModel userModel)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@UserId", userModel.Id));
            param.Add(new Parameter("@DeviceId", userModel.DeviceId));
            param.Add(new Parameter("@TenantId", userModel.TenantID));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteNonQuery(StoredProcedures.UpdateUserDeviceId, DBCommandType.Procedure, param.ToArray());
            if (r > 0)
            {
                return true;
            }
            return false;
        }

        private UserModel MapUser(DataRow row)
        {
            return new UserModel()
            {
                Id = Convert.ToInt32(row["UserID"]),
                FirstName = row["FirstName"] != DBNull.Value ? Convert.ToString(row["FirstName"]) : string.Empty,
                LastName = row["LastName"] != DBNull.Value ? Convert.ToString(row["LastName"]) : string.Empty,
                Email = row["EmailID"] != DBNull.Value ? Convert.ToString(row["EmailID"]) : string.Empty,
                IsPhoneNoVerified = row["IsPhoneNumberVerified"] != DBNull.Value ? Convert.ToBoolean(row["IsPhoneNumberVerified"]) : false,
                IsDeleted = row["IsDeleted"] != DBNull.Value ? (Convert.ToInt32(row["IsDeleted"]) == 1 ? true : false) : false,
                CreatedBy = row["CreatedBy"] != DBNull.Value ? Convert.ToInt32(row["CreatedBy"]) : 0,
                CreatedByEntity = row["CreatedByEntity"] != DBNull.Value ? Convert.ToInt32(row["CreatedByEntity"]) : 0,
                //model.CreatedOn = row["CreatedOn"]!=null ?  Convert.TOBrow["CreatedOn"];
                PhoneNumber = row["PhoneNumber"] != DBNull.Value ? Convert.ToString(row["PhoneNumber"]) : string.Empty,
                TenantID = row["TenantID"] != DBNull.Value ? Convert.ToInt32(row["TenantID"]) : -1,
                ModifiedBy = row["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(row["ModifiedBy"]) : 0,
                ModifiedByEntity = row["ModifiedByEntity"] != DBNull.Value ? Convert.ToInt32(row["ModifiedByEntity"]) : 0,
                DeviceId=Convert.ToString(row["DeviceId"])
                
            };
        }

        public IEnumerable<UserModel> GetUserList()
        {
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetUserList, DBCommandType.Procedure);
            return Map(dataSet.Tables[0]);
        }

        public UserModel SaveUser(UserModel model)
        {
            List<Parameter> param = GetSaveUserParams(model);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);

            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertUpdateUser, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt16(r);
            if (Id > 0)
            {
                model.Success = true;
                model.Id = Id;
            }
            else if (Id == -1)
            {
                model.Success = false;
                model.ErrorMessage = "User already exists with this email id.";
            }
            else if (Id == -2)
            {
                model.Success = false;
                model.ErrorMessage = "Email Id is blank.";
            }
            else if (Id == -3)
            {
                model.Success = false;
                model.ErrorMessage = "User already exists with this phone number.";
            }
            else
            {
                model.Success = false;
                model.ErrorMessage = "Error Occured.";
            }
            return model;
        }

        private List<Parameter> GetSaveUserParams(UserModel userModel)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@UserID", userModel.Id));
            param.Add(new Parameter("@EmailID", userModel.Email));
            param.Add(new Parameter("@Password", userModel.Password));
            param.Add(new Parameter("@PhoneNumber", userModel.PhoneNumber));
            param.Add(new Parameter("@IsPhoneNumberVerified", userModel.IsPhoneNoVerified));
            param.Add(new Parameter("@TenantID", userModel.TenantID));
            param.Add(new Parameter("@IsDeleted", userModel.IsDeleted));
            param.Add(new Parameter("@CreatedBy", userModel.CreatedBy));
            param.Add(new Parameter("@CreatedByEntity", userModel.CreatedByEntity));
            param.Add(new Parameter("@ModifiedBy", userModel.ModifiedBy));
            param.Add(new Parameter("@ModifiedByEntity", userModel.ModifiedByEntity));
            param.Add(new Parameter("@FirstName", userModel.FirstName));
            param.Add(new Parameter("@LastName", userModel.LastName));
            return param;
        }

        public OTPModel SaveOTP(OTPModel model)
        {
            ///incomplete  pls complete
            List<Parameter> param = new List<Parameter>();
            var p_otpCode = new Parameter("@otpCode", model.otpCode);
            var p_otpMobileNo = new Parameter("@otpMobileNo", model.otpMobileNo);

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_otpCode);
            param.Add(p_otpMobileNo);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.SaveOtp
                    , DBCommandType.Procedure
                    , param.ToArray()
                   );

            int retValue = Convert.ToInt32(r);

            if (retValue < (int)StoreProcStatusEnum.Success)
            {
                model.Success = false;
                model.ErrorMessage = p_ErrorMsg.ToString();
            }
            else
            {
                model.Success = true;
                model.Id = retValue;
            }
            return model;
        }

        public OTPModel ValidateOTP(OTPModel model)
        {
            ///incomplete  pls complete
            List<Parameter> param = new List<Parameter>();
            var p_otpCode = new Parameter("@otpCode", model.otpCode);
            var p_otpMobileNo = new Parameter("@otpMobileNo", model.otpMobileNo);
            var p_UserId = new Parameter("@userId", model.UserId);

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_otpCode);
            param.Add(p_otpMobileNo);
            param.Add(p_UserId);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);

            var r = dataBaseService.ExecuteScalar(StoredProcedures.stp_ValidateOTP
                    , DBCommandType.Procedure
                    , param.ToArray()
                   );

            int retValue = Convert.ToInt32(r);

            if (retValue < (int)StoreProcStatusEnum.Success)
            {
                model.Success = false;
                model.ErrorMessage = p_ErrorMsg.ToString();
            }
            else
            {
                model.Success = true;
                model.Id = retValue;
            }
            return model;
        }

        public IEnumerable<ConsultationModel> GetMyConsultations(long userId, int tanentId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@UserType", Convert.ToInt32(1)));
            param.Add(new Parameter("@Id", userId));
            param.Add(new Parameter("@TenantID", tanentId));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetMyConsultations, DBCommandType.Procedure, param.ToArray());
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var consultations = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapMyConsultation(r, tanentId)).ToList();
                return consultations;
            }
            return null;
        }

        private ConsultationModel MapMyConsultation(DataRow row, int tanentId)
        {
            ConsultationModel consultationModel = null;
            if (row != null)
            {
                consultationModel = new ConsultationModel();
                consultationModel.Id = Convert.ToInt32(row["ConsultationID"]);
                consultationModel.Description = row["ConsultationDescription"] != null ? Convert.ToString(row["ConsultationDescription"]) : string.Empty;
                consultationModel.ConsultationTime = new InternalDateTime(Convert.ToInt64(row["PreferredTime"]));
                consultationModel.PatientId = Convert.ToInt32(row["PatientID"]);
                consultationModel.DoctorId = row["DoctorID"] != null ? Convert.ToInt64(row["DoctorID"]) : 0;
                consultationModel.ConsultationModeID = row["ConsultationModeID"] != null ? Convert.ToInt32(row["ConsultationModeID"]) : 0;
                consultationModel.ConsultationMode = row["ModeDescription"] != null ? Convert.ToString(row["ModeDescription"]) : string.Empty;
                consultationModel.ConsultationStatusID = row["ConsultationStatusID"] != null ? Convert.ToInt32(row["ConsultationStatusID"]) : 0;
                consultationModel.ConsultationStatus = row["ConsultationStatusDesc"] != null ? Convert.ToString(row["ConsultationStatusDesc"]) : string.Empty;
                consultationModel.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                consultationModel.CreatedByEntity = Convert.ToInt32(row["CreatedByEntity"]);
                consultationModel.CreatedOn = new InternalDateTime(Convert.ToInt64(row["CreatedOn"]));
                consultationModel.ModifiedBy = row["ModifiedBy"] != null ? Convert.ToInt32(row["ModifiedBy"]) : 0;
                consultationModel.ModifiedByEntity = row["ModifiedByEntity"] != null ? Convert.ToInt32(row["ModifiedByEntity"]) : 0;
                consultationModel.ModifiedOn = new InternalDateTime(Convert.ToInt64(row["ModifiedOn"]));
                consultationModel.TenantID = row["TenantID"] != null ? Convert.ToInt32(row["TenantID"]) : 0;
                if (consultationModel.PatientId > 0)
                {
                    consultationModel.Patient = new PatientDetail()
                    {
                        PatientName = row["PatientName"] != null ? Convert.ToString(row["PatientName"]) : string.Empty
                    };
                }
                if (consultationModel.DoctorId > 0)
                {
                    consultationModel.Doctor = new DoctorModel()
                    {
                        FirstName = row["DoctorFirstName"] != null ? Convert.ToString(row["DoctorFirstName"]) : string.Empty,
                        LastName = row["DoctorLastName"] != null ? Convert.ToString(row["DoctorLastName"]) : string.Empty
                    };
                }
            }
            return consultationModel;
        }

        public IEnumerable<ConsultationModel> GetConsultations(long userId, int tanentId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@UserType", 1));
            param.Add(new Parameter("@Id", userId));
            param.Add(new Parameter("@TenantID", tanentId));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetConsultationList, DBCommandType.Procedure, param.ToArray());
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var consultations = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapConsultation(r, tanentId)).ToList();
                return consultations;
            }
            return null;
        }

        private ConsultationModel MapConsultation(DataRow row, int tanentId)
        {
            ConsultationModel consultationModel = null;
            if (row != null)
            {
                consultationModel = new ConsultationModel();
                consultationModel.Id = Convert.ToInt32(row["ConsultationID"]);
                consultationModel.ConsultationMode = row["ModeDescription"] != null ? Convert.ToString(row["ModeDescription"]) : string.Empty;
                consultationModel.ConsultationModeID = row["ConsultationModeID"] != null ? Convert.ToInt32(row["ConsultationModeID"]) : 0;
                consultationModel.ConsultationStatus = row["ConsultationStatusDesc"] != null ? Convert.ToString(row["ConsultationStatusDesc"]) : string.Empty;
                consultationModel.ConsultationStatusID = row["ConsultationStatusID"] != null ? Convert.ToInt32(row["ConsultationStatusID"]) : 0;
                consultationModel.ConsultationTime = new InternalDateTime(Convert.ToInt64(row["PreferredTime"]));
                consultationModel.Description = row["ConsultationDescription"] != null ? Convert.ToString(row["ConsultationDescription"]) : string.Empty;
                consultationModel.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                consultationModel.CreatedByEntity = Convert.ToInt32(row["CreatedByEntity"]);
                consultationModel.CreatedOn = new InternalDateTime(Convert.ToInt64(row["CreatedOn"]));
                consultationModel.ModifiedBy = row["ModifiedBy"] != null ? Convert.ToInt32(row["ModifiedBy"]) : 0;
                consultationModel.ModifiedByEntity = row["ModifiedByEntity"] != null ? Convert.ToInt32(row["ModifiedByEntity"]) : 0;
                consultationModel.ModifiedOn = new InternalDateTime(Convert.ToInt64(row["ModifiedOn"]));
                consultationModel.TenantID = row["TenantID"] != null ? Convert.ToInt32(row["TenantID"]) : 0;
                consultationModel.DoctorId = row["DoctorID"] != null ? Convert.ToInt64(row["DoctorID"]) : 0;
                consultationModel.PatientId = row["PatientID"] != null ? Convert.ToInt64(row["PatientID"]) : 0;
                consultationModel.AmountCharged = Convert.ToDecimal(row["AmountCharged"]);
                //consultationModel.PrescriptionList = row["Prescriptions"] != null ? Convert.ToString(row["Prescriptions"]) : string.Empty;
                //consultationModel.CaseNotesList = row["CaseNotes"] != null ? Convert.ToString(row["CaseNotes"]) : string.Empty;
                consultationModel.Patient = new PatientDetail()
                {
                    Id = Convert.ToInt32(row["PatientID"]),
                    PatientName = row["PatientName"] != null ? Convert.ToString(row["PatientName"]) : string.Empty,
                    PatientGender = row["PatientGender"] != null ? Convert.ToChar(row["PatientGender"]) : ' ',
                    PatientPhone = row["PatientPhone"] != null ? Convert.ToString(row["PatientPhone"]) : string.Empty,
                    PatientAge = row["PatientAge"] != null ? Convert.ToInt16(row["PatientAge"]) : (short)0,
                    Complaints = row["Complaints"] != null ? Convert.ToString(row["Complaints"]) : string.Empty,
                    UserID = Convert.ToInt32(row["UserID"]),
                    CreatedBy = Convert.ToInt32(row["PatientCreatedBy"]),
                    CreatedByEntity = Convert.ToInt32(row["PatientCreatedByEntity"]),
                    //CreatedOn = (InternalDateTime)row["PatientCreatedOn"],
                    ModifiedBy = row["PatientModifiedBy"] != null ? Convert.ToInt32(row["PatientModifiedBy"]) : 0,
                    ModifiedByEntity = row["PatientModifiedByEntity"] != null ? Convert.ToInt32(row["PatientModifiedByEntity"]) : 0,
                    //ModifiedOn = (InternalDateTime)row["PatientModifiedOn"],
                    TenantID = row["PatientTenantId"] != null ? Convert.ToInt32(row["PatientTenantId"]) : 0,
                    IsDeleted = Convert.ToBoolean(row["PatientIsDeleted"]),
                    Files = row["Files"] != null ? Convert.ToString(row["Files"]) : string.Empty,
                    Case = row["Case"] != null ? Convert.ToInt16(row["Case"]) : (Int16)0,
                    PreferredTime = row["PatientPreferredTime"] != null ? Convert.ToInt16(row["PatientPreferredTime"]) : (Int16)0
                };
                DoctorService doctoreService = new DoctorService(this.dataBaseService);
                consultationModel.Doctor = doctoreService.GetDoctor(consultationModel.DoctorId, tanentId);
            }
            return consultationModel;
        }

        public ChangePasswordModel ChangeUserPassword(ChangePasswordModel changePasswordModel)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Email", changePasswordModel.Email));
            param.Add(new Parameter("@OldPassword", changePasswordModel.OldPassword));
            param.Add(new Parameter("@NewPassword", changePasswordModel.NewPassword));
            param.Add(new Parameter("@NewHashPassword", changePasswordModel.NewHashPassword));
            param.Add(new Parameter("@TenantId", changePasswordModel.TenantID));
            param.Add(new Parameter("@CreatedByEntity", changePasswordModel.CreatedByEntity));

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.ChangeUserPassword, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt16(r);
            if (Id > 0)
            {
                changePasswordModel.Success = true;
                changePasswordModel.Id = Id;
            }
            else if (Id == -3)
            {
                changePasswordModel.Success = false;
                changePasswordModel.ErrorMessage = "Old password not correct";
            }
            else if (Id == -2)
            {
                changePasswordModel.Success = false;
                changePasswordModel.ErrorMessage = "Invalid login details provided.";
            }
            return changePasswordModel;
        }

        public UserModel ValidateUserEmail(ChangePasswordModel changePasswordModel)
        {
            var userModel = new UserModel();
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Email", changePasswordModel.Email));
            param.Add(new Parameter("@DefaultPassword", changePasswordModel.NewPassword));
            param.Add(new Parameter("@TenantId", changePasswordModel.TenantID));
            param.Add(new Parameter("@CreatedByEntity", changePasswordModel.CreatedByEntity));

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            DataSet dataSet = dataBaseService.GetDataSet(StoredProcedures.ValidateUserEmail, DBCommandType.Procedure, param.ToArray());
            if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
            {
                userModel.Success = true;
                userModel = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapUser(r)).FirstOrDefault();
            }

            return userModel;
        }

        public MessageModel SaveMessage(MessageModel messageModel, int tanentId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@ID", messageModel.Id));
            param.Add(new Parameter("@ConsultationID", messageModel.ConsultationId));
            param.Add(new Parameter("@TenantID", tanentId));
            param.Add(new Parameter("@IsDeleted", 0));
            param.Add(new Parameter("@CreatedBy", messageModel.CreatedBy));
            param.Add(new Parameter("@CreatedByEntity", messageModel.CreatedByEntity));
            param.Add(new Parameter("@ModifiedBy", messageModel.ModifiedBy));
            param.Add(new Parameter("@ModifiedByEntity", messageModel.ModifiedByEntity));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertUpdateMessage, DBCommandType.Procedure, param.ToArray<Parameter>());
            int Id;
            if (int.TryParse(r.ToString(), out Id))
            {
                messageModel.Success = true;
                messageModel.Id = Id;
            }
            else
            {
                messageModel.Success = false;
                messageModel.ErrorMessage = "Error in insert or update user";
            }
            return messageModel;
        }

        public bool UpdateDeviceId(Int64 userId, string deviceId, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@UserId", userId));
            param.Add(new Parameter("@DeviceId", deviceId));
            param.Add(new Parameter("@TenantID", tenantId));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            int rowUpdated = dataBaseService.ExecuteNonQuery(StoredProcedures.UpdateUserDeviceId, DBCommandType.Procedure, param.ToArray<Parameter>());
            if (rowUpdated > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
    }

}
