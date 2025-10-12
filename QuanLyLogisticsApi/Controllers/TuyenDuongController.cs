using Microsoft.AspNetCore.Mvc;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TuyenDuongController : ControllerBase
    {
        private readonly TuyenDuongBUS _bus;

        public TuyenDuongController(IConfiguration config)
        {
            _bus = new TuyenDuongBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] TuyenDuong t)
        {
            if (_bus.Add(t))
                return Ok(new { message = "Thêm tuyến đường thành công" });
            return BadRequest(new { message = "Lỗi khi thêm tuyến đường" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] TuyenDuong t)
        {
            if (_bus.Update(t))
                return Ok(new { message = "Cập nhật thành công" });
            return BadRequest(new { message = "Cập nhật thất bại" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (_bus.Delete(id))
                return Ok(new { message = "Xóa thành công" });
            return BadRequest(new { message = "Lỗi khi xóa tuyến đường" });
        }
    }
}
