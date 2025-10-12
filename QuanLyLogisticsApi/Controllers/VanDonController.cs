using Microsoft.AspNetCore.Mvc;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VanDonController : ControllerBase
    {
        private readonly VanDonBUS _bus;
        public VanDonController(IConfiguration config)
        {
            _bus = new VanDonBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] VanDon v)
        {
            if (_bus.Add(v))
                return Ok(new { message = "Thêm vận đơn thành công" });
            return BadRequest(new { message = "Lỗi khi thêm vận đơn" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] VanDon v)
        {
            if (_bus.Update(v))
                return Ok(new { message = "Cập nhật thành công" });
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
