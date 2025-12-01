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
                    MaVanDon = dr["MaVanDon"] == DBNull.Value ? "" : dr["MaVanDon"].ToString(),
                    MaKhachGui = dr["MaKhachGui"].ToString(),
                    MaKhachNhan = dr["MaKhachNhan"].ToString(),
                    MaDiaChiLay = dr["MaDiaChiLay"].ToString(),
                    MaDiaChiGiao = dr["MaDiaChiGiao"].ToString(),
                    LoaiHang = dr["LoaiHang"].ToString(),
                    KhoiLuong = dr["KhoiLuong"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["KhoiLuong"]),
                    GiaTriKhaiBao = dr["GiaTriKhaiBao"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["GiaTriKhaiBao"]),
                    NguoiTao = dr["NguoiTao"].ToString(),
                    NgayTao = dr["NgayTao"] == DBNull.Value ? null : (DateTime?)dr["NgayTao"],
                    MaTuyen = dr["MaTuyen"] == DBNull.Value ? "" : dr["MaTuyen"].ToString(),
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
            cmd.Parameters.AddWithValue("@kl",
                d.KhoiLuong == 0 ? (object)DBNull.Value : d.KhoiLuong);
            cmd.Parameters.AddWithValue("@gt",
                d.GiaTriKhaiBao == 0 ? (object)DBNull.Value : d.GiaTriKhaiBao);
            cmd.Parameters.AddWithValue("@ngtao", d.NguoiTao);
            cmd.Parameters.AddWithValue("@ngay",
                d.NgayTao ?? DateTime.Now);
            cmd.Parameters.AddWithValue("@tuyen", d.MaTuyen);
            cmd.Parameters.AddWithValue("@tt", d.TrangThai);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(DonVanChuyen d)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"
        UPDATE DonVanChuyen SET 
            MaDonCode = @code,
            MaVanDon = @vandon,
            MaKhachGui = @khg,
            MaKhachNhan = @khn,
            MaDiaChiLay = @lay,
            MaDiaChiGiao = @giao,
            LoaiHang = @loai,
            KhoiLuong = @kl,
            GiaTriKhaiBao = @gt,
            NguoiTao = @ngtao,
            NgayTao = @ngay,
            MaTuyen = @tuyen,
            TrangThai = @tt
        WHERE MaDon = @madon", conn);

            cmd.Parameters.AddWithValue("@madon", d.MaDon);
            cmd.Parameters.AddWithValue("@code", d.MaDonCode ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@vandon", d.MaVanDon ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@khg", d.MaKhachGui ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@khn", d.MaKhachNhan ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@lay", d.MaDiaChiLay ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@giao", d.MaDiaChiGiao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@loai", d.LoaiHang ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kl",
                d.KhoiLuong == 0 ? (object)DBNull.Value : d.KhoiLuong);
            cmd.Parameters.AddWithValue("@gt",
                d.GiaTriKhaiBao == 0 ? (object)DBNull.Value : d.GiaTriKhaiBao);
            cmd.Parameters.AddWithValue("@ngtao", d.NguoiTao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ngay", d.NgayTao ?? DateTime.Now);
            cmd.Parameters.AddWithValue("@tuyen", d.MaTuyen ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@tt", d.TrangThai ?? (object)DBNull.Value);

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

        public DonVanChuyen GetById(string id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM DonVanChuyen WHERE MaDon=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new DonVanChuyen
                {
                    MaDon = dr["MaDon"].ToString(),
                    MaDonCode = dr["MaDonCode"].ToString(),
                    MaVanDon = dr["MaVanDon"].ToString(),
                    MaKhachGui = dr["MaKhachGui"].ToString(),
                    MaKhachNhan = dr["MaKhachNhan"].ToString(),
                    MaDiaChiLay = dr["MaDiaChiLay"].ToString(),
                    MaDiaChiGiao = dr["MaDiaChiGiao"].ToString(),
                    LoaiHang = dr["LoaiHang"].ToString(),
                    KhoiLuong = dr["KhoiLuong"] != DBNull.Value ? Convert.ToDecimal(dr["KhoiLuong"]) : 0,
                    GiaTriKhaiBao = dr["GiaTriKhaiBao"] != DBNull.Value ? Convert.ToDecimal(dr["GiaTriKhaiBao"]) : 0,
                    NguoiTao = dr["NguoiTao"].ToString(),
                    NgayTao = dr["NgayTao"] as DateTime?,
                    MaTuyen = dr["MaTuyen"].ToString(),
                    TrangThai = dr["TrangThai"].ToString()
                };
            }
            return null;
        }
    }
}
