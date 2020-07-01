using System;
using System.Collections.Generic;

using System.Web.Http;
using ImTech.DataServices;
using ImTech.Service.ViewModels;
using ImTech.Service.Mapper;
using System.Net.Http;
using System.Net;
using ImTech.Service.ViewModels.Doctor;
using ImTech.Model;

namespace ImTech.Service.Controllers
{
    public class DoctorController : ApiController
    {
        private readonly StaticDataServices _staticDataServices;
        private DataServices.DataServices _dataServices;
        private ViewModelFactory _viewModelFactory;
        private ILogger _logger;
        private DoctorMapper _doctorMapper;


        public DoctorController(StaticDataServices staticDataServices, DataServices.DataServices dataServices, ViewModelFactory viewModelFactory, DoctorMapper doctorMapper, ILogger logger)
        {
            this._staticDataServices = staticDataServices;
            this._dataServices = dataServices;
            this._viewModelFactory = viewModelFactory;
            this._doctorMapper = doctorMapper;
            this._logger = logger;
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult DoctorLogin(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var doctorLogOnViewModel = WebCommon.Instance.GetObject<DoctorLogonViewModel>(requestCarrier.PayLoad);
                    string validateError = this._doctorMapper.ValidateDoctorLogOn(doctorLogOnViewModel);
                    if (string.IsNullOrEmpty(validateError))
                    {
                        doctorLogOnViewModel.TenantID = requestCarrier.TanentId;
                        var doctorModel = this._doctorMapper.Map(doctorLogOnViewModel);
                        var result = this._dataServices.DoctorService.ValidateDoctorLogOn(doctorModel);

                        if (result.Success)
                        {
                            var doctorModelres = this._dataServices.DoctorService.GetDoctor(result.Id, requestCarrier.TanentId);
                            response = new ResponseCarrier() { Status = true, PayLoad = doctorModelres, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                        }
                        else
                        {
                            response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = result.ErrorMessage, TanentId = requestCarrier.TanentId };
                        }
                        //var result = this._dataServices.UserService.UserLogOn(userModel);
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = validateError, TanentId = requestCarrier.TanentId };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid request.User Login fails", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid request.User Login fails", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult UpdateDevice(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var doctorLogOnViewModel = WebCommon.Instance.GetObject<DoctorLogonViewModel>(requestCarrier.PayLoad);
                    string validateError = this._doctorMapper.ValidateDoctorDevice(doctorLogOnViewModel);
                    if (string.IsNullOrEmpty(validateError))
                    {
                        doctorLogOnViewModel.TenantID = requestCarrier.TanentId;
                        var doctorModel = this._doctorMapper.Map(doctorLogOnViewModel);
                        doctorModel.Id = doctorLogOnViewModel.Id;
                        var result = this._dataServices.DoctorService.UpdateDeviceId(doctorModel);

                        if (result)
                        {
                            response = new ResponseCarrier() { Status = true, PayLoad = result, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                        }
                        else
                        {
                            response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Error in Updating Device Id", TanentId = requestCarrier.TanentId };
                        }
                        //var result = this._dataServices.UserService.UserLogOn(userModel);
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = validateError, TanentId = requestCarrier.TanentId };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid request.User Login fails", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid request.User Login fails", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor" + "," + "Admin")]
        [HttpPost]
        public IHttpActionResult GetDoctor(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var doctorViewModel = WebCommon.Instance.GetObject<DoctorViewModel>(requestCarrier.PayLoad);
                    if (doctorViewModel != null && doctorViewModel.Id > 0)
                    {
                        var doctorModel = this._dataServices.DoctorService.GetDoctor(doctorViewModel.Id, requestCarrier.TanentId);
                        _staticDataServices.Start();
                        doctorViewModel = this._doctorMapper.Map(doctorModel);

                        response = new ResponseCarrier()
                        {
                            Status = true,
                            PayLoad = doctorViewModel,
                            ErrorMessage = string.Empty
                        };
                    }
                    else
                    {
                        doctorViewModel = _viewModelFactory.CreateDoctorViewModel();
                        response = new ResponseCarrier()
                        {
                            Status = true,
                            PayLoad = doctorViewModel,
                            ErrorMessage = string.Empty
                        };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid Request. Doctor Id not provided.", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId(doctorId) not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor" + "," + "Admin")]
        [HttpPost]
        public IHttpActionResult SaveDoctor(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue)
            {
                DoctorViewModel dViewModel = WebCommon.Instance.GetObject<DoctorViewModel>(requestCarrier.PayLoad);
                string validationResponse = this._doctorMapper.ValidateDoctor(dViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    dViewModel.TanentId = requestCarrier.TanentId;
                    var doctorModel = this._doctorMapper.Map(dViewModel);
                    doctorModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    doctorModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    doctorModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    doctorModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    doctorModel.Password = ConfigurationManager.DefaultPassword;
                    string encryptedPassword = Common.SecurityManager.EncryptText(doctorModel.Password);
                    doctorModel.HashPassword = encryptedPassword;
                    var userResponse = _dataServices.DoctorService.SaveDoctor(doctorModel);
                    if (userResponse.Success)
                    {
                        dViewModel.Id = userResponse.Id;
                        dViewModel.Password = string.Empty;
                        response = new ResponseCarrier() { Status = true, PayLoad = dViewModel, TanentId = requestCarrier.TanentId };
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = userResponse.ErrorMessage };
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

        [Authorize(Roles = "Doctor" + "," + "Admin")]
        [HttpPost]
        public IHttpActionResult GetDoctorList([FromBody]RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty)
            {
                var result = _doctorMapper.Map(_dataServices.DoctorService.GetDoctorList(-1, requestCarrier.TanentId));
                response = new ResponseCarrier() { Status = true, PayLoad = result, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult DeleteDoctor(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty)
            {
                var doctorModel = this._doctorMapper.Map(WebCommon.Instance.GetObject<DoctorViewModel>(requestCarrier.PayLoad));
                doctorModel.TenantID = requestCarrier.TanentId;
                var userResponse = _dataServices.DoctorService.DeleteDoctor(doctorModel);
                if (userResponse.Success)
                {
                    response = new ResponseCarrier() { Status = true, TanentId = requestCarrier.TanentId };
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, TanentId = requestCarrier.TanentId, ErrorMessage = userResponse.ErrorMessage };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor, User")]
        [HttpPost]
        public IHttpActionResult GetDoctorsByCode(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue)
            {
                string code = requestCarrier.PayLoad.ToString();
                var lstDoctorsModel = _dataServices.DoctorService.GetDoctorsByCode(code, requestCarrier.TanentId);
                if (lstDoctorsModel != null)
                {
                    var lstDoctorViewModel = _doctorMapper.MapDoctorShortModel(lstDoctorsModel);
                    response = new ResponseCarrier() { Status = true, PayLoad = lstDoctorViewModel, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Wrong code provided.", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult MapDoctor(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                string code = Convert.ToString(requestCarrier.PayLoad);
                Int64 userId = (Int64)requestCarrier.UserId.Value;
                Int64 createdBy = (int)requestCarrier.UserId.Value;
                Int64 createdByEntity = Convert.ToInt32(requestCarrier.From);
                string isMapped = _dataServices.DoctorService.MapDoctorToUser(userId, code, requestCarrier.TanentId, createdBy, createdByEntity);
                if (string.IsNullOrEmpty(isMapped))
                {
                    response = new ResponseCarrier() { Status = true, PayLoad = true, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = isMapped, TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult GetMappedDoctorForUser(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                string code = requestCarrier.PayLoad.ToString();
                Int64 userId = requestCarrier.UserId.Value;

                var lstDoctorsModel = _dataServices.DoctorService.GetMappedDoctorForUser(userId, requestCarrier.TanentId);
                if (lstDoctorsModel != null)
                {
                    var lstDoctorViewModel = _doctorMapper.MapDoctorShortModel(lstDoctorsModel);
                    response = new ResponseCarrier() { Status = true, PayLoad = lstDoctorViewModel, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "No Doctor is mapped to this user.", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }


        [HttpGet]
        public IHttpActionResult GetHeartBeat()
        {
            _logger.LogError(new LogMessage
            {
                Summary = "payment object",
                Exception = Convert.ToString("bhanu"),
                UserId = 0,
                UserType = 0,
                Application = "Web"
            });
            return Json(DateTime.Now);
        }

        //[HttpPost]
        //public IHttpActionResult GetMyConsultations(RequestCarrier requestCarrier)
        //{
        //    ResponseCarrier response;
        //    if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
        //    {
        //        if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
        //        {
        //            Int64 userId = requestCarrier.UserId.Value;
        //            var result = _dataServices.DoctorService.GetMyConsultations(userId, requestCarrier.TanentId);
        //            var lstConsultationViewModel = _doctorMapper.MapMyConsultations(result);
        //            response = new ResponseCarrier() { Status = true, PayLoad = lstConsultationViewModel, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
        //        }
        //        else
        //        {
        //            response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
        //        }
        //    }
        //    else
        //    {
        //        response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
        //    }
        //    return Json(response);
        //}

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult GetConsultations(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
                {
                    Int64 userId = requestCarrier.UserId.Value;
                    var result = _dataServices.DoctorService.GetConsultations(userId, requestCarrier.TanentId);
                    var lstConsultationViewModel = _doctorMapper.MapDoctorConsultation(result);
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

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult SaveMessage(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
                {
                    Int64 userId = requestCarrier.UserId.Value;
                    MessageViewModel dViewModel = WebCommon.Instance.GetObject<MessageViewModel>(requestCarrier.PayLoad);
                    dViewModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    dViewModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    dViewModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    dViewModel.ModifiedEntity = Convert.ToInt32(requestCarrier.From);
                    var messageModel = _doctorMapper.MapDoctorMessage(dViewModel);
                    var result = _dataServices.DoctorService.SaveMessage(messageModel, requestCarrier.TanentId);
                    response = new ResponseCarrier() { Status = true, PayLoad = messageModel, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
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

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult ChangeDoctorPassword(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var changePasswordViewModel = WebCommon.Instance.GetObject<ChangePasswordViewModel>(requestCarrier.PayLoad);
                    changePasswordViewModel.TanentId = requestCarrier.TanentId;
                    string validateError = this._doctorMapper.ValidateChangePasswordOn(changePasswordViewModel);
                    if (string.IsNullOrEmpty(validateError))
                    {
                        changePasswordViewModel.NewHashPassword = Common.SecurityManager.EncryptText(changePasswordViewModel.NewPassword);
                        var changePassword = this._doctorMapper.MapChangePassword(changePasswordViewModel);
                        changePassword.CreatedBy = (int)requestCarrier.UserId.Value;
                        changePassword.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                        changePassword.ModifiedBy = (int)requestCarrier.UserId.Value;
                        changePassword.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                        var result = this._dataServices.DoctorService.ChangeDoctorPassword(changePassword);
                        if (result.Success)
                        {
                            response = new ResponseCarrier() { Status = true, PayLoad = null, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                        }
                        else
                        {
                            response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = result.ErrorMessage, TanentId = requestCarrier.TanentId };
                        }
                        //var result = this._dataServices.UserService.UserLogOn(userModel);
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = validateError, TanentId = requestCarrier.TanentId };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid request.Password change failed", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid request.User Login fails", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult ChangePasswordFromAdmin(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var changePasswordViewModel = WebCommon.Instance.GetObject<ChangePasswordViewModel>(requestCarrier.PayLoad);
                    changePasswordViewModel.TanentId = requestCarrier.TanentId;
                    string validateError = this._doctorMapper.ValidateChangePasswordFromAdmin(changePasswordViewModel);
                    if (string.IsNullOrEmpty(validateError))
                    {
                        changePasswordViewModel.NewHashPassword = Common.SecurityManager.EncryptText(changePasswordViewModel.NewPassword);
                        var changePassword = this._doctorMapper.MapChangePassword(changePasswordViewModel);
                        changePassword.LongId = changePasswordViewModel.LongId;
                        changePassword.CreatedBy = (int)requestCarrier.UserId.Value;
                        changePassword.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                        changePassword.ModifiedBy = (int)requestCarrier.UserId.Value;
                        changePassword.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                        var result = this._dataServices.DoctorService.ChangeDoctorPasswordFromAdmin(changePassword);
                        if (result.Success)
                        {
                            response = new ResponseCarrier() { Status = true, PayLoad = null, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                        }
                        else
                        {
                            response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = result.ErrorMessage, TanentId = requestCarrier.TanentId };
                        }
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = validateError, TanentId = requestCarrier.TanentId };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid request.Password change failed", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid request.User Login fails", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }


        [HttpPost]
        public IHttpActionResult ForgotDoctorPassword(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var changePasswordViewModel = WebCommon.Instance.GetObject<ChangePasswordViewModel>(requestCarrier.PayLoad);
                    changePasswordViewModel.TanentId = requestCarrier.TanentId;
                    string validateError = this._doctorMapper.ValidateChangePasswordOn(changePasswordViewModel);
                    if (string.IsNullOrEmpty(validateError))
                    {
                        var changePassword = this._doctorMapper.MapChangePassword(changePasswordViewModel);
                        changePassword.CreatedBy = (int)requestCarrier.UserId.Value;
                        changePassword.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                        changePassword.ModifiedBy = (int)requestCarrier.UserId.Value;
                        changePassword.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                        var result = this._dataServices.DoctorService.ValidateDoctorEmail(changePassword);
                        if (result.Success)
                        {
                            response = new ResponseCarrier() { Status = true, PayLoad = null, ErrorMessage = "You will receive sms with default password", TanentId = requestCarrier.TanentId };
                        }
                        else
                        {
                            response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = result.ErrorMessage, TanentId = requestCarrier.TanentId };
                        }
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = validateError, TanentId = requestCarrier.TanentId };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid request.Password change failed", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid request.Operation fails", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult GetStats(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {

                Int64 userId = requestCarrier.UserId.Value;
                var statModel = _dataServices.DoctorService.GetDoctorStats(userId, requestCarrier.TanentId);
                var result = _doctorMapper.Map(statModel);
                response = new ResponseCarrier() { Status = true, PayLoad = result, ErrorMessage = string.Empty };
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult SetIsAvailable(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                if (requestCarrier.PayLoad != null)
                {
                    Int64 doctorId = requestCarrier.UserId.Value;
                    bool? IsAvailable = null;
                    if (requestCarrier.PayLoad is bool)
                    {
                        IsAvailable = (bool)requestCarrier.PayLoad;
                    }
                    else if (requestCarrier.PayLoad is int)
                    {
                        IsAvailable = (int)requestCarrier.PayLoad == 1 ? true : false;
                    }
                    if (IsAvailable.HasValue)
                    {
                        bool updated = _dataServices.DoctorService.SetDoctorAvailability(doctorId, IsAvailable.Value, requestCarrier.TanentId);
                        if (updated)
                        {
                            response = new ResponseCarrier() { Status = true, PayLoad = updated, ErrorMessage = string.Empty };
                        }
                        else
                        {
                            response = new ResponseCarrier() { Status = false, PayLoad = updated, ErrorMessage = "Error in updating Avilability" };
                        }
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Doctor IsAvailability is not provided.", TanentId = requestCarrier.TanentId };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Doctor IsAvailability is not provided.", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }
        //[HttpPost]
        //public IHttpActionResult GetDoctorStatistics(RequestCarrier requestCarrier)
        //{
        //    ResponseCarrier response;
        //    if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null)
        //    {
        //        if (requestCarrier.PayLoad != null)
        //        {
        //            Int64 userId = requestCarrier.UserId.Value;
        //            var result = _dataServices.DoctorService.GetDoctorStat(userId, requestCarrier.TanentId);
        //        }
        //    }
        //}

        //[HttpPost]
        //IHttpActionResult UpdateConsultationStatus(RequestCarrier requestCarrier)
        //{
        //    ResponseCarrier response;
        //    if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
        //    {
        //        if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
        //        {
        //            Int64 userId = requestCarrier.UserId.Value;
        //            ConsultationViewModel dViewModel = WebCommon.Instance.GetObject<ConsultationViewModel>(requestCarrier.PayLoad);
        //            dViewModel.TanentId = Convert.ToInt32(requestCarrier.TanentId);
        //            var model = _doctorMapper.MapShortConsultationModel(dViewModel);
        //            _dataServices.ConsultationService.UpdateConsultationStatus(model);
        //            _dataServices.DoctorService.UpdateConsultationMessage(model)
        //        }
        //    }
        //}
    }
}