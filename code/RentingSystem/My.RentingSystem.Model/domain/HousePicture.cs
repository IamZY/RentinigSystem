using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.RentingSystem.Model.domain
{
    public class HousePicture
    {
        public int hid { get; set; }
        public int area { get; set; }
        public int price { get; set; }
        public string pay { get; set; }
        public string unitType { get; set; }
        public string floor { get; set; }
        public string community { get; set; }
        public string pubPerson { get; set; }
        public string time { get; set; }
        public string type { get; set; }
        public string isRecommend { get; set; }
        public string houseDesc { get; set; }
        public string region { get; set; }
        public string audit { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string state { get; set; }
        public List<Picture> pics { get; set; }
    }
}
