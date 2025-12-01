using System.Data.SqlClient;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.DAL
{
    public class TuyenDuongDAL
    {
        private readonly string _conn;
        public TuyenDuongDAL(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public List<TuyenDuong> GetAll()
        {
            var list = new List<TuyenDuong>();
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM TuyenDuong", conn);

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new TuyenDuong
                {
                    MaTuyen = dr["MaTuyen"].ToString(),
                    MaTuyenCode = dr["MaTuyenCode"].ToString(),
                    MaTaiXe = dr["MaTaiXe"].ToString(),
                    PhuongTien = dr["PhuongTien"].ToString(),

                    ThoiGianBatDau = dr["ThoiGianBatDau"] == DBNull.Value ? null : Convert.ToDateTime(dr["ThoiGianBatDau"]),
                    ThoiGianKetThuc = dr["ThoiGianKetThuc"] == DBNull.Value ? null : Convert.ToDateTime(dr["ThoiGianKetThuc"]),

                    MaKhuVuc = dr["MaKhuVuc"].ToString(),

                    DoanhThuUocTinh = dr["DoanhThuUocTinh"] == DBNull.Value
                        ? null
                        : Convert.ToDecimal(dr["DoanhThuUocTinh"]),

                    NgayTao = dr["NgayTao"] == DBNull.Value ? null : Convert.ToDateTime(dr["NgayTao"])
                });
            }
            return list;
        }

        public bool Add(TuyenDuong t)
        {
            using SqlConnection conn = new(_conn);

            SqlCommand cmd = new(@"INSERT INTO TuyenDuong 
        (MaTuyen, MaTuyenCode, MaTaiXe, PhuongTien, ThoiGianBatDau,
         ThoiGianKetThuc, MaKhuVuc, DoanhThuUocTinh, NgayTao)
        VALUES (@ma, @code, @tx, @pt, @bd, @kt, @kv, @dt, @ngay)", conn);

            cmd.Parameters.AddWithValue("@ma", t.MaTuyen);
            cmd.Parameters.AddWithValue("@code", t.MaTuyenCode ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@tx", t.MaTaiXe ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@pt", t.PhuongTien ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@bd", t.ThoiGianBatDau ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kt", t.ThoiGianKetThuc ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kv", t.MaKhuVuc ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@dt", t.DoanhThuUocTinh ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ngay", t.NgayTao ?? (object)DBNull.Value);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(TuyenDuong t)
        {
            using SqlConnection conn = new(_conn);

            SqlCommand cmd = new(@"UPDATE TuyenDuong SET 
            MaTuyenCode = @code,
            MaTaiXe = @tx,
            PhuongTien = @pt,
            ThoiGianBatDau = @bd,
            ThoiGianKetThuc = @kt,
            MaKhuVuc = @kv,
            DoanhThuUocTinh = @dt,
            NgayTao = @ngay
        WHERE MaTuyen = @ma", conn);

            cmd.Parameters.AddWithValue("@ma", t.MaTuyen);
            cmd.Parameters.AddWithValue("@code", t.MaTuyenCode ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@tx", t.MaTaiXe ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@pt", t.PhuongTien ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@bd", t.ThoiGianBatDau ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kt", t.ThoiGianKetThuc ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@kv", t.MaKhuVuc ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@dt", t.DoanhThuUocTinh ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ngay", t.NgayTao ?? (object)DBNull.Value);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }


        public bool Delete(string id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("DELETE FROM TuyenDuong WHERE MaTuyen=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public TuyenDuong? GetById(string id)
        {
            using SqlConnection conn = new(_conn);
            SqlCommand cmd = new("SELECT * FROM TuyenDuong WHERE MaTuyen=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            using var r = cmd.ExecuteReader();

            if (r.Read())
            {
                return new TuyenDuong
                {
                    MaTuyen = r["MaTuyen"].ToString(),
                    MaTuyenCode = r["MaTuyenCode"].ToString(),
                    MaTaiXe = r["MaTaiXe"].ToString(),
                    PhuongTien = r["PhuongTien"].ToString(),

                    ThoiGianBatDau = r["ThoiGianBatDau"] == DBNull.Value ? null : Convert.ToDateTime(r["ThoiGianBatDau"]),
                    ThoiGianKetThuc = r["ThoiGianKetThuc"] == DBNull.Value ? null : Convert.ToDateTime(r["ThoiGianKetThuc"]),

                    MaKhuVuc = r["MaKhuVuc"].ToString(),

                    DoanhThuUocTinh = r["DoanhThuUocTinh"] == DBNull.Value
                        ? null
                        : Convert.ToDecimal(r["DoanhThuUocTinh"]),

                    NgayTao = r["NgayTao"] == DBNull.Value ? null : Convert.ToDateTime(r["NgayTao"])
                };
            }

            return null;
        }
    }
}
