using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class DiemDungDAL
    {
        private readonly string _conn;
        public DiemDungDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public List<DiemDung> GetAll()
        {
            var list = new List<DiemDung>();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM DiemDung", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new DiemDung
                {
                    MaDiemDung = Convert.ToInt32(dr["MaDiemDung"]),
                    MaTuyen = dr["MaTuyen"].ToString(),
                    ThuTuDung = Convert.ToInt32(dr["ThuTuDung"]),
                    MaDon = dr["MaDon"].ToString(),
                    DuKienDen = dr["DuKienDen"] as DateTime?,
                    ThucTeDen = dr["ThucTeDen"] as DateTime?
                });
            }
            return list;
        }

        public bool Add(DiemDung d)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"INSERT INTO DiemDung 
                (MaTuyen, ThuTuDung, MaDon, DuKienDen, ThucTeDen)
                VALUES (@tuyen, @thu, @don, @dukien, @thucte)", conn);
            cmd.Parameters.AddWithValue("@tuyen", d.MaTuyen);
            cmd.Parameters.AddWithValue("@thu", d.ThuTuDung);
            cmd.Parameters.AddWithValue("@don", d.MaDon);
            cmd.Parameters.AddWithValue("@dukien", (object?)d.DuKienDen ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@thucte", (object?)d.ThucTeDen ?? DBNull.Value);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(DiemDung d)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"UPDATE DiemDung 
                SET DuKienDen=@dukien, ThucTeDen=@thucte 
                WHERE MaDiemDung=@id", conn);
            cmd.Parameters.AddWithValue("@id", d.MaDiemDung);
            cmd.Parameters.AddWithValue("@dukien", (object?)d.DuKienDen ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@thucte", (object?)d.ThucTeDen ?? DBNull.Value);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("DELETE FROM DiemDung WHERE MaDiemDung=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
