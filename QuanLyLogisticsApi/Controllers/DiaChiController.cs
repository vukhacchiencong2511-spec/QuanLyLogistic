using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiaChiController : ControllerBase
    {
        private readonly DiaChiBUS _bus;

        public DiaChiController(IConfiguration config)
        {
            _bus = new DiaChiBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] DiaChi d)
        {
            if (_bus.Add(d))
                return Ok(new { message = "Thêm địa chỉ thành công" });
            return BadRequest(new { message = "Lỗi khi thêm địa chỉ" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] DiaChi d)
        {
            if (_bus.Update(d))
                return Ok(new { message = "Cập nhật địa chỉ thành công" });
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
            var diaChi = _bus.GetById(id);
            if (diaChi == null)
                return NotFound(new { message = "Không tìm thấy địa chỉ." });
            return Ok(diaChi);
        }

        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            using var pkg = new ExcelPackage();
            var ws = pkg.Workbook.Worksheets.Add("DiaChi");

            ws.Cells[1, 1].Value = "Mã địa chỉ";
            ws.Cells[1, 2].Value = "Mã khách hàng";
            ws.Cells[1, 3].Value = "Địa chỉ chi tiết";
            ws.Cells[1, 4].Value = "Thành phố";
            ws.Cells[1, 5].Value = "Phường";
            ws.Cells[1, 6].Value = "Mã bưu điện";

            var list = _bus.GetAll();
            int r = 2;

            foreach (var x in list)
            {
                ws.Cells[r, 1].Value = x.MaDiaChi;
                ws.Cells[r, 2].Value = x.MaKhachHang;
                ws.Cells[r, 3].Value = x.DiaChiChiTiet;
                ws.Cells[r, 4].Value = x.ThanhPho;
                ws.Cells[r, 5].Value = x.QuanHuyen;
                ws.Cells[r, 6].Value = x.MaBuuDien;
                r++;
            }

            return File(pkg.GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "DiaChi.xlsx");
        }
    }
}
