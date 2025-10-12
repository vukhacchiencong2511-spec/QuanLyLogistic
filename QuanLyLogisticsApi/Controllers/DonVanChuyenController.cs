using Microsoft.AspNetCore.Mvc;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonVanChuyenController : ControllerBase
    {
        private readonly DonVanChuyenBUS _bus;

        public DonVanChuyenController(IConfiguration config)
        {
            _bus = new DonVanChuyenBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] DonVanChuyen d)
        {
            if (_bus.Add(d))
                return Ok(new { message = "Thêm đơn vận chuyển thành công" });
            return BadRequest(new { message = "Lỗi khi thêm" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] DonVanChuyen d)
        {
            if (_bus.Update(d))
                return Ok(new { message = "Cập nhật thành công" });
            return BadRequest(new { message = "Cập nhật thất bại" });
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
