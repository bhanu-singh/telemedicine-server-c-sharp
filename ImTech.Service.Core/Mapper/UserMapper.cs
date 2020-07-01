using ImTech.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImTech.Model;
using am = AutoMapper;

namespace ImTech.Service.Mapper
{

    public class UserMapper
    {
        private ViewModelFactory _viewModelFactory;
        public UserMapper(ViewModelFactory viewModelFactory)
        {
            this._viewModelFactory = viewModelFactory;

        }

        public UserViewModel Map(UserModel userModel)
        {
            //var vm = am.Mapper.Map<UserViewModel>(userModel);
            //return vm;
            return new UserViewModel()
            {
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                IsPhoneNoVerified = userModel.IsPhoneNoVerified,
                PhoneNumber = userModel.PhoneNumber,
                Email = userModel.Email,
                CreatedBy = userModel.CreatedBy,
                CreatedByEntity = userModel.CreatedByEntity,
                CreatedOn = userModel.CreatedOn,
                ModifiedBy = userModel.ModifiedBy,
                ModifiedOn = userModel.ModifiedOn,
                ModifiedEntity = userModel.ModifiedByEntity,
                IsDeleted = userModel.IsDeleted,
                TanentId = userModel.TenantID,
                DeviceId = userModel.DeviceId
            };
        }

        public IEnumerable<UserViewModel> Map(IEnumerable<UserModel> userModel)
        {
            return userModel.Select(Map);

        }

        public UserModel Map(UserViewModel userViewModel)
        {
            return new UserModel
            {
                Id = userViewModel.Id,
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                Email = userViewModel.Email,
                PhoneNumber = userViewModel.PhoneNumber,
                IsPhoneNoVerified = userViewModel.IsPhoneNoVerified,
                TenantID = userViewModel.TanentId,
                CreatedBy = userViewModel.CreatedBy,
                CreatedOn = userViewModel.CreatedOn,
                CreatedByEntity = userViewModel.CreatedByEntity,
                IsDeleted = userViewModel.IsDeleted,
                ModifiedBy = userViewModel.ModifiedBy,
                ModifiedOn = userViewModel.ModifiedOn,
                ModifiedByEntity = userViewModel.ModifiedEntity,
                Password = userViewModel.Password,
                DeviceId = userViewModel.DeviceId
            };
            //return am.Mapper.Map<UserModel>(userViewModel);
        }

        public UserLogOnModel Map(UserLogonViewModel logOnViewModel)
        {
            return new UserLogOnModel()
            {
                Email = logOnViewModel.Email,
                Password = logOnViewModel.Password,
                PhoneNumber = logOnViewModel.PhoneNumber,
                LoginLocation = logOnViewModel.LoginLocation,
                RequestURL = logOnViewModel.RequestURL,
                TenantID = logOnViewModel.TanentId,
                CreatedByEntity = logOnViewModel.CreatedByEntity,
                DeviceId = logOnViewModel.DeviceId
            };
        }

        public IEnumerable<UserModel> Map(IEnumerable<UserViewModel> userModels)
        {
            return userModels.Select(Map);
        }

        public IEnumerable<ConsultationViewModel> MapMyConsultations(IEnumerable<ConsultationModel> consultations)
        {
            return consultations.Select(MapMyConsultations);
        }

        public ConsultationViewModel MapMyConsultations(ConsultationModel model)
        {
            return new ConsultationViewModel
            {
                Id = model.Id,
                Description = model.Description,
                ConsultationTime = model.ConsultationTime,
                PatientId = model.PatientId,
                DoctorId = model.DoctorId,
                ConsultationModeID = model.ConsultationModeID,
                ConsultationMode = model.ConsultationMode,
                ConsultationStatusID = model.ConsultationStatusID,
                ConsultationStatus = model.ConsultationStatus,
                CreatedBy = model.CreatedBy,
                CreatedByEntity = model.CreatedByEntity,
                CreatedOn = model.CreatedOn,
                ModifiedBy = model.ModifiedBy,
                ModifiedEntity = model.ModifiedByEntity,
                ModifiedOn = model.ModifiedOn,
                TanentId = model.TenantID
            };
        }


