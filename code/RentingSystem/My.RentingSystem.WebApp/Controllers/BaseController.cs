using My.RentingSystem.BLL;
using My.RentingSystem.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace My.RentingSystem.WebApp.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        public UserService UserService 
        {
            get 
            {
                return SpringHelper.GetObject<UserService>("UserService");
            }
        }

        public HouseService HouseService 
        {
            get 
            {
                return SpringHelper.GetObject<HouseService>("HouseService");
            }
        }

        public PictureService PictureService
        {
            get 
            {
                return SpringHelper.GetObject<PictureService>("PictureService");
            }
        }

        public DataDictionaryService DataDictionaryService 
        {
            get 
            {
                return SpringHelper.GetObject<DataDictionaryService>("DataDictionaryService");
            }
        }


        public UserInfoService UserInfoService 
        {
            get 
            {
                return SpringHelper.GetObject<UserInfoService>("UserInfoService");
            }
        }

        public UserUserInfoService UserUserInfoService 
        {
            get 
            {
                return SpringHelper.GetObject<UserUserInfoService>("UserUserInfoService");
            }
        }

        public OrdersService OrdersService 
        {
            get 
            {
                return SpringHelper.GetObject<OrdersService>("OrdersService");
            }
        }


        public OrdersUserHouseService OrdersUserHouseService 
        {
            get 
            {
                return SpringHelper.GetObject<OrdersUserHouseService>("OrdersUserHouseService");
            }
        }

        public MessageService MessageService 
        {
            get 
            {
                return SpringHelper.GetObject<MessageService>("MessageService");
            }
        }

        public HouseMessageService HouseMessageService 
        {
            get 
            {
                return SpringHelper.GetObject<HouseMessageService>("HouseMessageService");
            }
        }

        public UserAuthService UserAuthService 
        {
            get 
            {
                return SpringHelper.GetObject<UserAuthService>("UserAuthService");
            }
        } 

    }
}
