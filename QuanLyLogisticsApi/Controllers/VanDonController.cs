using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var result = _bus.GetById(id);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy vận đơn." });
            return Ok(result);
        }

        [HttpGet("tra-cuu/{soVanDon}")]
        public IActionResult TraCuu(string soVanDon)
        {
            var data = _bus.GetDetail(soVanDon);
            if (data == null)
                return NotFound(new { message = "Không tìm thấy vận đơn." });
            return Ok(data);
        }

        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            using var pkg = new ExcelPackage();
            var ws = pkg.Workbook.Worksheets.Add("VanDon");

            ws.Cells[1, 1].Value = "Mã vận đơn";
            ws.Cells[1, 2].Value = "Số vận đơn";
            ws.Cells[1, 3].Value = "Mã đơn";
            ws.Cells[1, 4].Value = "Ngày phát hành";
            ws.Cells[1, 5].Value = "Thông tin nhà xe";

            var list = _bus.GetAll();
            int r = 2;

            foreach (var x in list)
            {
                ws.Cells[r, 1].Value = x.MaVanDon;
                ws.Cells[r, 2].Value = x.SoVanDon;
                ws.Cells[r, 3].Value = x.MaDon;
                ws.Cells[r, 4].Value = x.NgayPhatHanh;
                ws.Cells[r, 5].Value = x.ThongTinNhaXe;
                r++;
            }

            return File(pkg.GetAsByteArray(),
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              "VanDon.xlsx");
        }
    }
}