        public IEnumerable<ConsultationViewListModel> MapUserListViewConsultation(IEnumerable<ConsultationModel> consultations)
        {
            return consultations.Select(MapUserListViewConsultation);
        }

        public ConsultationViewListModel MapUserListViewConsultation(ConsultationModel model)
        {
            return new ConsultationViewListModel
            {
                Id = model.Id,
                ConsultationMode = model.ConsultationMode,
                ConsultationStatus = model.ConsultationStatus,
                ConsultationTime = model.ConsultationTime,
                Description = model.Description,
                CreatedBy = model.CreatedBy,
                CreatedByEntity = model.CreatedByEntity,
                ModifiedBy = model.ModifiedBy,
                ModifiedEntity = model.ModifiedByEntity,
                TanentId = model.TenantID,
                PrescriptionList = model.PrescriptionList != null ? model.PrescriptionList.Split(';').ToList<string>() : new List<string>(),
                CaseNoteList = model.CaseNotesList != null ? model.CaseNotesList.Split(';').ToList<string>() : new List<string>(),
                PatientName = model.Patient != null ? model.Patient.PatientName : string.Empty,
                DoctorFirstName = model.Doctor != null ? model.Doctor.FirstName : string.Empty,
                DoctorLastName = model.Doctor != null ? model.Doctor.LastName : string.Empty
            };
        }


        public IEnumerable<ConsultationViewModel> MapUserConsultation(IEnumerable<ConsultationModel> consultations)
        {
            return consultations.Select(MapUserConsultation);
        }

        public ConsultationViewModel MapUserConsultation(ConsultationModel model)
        {
            return new ConsultationViewModel
            {
                Id = model.Id,
                ConsultationMode = model.ConsultationMode,
                ConsultationModeID = model.ConsultationModeID,
                ConsultationStatus = model.ConsultationStatus,
                ConsultationStatusID = model.ConsultationStatusID,
                ConsultationTime = model.ConsultationTime,
                Description = model.Description,
                CreatedBy = model.CreatedBy,
                CreatedByEntity = model.CreatedByEntity,
                ModifiedBy = model.ModifiedBy,
                ModifiedEntity = model.ModifiedByEntity,
                TanentId = model.TenantID,
                PrescriptionList = model.PrescriptionList != null ? model.PrescriptionList.Split(';').ToList<string>() : new List<string>(),
                CaseNoteList = model.CaseNotesList != null ? model.CaseNotesList.Split(';').ToList<string>() : new List<string>(),
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
                AmountCharged = model.AmountCharged,
                Patient = new PatientDetailViewModel()
                {
                    Id = model.Patient.Id,
                    PatientName = model.Patient.PatientName,
                    PatientGender = model.Patient.PatientGender,
                    PatientAge = model.Patient.PatientAge,
                    PatientPhone = model.Patient.PatientPhone,
                    Complaints = model.Patient.Complaints,
                    UserID = model.Patient.UserID,
                    CreatedBy = model.Patient.CreatedBy,
                    CreatedByEntity = model.Patient.CreatedByEntity,
                    ModifiedBy = model.Patient.ModifiedBy,
                    ModifiedEntity = model.Patient.ModifiedByEntity,
                    IsDeleted = model.Patient.IsDeleted,
                    TanentId = model.Patient.TenantID,
                    PreferredTime = model.Patient.PreferredTime,
                    Case = model.Patient.Case,
                    Files = model.Patient.Files != null ? model.Patient.Files.Split(',').ToList<string>() : new List<string>()
                },
                Doctor = new DoctorViewModel()
                {
                    FirstName = model.Doctor.FirstName,
                    LastName = model.Doctor.LastName,
                    Address1 = model.Doctor.Address1,
                    CityID = model.Doctor.CityID,
                    PhoneNumber = model.Doctor.PhoneNumber,
                    DoctorCode = model.Doctor.DoctorCode,
                    DoctorDescription = model.Doctor.DoctorDescription,
                    DoctorDegree = model.Doctor.DoctorDegree.ToList(),
                    DoctorDesease = model.Doctor.DoctorDesease.ToList(),
                    DoctorHospital = model.Doctor.DoctorHospital.ToList(),
                    DoctorSpecialization = model.Doctor.DoctorSpecialization.ToList(),
                    EmailAddress = model.Doctor.EmailAddress,
                    OtherInformation = model.Doctor.OtherInformation,
                    Id = model.Doctor.Id,
                    IsDeleted = model.Doctor.IsDeleted,
                    CreatedBy = model.Doctor.CreatedBy,
                    CreatedByEntity = model.Doctor.CreatedByEntity,
                    CreatedOn = model.Doctor.CreatedOn,
                    ModifiedBy = model.Doctor.ModifiedBy,
                    ModifiedEntity = model.Doctor.ModifiedByEntity,
                    ModifiedOn = model.Doctor.ModifiedOn,
                    Pincode = model.Doctor.Pincode,
                    ProfilePhotoID = model.Doctor.ProfilePhotoID,
                    RegistrationNumber = model.Doctor.RegistrationNumber,
                    TanentId = model.Doctor.TenantID,
                    PersonalConsultationFee = model.Doctor.PersonalConsultationFee,
                    TextConsultationFee = model.Doctor.TextConsultationFee,
                    VideoConsultationFee = model.Doctor.VideoConsultationFee,
                    PhoneConsultationFee = model.Doctor.PhoneConsultationFee
                }
            };
        }


