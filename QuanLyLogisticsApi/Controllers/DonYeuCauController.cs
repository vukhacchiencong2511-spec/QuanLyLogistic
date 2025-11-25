using Microsoft.AspNetCore.Mvc;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;
using OfficeOpenXml;
using System.Data;

namespace QuanLyLogisticsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonYeuCauController : ControllerBase
    {
        private readonly DonYeuCauBUS _bus;

        public DonYeuCauController(IConfiguration config)
        {
            _bus = new DonYeuCauBUS(config);
            ExcelPackage.License.SetNonCommercialPersonal("YourName");
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var data = _bus.GetById(id);
            return data == null ? NotFound() : Ok(data);
        }

        [HttpPost]
        public IActionResult Add([FromBody] DonYeuCau d)
        {
            if (string.IsNullOrWhiteSpace(d.MaYeuCau))
                d.MaYeuCau = "YC" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (_bus.Add(d))
                return Ok("Thêm yêu cầu thành công");

            return BadRequest("Không thể thêm yêu cầu");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return _bus.Delete(id) ? Ok("Đã xóa") : BadRequest("Xóa thất bại");
        }

        [HttpGet("search")]
        public IActionResult Search(string key)
        {
            return Ok(_bus.Search(key));
        }

        // ===== EXPORT EXCEL =====
        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            var list = _bus.GetAll();
            if (list.Count == 0) return BadRequest("Không có dữ liệu");

            var stream = new MemoryStream();
            using (var pkg = new ExcelPackage())
            {
                var ws = pkg.Workbook.Worksheets.Add("DonYeuCau");
                ws.Cells[1, 1].LoadFromCollection(list, true);
                pkg.SaveAs(stream);
            }
            stream.Position = 0;

            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "DonYeuCau.xlsx");
        }
    }
}
