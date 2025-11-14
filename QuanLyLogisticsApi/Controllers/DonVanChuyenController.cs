using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QuanLyLogisticsApi.BUS;
using QuanLyLogisticsApi.Models;

namespace QuanLyLogisticsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonVanChuyenController : ControllerBase
    {
        private readonly DonVanChuyenBUS _bus;

        public DonVanChuyenController(IConfiguration config)
        {
            _bus = new DonVanChuyenBUS(config);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_bus.GetAll());

        [HttpPost]
        public IActionResult Add([FromBody] DonVanChuyen d)
        {
            if (_bus.Add(d))
                return Ok(new { message = "Thêm đơn vận chuyển thành công" });
            return BadRequest(new { message = "Lỗi khi thêm" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] DonVanChuyen d)
        {
            if (_bus.Update(d))
                return Ok(new { message = "Cập nhật thành công" });
            return BadRequest(new { message = "Cập nhật thất bại" });
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
            var item = _bus.GetById(id);
            if (item == null) return NotFound(new { message = "Không tìm thấy đơn vận chuyển." });
            return Ok(item);
        }


        [HttpGet("export-excel")]
        public IActionResult ExportExcel()
        {
            try
            {
                using var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add("DonVanChuyen");

                // Tiêu đề cột
                ws.Cells[1, 1].Value = "Mã đơn";
                ws.Cells[1, 2].Value = "Mã đơn Code";
                ws.Cells[1, 3].Value = "Mã vận đơn";
                ws.Cells[1, 4].Value = "Mã khách gửi";
                ws.Cells[1, 5].Value = "Mã khách nhận";
                ws.Cells[1, 6].Value = "Mã địa chỉ lấy";
                ws.Cells[1, 7].Value = "Mã địa chỉ giao";
                ws.Cells[1, 8].Value = "Loại hàng";
                ws.Cells[1, 9].Value = "Khối lượng";
                ws.Cells[1, 10].Value = "Giá trị khai báo";
                ws.Cells[1, 11].Value = "Người tạo";
                ws.Cells[1, 12].Value = "Ngày tạo";
                ws.Cells[1, 13].Value = "Mã tuyến";
                ws.Cells[1, 14].Value = "Trạng thái";

                // Dữ liệu
                var list = _bus.GetAll();
                int row = 2;
                foreach (var d in list)
                {
                    ws.Cells[row, 1].Value = d.MaDon;
                    ws.Cells[row, 2].Value = d.MaDonCode;
                    ws.Cells[row, 3].Value = d.MaVanDon;
                    ws.Cells[row, 4].Value = d.MaKhachGui;
                    ws.Cells[row, 5].Value = d.MaKhachNhan;
                    ws.Cells[row, 6].Value = d.MaDiaChiLay;
                    ws.Cells[row, 7].Value = d.MaDiaChiGiao;
                    ws.Cells[row, 8].Value = d.LoaiHang;
                    ws.Cells[row, 9].Value = d.KhoiLuong;
                    ws.Cells[row, 10].Value = d.GiaTriKhaiBao;
                    ws.Cells[row, 11].Value = d.NguoiTao;
                    ws.Cells[row, 12].Value = d.NgayTao;
                    ws.Cells[row, 13].Value = d.MaTuyen;
                    ws.Cells[row, 14].Value = d.TrangThai;
                    row++;
                }

                // Trả file
                var bytes = package.GetAsByteArray();
                return File(
                    bytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "DonVanChuyen.xlsx"
                );
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi export: " + ex.Message);
            }
        }
    }
}

