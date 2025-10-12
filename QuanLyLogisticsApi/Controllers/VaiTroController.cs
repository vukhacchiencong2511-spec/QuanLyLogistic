using Microsoft.AspNetCore.Mvc;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaiTroController : ControllerBase
    {
        private readonly VaiTroBUS bus;

        public VaiTroController(IConfiguration config)
        {
            bus = new VaiTroBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = bus.GetAll();
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Add([FromBody] VaiTro vt)
        {
            if (string.IsNullOrWhiteSpace(vt.TenVaiTro))
                return BadRequest(new { message = "Tên vai trò không được trống!" });

            bool result = bus.Add(vt);
            return result ? Ok(new { message = "Thêm vai trò thành công!" })
                          : BadRequest(new { message = "Không thể thêm vai trò." });
        }

        [HttpPut]
        public IActionResult Update([FromBody] VaiTro vt)
        {
            try
            {
                bool result = bus.Update(vt);
                return result ? Ok(new { message = "Cập nhật vai trò thành công!" })
                              : BadRequest(new { message = "Không thể cập nhật vai trò." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                bool result = bus.Delete(id);
                return result ? Ok(new { message = "Xóa vai trò thành công!" })
                              : BadRequest(new { message = "Không thể xóa vai trò." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
