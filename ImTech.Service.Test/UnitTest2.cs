using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImTech.DataBase;
using ImTech.DataServices;
using ImTech.Service.ViewModels;
using ImTech.Service.Mapper;
using ImTech.Service.Controllers;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace ImTech.Service.Test
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestGetUser()
        {
            IDataBaseService iDataBaseService = new DataBaseService();
            UserService uService = new UserService(iDataBaseService);
            StaticDataServices staticDataService = new StaticDataServices(iDataBaseService);
            ILogger logger = new Logger(iDataBaseService);
            DataServices.DataServices dServ = new DataServices.DataServices(iDataBaseService);
            ViewModelFactory factory = new ViewModelFactory(dServ, staticDataService);
            UserMapper uMapper = new UserMapper(factory);
            OtpMapper oMapper = new OtpMapper(factory);
            DoctorMapper doctorMapper = new DoctorMapper(factory);
            UserController uController = new UserController(staticDataService, dServ, factory, uMapper, doctorMapper,oMapper,logger);

            uController.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/User/GetUser")
            };

            uController.Configuration = new HttpConfiguration();
            uController.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            uController.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "user" } });

            RequestCarrier req = new RequestCarrier();
            req.From = "Test";
            req.TanentId = -1;
            UserViewModel uvModel = new UserViewModel(staticDataService.GetCountry(), staticDataService.GetState(), staticDataService.GetCity());
            uvModel.Id = 3;
            req.PayLoad = uvModel;
            var response = uController.GetUser(req);
        }

        [TestMethod]
        public void TestSaveUser()
        {
            IDataBaseService iDataBaseService = new DataBaseService();
            UserService uService = new UserService(iDataBaseService);
            StaticDataServices staticDataService = new StaticDataServices(iDataBaseService);
            ILogger logger = new Logger(iDataBaseService);
            DataServices.DataServices dServ = new DataServices.DataServices(iDataBaseService);
            ViewModelFactory factory = new ViewModelFactory(dServ, staticDataService);
            UserMapper uMapper = new UserMapper(factory);
            OtpMapper oMapper = new OtpMapper(factory);
            DoctorMapper doctorMapper = new DoctorMapper(factory);
            UserController uController = new UserController(staticDataService, dServ, factory, uMapper, doctorMapper, oMapper, logger);

            uController.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/User/SaveUser")
            };
            uController.Configuration = new HttpConfiguration();
            uController.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            uController.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "user" } });

            RequestCarrier req = new RequestCarrier();
            req.From = "Test";
            req.TanentId = -1;

            UserViewModel dvModel = new UserViewModel(staticDataService.GetCountry(), staticDataService.GetState(), staticDataService.GetCity());
            dvModel.Id = 6;
            dvModel.Email = "test@medLyte.com";
            dvModel.Password = "admin@123";
            dvModel.FirstName = "Shubham";
            dvModel.LastName = "Joshi";
            dvModel.CreatedByEntity = 1;
            dvModel.PhoneNumber = "8968574635";
            dvModel.IsPhoneNoVerified = false;
            dvModel.IsDeleted = false;
            dvModel.TanentId = -1;
            dvModel.CreatedBy = 1;
            //dvModel.UpdatedBy = 1;
            req.PayLoad = dvModel;
            var response = uController.SaveUser(req);
            Assert.IsNotNull(response);
        }
    }
}
