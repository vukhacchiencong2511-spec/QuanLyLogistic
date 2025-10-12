using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class DiaChiDAL
    {
        private readonly string _conn;
        public DiaChiDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public List<DiaChi> GetAll()
        {
            var list = new List<DiaChi>();
            using SqlConnection conn = new SqlConnection(_conn);
            SqlCommand cmd = new("SELECT * FROM DiaChi", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DiaChi
                {
                    MaDiaChi = dr["MaDiaChi"].ToString(),
                    MaKhachHang = dr["MaKhachHang"].ToString(),
                    DiaChiChiTiet = dr["DiaChiChiTiet"].ToString(),
                    ThanhPho = dr["ThanhPho"].ToString(),
                    QuanHuyen = dr["QuanHuyen"].ToString(),
                    MaBuuDien = dr["MaBuuDien"].ToString()
                });
            }
            return list;
        }

        public bool Add(DiaChi d)
        {
            using SqlConnection conn = new SqlConnection(_conn);
            SqlCommand cmd = new(@"INSERT INTO DiaChi 
                (MaDiaChi, MaKhachHang, DiaChiChiTiet, ThanhPho, QuanHuyen, MaBuuDien)
                VALUES (@ma, @kh, @dc, @tp, @qh, @mbd)", conn);
            cmd.Parameters.AddWithValue("@ma", d.MaDiaChi);
            cmd.Parameters.AddWithValue("@kh", d.MaKhachHang);
            cmd.Parameters.AddWithValue("@dc", d.DiaChiChiTiet);
            cmd.Parameters.AddWithValue("@tp", d.ThanhPho);
            cmd.Parameters.AddWithValue("@qh", d.QuanHuyen);
            cmd.Parameters.AddWithValue("@mbd", d.MaBuuDien);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(DiaChi d)
        {
            using SqlConnection conn = new SqlConnection(_conn);
            SqlCommand cmd = new(@"UPDATE DiaChi SET 
                MaKhachHang=@kh, DiaChiChiTiet=@dc, ThanhPho=@tp, QuanHuyen=@qh, MaBuuDien=@mbd
                WHERE MaDiaChi=@ma", conn);
            cmd.Parameters.AddWithValue("@ma", d.MaDiaChi);
            cmd.Parameters.AddWithValue("@kh", d.MaKhachHang);
            cmd.Parameters.AddWithValue("@dc", d.DiaChiChiTiet);
            cmd.Parameters.AddWithValue("@tp", d.ThanhPho);
            cmd.Parameters.AddWithValue("@qh", d.QuanHuyen);
            cmd.Parameters.AddWithValue("@mbd", d.MaBuuDien);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(string id)
        {
            using SqlConnection conn = new SqlConnection(_conn);
            SqlCommand cmd = new("DELETE FROM DiaChi WHERE MaDiaChi=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
