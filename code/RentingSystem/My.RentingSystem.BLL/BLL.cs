using My.RentingSystem.IBLL;
using My.RentingSystem.Model;

namespace My.RentingSystem.BLL
{
	public partial class AdvertisementService :BaseService<Advertisement>,IAdvertisementService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IAdvertisementRepository;
        }
    }   
	public partial class DataDictionaryService :BaseService<DataDictionary>,IDataDictionaryService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IDataDictionaryRepository;
        }
    }   
	public partial class HouseService :BaseService<House>,IHouseService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IHouseRepository;
        }
    }   
	public partial class HouseMessageService :BaseService<HouseMessage>,IHouseMessageService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IHouseMessageRepository;
        }
    }   
	public partial class MessageService :BaseService<Message>,IMessageService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IMessageRepository;
        }
    }   
	public partial class OrdersService :BaseService<Orders>,IOrdersService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IOrdersRepository;
        }
    }   
	public partial class OrdersUserHouseService :BaseService<OrdersUserHouse>,IOrdersUserHouseService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IOrdersUserHouseRepository;
        }
    }   
	public partial class PictureService :BaseService<Picture>,IPictureService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IPictureRepository;
        }
    }   
	public partial class UserService :BaseService<User>,IUserService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IUserRepository;
        }
    }   
	public partial class UserAuthService :BaseService<UserAuth>,IUserAuthService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IUserAuthRepository;
        }
    }   
	public partial class UserInfoService :BaseService<UserInfo>,IUserInfoService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IUserInfoRepository;
        }
    }   
	public partial class UserUserInfoService :BaseService<UserUserInfo>,IUserUserInfoService
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.IUserUserInfoRepository;
        }
    }   
	
}