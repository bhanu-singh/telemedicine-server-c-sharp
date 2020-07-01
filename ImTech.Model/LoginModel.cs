using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class LoginModel : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string HashPassword { get; set; }
        public Int16 RoleId { get; set; }
    }
}
