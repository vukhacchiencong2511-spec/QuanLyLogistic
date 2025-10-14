using Microsoft.AspNetCore.Mvc;
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
    }
}
