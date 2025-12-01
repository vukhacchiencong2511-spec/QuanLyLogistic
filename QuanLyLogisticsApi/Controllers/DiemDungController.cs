using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var item = _bus.GetById(id);
            if (item == null) return NotFound(new { message = "Không tìm thấy Điểm dừng." });
            return Ok(item);
        }

        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            using var pkg = new ExcelPackage();
            var ws = pkg.Workbook.Worksheets.Add("DiemDung");

            ws.Cells[1, 1].Value = "Mã điểm dừng";
            ws.Cells[1, 2].Value = "Mã tuyến";
            ws.Cells[1, 3].Value = "Thứ tự dừng";
            ws.Cells[1, 4].Value = "Mã đơn";
            ws.Cells[1, 5].Value = "Dự kiến đến";
            ws.Cells[1, 6].Value = "Thực tế đến";

            var list = _bus.GetAll();
            int r = 2;

            foreach (var x in list)
            {
                ws.Cells[r, 1].Value = x.MaDiemDung;
                ws.Cells[r, 2].Value = x.MaTuyen;
                ws.Cells[r, 3].Value = x.ThuTuDung;
                ws.Cells[r, 4].Value = x.MaDon;
                ws.Cells[r, 5].Value = x.DuKienDen;
                ws.Cells[r, 6].Value = x.ThucTeDen;
                r++;
            }

            return File(pkg.GetAsByteArray(),
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              "DiemDung.xlsx");
        }

        [HttpPost("import-excel")]
        public IActionResult ImportExcel(IFormFile file)
        {
            if (file == null) return BadRequest();

            using var st = new MemoryStream();
            file.CopyTo(st);

            using var pkg = new ExcelPackage(st);
            var ws = pkg.Workbook.Worksheets[0];
            int rows = ws.Dimension.Rows;

            for (int i = 2; i <= rows; i++)
            {
                var d = new DiemDung
                {
                    MaDiemDung = ws.Cells[i, 1].Text,
                    MaTuyen = ws.Cells[i, 2].Text,
                    ThuTuDung = int.TryParse(ws.Cells[i, 3].Text, out var tt) ? tt : 0,
                    MaDon = ws.Cells[i, 4].Text,
                    DuKienDen = DateTime.TryParse(ws.Cells[i, 5].Text, out var dk) ? dk : null,
                    ThucTeDen = DateTime.TryParse(ws.Cells[i, 6].Text, out var th) ? th : null
                };
                _bus.Add(d);
            }

            return Ok("Import thành công!");
        }
    }
}
