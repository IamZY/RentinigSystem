using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.RentingSystem.Model.domain
{
    public class UserAuthEntity : UserAuth
    {
        public int uid { get; set; }
        public string username { get; set; }
        public string house123 { get; set; }
    }
}
