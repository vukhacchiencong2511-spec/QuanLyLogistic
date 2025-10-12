using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.BUS
{
    public class DonVanChuyenBUS
    {
        private readonly DonVanChuyenDAL _dal;
        public DonVanChuyenBUS(IConfiguration config)
        {
            _dal = new DonVanChuyenDAL(config);
        }

        public List<DonVanChuyen> GetAll() => _dal.GetAll();

        public bool Add(DonVanChuyen d)
        {
            if (string.IsNullOrEmpty(d.MaDon) || string.IsNullOrEmpty(d.MaDonCode))
                throw new ArgumentException("Thông tin đơn vận chuyển không hợp lệ.");
            return _dal.Add(d);
        }

        public bool Update(DonVanChuyen d)
        {
            if (string.IsNullOrEmpty(d.MaDon))
                throw new ArgumentException("Mã đơn vận chuyển không hợp lệ.");
            return _dal.Update(d);
        }

        public bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Mã đơn vận chuyển không hợp lệ.");
            return _dal.Delete(id);
        }
    }
}
