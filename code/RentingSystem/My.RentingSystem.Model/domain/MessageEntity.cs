using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.RentingSystem.Model.domain
{
    public class MessageEntity : House
    {
        public int hmid { get; set; }
        public string username { get; set; }
        public string message { get; set; }
        public string time { get; set; }
    }
}
