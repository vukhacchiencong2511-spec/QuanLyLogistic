using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class TuyenDuongDAL
    {
        private readonly string _conn;
        public TuyenDuongDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public List<TuyenDuong> GetAll()
        {
            var list = new List<TuyenDuong>();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM TuyenDuong", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new TuyenDuong
                {
                    MaTuyen = dr["MaTuyen"].ToString(),
                    MaTuyenCode = dr["MaTuyenCode"].ToString(),
                    MaTaiXe = dr["MaTaiXe"].ToString(),
                    PhuongTien = dr["PhuongTien"].ToString(),
                    ThoiGianBatDau = Convert.ToDateTime(dr["ThoiGianBatDau"]),
                    ThoiGianKetThuc = Convert.ToDateTime(dr["ThoiGianKetThuc"]),
                    MaKhuVuc = dr["MaKhuVuc"].ToString(),
                    DoanhThuUocTinh = Convert.ToDecimal(dr["DoanhThuUocTinh"]),
                    NgayTao = Convert.ToDateTime(dr["NgayTao"])
                });
            }
            return list;
        }

        public bool Add(TuyenDuong t)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"INSERT INTO TuyenDuong 
                (MaTuyen, MaTuyenCode, MaTaiXe, PhuongTien, ThoiGianBatDau, 
                 ThoiGianKetThuc, MaKhuVuc, DoanhThuUocTinh, NgayTao)
                VALUES (@ma, @code, @tx, @pt, @bd, @kt, @kv, @dt, @ngay)", conn);
            cmd.Parameters.AddWithValue("@ma", t.MaTuyen);
            cmd.Parameters.AddWithValue("@code", t.MaTuyenCode);
            cmd.Parameters.AddWithValue("@tx", t.MaTaiXe);
            cmd.Parameters.AddWithValue("@pt", t.PhuongTien);
            cmd.Parameters.AddWithValue("@bd", t.ThoiGianBatDau);
            cmd.Parameters.AddWithValue("@kt", t.ThoiGianKetThuc);
            cmd.Parameters.AddWithValue("@kv", t.MaKhuVuc);
            cmd.Parameters.AddWithValue("@dt", t.DoanhThuUocTinh);
            cmd.Parameters.AddWithValue("@ngay", t.NgayTao);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(TuyenDuong t)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"UPDATE TuyenDuong 
                SET PhuongTien=@pt, ThoiGianBatDau=@bd, ThoiGianKetThuc=@kt, 
                    DoanhThuUocTinh=@dt 
                WHERE MaTuyen=@ma", conn);
            cmd.Parameters.AddWithValue("@ma", t.MaTuyen);
            cmd.Parameters.AddWithValue("@pt", t.PhuongTien);
            cmd.Parameters.AddWithValue("@bd", t.ThoiGianBatDau);
            cmd.Parameters.AddWithValue("@kt", t.ThoiGianKetThuc);
            cmd.Parameters.AddWithValue("@dt", t.DoanhThuUocTinh);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(string id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("DELETE FROM TuyenDuong WHERE MaTuyen=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
