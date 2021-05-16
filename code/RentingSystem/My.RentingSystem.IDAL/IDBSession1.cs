
namespace My.RentingSystem.IDAL
{
	public partial interface IDBSession
    {
		IAdvertisementRepository IAdvertisementRepository{get;set;}
		IDataDictionaryRepository IDataDictionaryRepository{get;set;}
		IHouseRepository IHouseRepository{get;set;}
		IHouseMessageRepository IHouseMessageRepository{get;set;}
		IMessageRepository IMessageRepository{get;set;}
		IOrdersRepository IOrdersRepository{get;set;}
		IOrdersUserHouseRepository IOrdersUserHouseRepository{get;set;}
		IPictureRepository IPictureRepository{get;set;}
		IUserRepository IUserRepository{get;set;}
		IUserAuthRepository IUserAuthRepository{get;set;}
		IUserInfoRepository IUserInfoRepository{get;set;}
		IUserUserInfoRepository IUserUserInfoRepository{get;set;}
    }
}