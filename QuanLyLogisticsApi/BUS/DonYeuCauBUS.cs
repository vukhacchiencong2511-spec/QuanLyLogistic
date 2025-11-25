using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.BUS
{
    public class DonYeuCauBUS
    {
        private readonly DonYeuCauDAL _dal;

        public DonYeuCauBUS(IConfiguration config)
        {
            _dal = new DonYeuCauDAL(config);
        }

        public List<DonYeuCau> GetAll() => _dal.GetAll();
        public DonYeuCau GetById(string id) => _dal.GetById(id);

        public bool Add(DonYeuCau d)
        {
            d.MaYeuCau = _dal.GenerateNewId();
            return _dal.Add(d);
        }

        public bool Delete(string id) => _dal.Delete(id);

        public List<DonYeuCau> Search(string key) => _dal.Search(key);
    }
}
