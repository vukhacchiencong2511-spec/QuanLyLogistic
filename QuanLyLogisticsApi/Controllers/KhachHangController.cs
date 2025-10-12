using Microsoft.AspNetCore.Mvc;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;


namespace QuanLyLogisticsApi.Controllers
{
    public class KhachHangController : ControllerBase
    {
        private readonly KhachHangBUS _bus;

        public KhachHangController(KhachHangBUS bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _bus.GetAll();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var kh = _bus.GetById(id);
            if (kh == null)
                return NotFound(new { message = "Không tìm thấy khách hàng." });
            return Ok(kh);
        }

        [HttpPost]
        public IActionResult Create([FromBody] KhachHang kh)
        {
            try
            {
                _bus.Add(kh);
                return Ok(new { message = "Thêm khách hàng thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] KhachHang kh)
        {
            try
            {
                kh.MaKhachHang = id;
                _bus.Update(kh);
                return Ok(new { message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                _bus.Delete(id);
                return Ok(new { message = "Xóa thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
