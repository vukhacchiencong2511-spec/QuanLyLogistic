namespace QuanLyLogisticsApi.Models
{
    public class DiemDung
    {
        public int MaDiemDung { get; set; }
        public string MaTuyen { get; set; }
        public int ThuTuDung { get; set; }
        public string MaDon { get; set; }
        public DateTime? DuKienDen { get; set; }
        public DateTime? ThucTeDen { get; set; }
    }
}
