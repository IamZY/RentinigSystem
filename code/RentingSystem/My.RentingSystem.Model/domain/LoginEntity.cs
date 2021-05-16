using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.RentingSystem.Model.domain
{
    public class LoginEntity
    {
        public string msg { get; set; }
        public string code { get; set; }
        public User user { get; set; }
        public string avatar { get; set; }
        public UserAuth userAuth { get; set; }
    }
}