        public MessageModel MapUserMessage(MessageViewModel viewModel)
        {
            return new MessageModel
            {
                Id = viewModel.Id,
                ConsultationId = viewModel.ConsultationId,
                TextMessage = viewModel.TextMessage,
                CreatedByEntity = viewModel.CreatedByEntity,
                CreatedOn = viewModel.CreatedOn,
                CreatedBy = viewModel.CreatedBy,
                ModifiedByEntity = viewModel.ModifiedEntity,
                ModifiedOn = viewModel.ModifiedOn,
                ModifiedBy = viewModel.ModifiedBy
            };
        }


        public UserViewModel CreateViewModel()
        {
            return this._viewModelFactory.CreateUserViewModel();
        }
        public ChangePasswordViewModel MapChangePassword(ChangePasswordModel model)
        {
            return new ChangePasswordViewModel
            {
                OldPassword = model.OldPassword,
                NewPassword = model.NewPassword,
                Email = model.Email,
                CreatedBy = model.CreatedBy,
                CreatedByEntity = model.CreatedByEntity,
                CreatedOn = model.CreatedOn,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                ModifiedEntity = model.ModifiedByEntity,
                TanentId = model.TenantID
            };
        }
        public ChangePasswordModel MapChangePassword(ChangePasswordViewModel model)
        {
            return new ChangePasswordModel
            {
                OldPassword = model.OldPassword,
                NewPassword = model.NewPassword,
                NewHashPassword = model.NewHashPassword,
                Email = model.Email,
                CreatedBy = model.CreatedBy,
                CreatedByEntity = model.CreatedByEntity,
                CreatedOn = model.CreatedOn,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                ModifiedByEntity = model.ModifiedEntity,
                TenantID = model.TanentId
            };
        }


        public string ValidateUser(UserModel userModel)
        {
            return "";
        }
        public string ValidateUserRegistration(UserViewModel userViewModel)
        {
            return "";
        }
        public string ValidateUserLogOn(UserLogonViewModel userLogOnViewModel)
        {
            return "";
        }

        public string ValidateUserChangePasswordOn(ChangePasswordViewModel changePasswordViewModel)
        {
            return "";
        }

    }
}