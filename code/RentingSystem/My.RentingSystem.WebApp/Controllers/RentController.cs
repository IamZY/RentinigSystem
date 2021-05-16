using My.RentingSystem.Model.domain;
using My.RentingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace My.RentingSystem.WebApp.Controllers
{
    public class RentController : BaseController
    {
        //
        // GET: /Rent/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RentHouse() 
        {
            //if (Session["city"] != null) 
            //{
            //    string city = Session["city"].ToString();
            //    DataDictionaryService.LoadEntities(d => d.dkey == "region" && d.dvalue2 == city);
            //}
            Session["city"] = Session["rentCity"];
            return View();
        }

        public ActionResult partialRentHouse() 
        {
            if (Session["houseDic"] != null)
            {
                ViewBag.houseDic = Session["houseDic"];
                Session["houseDic"] = null;
                return View();
            }

            string city = Session["rentCity"].ToString();

            Dictionary<string, List<HousePicture>> houseDic = new Dictionary<string, List<HousePicture>>();
            // 出租房 type = 1 出租 state = 0 未出售  audit 房屋审核通过允许上架
            List<House> houseList = HouseService.LoadEntities(t => t.type == "1" && t.state == "0" && t.audit == "1" && t.city == city).ToList();
            List<HousePicture> hps = newHousePicture(houseList);

            //Console.WriteLine(houseList);
            houseDic.Add("rent", hps);

            List<DataDictionary> datas = DataDictionaryService.LoadEntities(t => t.dkey == "region").ToList();

            

            ViewBag.houseDic = houseDic;
            ViewBag.dataDic = datas;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="rent"></param>
        /// <param name="area"></param>
        /// <param name="unitType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RentHouse(string region, string rent, string area, string unitType, string order, string orderPrice) 
        {
            Session["houseDic"] = null;
            Dictionary<string, List<HousePicture>> houseDic = new Dictionary<string, List<HousePicture>>();

            string city = Session["rentCity"].ToString();
            List<House> houseList = HouseService.LoadEntities(t => t.type == "1" && t.state == "0" && t.audit == "1" && t.city == city).ToList();
            if (region != "不限")
            {
                houseList = houseList.Where(t => t.region == region).ToList();
            }
            

            if (rent != "不限") 
            {
                if(rent.IndexOf("以上") != -1)
                {
                    houseList = houseList.Where(t => t.price > 3000).ToList();
                }

                if (rent.IndexOf("-") != -1) 
                {
                    int lowPrice = Convert.ToInt32(rent.Split(new char[] { '-' })[0]);
                    int highPrice = Convert.ToInt32(rent.Split(new char[] { '-' })[1]);
                    houseList = houseList.Where(t => t.price > lowPrice & t.price <= highPrice).ToList();
                }
            }

            if(area != "不限")
            {
                if (area.IndexOf("平方以上") != -1) 
                {
                    houseList = houseList.Where(t => t.area > 100).ToList();
                }

                if (area.IndexOf("-") != -1) 
                {
                    int lowArea = Convert.ToInt32(area.Split(new char[] { '-' })[0]);
                    int highArea = Convert.ToInt32(area.Split(new char[] { '-' })[1].Substring(0, area.Split(new char[] { '-' })[1].Length - 2));  // 200平方
                    houseList = houseList.Where(t => t.area > lowArea && t.area <= highArea).ToList();
                }

            }

            if (unitType != "不限") 
            {
                houseList = houseList.Where(t => t.unitType == unitType).ToList();
            }
            

            // 1001-2000  40-80平方

            // 排序
            if (order == "价格" && orderPrice == "asc") 
            {
                houseList = houseList.OrderBy(h => h.price).ToList();
            }
            else if (order == "价格" && orderPrice == "desc")
            {
                houseList = houseList.OrderByDescending(h => h.price).ToList();
            }
            else if (order == "最新") 
            {
                houseList = houseList.OrderByDescending(h => h.time).ToList();
            }


            List<HousePicture> hps = newHousePicture(houseList);
            houseDic.Add("rent", hps);
            ViewBag.houseDic = houseDic;
            Session["houseDic"] = houseDic;
            //Console.WriteLine(rent);
            //return View();
            return RedirectToAction("partialRentHouse");
        }

/// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult NewHouse()
        {
            Session["city"] = Session["rentCity"];
            return View();
        }


        public ActionResult partialNewHouse()
        {
            if (Session["houseDic"] != null)
            {
                ViewBag.houseDic = Session["houseDic"];
                Session["houseDic"] = null;
                return View();
            }
            string city = Session["rentCity"].ToString();
            Dictionary<string, List<HousePicture>> houseDic = new Dictionary<string, List<HousePicture>>();
            // 出租房
            List<House> houseList = HouseService.LoadEntities(t => t.type == "2" && t.state == "0" && t.audit == "1" && t.city == city).ToList();
            List<HousePicture> hps = newHousePicture(houseList);

            //Console.WriteLine(houseList);
            houseDic.Add("new", hps);

            List<DataDictionary> datas = DataDictionaryService.LoadEntities(t => t.dkey == "region").ToList();

            ViewBag.houseDic = houseDic;
            ViewBag.dataDic = datas;
            return View();
        }


        [HttpPost]
        public ActionResult NewHouse(string region, string rent, string area, string unitType, string order, string orderPrice)
        {
            Session["houseDic"] = null;
            Dictionary<string, List<HousePicture>> houseDic = new Dictionary<string, List<HousePicture>>();

            string city = Session["rentCity"].ToString();
            List<House> houseList = HouseService.LoadEntities(t => t.type == "2" && t.state == "0" && t.audit == "1" && t.city == city).ToList();
            if (region != "不限")
            {
                houseList = houseList.Where(t => t.region == region).ToList();
            }


            if (rent != "不限")
            {
                if (rent.IndexOf("以上") != -1)
                {
                    houseList = houseList.Where(t => t.price > 3000).ToList();
                }

                if (rent.IndexOf("-") != -1)
                {
                    int lowPrice = Convert.ToInt32(rent.Split(new char[] { '-' })[0]);
                    int highPrice = Convert.ToInt32(rent.Split(new char[] { '-' })[1]);
                    houseList = houseList.Where(t => t.price > lowPrice & t.price <= highPrice).ToList();
                }
            }

            if (area != "不限")
            {
                if (area.IndexOf("平方以上") != -1)
                {
                    houseList = houseList.Where(t => t.area > 100).ToList();
                }

                if (area.IndexOf("-") != -1)
                {
                    int lowArea = Convert.ToInt32(area.Split(new char[] { '-' })[0]);
                    int highArea = Convert.ToInt32(area.Split(new char[] { '-' })[1].Substring(0, area.Split(new char[] { '-' })[1].Length - 2));  // 200平方
                    houseList = houseList.Where(t => t.area > lowArea && t.area <= highArea).ToList();
                }

            }

            if (unitType != "不限")
            {
                houseList = houseList.Where(t => t.unitType == unitType).ToList();
            }


            // 1001-2000  40-80平方
            // 排序
            if (order == "价格" && orderPrice == "asc")
            {
                houseList = houseList.OrderBy(h => h.price).ToList();
            }
            else if (order == "价格" && orderPrice == "desc")
            {
                houseList = houseList.OrderByDescending(h => h.price).ToList();
            }
            else if (order == "最新")
            {
                houseList = houseList.OrderByDescending(h => h.time).ToList();
            }

            List<HousePicture> hps = newHousePicture(houseList);
            houseDic.Add("new", hps);
            ViewBag.houseDic = houseDic;
            Session["houseDic"] = houseDic;
            //Console.WriteLine(rent);
            //return View();
            return RedirectToAction("partialNewHouse");
        }

/// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public ActionResult SecondHouse()
        {
            Session["city"] = Session["rentCity"];
            return View();
        }

        public ActionResult partialSecondHouse()
        {
            if (Session["houseDic"] != null)
            {
                ViewBag.houseDic = Session["houseDic"];
                Session["houseDic"] = null;
                return View();
            }
            string city = Session["rentCity"].ToString();
            Dictionary<string, List<HousePicture>> houseDic = new Dictionary<string, List<HousePicture>>();
            // 出租房
            List<House> houseList = HouseService.LoadEntities(t => t.type == "3" && t.state == "0" && t.audit == "1" && t.city == city).ToList();
            List<HousePicture> hps = newHousePicture(houseList);

            //Console.WriteLine(houseList);
            houseDic.Add("second", hps);

            List<DataDictionary> datas = DataDictionaryService.LoadEntities(t => t.dkey == "region").ToList();

            ViewBag.houseDic = houseDic;
            ViewBag.dataDic = datas;
            return View();
        }


        [HttpPost]
        public ActionResult SecondHouse(string region, string rent, string area, string unitType, string order, string orderPrice)
        {
            Session["houseDic"] = null;
            Dictionary<string, List<HousePicture>> houseDic = new Dictionary<string, List<HousePicture>>();

            string city = Session["rentCity"].ToString();
            List<House> houseList = HouseService.LoadEntities(t => t.type == "3" && t.state == "0" && t.audit == "1" && t.city == city).ToList();
            if (region != "不限")
            {
                houseList = houseList.Where(t => t.region == region).ToList();
            }


            if (rent != "不限")
            {
                if (rent.IndexOf("以上") != -1)
                {
                    houseList = houseList.Where(t => t.price > 3000).ToList();
                }

                if (rent.IndexOf("-") != -1)
                {
                    int lowPrice = Convert.ToInt32(rent.Split(new char[] { '-' })[0]);
                    int highPrice = Convert.ToInt32(rent.Split(new char[] { '-' })[1]);
                    houseList = houseList.Where(t => t.price > lowPrice & t.price <= highPrice).ToList();
                }
            }

            if (area != "不限")
            {
                if (area.IndexOf("平方以上") != -1)
                {
                    houseList = houseList.Where(t => t.area > 100).ToList();
                }

                if (area.IndexOf("-") != -1)
                {
                    int lowArea = Convert.ToInt32(area.Split(new char[] { '-' })[0]);
                    int highArea = Convert.ToInt32(area.Split(new char[] { '-' })[1].Substring(0, area.Split(new char[] { '-' })[1].Length - 2));  // 200平方
                    houseList = houseList.Where(t => t.area > lowArea && t.area <= highArea).ToList();
                }

            }

            if (unitType != "不限")
            {
                houseList = houseList.Where(t => t.unitType == unitType).ToList();
            }


            // 1001-2000  40-80平方
            // 排序
            if (order == "价格" && orderPrice == "asc")
            {
                houseList = houseList.OrderBy(h => h.price).ToList();
            }
            else if (order == "价格" && orderPrice == "desc")
            {
                houseList = houseList.OrderByDescending(h => h.price).ToList();
            }
            else if (order == "最新")
            {
                houseList = houseList.OrderByDescending(h => h.time).ToList();
            }

            List<HousePicture> hps = newHousePicture(houseList);
            houseDic.Add("second", hps);
            ViewBag.houseDic = houseDic;
            Session["houseDic"] = houseDic;
            //Console.WriteLine(rent);
            //return View();
            return RedirectToAction("partialSecondHouse");
        }

/// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public ActionResult HouseInfo(string id,int flag = 0) 
        {
            int idInt = Convert.ToInt32(id);
            
            House h = HouseService.LoadEntities(t => t.hid == idInt).FirstOrDefault();

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


            List<HouseMessage> hmList = HouseMessageService.LoadEntities(m => m.hid == h.hid).ToList();

            List<HouseMessageEntity> entities = new List<HouseMessageEntity>();

            foreach (HouseMessage item in hmList) 
            {
                User user = UserService.LoadEntities(u => u.uid == item.uid).FirstOrDefault();
                Message message = MessageService.LoadEntities(m => m.mid == item.mid).FirstOrDefault();
                entities.Add(new HouseMessageEntity { 
                    user = user,
                    message = message
                });
            }

            ViewBag.detail = flag;
            ViewBag.house = hp;
            ViewBag.hm = entities;
            return View();
        }


        public JsonResult LeaveMessage(string hid,string msg)
        {

            int hidInt = Convert.ToInt32(hid);
            User user = Session["user"] as My.RentingSystem.Model.User;

            DateTime dt = DateTime.Now;
            string date = dt.ToString();

            Message message = MessageService.AddEntity(new Message { 
                message1 = msg,
                time = date
            });

            HouseMessage hm = HouseMessageService.AddEntity(new HouseMessage { 
                hid = hidInt,
                mid = message.mid,
                uid = user.uid
            });


            return Json("success", JsonRequestBehavior.AllowGet);
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
