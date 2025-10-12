using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.BUS
{
    public class DiemDungBUS
    {
        private readonly DiemDungDAL _dal;
        public DiemDungBUS(IConfiguration config)
        {
            _dal = new DiemDungDAL(config);
        }

        public List<DiemDung> GetAll() => _dal.GetAll();

        public bool Add(DiemDung d)
        {
            if (string.IsNullOrEmpty(d.MaTuyen))
                throw new ArgumentException("Mã tuyến không được trống.");
            return _dal.Add(d);
        }

        public bool Update(DiemDung d)
        {
            if (d.MaDiemDung <= 0)
                throw new ArgumentException("Mã điểm dừng không hợp lệ.");
            return _dal.Update(d);
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Mã điểm dừng không hợp lệ.");
            return _dal.Delete(id);
        }
    }
}
