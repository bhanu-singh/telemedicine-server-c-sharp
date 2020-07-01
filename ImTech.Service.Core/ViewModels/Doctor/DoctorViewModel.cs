using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using ImTech.Model;

namespace ImTech.Service.ViewModels
{
    public class DoctorViewModel : ViewModelBase
    {
        public DoctorViewModel()
        {

        }

        public DoctorViewModel(IEnumerable<Country> countryModelCollection, IEnumerable<State> stateModelCollection,
            IEnumerable<City> cityModelCollection, IEnumerable<HospitalModel> hospitalModelCollection, IEnumerable<DegreeModel> degreeModelCollection,
            IEnumerable<SpecializationModel> specializationModelCollection, IEnumerable<DeseaseModel> deseasesModelCollection)
        {
            this.CountryList = countryModelCollection.ToList();
            this.StateList = stateModelCollection.ToList();
            this.CityList = cityModelCollection.ToList();
            this.HospitalList = hospitalModelCollection.ToList();
            this.DegreeList = degreeModelCollection.ToList();
            this.SpecializationList = specializationModelCollection.ToList();
            this.DeseaseList = deseasesModelCollection.ToList();
        }

        public List<Country> CountryList { get; set; }
        public List<State> StateList { get; set; }
        public List<City> CityList { get; set; }
        public List<DegreeModel> DegreeList { get; set; }
        public List<HospitalModel> HospitalList { get; set; }
        public List<SpecializationModel> SpecializationList { get; set; }
        public List<DeseaseModel> DeseaseList { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Address1 { get; set; }

        public int CityID { get; set; }

        public List<int> DoctorDegree { get; set; }

        public List<int> DoctorDesease { get; set; }

        public List<int> DoctorSpecialization { get; set; }

        public List<int> DoctorHospital { get; set; }

        public string Password { get; set; }

        public string Pincode { get; set; }

        public string PhoneNumber { get; set; }

        public string OtherInformation { get; set; }

        public string RegistrationNumber { get; set; }

        public int ProfilePhotoID { get; set; }

        public decimal PersonalConsultationFee { get; set; }

        public decimal PhoneConsultationFee { get; set; }

        public decimal TextConsultationFee { get; set; }

        public decimal VideoConsultationFee { get; set; }

        public string DoctorDescription { get; set; }

        public string DoctorCode { get; set; }


    }
}