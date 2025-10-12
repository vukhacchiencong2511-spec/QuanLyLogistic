using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class VaiTroDAL
    {
        private readonly string _connectionString;
        public VaiTroDAL(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // ====== LẤY TẤT CẢ ======
        public List<VaiTro> GetAll()
        {
            var list = new List<VaiTro>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM VaiTro ORDER BY MaVaiTro";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new VaiTro
                    {
                        MaVaiTro = Convert.ToInt32(dr["MaVaiTro"]),
                        TenVaiTro = dr["TenVaiTro"].ToString()
                    });
                }
            }
            return list;
        }

        // ====== THÊM ======
        public bool Add(VaiTro vt)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string sql = "INSERT INTO VaiTro (TenVaiTro) VALUES (@ten)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ten", vt.TenVaiTro);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // ====== CẬP NHẬT ======
        public bool Update(VaiTro vt)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string sql = "UPDATE VaiTro SET TenVaiTro=@ten WHERE MaVaiTro=@id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ten", vt.TenVaiTro);
            cmd.Parameters.AddWithValue("@id", vt.MaVaiTro);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // ====== XOÁ ======
        public bool Delete(int id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string sql = "DELETE FROM VaiTro WHERE MaVaiTro=@id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // ====== KIỂM TRA TỒN TẠI ======
        public bool Exists(int id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string sql = "SELECT COUNT(*) FROM VaiTro WHERE MaVaiTro=@id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }
}
