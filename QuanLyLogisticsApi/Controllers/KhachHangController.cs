using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;


namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPut]
        public IActionResult Update([FromBody] KhachHang kh)
        {
            try
            {
                if (string.IsNullOrEmpty(kh.MaKhachHang))
                    return BadRequest(new { error = "Thiếu mã khách hàng." });

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

        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            using var pkg = new ExcelPackage();
            var ws = pkg.Workbook.Worksheets.Add("KhachHang");

            ws.Cells[1, 1].Value = "Mã khách hàng";
            ws.Cells[1, 2].Value = "Tên khách hàng";
            ws.Cells[1, 3].Value = "Số điện thoại";
            ws.Cells[1, 4].Value = "Email";
            ws.Cells[1, 5].Value = "Ngày tạo";

            var list = _bus.GetAll();
            int r = 2;

            foreach (var x in list)
            {
                ws.Cells[r, 1].Value = x.MaKhachHang;
                ws.Cells[r, 2].Value = x.TenKhachHang;
                ws.Cells[r, 3].Value = x.SoDienThoai;
                ws.Cells[r, 4].Value = x.Email;
                ws.Cells[r, 5].Value = x.NgayTao;
                r++;
            }

            return File(pkg.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "KhachHang.xlsx");
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
                var d = new KhachHang
                {
                    MaKhachHang = ws.Cells[i, 1].Text,
                    TenKhachHang = ws.Cells[i, 2].Text,
                    SoDienThoai = ws.Cells[i, 3].Text,
                    Email = ws.Cells[i, 4].Text,
                    NgayTao = DateTime.TryParse(ws.Cells[i, 12].Text, out var nt) ? nt : DateTime.Now
                };
                _bus.Add(d);
            }

            return Ok("Import thành công!");
        }
    }
}
