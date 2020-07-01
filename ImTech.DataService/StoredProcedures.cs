using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.DataServices
{
    public class StoredProcedures
    {
        #region Common

        public const string GetCountryOrCountryList = "stp_GetCountries";
        public const string GetStateOrStateList = "stp_GetStates";
        public const string GetCityOrCityList = "stp_GetCities";
        public const string InsertLog = "stp_InsertLog";

        public const string GetDegree = "";
        public const string GetDesease = "";
        public const string GetSpecialization = "";
        public const string GetHospital = "";
        #endregion

        #region User
        public const string ValidateUserLogin = "stp_ValidateUserLogin";

        public const string InsertUpdateUser = "stp_InsertUpdateUser";

        public const string RegisterUser = "stp_RegisterUser";

        public const string ValidateLoginDetail = "stp_ValidateUserLoginDetail";

        public const string UpdateUserDeviceId = "stp_UpdateUserDeviceId";

        public const string ChangeUserPassword = "stp_ChangeUserPassword_new";

        public const string ChangeDoctorPassword = "stp_ChangeDoctorPassword_new";

        public const string ChangeDoctorPasswordById = "stp_ChangeDoctorPasswordById";

        public const string ValidateUserEmail = "stp_ValidateUserEmail";

        public const string ValidateDoctorEmail = "stp_ValidateDoctorEmail";

        public const string InsertPatient = "stp_InsertPatientDetail";

        public const string CheckSecureFileAccess = "stp_CheckSecureFileAccess";

        public const string InsertConsultation = "stp_InsertConsultationDetail";

        public const string InsertPaymentDetails = "stp_InsertPaymentDetail";

        public const string InsertPrescreptionDetails = "stp_InsertPrescriptions";

        public const string UpdateConsultationStatus = "stp_UpdateConsultationStatus";

        public const string UpdateConsultationTime = "stp_UpdateConsultationTime";

        public const string GetConsultationMessages = "stp_GetConsultationMessages";

        public const string InsertCaseNote = "stp_InsertCaseNotes";

        public const string GetUserOrUserList = "stp_GetUsers";

        public const string GetUsersByPhoneNo = "stp_GetUsersByPhoneNo";

        public const string GetUserList = "";

        public const string SaveOtp = "stp_InsertOTPDetails";

        public const string stp_ValidateOTP = "stp_ValidateOTP";
        #endregion

        #region Doctor
        public const string ValidateDoctorLoginDetail = "stp_ValidateDoctorLoginDetail";
        public const string UpdateDoctorDeviceId = "stp_UpdateDoctorDeviceId";
        public const string UpdateDoctorAvailability = "stp_UpdateDoctorAvailability";
        public const string GetDoctorOrDoctorList = "stp_GetDoctors";
        public const string GetDoctorOrDoctorListByCode = "stp_GetDoctorsByCode";
        public const string GetDoctorDegrees = "stp_GetDoctorDegrees";
        public const string InsertUpdateDegree = "stp_InsertUpdateDegree";
        public const string GetDoctorSpecializations = "stp_GetDoctorSpecializations";
        public const string InsertUpdateSpecialization = "stp_InsertUpdateSpecialization";
        public const string GetDoctorHospitals = "stp_GetDoctorHospitals";
        public const string InsertUpdateHospital = "stp_InsertUpdateHospital";
        public const string GetDoctorDeseases = "stp_DoctorDeseases";
        public const string InsertUpdateDesease = "stp_InsertUpdateDeseases";
        public const string GetDoctorStats = "stp_GetDoctorStats";

        public const string UpdateUserDoctorMapping = "stp_UpdateUserDoctorMapping";
        public const string GetMappedDoctorForUser = "stp_GetMappedDoctorForUser";

        public const string InsertUpdateDoctor = "stp_InsertUpdateDoctor";
        public const string UpdateDoctor = "";
        public const string DeleteDoctor = "";

        public const string GetDegreeOrDegreeList = "stp_GetDegrees";
        public const string GetDeseaseOrDeseaseList = "stp_GetDesease";
        public const string GetHospitalOrHospitalList = "stp_GetHospital";
        public const string GetSpecializationOrSpecializationList = "stp_GetSpecialization";

        public const string GetConsultationList = "stp_GetConsultations";
        public const string GetMyConsultations = "stp_GetMyConsultations";
        public const string GetConsultationDetails = "stp_GetConsultationDetails";
        public const string GetConsultationPrescriptions = "stp_GetConsultationPrescriptions";
        public const string GetConsultationNotes = "stp_GetConsultationNotes";
        #endregion

        #region File

        public const string InsertUpdateDeleteSecureFile = "stp_InsertUpdateDeleteSecureFile";
        public const string GetSecureFileDetail = "stp_GetSecureFileDetail";

        public const string InsertUpdateDeleteNonSecureFile = "stp_InsertUpdateDeleteNonSecureFile";
        public const string GetNonSecureFileDetail = "stp_GetNonSecureFileDetail";

        #endregion

        #region Message
        public const string InsertUpdateMessage = "stp_InsertUpdateMessage";
        #endregion
    }
}

