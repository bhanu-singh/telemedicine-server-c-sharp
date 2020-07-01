using ImTech.Service.ViewModels.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels.Doctor
{
    public class DoctorShortViewModel : ViewModelBase
    {
        public DoctorShortViewModel()
        {

        }

        public IEnumerable<DegreeViewModel> DoctorDegrees { get; set; }
        public IEnumerable<HospitalViewModel> DoctorHospitals { get; set; }
        public IEnumerable<SpecializationViewModel> DoctorSpecializations { get; set; }
        public IEnumerable<DeseasesViewModel> DoctorDeseases { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Address1 { get; set; }

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

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }
    }
}