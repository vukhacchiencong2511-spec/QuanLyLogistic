using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.BUS
{
    public class DiaChiBUS
    {
        private readonly DiaChiDAL _dal;
        public DiaChiBUS(IConfiguration config)
        {
            _dal = new DiaChiDAL(config);
        }

        public List<DiaChi> GetAll() => _dal.GetAll();

        public bool Add(DiaChi d)
        {
            if (string.IsNullOrEmpty(d.MaDiaChi))
                throw new ArgumentException("Mã địa chỉ không được để trống.");
            return _dal.Add(d);
        }

        public bool Update(DiaChi d)
        {
            if (string.IsNullOrEmpty(d.MaDiaChi))
                throw new ArgumentException("Mã địa chỉ không hợp lệ.");
            return _dal.Update(d);
        }

        public bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Mã địa chỉ không hợp lệ.");
            return _dal.Delete(id);
        }
    }
}
