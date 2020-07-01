using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using ImTech.DataBase;
using ImTech.DataServices;
using ImTech.Model;
using ImTech.Model.Doctor;

namespace ImTech.DataServices
{
    public class DoctorService
    {
        IDataBaseService dataBaseService;
        public DoctorService(IDataBaseService databaseService)
        {
            dataBaseService = databaseService;
        }

        #region private Method



        #endregion

        #region public Method

        public DoctorLogOnModel ValidateDoctorLogOn(DoctorLogOnModel logOnModel)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Email", logOnModel.Email));
            param.Add(new Parameter("@Password", logOnModel.Password));
            param.Add(new Parameter("@TenantId", logOnModel.TenantID));
            param.Add(new Parameter("@Location", logOnModel.LoginLocation == null ? string.Empty : logOnModel.LoginLocation));
            //param.Add(new Parameter("@LoginDate", logOnModel.LoginDate.DateTimeInLong));
            param.Add(new Parameter("@CreatedByEntity", logOnModel.CreatedByEntity));

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.ValidateDoctorLoginDetail, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt32(r);
            if (Id > 0)
            {
                logOnModel.Success = true;
                logOnModel.Id = Id;
            }
            else
            {
                logOnModel.Success = false;
                logOnModel.ErrorMessage = "Invalid userid/password";
            }
            return logOnModel;
        }

