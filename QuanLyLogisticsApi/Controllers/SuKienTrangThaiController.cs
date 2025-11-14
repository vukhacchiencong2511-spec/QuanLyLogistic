using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuKienTrangThaiController : ControllerBase
    {
        private readonly SuKienTrangThaiBUS _bus;

        public SuKienTrangThaiController(IConfiguration config)
        {
            _bus = new SuKienTrangThaiBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] SuKienTrangThai s)
        {
            if (_bus.Add(s))
                return Ok(new { message = "Thêm sự kiện trạng thái thành công" });
            return BadRequest(new { message = "Lỗi khi thêm" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] SuKienTrangThai s)
        {
            if (_bus.Update(s))
                return Ok(new { message = "Cập nhật thành công" });
            return BadRequest(new { message = "Cập nhật thất bại" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (_bus.Delete(id))
                return Ok(new { message = "Xóa thành công" });
            return BadRequest(new { message = "Lỗi khi xóa" });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var result = _bus.GetById(id);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy sự kiện." });
            return Ok(result);
        }

        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            using var pkg = new ExcelPackage();
            var ws = pkg.Workbook.Worksheets.Add("SuKienTrangThai");

            string[] cols = {
        "Mã sự kiện","Mã đơn","Trạng thái","Lý do",
        "Thời gian","Người cập nhật","Dữ liệu thêm",
        "Mã sự kiện ngoài","Khóa Idempotent","Ngày tạo"
    };

            for (int i = 0; i < cols.Length; i++)
                ws.Cells[1, i + 1].Value = cols[i];

            var list = _bus.GetAll();
            int r = 2;

            foreach (var x in list)
            {
                ws.Cells[r, 1].Value = x.MaSuKien;
                ws.Cells[r, 2].Value = x.MaDon;
                ws.Cells[r, 3].Value = x.TrangThai;
                ws.Cells[r, 4].Value = x.LyDo;
                ws.Cells[r, 5].Value = x.ThoiGian;
                ws.Cells[r, 6].Value = x.NguoiCapNhat;
                ws.Cells[r, 7].Value = x.DuLieuThem;
                ws.Cells[r, 8].Value = x.MaSuKienNgoai;
                ws.Cells[r, 9].Value = x.KhoaIdempotent;
                ws.Cells[r, 10].Value = x.NgayTao;
                r++;
            }

            return File(pkg.GetAsByteArray(),
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              "SuKienTrangThai.xlsx");
        }
    }
}
