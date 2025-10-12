namespace QuanLyLogisticsApi.Models
{
    public class GiaoDichCOD
    {
        public long MaGiaoDich { get; set; }
        public string MaDon { get; set; }
        public decimal SoTien { get; set; }
        public string NguoiThu { get; set; }
        public DateTime? NgayThu { get; set; }
        public bool DaDoiSoat { get; set; }
        public DateTime? NgayDoiSoat { get; set; }
        public decimal? SoTienThanhToan { get; set; }
        public string DuLieuThem { get; set; }
    }
}
