using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class HospitalModel : BaseModel
    {
        public int HospitalID { get; set; }

        public string HospitalName { get; set; }

        public string HospitalAddress1 { get; set; }

        public int HospitalCity { get; set; }

        public int HospitalState { get; set; }

        public int HospitalCountry { get; set; }

        public string HospitalPhone1 { get; set; }

        public string HospitalPhone2 { get; set; }

        public string HospitalFax { get; set; }

        public string HospitalEmail { get; set; }

        public string HospitalCode { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        public long StateId { get; set; }

        public long CityId { get; set; }
    }
}
