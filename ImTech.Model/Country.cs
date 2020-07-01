using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class Country : BaseModel
    {
        public string CountryName { get; set; }
    }

    public class State : BaseModel
    {
        public string StateName { get; set; }
        public int CountryId { get; set; }
    }

    public class City : BaseModel
    {
        public string CityName { get; set; }
        public int StateId { get; set; }
    }

}
