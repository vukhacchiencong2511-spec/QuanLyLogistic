using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class VanDonDAL
    {
        private readonly string _conn;
        public VanDonDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public List<VanDon> GetAll()
        {
            var list = new List<VanDon>();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM VanDon", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new VanDon
                {
                    MaVanDon = dr["MaVanDon"].ToString(),
                    SoVanDon = dr["SoVanDon"].ToString(),
                    MaDon = dr["MaDon"].ToString(),
                    NgayPhatHanh = Convert.ToDateTime(dr["NgayPhatHanh"]),
                    ThongTinNhaXe = dr["ThongTinNhaXe"].ToString()
                });
            }
            return list;
        }

        public bool Add(VanDon v)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"INSERT INTO VanDon 
                (MaVanDon, SoVanDon, MaDon, NgayPhatHanh, ThongTinNhaXe)
                VALUES (@ma, @so, @don, @ngay, @ttnx)", conn);
            cmd.Parameters.AddWithValue("@ma", v.MaVanDon);
            cmd.Parameters.AddWithValue("@so", v.SoVanDon);
            cmd.Parameters.AddWithValue("@don", v.MaDon);
            cmd.Parameters.AddWithValue("@ngay", v.NgayPhatHanh);
            cmd.Parameters.AddWithValue("@ttnx", v.ThongTinNhaXe);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(VanDon v)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"UPDATE VanDon 
                SET SoVanDon=@so, NgayPhatHanh=@ngay, ThongTinNhaXe=@ttnx 
                WHERE MaVanDon=@ma", conn);
            cmd.Parameters.AddWithValue("@ma", v.MaVanDon);
            cmd.Parameters.AddWithValue("@so", v.SoVanDon);
            cmd.Parameters.AddWithValue("@ngay", v.NgayPhatHanh);
            cmd.Parameters.AddWithValue("@ttnx", v.ThongTinNhaXe);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(string id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("DELETE FROM VanDon WHERE MaVanDon=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public VanDon? GetById(string id)
        {
            using SqlConnection conn = new(_conn);
            {
                conn.Open();
                string sql = "SELECT * FROM VanDon WHERE MaVanDon = @id OR SoVanDon = @id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new VanDon
                            {
                                MaVanDon = reader["MaVanDon"].ToString(),
                                SoVanDon = reader["SoVanDon"].ToString(),
                                MaDon = reader["MaDon"].ToString(),
                                NgayPhatHanh = reader["NgayPhatHanh"] == DBNull.Value ? null : (DateTime?)reader["NgayPhatHanh"],
                                ThongTinNhaXe = reader["ThongTinNhaXe"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public VanDonDetailDTO GetDetail(string soVanDon)
        {
            using SqlConnection conn = new(_conn);
            string sql = @"
        SELECT 
            vd.SoVanDon,
            vd.ThongTinNhaXe,
            dv.TrangThai,
            dv.LoaiHang,
            khGui.TenKhachHang AS TenKhachGui,
            khNhan.TenKhachHang AS TenKhachNhan,
            dcLay.DiaChiChiTiet AS DiaChiLay,
            dcGiao.DiaChiChiTiet AS DiaChiGiao,
            dv.MaTuyen,
            dv.GiaTriKhaiBao,
            ct.KyNhan AS ChungTu,
            dv.NgayTao
        FROM VanDon vd
        JOIN DonVanChuyen dv ON vd.MaDon = dv.MaDon
        LEFT JOIN KhachHang khGui ON dv.MaKhachGui = khGui.MaKhachHang
        LEFT JOIN KhachHang khNhan ON dv.MaKhachNhan = khNhan.MaKhachHang
        LEFT JOIN DiaChi dcLay ON dv.MaDiaChiLay = dcLay.MaDiaChi
        LEFT JOIN DiaChi dcGiao ON dv.MaDiaChiGiao = dcGiao.MaDiaChi
        LEFT JOIN ChungTu ct ON ct.MaDon = dv.MaDon
        WHERE vd.SoVanDon = @soVanDon";

            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@soVanDon", soVanDon);
            conn.Open();

            using SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new VanDonDetailDTO
            {
                SoVanDon = r["SoVanDon"]?.ToString(),
                ThongTinNhaXe = r["ThongTinNhaXe"]?.ToString(),
                TrangThai = r["TrangThai"]?.ToString(),
                LoaiHang = r["LoaiHang"]?.ToString(),
                TenKhachGui = r["TenKhachGui"]?.ToString(),
                TenKhachNhan = r["TenKhachNhan"]?.ToString(),
                DiaChiLay = r["DiaChiLay"]?.ToString(),
                DiaChiGiao = r["DiaChiGiao"]?.ToString(),
                MaTuyen = r["MaTuyen"]?.ToString(),
                GiaTriKhaiBao = r["GiaTriKhaiBao"] == DBNull.Value ? 0 : Convert.ToDecimal(r["GiaTriKhaiBao"]),
                ChungTu = r["ChungTu"]?.ToString(),
                NgayTao = r["NgayTao"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(r["NgayTao"])
            };
        }

    }
}
