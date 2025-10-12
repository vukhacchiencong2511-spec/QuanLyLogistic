using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.BUS
{
    public class ChungTuBUS
    {
        private readonly ChungTuDAL _dal;
        public ChungTuBUS(IConfiguration config)
        {
            _dal = new ChungTuDAL(config);
        }

        public List<ChungTu> GetAll() => _dal.GetAll();

        public bool Add(ChungTu c)
        {
            if (string.IsNullOrEmpty(c.MaDon))
                throw new ArgumentException("Mã đơn không được trống.");
            return _dal.Add(c);
        }

        public bool Update(ChungTu c)
        {
            if (c.MaChungTu <= 0)
                throw new ArgumentException("Mã chứng từ không hợp lệ.");
            return _dal.Update(c);
        }

        public bool Delete(long id)
        {
            if (id <= 0)
                throw new ArgumentException("Mã chứng từ không hợp lệ.");
            return _dal.Delete(id);
        }
    }
}
