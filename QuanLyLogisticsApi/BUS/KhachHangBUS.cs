using System.Collections.Generic;
using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;


namespace QuanLyLogisticsApi.BUS
{
    public class KhachHangBUS
    {
        private readonly KhachHangDAL _dal;

        public KhachHangBUS(KhachHangDAL dal)
        {
            _dal = dal;
        }

        public List<KhachHang> GetAll() => _dal.GetAll();

        public KhachHang GetById(string id) => _dal.GetById(id);

        public void Add(KhachHang kh)
        {
            if (string.IsNullOrEmpty(kh.MaKhachHang) || string.IsNullOrEmpty(kh.TenKhachHang))
                throw new Exception("Thiếu thông tin bắt buộc!");
            _dal.Insert(kh);
        }

        public void Update(KhachHang kh)
        {
            if (string.IsNullOrEmpty(kh.MaKhachHang))
                throw new Exception("Cần mã khách hàng để cập nhật!");
            _dal.Update(kh);
        }

        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("Thiếu mã khách hàng để xóa!");
            _dal.Delete(id);
        }
    }
}
