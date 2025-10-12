using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuanLyLogisticsApi.Models;


namespace QuanLyLogisticsApi.DAL
{
    public class KhachHangDAL
    {
        private readonly string _connectionString;

        public KhachHangDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Lấy tất cả khách hàng
        public List<KhachHang> GetAll()
        {
            var list = new List<KhachHang>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM KhachHang ORDER BY NgayTao DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(new KhachHang
                    {
                        MaKhachHang = rd["MaKhachHang"].ToString(),
                        TenKhachHang = rd["TenKhachHang"].ToString(),
                        SoDienThoai = rd["SoDienThoai"].ToString(),
                        Email = rd["Email"].ToString(),
                        NgayTao = (DateTime)rd["NgayTao"]
                    });
                }
            }
            return list;
        }

        // Lấy 1 khách hàng theo mã
        public KhachHang GetById(string id)
        {
            KhachHang kh = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM KhachHang WHERE MaKhachHang = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    kh = new KhachHang
                    {
                        MaKhachHang = rd["MaKhachHang"].ToString(),
                        TenKhachHang = rd["TenKhachHang"].ToString(),
                        SoDienThoai = rd["SoDienThoai"].ToString(),
                        Email = rd["Email"].ToString(),
                        NgayTao = (DateTime)rd["NgayTao"]
                    };
                }
            }
            return kh;
        }

        // Thêm khách hàng
        public void Insert(KhachHang kh)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO KhachHang(MaKhachHang, TenKhachHang, SoDienThoai, Email, NgayTao)
                               VALUES(@MaKhachHang, @TenKhachHang, @SoDienThoai, @Email, GETDATE())";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaKhachHang", kh.MaKhachHang);
                cmd.Parameters.AddWithValue("@TenKhachHang", kh.TenKhachHang);
                cmd.Parameters.AddWithValue("@SoDienThoai", kh.SoDienThoai);
                cmd.Parameters.AddWithValue("@Email", kh.Email);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Cập nhật khách hàng
        public void Update(KhachHang kh)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE KhachHang
                               SET TenKhachHang = @TenKhachHang, SoDienThoai = @SoDienThoai, Email = @Email
                               WHERE MaKhachHang = @MaKhachHang";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaKhachHang", kh.MaKhachHang);
                cmd.Parameters.AddWithValue("@TenKhachHang", kh.TenKhachHang);
                cmd.Parameters.AddWithValue("@SoDienThoai", kh.SoDienThoai);
                cmd.Parameters.AddWithValue("@Email", kh.Email);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Xóa khách hàng
        public void Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM KhachHang WHERE MaKhachHang = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
