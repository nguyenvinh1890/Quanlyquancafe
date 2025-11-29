using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.Model;

namespace QLCF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly HoaDonBUS _bus;

        public HoaDonController(HoaDonBUS bus)
        {
            _bus = bus;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var result = _bus.GetAll();
            if (!result.Item1) return BadRequest(result.Item2);
            return Ok(result.Item3);
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] HoaDon hd)
        {
            var msg = _bus.Add(hd);
            // Kiểm tra nếu có lỗi validation (message chứa "đã có hóa đơn" hoặc "Lỗi")
            if (msg.Contains("đã có hóa đơn") || msg.Contains("Lỗi") || msg.Contains("Không thể"))
            {
                return BadRequest(new { message = msg });
            }
            return Ok(new { message = msg });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] HoaDon hd)
        {
            var msg = _bus.Update(hd);
            // Kiểm tra nếu có lỗi validation (message chứa "đã có hóa đơn" hoặc "Lỗi")
            if (msg.Contains("đã có hóa đơn") || msg.Contains("Lỗi") || msg.Contains("Không"))
            {
                return BadRequest(new { message = msg });
            }
            return Ok(new { message = msg });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var msg = _bus.Delete(id);
            // Kiểm tra nếu có lỗi validation (message chứa "Không thể xóa", "Lỗi", "Không tìm thấy")
            if (msg.Contains("Không thể xóa") || msg.Contains("Lỗi") || msg.Contains("Không tìm thấy"))
            {
                return BadRequest(new { message = msg });
            }
            return Ok(new { message = msg });
        }
    }
}
