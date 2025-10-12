namespace QuanLyLogisticsApi.Models
{
    public class KhachHang
    {
        public string MaKhachHang { get; set; } = string.Empty;
        public string TenKhachHang { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
