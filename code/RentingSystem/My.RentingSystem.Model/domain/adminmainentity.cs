using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.RentingSystem.Model.domain
{
    public class AdminMainEntity
    {
        public int userToday { get; set; }
        public int userMonth { get; set; }
        public int userHis { get; set; }
        public int orderToday { get; set; }
        public int orderMonth { get; set; }
        public int orderHis { get; set; }
        public int waitOrder { get; set; }
        public int placedOrder { get; set; }
        public int houseToday { get; set; }
        public int houseMonth { get; set; }
        public int houseHis { get; set; }
        public List<string> userChartX { get; set; }
        public List<int> userChartY { get; set; }
        public List<string> houseDicX { get; set; }
        public List<int> houseDicY { get; set; }
        public List<HousePie> housePie { get; set; }
        public List<string> provinces { get; set; }
    }
}
