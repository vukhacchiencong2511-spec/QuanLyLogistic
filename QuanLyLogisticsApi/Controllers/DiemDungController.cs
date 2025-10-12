using Microsoft.AspNetCore.Mvc;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiemDungController : ControllerBase
    {
        private readonly DiemDungBUS _bus;

        public DiemDungController(IConfiguration config)
        {
            _bus = new DiemDungBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] DiemDung d)
        {
            if (_bus.Add(d))
                return Ok(new { message = "Thêm điểm dừng thành công" });
            return BadRequest(new { message = "Lỗi khi thêm điểm dừng" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] DiemDung d)
        {
            if (_bus.Update(d))
                return Ok(new { message = "Cập nhật thành công" });
            return BadRequest(new { message = "Cập nhật thất bại" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_bus.Delete(id))
                return Ok(new { message = "Xóa thành công" });
            return BadRequest(new { message = "Lỗi khi xóa điểm dừng" });
        }
    }
}
