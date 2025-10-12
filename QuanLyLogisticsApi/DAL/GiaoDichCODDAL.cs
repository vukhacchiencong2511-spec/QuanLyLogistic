using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class GiaoDichCODDAL
    {
        private readonly string _conn;
        public GiaoDichCODDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public List<GiaoDichCOD> GetAll()
        {
            var list = new List<GiaoDichCOD>();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM GiaoDichCOD", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new GiaoDichCOD
                {
                    MaGiaoDich = Convert.ToInt64(dr["MaGiaoDich"]),
                    MaDon = dr["MaDon"].ToString(),
                    SoTien = Convert.ToDecimal(dr["SoTien"]),
                    NguoiThu = dr["NguoiThu"].ToString(),
                    NgayThu = dr["NgayThu"] == DBNull.Value ? null : Convert.ToDateTime(dr["NgayThu"]),
                    DaDoiSoat = Convert.ToBoolean(dr["DaDoiSoat"]),
                    NgayDoiSoat = dr["NgayDoiSoat"] == DBNull.Value ? null : Convert.ToDateTime(dr["NgayDoiSoat"]),
                    SoTienThanhToan = dr["SoTienThanhToan"] == DBNull.Value ? null : Convert.ToDecimal(dr["SoTienThanhToan"]),
                    DuLieuThem = dr["DuLieuThem"].ToString()
                });
            }
            return list;
        }

        public bool Add(GiaoDichCOD g)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"INSERT INTO GiaoDichCOD 
                (MaDon, SoTien, NguoiThu, NgayThu, DaDoiSoat, NgayDoiSoat, SoTienThanhToan, DuLieuThem)
                VALUES (@don, @tien, @thu, @ngay, @doi, @doi_ngay, @tt, @dulieu)", conn);
            cmd.Parameters.AddWithValue("@don", g.MaDon);
            cmd.Parameters.AddWithValue("@tien", g.SoTien);
            cmd.Parameters.AddWithValue("@thu", g.NguoiThu);
            cmd.Parameters.AddWithValue("@ngay", (object?)g.NgayThu ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@doi", g.DaDoiSoat);
            cmd.Parameters.AddWithValue("@doi_ngay", (object?)g.NgayDoiSoat ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@tt", (object?)g.SoTienThanhToan ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@dulieu", g.DuLieuThem);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(GiaoDichCOD g)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"UPDATE GiaoDichCOD 
                SET DaDoiSoat=@doi, NgayDoiSoat=@ngay, SoTienThanhToan=@tien 
                WHERE MaGiaoDich=@id", conn);
            cmd.Parameters.AddWithValue("@id", g.MaGiaoDich);
            cmd.Parameters.AddWithValue("@doi", g.DaDoiSoat);
            cmd.Parameters.AddWithValue("@ngay", (object?)g.NgayDoiSoat ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@tien", (object?)g.SoTienThanhToan ?? DBNull.Value);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(long id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("DELETE FROM GiaoDichCOD WHERE MaGiaoDich=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
