using My.RentingSystem.DAL;
using My.RentingSystem.IDAL;

namespace My.RentingSystem.DALSessionFactory
{
	public partial class DBSession : IDBSession
    {
	
		private IAdvertisementRepository _AdvertisementRepository;
        public IAdvertisementRepository IAdvertisementRepository
        {
            get
            {
                if(_AdvertisementRepository == null)
                {
                   // _AdvertisementRepository = new AdvertisementRepository();
				    _AdvertisementRepository =DalFactory.GetAdvertisementRepository;
                }
                return _AdvertisementRepository;
            }
            set { _AdvertisementRepository = value; }
        }
	
		private IDataDictionaryRepository _DataDictionaryRepository;
        public IDataDictionaryRepository IDataDictionaryRepository
        {
            get
            {
                if(_DataDictionaryRepository == null)
                {
                   // _DataDictionaryRepository = new DataDictionaryRepository();
				    _DataDictionaryRepository =DalFactory.GetDataDictionaryRepository;
                }
                return _DataDictionaryRepository;
            }
            set { _DataDictionaryRepository = value; }
        }
	
		private IHouseRepository _HouseRepository;
        public IHouseRepository IHouseRepository
        {
            get
            {
                if(_HouseRepository == null)
                {
                   // _HouseRepository = new HouseRepository();
				    _HouseRepository =DalFactory.GetHouseRepository;
                }
                return _HouseRepository;
            }
            set { _HouseRepository = value; }
        }
	
		private IHouseMessageRepository _HouseMessageRepository;
        public IHouseMessageRepository IHouseMessageRepository
        {
            get
            {
                if(_HouseMessageRepository == null)
                {
                   // _HouseMessageRepository = new HouseMessageRepository();
				    _HouseMessageRepository =DalFactory.GetHouseMessageRepository;
                }
                return _HouseMessageRepository;
            }
            set { _HouseMessageRepository = value; }
        }
	
		private IMessageRepository _MessageRepository;
        public IMessageRepository IMessageRepository
        {
            get
            {
                if(_MessageRepository == null)
                {
                   // _MessageRepository = new MessageRepository();
				    _MessageRepository =DalFactory.GetMessageRepository;
                }
                return _MessageRepository;
            }
            set { _MessageRepository = value; }
        }
	
		private IOrdersRepository _OrdersRepository;
        public IOrdersRepository IOrdersRepository
        {
            get
            {
                if(_OrdersRepository == null)
                {
                   // _OrdersRepository = new OrdersRepository();
				    _OrdersRepository =DalFactory.GetOrdersRepository;
                }
                return _OrdersRepository;
            }
            set { _OrdersRepository = value; }
        }
	
		private IOrdersUserHouseRepository _OrdersUserHouseRepository;
        public IOrdersUserHouseRepository IOrdersUserHouseRepository
        {
            get
            {
                if(_OrdersUserHouseRepository == null)
                {
                   // _OrdersUserHouseRepository = new OrdersUserHouseRepository();
				    _OrdersUserHouseRepository =DalFactory.GetOrdersUserHouseRepository;
                }
                return _OrdersUserHouseRepository;
            }
            set { _OrdersUserHouseRepository = value; }
        }
	
		private IPictureRepository _PictureRepository;
        public IPictureRepository IPictureRepository
        {
            get
            {
                if(_PictureRepository == null)
                {
                   // _PictureRepository = new PictureRepository();
				    _PictureRepository =DalFactory.GetPictureRepository;
                }
                return _PictureRepository;
            }
            set { _PictureRepository = value; }
        }
	
		private IUserRepository _UserRepository;
        public IUserRepository IUserRepository
        {
            get
            {
                if(_UserRepository == null)
                {
                   // _UserRepository = new UserRepository();
				    _UserRepository =DalFactory.GetUserRepository;
                }
                return _UserRepository;
            }
            set { _UserRepository = value; }
        }
	
		private IUserAuthRepository _UserAuthRepository;
        public IUserAuthRepository IUserAuthRepository
        {
            get
            {
                if(_UserAuthRepository == null)
                {
                   // _UserAuthRepository = new UserAuthRepository();
				    _UserAuthRepository =DalFactory.GetUserAuthRepository;
                }
                return _UserAuthRepository;
            }
            set { _UserAuthRepository = value; }
        }
	
		private IUserInfoRepository _UserInfoRepository;
        public IUserInfoRepository IUserInfoRepository
        {
            get
            {
                if(_UserInfoRepository == null)
                {
                   // _UserInfoRepository = new UserInfoRepository();
				    _UserInfoRepository =DalFactory.GetUserInfoRepository;
                }
                return _UserInfoRepository;
            }
            set { _UserInfoRepository = value; }
        }
	
		private IUserUserInfoRepository _UserUserInfoRepository;
        public IUserUserInfoRepository IUserUserInfoRepository
        {
            get
            {
                if(_UserUserInfoRepository == null)
                {
                   // _UserUserInfoRepository = new UserUserInfoRepository();
				    _UserUserInfoRepository =DalFactory.GetUserUserInfoRepository;
                }
                return _UserUserInfoRepository;
            }
            set { _UserUserInfoRepository = value; }
        }
	}	
}