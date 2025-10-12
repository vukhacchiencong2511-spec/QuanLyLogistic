using Microsoft.AspNetCore.Mvc;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuKienTrangThaiController : ControllerBase
    {
        private readonly SuKienTrangThaiBUS _bus;

        public SuKienTrangThaiController(IConfiguration config)
        {
            _bus = new SuKienTrangThaiBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] SuKienTrangThai s)
        {
            if (_bus.Add(s))
                return Ok(new { message = "Thêm sự kiện trạng thái thành công" });
            return BadRequest(new { message = "Lỗi khi thêm" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] SuKienTrangThai s)
        {
            if (_bus.Update(s))
                return Ok(new { message = "Cập nhật thành công" });
            return BadRequest(new { message = "Cập nhật thất bại" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (_bus.Delete(id))
                return Ok(new { message = "Xóa thành công" });
            return BadRequest(new { message = "Lỗi khi xóa" });
        }
    }
}
