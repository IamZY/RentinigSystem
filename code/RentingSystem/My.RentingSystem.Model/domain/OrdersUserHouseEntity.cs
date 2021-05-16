using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.RentingSystem.Model.domain
{
    public class OrdersUserHouseEntity : Orders
    {
        public int ouhid { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public int hid { get; set; }
        public string community { get; set; }
        public int area { get; set; }
        public string unitType { get; set; }
        public string floor { get; set; }
    }
}
