using My.RentingSystem.IDAL;
using My.RentingSystem.Ioc;

namespace My.RentingSystem.DAL
{
  public partial class DalFactory
  {
	 public static IAdvertisementRepository GetAdvertisementRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IAdvertisementRepository>("AdvertisementRepository");
	    }
	 }
  	 public static IDataDictionaryRepository GetDataDictionaryRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IDataDictionaryRepository>("DataDictionaryRepository");
	    }
	 }
  	 public static IHouseRepository GetHouseRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IHouseRepository>("HouseRepository");
	    }
	 }
  	 public static IHouseMessageRepository GetHouseMessageRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IHouseMessageRepository>("HouseMessageRepository");
	    }
	 }
  	 public static IMessageRepository GetMessageRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IMessageRepository>("MessageRepository");
	    }
	 }
  	 public static IOrdersRepository GetOrdersRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IOrdersRepository>("OrdersRepository");
	    }
	 }
  	 public static IOrdersUserHouseRepository GetOrdersUserHouseRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IOrdersUserHouseRepository>("OrdersUserHouseRepository");
	    }
	 }
  	 public static IPictureRepository GetPictureRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IPictureRepository>("PictureRepository");
	    }
	 }
  	 public static IUserRepository GetUserRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IUserRepository>("UserRepository");
	    }
	 }
  	 public static IUserAuthRepository GetUserAuthRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IUserAuthRepository>("UserAuthRepository");
	    }
	 }
  	 public static IUserInfoRepository GetUserInfoRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IUserInfoRepository>("UserInfoRepository");
	    }
	 }
  	 public static IUserUserInfoRepository GetUserUserInfoRepository
	 {
	   get
	    {
	     return SpringHelper.GetObject<IUserUserInfoRepository>("UserUserInfoRepository");
	    }
	 }
   }
}