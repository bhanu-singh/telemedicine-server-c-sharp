using ImTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        public UserViewModel()
        {
        }

        public UserViewModel(IEnumerable<Country> countryModelCollection, IEnumerable<State> stateModelCollection,
           IEnumerable<City> cityModelCollection)
        {
            this.CountryList = countryModelCollection.ToList();
            this.StateList = stateModelCollection.ToList();
            this.CityList = cityModelCollection.ToList();
        }

        public List<Country> CountryList { get; set; }

        public List<State> StateList { get; set; }

        public List<City> CityList { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPhoneNoVerified { get; set; }

        public string DeviceId { get; set; }
    }
}