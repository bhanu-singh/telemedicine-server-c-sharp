using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class DoctorModel : BaseModel
    {
        public DoctorModel()
        {
            DoctorDegreeList = new List<DegreeModel>();
            DoctorDeseaseList = new List<DeseaseModel>();
            DoctorHospitalList = new List<HospitalModel>();
            DoctorSpecialzationList = new List<SpecializationModel>();
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Address1 { get; set; }

        public int CityID { get; set; }

        public string Pincode { get; set; }

        public string PhoneNumber { get; set; }

        public string OtherInformation { get; set; }

        public string RegistrationNumber { get; set; }

        public int ProfilePhotoID { get; set; }

        public IEnumerable<int> DoctorDegree { get; set; }

        public IEnumerable<int> DoctorDesease { get; set; }

        public IEnumerable<int> DoctorSpecialization { get; set; }

        public IEnumerable<int> DoctorHospital { get; set; }

        public decimal PersonalConsultationFee { get; set; }

        public decimal PhoneConsultationFee { get; set; }

        public string Password { get; set; }

        public string HashPassword { get; set; }

        public decimal TextConsultationFee { get; set; }

        public decimal VideoConsultationFee { get; set; }

        public string DoctorDescription { get; set; }

        public string DoctorCode { get; set; }

        #region Property For Doctor Short View Model
        /// <summary>
        /// Below Properties used only when need specific 
        /// doctor data and should always be mapped with Doctor Short View Model
        /// *Never Map this property in DoctorView Model
        /// </summary>
        public List<DegreeModel> DoctorDegreeList { get; set; }

        public List<DeseaseModel> DoctorDeseaseList { get; set; }

        public List<SpecializationModel> DoctorSpecialzationList { get; set; }

        public List<HospitalModel> DoctorHospitalList { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        #endregion
    }
}
