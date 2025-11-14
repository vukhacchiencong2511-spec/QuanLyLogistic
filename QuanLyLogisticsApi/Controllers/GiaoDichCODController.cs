using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var result = _bus.GetById(id);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy giao dịch." });
            return Ok(result);
        }

        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            using var pkg = new ExcelPackage();
            var ws = pkg.Workbook.Worksheets.Add("GiaoDichCOD");

            string[] cols = {
        "Mã giao dịch","Mã đơn","Số tiền","Người thu",
        "Ngày thu","Đối soát","Ngày đối soát",
        "Số tiền thanh toán","Dữ liệu thêm"
    };

            for (int i = 0; i < cols.Length; i++)
                ws.Cells[1, i + 1].Value = cols[i];

            var list = _bus.GetAll();
            int r = 2;

            foreach (var x in list)
            {
                ws.Cells[r, 1].Value = x.MaGiaoDich;
                ws.Cells[r, 2].Value = x.MaDon;
                ws.Cells[r, 3].Value = x.SoTien;
                ws.Cells[r, 4].Value = x.NguoiThu;
                ws.Cells[r, 5].Value = x.NgayThu;
                ws.Cells[r, 6].Value = x.DaDoiSoat;
                ws.Cells[r, 7].Value = x.NgayDoiSoat;
                ws.Cells[r, 8].Value = x.SoTienThanhToan;
                ws.Cells[r, 9].Value = x.DuLieuThem;
                r++;
            }

            return File(pkg.GetAsByteArray(),
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              "GiaoDichCOD.xlsx");
        }
    }
}
