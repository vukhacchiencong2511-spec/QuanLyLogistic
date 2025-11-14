using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class ChungTuDAL
    {
        private readonly string _conn;
        public ChungTuDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public List<ChungTu> GetAll()
        {
            var list = new List<ChungTu>();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM ChungTu", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new ChungTu
                {
                    MaChungTu = Convert.ToInt64(dr["MaChungTu"]),
                    MaDon = dr["MaDon"].ToString(),
                    NguoiUpload = dr["NguoiUpload"].ToString(),
                    NgayUpload = Convert.ToDateTime(dr["NgayUpload"]),
                    KyNhan = dr["KyNhan"].ToString(),
                    DuongDanThuNho = dr["DuongDanThuNho"].ToString(),
                    LoaiKyNhan = dr["LoaiKyNhan"].ToString()
                });
            }
            return list;
        }

        public bool Add(ChungTu c)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"INSERT INTO ChungTu 
                (MaDon, NguoiUpload, NgayUpload, KyNhan, DuongDanThuNho, LoaiKyNhan)
                VALUES (@don, @up, @ngay, @anh, @thumb, @loai)", conn);
            cmd.Parameters.AddWithValue("@don", c.MaDon);
            cmd.Parameters.AddWithValue("@up", c.NguoiUpload);
            cmd.Parameters.AddWithValue("@ngay", c.NgayUpload);
            cmd.Parameters.AddWithValue("@anh", c.KyNhan);
            cmd.Parameters.AddWithValue("@thumb", c.DuongDanThuNho);
            cmd.Parameters.AddWithValue("@loai", c.LoaiKyNhan);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(ChungTu c)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new(@"UPDATE ChungTu 
                SET KyNhan=@anh, DuongDanThuNho=@thumb, LoaiKyNhan=@loai 
                WHERE MaChungTu=@id", conn);
            cmd.Parameters.AddWithValue("@id", c.MaChungTu);
            cmd.Parameters.AddWithValue("@anh", c.KyNhan);
            cmd.Parameters.AddWithValue("@thumb", c.DuongDanThuNho);
            cmd.Parameters.AddWithValue("@loai", c.LoaiKyNhan);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(long id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("DELETE FROM ChungTu WHERE MaChungTu=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public ChungTu? GetById(long id)
        {
            using SqlConnection conn = new(_conn);
            string sql = "SELECT * FROM ChungTu WHERE MaChungTu = @id";
            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                return new ChungTu
                {
                    MaChungTu = Convert.ToInt64(r["MaChungTu"]),
                    MaDon = r["MaDon"].ToString(),
                    NguoiUpload = r["NguoiUpload"].ToString(),
                    NgayUpload = r["NgayUpload"] as DateTime?,
                    KyNhan = r["KyNhan"].ToString(),
                    DuongDanThuNho = r["DuongDanThuNho"].ToString(),
                    LoaiKyNhan = r["LoaiKyNhan"].ToString()
                };
            }
            return null;
        }
    }
}
