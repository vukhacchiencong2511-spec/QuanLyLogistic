using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class SuKienTrangThaiDAL
    {
        private readonly string _conn;
        public SuKienTrangThaiDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public List<SuKienTrangThai> GetAll()
        {
            var list = new List<SuKienTrangThai>();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM SuKienTrangThai", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new SuKienTrangThai
                {
                    MaSuKien = Convert.ToInt64(dr["MaSuKien"]),
                    MaDon = dr["MaDon"].ToString(),
                    TrangThai = dr["TrangThai"].ToString(),
                    LyDo = dr["LyDo"].ToString(),
                    ThoiGian = Convert.ToDateTime(dr["ThoiGian"]),
                    NguoiCapNhat = dr["NguoiCapNhat"].ToString(),
                    DuLieuThem = dr["DuLieuThem"].ToString(),
                    MaSuKienNgoai = dr["MaSuKienNgoai"].ToString(),
                    KhoaIdempotent = dr["KhoaIdempotent"].ToString(),
                    NgayTao = Convert.ToDateTime(dr["NgayTao"])
                });
            }
            return list;
        }

        public bool Add(SuKienTrangThai s)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"INSERT INTO SuKienTrangThai 
                (MaDon, TrangThai, LyDo, ThoiGian, NguoiCapNhat, DuLieuThem, 
                 MaSuKienNgoai, KhoaIdempotent, NgayTao)
                VALUES (@don, @tt, @lydo, @time, @ngcap, @dulieu, @ngoai, @khoa, @ngay)", conn);
            cmd.Parameters.AddWithValue("@don", s.MaDon);
            cmd.Parameters.AddWithValue("@tt", s.TrangThai);
            cmd.Parameters.AddWithValue("@lydo", s.LyDo);
            cmd.Parameters.AddWithValue("@time", s.ThoiGian);
            cmd.Parameters.AddWithValue("@ngcap", s.NguoiCapNhat);
            cmd.Parameters.AddWithValue("@dulieu", s.DuLieuThem);
            cmd.Parameters.AddWithValue("@ngoai", s.MaSuKienNgoai);
            cmd.Parameters.AddWithValue("@khoa", s.KhoaIdempotent);
            cmd.Parameters.AddWithValue("@ngay", s.NgayTao);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(SuKienTrangThai s)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"UPDATE SuKienTrangThai 
                SET TrangThai=@tt, LyDo=@lydo, ThoiGian=@time 
                WHERE MaSuKien=@id", conn);
            cmd.Parameters.AddWithValue("@id", s.MaSuKien);
            cmd.Parameters.AddWithValue("@tt", s.TrangThai);
            cmd.Parameters.AddWithValue("@lydo", s.LyDo);
            cmd.Parameters.AddWithValue("@time", s.ThoiGian);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(long id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("DELETE FROM SuKienTrangThai WHERE MaSuKien=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
