using ImTech.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImTech.Model;
namespace ImTech.Service.Mapper
{
    public class ConsultationMapper
    {
        private ViewModelFactory _viewModelFactory;
        public ConsultationMapper(ViewModelFactory viewModelFactory)
        {
            this._viewModelFactory = viewModelFactory;
        }
        public ConsultationViewModel MapConsultation(ConsultationModel model)
        {
            return new ConsultationViewModel
            {
                Id = model.Id,
                IsDeleted = model.IsDeleted,
                CreatedBy = model.CreatedBy,
                CreatedByEntity = model.CreatedByEntity,
                CreatedOn = model.CreatedOn,
                ModifiedBy = model.ModifiedBy,
                ModifiedEntity = model.ModifiedByEntity,
                ModifiedOn = model.ModifiedOn,
                ConsultationModeID = model.ConsultationModeID,
                ConsultationStatusID = model.ConsultationStatusID,
                ConsultationTime = model.ConsultationTime,
                Description = model.Description,
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
                TanentId = model.TenantID,
                Prescriptions = MapConsultationPrescriptions(model.Prescriptions).ToList(),
                CaseNotes = MapConsultationNotes(model.CaseNotes).ToList(),
                Patient = MapPatient(model.Patient)
            };
        }

        public IEnumerable<PrescreptionViewModel> MapConsultationPrescriptions(IEnumerable<PrescreptionModel> models)
        {
            return models.Select(MapConsultationPrescription);
        }

        public PrescreptionViewModel MapConsultationPrescription(PrescreptionModel model)
        {
            return new PrescreptionViewModel()
            {
                ConsultationId = model.ConsultationId,
                PrescreptionText = model.PrescreptionText,
                IsDeleted = model.IsDeleted,
                TanentId = model.TenantID,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn,
                CreatedByEntity = model.CreatedByEntity,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                ModifiedEntity = model.ModifiedByEntity
            };
        }

        public IEnumerable<CaseNoteViewModel> MapConsultationNotes(IEnumerable<CaseNoteModel> models)
        {
            return models.Select(MapConsultationNote);
        }

        public CaseNoteViewModel MapConsultationNote(CaseNoteModel model)
        {
            return new CaseNoteViewModel()
            {
                Id = model.Id,
                ConsultationId = model.ConsultationId,
                NoteText = model.NoteText,
                IsDeleted = model.IsDeleted,
                TanentId = model.TenantID,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn,
                CreatedByEntity = model.CreatedByEntity,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                ModifiedEntity = model.ModifiedByEntity,
                //Type = model.Type
            };
        }

        public ConsultationModel MapConsultation(ConsultationViewModel vmodel)
        {
            return new ConsultationModel
            {
                Id = vmodel.Id,
                IsDeleted = vmodel.IsDeleted,
                CreatedBy = vmodel.CreatedBy,
                CreatedByEntity = vmodel.CreatedByEntity,
                CreatedOn = vmodel.CreatedOn,
                ModifiedBy = vmodel.ModifiedBy,
                ModifiedByEntity = vmodel.ModifiedEntity,
                ModifiedOn = vmodel.ModifiedOn,
                ConsultationModeID = vmodel.ConsultationModeID,
                ConsultationStatusID = vmodel.ConsultationStatusID,
                ConsultationTime = vmodel.ConsultationTime,
                Description = vmodel.Description,
                DoctorId = vmodel.DoctorId,
                PatientId = vmodel.PatientId,
                TenantID = vmodel.TanentId

            };
        }

