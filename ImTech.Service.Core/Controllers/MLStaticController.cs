using ImTech.DataServices;
using ImTech.Model;
using ImTech.Service.Mapper;
using ImTech.Service.ViewModels;
using ImTech.Service.ViewModels.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ImTech.Service.Controllers
{
    public class MLStaticController : ApiController
    {
        private readonly StaticDataServices _staticDataServices;
        private DataServices.DataServices _dataServices;
        private DoctorMapper _doctorMapper;
        private ViewModelFactory _viewModelFactory;
        private ILogger _logger;

        public MLStaticController(StaticDataServices staticDataServices, DataServices.DataServices dataServices, ViewModelFactory viewModelFactory, DoctorMapper doctorMapper, ILogger logger)
        {
            _staticDataServices = staticDataServices;
            _dataServices = dataServices;
            _viewModelFactory = viewModelFactory;
            _logger = logger;
            _doctorMapper = doctorMapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult GetHospitals(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                IEnumerable<HospitalModel> hospitals = _staticDataServices.GetHospitalorHospitalsListDetail();
                var result = _doctorMapper.MapDoctorHospital(hospitals);
                response = new ResponseCarrier() { Status = true, PayLoad = result, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Error in fetching Hospital", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult GetHospitalDetail(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                if (requestCarrier.PayLoad != null)
                {
                    long id = 0;
                    if (long.TryParse(requestCarrier.PayLoad.ToString(), out id) && id > 0)
                    {
                        HospitalModel hospital = _staticDataServices.GetHospitalDetail(id);
                        var result = _doctorMapper.MapHospitalDetail(hospital);
                        response = new ResponseCarrier() { Status = true, PayLoad = result, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                    }
                    else if (id == -1)
                    {
                        HospitalModel hospital = new HospitalModel();
                        var result = _doctorMapper.MapHospitalDetail(hospital);
                        response = new ResponseCarrier() { Status = true, PayLoad = result, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
                    }
                    else
                    {
                        response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid Id supplied", TanentId = requestCarrier.TanentId };
                    }
                }
                else
                {
                    response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Invalid Id supplied", TanentId = requestCarrier.TanentId };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Error in fetching Hospital", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult SaveHospitalDetail(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue)
            {
                HospitalViewModel hViewModel = WebCommon.Instance.GetObject<HospitalViewModel>(requestCarrier.PayLoad);
                string validationResponse = this._doctorMapper.ValidateHospital(hViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    hViewModel.TanentId = requestCarrier.TanentId;
                    var hospitalModel = this._doctorMapper.MapHospital(hViewModel);
                    hospitalModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    hospitalModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    hospitalModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    hospitalModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    var userResponse = _staticDataServices.SaveHospital(hospitalModel);
                    if (userResponse.Success)
                    {
                        hViewModel.LongId = userResponse.LongId;
                        response = new ResponseCarrier() { Status = true, PayLoad = hViewModel, TanentId = requestCarrier.TanentId };
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult GetDegrees(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                IEnumerable<DegreeModel> degreeModel = _staticDataServices.GetDegreeModelCollection();
                var result = _doctorMapper.MapDoctorDegree(degreeModel);
                response = new ResponseCarrier() { Status = true, PayLoad = result, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Error in fetching Degree", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult SaveDegree(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue)
            {
                DegreeViewModel dViewModel = WebCommon.Instance.GetObject<DegreeViewModel>(requestCarrier.PayLoad);
                string validationResponse = this._doctorMapper.ValidateDegree(dViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    dViewModel.TanentId = requestCarrier.TanentId;
                    var degreeModel = this._doctorMapper.MapDoctorDegree(dViewModel);
                    degreeModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    degreeModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    degreeModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    degreeModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    var userResponse = _staticDataServices.SaveDegree(degreeModel);
                    if (userResponse.Success)
                    {
                        degreeModel.LongId = userResponse.LongId;
                        response = new ResponseCarrier() { Status = true, PayLoad = degreeModel, TanentId = requestCarrier.TanentId };
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult GetSpecializations(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                IEnumerable<SpecializationModel> specializationModel = _staticDataServices.GetSpecializationModelCollection();
                var result = _doctorMapper.MapDoctorSpecialization(specializationModel);
                response = new ResponseCarrier() { Status = true, PayLoad = result, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Error in fetching Degree", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult SaveSpecializations(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue)
            {
                SpecializationViewModel sViewModel = WebCommon.Instance.GetObject<SpecializationViewModel>(requestCarrier.PayLoad);
                string validationResponse = this._doctorMapper.ValidateSpecialization(sViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    sViewModel.TanentId = requestCarrier.TanentId;
                    var specializationModel = this._doctorMapper.MapDoctorSpecialization(sViewModel);
                    specializationModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    specializationModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    specializationModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    specializationModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    var userResponse = _staticDataServices.SaveSpecialization(specializationModel);
                    if (userResponse.Success)
                    {
                        specializationModel.LongId = userResponse.LongId;
                        response = new ResponseCarrier() { Status = true, PayLoad = specializationModel, TanentId = requestCarrier.TanentId };
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult GetDeseases(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue && requestCarrier.UserId.Value > 0)
            {
                IEnumerable<DeseaseModel> deseasesModel = _staticDataServices.GetDeseaseModelCollection();
                var result = _doctorMapper.MapDoctorDesease(deseasesModel);
                response = new ResponseCarrier() { Status = true, PayLoad = result, ErrorMessage = string.Empty, TanentId = requestCarrier.TanentId };
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "Error in fetching Degree", TanentId = requestCarrier.TanentId };
            }
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult SaveDesease(RequestCarrier requestCarrier)
        {
            ResponseCarrier response;
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty && requestCarrier.UserId.HasValue)
            {
                DeseasesViewModel dViewModel = WebCommon.Instance.GetObject<DeseasesViewModel>(requestCarrier.PayLoad);
                string validationResponse = this._doctorMapper.ValidateDesease(dViewModel);
                if (string.IsNullOrEmpty(validationResponse))
                {
                    dViewModel.TanentId = requestCarrier.TanentId;
                    var deseaseModel = this._doctorMapper.MapDoctorDesease(dViewModel);
                    deseaseModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    deseaseModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    deseaseModel.ModifiedBy = (int)requestCarrier.UserId.Value;
                    deseaseModel.ModifiedByEntity = Convert.ToInt32(requestCarrier.From);
                    var userResponse = _staticDataServices.SaveDesease(deseaseModel);
                    if (userResponse.Success)
                    {
                        deseaseModel.LongId = userResponse.LongId;
                        response = new ResponseCarrier() { Status = true, PayLoad = deseaseModel, TanentId = requestCarrier.TanentId };
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

    }
}
