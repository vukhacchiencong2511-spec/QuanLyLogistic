using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.BUS
{
    public class NguoiDungBUS
    {
        private readonly NguoiDungDAL _dal;
        public NguoiDungBUS(IConfiguration config)
        {
            _dal = new NguoiDungDAL(config);
        }

        public List<NguoiDung> GetAll() => _dal.GetAll();

        public bool Add(NguoiDung nd)
        {
            if (string.IsNullOrWhiteSpace(nd.TenDangNhap) || string.IsNullOrWhiteSpace(nd.MatKhau))
                throw new ArgumentException("Tên đăng nhập và mật khẩu không được trống.");
            return _dal.Add(nd);
        }

        public bool Update(NguoiDung nd)
        {
            if (string.IsNullOrEmpty(nd.MaNguoiDung))
                throw new ArgumentException("Mã người dùng không hợp lệ.");
            return _dal.Update(nd);
        }

        public bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Mã người dùng không hợp lệ.");
            return _dal.Delete(id);
        }

        public NguoiDung Login(string username, string password)
        {
            var user = _dal.GetByUsername(username);
            if (user == null) return null;
            return user.MatKhau == password ? user : null;
        }
    }
}