        public PatientDetailViewModel MapPatient(PatientDetail model)
        {
            return new PatientDetailViewModel
            {
                Complaints = model.Complaints,
                ConsultationType = model.ConsultationType,
                CreatedBy = model.CreatedBy,
                CreatedByEntity = model.CreatedByEntity,
                CreatedOn = model.CreatedOn,
                Files = model.Files == string.Empty ? new List<string>() : new List<string>(model.Files.Split(',')),
                FileId = model.FileId,
                Id = model.Id,
                IsDeleted = model.IsDeleted,
                ModifiedBy = model.ModifiedBy,
                ModifiedEntity = model.ModifiedByEntity,
                ModifiedOn = model.ModifiedOn,
                PatientAge = model.PatientAge,
                PatientGender = model.PatientGender,
                PatientName = model.PatientName,
                PatientPhone = model.PatientPhone,
                TanentId = model.TenantID,
                DoctorId = model.DoctorId,
                UserID = model.UserID,
                Case = model.Case,
                PreferredTime = model.PreferredTime
            };
        }
        public PatientDetail MapPatient(PatientDetailViewModel vmodel)
        {
            return new PatientDetail
            {
                Complaints = vmodel.Complaints,
                ConsultationType = vmodel.ConsultationType,
                CreatedBy = vmodel.CreatedBy,
                CreatedByEntity = vmodel.CreatedByEntity,
                CreatedOn = vmodel.CreatedOn,
                FileId = vmodel.FileId,
                Id = vmodel.Id,
                IsDeleted = vmodel.IsDeleted,
                ModifiedBy = vmodel.ModifiedBy,
                ModifiedByEntity = vmodel.ModifiedEntity,
                ModifiedOn = vmodel.ModifiedOn,
                PatientAge = vmodel.PatientAge,
                PatientGender = vmodel.PatientGender,
                PatientName = vmodel.PatientName,
                PatientPhone = vmodel.PatientPhone,
                TenantID = vmodel.TanentId,
                DoctorId = vmodel.DoctorId,
                UserID = vmodel.UserID,
                PreferredTime = Convert.ToInt16(vmodel.PreferredTime),
                Case = Convert.ToInt16(vmodel.Case)

            };
        }

        public PaymentViewModel MapPayment(PaymentModel model)
        {
            return new PaymentViewModel
            {
                AmountActual = model.AmountActual,
                Id = model.Id,
                IsDeleted = model.IsDeleted,
                ModifiedBy = model.ModifiedBy,
                ModifiedEntity = model.ModifiedByEntity,
                ModifiedOn = model.ModifiedOn,
                CreatedBy = model.CreatedBy,
                CreatedByEntity = model.CreatedByEntity,
                CreatedOn = model.CreatedOn,
                AmountCharged = model.AmountCharged,
                ConsultationID = model.ConsultationID,
                DiscountAmount = model.DiscountAmount,
                GatewayResponse = model.GatewayResponse,
                PaymentStatusID = model.PaymentStatusID,
                PromotionalID = model.PromotionalID,
                TanentId = model.TenantID
            };
        }
        public PaymentModel MapPayment(PaymentViewModel vmodel)
        {
            return new PaymentModel
            {
                AmountActual = vmodel.AmountActual,
                Id = vmodel.Id,
                IsDeleted = vmodel.IsDeleted,
                ModifiedBy = vmodel.ModifiedBy,
                ModifiedByEntity = vmodel.ModifiedEntity,
                ModifiedOn = vmodel.ModifiedOn,
                CreatedBy = vmodel.CreatedBy,
                CreatedByEntity = vmodel.CreatedByEntity,
                CreatedOn = vmodel.CreatedOn,
                AmountCharged = vmodel.AmountCharged,
                ConsultationID = vmodel.ConsultationID,
                DiscountAmount = vmodel.DiscountAmount,
                GatewayResponse = vmodel.GatewayResponse,
                PaymentStatusID = vmodel.PaymentStatusID,
                PromotionalID = vmodel.PromotionalID,
                TenantID = vmodel.TanentId
            };
        }

        public PrescreptionViewModel MapPrescreption(PrescreptionModel model)
        {
            return new PrescreptionViewModel
            {
                ConsultationId = model.ConsultationId,
                PrescreptionText = model.PrescreptionText,
                IsDeleted = model.IsDeleted,
                ModifiedBy = model.ModifiedBy,
                ModifiedEntity = model.ModifiedByEntity,
                ModifiedOn = model.ModifiedOn,
                CreatedBy = model.CreatedBy,
                CreatedByEntity = model.CreatedByEntity,
                CreatedOn = model.CreatedOn,
                TanentId = model.TenantID,
                Type = model.Type
            };
        }
        public PrescreptionModel MapPrescreption(PrescreptionViewModel vmodel)
        {
            return new PrescreptionModel
            {
                PrescreptionText = vmodel.PrescreptionText,
                ConsultationId = vmodel.ConsultationId,
                ModifiedBy = vmodel.ModifiedBy,
                ModifiedByEntity = vmodel.ModifiedEntity,
                ModifiedOn = vmodel.ModifiedOn,
                CreatedBy = vmodel.CreatedBy,
                CreatedByEntity = vmodel.CreatedByEntity,
                CreatedOn = vmodel.CreatedOn,
                TenantID = vmodel.TanentId,
                Type = vmodel.Type
            };
        }

