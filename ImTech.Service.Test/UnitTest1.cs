using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImTech.Service.Controllers;
using ImTech.DataServices;
using ImTech.DataBase;
using ImTech.Service.Mapper;
using ImTech.Service.ViewModels;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using ImTech.Model;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace ImTech.Service.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void TestSaveDoctor()
        {
            IDataBaseService iDataBaseService = new DataBaseService();
            DoctorService dService = new DoctorService(iDataBaseService);
            StaticDataServices staticDataService = new StaticDataServices(iDataBaseService);
            ILogger logger = new Logger(iDataBaseService);
            DataServices.DataServices dServ = new DataServices.DataServices(iDataBaseService);
            ViewModelFactory factory = new ViewModelFactory(dServ, staticDataService);
            DoctorMapper dMapper = new DoctorMapper(factory);
            DoctorController dController = new DoctorController(staticDataService, dServ, factory, dMapper, logger);

            dController.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/Doctor/SaveDoctor")
            };
            dController.Configuration = new HttpConfiguration();
            dController.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            dController.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "doctor" } });

            RequestCarrier req = new RequestCarrier();
            req.From = "Test";
            req.TanentId = -1;

            DoctorViewModel dvModel = new DoctorViewModel(staticDataService.GetCountry(), staticDataService.GetState(), staticDataService.GetCity(), staticDataService.GetHospital(), staticDataService.GetDegree(), staticDataService.GetSpecialization(), staticDataService.GetDesease());
            dvModel.Address1 = "12, DP Pharma Road";
            dvModel.CityID = 1;
            dvModel.CreatedBy = 1;
            dvModel.CreatedByEntity = 1;
            dvModel.DoctorDegree = new List<int>();
            dvModel.DoctorDesease = new List<int>();
            dvModel.DoctorHospital = new List<int>();
            dvModel.DoctorSpecialization = new List<int>();
            dvModel.EmailAddress = "raj@gmail.com";
            dvModel.FirstName = "Reaj";
            dvModel.LastName = "Sharma";
            dvModel.OtherInformation = "Heart Specilist";
            dvModel.PhoneNumber = "8989889889";
            dvModel.Pincode = "411014";
            dvModel.ProfilePhotoID = 1;
            dvModel.RegistrationNumber = "RJ12123";
            dvModel.TanentId = -1;
            //dvModel.UpdatedBy = 1;
            req.PayLoad = dvModel;
            var response = dController.SaveDoctor(req);
            Assert.IsNotNull(response);

        }

        [TestMethod]
        public void TestUpdateDoctor()
        {
            IDataBaseService iDataBaseService = new DataBaseService();
            DoctorService dService = new DoctorService(iDataBaseService);
            StaticDataServices staticDataService = new StaticDataServices(iDataBaseService);
            ILogger logger = new Logger(iDataBaseService);
            DataServices.DataServices dServ = new DataServices.DataServices(iDataBaseService);
            ViewModelFactory factory = new ViewModelFactory(dServ, staticDataService);
            DoctorMapper dMapper = new DoctorMapper(factory);
            DoctorController dController = new DoctorController(staticDataService, dServ, factory, dMapper, logger);

            dController.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/Doctor/SaveDoctor")
            };
            dController.Configuration = new HttpConfiguration();
            dController.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            dController.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "doctor" } });

            RequestCarrier req = new RequestCarrier();
            req.From = "Test";
            req.TanentId = -1;

            DoctorViewModel dvModel = new DoctorViewModel(staticDataService.GetCountry(), staticDataService.GetState(), staticDataService.GetCity(), staticDataService.GetHospital(), staticDataService.GetDegree(), staticDataService.GetSpecialization(), staticDataService.GetDesease());
            dvModel.Id = 8;
            dvModel.Address1 = "12, DP Pharma Road";
            dvModel.CityID = 1;
            dvModel.CreatedBy = 1;
            dvModel.CreatedByEntity = 1;
            dvModel.DoctorDegree = new List<int>();
            dvModel.DoctorDesease = new List<int>();
            dvModel.DoctorHospital = new List<int>();
            dvModel.DoctorSpecialization = new List<int>();
            dvModel.EmailAddress = "raj@gmail.com";
            dvModel.FirstName = "Raj";
            dvModel.LastName = "Sharma";
            dvModel.OtherInformation = "Heart Specilist";
            dvModel.PhoneNumber = "8989889889";
            dvModel.Pincode = "411014";
            dvModel.ProfilePhotoID = 1;
            dvModel.RegistrationNumber = "RJ12123";
            dvModel.TanentId = -1;
            //dvModel.UpdatedBy = 1;
            req.PayLoad = dvModel;
            var response = dController.SaveDoctor(req);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void TestGetDoctor()
        {
            IDataBaseService iDataBaseService = new DataBaseService();
            DoctorService dService = new DoctorService(iDataBaseService);
            StaticDataServices staticDataService = new StaticDataServices(iDataBaseService);
            ILogger logger = new Logger(iDataBaseService);
            DataServices.DataServices dServ = new DataServices.DataServices(iDataBaseService);
            ViewModelFactory factory = new ViewModelFactory(dServ, staticDataService);
            DoctorMapper dMapper = new DoctorMapper(factory);
            DoctorController dController = new DoctorController(staticDataService, dServ, factory, dMapper, logger);

            dController.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/Doctor/GetDoctor")
            };

            dController.Configuration = new HttpConfiguration();
            dController.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            dController.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "doctor" } });

            RequestCarrier req = new RequestCarrier();
            req.From = "Test";
            req.TanentId = -1;
            DoctorViewModel dvModel = new DoctorViewModel(staticDataService.GetCountry(), staticDataService.GetState(), staticDataService.GetCity(), staticDataService.GetHospital(), staticDataService.GetDegree(), staticDataService.GetSpecialization(), staticDataService.GetDesease());
            dvModel.Id = 8;
            req.PayLoad = dvModel;
            var response = dController.GetDoctor(req);

        }

        [TestMethod]
        public void TestStaticService()
        {
            IDataBaseService iDataBaseService = new DataBaseService();
            StaticDataServices staticDataService = new StaticDataServices(iDataBaseService);
            IEnumerable<City> cityList = staticDataService.GetCity();
            IEnumerable<Country> countryList = staticDataService.GetCountry();
            IEnumerable<State> stateList = staticDataService.GetState();
            IEnumerable<HospitalModel> hospitalList = staticDataService.GetHospital();
            IEnumerable<DeseaseModel> deseaseList = staticDataService.GetDesease();
            IEnumerable<SpecializationModel> specializationList = staticDataService.GetSpecialization();
            Assert.IsNotNull(cityList);
            Assert.IsNotNull(countryList);
            Assert.IsNotNull(stateList);
            Assert.IsNotNull(hospitalList);
            Assert.IsNotNull(deseaseList);
            Assert.IsNotNull(specializationList);
        }
    }
}