        public bool UpdateDeviceId(DoctorLogOnModel logOnModel)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@DoctorId", logOnModel.Id));
            param.Add(new Parameter("@DeviceId", logOnModel.DeviceId));
            param.Add(new Parameter("@TenantId", logOnModel.TenantID));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteNonQuery(StoredProcedures.UpdateDoctorDeviceId, DBCommandType.Procedure, param.ToArray());
            if (r > 0)
            {
                return true;
            }
            return false;
        }

        public bool SetDoctorAvailability(long doctorId, bool IsAvailable, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@DoctorId", doctorId));
            param.Add(new Parameter("@IsAvailable", IsAvailable));
            param.Add(new Parameter("@TenantId", tenantId));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteNonQuery(StoredProcedures.UpdateDoctorAvailability, DBCommandType.Procedure, param.ToArray());
            if (r > 0)
            {
                return true;
            }
            return false;
        }

        public DoctorModel GetDoctor(Int64 id, int tenantId)
        {
            DoctorModel doctorModel = new DoctorModel();
            List<Parameter> doctorParams = new List<Parameter>();

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);

            doctorParams.Add(new Parameter("@DoctorID", id));
            doctorParams.Add(new Parameter("@TenantID", tenantId));
            doctorParams.Add(p_IsError);
            doctorParams.Add(p_ErrorMsg);

            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDoctorOrDoctorList, DBCommandType.Procedure, doctorParams.ToArray());
            if (dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                MapDoctorPersonalDetail(dataSet.Tables[0].Rows[0], ref doctorModel);
                List<int> lst = null;
                if (dataSet.Tables[0].Rows[0] != null)
                {
                    string degreesMapped = Convert.ToString(dataSet.Tables[0].Rows[0]["DoctorDegrees"]);
                    lst = new List<int>();
                    foreach (string degreeID in degreesMapped.Split(',').ToList<string>())
                    {
                        int dId;
                        if (!string.IsNullOrEmpty(degreeID) && int.TryParse(degreeID, out dId))
                        {
                            lst.Add(dId);
                        }
                        doctorModel.DoctorDegree = lst;
                    }
                    string deceasesMapped = Convert.ToString(dataSet.Tables[0].Rows[0]["Diseases"]);
                    lst = new List<int>();
                    foreach (string deceaseID in deceasesMapped.Split(',').ToList<string>())
                    {
                        int dId;
                        if (!string.IsNullOrEmpty(deceaseID) && int.TryParse(deceaseID, out dId))
                        {
                            lst.Add(dId);
                        }
                        doctorModel.DoctorDesease = lst;
                    }
                    lst = new List<int>();
                    string hospitalMapped = Convert.ToString(dataSet.Tables[0].Rows[0]["Hospitals"]);
                    foreach (string hospitalID in hospitalMapped.Split(',').ToList<string>())
                    {
                        int hId;
                        if (!string.IsNullOrEmpty(hospitalID) && int.TryParse(hospitalID, out hId))
                        {
                            lst.Add(hId);
                        }
                        doctorModel.DoctorHospital = lst;
                    }
                    lst = new List<int>();
                    string specializationMapped = Convert.ToString(dataSet.Tables[0].Rows[0]["Specializations"]);
                    foreach (string specializationID in specializationMapped.Split(',').ToList<string>())
                    {
                        int sId;
                        if (!string.IsNullOrEmpty(specializationID) && int.TryParse(specializationID, out sId))
                        {
                            lst.Add(sId);
                        }
                        doctorModel.DoctorSpecialization = lst;
                    }
                }
                doctorModel.Success = true;
                doctorModel.ErrorMessage = string.Empty;
            }
            else
            {
                doctorModel.Success = false;
                doctorModel.ErrorMessage = string.Format("Error, Not able to fetch doctor.");
            }
            return doctorModel;
        }

        private void MapDoctorPersonalDetail(DataRow row, ref DoctorModel model)
        {
            if (row != null && model != null)
            {
                model.Id = Convert.ToInt32(row["DoctorId"]);
                model.FirstName = row["DoctorFirstName"] != null ? Convert.ToString(row["DoctorFirstName"]) : string.Empty;
                model.LastName = row["DoctorLastName"] != null ? Convert.ToString(row["DoctorLastName"]) : string.Empty;
                model.EmailAddress = row["DoctorEmail"] != null ? Convert.ToString(row["DoctorEmail"]) : string.Empty;
                model.Address1 = row["DoctorAddress1"] != null ? Convert.ToString(row["DoctorEmail"]) : string.Empty;
                model.CityID = row["DoctorCityId"] != null ? Convert.ToInt32(row["DoctorCityId"]) : 0;
                model.Pincode = row["Pincode"] != null ? Convert.ToString(row["Pincode"]) : string.Empty;
                model.PhoneNumber = row["DoctorPhoneNumber"] != null ? Convert.ToString(row["DoctorPhoneNumber"]) : string.Empty;
                model.DoctorDescription = row["DoctorDescription"] != null ? Convert.ToString(row["DoctorDescription"]) : string.Empty;
                model.OtherInformation = row["OtherInformation"] != null ? Convert.ToString(row["OtherInformation"]) : string.Empty;
                model.RegistrationNumber = row["RegistrationNumber"] != null ? Convert.ToString(row["RegistrationNumber"]) : string.Empty;
                model.ProfilePhotoID = row["ProfilePhotoId"] != null ? Convert.ToInt32(row["ProfilePhotoId"]) : 0;
                model.TenantID = row["TenantID"] != null ? Convert.ToInt32(row["TenantID"]) : -1;
                model.IsDeleted = Convert.ToInt32(row["IsDeleted"]) == 0 ? true : false;
                model.CreatedBy = row["CreatedBy"] != null ? Convert.ToInt32(row["CreatedBy"]) : -1;
                model.CreatedByEntity = row["CreatedByEntity"] != null ? Convert.ToInt32(row["CreatedByEntity"]) : -1;
                model.ModifiedBy = row["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(row["ModifiedBy"]) : -1;
                model.ModifiedByEntity = row["ModifiedByEntity"] != DBNull.Value ? Convert.ToInt32(row["ModifiedByEntity"]) : -1;
                model.DoctorCode = row["DoctorCode"] != null ? Convert.ToString(row["DoctorCode"]) : string.Empty;
                model.VideoConsultationFee = row["VideoConsultantFees"] != null ? Convert.ToDecimal(row["VideoConsultantFees"]) : (decimal)0;
            }
        }

        public DoctorModel SaveDoctor(DoctorModel doctorModel)
        {
            List<Parameter> saveDoctorParam = GetSaveDoctorParams(doctorModel);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            saveDoctorParam.Add(p_IsError);
            saveDoctorParam.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertUpdateDoctor, DBCommandType.Procedure, saveDoctorParam.ToArray<Parameter>());
            int retValue = Convert.ToInt32(r);
            if (retValue < (int)StoreProcStatusEnum.Success)
            {
                doctorModel.Success = false;
                doctorModel.ErrorMessage = p_ErrorMsg.ToString();
            }
            else
            {
                doctorModel.Success = true;
                doctorModel.Id = retValue;
            }
            return doctorModel;
        }

        private List<Parameter> GetSaveDoctorParams(DoctorModel doctorModel)
        {
            return new Parameter[]
            {
                new Parameter("@Id",  doctorModel.Id),
                new Parameter("@FirstName", doctorModel.FirstName),
                new Parameter("@LastName", doctorModel.LastName),
                new Parameter("@EmailAddress", doctorModel.EmailAddress),
                new Parameter("@Address1", doctorModel.Address1),
                new Parameter("@CityID", doctorModel.CityID),
                new Parameter("@Password",doctorModel.Password),
                new Parameter("@HashPassword",doctorModel.HashPassword),
                new Parameter("@Pincode", doctorModel.Pincode),
                new Parameter("@PhoneNumber",  doctorModel.PhoneNumber),
                new Parameter("@OtherInfo", doctorModel.OtherInformation),
                new Parameter("@RegistrationNumber", doctorModel.RegistrationNumber),
                new Parameter("@ProfilePhotoID", doctorModel.ProfilePhotoID),
                new Parameter("@TenantID", doctorModel.TenantID),
                new Parameter("@CreatedByUserID", doctorModel.CreatedBy),
                new Parameter("@CreatedByEntity", doctorModel.CreatedByEntity),
                new Parameter("@UpdatedByUserID", doctorModel.ModifiedBy),
                new Parameter("@UpdatedByEntity", doctorModel.ModifiedByEntity),
                new Parameter("@degree",string.Join(",",doctorModel.DoctorDegree.ToArray())),
                new Parameter("@diseases", string.Join(",",doctorModel.DoctorDesease.ToArray())),
                new Parameter("@hospitals", string.Join(",",doctorModel.DoctorHospital.ToArray())),
                new Parameter("@specializations", string.Join(",",doctorModel.DoctorSpecialization.ToArray())),
                new Parameter("@PersonalConsultationFee", doctorModel.PersonalConsultationFee,ParameterDirection.Input,DbType.Decimal),
                new Parameter("@PhoneConsultationFee", doctorModel.PhoneConsultationFee,ParameterDirection.Input,DbType.Decimal),
                new Parameter("@TextConsultationFee", doctorModel.TextConsultationFee,ParameterDirection.Input,DbType.Decimal),
                new Parameter("@VideoConsultationFee", doctorModel.VideoConsultationFee,ParameterDirection.Input,DbType.Decimal),
                new Parameter("@DoctorDescription", doctorModel.DoctorDescription),
                new Parameter("@DoctorCode", doctorModel.DoctorCode),
            }.ToList<Parameter>();
        }

        public IEnumerable<DoctorModel> GetDoctorList(int doctorID, int tanentID)
        {
            DoctorModel doctorModel = new DoctorModel();
            List<Parameter> doctorParams = new List<Parameter>();
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);

            doctorParams.Add(new Parameter("@DoctorID", doctorID));
            doctorParams.Add(new Parameter("@TenantID", tanentID));
            doctorParams.Add(p_IsError);
            doctorParams.Add(p_ErrorMsg);

            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDoctorOrDoctorList, DBCommandType.Procedure, doctorParams.ToArray());
            List<DoctorModel> lst = null;
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                lst = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => Map(r)).ToList<DoctorModel>();
            }
            return lst;
        }

        IEnumerable<DoctorModel> Map(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows.Cast<DataRow>())
            {
                yield return Map(row);
            }
        }

        DoctorModel Map(DataRow row)
        {
            return new DoctorModel
            {
                Id = Convert.ToInt32(row["DoctorId"]),
                FirstName = row["DoctorFirstName"] != null ? Convert.ToString(row["DoctorFirstName"]) : string.Empty,
                LastName = row["DoctorLastName"] != null ? Convert.ToString(row["DoctorLastName"]) : string.Empty,
                EmailAddress = Convert.ToString(row["DoctorEmail"]),
                Address1 = row["DoctorAddress1"] != null ? Convert.ToString(row["DoctorAddress1"]) : string.Empty,
                CityID = row["DoctorCityID"] != null ? Convert.ToInt32(row["DoctorCityID"]) : 0,
                Pincode = row["Pincode"] != null ? Convert.ToString(row["Pincode"]) : string.Empty,
                PhoneNumber = row["DoctorPhoneNumber"] != null ? Convert.ToString(row["DoctorPhoneNumber"]) : string.Empty,
                OtherInformation = row["OtherInformation"] != null ? Convert.ToString(row["OtherInformation"]) : string.Empty,
                ProfilePhotoID = row["ProfilePhotoID"] != null ? Convert.ToInt32(row["ProfilePhotoID"]) : 0,
                PersonalConsultationFee = row["PersonalConsultantFees"] != null ? Convert.ToDecimal(row["PersonalConsultantFees"]) : (decimal)0,
                PhoneConsultationFee = row["PhoneConsultantFees"] != null ? Convert.ToDecimal(row["PhoneConsultantFees"]) : (decimal)0,
                TextConsultationFee = row["TextConsultantFees"] != null ? Convert.ToDecimal(row["TextConsultantFees"]) : (decimal)0,
                VideoConsultationFee = row["VideoConsultantFees"] != null ? Convert.ToDecimal(row["VideoConsultantFees"]) : (decimal)0,
                DoctorDescription = row["DoctorDescription"] != null ? Convert.ToString(row["DoctorDescription"]) : string.Empty,
                DoctorCode = row["DoctorCode"] != null ? Convert.ToString(row["DoctorCode"]) : string.Empty,
                TenantID = Convert.ToInt32(row["TenantID"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                CreatedOn = new InternalDateTime(Convert.ToInt64(row["CreatedOn"])),// row["CreatedOn"] != null; ? (InternalDateTime)Convert.ToInt64(row["CreatedOn"]) : null,
                CreatedByEntity = Convert.ToInt32(row["CreatedByEntity"]),
                ModifiedBy = row["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(row["ModifiedBy"]) : -1,
                //ModifiedOn = new InternalDateTime(Convert.ToInt64(row["ModifiedOn"])),// row["ModifiedOn"] != null ? (InternalDateTime)row["ModifiedOn"] : null,
                ModifiedByEntity = row["ModifiedByEntity"] != DBNull.Value ? Convert.ToInt32(row["ModifiedByEntity"]) : -1,
            };
        }

        public IEnumerable<ConsultationModel> GetMyConsultations(long userId, int tanentId)
        {
            List<Parameter> param = new List<Parameter>();
            int userType = 2; //for doctor
            param.Add(new Parameter("@UserType", userType));
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
            }
            return consultationModel;
        }

        public IEnumerable<ConsultationModel> GetConsultations(Int64 Id, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@UserType", Convert.ToInt32(2)));
            param.Add(new Parameter("@Id", Id));
            param.Add(new Parameter("@TenantID", tenantId));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetConsultationList, DBCommandType.Procedure, param.ToArray());
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var consultations = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapConsultation(r)).ToList();
                return consultations;
            }
            return null;
        }

        private ConsultationModel MapConsultation(DataRow row)
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
                consultationModel.DoctorId = row["DoctorID"] != null ? Convert.ToInt32(row["DoctorID"]) : 0;
                consultationModel.PatientId = row["PatientID"] != null ? Convert.ToInt32(row["PatientID"]) : 0;
                consultationModel.AmountCharged = Convert.ToDecimal(row["AmountCharged"]);
                //consultationModel.PrescriptionList = row["Prescriptions"] != null ? Convert.ToString(row["Prescriptions"]) : string.Empty;
                //consultationModel.CaseNotesList = row["CaseNotes"] != null ? Convert.ToString(row["CaseNotes"]) : string.Empty;
                consultationModel.Patient = new PatientDetail()
                {
                    Id = Convert.ToInt32(row["PatientID"]),
                    PatientName = row["PatientName"] != null ? Convert.ToString(row["PatientName"]) : string.Empty,
                    PatientGender = row["PatientGender"] != null ? Convert.ToChar(row["PatientGender"]) : ' ',
                    PatientPhone = row["PatientPhone"] != null ? Convert.ToString(row["PatientPhone"]) : string.Empty,
                    PatientAge = row["PatientAge"] != null ? Convert.ToInt16(row["PatientAge"]) : (short)-1,
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
            }
            return consultationModel;
        }

        public IEnumerable<DoctorModel> GetDoctorsByCode(string code, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Code", code));
            param.Add(new Parameter("@TenantID", tenantId));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDoctorOrDoctorListByCode, DBCommandType.Procedure, param.ToArray());
            List<DoctorModel> lst = null;
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var doctors = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapDoctor(r)).ToList<DoctorModel>();

                for (int i = 0; i < doctors.Count; i++)
                {
                    doctors[i].DoctorDegreeList.AddRange(GetDoctorDegrees(doctors[i].Id, tenantId).ToList());
                    doctors[i].DoctorHospitalList.AddRange(GetDoctorHospitals(doctors[i].Id, tenantId).ToList());
                    doctors[i].DoctorSpecialzationList.AddRange(GetDoctorSpecializations(doctors[i].Id, tenantId).ToList());
                    doctors[i].DoctorDeseaseList.AddRange(GetDoctorDeseases(doctors[i].Id, tenantId).ToList());
                }
                return doctors;
            }
            return null;
        }

        DoctorModel MapDoctor(DataRow row)
        {
            return new DoctorModel
            {
                Id = Convert.ToInt32(row["DoctorID"]),
                FirstName = row["DoctorFirstName"] != null ? Convert.ToString(row["DoctorFirstName"]) : string.Empty,
                LastName = row["DoctorLastName"] != null ? Convert.ToString(row["DoctorLastName"]) : string.Empty,
                EmailAddress = Convert.ToString(row["DoctorEmail"]),
                Address1 = row["DoctorAddress1"] != null ? Convert.ToString(row["DoctorAddress1"]) : string.Empty,
                CityName = row["CityName"] != null ? Convert.ToString(row["CityName"]) : string.Empty,
                StateName = row["StateName"] != null ? Convert.ToString(row["StateName"]) : string.Empty,
                CountryName = row["CountryName"] != null ? Convert.ToString(row["CountryName"]) : string.Empty,
                Pincode = row["Pincode"] != null ? Convert.ToString(row["Pincode"]) : string.Empty,
                PhoneNumber = row["DoctorPhoneNumber"] != null ? Convert.ToString(row["DoctorPhoneNumber"]) : string.Empty,
                OtherInformation = row["OtherInformation"] != null ? Convert.ToString(row["OtherInformation"]) : string.Empty,
                RegistrationNumber = row["RegistrationNumber"] != null ? Convert.ToString(row["RegistrationNumber"]) : string.Empty,
                TenantID = Convert.ToInt32(row["TenantID"]),
                ProfilePhotoID = row["ProfilePhotoID"] != null ? Convert.ToInt32(row["ProfilePhotoID"]) : 0,
                PersonalConsultationFee = row["PersonalConsultantFees"] != null ? Convert.ToDecimal(row["PersonalConsultantFees"]) : (decimal)0,
                PhoneConsultationFee = row["PhoneConsultantFees"] != null ? Convert.ToDecimal(row["PhoneConsultantFees"]) : (decimal)0,
                TextConsultationFee = row["TextConsultantFees"] != null ? Convert.ToDecimal(row["TextConsultantFees"]) : (decimal)0,
                VideoConsultationFee = row["VideoConsultantFees"] != null ? Convert.ToDecimal(row["VideoConsultantFees"]) : (decimal)0,
                DoctorDescription = row["DoctorDescription"] != null ? Convert.ToString(row["DoctorDescription"]) : string.Empty,
                DoctorCode = row["DoctorCode"] != null ? Convert.ToString(row["DoctorCode"]) : string.Empty
            };
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

        public IEnumerable<HospitalModel> GetDoctorHospitals(int doctorId, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@DoctorID", doctorId));
            param.Add(new Parameter("@TenantID", tenantId));
            var p_IsError = new Parameter("@IsError", ParameterDirection.Output);
            var p_ErrorMsg = new Parameter("@ErrorMsg", ParameterDirection.Output);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDoctorHospitals, DBCommandType.Procedure, param.ToArray());

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var hospitals = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapDoctorHospitals(r));
                return hospitals;
            }
            return null;
        }

        HospitalModel MapDoctorHospitals(DataRow row)
        {
            return new HospitalModel
            {
                Id = Convert.ToInt32(row["HospitalID"]),
                HospitalName = Convert.ToString(row["HospitalName"]),
                HospitalAddress1 = Convert.ToString(row["HospitalAddress1"]),
                HospitalEmail = Convert.ToString(row["HospitalEmailID"]),
                HospitalFax = Convert.ToString(row["HospitalFaxNumber"]),
                HospitalPhone1 = Convert.ToString(row["HospitalPhoneNumber1"]),
                HospitalPhone2 = Convert.ToString(row["HospitalPhoneNumber2"]),
                CityName = Convert.ToString(row["CityName"]),
                StateName = Convert.ToString(row["StateName"]),
                CountryName = Convert.ToString(row["CountryName"]),
                HospitalCode = Convert.ToString(row["HospitalCode"]),
                TenantID = Convert.ToInt32(row["TenantID"]),

            };
        }

        public IEnumerable<SpecializationModel> GetDoctorSpecializations(int doctorId, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@DoctorID", doctorId));
            param.Add(new Parameter("@TenantID", tenantId));
            var p_IsError = new Parameter("@IsError", ParameterDirection.Output);
            var p_ErrorMsg = new Parameter("@ErrorMsg", ParameterDirection.Output);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDoctorSpecializations, DBCommandType.Procedure, param.ToArray());

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var specializations = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapDoctorSpecializations(r));
                return specializations;
            }
            return null;
        }

        SpecializationModel MapDoctorSpecializations(DataRow row)
        {
            return new SpecializationModel
            {
                Id = Convert.ToInt32(row["SpecializationID"]),
                SpecializationName = Convert.ToString(row["SpecializationName"]),
                TenantID = Convert.ToInt32(row["TenantID"])
            };
        }

        public IEnumerable<DeseaseModel> GetDoctorDeseases(int doctorId, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@DoctorID", doctorId));
            param.Add(new Parameter("@TenantID", tenantId));
            var p_IsError = new Parameter("@IsError", ParameterDirection.Output);
            var p_ErrorMsg = new Parameter("@ErrorMsg", ParameterDirection.Output);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDoctorDeseases, DBCommandType.Procedure, param.ToArray());

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var deseases = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapDoctorDeseases(r));
                return deseases;
            }
            return null;
        }

        DeseaseModel MapDoctorDeseases(DataRow row)
        {
            return new DeseaseModel
            {
                Id = Convert.ToInt32(row["DeseaseID"]),
                DeseaseName = Convert.ToString(row["DeseaseName"]),
                TenantID = Convert.ToInt32(row["TenantID"])
            };
        }

        public string MapDoctorToUser(Int64 userId, string code, int tenantId, Int64 createdBy, Int64 createdByEntity)
        {
            string error = string.Empty;
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@UserId", userId));
            param.Add(new Parameter("@Code", code));
            param.Add(new Parameter("@TenantID", tenantId));
            param.Add(new Parameter("@CreatedBy", createdBy));
            param.Add(new Parameter("@CreatedByEntity", createdByEntity));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var res = dataBaseService.ExecuteScalar(StoredProcedures.UpdateUserDoctorMapping, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt16(res);
            if (Id >= 0)
            {
                error = "";
            }
            else if (Id == -3)
            {
                error = "Doctor Already Added To Your List.";
            }
            else if ((Id == -1) || (Id == -4) || (Id == -5))
            {
                error = "Error! Please Check Doctor Code.";
            }
            return error;
        }

        public IEnumerable<DoctorModel> GetMappedDoctorForUser(Int64 userId, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@UserId", userId));
            param.Add(new Parameter("@TenantID", tenantId));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);

            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetMappedDoctorForUser, DBCommandType.Procedure, param.ToArray());
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var doctors = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapDoctor(r)).ToList<DoctorModel>();

                for (int i = 0; i < doctors.Count; i++)
                {
                    doctors[i].DoctorDegreeList.AddRange(GetDoctorDegrees(doctors[i].Id, tenantId).ToList());
                    doctors[i].DoctorHospitalList.AddRange(GetDoctorHospitals(doctors[i].Id, tenantId).ToList());
                    doctors[i].DoctorSpecialzationList.AddRange(GetDoctorSpecializations(doctors[i].Id, tenantId).ToList());
                    doctors[i].DoctorDeseaseList.AddRange(GetDoctorDeseases(doctors[i].Id, tenantId).ToList());
                }
                //foreach (DoctorModel doctorModel in doctors)
                //{
                //    doctorModel.DoctorDegreeList.AddRange(GetDoctorDegrees(doctorModel.Id, tenantId).ToList());
                //    doctorModel.DoctorHospitalList.AddRange(GetDoctorHospitals(doctorModel.Id, tenantId).ToList());
                //    doctorModel.DoctorSpecialzationList.AddRange(GetDoctorSpecializations(doctorModel.Id, tenantId).ToList());
                //    doctorModel.DoctorDeseaseList.AddRange(GetDoctorDeseases(doctorModel.Id, tenantId).ToList());
                //}
                return doctors;
            }
            return null;
        }

        public DoctorModel DeleteDoctor(DoctorModel model)
        {
            var p_DoctorID = new Parameter("@DoctorID", model.Id);
            var p_TenantID = new Parameter("@TenantID", model.TenantID);

            var p_IsError = new Parameter("@IsError", ParameterDirection.Output);
            var p_ErrorMsg = new Parameter("@ErrorMsg", ParameterDirection.Output);

            var r = dataBaseService.ExecuteScalar(StoredProcedures.DeleteDoctor
                    , DBCommandType.Procedure
                    , p_DoctorID
                    , p_TenantID

                    , p_IsError
                    , p_ErrorMsg
                   );

            int retValue = Convert.ToInt32(p_IsError);

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

        public ChangePasswordModel ChangeDoctorPasswordFromAdmin(ChangePasswordModel changePasswordModel)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Id", changePasswordModel.LongId));
            param.Add(new Parameter("@NewPassword", changePasswordModel.NewPassword));
            param.Add(new Parameter("@NewHashPassword", changePasswordModel.NewHashPassword));
            param.Add(new Parameter("@TenantId", changePasswordModel.TenantID));
            param.Add(new Parameter("@ModifiedByEntity", changePasswordModel.CreatedByEntity));
            param.Add(new Parameter("@ModifiedBy", changePasswordModel.CreatedBy));

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.ChangeDoctorPasswordById, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt16(r);
            if (Id > 0)
            {
                changePasswordModel.Success = true;
                changePasswordModel.Id = Id;
            }
            else
            {
                changePasswordModel.Success = false;
                changePasswordModel.ErrorMessage = "Invalid details";
            }
            return changePasswordModel;
        }

        public ChangePasswordModel ChangeDoctorPassword(ChangePasswordModel changePasswordModel)
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
            var r = dataBaseService.ExecuteScalar(StoredProcedures.ChangeDoctorPassword, DBCommandType.Procedure, param.ToArray());
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
            else if (Id == -1)
            {
                changePasswordModel.Success = false;
                changePasswordModel.ErrorMessage = "Invalid email id provided.";
            }
            return changePasswordModel;
        }

        public ChangePasswordModel ValidateDoctorEmail(ChangePasswordModel changePasswordModel)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Email", changePasswordModel.Email));
            param.Add(new Parameter("@TenantId", changePasswordModel.TenantID));
            param.Add(new Parameter("@CreatedByEntity", changePasswordModel.CreatedByEntity));

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.ValidateDoctorEmail, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt16(r);
            if (Id > 0)
            {
                changePasswordModel.Success = true;
                changePasswordModel.Id = Id;
            }
            else if (Id == -4)
            {
                changePasswordModel.Success = false;
                changePasswordModel.ErrorMessage = "Invalid user email provided provided.";
            }
            return changePasswordModel;
        }

        public DoctorStatsModel GetDoctorStats(long doctorId, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@DoctorID", doctorId));
            param.Add(new Parameter("@TenantID", tenantId));
            var p_IsError = new Parameter("@IsError", ParameterDirection.Output);
            var p_ErrorMsg = new Parameter("@ErrorMsg", ParameterDirection.Output);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDoctorStats, DBCommandType.Procedure, param.ToArray());
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var stat = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapDoctorStat(r)).FirstOrDefault();
                return stat;
            }
            return null;
        }

        DoctorStatsModel MapDoctorStat(DataRow row)
        {
            return new DoctorStatsModel
            {
                CountTxtConsultationInitiatedOrInProgress = Convert.ToInt32(row["CountTxtConsultationInitiatedOrInProgress"]),
                CountCallConsultationInitiatedOrInProgress = Convert.ToInt32(row["CountCallConsultationInitiatedOrInProgress"]),
                CountVideoConsultationInitiatedOrInProgress = Convert.ToInt32(row["CountVideoConsultationInitiatedOrInProgress"]),
                CountTxtConsultationClosed = Convert.ToInt32(row["CountTxtConsultationClosed"]),
                CountCallConsultationClosed = Convert.ToInt32(row["CountCallConsultationClosed"]),
                CountVideoConsultationCancelled = Convert.ToInt32(row["CountVideoConsultationCancelled"]),
                CountTxtConsultationCancelled = Convert.ToInt32(row["CountTxtConsultationCancelled"]),
                CountCallConsultationCancelled = Convert.ToInt32(row["CountCallConsultationCancelled"]),
                CountVideoConsultationClosed = Convert.ToInt32(row["CountVideoConsultationClosed"]),
                LastMonthRevenue = Convert.ToInt64(row["LastMonthRevenue"]),
                TotalRevenue = Convert.ToInt64(row["TotalRevenue"]),
            };
        }
        #endregion

        #region Code Not Used 
        //private void MapDoctorDeceases(DoctorModel doctorModel, string deceasesMapped)
        //{
        //    List<string> listDeceased = deceasesMapped.Split(',').ToList<string>();
        //    List<DeseaseModel> lst = new List<DeseaseModel>();
        //    if (listDeceased != null && listDeceased.Count > 0)
        //    {
        //        foreach (string decease in listDeceased)
        //        {
        //            int deceaseID = Convert.ToInt32(decease);
        //            DeseaseModel deceaseModel = GetDeseaseDetail(deceaseID, -1);
        //            if (deceaseModel != null)
        //            {
        //                if (doctorModel.DoctorDesease == null)
        //                {
        //                    doctorModel.DoctorDesease = new List<DeseaseModel>();
        //                }
        //                lst.Add(deceaseModel);
        //            }
        //        }
        //    }
        //    doctorModel.DoctorDesease = lst;
        //}

        //private void MapDoctorHospital(DoctorModel doctorModel, string hospitalMapped)
        //{
        //    List<string> listHospital = hospitalMapped.Split(',').ToList<string>();
        //    List<HospitalModel> lst = new List<HospitalModel>();
        //    if (listHospital != null && listHospital.Count > 0)
        //    {
        //        foreach (string hospital in listHospital)
        //        {
        //            int hospitalID = Convert.ToInt32(hospital);
        //            HospitalModel hospitalModel = GetHospitalDetail(hospitalID, -1);
        //            if (hospitalModel != null)
        //            {
        //                if (doctorModel.DoctorHospital == null)
        //                {
        //                    doctorModel.DoctorHospital = new List<HospitalModel>();
        //                }
        //                lst.Add(hospitalModel);
        //            }
        //        }
        //    }
        //    doctorModel.DoctorHospital = lst;
        //}

        //private void MapDoctorSpecialization(DoctorModel doctorModel, string specializationMapped)
        //{
        //    List<string> listSpecialization = specializationMapped.Split(',').ToList<string>();
        //    List<SpecializationModel> lst = new List<SpecializationModel>();
        //    if (listSpecialization != null && listSpecialization.Count > 0)
        //    {
        //        foreach (string specialization in listSpecialization)
        //        {
        //            int specializationID = Convert.ToInt32(specialization);
        //            SpecializationModel specializationModel = GetSpecializationDetail(specializationID, -1);
        //            if (specializationModel != null)
        //            {
        //                if (doctorModel.DoctorSpecialization == null)
        //                {
        //                    doctorModel.DoctorSpecialization = new List<SpecializationModel>();
        //                }
        //                lst.Add(specializationModel);
        //            }
        //        }
        //    }
        //    doctorModel.DoctorSpecialization = lst;
        //}

        //private void MapDoctorDegree(DoctorModel doctorModel, string degreesMapped)
        //{
        //    List<string> listDegrees = degreesMapped.Split(',').ToList<string>();
        //    List<DegreeModel> lst = new List<DegreeModel>();
        //    if (listDegrees != null && listDegrees.Count > 0)
        //    {
        //        foreach (string degree in listDegrees)
        //        {
        //            int degreeID = Convert.ToInt32(degree);
        //            DegreeModel degreeModel = GetDegreeDetail(degreeID, -1);
        //            if (degreeModel != null)
        //            {
        //                if (doctorModel.DoctorDegree == null)
        //                {
        //                    doctorModel.DoctorDegree = new List<DegreeModel>();
        //                }
        //                lst.Add(degreeModel);
        //            }
        //        }
        //    }
        //    doctorModel.DoctorDegree = lst;
        //}

        /// <summary>
        /// When you need degree for doctor pass tenant ID as -1
        /// When you need degree detail w.r.t. degree the pass usual tenantId
        /// </summary>
        /// <param name="degreeId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public DegreeModel GetDegreeDetail(int degreeId, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            DegreeModel degreeModel = new DegreeModel();
            param.Add(new Parameter("@Id", degreeId));
            param.Add(new Parameter("@TenantID", tenantId));
            var p_IsError = new Parameter("@IsError", ParameterDirection.Output);
            var p_ErrorMsg = new Parameter("@ErrorMsg", ParameterDirection.Output);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDegreeOrDegreeList, DBCommandType.Procedure, param.ToArray());
            int retValue = Convert.ToInt32(p_IsError);
            if (retValue < (int)StoreProcStatusEnum.Success)
            {

                degreeModel.Success = false;
                degreeModel.ErrorMessage = p_ErrorMsg.ToString();
            }
            else
            {
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    degreeModel.Success = true;
                    MapDegree(dataSet.Tables[0].Rows[0], ref degreeModel);
                }
                else
                {
                    degreeModel.Success = false;
                    degreeModel.ErrorMessage = string.Format("Error, Not able to fetch doctor.");
                }
            }
            return degreeModel;
        }

        private void MapDegree(DataRow row, ref DegreeModel model)
        {
            if (row != null && model != null)
            {
                model.Id = Convert.ToInt32(row["DegreeID"]);
                model.DegreeName = Convert.ToString(row["DegreeName"]);
                model.TenantID = Convert.ToInt32(row["TenantID"]);
                model.IsDeleted = Convert.ToInt32(row["IsDeleted"]) == 0 ? true : false;
                model.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                model.CreatedByEntity = Convert.ToInt32(row["CreatedByEntity"]);
                model.ModifiedBy = Convert.ToInt32(row["ModifiedBy"]);
                model.ModifiedByEntity = Convert.ToInt32(row["ModifiedByEntity"]);
                //model.ModifiedOn= Convert.ToDateTime(row["ModifiedOn"]);
                //model.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
            }
        }

        /// <summary>
        /// When you need desease for doctor pass tenant ID as -1
        /// When you need desease detail w.r.t. desease the pass usual tenantId
        /// </summary>
        /// <param name="deseaseId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public DeseaseModel GetDeseaseDetail(int deseaseId, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            DeseaseModel deseaseModel = new DeseaseModel();
            param.Add(new Parameter("@Id", deseaseId));
            param.Add(new Parameter("@TenantID", tenantId));
            var p_IsError = new Parameter("@IsError", ParameterDirection.Output);
            var p_ErrorMsg = new Parameter("@ErrorMsg", ParameterDirection.Output);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDeseaseOrDeseaseList, DBCommandType.Procedure, param.ToArray());
            int retValue = Convert.ToInt32(p_IsError);
            if (retValue < (int)StoreProcStatusEnum.Success)
            {
                deseaseModel.Success = false;
                deseaseModel.ErrorMessage = p_ErrorMsg.ToString();
            }
            else
            {
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    deseaseModel.Success = true;
                    MapDesease(dataSet.Tables[0].Rows[0], ref deseaseModel);
                }
                else
                {
                    deseaseModel.Success = false;
                    deseaseModel.ErrorMessage = string.Format("Error, Not able to fetch doctor.");
                }
            }
            return deseaseModel;
        }

        public void MapDesease(DataRow row, ref DeseaseModel model)
        {
            if (row != null && model != null)
            {
                model.Id = Convert.ToInt32(row["DeseaseID"]);
                model.DeseaseName = Convert.ToString(row["DeseaseName"]);
                model.TenantID = Convert.ToInt32(row["TenantID"]);
                model.IsDeleted = Convert.ToInt32(row["IsDeleted"]) == 0 ? true : false;
                model.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                model.CreatedByEntity = Convert.ToInt32(row["CreatedByEntity"]);
                model.ModifiedBy = Convert.ToInt32(row["ModifiedBy"]);
                model.ModifiedByEntity = Convert.ToInt32(row["ModifiedByEntity"]);
                //model.ModifiedOn= Convert.ToDateTime(row["ModifiedOn"]);
                //model.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
            }
        }

        /// <summary>
        /// When you need hospital for doctor pass tenant ID as -1
        /// When you need hospital detail w.r.t. hospital the pass usual tenantId
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public HospitalModel GetHospitalDetail(int hospitalID, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            HospitalModel hospitalModel = new HospitalModel();
            param.Add(new Parameter("@Id", hospitalID));
            param.Add(new Parameter("@TenantID", tenantId));
            var p_IsError = new Parameter("@IsError", ParameterDirection.Output);
            var p_ErrorMsg = new Parameter("@ErrorMsg", ParameterDirection.Output);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetHospitalOrHospitalList, DBCommandType.Procedure, param.ToArray());
            int retValue = Convert.ToInt32(p_IsError);
            if (retValue < (int)StoreProcStatusEnum.Success)
            {

                hospitalModel.Success = false;
                hospitalModel.ErrorMessage = p_ErrorMsg.ToString();
            }
            else
            {
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    hospitalModel.Success = true;
                    MapHospital(dataSet.Tables[0].Rows[0], ref hospitalModel);
                }
                else
                {
                    hospitalModel.Success = false;
                    hospitalModel.ErrorMessage = string.Format("Error, Not able to fetch doctor.");
                }
            }
            return hospitalModel;
        }

        public void MapHospital(DataRow row, ref HospitalModel model)
        {
            if (row != null && model != null)
            {
                model.Id = Convert.ToInt32(row["HospitalID"]);
                model.HospitalName = Convert.ToString(row["HospitalName"]);
                model.HospitalAddress1 = Convert.ToString(row["HospitalAddress1"]);
                model.HospitalCity = Convert.ToInt32(row["HospitalCity"]);
                model.HospitalState = Convert.ToInt32(row["HospitalState"]);
                model.HospitalCountry = Convert.ToInt32(row["HospitalCountry"]);
                model.HospitalPhone1 = Convert.ToString(row["HospitalPhoneNumber1"]);
                model.HospitalPhone2 = Convert.ToString(row["HospitalPhoneNumber2"]);
                model.HospitalFax = Convert.ToString(row["HospitalFaxNumber"]);
                model.HospitalEmail = Convert.ToString(row["HospitalEmailID"]);
                model.TenantID = Convert.ToInt32(row["TenantID"]);
                model.IsDeleted = Convert.ToInt32(row["IsDeleted"]) == 0 ? true : false;
                model.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                model.CreatedByEntity = Convert.ToInt32(row["CreatedByEntity"]);
                model.ModifiedBy = Convert.ToInt32(row["ModifiedBy"]);
                model.ModifiedByEntity = Convert.ToInt32(row["ModifiedByEntity"]);
                //model.ModifiedOn= Convert.ToDateTime(row["ModifiedOn"]);
                //model.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
            }
        }

        /// <summary>
        /// When you need specialization for doctor pass tenant ID as -1
        /// When you need specialization detail w.r.t. specialization the pass usual tenantId
        /// </summary>
        /// <param name="specializationID"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public SpecializationModel GetSpecializationDetail(int specializationId, int tenantId)
        {
            List<Parameter> param = new List<Parameter>();
            SpecializationModel specializationModel = new SpecializationModel();
            param.Add(new Parameter("@Id", specializationId));
            param.Add(new Parameter("@TenantID", tenantId));
            var p_IsError = new Parameter("@IsError", ParameterDirection.Output);
            var p_ErrorMsg = new Parameter("@ErrorMsg", ParameterDirection.Output);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetSpecializationOrSpecializationList, DBCommandType.Procedure, param.ToArray());
            int retValue = Convert.ToInt32(p_IsError);
            if (retValue < (int)StoreProcStatusEnum.Success)
            {

                specializationModel.Success = false;
                specializationModel.ErrorMessage = p_ErrorMsg.ToString();
            }
            else
            {
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    specializationModel.Success = true;
                    MapSpecialization(dataSet.Tables[0].Rows[0], ref specializationModel);
                }
                else
                {
                    specializationModel.Success = false;
                    specializationModel.ErrorMessage = string.Format("Error, Not able to fetch doctor.");
                }
            }
            return specializationModel;
        }

        public void MapSpecialization(DataRow row, ref SpecializationModel model)
        {
            if (row != null && model != null)
            {
                model.Id = Convert.ToInt32(row["SpecializationID"]);
                model.SpecializationName = Convert.ToString(row["SpecializationName"]);
                model.TenantID = Convert.ToInt32(row["TenantID"]);
                model.IsDeleted = Convert.ToInt32(row["IsDeleted"]) == 0 ? true : false;
                model.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                model.CreatedByEntity = Convert.ToInt32(row["CreatedByEntity"]);
                model.ModifiedBy = Convert.ToInt32(row["ModifiedBy"]);
                model.ModifiedByEntity = Convert.ToInt32(row["ModifiedByEntity"]);
                model.ModifiedOn.DateTimeInLong = Convert.ToInt64(row["ModifiedOn"]);
                //model.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                model.ParentID = Convert.ToInt32(row["ParentID"]);
            }
        }

        #endregion

    }
}
