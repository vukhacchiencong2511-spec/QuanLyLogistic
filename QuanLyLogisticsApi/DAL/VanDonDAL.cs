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
    }
}
