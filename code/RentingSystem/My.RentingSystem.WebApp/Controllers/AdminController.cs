using My.RentingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using My.RentingSystem.Model.domain;
using System.IO;
using System.Data.Linq.SqlClient;

namespace My.RentingSystem.WebApp.Controllers
{
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string username,string password) 
        {
            User user = UserService.LoadEntities(u => u.username == username & u.password == password && u.isSys == "1").FirstOrDefault();

            if (user != null)
            {
                UserAuth userAuth = UserAuthService.LoadEntities(u => u.uid == user.uid).FirstOrDefault();
                return Json(new LoginEntity { msg = "success", code = "200", user = user, userAuth = userAuth }, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json(new LoginEntity { msg = "error", code = "201", user = null }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpGet]
        public JsonResult getUser(
            string pageSize,
            string name,
            string page,
            string username,
            string password,
            string phone,
            string sex,
            string isSys) 
        {
            List<UserUserInfo> uuiList = UserUserInfoService.LoadEntities(t => true).ToList();

            List<UserUserInfoEntity> entity = new List<UserUserInfoEntity>();

            foreach(UserUserInfo item in uuiList) 
            {
                User user = UserService.LoadEntities(u => u.uid == item.uid).FirstOrDefault();
                UserInfo userInfo = UserInfoService.LoadEntities(u => u.uiid == item.uiid).FirstOrDefault();
                entity.Add(new UserUserInfoEntity
                {
                    uid = user.uid,
                    uiid = userInfo.uiid,
                    username = user.username,
                    password = user.password,
                    name = userInfo.name,
                    sex = userInfo.sex,
                    phone = userInfo.phone,
                    isSys = user.isSys
                });
            }

            int pageInt = Convert.ToInt32(page);
            int pageSizeInt = Convert.ToInt32(pageSize);

            if (!string.IsNullOrEmpty(name))
            {
                entity = entity.Where(u => u.name.Contains(name)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(username))
            {
                entity = entity.Where(u => u.username.Contains(username)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(isSys))
            {
                entity = entity.Where(u => u.isSys == isSys).ToList();
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                entity = entity.Where(u => u.password.Contains(password)).ToList();
            }


            if (!string.IsNullOrWhiteSpace(sex))
            {
                entity = entity.Where(u => u.sex == sex).ToList();
            }


            if (!string.IsNullOrWhiteSpace(phone))
            {
                entity = entity.Where(u => u.phone.Contains(phone)).ToList();
            }
            int total = entity.Count;
            entity = entity.Skip((pageInt - 1) * pageSizeInt).Take(pageSizeInt).ToList();

            return Json(new { entity=entity, total =  total}, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        //public JsonResult editUser(string username, string password, string name, string sex, string phone) 
        //public JsonResult editUser(FormCollection form) 
        public JsonResult editUser(UserUserInfoEntity entity) 
        {
            //string username1 = Request.Form["username"];
            //string username = form["username"];
            //Console.WriteLine(entity);
            
            User user = UserService.LoadEntities(u=>u.uid == entity.uid).FirstOrDefault();
            UserInfo userInfo = UserInfoService.LoadEntities(u=>u.uiid == entity.uiid).FirstOrDefault();

            user.username = entity.username;
            user.password = entity.password;
            user.isSys = entity.isSys;

            userInfo.name = entity.name;
            userInfo.phone = entity.phone;
            userInfo.sex = entity.sex;

            UserService.EditEntity(user);
            UserInfoService.EditEntity(userInfo);


            return Json("success", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult addUser(UserUserInfoEntity entity)
        {
            User user = UserService.AddEntity(new User { 
                username = entity.username,
                password = entity.password,
                isSys = entity.isSys
            });

            if (entity.isSys == "1") 
            {
                UserAuthService.AddEntity(new UserAuth { main = "0",users="0",orders="0",house="0",others="0",uid = user.uid  });
            }

            UserInfo userInfo = UserInfoService.AddEntity(new UserInfo { 
                name = entity.name,
                sex = entity.sex,
                phone = entity.phone
            });

            UserUserInfoService.AddEntity(new UserUserInfo { 
                uid = user.uid,
                uiid = userInfo.uiid,
                pubTime = DateTime.Now.ToString("yyyyMMdd")
            });

            return Json("success", JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult deleteUser(string uid,string uiid) 
        {
            //Console.WriteLine();

            int uidInt = Convert.ToInt32(uid);
            int uiidInt = Convert.ToInt32(uiid);

            UserUserInfo uui = UserUserInfoService.LoadEntities(u=>u.uid == uidInt && u.uiid == uiidInt).FirstOrDefault();

            UserUserInfoService.DeleteEntity(uui);

            UserAuth userAuth = UserAuthService.LoadEntities(u => u.uid == uidInt).FirstOrDefault();

            UserAuthService.DeleteEntity(userAuth);
            //UserService.DeleteEntity(user);

            return Json("success", JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult getHouse(string pageSize, string page,
            string area,
            string price,
            string pay,
            string unitType,
            string floor,
            string community,
            string type,
            string region,
            string city    
            ) 
        {
            List<House> houseList = HouseService.LoadEntities(h => true).ToList();

            List<HousePicture> hps = new List<HousePicture>();

            hps = newHousePicture(houseList);

            int pageInt = Convert.ToInt32(page);
            int pageSizeInt = Convert.ToInt32(pageSize);

            

            if (!string.IsNullOrWhiteSpace(area))
            {
                int areaInt = Convert.ToInt32(area);
                hps = hps.Where(h => h.area == areaInt).ToList();
            }

            if (!string.IsNullOrWhiteSpace(price))
            {
                int priceInt = Convert.ToInt32(price);
                hps = hps.Where(h => h.price == priceInt).ToList();
            }

            if (!string.IsNullOrWhiteSpace(community))
            {
                hps = hps.Where(h => h.community.Contains(community)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(unitType))
            {
                hps = hps.Where(h => h.unitType.Contains(unitType)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(floor))
            {
                hps = hps.Where(h => h.floor.Contains(floor)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                hps = hps.Where(h => h.type==type).ToList();
            }
            if (!string.IsNullOrWhiteSpace(region))
            {
                hps = hps.Where(h => h.region.Contains(region)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                hps = hps.Where(h => h.city.Contains(city)).ToList();
            }

            int total = hps.Count;
            hps = hps.Skip((pageInt - 1) * pageSizeInt).Take(pageSizeInt).ToList();

            return Json(new {hps = hps, total = total}, JsonRequestBehavior.AllowGet);
        
        }


        [HttpGet]
        public JsonResult recommendUser(string hid) 
        {
            int hidInt = Convert.ToInt32(hid);

            House house = HouseService.LoadEntities(h=>h.hid == hidInt).FirstOrDefault();
            
            if(house.isRecommend == "1") 
            {
                house.isRecommend = "0";
            } else 
            {
                house.isRecommend = "1";
            }


            bool res = HouseService.EditEntity(house);

            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult addHouse(string user,string area,
                                    string price,
                                    string pay,
                                    string unitType,
                                    string floor,
                                    string community,
                                    string type,
                                    string houseDesc,
                                    string region
                                    )
        {
            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

            House house = new House();

            house.isRecommend = "0";
            house.pubPerson = user;
            DateTime date = DateTime.Now;
            string now = date.ToString("yyyyMMdd");
            house.time = now;
            house.state = "0";

            house.area = Convert.ToInt32(area);
            house.price = Convert.ToInt32(price);
            house.pay = pay;
            house.unitType = unitType;
            house.floor = floor;
            house.community = community;
            house.type = type;
            house.houseDesc = houseDesc;
            house.region = region;
            house.audit = "1";

            House h = HouseService.AddEntity(house);

            //if (files.Count == 0)
            //    return View();
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
                        PictureService.AddEntity(new Picture
                        {
                            hid = h.hid,
                            path = filePath
                        });
                    }
                }

            }


            return Json("success", JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult deleteHouse(string hid) 
        {

            int hidInt = Convert.ToInt32(hid);
            House house = HouseService.LoadEntities(h=>h.hid == hidInt).FirstOrDefault();

            bool res = HouseService.DeleteEntity(house);


            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editHouse(string hid,
                                    string area,
                                    string price,
                                    string pay,
                                    string unitType,
                                    string floor,
                                    string community,
                                    string type,
                                    string houseDesc,
                                    string region)
        {

            int hidInt = Convert.ToInt32(hid);

            House house = HouseService.LoadEntities(h => h.hid == hidInt).FirstOrDefault();
            house.area = Convert.ToInt32(area);
            house.price = Convert.ToInt32(price);
            house.pay = pay;
            house.unitType = unitType;
            house.floor = floor;
            house.community = community;
            house.type = type;
            house.houseDesc = houseDesc;
            house.region = region;

            bool res = HouseService.EditEntity(house);


            return Json(res, JsonRequestBehavior.AllowGet);

        } 


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
                    audit = h.audit,
                    city = h.city,
                    state = h.state,
                    province = h.province
                };
                hp.pics = PictureService.LoadEntities(p => p.hid == h.hid).ToList();
                hps.Add(hp);
            }
            return hps;
        }


        [HttpGet]
        public JsonResult getOrders(string pageSize, string page,
                    string name,
                    string oid,
                    string community,
                    string area,
                    string unitType,
                    string duration,
                    string rent,
                    string floor,
                    string time,
                    string state
            
            ) 
        {

            List<OrdersUserHouse> ouhList = OrdersUserHouseService.LoadEntities(o => true).ToList();

            List<OrdersUserHouseEntity> entity = new List<OrdersUserHouseEntity>();
            
            foreach (OrdersUserHouse item in ouhList) 
            {
                Orders orders = OrdersService.LoadEntities(o => o.oid == item.oid).FirstOrDefault();
                User user = UserService.LoadEntities(o => o.uid == item.uid).FirstOrDefault();

                int uiid = UserUserInfoService.LoadEntities(u=>u.uid == user.uid).FirstOrDefault().uiid;
                UserInfo userInfo = UserInfoService.LoadEntities(u=>u.uiid == uiid).FirstOrDefault();
                House house = HouseService.LoadEntities(h => h.hid == item.hid).FirstOrDefault();

                entity.Add(new OrdersUserHouseEntity() { 
                    name = userInfo.name,
                    phone = userInfo.phone,
                    duration = orders.duration,
                    rent = orders.rent,
                    state = orders.state,
                    time = orders.time,
                    ouhid = item.ouhid,
                    hid = house.hid,
                    community = house.community,
                    unitType = house.unitType,
                    area = house.area,
                    oid = orders.oid,
                    floor = house.floor
                });

            }

            int pageInt = Convert.ToInt32(page);
            int pageSizeInt = Convert.ToInt32(pageSize);


            if (!string.IsNullOrWhiteSpace(state))
            {
                //int oidInt = Convert.ToInt32(oid);
                entity = entity.Where(u => u.state == state).ToList();
            }

            //time = time.Replace("-","");
            // 20210326
            if (!string.IsNullOrWhiteSpace(time))
            {
                time = time.Substring(0, 10).Replace("-","");
                DateTime itemDate = DateTime.ParseExact(time, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                itemDate = itemDate.AddDays(1);
                string formatDate = itemDate.ToString("yyyyMMdd");
                //int oidInt = Convert.ToInt32(oid);
                entity = entity.Where(u => u.time == formatDate).ToList();
            }


            if (!string.IsNullOrWhiteSpace(oid))
            {
                int oidInt = Convert.ToInt32(oid);
                entity = entity.Where(u => u.oid == oidInt).ToList();
            }


            if (!string.IsNullOrWhiteSpace(area))
            {
                int areaInt = Convert.ToInt32(area);
                entity = entity.Where(u => u.area == areaInt).ToList();
            }

            if (!string.IsNullOrWhiteSpace(duration))
            {
                int durationInt = Convert.ToInt32(duration);
                entity = entity.Where(u => u.duration == durationInt).ToList();
            }

            if (!string.IsNullOrWhiteSpace(rent))
            {
                int rentInt = Convert.ToInt32(rent);
                entity = entity.Where(u => u.rent == rentInt).ToList();
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                entity = entity.Where(u=>u.name.Contains(name)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(community))
            {
                entity = entity.Where(u => u.community.Contains(community)).ToList();
            }


            if (!string.IsNullOrWhiteSpace(unitType))
            {
                entity = entity.Where(u => u.unitType.Contains(unitType)).ToList();
            }


            if (!string.IsNullOrWhiteSpace(floor))
            {
                entity = entity.Where(u => u.floor.Contains(floor)).ToList();
            }


            int total = entity.Count;
            entity = entity.Skip((pageInt - 1) * pageSizeInt).Take(pageSizeInt).ToList();


            return Json(new { entity = entity, total = total }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult deleteOrder(string ouhid) 
        {
            int ouhidInt = Convert.ToInt32(ouhid);
            OrdersUserHouse ouh =  OrdersUserHouseService.LoadEntities(o=>o.ouhid == ouhidInt).FirstOrDefault();

            bool res = OrdersUserHouseService.DeleteEntity(ouh);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editOrder(string ouhid,
                    string oid,
                    string community,
                    string area,
                    string unitType,
                    string duration,
                    string rent,
                    string floor,
                    string state
                    ) 
        {

            int oidInt = Convert.ToInt32(oid);
            Orders order = OrdersService.LoadEntities(o => o.oid == oidInt).FirstOrDefault();
            int ouhidInt = Convert.ToInt32(ouhid);
            int hid = OrdersUserHouseService.LoadEntities(o => o.ouhid == ouhidInt).FirstOrDefault().hid;

            House house = HouseService.LoadEntities(h => h.hid == hid).FirstOrDefault();

            // 如果时长不变 只变价格
            if (Convert.ToInt32(duration) == order.duration)
            {
                order.rent = Convert.ToInt32(rent);
                order.state = state;
            }
            else 
            {
                // 改变时长 重新计算价格
                order.state = state;
                order.duration = Convert.ToInt32(duration);
                string service = DataDictionaryService.LoadEntities(d => d.remark == "服务费").FirstOrDefault().dvalue;

                order.rent = (Convert.ToInt32(duration) * house.price) +  Convert.ToInt32(service);
            }

            bool res = OrdersService.EditEntity(order);

            return Json(res,JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult getMessage(string pageSize, 
            string community, 
            string page,
            string message,
            string username,
            string time
            ) 
        {

            List<HouseMessage> hmList = HouseMessageService.LoadEntities(m => true).ToList();

            List<MessageEntity> entity = new List<MessageEntity>();

            foreach(HouseMessage item in hmList) 
            {
                User user = UserService.LoadEntities(u=>u.uid == item.uid).FirstOrDefault();
                Message msg = MessageService.LoadEntities(m=>m.mid == item.mid).FirstOrDefault();
                House house = HouseService.LoadEntities(h=>h.hid == item.hid).FirstOrDefault();

                entity.Add(new MessageEntity() { 
                    hmid = item.hmid,
                    username = user.username,
                    message = msg.message1,
                    time = msg.time,
                    community = house.community,
                    floor = house.floor,
                    area = house.area
                });

            }

            int pageInt = Convert.ToInt32(page);
            int pageSizeInt = Convert.ToInt32(pageSize);

            if (!string.IsNullOrWhiteSpace(community))
            {
                entity = entity.Where(m => m.community.Contains(community)).Skip((pageInt - 1) * pageSizeInt).Take(pageSizeInt).ToList();
            }



            if (!string.IsNullOrWhiteSpace(username))
            {
                entity = entity.Where(m => m.username.Contains(username)).Skip((pageInt - 1) * pageSizeInt).Take(pageSizeInt).ToList();
            }



            if (!string.IsNullOrWhiteSpace(message))
            {
                entity = entity.Where(m => m.message.Contains(message)).Skip((pageInt - 1) * pageSizeInt).Take(pageSizeInt).ToList();
            }


            if (!string.IsNullOrWhiteSpace(time))
            {
                time = time.Substring(0, 10).Replace("-", "");
                DateTime itemDate = DateTime.ParseExact(time, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                itemDate = itemDate.AddDays(1);
                string formatDate = itemDate.ToString();
                string formatDateSub = formatDate.Substring(0, 9);
                entity = entity.Where(m => m.time.Contains(formatDateSub)).Skip((pageInt - 1) * pageSizeInt).Take(pageSizeInt).ToList();
            }
           
            entity = entity.Skip((pageInt - 1) * pageSizeInt).Take(pageSizeInt).ToList();
            

            int total = entity.Count;
            return Json(new { entity = entity,total = total }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult deleteMessage(string hmid)
        {

            int hmidInt = Convert.ToInt32(hmid);

            HouseMessage hm = HouseMessageService.LoadEntities(h=>h.hmid == hmidInt).FirstOrDefault();

            bool res = HouseMessageService.DeleteEntity(hm);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editMessage(string hmid,string message)
        {
            int hmidInt = Convert.ToInt32(hmid);

            int mid = HouseMessageService.LoadEntities(h=>h.hmid == hmidInt).FirstOrDefault().mid;

            Message msg = MessageService.LoadEntities(m=>m.mid == mid).FirstOrDefault();
            msg.message1 = message;
            bool res = MessageService.EditEntity(msg);

            return Json(res,JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult getAudit(string pageSize, string page,
            string area,
            string price,
            string pay,
            string unitType,
            string floor,
            string community,
            string type,
            string region,
            string city    
            )
        {
            List<House> houseList = HouseService.LoadEntities(h => true).ToList();

            List<HousePicture> hps = new List<HousePicture>();

            hps = newHousePicture(houseList);

            int pageInt = Convert.ToInt32(page);
            int pageSizeInt = Convert.ToInt32(pageSize);

            if (!string.IsNullOrWhiteSpace(area))
            {
                int areaInt = Convert.ToInt32(area);
                hps = hps.Where(h => h.area == areaInt).ToList();
            }

            if (!string.IsNullOrWhiteSpace(price))
            {
                int priceInt = Convert.ToInt32(price);
                hps = hps.Where(h => h.price == priceInt).ToList();
            }

            if (!string.IsNullOrWhiteSpace(community))
            {
                hps = hps.Where(h => h.community.Contains(community)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(unitType))
            {
                hps = hps.Where(h => h.unitType.Contains(unitType)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(floor))
            {
                hps = hps.Where(h => h.floor.Contains(floor)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                hps = hps.Where(h => h.type == type).ToList();
            }
            if (!string.IsNullOrWhiteSpace(region))
            {
                hps = hps.Where(h => h.region.Contains(region)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                hps = hps.Where(h => h.city.Contains(city)).ToList();
            }

            int total = hps.Count;
            hps = hps.Skip((pageInt - 1) * pageSizeInt).Take(pageSizeInt).ToList();

            return Json(new { hps = hps, total = total }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult editAudit(string hid)
        {

            int hidInt = Convert.ToInt32(hid);

            House house = HouseService.LoadEntities(h=>h.hid == hidInt).FirstOrDefault();

            house.audit = "1";

            bool res = HouseService.EditEntity(house);

            return Json(res,JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult removeAudit(string hid)
        {

            int hidInt = Convert.ToInt32(hid);

            House house = HouseService.LoadEntities(h => h.hid == hidInt).FirstOrDefault();

            house.audit = "2";

            bool res = HouseService.EditEntity(house);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getMainData(string province) 
        {
            int userToday = 0;
            int userMonth = 0;
            int userHis = 0;

            DateTime dt = DateTime.Now;
            string now = dt.ToString("yyyyMMdd");
            // 用户
            List<UserUserInfo> userList = UserUserInfoService.LoadEntities(u => true).ToList();
            userToday = userList.Where(u => u.pubTime == now).ToList().Count;
            foreach (UserUserInfo item in userList) 
            {
                DateTime itemDate = DateTime.ParseExact(item.pubTime, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                //DateTime _itemDate = itemDate.ToShortDateString();
                DateTime startMonth = DateTime.ParseExact(dt.AddDays(1 - dt.Day).ToString("yyyyMMdd"), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                      //本月月初 带分钟
                //DateTime _startMonth = DateTime.ParseExact(startMonth.ToShortDateString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                //DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末
                DateTime endMonth = DateTime.ParseExact(dt.AddDays(1 - dt.Day).AddMonths(1).AddDays(-1).ToString("yyyyMMdd"), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                //DateTime _endMonth = DateTime.ParseExact(startMonth.ToShortDateString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                if (itemDate >= startMonth && itemDate <= endMonth) { userMonth++; }
                
            }

            userHis = userList.Count;

            // 订单
            int orderToday = 0;
            int orderMonth = 0;
            int orderHis = 0;
            int waitOrder = 0;
            int placedOrder = 0;
            List<OrdersUserHouse> ouhList = OrdersUserHouseService.LoadEntities(o => true).ToList();
            orderToday = ouhList.Where(o => o.pubTime == now).ToList().Count;
            foreach(OrdersUserHouse ouh in ouhList) 
            {
                Orders order = OrdersService.LoadEntities(o => o.oid == ouh.oid).FirstOrDefault();

                DateTime itemDate = DateTime.ParseExact(ouh.pubTime, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初
                DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末

                if (itemDate >= startMonth && itemDate <= endMonth) { orderMonth++; }

                if (order.state == "0") { waitOrder++; }

                if (order.state == "1") { placedOrder++;}
            }

            orderHis = ouhList.Count;

            // 房屋
            int houseToday = 0;
            int houseMonth = 0;
            int houseHis = 0;
            List<House> houseList = HouseService.LoadEntities(h => true).ToList();
            houseToday = houseList.Where(h => h.time == now).ToList().Count;
            foreach (House item in houseList)
            {
                DateTime itemDate = DateTime.ParseExact(item.time, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初
                DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末

                if (itemDate >= startMonth && itemDate <= endMonth) { houseMonth++; }

            }

            houseHis = houseList.Count;


            Dictionary<string, int> userChart = new Dictionary<string, int>();
            Dictionary<string, int> houseChart = new Dictionary<string, int>();
            DateTime last7Day = DateTime.Now.AddDays(-6);
            
            for (int i = 0; i <= 6; i++) 
            {
                string date = last7Day.ToString("yyyyMMdd");
                //DateTime itemDate = DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                int count = UserUserInfoService.LoadEntities(u => u.pubTime == date).ToList().Count;
                int countHouse = HouseService.LoadEntities(h => h.time == date).ToList().Count;
                userChart.Add(date, count);
                houseChart.Add(date, countHouse);
                last7Day = last7Day.AddDays(1);
            }


            houseList = HouseService.LoadEntities(h => h.province == province).ToList();
            Dictionary<string, int> houseDic = new Dictionary<string, int>();
            foreach (House item in houseList) 
            {
                if (!houseDic.ContainsKey(item.city))
                {
                    houseDic.Add(item.city, 1);
                }
                else 
                {
                    int count = houseDic[item.city] += 1;
                    houseDic[item.city] = count;
                }
            }

            List<string> userChartX = userChart.Keys.ToList();
            List<int> userChartY = userChart.Values.ToList();

            //List<string> houseDicX = houseDic.Keys.ToList();
            List<int> houseDicY = houseDic.Values.ToList();
            List<HousePie> housePie = new List<HousePie>();
            foreach (var item in houseDic) 
            {
                housePie.Add(new HousePie { name=item.Key,value=item.Value.ToString() });
            }

            List<string> provinces = new List<string>();
            List<DataDictionary> datas = DataDictionaryService.LoadEntities(d => d.dkey == "region").ToList();

            foreach (var item in datas)
            {
                if (!provinces.Contains(item.dvalue3))
                {
                    provinces.Add(item.dvalue3);
                }
            }

            AdminMainEntity entity = new AdminMainEntity 
            {
                userToday = userToday,
                userMonth = userMonth,
                userHis = userHis,
                orderToday = orderToday,
                orderMonth = orderMonth,
                orderHis = orderHis,
                waitOrder = waitOrder,
                placedOrder = placedOrder,
                houseToday = houseToday,
                houseMonth = houseMonth,
                houseHis = houseHis,
                userChartX = userChartX,
                userChartY = userChartY,
                //houseDicX = houseDicX,
                houseDicY = houseChart.Values.ToList(),
                housePie = housePie,
                provinces = provinces
            };

            return Json(entity, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult getUserAuth(string page,string pageSize) 
        {
            List<UserAuth> userAuths = UserAuthService.LoadEntities(u => true).ToList();

            int pageInt = Convert.ToInt32(page);
            int pageSizeInt = Convert.ToInt32(pageSize);

            List<UserAuthEntity> users = new List<UserAuthEntity>();

            foreach (UserAuth item in userAuths) 
            {
                users.Add(new UserAuthEntity { 
                    username =UserService.LoadEntities(u=>u.uid == item.uid).FirstOrDefault().username,
                    main = item.main,
                    house = item.house,
                    users = item.users,
                    orders = item.orders,
                    others = item.others,
                    uid = item.uid,
                    uaid = item.uaid,
                    house123 = item.main
                });
            }

            int total = users.Count;
            users = users.Skip((pageInt - 1) * pageSizeInt).Take(pageSizeInt).ToList();

            return Json(new { users = users,total = total }, JsonRequestBehavior.AllowGet);
        
        }

        [HttpGet]
        public JsonResult updateAuth(UserAuth userauth) 
        {
            bool res = UserAuthService.EditEntity(userauth);
            return Json(res,JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult getProvince() 
        {
            List<string> province = new List<string>();
            List<DataDictionary> datas =  DataDictionaryService.LoadEntities(d=>d.dkey == "region").ToList();

            foreach (var item in datas)
	        {
		        if(!province.Contains(item.dvalue3))
                {
                    province.Add(item.dvalue3);
                }
	        }

            return Json(province, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public JsonResult getCity(string province) 
        {
            List<string> city = new List<string>();
            List<DataDictionary> datas =  DataDictionaryService.LoadEntities(d=>d.dkey == "region" && d.dvalue3 == province).ToList();

            foreach (var item in datas)
	        {
                if (!city.Contains(item.dvalue2))
                {
                    city.Add(item.dvalue2);
                }
	        }

            return Json(city, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult getRegion(string city)
        {
            List<string> region = new List<string>();
            List<DataDictionary> datas = DataDictionaryService.LoadEntities(d => d.dkey == "region" && d.dvalue2 == city).ToList();

            foreach (var item in datas)
            {
                if (!region.Contains(item.dvalue))
                {
                    region.Add(item.dvalue);
                }
            }

            return Json(region, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult getDataDic(string pageSize, string page,
            string dkey, string dvalue, string dvalue2, string dvalue3
            )
        {
            List<DataDictionary> datas = DataDictionaryService.LoadEntities(d=>true).ToList();

            int pageInt = Convert.ToInt32(page);
            int pageSizeInt = Convert.ToInt32(pageSize);

            if (!string.IsNullOrWhiteSpace(dkey))
            {
                datas = datas.Where(d=>d.dkey.Contains(dkey)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(dvalue))
            {
                datas = datas.Where(d => d.dvalue.Contains(dvalue)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(dvalue2))
            {
                datas = datas.Where(d => d.dvalue2.Contains(dvalue2)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(dvalue3))
            {
                datas = datas.Where(d => d.dvalue3.Contains(dvalue3)).ToList();
            }

            int count = datas.Count;
            datas = datas.Skip((pageInt - 1) * pageSizeInt).Take(pageSizeInt).ToList();

            return Json(new { datas = datas, count = count }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult addDataDic(DataDictionary dataDic) 
        {
            if(string.IsNullOrEmpty(dataDic.dvalue)) 
            {
                dataDic.dvalue = " ";
            }

            if (string.IsNullOrEmpty(dataDic.dvalue2))
            {
                dataDic.dvalue2 = " ";
            }


            if (string.IsNullOrEmpty(dataDic.dvalue3))
            {
                dataDic.dvalue3 = " ";
            }

            if (string.IsNullOrEmpty(dataDic.remark)) 
            {
                dataDic.remark = " ";
            }

            DataDictionary res = DataDictionaryService.AddEntity(dataDic);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult editDataDic(DataDictionary dataDic)
        {
            bool res = DataDictionaryService.EditEntity(dataDic);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult removeDataDic(string did)
        {
            int didInt = Convert.ToInt32(did);
            DataDictionary data = DataDictionaryService.LoadEntities(d => d.did == didInt).FirstOrDefault();

            bool res = DataDictionaryService.DeleteEntity(data);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


    }
}
