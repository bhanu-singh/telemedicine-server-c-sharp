using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ImTech.Model;
using am = AutoMapper;
using ImTech.Service.ViewModels;
using ImTech.Service.ViewModels.Doctor;
using ImTech.Service.ViewModels.Other;
using ImTech.Model.Doctor;

namespace ImTech.Service.Mapper
{
    public class DoctorMapper
    {
        private ViewModelFactory _viewModelFactory;
        public DoctorMapper(ViewModelFactory viewModelFactory)
        {
            this._viewModelFactory = viewModelFactory;

        }

        #region Mapping Methods

        public DoctorViewModel Map(DoctorModel doctorModel)
        {
            //var vm = am.Mapper.Map<DoctorViewModel>(doctorModel);
            //return vm;
            DoctorViewModel doctorViewModel = _viewModelFactory.CreateDoctorViewModel();
            doctorViewModel.Id = doctorModel.Id;
            doctorViewModel.Password = doctorModel.Password;
            doctorViewModel.CityID = doctorModel.CityID;
            doctorViewModel.EmailAddress = doctorModel.EmailAddress;
            doctorViewModel.Address1 = doctorModel.Address1;
            doctorViewModel.CreatedBy = doctorModel.CreatedBy;
            doctorViewModel.CreatedByEntity = doctorModel.CreatedByEntity;
            doctorViewModel.CreatedOn = doctorModel.CreatedOn;
            doctorViewModel.FirstName = doctorModel.FirstName;
            doctorViewModel.LastName = doctorModel.LastName;
            doctorViewModel.OtherInformation = doctorModel.OtherInformation;
            doctorViewModel.ModifiedBy = doctorModel.ModifiedBy;
            doctorViewModel.ModifiedEntity = doctorModel.ModifiedByEntity;
            doctorViewModel.ModifiedOn = doctorModel.ModifiedOn;
            doctorViewModel.IsDeleted = doctorModel.IsDeleted;
            doctorViewModel.PhoneNumber = doctorModel.PhoneNumber;
            doctorViewModel.Pincode = doctorModel.Pincode;
            doctorViewModel.TanentId = doctorModel.TenantID;
            doctorViewModel.ProfilePhotoID = doctorModel.ProfilePhotoID <= 0 ? -1 : doctorModel.ProfilePhotoID;
            doctorViewModel.RegistrationNumber = doctorModel.RegistrationNumber;
            doctorViewModel.PersonalConsultationFee = doctorModel.PhoneConsultationFee;
            doctorViewModel.PhoneConsultationFee = doctorModel.PhoneConsultationFee;
            doctorViewModel.TextConsultationFee = doctorModel.TextConsultationFee;
            doctorViewModel.VideoConsultationFee = doctorModel.VideoConsultationFee;
            doctorViewModel.DoctorDescription = doctorModel.DoctorDescription;
            doctorViewModel.DoctorCode = doctorModel.DoctorCode;
            doctorViewModel.DoctorDegree = doctorModel.DoctorDegree != null ? doctorModel.DoctorDegree.ToList() : new List<int>();
            doctorViewModel.DoctorSpecialization = doctorModel.DoctorSpecialization != null ? doctorModel.DoctorSpecialization.ToList() : new List<int>();
            doctorViewModel.DoctorDesease = doctorModel.DoctorDesease != null ? doctorModel.DoctorDesease.ToList() : new List<int>();
            doctorViewModel.DoctorHospital = doctorModel.DoctorHospital != null ? doctorModel.DoctorHospital.ToList() : new List<int>();
            return doctorViewModel;
        }

        public IEnumerable<DoctorViewModel> Map(IEnumerable<DoctorModel> doctorViewModels)
        {
            return doctorViewModels.Select(Map);
        }

        public IEnumerable<DoctorModel> Map(IEnumerable<DoctorViewModel> doctorModels)
        {
            return doctorModels.Select(Map);
        }