        public CaseNoteViewModel MapCaseNote(CaseNoteModel model)
        {
            return new CaseNoteViewModel
            {
                ConsultationId = model.ConsultationId,
                NoteText = model.NoteText,
                Type = model.Type,
                IsDeleted = model.IsDeleted,
                ModifiedBy = model.ModifiedBy,
                ModifiedEntity = model.ModifiedByEntity,
                ModifiedOn = model.ModifiedOn,
                CreatedBy = model.CreatedBy,
                CreatedByEntity = model.CreatedByEntity,
                CreatedOn = model.CreatedOn,
                TanentId = model.TenantID,
                //Type = model.Type
            };
        }
        public CaseNoteModel MapCaseNote(CaseNoteViewModel vmodel)
        {
            return new CaseNoteModel
            {
                NoteText = vmodel.NoteText,
                ConsultationId = vmodel.ConsultationId,
                Type = vmodel.Type,
                ModifiedBy = vmodel.ModifiedBy,
                ModifiedByEntity = vmodel.ModifiedEntity,
                ModifiedOn = vmodel.ModifiedOn,
                CreatedBy = vmodel.CreatedBy,
                CreatedByEntity = vmodel.CreatedByEntity,
                CreatedOn = vmodel.CreatedOn,
                TenantID = vmodel.TanentId
            };
        }

        public IEnumerable<MessageViewModel> MapMessages(IEnumerable<MessageModel> models)
        {
            return models.Select(MapMessages);
        }

        public MessageViewModel MapMessages(MessageModel vmodel)
        {
            return new MessageViewModel
            {
                Id = vmodel.Id,
                TextMessage = vmodel.TextMessage,
                TanentId = vmodel.TenantID,
                ConsultationId = vmodel.ConsultationId,
                PatientId = vmodel.PatientId,
                DoctorId = vmodel.DoctorId,
                ModifiedBy = vmodel.ModifiedBy,
                ModifiedEntity = vmodel.ModifiedByEntity,
                ModifiedOn = vmodel.ModifiedOn,
                CreatedBy = vmodel.CreatedBy,
                CreatedByEntity = vmodel.CreatedByEntity,
                CreatedOn = vmodel.CreatedOn
            };
        }


        public string ValidatePaymentDetails(PaymentViewModel paymentViewModel)
        {
            return string.Empty;
        }
        public string ValidatePrescreption(PrescreptionViewModel prescreptionViewModel)
        {
            if (string.IsNullOrEmpty(prescreptionViewModel.PrescreptionText))
                return "PrescreptionText is empty";
            if (prescreptionViewModel.Type == 0)
                return "Type Not Provided";
            return string.Empty;
        }
        public string ValidateCaseNote(CaseNoteViewModel caseNoteViewModel)
        {
            if (caseNoteViewModel.Type == 0)
                return "Type Not Provided";
            if (string.IsNullOrEmpty(caseNoteViewModel.NoteText))
                return "NoteText Provided";
            return string.Empty;
        }
        public string ValidatePatientDetails(PatientDetailViewModel patientDetailViewModel)
        {
            if (string.IsNullOrEmpty(patientDetailViewModel.PatientName))
            {
                return "Patient Name is require";
            }
            if (patientDetailViewModel.PatientGender == ' ')
            {
                return "Patient Gender is require";
            }
            if (patientDetailViewModel.PatientAge < 0)
            {
                return "Invalid Patient age";
            }
            if (!Validator.PhoneValidation(patientDetailViewModel.PatientPhone.ToString()))
            {
                return "Invalid Patient Phone";
            }
            if (string.IsNullOrEmpty(patientDetailViewModel.Complaints))
            {
                return "Patient Complaint require";
            }
            return string.Empty;
        }

        public string ValidateConsultationDetails(ConsultationViewModel patientDetailViewModel)
        {
            if (string.IsNullOrEmpty(patientDetailViewModel.Description))
            {
                return "Description is require";
            }
            if (patientDetailViewModel.ConsultationTime == null)
            {
                return "Consultation time is require";
            }
            if (patientDetailViewModel.DoctorId <= 0)
            {
                return "Doctor for Consultation is require";
            }
            if (patientDetailViewModel.PatientId <= 0)
            {
                return "Patient for Consultation is require";
            }
            return string.Empty;
        }
        public string ValidateConsultationDetailsForStatusUpdate(ConsultationViewModel patientDetailViewModel)
        {
            return string.Empty;
        }
        public string ValidateConsultationDetailsForTimeUpdate(ConsultationViewModel patientDetailViewModel)
        {
            return string.Empty;
        }
    }
}