namespace QuanLyLogisticsApi.Models
{
    public class DonVanChuyen
    {
        public string MaDon { get; set; }
        public string MaDonCode { get; set; }
        public string MaVanDon { get; set; }
        public string MaKhachGui { get; set; }
        public string MaKhachNhan { get; set; }
        public string MaDiaChiLay { get; set; }
        public string MaDiaChiGiao { get; set; }
        public string LoaiHang { get; set; }
        public decimal KhoiLuong { get; set; }
        public decimal GiaTriKhaiBao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string MaTuyen { get; set; }
        public string TrangThai { get; set; }
    }
}
