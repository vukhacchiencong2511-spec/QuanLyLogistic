using Microsoft.AspNetCore.Mvc;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChungTuController : ControllerBase
    {
        private readonly ChungTuBUS _bus;

        public ChungTuController(IConfiguration config)
        {
            _bus = new ChungTuBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] ChungTu c)
        {
            if (_bus.Add(c))
                return Ok(new { message = "Thêm chứng từ thành công" });
            return BadRequest(new { message = "Lỗi khi thêm chứng từ" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] ChungTu c)
        {
            if (_bus.Update(c))
                return Ok(new { message = "Cập nhật thành công" });
            return BadRequest(new { message = "Lỗi khi cập nhật chứng từ" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (_bus.Delete(id))
                return Ok(new { message = "Xóa thành công" });
            return BadRequest(new { message = "Lỗi khi xóa chứng từ" });
        }
    }
}
