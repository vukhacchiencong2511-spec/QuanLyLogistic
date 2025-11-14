namespace QuanLyLogisticsApi.Models
{
    public class TuyenDuong
    {
        public string MaTuyen { get; set; }
        public string MaTuyenCode { get; set; }
        public string MaTaiXe { get; set; }
        public string PhuongTien { get; set; }
        public DateTime? ThoiGianBatDau { get; set; } = DateTime.Now;
        public DateTime? ThoiGianKetThuc { get; set; } = DateTime.Now;
        public string MaKhuVuc { get; set; }
        public decimal DoanhThuUocTinh { get; set; }
        public DateTime? NgayTao { get; set; } = DateTime.Now;
    }
}
