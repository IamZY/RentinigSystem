using My.RentingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace My.RentingSystem.WebApp.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username,string password) 
        {
            if ("" == username || "" == password) 
            {
                ViewBag.message = "用户名或密码不能为空";
                return View();
            }

            List<User> users = UserService.LoadEntities(u => u.username == username && u.password == password && u.isSys == "0").ToList().ToList();
         
            int len = users.Count;
            Session["city"] = Session["rentCity"];
            if (len == 1) 
            {
                //ViewBag.user = users[0];
                Session["user"] = users[0];
                //Session["province"] = "北京市";
                return RedirectToAction("Index", "Home", new { city = "洛阳市" });
            }

            ViewBag.message = "用户名或密码错误";
            return View();
        }

        public ActionResult Reg() 
        {
            return View();
        }

        public ActionResult Logout() 
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult Reg(string username,string password,string phone,string sex,string name) 
        {

            if ("".Equals(username) || "".Equals(password) || "".Equals(phone) || "".Equals(sex) || "".Equals(name))
            {
                return View();
            }


            User user = UserService.AddEntity(new User { 
                username = username,
                password = password,
                isSys = "0"
            });


            UserInfo userInfo = UserInfoService.AddEntity(new UserInfo { 
                name=name,
                sex= sex,
                phone=phone,
            });

            UserUserInfoService.AddEntity(new UserUserInfo { 
                uid = user.uid,
                uiid = userInfo.uiid,
                pubTime = DateTime.Now.ToString("yyyyMMdd")
            });

            return RedirectToAction("Login","User");
        }


        public ActionResult Personal(string id) 
        {
            int uid = Convert.ToInt32(id);

            UserUserInfo userUserInfo = UserUserInfoService.LoadEntities(u=>u.uid == uid).FirstOrDefault();

            User user = UserService.LoadEntities(u => u.uid == userUserInfo.uid).FirstOrDefault();
            UserInfo userInfo = UserInfoService.LoadEntities(u => u.uiid == userUserInfo.uiid).FirstOrDefault();

            ViewBag.user = user;
            ViewBag.userInfo = userInfo;

            return View();
        }



    }
}
