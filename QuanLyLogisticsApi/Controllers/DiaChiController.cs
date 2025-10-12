using Microsoft.AspNetCore.Mvc;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiaChiController : ControllerBase
    {
        private readonly DiaChiBUS _bus;

        public DiaChiController(IConfiguration config)
        {
            _bus = new DiaChiBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] DiaChi d)
        {
            if (_bus.Add(d))
                return Ok(new { message = "Thêm địa chỉ thành công" });
            return BadRequest(new { message = "Lỗi khi thêm địa chỉ" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] DiaChi d)
        {
            if (_bus.Update(d))
                return Ok(new { message = "Cập nhật địa chỉ thành công" });
            return BadRequest(new { message = "Lỗi khi cập nhật" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (_bus.Delete(id))
                return Ok(new { message = "Xóa thành công" });
            return BadRequest(new { message = "Lỗi khi xóa" });
        }
    }
}
