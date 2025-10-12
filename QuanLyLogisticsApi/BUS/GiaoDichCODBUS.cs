using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.BUS
{
    public class GiaoDichCODBUS
    {
        private readonly GiaoDichCODDAL _dal;
        public GiaoDichCODBUS(IConfiguration config)
        {
            _dal = new GiaoDichCODDAL(config);
        }

        public List<GiaoDichCOD> GetAll() => _dal.GetAll();

        public bool Add(GiaoDichCOD g)
        {
            if (string.IsNullOrEmpty(g.MaDon))
                throw new ArgumentException("Mã đơn không được trống.");
            return _dal.Add(g);
        }

        public bool Update(GiaoDichCOD g)
        {
            if (g.MaGiaoDich <= 0)
                throw new ArgumentException("Mã giao dịch không hợp lệ.");
            return _dal.Update(g);
        }

        public bool Delete(long id)
        {
            if (id <= 0)
                throw new ArgumentException("Mã giao dịch không hợp lệ.");
            return _dal.Delete(id);
        }
    }
}
