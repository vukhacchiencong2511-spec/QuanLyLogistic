using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class NguoiDungDAL
    {
        private readonly string _conn;

        // ✅ Hàm khởi tạo: nhận IConfiguration để lấy chuỗi kết nối từ appsettings.json
        public NguoiDungDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        // ✅ Lấy tất cả người dùng
        public List<NguoiDung> GetAll()
        {
            var list = new List<NguoiDung>();
            using SqlConnection conn = new SqlConnection(_conn);
            SqlCommand cmd = new("SELECT * FROM NguoiDung", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new NguoiDung
                {
                    MaNguoiDung = dr["MaNguoiDung"].ToString(),
                    TenDangNhap = dr["TenDangNhap"].ToString(),
                    MatKhau = dr["MatKhau"].ToString(),
                    HoTen = dr["HoTen"].ToString(),
                    MaVaiTro = Convert.ToInt32(dr["MaVaiTro"]),
                    NgayTao = Convert.ToDateTime(dr["NgayTao"])
                });
            }
            return list;
        }

        // ✅ Thêm người dùng
        public bool Add(NguoiDung n)
        {
            using SqlConnection conn = new SqlConnection(_conn);
            SqlCommand cmd = new(@"INSERT INTO NguoiDung 
                (MaNguoiDung, TenDangNhap, MatKhau, HoTen, MaVaiTro, NgayTao)
                VALUES (@ma, @ten, @mk, @hoten, @vaitro, @ngay)", conn);
            cmd.Parameters.AddWithValue("@ma", n.MaNguoiDung);
            cmd.Parameters.AddWithValue("@ten", n.TenDangNhap);
            cmd.Parameters.AddWithValue("@mk", n.MatKhau);
            cmd.Parameters.AddWithValue("@hoten", n.HoTen);
            cmd.Parameters.AddWithValue("@vaitro", n.MaVaiTro);
            cmd.Parameters.AddWithValue("@ngay", n.NgayTao);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // ✅ Cập nhật người dùng
        public bool Update(NguoiDung n)
        {
            using SqlConnection conn = new SqlConnection(_conn);
            SqlCommand cmd = new(@"UPDATE NguoiDung SET 
                TenDangNhap=@ten, MatKhau=@mk, HoTen=@hoten, MaVaiTro=@vaitro 
                WHERE MaNguoiDung=@ma", conn);
            cmd.Parameters.AddWithValue("@ma", n.MaNguoiDung);
            cmd.Parameters.AddWithValue("@ten", n.TenDangNhap);
            cmd.Parameters.AddWithValue("@mk", n.MatKhau);
            cmd.Parameters.AddWithValue("@hoten", n.HoTen);
            cmd.Parameters.AddWithValue("@vaitro", n.MaVaiTro);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // ✅ Xóa người dùng
        public bool Delete(string id)
        {
            using SqlConnection conn = new SqlConnection(_conn);
            SqlCommand cmd = new("DELETE FROM NguoiDung WHERE MaNguoiDung=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // ✅ Thêm hàm login theo tên đăng nhập
        public NguoiDung GetByUsername(string username)
        {
            using SqlConnection conn = new SqlConnection(_conn);
            SqlCommand cmd = new("SELECT * FROM NguoiDung WHERE TenDangNhap=@username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                return new NguoiDung
                {
                    MaNguoiDung = dr["MaNguoiDung"].ToString(),
                    TenDangNhap = dr["TenDangNhap"].ToString(),
                    MatKhau = dr["MatKhau"].ToString(),
                    HoTen = dr["HoTen"].ToString(),
                    MaVaiTro = Convert.ToInt32(dr["MaVaiTro"]),
                    NgayTao = Convert.ToDateTime(dr["NgayTao"])
                };
            }
            return null;
        }
    }
}
