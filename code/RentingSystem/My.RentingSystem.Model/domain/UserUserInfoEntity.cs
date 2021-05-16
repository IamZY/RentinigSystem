using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.RentingSystem.Model.domain
{
    public class UserUserInfoEntity
    {
        public int uid { get; set; }
        public int uiid { get; set; }    
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public string phone { get; set; }
        public string isSys { get; set; }
    }
}
