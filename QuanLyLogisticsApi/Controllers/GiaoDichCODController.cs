using Microsoft.AspNetCore.Mvc;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiaoDichCODController : ControllerBase
    {
        private readonly GiaoDichCODBUS _bus;

        public GiaoDichCODController(IConfiguration config)
        {
            _bus = new GiaoDichCODBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] GiaoDichCOD g)
        {
            if (_bus.Add(g))
                return Ok(new { message = "Thêm giao dịch COD thành công" });
            return BadRequest(new { message = "Lỗi khi thêm giao dịch COD" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] GiaoDichCOD g)
        {
            if (_bus.Update(g))
                return Ok(new { message = "Cập nhật thành công" });
            return BadRequest(new { message = "Lỗi khi cập nhật giao dịch COD" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (_bus.Delete(id))
                return Ok(new { message = "Xóa thành công" });
            return BadRequest(new { message = "Lỗi khi xóa giao dịch COD" });
        }
    }
}
