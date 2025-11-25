using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class DonYeuCauDAL
    {
        private readonly string _conn;

        public DonYeuCauDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        // GET ALL
        public List<DonYeuCau> GetAll()
        {
            var list = new List<DonYeuCau>();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM DonYeuCau ORDER BY NgayTao DESC", conn);
            conn.Open();

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DonYeuCau
                {
                    MaYeuCau = dr["MaYeuCau"].ToString(),
                    TenNguoiGui = dr["TenNguoiGui"].ToString(),
                    SDTNguoiGui = dr["SDTNguoiGui"].ToString(),
                    EmailNguoiGui = dr["EmailNguoiGui"].ToString(),
                    DiaChiGui = dr["DiaChiGui"].ToString(),
                    TenNguoiNhan = dr["TenNguoiNhan"].ToString(),
                    SDTNguoiNhan = dr["SDTNguoiNhan"].ToString(),
                    EmailNguoiNhan = dr["EmailNguoiNhan"].ToString(),
                    DiaChiNhan = dr["DiaChiNhan"].ToString(),
                    LoaiHang = dr["LoaiHang"].ToString(),
                    KhoiLuong = dr["KhoiLuong"] != DBNull.Value ? Convert.ToDecimal(dr["KhoiLuong"]) : 0,
                    GiaTriKhaiBao = dr["GiaTriKhaiBao"] != DBNull.Value ? Convert.ToDecimal(dr["GiaTriKhaiBao"]) : 0,
                    GhiChu = dr["GhiChu"].ToString(),
                    NgayTao = dr["NgayTao"] as DateTime?
                });
            }
            return list;
        }

        // GET BY ID
        public DonYeuCau GetById(string id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM DonYeuCau WHERE MaYeuCau=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();

            using SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new DonYeuCau
                {
                    MaYeuCau = dr["MaYeuCau"].ToString(),
                    TenNguoiGui = dr["TenNguoiGui"].ToString(),
                    SDTNguoiGui = dr["SDTNguoiGui"].ToString(),
                    EmailNguoiGui = dr["EmailNguoiGui"].ToString(),
                    DiaChiGui = dr["DiaChiGui"].ToString(),
                    TenNguoiNhan = dr["TenNguoiNhan"].ToString(),
                    SDTNguoiNhan = dr["SDTNguoiNhan"].ToString(),
                    EmailNguoiNhan = dr["EmailNguoiNhan"].ToString(),
                    DiaChiNhan = dr["DiaChiNhan"].ToString(),
                    LoaiHang = dr["LoaiHang"].ToString(),
                    KhoiLuong = dr["KhoiLuong"] != DBNull.Value ? Convert.ToDecimal(dr["KhoiLuong"]) : 0,
                    GiaTriKhaiBao = dr["GiaTriKhaiBao"] != DBNull.Value ? Convert.ToDecimal(dr["GiaTriKhaiBao"]) : 0,
                    GhiChu = dr["GhiChu"].ToString(),
                    NgayTao = dr["NgayTao"] as DateTime?
                };
            }
            return null;
        }

        // ADD
        public bool Add(DonYeuCau d)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"
                INSERT INTO DonYeuCau VALUES 
                (@MaYeuCau,@TenNguoiGui,@SDTG,@EmailG,@DiaChiG,
                 @TenNguoiNhan,@SDTN,@EmailN,@DiaChiN,
                 @LoaiHang,@KhoiLuong,@GiaTri,@GhiChu,@NgayTao)
            ", conn);

            cmd.Parameters.AddWithValue("@MaYeuCau", d.MaYeuCau);
            cmd.Parameters.AddWithValue("@TenNguoiGui", d.TenNguoiGui);
            cmd.Parameters.AddWithValue("@SDTG", d.SDTNguoiGui);
            cmd.Parameters.AddWithValue("@EmailG", d.EmailNguoiGui);
            cmd.Parameters.AddWithValue("@DiaChiG", d.DiaChiGui);

            cmd.Parameters.AddWithValue("@TenNguoiNhan", d.TenNguoiNhan);
            cmd.Parameters.AddWithValue("@SDTN", d.SDTNguoiNhan);
            cmd.Parameters.AddWithValue("@EmailN", d.EmailNguoiNhan);
            cmd.Parameters.AddWithValue("@DiaChiN", d.DiaChiNhan);

            cmd.Parameters.AddWithValue("@LoaiHang", d.LoaiHang);
            cmd.Parameters.AddWithValue("@KhoiLuong", d.KhoiLuong);
            cmd.Parameters.AddWithValue("@GiaTri", d.GiaTriKhaiBao);
            cmd.Parameters.AddWithValue("@GhiChu", d.GhiChu ?? "");
            cmd.Parameters.AddWithValue("@NgayTao", d.NgayTao ?? DateTime.Now);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // DELETE
        public bool Delete(string id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("DELETE FROM DonYeuCau WHERE MaYeuCau=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // SEARCH
        public List<DonYeuCau> Search(string key)
        {
            var list = new List<DonYeuCau>();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"
                SELECT * FROM DonYeuCau 
                WHERE MaYeuCau LIKE @k OR TenNguoiGui LIKE @k OR SDTNguoiGui LIKE @k
            ", conn);

            cmd.Parameters.AddWithValue("@k", "%" + key + "%");
            conn.Open();

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DonYeuCau
                {
                    MaYeuCau = dr["MaYeuCau"].ToString(),
                    TenNguoiGui = dr["TenNguoiGui"].ToString(),
                    SDTNguoiGui = dr["SDTNguoiGui"].ToString(),
                    EmailNguoiGui = dr["EmailNguoiGui"].ToString(),
                    DiaChiGui = dr["DiaChiGui"].ToString(),
                    TenNguoiNhan = dr["TenNguoiNhan"].ToString(),
                    SDTNguoiNhan = dr["SDTNguoiNhan"].ToString(),
                    EmailNguoiNhan = dr["EmailNguoiNhan"].ToString(),
                    DiaChiNhan = dr["DiaChiNhan"].ToString(),
                    LoaiHang = dr["LoaiHang"].ToString(),
                    KhoiLuong = Convert.ToDecimal(dr["KhoiLuong"]),
                    GiaTriKhaiBao = Convert.ToDecimal(dr["GiaTriKhaiBao"]),
                    GhiChu = dr["GhiChu"].ToString(),
                    NgayTao = dr["NgayTao"] as DateTime?
                });
            }
            return list;
        }

        public string GenerateNewId()
        {
            using SqlConnection conn = new(_conn);
            string sql = "SELECT TOP 1 MaYeuCau FROM DonYeuCau ORDER BY MaYeuCau DESC";
            SqlCommand cmd = new(sql, conn);
            conn.Open();

            var result = cmd.ExecuteScalar()?.ToString();

            if (string.IsNullOrEmpty(result))
                return "YC000001";

            // Lấy phần số sau YC
            int number = int.Parse(result.Substring(2));
            number++;

            return "YC" + number.ToString("D6"); // format YC000001
        }
    }
}
