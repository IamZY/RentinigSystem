using My.RentingSystem.Model;

namespace My.RentingSystem.IBLL
{
	public partial interface IAdvertisementService : IBaseService<Advertisement>
    {      
    }   
	public partial interface IDataDictionaryService : IBaseService<DataDictionary>
    {      
    }   
	public partial interface IHouseService : IBaseService<House>
    {      
    }   
	public partial interface IHouseMessageService : IBaseService<HouseMessage>
    {      
    }   
	public partial interface IMessageService : IBaseService<Message>
    {      
    }   
	public partial interface IOrdersService : IBaseService<Orders>
    {      
    }   
	public partial interface IOrdersUserHouseService : IBaseService<OrdersUserHouse>
    {      
    }   
	public partial interface IPictureService : IBaseService<Picture>
    {      
    }   
	public partial interface IUserService : IBaseService<User>
    {      
    }   
	public partial interface IUserAuthService : IBaseService<UserAuth>
    {      
    }   
	public partial interface IUserInfoService : IBaseService<UserInfo>
    {      
    }   
	public partial interface IUserUserInfoService : IBaseService<UserUserInfo>
    {      
    }   
}