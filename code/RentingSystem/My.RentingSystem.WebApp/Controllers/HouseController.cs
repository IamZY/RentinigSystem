using My.RentingSystem.Model;
using My.RentingSystem.Model.domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace My.RentingSystem.WebApp.Controllers
{
    public class HouseController : BaseController
    {
        //
        // GET: /House/

        public ActionResult Index()
        {

            List<DataDictionary> regions = DataDictionaryService.LoadEntities(t => t.dkey == "region").ToList();

            List<string> province = new List<string>();

            foreach(DataDictionary item in regions)
            {
                if (!province.Contains(item.dvalue3)) 
                {
                    province.Add(item.dvalue3);
                }
            }

            List<string> region = new List<string>();
            regions = DataDictionaryService.LoadEntities(t => t.dkey == "region" && t.dvalue2 == "北京市").ToList();
            foreach (DataDictionary item in regions)
            {
                if (!region.Contains(item.dvalue))
                {
                    region.Add(item.dvalue);
                }
            }

            ViewBag.province = province;
            ViewBag.region = region;

            return View();
        }

        [HttpPost]
        public ActionResult submitHouse(House house)
        {
            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
          
            User user = Session["user"] as My.RentingSystem.Model.User;
            
            house.isRecommend = "0";
            house.pubPerson = user.username;
            DateTime date = DateTime.Now;
            string now = date.ToString("yyyyMMdd");
            house.time = now;
            house.state = "0";
            house.audit = "0";
            House h = HouseService.AddEntity(house);

            if (files.Count == 0)
                return View();
            for (int i = 0; i < files.AllKeys.Count(); i++)
            {
                if (files.AllKeys[i].Equals("images"))
                {
                    if (files[i].FileName.Length > 0)
                    {
                        System.Web.HttpPostedFile postedfile = files[i];
                        string filePath = "";
                        var ext = Path.GetExtension(postedfile.FileName);
                        var fileName = h.hid + "_" + System.Guid.NewGuid().ToString("N") + ext;
                        // 组合文件存储的相对路径
                        filePath = "/Upload/images/" + fileName;
                        // 将相对路径转换成物理路径
                        var path = Server.MapPath(filePath);
                        postedfile.SaveAs(path);
                        string fex = Path.GetExtension(postedfile.FileName);
                        PictureService.AddEntity(new Picture { 
                            hid = h.hid,
                            path = filePath
                        });
                    }
                }

            }


            //HttpPostedFileBase imageName = Request.Files["images"];
            //string fileName = imageName.FileName;
            
            //Console.WriteLine();
            Session["city"] = Session["rentCity"];
            return RedirectToAction("Index", "Home");
        }


        public ActionResult MyAddHouse(string id) 
        {
            @ViewBag.uid = id;
            return View();
        }

        public ActionResult MyAddHousePartial(string id) 
        {
            int uid = Convert.ToInt32(id);
            User user = UserService.LoadEntities(u => u.uid == uid).FirstOrDefault();

            List<House> myPubHouses = HouseService.LoadEntities(h => h.pubPerson == user.username).ToList();

            List<HousePicture> hps = newHousePicture(myPubHouses);

            ViewBag.hps = hps;

            return View(); 
        }

        public ActionResult getCity(string province) 
        {
            List<string> regions = new List<string>();
            //if (province == "北京市" || province == "上海市" || province == "重庆市" || province == "深圳市") 
            //{
            //    List<DataDictionary> region = DataDictionaryService.LoadEntities(d => d.dkey == "region" && d.dvalue2 == province).ToList();
            //    foreach (DataDictionary item in region)
            //    {
            //        if (!regions.Contains(item.dvalue))
            //        {
            //            regions.Add(item.dvalue);
            //        }
            //    }
            //}


            List<DataDictionary> datas = DataDictionaryService.LoadEntities(d => d.dkey == "region" && d.dvalue3 == province).ToList();

            List<string> city = new List<string>();
            //city.Add()
            foreach (DataDictionary item in datas)
            {
                if (!city.Contains(item.dvalue2))
                {
                    city.Add(item.dvalue2);
                }
            }

            string firstCity = city[0];
            List<DataDictionary> region = DataDictionaryService.LoadEntities(d => d.dkey == "region" && d.dvalue2 == firstCity).ToList();
            foreach (DataDictionary item in region)
            {
                if (!regions.Contains(item.dvalue))
                {
                    regions.Add(item.dvalue);
                }
            }


            return Json(new { province = province,city = city,region = regions }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult getRegion(string city)
        {
            List<DataDictionary> datas = DataDictionaryService.LoadEntities(d => d.dkey == "region" 
                && d.dvalue2 == city).ToList();

            List<string> region = new List<string>();
            //city.Add()
            foreach (DataDictionary item in datas)
            {
                if (!region.Contains(item.dvalue))
                {
                    region.Add(item.dvalue);
                }
            }

            return Json(region, JsonRequestBehavior.AllowGet);
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
                    region = h.region,
                    audit = h.audit
                };
                hp.pics = PictureService.LoadEntities(p => p.hid == h.hid).ToList();
                hps.Add(hp);
            }
            return hps;
        }

    }
}
