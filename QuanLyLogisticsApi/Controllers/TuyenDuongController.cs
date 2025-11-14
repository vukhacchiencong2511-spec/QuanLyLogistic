using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TuyenDuongController : ControllerBase
    {
        private readonly TuyenDuongBUS _bus;

        public TuyenDuongController(IConfiguration config)
        {
            _bus = new TuyenDuongBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] TuyenDuong t)
        {
            if (_bus.Add(t))
                return Ok(new { message = "Thêm tuyến đường thành công" });
            return BadRequest(new { message = "Lỗi khi thêm tuyến đường" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] TuyenDuong t)
        {
            if (_bus.Update(t))
                return Ok(new { message = "Cập nhật thành công" });
            return BadRequest(new { message = "Cập nhật thất bại" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (_bus.Delete(id))
                return Ok(new { message = "Xóa thành công" });
            return BadRequest(new { message = "Lỗi khi xóa tuyến đường" });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var item = _bus.GetById(id);
            if (item == null) return NotFound(new { message = "Không tìm thấy tuyến đường." });
            return Ok(item);
        }

        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            using var pkg = new ExcelPackage();
            var ws = pkg.Workbook.Worksheets.Add("TuyenDuong");

            string[] cols = {
        "Mã tuyến","Mã tuyến code","Mã tài xế","Phương tiện",
        "Thười gian bắt đầu","Thời gian kết thúc","Mã khu vực",
        "Doanh thu ước tính","Ngày tạo"
    };

            for (int i = 0; i < cols.Length; i++)
                ws.Cells[1, i + 1].Value = cols[i];

            var list = _bus.GetAll();
            int r = 2;

            foreach (var t in list)
            {
                ws.Cells[r, 1].Value = t.MaTuyen;
                ws.Cells[r, 2].Value = t.MaTuyenCode;
                ws.Cells[r, 3].Value = t.MaTaiXe;
                ws.Cells[r, 4].Value = t.PhuongTien;
                ws.Cells[r, 5].Value = t.ThoiGianBatDau;
                ws.Cells[r, 6].Value = t.ThoiGianKetThuc;
                ws.Cells[r, 7].Value = t.MaKhuVuc;
                ws.Cells[r, 8].Value = t.DoanhThuUocTinh;
                ws.Cells[r, 9].Value = t.NgayTao;
                r++;
            }

            return File(pkg.GetAsByteArray(),
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              "TuyenDuong.xlsx");
        }
    }
}