        public DoctorModel Map(DoctorViewModel dvModel)
        {
            return new DoctorModel()
            {
                Address1 = dvModel.Address1,
                CityID = dvModel.CityID,
                CreatedBy = dvModel.CreatedBy,
                CreatedByEntity = dvModel.CreatedByEntity,
                CreatedOn = dvModel.CreatedOn,
                DoctorDegree = dvModel.DoctorDegree,
                DoctorSpecialization = dvModel.DoctorSpecialization,
                DoctorDesease = dvModel.DoctorDesease,
                DoctorHospital = dvModel.DoctorHospital,
                EmailAddress = dvModel.EmailAddress,
                FirstName = dvModel.FirstName,
                Id = dvModel.Id,
                Password = dvModel.Password,
                LastName = dvModel.LastName,
                ModifiedBy = dvModel.CreatedBy,
                ModifiedByEntity = dvModel.CreatedByEntity,
                ModifiedOn = dvModel.CreatedOn,
                OtherInformation = dvModel.OtherInformation,
                PhoneNumber = dvModel.PhoneNumber,
                Pincode = dvModel.Pincode,
                ProfilePhotoID = dvModel.ProfilePhotoID,
                RegistrationNumber = dvModel.RegistrationNumber,
                TenantID = dvModel.TanentId,
                PersonalConsultationFee = dvModel.PersonalConsultationFee,
                PhoneConsultationFee = dvModel.PhoneConsultationFee,
                TextConsultationFee = dvModel.TextConsultationFee,
                VideoConsultationFee = dvModel.VideoConsultationFee,
                DoctorDescription = dvModel.DoctorDescription,
                DoctorCode = dvModel.DoctorCode
            };
        }

        internal string ValidateHospital(HospitalViewModel hViewModel)
        {
            return string.Empty;
        }

        public IEnumerable<DoctorShortViewModel> MapDoctorShortModel(IEnumerable<DoctorModel> doctorModels)
        {
            return doctorModels.Select(MapDoctorShortModel);
        }

        public DoctorShortViewModel MapDoctorShortModel(DoctorModel dModel)
        {
            DoctorShortViewModel viewModel = new DoctorShortViewModel();
            viewModel.Id = dModel.Id;
            viewModel.FirstName = dModel.FirstName;
            viewModel.LastName = dModel.LastName;
            viewModel.Address1 = dModel.Address1;
            viewModel.EmailAddress = dModel.EmailAddress;
            viewModel.PhoneNumber = dModel.PhoneNumber;
            viewModel.Pincode = dModel.Pincode;
            viewModel.ProfilePhotoID = dModel.ProfilePhotoID <= 0 ? -1 : dModel.ProfilePhotoID;
            viewModel.RegistrationNumber = dModel.RegistrationNumber;
            viewModel.CityName = dModel.CityName;
            viewModel.StateName = dModel.StateName;
            viewModel.CountryName = dModel.CountryName;
            viewModel.OtherInformation = dModel.OtherInformation;
            viewModel.DoctorDescription = dModel.DoctorDescription;
            viewModel.DoctorCode = dModel.DoctorCode;
            viewModel.PersonalConsultationFee = dModel.PersonalConsultationFee;
            viewModel.PhoneConsultationFee = dModel.PhoneConsultationFee;
            viewModel.TextConsultationFee = dModel.TextConsultationFee;
            viewModel.VideoConsultationFee = dModel.VideoConsultationFee;
            viewModel.TanentId = dModel.TenantID;
            viewModel.DoctorDegrees = MapDoctorDegree(dModel.DoctorDegreeList);
            viewModel.DoctorSpecializations = MapDoctorSpecialization(dModel.DoctorSpecialzationList);
            viewModel.DoctorHospitals = MapDoctorHospital(dModel.DoctorHospitalList);
            viewModel.DoctorDeseases = MapDoctorDesease(dModel.DoctorDeseaseList);
            return viewModel;

        }

        public DeseaseModel MapDoctorDesease(DeseasesViewModel dModel)
        {
            return new DeseaseModel()
            {
                Id = dModel.Id,
                DeseaseName = dModel.DeseaseName,
                TenantID = dModel.TanentId
            };
        }

        public IEnumerable<DeseasesViewModel> MapDoctorDesease(IEnumerable<DeseaseModel> doctordeseasesList)
        {
            return doctordeseasesList.Select(MapDoctorDesease);
        }

        public DeseasesViewModel MapDoctorDesease(DeseaseModel dModel)
        {
            return new DeseasesViewModel()
            {
                Id = dModel.Id,
                DeseaseName = dModel.DeseaseName,
                TanentId = dModel.TenantID
            };
        }

        internal string ValidateDegree(DegreeViewModel dViewMOdel)
        {
            return string.Empty;
        }
        internal string ValidateSpecialization(SpecializationViewModel dViewMOdel)
        {
            return string.Empty;
        }
        internal string ValidateDesease(DeseasesViewModel dModel)
        {
            return string.Empty;
        }

        public IEnumerable<HospitalViewModel> MapDoctorHospital(IEnumerable<HospitalModel> doctorHospitalList)
        {
            return doctorHospitalList.Select(MapDoctorHospital);
        }

