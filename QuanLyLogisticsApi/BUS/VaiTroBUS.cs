using QuanLyLogisticsApi.DAL;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.BUS
{
    public class VaiTroBUS
    {
        private readonly VaiTroDAL dal;
        public VaiTroBUS(IConfiguration config)
        {
            dal = new VaiTroDAL(config);
        }

        public List<VaiTro> GetAll() => dal.GetAll();

        public bool Add(VaiTro vt) => dal.Add(vt);

        public bool Update(VaiTro vt)
        {
            // Không cho sửa vai trò mặc định
            if (vt.MaVaiTro >= 1 && vt.MaVaiTro <= 6)
                throw new Exception("Không thể chỉnh sửa vai trò mặc định!");
            return dal.Update(vt);
        }

        public bool Delete(int id)
        {
            // Không cho xóa vai trò mặc định
            if (id >= 1 && id <= 6)
                throw new Exception("Không thể xóa vai trò mặc định!");
            return dal.Delete(id);
        }

        public bool Exists(int id) => dal.Exists(id);
    }
}
