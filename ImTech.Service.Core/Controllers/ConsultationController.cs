using System;
using System.Web.Http;
using ImTech.DataServices;
using ImTech.Service.ViewModels;
using ImTech.Service.Mapper;
using System.Collections.Generic;
using Razorpay.Api;
using Newtonsoft.Json;
using static ImTech.Service.WebCommon;
using ImTech.Model;
using ImTech.Notification.Client;

namespace ImTech.Service.Controllers
{

    public class ConsultationController : ApiController
    {
        private readonly StaticDataServices _staticDataServices;
        private DataServices.DataServices _dataServices;
        private ViewModelFactory _viewModelFactory;
        private ILogger _logger;
        private ConsultationMapper _consultationMapper;

        public ConsultationController(StaticDataServices staticDataServices, DataServices.DataServices dataServices, ViewModelFactory viewModelFactory, ConsultationMapper consultationMapper, ILogger logger)
        {
            _staticDataServices = staticDataServices;
            _dataServices = dataServices;
            _viewModelFactory = viewModelFactory;
            _consultationMapper = consultationMapper;
            _logger = logger;
        }

        [Authorize(Roles = "Doctor" + "," + "User")]
        [HttpPost]
        public IHttpActionResult GetConsultationDetails(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
                {
                    Int64 userId = requestCarrier.UserId.Value;
                    Int64 consultationId = Convert.ToInt64(requestCarrier.PayLoad);
                    var result = _dataServices.ConsultationService.GetConsultationDetails(consultationId, requestCarrier.TanentId);
                    var lstConsultationViewModel = _consultationMapper.MapConsultation(result);
                    response = new ResponseCarrier() { Status = true, PayLoad = lstConsultationViewModel, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor" + "," + "User")]
        [HttpPost]
        public IHttpActionResult GetConsultationPrescreptions(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
                {
                    Int64 userId = requestCarrier.UserId.Value;
                    Int64 consultationId = Convert.ToInt64(requestCarrier.PayLoad);
                    var result = _dataServices.ConsultationService.GetConsultationPrescriptions(consultationId, requestCarrier.TanentId);
                    response = new ResponseCarrier() { Status = true, PayLoad = result, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor" + "," + "User")]
        //[Authorize(Roles = "User")]
        [HttpPost]
        public IHttpActionResult GetConsultationCaseNotes(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
                {
                    Int64 userId = requestCarrier.UserId.Value;
                    Int64 consultationId = Convert.ToInt64(requestCarrier.PayLoad);
                    var result = _dataServices.ConsultationService.GetConsultationCaseNotes(consultationId, requestCarrier.TanentId);
                    response = new ResponseCarrier() { Status = true, PayLoad = result, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public IHttpActionResult SavePatientDetails(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                PatientDetailViewModel pViewModel = WebCommon.Instance.GetObject<PatientDetailViewModel>(requestCarrier.PayLoad);
                pViewModel.TanentId = requestCarrier.TanentId;
                string validationResponse = this._consultationMapper.ValidatePatientDetails(pViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    var patientDetailModel = this._consultationMapper.MapPatient(pViewModel);
                    patientDetailModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    patientDetailModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    patientDetailModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    patientDetailModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    patientDetailModel.UserID = (int)requestCarrier.UserId.Value;
                    if (!string.IsNullOrEmpty(patientDetailModel.FileId))
                    {
                        patientDetailModel.FileId = patientDetailModel.FileId.Replace('|', ',');
                    }
                    else
                    {
                        patientDetailModel.FileId = string.Empty;
                    }
                    var patientResponse = _dataServices.ConsultationService.SavePatientDetails(patientDetailModel);
                    if (patientResponse.Success)
                    {
                        pViewModel.Id = patientResponse.Id;
                        response = new ResponseCarrier() { Status = true, PayLoad = pViewModel, TanentId = requestCarrier.TanentId };
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = patientResponse.ErrorMessage };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = validationResponse };
                }

            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public IHttpActionResult SaveConsultationDetails(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                ConsultationViewModel cViewModel = WebCommon.Instance.GetObject<ConsultationViewModel>(requestCarrier.PayLoad);
                cViewModel.TanentId = requestCarrier.TanentId;
                string validationResponse = this._consultationMapper.ValidateConsultationDetails(cViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    var consultationDetailModel = this._consultationMapper.MapConsultation(cViewModel);
                    consultationDetailModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    consultationDetailModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    consultationDetailModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    consultationDetailModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    var consultationResponse = _dataServices.ConsultationService.SaveConsultationDetails(consultationDetailModel);
                    if (consultationResponse.Success)
                    {
                        cViewModel.Id = consultationResponse.Id;
                        response = new ResponseCarrier() { Status = true, PayLoad = cViewModel, TanentId = requestCarrier.TanentId };
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = consultationResponse.ErrorMessage };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = validationResponse };
                }

            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor" + "," + "User")]
        [HttpPost]
        public IHttpActionResult UpdateConsultationStatus(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                ConsultationViewModel cViewModel = WebCommon.Instance.GetObject<ConsultationViewModel>(requestCarrier.PayLoad);
                cViewModel.TanentId = requestCarrier.TanentId;
                string validationResponse = this._consultationMapper.ValidateConsultationDetailsForStatusUpdate(cViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    var consultationDetailModel = this._consultationMapper.MapConsultation(cViewModel);
                    consultationDetailModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    consultationDetailModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    consultationDetailModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    consultationDetailModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    var consultationResponse = _dataServices.ConsultationService.UpdateConsultationStatus(consultationDetailModel);
                    if (consultationResponse.Success)
                    {
                        cViewModel.Id = consultationResponse.Id;
                        if (consultationDetailModel.CreatedByEntity == 2 || consultationDetailModel.CreatedByEntity == 5)//doctor
                        {


                            if (WebCommon.NotificationEnabled)
                            {
                                var consultationDetails = _dataServices.ConsultationService.GetConsultationDetails(consultationDetailModel.Id, consultationDetailModel.TenantID);
                                if (consultationDetails != null)
                                {
                                    var userDetails = _dataServices.UserService.GetUser(consultationDetails.CreatedBy, consultationDetailModel.TenantID);
                                    var doctor = _dataServices.DoctorService.GetDoctor(consultationDetails.DoctorId, consultationDetailModel.TenantID);
                                    List<object> additionalParameters = new List<object>();
                                    additionalParameters.Add(userDetails.Email);
                                    additionalParameters.Add(userDetails.PhoneNumber);
                                    additionalParameters.Add(doctor.EmailAddress);
                                    additionalParameters.Add(doctor.PhoneNumber);
                                    additionalParameters.Add(consultationDetails.Patient.PatientName);
                                    additionalParameters.Add(doctor.FirstName + " " + doctor.LastName);
                                    additionalParameters.Add(consultationDetails.AmountCharged);
                                    additionalParameters.Add(consultationDetails.ConsultationMode);
                                    additionalParameters.Add(consultationDetails.ConsultationTime);
                                    if (consultationDetailModel.ConsultationStatusID == 2)//accepted by doctor
                                    {
                                        NotificationClient.Instance.SendMessage(null, ImTech.Notification.Messages.MessageType.Booking_Accepted_Patient, additionalParameters);
                                    }
                                    else if (consultationDetailModel.ConsultationStatusID == 3)//end by doctor
                                    {
                                        NotificationClient.Instance.SendMessage(null, ImTech.Notification.Messages.MessageType.Booking_End_Patient, additionalParameters);
                                        NotificationClient.Instance.SendMessage(null, ImTech.Notification.Messages.MessageType.Booking_End_Doctor, additionalParameters);
                                    }


                                }
                            }


                        }
                        else if (consultationDetailModel.CreatedByEntity == 1 || consultationDetailModel.CreatedByEntity == 4)//user
                        {
                            if (WebCommon.NotificationEnabled)
                            {
                                var consultationDetails = _dataServices.ConsultationService.GetConsultationDetails(consultationDetailModel.Id, consultationDetailModel.TenantID);
                                if (consultationDetails != null)
                                {
                                    var userDetails = _dataServices.UserService.GetUser(consultationDetails.CreatedBy, consultationDetailModel.TenantID);
                                    var doctor = _dataServices.DoctorService.GetDoctor(consultationDetails.DoctorId, consultationDetailModel.TenantID);
                                    List<object> additionalParameters = new List<object>();
                                    additionalParameters.Add(userDetails.Email);
                                    additionalParameters.Add(userDetails.PhoneNumber);
                                    additionalParameters.Add(doctor.EmailAddress);
                                    additionalParameters.Add(doctor.PhoneNumber);
                                    additionalParameters.Add(consultationDetails.Patient.PatientName);
                                    additionalParameters.Add(doctor.FirstName + " " + doctor.LastName);
                                    additionalParameters.Add(consultationDetails.AmountCharged);
                                    additionalParameters.Add(consultationDetails.ConsultationMode);
                                    additionalParameters.Add(consultationDetails.ConsultationTime);
                                    if (consultationDetailModel.ConsultationStatusID == 3)//cancelled by patient
                                    {
                                        NotificationClient.Instance.SendMessage(null, ImTech.Notification.Messages.MessageType.Booking_Cancelled_Patient, additionalParameters);
                                        NotificationClient.Instance.SendMessage(null, ImTech.Notification.Messages.MessageType.Booking_Cancelled_Doctor, additionalParameters);
                                    }
                                }
                            }
                        }

                        response = new ResponseCarrier() { Status = true, PayLoad = cViewModel, TanentId = requestCarrier.TanentId };
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = consultationResponse.ErrorMessage };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = validationResponse };
                }

            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor" + "," + "User")]
        [HttpPost]
        public IHttpActionResult UpdateConsultationTime(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                ConsultationViewModel cViewModel = WebCommon.Instance.GetObject<ConsultationViewModel>(requestCarrier.PayLoad);
                cViewModel.TanentId = requestCarrier.TanentId;
                string validationResponse = this._consultationMapper.ValidateConsultationDetailsForTimeUpdate(cViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    var consultationDetailModel = this._consultationMapper.MapConsultation(cViewModel);
                    consultationDetailModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    consultationDetailModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    consultationDetailModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    consultationDetailModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    var consultationResponse = _dataServices.ConsultationService.UpdateConsultationTime(consultationDetailModel);
                    if (consultationResponse.Success)
                    {
                        cViewModel.Id = consultationResponse.Id;
                        response = new ResponseCarrier() { Status = true, TanentId = requestCarrier.TanentId };
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = consultationResponse.ErrorMessage };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = validationResponse };
                }

            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public IHttpActionResult SavePaymentDetails(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                PaymentViewModel pViewModel = WebCommon.Instance.GetObject<PaymentViewModel>(requestCarrier.PayLoad);
                _logger.LogError(new LogMessage
                {
                    Summary = "payment object",
                    Exception = Convert.ToString(requestCarrier.PayLoad),
                    UserId = 0,
                    UserType = 0,
                    Application = "Web"
                });

                pViewModel.TanentId = requestCarrier.TanentId;
                string validationResponse = this._consultationMapper.ValidatePaymentDetails(pViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    var paymentDetailModel = this._consultationMapper.MapPayment(pViewModel);
                    paymentDetailModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    paymentDetailModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    paymentDetailModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    paymentDetailModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    paymentDetailModel.TenantID = requestCarrier.TanentId;
                    if (pViewModel.PaymentStatusID == 1)
                    {
                        if (!string.IsNullOrEmpty(paymentDetailModel.GatewayResponse))
                        {
                            if (this.CapturePayment(ref paymentDetailModel))
                            {
                                paymentDetailModel.PaymentStatusID = 4;
                            }
                            else
                            {
                                paymentDetailModel.PaymentStatusID = 5;
                            }
                        }
                    }
                    var paymentResponse = _dataServices.ConsultationService.SavePaymentDetails(paymentDetailModel);
                    if (paymentResponse.Success)
                    {
                        pViewModel.Id = paymentResponse.Id;
                        pViewModel.GatewayResponse = string.Empty;
                        response = new ResponseCarrier() { Status = true, PayLoad = pViewModel, TanentId = requestCarrier.TanentId };
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = paymentResponse.ErrorMessage };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = validationResponse };
                }

            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        private bool CapturePayment(ref Model.PaymentModel pViewModel)
        {
            try
            {
                decimal netPrice = pViewModel.AmountCharged;
                if (netPrice > 0)
                {
                    string paymentId = pViewModel.GatewayResponse;
                    Dictionary<string, object> input = new Dictionary<string, object>();
                    input.Add("amount", netPrice * 100); // this amount should be same as transaction amount
                    string key = ImTech.Service.Common.ConfigurationManager.PaymentGatewayKey;
                    string secret = ImTech.Service.Common.ConfigurationManager.PaymentGatewaySecret;
                    RazorpayClient client = new RazorpayClient(key, secret);
                    Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);
                    Razorpay.Api.Payment capturedPayment = payment.Capture(input);

                    string json = capturedPayment.Attributes.ToString();
                    dynamic JsonDe = JsonConvert.DeserializeObject(json);
                    PaymentStatus status = JsonDe.status.ToString().ToUpper() == "captured".ToUpper() ? PaymentStatus.Capture : PaymentStatus.CaptureFailed;
                    pViewModel.GatewayResponse = paymentId + "-->ResponseJson-->" + json;
                    if (status == PaymentStatus.Capture)
                    {

                        if (WebCommon.NotificationEnabled)
                        {
                            var consultationDetails = _dataServices.ConsultationService.GetConsultationDetails(pViewModel.ConsultationID, pViewModel.TenantID);
                            var doctor = _dataServices.DoctorService.GetDoctor(consultationDetails.DoctorId, pViewModel.TenantID);
                            if (consultationDetails != null)
                            {
                                var userDetails = _dataServices.UserService.GetUser(consultationDetails.CreatedBy, pViewModel.TenantID);
                                List<object> additionalParameters = new List<object>();
                                additionalParameters.Add(userDetails.Email);
                                additionalParameters.Add(userDetails.PhoneNumber);
                                additionalParameters.Add(doctor.EmailAddress);
                                additionalParameters.Add(doctor.PhoneNumber);
                                additionalParameters.Add(consultationDetails.Patient.PatientName);
                                additionalParameters.Add(doctor.FirstName + " " + doctor.LastName);
                                additionalParameters.Add(pViewModel.AmountCharged);
                                additionalParameters.Add(consultationDetails.ConsultationMode);
                                NotificationClient.Instance.SendMessage(null, ImTech.Notification.Messages.MessageType.Consultation_User, additionalParameters);
                                NotificationClient.Instance.SendMessage(null, ImTech.Notification.Messages.MessageType.Consultation_Doctor, additionalParameters);

                            }
                        }
                        return true;
                    }
                    else
                        return true;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                string errorMsg = string.Empty;
                if (ex.InnerException != null)
                {
                    errorMsg = ex.InnerException.Message;
                }
                else
                {
                    errorMsg = ex.Message;
                }
                _logger.LogError(new LogMessage
                {

                    Summary =errorMsg,
                    Exception = Convert.ToString(ex.StackTrace),
                    UserId = 0,
                    UserType = 0,
                    Application = "Web"
                });
                return true;
            }

        }

        [Authorize(Roles = "Doctor" + "," + "User")]
        [HttpPost]
        public IHttpActionResult SavePrescreption(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                PrescreptionViewModel pViewModel = WebCommon.Instance.GetObject<PrescreptionViewModel>(requestCarrier.PayLoad);
                pViewModel.TanentId = requestCarrier.TanentId;
                string validationResponse = this._consultationMapper.ValidatePrescreption(pViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    var prescreptionModel = this._consultationMapper.MapPrescreption(pViewModel);
                    prescreptionModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    prescreptionModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    prescreptionModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    prescreptionModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    var prescreptionResponse = _dataServices.ConsultationService.SavePrescreption(prescreptionModel);
                    if (prescreptionResponse.Success)
                    {
                        pViewModel.Id = prescreptionResponse.Id;
                        response = new ResponseCarrier() { Status = true, PayLoad = pViewModel, TanentId = requestCarrier.TanentId };
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = prescreptionResponse.ErrorMessage };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = validationResponse };
                }

            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor" + "," + "User")]
        [HttpPost]
        public IHttpActionResult SaveCaseNote(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                CaseNoteViewModel cViewModel = WebCommon.Instance.GetObject<CaseNoteViewModel>(requestCarrier.PayLoad);
                cViewModel.TanentId = requestCarrier.TanentId;
                string validationResponse = this._consultationMapper.ValidateCaseNote(cViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    var caseNoteModel = this._consultationMapper.MapCaseNote(cViewModel);
                    caseNoteModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    caseNoteModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    caseNoteModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    caseNoteModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    var caseNoteResponse = _dataServices.ConsultationService.SaveCaseNote(caseNoteModel);
                    if (caseNoteResponse.Success)
                    {
                        cViewModel.Id = caseNoteResponse.Id;
                        response = new ResponseCarrier() { Status = true, PayLoad = cViewModel, TanentId = requestCarrier.TanentId };
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = caseNoteResponse.ErrorMessage };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = validationResponse };
                }

            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor" + "," + "User")]
        public IHttpActionResult GetConsultationMessages(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                Int64 consultationId = (Int64)requestCarrier.PayLoad;

                var result = _dataServices.ConsultationService.GetMessages(consultationId, requestCarrier.TanentId);
                if (result != null)
                {
                    var lstMessageViewModel = _consultationMapper.MapMessages(result);
                    response = new ResponseCarrier() { Status = true, PayLoad = lstMessageViewModel, TanentId = requestCarrier.TanentId };
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = "No Message Retrived for input consultation" };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }


    }
}
