namespace QuanLyLogisticsApi.Models
{
    public class SuKienTrangThai
    {
        public long MaSuKien { get; set; }
        public string MaDon { get; set; }
        public string TrangThai { get; set; }
        public string LyDo { get; set; }
        public DateTime ThoiGian { get; set; }
        public string NguoiCapNhat { get; set; }
        public string DuLieuThem { get; set; }
        public string MaSuKienNgoai { get; set; }
        public string KhoaIdempotent { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
