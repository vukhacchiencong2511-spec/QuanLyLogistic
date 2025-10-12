using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.BUS
{
    public class VanDonBUS
    {
        private readonly VanDonDAL _dal;
        public VanDonBUS(IConfiguration config)
        {
            _dal = new VanDonDAL(config);
        }

        public List<VanDon> GetAll() => _dal.GetAll();

        public bool Add(VanDon v)
        {
            if (string.IsNullOrEmpty(v.MaVanDon))
                throw new ArgumentException("Mã vận đơn không hợp lệ.");
            return _dal.Add(v);
        }

        public bool Update(VanDon v)
        {
            if (string.IsNullOrEmpty(v.MaVanDon))
                throw new ArgumentException("Mã vận đơn không hợp lệ.");
            return _dal.Update(v);
        }

        public bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Mã vận đơn không hợp lệ.");
            return _dal.Delete(id);
        }
    }
}
