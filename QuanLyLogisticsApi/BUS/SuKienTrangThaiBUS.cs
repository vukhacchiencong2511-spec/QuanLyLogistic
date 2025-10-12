using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.BUS
{
    public class SuKienTrangThaiBUS
    {
        private readonly SuKienTrangThaiDAL _dal;
        public SuKienTrangThaiBUS(IConfiguration config)
        {
            _dal = new SuKienTrangThaiDAL(config);
        }

        public List<SuKienTrangThai> GetAll() => _dal.GetAll();

        public bool Add(SuKienTrangThai s)
        {
            if (string.IsNullOrEmpty(s.MaDon))
                throw new ArgumentException("Mã đơn không được trống.");
            return _dal.Add(s);
        }

        public bool Update(SuKienTrangThai s)
        {
            if (s.MaSuKien <= 0)
                throw new ArgumentException("Mã sự kiện không hợp lệ.");
            return _dal.Update(s);
        }

        public bool Delete(long id)
        {
            if (id <= 0)
                throw new ArgumentException("Mã sự kiện không hợp lệ.");
            return _dal.Delete(id);
        }
    }
}
