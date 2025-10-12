using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.BUS
{
    public class TuyenDuongBUS
    {
        private readonly TuyenDuongDAL _dal;
        public TuyenDuongBUS(IConfiguration config)
        {
            _dal = new TuyenDuongDAL(config);
        }

        public List<TuyenDuong> GetAll() => _dal.GetAll();

        public bool Add(TuyenDuong t)
        {
            if (string.IsNullOrEmpty(t.MaTuyen))
                throw new ArgumentException("Mã tuyến đường không hợp lệ.");
            return _dal.Add(t);
        }

        public bool Update(TuyenDuong t)
        {
            if (string.IsNullOrEmpty(t.MaTuyen))
                throw new ArgumentException("Mã tuyến đường không hợp lệ.");
            return _dal.Update(t);
        }

        public bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Mã tuyến đường không hợp lệ.");
            return _dal.Delete(id);
        }
    }
}
