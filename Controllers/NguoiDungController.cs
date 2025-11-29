using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.Model;

namespace QLCF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly NguoiDungBUS _bus;

        public NguoiDungController(NguoiDungBUS bus)
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
        public IActionResult Add([FromBody] NguoiDung nd)
        {
            var msg = _bus.Add(nd);
            // Kiểm tra nếu có lỗi (message chứa "đã tồn tại", "Lỗi", "Không thể")
            if (msg.Contains("đã tồn tại") || msg.Contains("Lỗi") || msg.Contains("Không thể"))
            {
                return BadRequest(new { message = msg });
            }
            return Ok(new { message = msg });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] NguoiDung nd)
        {
            var msg = _bus.Update(nd);
            return Ok(msg);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var msg = _bus.Delete(id);
            return Ok(msg);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Model.DTOs.LoginRequest request)
        {
            var result = _bus.Login(request);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            // Trả về token và user info (hiện tại chưa có JWT, tạm thời trả về fake token)
            
            return Ok(new
            {
                token = "jwt-token-" + result.Data?.MaND, // Tạm thời - cần thay bằng JWT thật
                user = new
                {
                    maND = result.Data?.MaND,
                    hoTen = result.Data?.HoTen,
                    taiKhoan = result.Data?.TaiKhoan,
                    tenVaiTro = result.Data?.TenVaiTro,
                    maVT = result.Data?.MaVT
                }
            });
        }
    }
}
