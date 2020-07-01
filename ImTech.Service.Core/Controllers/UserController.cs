using System.Web.Http;
using ImTech.DataServices;
using ImTech.Service.ViewModels;
using ImTech.Service.Mapper;
using static ImTech.Service.WebCommon;
using System;
using System.Collections.Generic;
using ImTech.Notification.Client;
using ImTech.Model;

namespace ImTech.Service.Controllers
{
    public class UserController : ApiController
    {
        private readonly StaticDataServices _staticDataServices;
        private DataServices.DataServices _dataServices;
        private ViewModelFactory _viewModelFactory;
        private ILogger _logger;
        private UserMapper _userMapper;
        private OtpMapper _otpMapper;
        private DoctorMapper _doctorMapper;


        public UserController(StaticDataServices staticDataServices, DataServices.DataServices dataServices, ViewModelFactory viewModelFactory, UserMapper userMapper, DoctorMapper doctorMapper, OtpMapper otpMapper, ILogger logger)
        {
            this._staticDataServices = staticDataServices;
            this._dataServices = dataServices;
            this._viewModelFactory = viewModelFactory;
            this._logger = logger;
            this._userMapper = userMapper;
            this._otpMapper = otpMapper;
            this._doctorMapper = doctorMapper;
        }


        [HttpPost]
        public IHttpActionResult Register(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null)
            {

                var userViewModel = WebCommon.Instance.GetObject<UserViewModel>(requestCarrier.PayLoad);

                string validateError = this._userMapper.ValidateUserRegistration(userViewModel);

                if (string.IsNullOrEmpty(validateError))
                {
                    userViewModel.TanentId = requestCarrier.TanentId;
                    userViewModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    userViewModel.ModifiedEntity = Convert.ToInt32(requestCarrier.From);
                    string encryptedPassword = Common.SecurityManager.EncryptText(userViewModel.Password);
                    var userModel = this._userMapper.Map(userViewModel);
                    userModel.HashPassword = encryptedPassword;
                    var result = this._dataServices.UserService.RegisterUser(userModel);
                    if (result.Success)
                    {
                        //userViewModel.Id = result.Id
                        response = new ResponseCarrier() { Status = true, PayLoad = null, ErrorMessage = "Registration Successful.", TanentId = requestCarrier.TanentId };
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid Request. Users Id not provided.", TanentId = requestCarrier.TanentId };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = validateError, TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid Request. User Registration can not be processed.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }


        [Authorize(Roles = "User" + "," + "Admin")]
        [HttpPost]
        public IHttpActionResult UserLogin([FromBody]RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var userLogOnViewModel = WebCommon.Instance.GetObject<UserLogonViewModel>(requestCarrier.PayLoad);
                    userLogOnViewModel.TanentId = requestCarrier.TanentId;
                    string validateError = this._userMapper.ValidateUserLogOn(userLogOnViewModel);
                    if (string.IsNullOrEmpty(validateError))
                    {
                        var userModel = this._userMapper.Map(userLogOnViewModel);
                        //userModel.CreatedBy = (int)requestCarrier.UserId.Value;
                        //userModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                        //userModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                        //userModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                        var result = this._dataServices.UserService.ValidateUserLogOn(userModel);
                        if (result.Success)
                        {
                            var userData = this._userMapper.Map(this._dataServices.UserService.GetUser(result.Id, result.TenantID));
                            response = new ResponseCarrier() { Status = true, PayLoad = userData, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
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

        [Authorize(Roles = "User")]
        [HttpPost]
        public IHttpActionResult UpdateDevice(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var userLogOnViewModel = WebCommon.Instance.GetObject<UserLogonViewModel>(requestCarrier.PayLoad);
                    string validateError = this._userMapper.ValidateUserLogOn(userLogOnViewModel);
                    if (string.IsNullOrEmpty(validateError))
                    {
                        userLogOnViewModel.TanentId = requestCarrier.TanentId;
                        var userModel = this._userMapper.Map(userLogOnViewModel);
                        var result = this._dataServices.UserService.UpdateDeviceId(userModel);

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


        [Authorize(Roles = "User")]
        [HttpPost]
        public IHttpActionResult GetUser(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var userViewModel = WebCommon.Instance.GetObject<UserViewModel>(requestCarrier.PayLoad);
                    // userViewModel = (UserViewModel)requestCarrier.PayLoad;
                    if (userViewModel != null && userViewModel.Id > 0)
                    {
                        userViewModel.TanentId = requestCarrier.TanentId;
                        var userModel = this._dataServices.UserService.GetUser(userViewModel.Id, userViewModel.TanentId);

                        userViewModel = this._userMapper.Map(userModel);
                        response = new ResponseCarrier()
                        {
                            Status = true,
                            PayLoad = userViewModel,
                            ErrorMessage = string.Empty
                        };
                    }
                    else
                    {
                        userViewModel = _viewModelFactory.CreateUserViewModel();
                        response = new ResponseCarrier()
                        {
                            Status = true,
                            PayLoad = userViewModel,
                            ErrorMessage = string.Empty
                        };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid Request. Users Id not provided.", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }


        public IHttpActionResult GetAddUser(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue)
            {
                var userVM = this._userMapper.CreateViewModel();
                userVM.TanentId = requestCarrier.TanentId;
                response = new ResponseCarrier() { Status = true, PayLoad = userVM, TanentId = requestCarrier.TanentId };
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }


        [HttpPost]
        public IHttpActionResult SaveUser(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue)
            {
                var userModel = this._userMapper.Map(WebCommon.Instance.GetObject<UserViewModel>(requestCarrier.PayLoad));
                userModel.CreatedBy = (int)requestCarrier.UserId.Value;
                userModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                userModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                userModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                userModel.TenantID = requestCarrier.TanentId;
                string validationResponse = this._userMapper.ValidateUser(userModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    var userResponse = _dataServices.UserService.SaveUser(userModel);
                    if (userResponse.Success)
                    {
                        response = new ResponseCarrier() { Status = true, TanentId = requestCarrier.TanentId, PayLoad = userResponse };
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
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UrerId not provided (in case of user id please provide -1).", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IHttpActionResult GetMappedDoctorForUser(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {

                //string code = requestCarrier.PayLoad.ToString();
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


        [Authorize(Roles = "User")]
        [HttpPost]
        public IHttpActionResult GetOTP(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue)
            {

                var otpModel = this._otpMapper.Map(WebCommon.Instance.GetObject<otpViewmodel>(requestCarrier.PayLoad));
                otpModel.CreatedBy = (int)requestCarrier.UserId.Value;
                otpModel.UserId = (int)requestCarrier.UserId.Value;
                otpModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                otpModel.otpCode = WebCommon.Instance.GetOTP();
                this._dataServices.UserService.SaveOTP(otpModel);
                UserModel userInfo = null;
                if (otpModel.Success)
                {
                    if (WebCommon.NotificationEnabled)
                    {
                        userInfo = this._dataServices.UserService.GetUserByPhoneNo(otpModel.otpMobileNo, requestCarrier.TanentId);
                        userInfo.From = Convert.ToInt16(requestCarrier.From);
                        if (userInfo.Success)
                        {
                            List<object> additionalParameters = new List<object>();
                            additionalParameters.Add(userInfo.Email);
                            additionalParameters.Add(userInfo.PhoneNumber);
                            additionalParameters.Add(otpModel.otpCode);
                            additionalParameters.Add(userInfo.DeviceId);
                            NotificationClient.Instance.SendMessage(null, ImTech.Notification.Messages.MessageType.User_OTP, additionalParameters);
                        }
                    }

                }
                response = new ResponseCarrier() { Status = userInfo.Success, PayLoad = null, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom  or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IHttpActionResult ValidateOTP(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                var otpModel = this._otpMapper.Map(WebCommon.Instance.GetObject<otpViewmodel>(requestCarrier.PayLoad));
                otpModel.CreatedBy = (int)requestCarrier.UserId.Value;
                otpModel.UserId = (int)requestCarrier.UserId.Value;
                otpModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                this._dataServices.UserService.ValidateOTP(otpModel);
                if (otpModel.Success)
                {
                    if (WebCommon.NotificationEnabled)
                    {
                        var userInfo = this._dataServices.UserService.GetUserByPhoneNo(otpModel.otpMobileNo, requestCarrier.TanentId);
                        if (userInfo.Success)
                        {
                            List<object> additionalParameters = new List<object>();
                            additionalParameters.Add(userInfo.Email);
                            additionalParameters.Add(userInfo.PhoneNumber);
                            additionalParameters.Add(userInfo.FirstName + " " + userInfo.LastName);
                            NotificationClient.Instance.SendMessage(null, ImTech.Notification.Messages.MessageType.User_Registration, additionalParameters);
                        }
                    }

                }
                response = new ResponseCarrier() { Status = otpModel.Success, PayLoad = null, ErrorMessage = otpModel.ErrorMessage, TanentId = requestCarrier.TanentId };
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "TanentId or RequestFrom or UserId not provided.", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IHttpActionResult GetMyConsultations(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
                {
                    Int64 userId = requestCarrier.UserId.Value;
                    var result = _dataServices.UserService.GetConsultations(userId, requestCarrier.TanentId);
                    var lstConsultationViewModel = _userMapper.MapUserConsultation(result);
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

        [Authorize(Roles = "User")]
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
                    var messageModel = _userMapper.MapUserMessage(dViewModel);
                    var result = _dataServices.UserService.SaveMessage(messageModel, requestCarrier.TanentId);
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

        [Authorize(Roles = "User")]
        [HttpPost]
        public IHttpActionResult ChangeUserPassword(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var changePasswordViewModel = WebCommon.Instance.GetObject<ChangePasswordViewModel>(requestCarrier.PayLoad);
                    changePasswordViewModel.TanentId = requestCarrier.TanentId;
                    string validateError = this._userMapper.ValidateUserChangePasswordOn(changePasswordViewModel);
                    if (string.IsNullOrEmpty(validateError))
                    {
                        string encryptedPassword = Common.SecurityManager.EncryptText(changePasswordViewModel.NewPassword);
                        changePasswordViewModel.NewHashPassword = encryptedPassword;
                        var changePassword = this._userMapper.MapChangePassword(changePasswordViewModel);
                        changePassword.CreatedBy = (int)requestCarrier.UserId.Value;
                        changePassword.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                        changePassword.ModifiedBy = (int)requestCarrier.UserId.Value;
                        changePassword.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                        var result = this._dataServices.UserService.ChangeUserPassword(changePassword);
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
        [HttpPost]
        public IHttpActionResult ForgotUserPassword(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.PayLoad != null)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var changePasswordViewModel = WebCommon.Instance.GetObject<ChangePasswordViewModel>(requestCarrier.PayLoad);
                    changePasswordViewModel.TanentId = requestCarrier.TanentId;
                    string validateError = this._userMapper.ValidateUserChangePasswordOn(changePasswordViewModel);
                    if (string.IsNullOrEmpty(validateError))
                    {
                        var changePassword = this._userMapper.MapChangePassword(changePasswordViewModel);
                        changePassword.CreatedBy = (int)requestCarrier.UserId.Value;
                        changePassword.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                        changePassword.ModifiedBy = (int)requestCarrier.UserId.Value;
                        changePassword.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                        changePassword.NewPassword = ConfigurationManager.DefaultPassword;
                        var result = this._dataServices.UserService.ValidateUserEmail(changePassword);
                        if (result.Success)
                        {
                            if (WebCommon.NotificationEnabled)
                            {
                                if (result != null)
                                {
                                    List<object> additionalParameters = new List<object>();
                                    additionalParameters.Add(result.Email);
                                    additionalParameters.Add(result.PhoneNumber);
                                    additionalParameters.Add(changePassword.NewPassword);
                                    NotificationClient.Instance.SendMessage(null, ImTech.Notification.Messages.MessageType.Forgot_Password, additionalParameters);
                                }
                            }

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

        [Authorize(Roles = "User")]
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
    }
}