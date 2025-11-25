using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NguoiDungController : ControllerBase
    {
        private readonly NguoiDungBUS _bus;

        public NguoiDungController(IConfiguration config)
        {
            _bus = new NguoiDungBUS(config);
        }

        // ✅ Lấy danh sách tất cả người dùng
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _bus.GetAll();
            return Ok(data);
        }

        // ✅ Thêm người dùng mới
        [HttpPost]
        public IActionResult Add([FromBody] NguoiDung n)
        {
            try
            {
                if (_bus.Add(n))
                    return Ok(new { message = "Thêm người dùng thành công!" });
                return BadRequest(new { message = "Thêm người dùng thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ Cập nhật người dùng
        [HttpPut]
        public IActionResult Update([FromBody] NguoiDung n)
        {
            try
            {
                if (_bus.Update(n))
                    return Ok(new { message = "Cập nhật thành công!" });
                return BadRequest(new { message = "Cập nhật thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ Xóa người dùng
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                if (_bus.Delete(id))
                    return Ok(new { message = "Xóa thành công!" });
                return BadRequest(new { message = "Xóa thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ API đăng nhập
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _bus.GetByUsername(request.TenDangNhap);

            if (user != null && user.MatKhau == request.MatKhau)
                return Ok(new { message = "Đăng nhập thành công", user });

            return Unauthorized(new { message = "Sai tên đăng nhập hoặc mật khẩu" });
        }

        // Tạo class riêng để nhận dữ liệu đăng nhập
        public class LoginRequest
        {
            public string TenDangNhap { get; set; }
            public string MatKhau { get; set; }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var data = _bus.GetById(id);
            if (data == null)
                return NotFound(new { message = "Không tìm thấy người dùng." });
            return Ok(data);
        }

        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            var list = _bus.GetAll();
            if (list == null || list.Count == 0)
                return BadRequest("Không có dữ liệu để xuất Excel.");

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("NguoiDung");

                // ======= HEADER =======
                ws.Cells[1, 1].Value = "Mã người dùng";
                ws.Cells[1, 2].Value = "Tên đăng nhập";
                ws.Cells[1, 3].Value = "Họ tên";
                ws.Cells[1, 4].Value = "Mã vai trò";
                ws.Cells[1, 5].Value = "Ngày tạo";

                // ======= DATA =======
                int row = 2;
                foreach (var u in list)
                {
                    ws.Cells[row, 1].Value = u.MaNguoiDung;
                    ws.Cells[row, 2].Value = u.TenDangNhap;
                    ws.Cells[row, 3].Value = u.HoTen;
                    ws.Cells[row, 4].Value = u.MaVaiTro;
                    ws.Cells[row, 5].Value = u.NgayTao?.ToString("yyyy-MM-dd HH:mm:ss");

                    row++;
                }

                // Tự chỉnh độ rộng cột
                ws.Cells.AutoFitColumns();

                var bytes = package.GetAsByteArray();

                return File(
                    bytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "NguoiDung.xlsx"
                );
            }
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword(ChangePasswordDTO dto)
        {
            var user = _bus.GetById(dto.MaNguoiDung);

            if (user == null)
                return NotFound("Không tìm thấy người dùng");

            if (user.MatKhau != dto.MatKhauCu)
                return BadRequest("Mật khẩu cũ không đúng");

            if (_bus.ChangePassword(dto.MaNguoiDung, dto.MatKhauMoi))
                return Ok("Đổi mật khẩu thành công");

            return BadRequest("Không thể đổi mật khẩu");
        }
    }
}
