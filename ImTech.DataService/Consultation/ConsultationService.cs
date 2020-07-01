using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ImTech.DataBase;
using ImTech.DataServices;
using ImTech.Model;

namespace ImTech.DataServices
{
    public class ConsultationService
    {
        IDataBaseService dataBaseService;

        public ConsultationService(IDataBaseService databaseService)
        {
            dataBaseService = databaseService;
        }

        #region Patient Details

        public ConsultationModel GetConsultationDetails(Int64 consultationId, int tanentId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@ConsultationID", consultationId));
            param.Add(new Parameter("@TenantID", tanentId));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetConsultationDetails, DBCommandType.Procedure, param.ToArray());
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var consultation = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapConsultation(r, tanentId)).FirstOrDefault();
                if (consultation.Id > 0)
                {
                    consultation.Prescriptions = GetConsultationPrescriptions(consultationId, tanentId);
                    consultation.CaseNotes = GetConsultationCaseNotes(consultationId, tanentId);
                }
                return consultation;
            }
            return null;
        }

        public IEnumerable<PrescreptionModel> GetConsultationPrescriptions(long consultationId, int tanentId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@ConsultationID", consultationId));
            param.Add(new Parameter("@TenantID", tanentId));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetConsultationPrescriptions, DBCommandType.Procedure, param.ToArray());
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var result = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapConsultationPrescription(r));
                return result;
            }
            return null;
        }

        private PrescreptionModel MapConsultationPrescription(DataRow row)
        {
            if (row != null)
            {
                PrescreptionModel psModel = new PrescreptionModel();
                psModel.ConsultationId = Convert.ToInt32(row["ConsultationID"]);
                psModel.PrescreptionText = row["PrescriptionText"] != null ? Convert.ToString(row["PrescriptionText"]) : string.Empty;
                psModel.TenantID = row["TenantID"] != null ? Convert.ToInt32(row["TenantID"]) : 0;
                psModel.Type = row["Type"] != null ? Convert.ToInt32(row["Type"]) : 0;
                psModel.IsDeleted = Convert.ToInt32(row["IsDeleted"]) == 0 ? true : false;
                psModel.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                psModel.CreatedByEntity = Convert.ToInt32(row["CreatedByEntity"]);
                psModel.CreatedOn = row["CreatedOn"] != null ? new InternalDateTime(Convert.ToInt64(row["CreatedOn"])) : new InternalDateTime();
                psModel.ModifiedBy = row["ModifiedBy"] != null ? Convert.ToInt32(row["ModifiedBy"]) : 0;
                psModel.ModifiedByEntity = row["ModifiedByEntity"] != null ? Convert.ToInt32(row["ModifiedByEntity"]) : 0;
                psModel.ModifiedOn = row["ModifiedOn"] != null ? new InternalDateTime(Convert.ToInt64(row["ModifiedOn"])) : new InternalDateTime();
                return psModel;
            }
            return null;
        }

        public IEnumerable<CaseNoteModel> GetConsultationCaseNotes(long consultationId, int tanentId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@ConsultationID", consultationId));
            param.Add(new Parameter("@TenantID", tanentId));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetConsultationNotes, DBCommandType.Procedure, param.ToArray());
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var result = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapConsultationNote(r));
                return result;
            }
            return null;
        }

        private CaseNoteModel MapConsultationNote(DataRow row)
        {
            if (row != null)
            {
                CaseNoteModel cModel = new CaseNoteModel();
                cModel.Id = Convert.ToInt32(row["NoteID"]);
                cModel.ConsultationId = Convert.ToInt32(row["ConsultationID"]);
                cModel.NoteText = row["NoteText"] != null ? Convert.ToString(row["NoteText"]) : string.Empty;
                cModel.TenantID = row["TenantID"] != null ? Convert.ToInt32(row["TenantID"]) : 0;
                cModel.IsDeleted = Convert.ToInt32(row["IsDeleted"]) == 0 ? true : false;
                cModel.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                cModel.CreatedByEntity = Convert.ToInt32(row["CreatedByEntity"]);
                cModel.CreatedOn = row["CreatedOn"] != null ? new InternalDateTime(Convert.ToInt64(row["CreatedOn"])) : new InternalDateTime();
                cModel.ModifiedBy = row["ModifiedBy"] != null ? Convert.ToInt32(row["ModifiedBy"]) : 0;
                cModel.ModifiedByEntity = row["ModifiedByEntity"] != null ? Convert.ToInt32(row["ModifiedByEntity"]) : 0;
                cModel.ModifiedOn = row["ModifiedOn"] != null ? new InternalDateTime(Convert.ToInt64(row["ModifiedOn"])) : new InternalDateTime();
                cModel.Type = row["Type"] != null ? Convert.ToInt32(row["Type"]) : 0;
                return cModel;
            }
            return null;
        }

        public PatientDetail SavePatientDetails(PatientDetail patientDetail)
        {
            List<Parameter> param = GetPatientParams(patientDetail);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);

            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertPatient, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt32(r);
            if (Id > 0)
            {
                patientDetail.Success = true;
                patientDetail.Id = Id;
            }
            else
            {
                patientDetail.Success = false;
                patientDetail.ErrorMessage = "Error in insert or update patient details";
            }
            return patientDetail;
        }

        public ConsultationModel SaveConsultationDetails(ConsultationModel consultationModel)
        {
            List<Parameter> param = GetConsultationParams(consultationModel);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);

            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertConsultation, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt32(r);
            if (Id > 0)
            {
                consultationModel.Success = true;
                consultationModel.Id = Id;
            }
            else
            {
                consultationModel.Success = false;
                consultationModel.ErrorMessage = "Error in insert or update consultation details";
            }
            return consultationModel;
        }

        public ConsultationModel UpdateConsultationStatus(ConsultationModel consultationModel)
        {
            List<Parameter> param = GetConsultationParamsForStatusUpdate(consultationModel);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);

            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.UpdateConsultationStatus, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt32(r);
            if (Id > 0)
            {
                consultationModel.Success = true;
                consultationModel.Id = Id;
            }
            else
            {
                consultationModel.Success = false;
                consultationModel.ErrorMessage = "Error in insert or update consultation details";
            }
            return consultationModel;
        }

        public ConsultationModel UpdateConsultationTime(ConsultationModel consultationModel)
        {
            List<Parameter> param = GetConsultationParamsForTimeUpdate(consultationModel);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);

            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.UpdateConsultationTime, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt32(r);
            if (Id > 0)
            {
                consultationModel.Success = true;
                consultationModel.Id = Id;
            }
            else
            {
                consultationModel.Success = false;
                consultationModel.ErrorMessage = "Error in insert or update consultation time";
            }
            return consultationModel;
        }

        public PaymentModel SavePaymentDetails(PaymentModel paymentModel)
        {
            List<Parameter> param = GetPaymentParams(paymentModel);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);

            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertPaymentDetails, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt32(r);
            if (Id > 0)
            {
                paymentModel.Success = true;
                paymentModel.Id = Id;
            }
            else
            {
                paymentModel.Success = false;
                paymentModel.ErrorMessage = "Error in insert or update payment details";
            }
            return paymentModel;
        }

        public PrescreptionModel SavePrescreption(PrescreptionModel prescreptionModel)
        {
            List<Parameter> param = GetPrescreptionParams(prescreptionModel);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);

            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertPrescreptionDetails, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt32(r);
            if (Id > 0)
            {
                prescreptionModel.Success = true;
                prescreptionModel.Id = Id;
            }
            else
            {
                prescreptionModel.Success = false;
                prescreptionModel.ErrorMessage = "Error inserting prescreption";
            }
            return prescreptionModel;
        }

        public CaseNoteModel SaveCaseNote(CaseNoteModel caseNoteModel)
        {
            List<Parameter> param = GetCaseNoteParams(caseNoteModel);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);

            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertCaseNote, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt32(r);
            if (Id > 0)
            {
                caseNoteModel.Success = true;
                caseNoteModel.Id = Id;
            }
            else
            {
                caseNoteModel.Success = false;
                caseNoteModel.ErrorMessage = "Error inserting case note";
            }
            return caseNoteModel;
        }
        #endregion

        #region Message
        public IEnumerable<MessageModel> GetMessages(long consultationId, int tanentId)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@ConsultationID", consultationId));
            param.Add(new Parameter("@TenantID", tanentId));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetConsultationMessages, DBCommandType.Procedure, param.ToArray());
            List<MessageModel> lst = null;
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                var lstMessages = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => Map(r));
            }
            return lst;
        }

        private MessageModel Map(DataRow row)
        {
            return new MessageModel()
            {
                Id = Convert.ToInt32(row["MessageID"]),
                TextMessage = row["MessageText"] != null ? Convert.ToString(row["MessageText"]) : string.Empty,
                PatientId = row["PatientID"] != null ? Convert.ToInt32(row["PatientID"]) : 0,
                DoctorId = row["DoctorID"] != null ? Convert.ToInt32(row["DoctorID"]) : 0,
                TenantID = Convert.ToInt32(row["TenantID"]),
                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                CreatedOn = new InternalDateTime(Convert.ToInt64(row["CreatedOn"])),
                CreatedByEntity = Convert.ToInt32(row["CreatedByEntity"]),
                ModifiedBy = Convert.ToInt32(row["ModifiedBy"]),
                ModifiedOn = new InternalDateTime(Convert.ToInt64(row["ModifiedON"])),
                ModifiedByEntity = Convert.ToInt32(row["ModifiedByEntity"]),
            };
        }

        #endregion

        #region Private Map Methods
        private ConsultationModel MapConsultation(DataRow row, int tanentId)
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
                        //Id = consultationModel.PatientId,(base Model ID shouls be long currently all IDs Type are not in sync)
                        PatientName = row["PatientName"] != null ? Convert.ToString(row["PatientName"]) : string.Empty,
                        PatientGender = row["PatientGender"] != null ? Convert.ToChar(row["PatientGender"]) : ' ',
                        Complaints = row["Complaints"] != null ? Convert.ToString(row["Complaints"]) : string.Empty,
                        ConsultationType = row["ConsultationType"] != null ? Convert.ToInt32(row["ConsultationType"]) : 0,
                        PatientPhone = row["PatientPhone"] != null ? Convert.ToString(row["PatientPhone"]) : string.Empty,
                        PatientAge = row["PatientAge"] != null ? Convert.ToInt16(row["PatientAge"]) : (short)0,
                        Files = row["PatientFileIds"] != null ? Convert.ToString(row["PatientFileIds"]) : string.Empty,
                        Case = row["Case"] != null ? Convert.ToInt16(row["Case"]) : (short)0,
                        PreferredTime = row["PatientPreferredTime"] != null ? Convert.ToInt16(row["PatientPreferredTime"]) : (short)0
                    };
                }
            }
            return consultationModel;
        }
        #endregion

        #region Private Methods
        private List<Parameter> GetPatientParams(PatientDetail model)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@UserID", model.UserID));
            param.Add(new Parameter("@DoctorID", model.DoctorId));
            param.Add(new Parameter("@Name", model.PatientName));
            param.Add(new Parameter("@Age", model.PatientAge));
            param.Add(new Parameter("@Phone", model.PatientPhone));
            param.Add(new Parameter("@Gender", model.PatientGender));
            param.Add(new Parameter("@TenantID", model.TenantID));
            param.Add(new Parameter("@CreatedUserID", model.CreatedBy));
            param.Add(new Parameter("@CreatedByEntity", model.CreatedByEntity));
            param.Add(new Parameter("@Complaints", model.Complaints));
            param.Add(new Parameter("@ConsultationType", model.ConsultationType));
            param.Add(new Parameter("@FileId", model.FileId));
            param.Add(new Parameter("@Case", model.Case));
            param.Add(new Parameter("@PreferredTime", model.PreferredTime));
            return param;
        }

        private List<Parameter> GetConsultationParams(ConsultationModel model)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Description", model.Description));
            param.Add(new Parameter("@PreferredTime", model.ConsultationTime.DateTimeInLong));
            param.Add(new Parameter("@PatientID", model.PatientId));
            param.Add(new Parameter("@DoctorID", model.DoctorId));
            param.Add(new Parameter("@ConsultationModeID", model.ConsultationModeID));
            param.Add(new Parameter("@ConsultationStatusID", model.ConsultationStatusID));
            param.Add(new Parameter("@TenantID", model.TenantID));
            param.Add(new Parameter("@CreatedUserID", model.CreatedBy));
            param.Add(new Parameter("@CreatedByEntity", model.CreatedByEntity));
            return param;
        }

        private List<Parameter> GetConsultationParamsForTimeUpdate(ConsultationModel model)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@ConsultationID", model.Id));
            param.Add(new Parameter("@PreferredTime", model.ConsultationTime.DateTimeInLong));
            param.Add(new Parameter("@ConsultationStatusID", model.ConsultationStatusID));
            param.Add(new Parameter("@TenantID", model.TenantID));
            param.Add(new Parameter("@CreatedUserID", model.CreatedBy));
            param.Add(new Parameter("@CreatedByEntity", model.CreatedByEntity));
            return param;
        }

        private List<Parameter> GetConsultationParamsForStatusUpdate(ConsultationModel model)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@ConsultationID", model.Id));
            param.Add(new Parameter("@ConsultationStatusID", model.ConsultationStatusID));
            param.Add(new Parameter("@TenantID", model.TenantID));
            param.Add(new Parameter("@CreatedUserID", model.CreatedBy));
            param.Add(new Parameter("@CreatedByEntity", model.CreatedByEntity));
            return param;
        }

        private List<Parameter> GetPaymentParams(PaymentModel model)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@PaymentId", model.Id));
            param.Add(new Parameter("@ConsultationID", model.ConsultationID));
            param.Add(new Parameter("@PaymentStatusID", model.PaymentStatusID));
            param.Add(new Parameter("@PromotionalID", model.PromotionalID));
            param.Add(new Parameter("@GatewayResponse", model.GatewayResponse));
            param.Add(new Parameter("@AmountCharged", model.AmountCharged));
            param.Add(new Parameter("@AmountActual", model.AmountActual));
            param.Add(new Parameter("@DiscountAmount", model.DiscountAmount));
            param.Add(new Parameter("@TenantID", model.TenantID));
            param.Add(new Parameter("@CreatedUserID", model.CreatedBy));
            param.Add(new Parameter("@CreatedByEntity", model.CreatedByEntity));
            return param;
        }

        private List<Parameter> GetPrescreptionParams(PrescreptionModel model)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@PrescriptionText", model.PrescreptionText));
            param.Add(new Parameter("@Type", model.Type));
            param.Add(new Parameter("@ConsultationID", model.ConsultationId));
            param.Add(new Parameter("@TenantID", model.TenantID));
            param.Add(new Parameter("@CreatedUserID", model.CreatedBy));
            param.Add(new Parameter("@CreatedByEntity", model.CreatedByEntity));
            return param;
        }

        private List<Parameter> GetCaseNoteParams(CaseNoteModel model)
        {
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@NoteText", model.NoteText));
            param.Add(new Parameter("@Type", model.Type));
            param.Add(new Parameter("@ConsultationID", model.ConsultationId));
            param.Add(new Parameter("@TenantID", model.TenantID));
            param.Add(new Parameter("@CreatedUserID", model.CreatedBy));
            param.Add(new Parameter("@CreatedByEntity", model.CreatedByEntity));
            return param;
        }


        #endregion
    }
}
