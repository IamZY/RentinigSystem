using My.RentingSystem.Model;
using My.RentingSystem.Model.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace My.RentingSystem.WebApp.Controllers
{
    public class OrdersController : BaseController
    {
        //
        // GET: /Order/

        public JsonResult Index()
        {
            if (Session["user"] == null) 
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            } // 

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Order(string id) 
        {
            // if(Session["user"]) {}
            int hid = Convert.ToInt32(id);

            House house = HouseService.LoadEntities(h => h.hid == hid).FirstOrDefault();

            @ViewBag.house = house;
            return View();
        }

        public ActionResult MyOrder(string id) 
        {
            @ViewBag.uid = id;
            return View();
        }

        /// <summary>
        /// 个人订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MyOrderPartial(string id) 
        {
            // userid = id
            int uid = Convert.ToInt32(id);

            //My.RentingSystem.Model.User user = Session["user"] as My.RentingSystem.Model.User;

            List<OrdersUserHouse> ouhList = OrdersUserHouseService.LoadEntities(o => o.uid == uid).ToList();
            List<House> houseList = new List<House>();
            List<Orders> ordersList = new List<Orders>();
            foreach(OrdersUserHouse ouh in ouhList)
            {
                houseList.Add(HouseService.LoadEntities(h => h.hid == ouh.hid).FirstOrDefault());
                ordersList.Add(OrdersService.LoadEntities(o => o.oid == ouh.oid).FirstOrDefault());
            }

            List<HousePicture> hps = newHousePicture(houseList);

            ViewBag.hps = hps;
            ViewBag.orders = ordersList;

            return View(); 
        }


        
        public JsonResult PlaceOrder(string id, string duration)
        {
            int hid = Convert.ToInt32(id);
            House house = HouseService.LoadEntities(h => h.hid == hid).FirstOrDefault();

            // 服务费
            DataDictionary servicePrice = DataDictionaryService.LoadEntities(t => t.dkey == "service").FirstOrDefault();

            int rent = Convert.ToInt32(duration) * house.price + Convert.ToInt32(servicePrice.dvalue);

            DateTime df = DateTime.Now;
            string now = df.ToString();

            Orders newOrder = OrdersService.AddEntity(new Orders {
                duration = Convert.ToInt32(duration),
                rent = rent,
                state = "0",
                time = now
            });

            User user = Session["user"] as My.RentingSystem.Model.User;


            // 
            OrdersUserHouse newOrderUserHouse = OrdersUserHouseService.AddEntity(new OrdersUserHouse { 
                oid = newOrder.oid,
                uid = user.uid,
                hid = hid,
                pubTime = df.ToString("yyyyMMdd")
            });

            // 更新房屋状态state=1
            house.state = "1";
            HouseService.EditEntity(house);
            Session["city"] = Session["rentCity"];

            if (newOrder == null || newOrderUserHouse == null)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult PayOrder(string id) 
        {
            int oid = Convert.ToInt32(id);

            Orders orders = OrdersService.LoadEntities(o=>o.oid == oid).FirstOrDefault();
            orders.state = "1";
            OrdersService.EditEntity(orders);

            My.RentingSystem.Model.User user = Session["user"] as My.RentingSystem.Model.User;
            @ViewBag.uid = user.uid;
            return View("MyOrder", new { id=user.uid });
        }


        public ActionResult CancelOrder(string oid, string hid) 
        {

            int oidInt = Convert.ToInt32(oid);
            int hidInt = Convert.ToInt32(hid);

            Orders order = OrdersService.LoadEntities(o => o.oid == oidInt).FirstOrDefault();
            House house = HouseService.LoadEntities(h => h.hid == hidInt).FirstOrDefault();
            house.state = "0";
            OrdersUserHouse ordersUserHouse = OrdersUserHouseService.LoadEntities(o => o.oid == oidInt).FirstOrDefault();

            OrdersService.DeleteEntity(order);
            HouseService.EditEntity(house);
            OrdersUserHouseService.DeleteEntity(ordersUserHouse);

            My.RentingSystem.Model.User user = Session["user"] as My.RentingSystem.Model.User;
            @ViewBag.uid = user.uid;
            return View("MyOrder", new { id = user.uid });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="houseList"></param>
        /// <returns></returns>
        private List<HousePicture> newHousePicture(List<House> houseList)
        {
            List<HousePicture> hps = new List<HousePicture>();


            foreach (House h in houseList)
            {
                HousePicture hp = new HousePicture
                {
                    hid = h.hid,
                    area = h.area,
                    price = h.price,
                    pay = h.pay,
                    unitType = h.unitType,
                    floor = h.floor,
                    community = h.community,
                    pubPerson = h.pubPerson,
                    time = h.time,
                    type = h.type,
                    isRecommend = h.isRecommend,
                    houseDesc = h.houseDesc,
                    region = h.region
                };
                hp.pics = PictureService.LoadEntities(p => p.hid == h.hid).ToList();
                hps.Add(hp);
            }
            return hps;
        }


    }
}