        public HospitalModel MapHospital(HospitalViewModel viewModel)
        {
            return new HospitalModel()
            {
                LongId = viewModel.LongId,
                HospitalName = viewModel.HospitalName,
                HospitalAddress1 = viewModel.HospitalAddress1,
                CityId = viewModel.CityId,
                StateId = viewModel.StateId,
                HospitalPhone1 = viewModel.HospitalPhone1,
                HospitalPhone2 = viewModel.HospitalPhone2,
                HospitalFax = viewModel.HospitalFax,
                HospitalEmail = viewModel.HospitalEmail,
                HospitalCode = viewModel.HospitalCode
            };
        }

        public HospitalViewModel MapDoctorHospital(HospitalModel dModel)
        {
            return new HospitalViewModel()
            {
                LongId = dModel.LongId,
                Id = dModel.Id,
                HospitalName = dModel.HospitalName,
                HospitalAddress1 = dModel.HospitalAddress1,
                CityId = dModel.CityId,
                StateId = dModel.StateId,
                HospitalCity = dModel.CityName,
                HospitalState = dModel.StateName,
                HospitalCountry = dModel.CountryName,
                HospitalPhone1 = dModel.HospitalPhone1,
                HospitalPhone2 = dModel.HospitalPhone2,
                HospitalFax = dModel.HospitalFax,
                HospitalEmail = dModel.HospitalEmail,
                HospitalCode = dModel.HospitalCode,
                TanentId = dModel.TenantID
            };
        }


        public HospitalViewModel MapHospitalDetail(HospitalModel model)
        {
            HospitalViewModel hospitalViewModel = _viewModelFactory.CreateHospitalViewModel();
            hospitalViewModel.LongId = model.LongId;
            hospitalViewModel.HospitalName = model.HospitalName;
            hospitalViewModel.HospitalAddress1 = model.HospitalAddress1;
            hospitalViewModel.CityId = model.CityId;
            hospitalViewModel.StateId = model.StateId;
            hospitalViewModel.HospitalCity = model.CityName;
            hospitalViewModel.HospitalState = model.StateName;
            hospitalViewModel.HospitalCountry = model.CountryName;
            hospitalViewModel.HospitalPhone1 = model.HospitalPhone1;
            hospitalViewModel.HospitalPhone2 = model.HospitalPhone2;
            hospitalViewModel.HospitalFax = model.HospitalFax;
            hospitalViewModel.HospitalEmail = model.HospitalEmail;
            hospitalViewModel.HospitalCode = model.HospitalCode;
            hospitalViewModel.TanentId = model.TenantID;
            return hospitalViewModel;

        }

        public IEnumerable<SpecializationViewModel> MapDoctorSpecialization(IEnumerable<SpecializationModel> doctorSpecialization)
        {
            return doctorSpecialization.Select(MapDoctorSpecialization);
        }

        public SpecializationViewModel MapDoctorSpecialization(SpecializationModel dModel)
        {
            return new SpecializationViewModel()
            {
                Id = dModel.Id,
                SpecializationName = dModel.SpecializationName,
                TanentId = dModel.TenantID
            };
        }

        public SpecializationModel MapDoctorSpecialization(SpecializationViewModel sModel)
        {
            return new SpecializationModel()
            {
                Id = sModel.Id,
                SpecializationName = sModel.SpecializationName,
                TenantID = sModel.TanentId
            };
        }

        public IEnumerable<DegreeViewModel> MapDoctorDegree(IEnumerable<DegreeModel> doctorDegreeList)
        {
            return doctorDegreeList.Select(MapDoctorDegree);
        }

        public DegreeViewModel MapDoctorDegree(DegreeModel dModel)
        {
            return new DegreeViewModel()
            {
                Id = dModel.Id,
                DegreeName = dModel.DegreeName,
                TanentId = dModel.TenantID
            };
        }

        public DegreeModel MapDoctorDegree(DegreeViewModel dModel)
        {
            return new DegreeModel()
            {
                Id = dModel.Id,
                DegreeName = dModel.DegreeName,
                TenantID = dModel.TanentId
            };
        }

        public ConsultationModel MapShortConsultationModel(ConsultationViewModel consultation)
        {
            return new ConsultationModel()
            {
                Id = consultation.Id,
                ConsultationStatusID = consultation.ConsultationStatusID,
                ConsultationTime = consultation.ConsultationTime,
                ModifiedBy = consultation.ModifiedBy,
                ModifiedByEntity = consultation.ModifiedEntity,
                TenantID = consultation.TanentId
            };
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
                TanentId = model.TenantID,
                AmountCharged = model.AmountCharged

            };
        }

