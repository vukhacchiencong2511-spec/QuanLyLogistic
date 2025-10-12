namespace QuanLyLogisticsApi.Models
{
    public class TuyenDuong
    {
        public string MaTuyen { get; set; }
        public string MaTuyenCode { get; set; }
        public string MaTaiXe { get; set; }
        public string PhuongTien { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public string MaKhuVuc { get; set; }
        public decimal DoanhThuUocTinh { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
