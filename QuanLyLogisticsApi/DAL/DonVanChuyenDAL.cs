using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class DonVanChuyenDAL
    {
        private readonly string _conn;
        public DonVanChuyenDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public List<DonVanChuyen> GetAll()
        {
            var list = new List<DonVanChuyen>();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM DonVanChuyen", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DonVanChuyen
                {
                    MaDon = dr["MaDon"].ToString(),
                    MaDonCode = dr["MaDonCode"].ToString(),
                    MaVanDon = dr["MaVanDon"].ToString(),
                    MaKhachGui = dr["MaKhachGui"].ToString(),
                    MaKhachNhan = dr["MaKhachNhan"].ToString(),
                    MaDiaChiLay = dr["MaDiaChiLay"].ToString(),
                    MaDiaChiGiao = dr["MaDiaChiGiao"].ToString(),
                    LoaiHang = dr["LoaiHang"].ToString(),
                    KhoiLuong = Convert.ToDecimal(dr["KhoiLuong"]),
                    GiaTriKhaiBao = Convert.ToDecimal(dr["GiaTriKhaiBao"]),
                    NguoiTao = dr["NguoiTao"].ToString(),
                    NgayTao = Convert.ToDateTime(dr["NgayTao"]),
                    MaTuyen = dr["MaTuyen"].ToString(),
                    TrangThai = dr["TrangThai"].ToString()
                });
            }
            return list;
        }

        public bool Add(DonVanChuyen d)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"INSERT INTO DonVanChuyen
                (MaDon, MaDonCode, MaVanDon, MaKhachGui, MaKhachNhan, MaDiaChiLay, MaDiaChiGiao, 
                LoaiHang, KhoiLuong, GiaTriKhaiBao, NguoiTao, NgayTao, MaTuyen, TrangThai)
                VALUES (@madon, @code, @vandon, @khg, @khn, @lay, @giao, @loai, @kl, @gt, @ngtao, @ngay, @tuyen, @tt)", conn);
            cmd.Parameters.AddWithValue("@madon", d.MaDon);
            cmd.Parameters.AddWithValue("@code", d.MaDonCode);
            cmd.Parameters.AddWithValue("@vandon", d.MaVanDon);
            cmd.Parameters.AddWithValue("@khg", d.MaKhachGui);
            cmd.Parameters.AddWithValue("@khn", d.MaKhachNhan);
            cmd.Parameters.AddWithValue("@lay", d.MaDiaChiLay);
            cmd.Parameters.AddWithValue("@giao", d.MaDiaChiGiao);
            cmd.Parameters.AddWithValue("@loai", d.LoaiHang);
            cmd.Parameters.AddWithValue("@kl", d.KhoiLuong);
            cmd.Parameters.AddWithValue("@gt", d.GiaTriKhaiBao);
            cmd.Parameters.AddWithValue("@ngtao", d.NguoiTao);
            cmd.Parameters.AddWithValue("@ngay", d.NgayTao);
            cmd.Parameters.AddWithValue("@tuyen", d.MaTuyen);
            cmd.Parameters.AddWithValue("@tt", d.TrangThai);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(DonVanChuyen d)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"UPDATE DonVanChuyen SET 
                TrangThai=@tt, LoaiHang=@loai, KhoiLuong=@kl, GiaTriKhaiBao=@gt 
                WHERE MaDon=@madon", conn);
            cmd.Parameters.AddWithValue("@madon", d.MaDon);
            cmd.Parameters.AddWithValue("@tt", d.TrangThai);
            cmd.Parameters.AddWithValue("@loai", d.LoaiHang);
            cmd.Parameters.AddWithValue("@kl", d.KhoiLuong);
            cmd.Parameters.AddWithValue("@gt", d.GiaTriKhaiBao);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(string id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("DELETE FROM DonVanChuyen WHERE MaDon=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
