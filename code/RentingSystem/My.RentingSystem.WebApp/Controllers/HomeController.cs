using My.RentingSystem.Model;
using My.RentingSystem.Model.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace My.RentingSystem.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index(string city = "洛阳市")
        {
            Session["nav"] = 0;
            List<string> provinces = new List<string>();
            if (Session["user"] != null) 
            {
                ViewBag.user = Session["user"];
            }
            List<string> citys = new List<string>();

            if (Session["city"] != null)
            {
                city = Session["city"].ToString();

                // 通过城市反查询省
                string thisProvince = DataDictionaryService.LoadEntities(d => d.dkey == "region" && d.dvalue2 == city).FirstOrDefault().dvalue3;

                provinces.Add(thisProvince);

                List<DataDictionary> dataDic = DataDictionaryService.LoadEntities(d => d.dkey == "region").ToList();

                foreach (DataDictionary data in dataDic)
                {
                    if (!provinces.Contains(data.dvalue3))
                    {
                        provinces.Add(data.dvalue3);
                    }
                }

                // 同理需要查询市
                citys.Add(city);
                dataDic = DataDictionaryService.LoadEntities(d => d.dkey == "region" && d.dvalue3 == thisProvince).ToList();

                foreach (DataDictionary data in dataDic)
                {
                    if (!citys.Contains(data.dvalue2))
                    {
                        citys.Add(data.dvalue2);
                    }
                }

                //ViewBag.citys = citys;
                Session["selectCity"] = citys;
                // 给租房列表使用
                Session["rentCity"] = city;
                //Session["city"] = null;

            }
            else 
            {
                // 查询所有
                List<DataDictionary> dataDic = DataDictionaryService.LoadEntities(d => d.dkey == "region").ToList();
                provinces.Add("河南省");
                foreach (DataDictionary data in dataDic)
                {
                    if (!provinces.Contains(data.dvalue3))
                    {
                        provinces.Add(data.dvalue3);
                    }
                }

                dataDic = DataDictionaryService.LoadEntities(d => d.dkey == "region" && d.dvalue3 == "河南省").ToList();
                citys.Add("洛阳市");
                foreach (DataDictionary data in dataDic)
                {
                    if (!citys.Contains(data.dvalue2))
                    {
                        citys.Add(data.dvalue2);
                    }
                }
                Session["city"] = Session["rentCity"];
                Session["defaultCity"] = citys;
                Session["rentCity"] = city;
            }

            Dictionary<string, List<HousePicture>> houseDic = new Dictionary<string, List<HousePicture>>();
            // 出租房
            List<House> houseList = HouseService.LoadEntities(t => t.type == "1" && t.state == "0" && t.audit == "1" && t.city == city).ToList();
            List<HousePicture> hps = newHousePicture(houseList);

            //Console.WriteLine(houseList);
            houseDic.Add("rent",hps);
            // 新房
            houseList = HouseService.LoadEntities(t => t.type == "2" && t.state == "0" && t.audit == "1" && t.city == city).ToList();
            hps = newHousePicture(houseList);
            houseDic.Add("new", hps);

            houseList = HouseService.LoadEntities(t => t.type == "3" && t.state == "0" && t.audit == "1" && t.city == city).ToList();
            // houseList.Capacity
            // 二手房
            hps = newHousePicture(houseList);
            houseDic.Add("second", hps);

            // 数据字典区域划分 提取session
            List<DataDictionary> datas = DataDictionaryService.LoadEntities(t => t.dvalue2 == city).ToList();

            Session["region"] = datas;

            // 推荐房源
            List<House> recommendhouseList = HouseService.LoadEntities(t => t.isRecommend == "1" && t.audit == "1").ToList();
            List<HousePicture> rhp = newHousePicture(recommendhouseList);
            Session["recommendHouse"] = rhp;


            Session["provinces"] = provinces;

            ViewBag.houseDic = houseDic;
            return View();
        }


        public JsonResult getProvince() 
        {

            DataDictionaryService.LoadEntities(d => d.dkey == "region").ToList();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCity(string province)
        {
            List<DataDictionary> datas =  DataDictionaryService.LoadEntities(d => d.dkey == "region" && d.dvalue3 == province).ToList();

            List<string> city = new List<string>();
            //city.Add()
            foreach(DataDictionary item in datas) 
            {
                if (!city.Contains(item.dvalue2)) 
                {
                    city.Add(item.dvalue2);
                }
            }

            Session["city"] = city[0];

            return Json(city, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getRegion(string city)
        {

            DataDictionaryService.LoadEntities(d => d.dkey == "region" && d.dvalue2 == city).ToList();
            return Json(null, JsonRequestBehavior.AllowGet);
        }


        public JsonResult changeSession(string city) 
        {
            Session["city"] = city;
            return Json("success",JsonRequestBehavior.AllowGet);
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
                    area = Convert.ToInt32(h.area),
                    price = Convert.ToInt32(h.price),
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
