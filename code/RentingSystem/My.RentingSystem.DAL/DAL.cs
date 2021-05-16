using My.RentingSystem.Model;
using My.RentingSystem.IDAL;

namespace My.RentingSystem.DAL
{
	public partial class AdvertisementRepository : BaseRepository<Advertisement>,IAdvertisementRepository
    {
    }
	public partial class DataDictionaryRepository : BaseRepository<DataDictionary>,IDataDictionaryRepository
    {
    }
	public partial class HouseRepository : BaseRepository<House>,IHouseRepository
    {
    }
	public partial class HouseMessageRepository : BaseRepository<HouseMessage>,IHouseMessageRepository
    {
    }
	public partial class MessageRepository : BaseRepository<Message>,IMessageRepository
    {
    }
	public partial class OrdersRepository : BaseRepository<Orders>,IOrdersRepository
    {
    }
	public partial class OrdersUserHouseRepository : BaseRepository<OrdersUserHouse>,IOrdersUserHouseRepository
    {
    }
	public partial class PictureRepository : BaseRepository<Picture>,IPictureRepository
    {
    }
	public partial class UserRepository : BaseRepository<User>,IUserRepository
    {
    }
	public partial class UserAuthRepository : BaseRepository<UserAuth>,IUserAuthRepository
    {
    }
	public partial class UserInfoRepository : BaseRepository<UserInfo>,IUserInfoRepository
    {
    }
	public partial class UserUserInfoRepository : BaseRepository<UserUserInfo>,IUserUserInfoRepository
    {
    }
}