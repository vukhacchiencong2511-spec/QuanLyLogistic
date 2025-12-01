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

        [HttpPost("import-excel")]
        public IActionResult ImportExcel(IFormFile file)
        {
            if (file == null) return BadRequest("Chưa chọn file.");

            using var st = new MemoryStream();
            file.CopyTo(st);

            using var pkg = new ExcelPackage(st);
            var ws = pkg.Workbook.Worksheets[0];
            int rows = ws.Dimension.Rows;

            for (int i = 2; i <= rows; i++)
            {
                var d = new DonYeuCau
                {
                    MaYeuCau = ws.Cells[i, 1].Text,
                    TenNguoiGui = ws.Cells[i, 2].Text,
                    SDTNguoiGui = ws.Cells[i, 3].Text,
                    EmailNguoiGui = ws.Cells[i, 4].Text,
                    DiaChiGui = ws.Cells[i, 5].Text,

                    TenNguoiNhan = ws.Cells[i, 6].Text,
                    SDTNguoiNhan = ws.Cells[i, 7].Text,
                    EmailNguoiNhan = ws.Cells[i, 8].Text,
                    DiaChiNhan = ws.Cells[i, 9].Text,

                    LoaiHang = ws.Cells[i, 10].Text,
                    KhoiLuong = decimal.TryParse(ws.Cells[i, 11].Text, out var kl) ? kl : 0,
                    GiaTriKhaiBao = decimal.TryParse(ws.Cells[i, 12].Text, out var gt) ? gt : 0,

                    GhiChu = ws.Cells[i, 13].Text,
                    NgayTao = DateTime.TryParse(ws.Cells[i, 14].Text, out var nt) ? nt : DateTime.Now
                };

                _bus.Add(d);
            }

            return Ok("Import DonYeuCau thành công!");
        }
    }
}
