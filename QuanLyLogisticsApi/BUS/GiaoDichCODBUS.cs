using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChungTuController : ControllerBase
    {
        private readonly ChungTuBUS _bus;

        public ChungTuController(IConfiguration config)
        {
            _bus = new ChungTuBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] ChungTu c)
        {
            if (_bus.Add(c))
                return Ok(new { message = "Thêm chứng từ thành công" });
            return BadRequest(new { message = "Lỗi khi thêm chứng từ" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] ChungTu c)
        {
            if (_bus.Update(c))
                return Ok(new { message = "Cập nhật thành công" });
            return BadRequest(new { message = "Lỗi khi cập nhật chứng từ" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (_bus.Delete(id))
                return Ok(new { message = "Xóa thành công" });
            return BadRequest(new { message = "Lỗi khi xóa chứng từ" });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var result = _bus.GetById(id);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy hóa đơn." });
            return Ok(result);
        }

        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            using var pkg = new ExcelPackage();
            var ws = pkg.Workbook.Worksheets.Add("ChungTu");

            ws.Cells[1, 1].Value = "Mã chứng từ";
            ws.Cells[1, 2].Value = "Mã đơn";
            ws.Cells[1, 3].Value = "Người upload";
            ws.Cells[1, 4].Value = "Ngày upload";
            ws.Cells[1, 5].Value = "Ký nhận";
            ws.Cells[1, 6].Value = "Đường dẫn ";
            ws.Cells[1, 7].Value = "Loại ký nhận";

            var list = _bus.GetAll();
            int r = 2;

            foreach (var x in list)
            {
                ws.Cells[r, 1].Value = x.MaChungTu;
                ws.Cells[r, 2].Value = x.MaDon;
                ws.Cells[r, 3].Value = x.NguoiUpload;
                ws.Cells[r, 4].Value = x.NgayUpload;
                ws.Cells[r, 5].Value = x.KyNhan;
                ws.Cells[r, 6].Value = x.DuongDanThuNho;
                ws.Cells[r, 7].Value = x.LoaiKyNhan;
                r++;
            }

            return File(pkg.GetAsByteArray(),
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              "ChungTu.xlsx");
        }
    }
}
