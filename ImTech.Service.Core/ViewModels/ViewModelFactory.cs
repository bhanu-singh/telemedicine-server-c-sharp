using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImTech.DataServices;
using ImTech.Service.ViewModels.Other;

namespace ImTech.Service.ViewModels
{
    public class ViewModelFactory
    {
        private DataServices.DataServices dataServices;
        private StaticDataServices staticDataServices;

        public ViewModelFactory(DataServices.DataServices dataServices, StaticDataServices staticDataServices)
        {
            this.dataServices = dataServices;
            this.staticDataServices = staticDataServices;
        }
        public ResponseCarrier CreateResponseCarrier()
        {
            return new ResponseCarrier();
        }

        public RequestCarrier CreateRequestCarrier()
        {
            return new RequestCarrier();
        }

        public UserViewModel CreateUserViewModel()
        {
            return new UserViewModel(staticDataServices.GetCountry(), staticDataServices.GetState(), staticDataServices.GetCity());
        }

        public DoctorViewModel CreateDoctorViewModel()
        {
            return new DoctorViewModel(staticDataServices.GetCountry(), staticDataServices.GetState(), staticDataServices.GetCity(), staticDataServices.GetHospital(), staticDataServices.GetDegree(), staticDataServices.GetSpecialization(), staticDataServices.GetDesease());
        }

        public HospitalViewModel CreateHospitalViewModel()
        {
            return new HospitalViewModel(staticDataServices.GetCountry(), staticDataServices.GetState(), staticDataServices.GetCity());
        }



        public SecureFileViewModel CreateSecureFileViewModel()
        {
            return new SecureFileViewModel();
        }

        public NonSecureFileViewModel CreateNonSecureFileViewModel()
        {
            return new NonSecureFileViewModel();
        }
    }
}