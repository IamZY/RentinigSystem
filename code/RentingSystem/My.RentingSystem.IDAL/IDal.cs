using My.RentingSystem.Model;

namespace My.RentingSystem.IDAL
{   
	public partial interface IAdvertisementRepository :IBaseRepository<Advertisement>
    {      
    }
	public partial interface IDataDictionaryRepository :IBaseRepository<DataDictionary>
    {      
    }
	public partial interface IHouseRepository :IBaseRepository<House>
    {      
    }
	public partial interface IHouseMessageRepository :IBaseRepository<HouseMessage>
    {      
    }
	public partial interface IMessageRepository :IBaseRepository<Message>
    {      
    }
	public partial interface IOrdersRepository :IBaseRepository<Orders>
    {      
    }
	public partial interface IOrdersUserHouseRepository :IBaseRepository<OrdersUserHouse>
    {      
    }
	public partial interface IPictureRepository :IBaseRepository<Picture>
    {      
    }
	public partial interface IUserRepository :IBaseRepository<User>
    {      
    }
	public partial interface IUserAuthRepository :IBaseRepository<UserAuth>
    {      
    }
	public partial interface IUserInfoRepository :IBaseRepository<UserInfo>
    {      
    }
	public partial interface IUserUserInfoRepository :IBaseRepository<UserUserInfo>
    {      
    }
	
}