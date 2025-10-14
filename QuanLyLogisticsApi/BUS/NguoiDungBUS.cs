using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;
using System;
using System.Collections.Generic;

namespace QuanLyLogisticsApi.BUS
{
    public class NguoiDungBUS
    {
        private readonly NguoiDungDAL _dal;

        // ✅ Nhận IConfiguration để DAL lấy chuỗi kết nối
        public NguoiDungBUS(IConfiguration config)
        {
            _dal = new NguoiDungDAL(config);
        }

        // ✅ Lấy danh sách toàn bộ người dùng
        public List<NguoiDung> GetAll()
        {
            return _dal.GetAll();
        }

        // ✅ Thêm người dùng mới
        public bool Add(NguoiDung n)
        {
            if (n == null) return false;
            return _dal.Add(n);
        }

        // ✅ Cập nhật thông tin người dùng
        public bool Update(NguoiDung n)
        {
            if (n == null) return false;
            return _dal.Update(n);
        }

        // ✅ Xóa người dùng
        public bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            return _dal.Delete(id);
        }

        // ✅ Lấy người dùng theo tên đăng nhập (dùng cho login)
        public NguoiDung GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;
            return _dal.GetByUsername(username.Trim());
        }

        // ✅ Kiểm tra đăng nhập (login)
        public NguoiDung Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = _dal.GetByUsername(username.Trim());

            if (user != null && user.MatKhau.Trim() == password.Trim())
                return user;

            return null;
        }
    }
}
