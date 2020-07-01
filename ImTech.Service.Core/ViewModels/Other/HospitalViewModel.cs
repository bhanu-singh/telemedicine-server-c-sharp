using ImTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels.Other
{
    public class HospitalViewModel : ViewModelBase
    {


        public HospitalViewModel()
        {

        }

        public HospitalViewModel(IEnumerable<Country> countryModelCollection, IEnumerable<State> stateModelCollection,
            IEnumerable<City> cityModelCollection)
        {
            this.CountryList = countryModelCollection.ToList();
            this.StateList = stateModelCollection.ToList();
            this.CityList = cityModelCollection.ToList();
        }

        public List<Country> CountryList { get; set; }
        public List<State> StateList { get; set; }
        public List<City> CityList { get; set; }
        public string HospitalName { get; set; }

        public string HospitalAddress1 { get; set; }

        public string HospitalCity { get; set; }

        public string HospitalState { get; set; }

        public string HospitalCountry { get; set; }

        public string HospitalPhone1 { get; set; }

        public string HospitalPhone2 { get; set; }

        public string HospitalFax { get; set; }

        public string HospitalEmail { get; set; }

        public string HospitalCode { get; set; }

        public long StateId { get; set; }

        public long CityId { get; set; }
    }
}