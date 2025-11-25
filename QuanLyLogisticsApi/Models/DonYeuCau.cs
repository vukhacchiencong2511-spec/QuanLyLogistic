namespace QuanLyLogisticsApi.Models
{
    public class DonYeuCau
    {
        public string MaYeuCau { get; set; }
        public string TenNguoiGui { get; set; }
        public string SDTNguoiGui { get; set; }
        public string EmailNguoiGui { get; set; }
        public string DiaChiGui { get; set; }

        public string TenNguoiNhan { get; set; }
        public string SDTNguoiNhan { get; set; }
        public string EmailNguoiNhan { get; set; }
        public string DiaChiNhan { get; set; }

        public string LoaiHang { get; set; }
        public decimal KhoiLuong { get; set; }
        public decimal GiaTriKhaiBao { get; set; }
        public string GhiChu { get; set; }

        public DateTime? NgayTao { get; set; } = DateTime.Now;
    }
}