        public IEnumerable<ConsultationViewModel> MapDoctorConsultation(IEnumerable<ConsultationModel> consultations)
        {
            return consultations.Select(MapDoctorConsultationModel);
        }

        public ConsultationViewModel MapDoctorConsultationModel(ConsultationModel model)
        {
            return new ConsultationViewModel
            {
                Id = model.Id,
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
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
                AmountCharged = model.AmountCharged,
                TanentId = model.TenantID,
                PrescriptionList = model.PrescriptionList != null ? model.PrescriptionList.Split(';').ToList<string>() : new List<string>(),
                CaseNoteList = model.CaseNotesList != null ? model.CaseNotesList.Split(';').ToList<string>() : new List<string>(),
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
                    Files = model.Patient.Files != null ? model.Patient.Files.Split(',').ToList<string>() : new List<string>(),
                    Case = model.Patient.Case,
                    PreferredTime = model.Patient.PreferredTime
                }
            };
        }

        public MessageModel MapDoctorMessage(MessageViewModel viewModel)
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

        public DoctorStatsViewModel Map(DoctorStatsModel model)
        {
            return new DoctorStatsViewModel()
            {
                CountTxtConsultationInitiatedOrInProgress = model.CountTxtConsultationInitiatedOrInProgress,
                CountCallConsultationInitiatedOrInProgress = model.CountCallConsultationInitiatedOrInProgress,
                CountVideoConsultationInitiatedOrInProgress = model.CountVideoConsultationInitiatedOrInProgress,
                CountTxtConsultationClosed = model.CountTxtConsultationClosed,
                CountCallConsultationClosed = model.CountCallConsultationClosed,
                CountVideoConsultationClosed = model.CountVideoConsultationClosed,
                CountTxtConsultationCancelled = model.CountTxtConsultationCancelled,
                CountCallConsultationCancelled = model.CountCallConsultationCancelled,
                CountVideoConsultationCancelled = model.CountVideoConsultationCancelled,
                LastMonthRevenue = model.LastMonthRevenue,
                TotalRevenue = model.TotalRevenue
            };
        }

        #endregion

        public DoctorViewModel CreateViewModel()
        {
            return this._viewModelFactory.CreateDoctorViewModel();
        }

        public DoctorLogOnModel Map(DoctorLogonViewModel logOnViewModel)
        {
            return new DoctorLogOnModel()
            {
                Email = logOnViewModel.Email,
                Password = logOnViewModel.Password,
                PhoneNumber = logOnViewModel.PhoneNumber,
                LoginLocation = logOnViewModel.LoginLocation,
                LoginDate = logOnViewModel.LoginDate,
                RequestURL = logOnViewModel.RequestURL,
                TenantID = logOnViewModel.TenantID,
                CreatedByEntity = logOnViewModel.CreatedByEntity,
                DeviceId = logOnViewModel.DeviceId
            };
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

        public string ValidateDoctor(DoctorViewModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.FirstName))
            {
                return "First Name require";
            }
            else if (string.IsNullOrEmpty(userModel.LastName))
            {
                return "Last Name require";
            }
            else if (string.IsNullOrEmpty(userModel.PhoneNumber))
            {
                return "Phone Number require";
            }
            else if (string.IsNullOrEmpty(userModel.EmailAddress))
            {
                return "Email is require";
            }
            else if (string.IsNullOrEmpty(userModel.RegistrationNumber))
            {
                return "Registration number is require";
            }
            return "";
        }

        public string ValidateDoctorLogOn(DoctorLogonViewModel doctorLogOnViewModel)
        {
            if (!Validator.EmailValidation(doctorLogOnViewModel.Email))
            {
                return "Invalid Email";
            }
            if (string.IsNullOrEmpty(doctorLogOnViewModel.Password))
            {
                return "Invalid Password";
            }
            return string.Empty;
        }

        public string ValidateDoctorDevice(DoctorLogonViewModel doctorLogOnViewModel)
        {
            if (doctorLogOnViewModel.Id <= 0)
            {
                return "Invalid Doctor Id";
            }
            if (string.IsNullOrEmpty(doctorLogOnViewModel.DeviceId))
            {
                return "Invalid Device Id";
            }
            return string.Empty;
        }

        public string ValidateChangePasswordOn(ChangePasswordViewModel changePasswordViewModel)
        {
            return "";
        }

        public string ValidateChangePasswordFromAdmin(ChangePasswordViewModel changePasswordViewModel)
        {
            if (changePasswordViewModel.LongId <= 0)
            {
                return "Invalid Id Supplied";
            }
            return string.Empty;
        }


    }
}