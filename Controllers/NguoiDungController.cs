using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.Model;
using QLCF.Model.DTOs;

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

        //  Lấy danh sách tất cả người dùng
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var (success, message, data) = _bus.GetAll();
            if (!success)
                return BadRequest(new { success = false, message });

            return Ok(new { success = true, message, data });
        }

        //  Thêm người dùng mới
        [HttpPost("add")]
        public IActionResult Add([FromBody] NguoiDung nd)
        {
            if (nd == null)
                return BadRequest(new { success = false, message = "Dữ liệu người dùng không hợp lệ" });

            var msg = _bus.Add(nd);
            return Ok(new { success = true, message = msg });
        }

        //  Cập nhật thông tin người dùng
        [HttpPut("update")]
        public IActionResult Update([FromBody] NguoiDung nd)
        {
            if (nd == null)
                return BadRequest(new { success = false, message = "Dữ liệu người dùng không hợp lệ" });

            var msg = _bus.Update(nd);
            return Ok(new { success = true, message = msg });
        }

        //  Xóa người dùng theo ID
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { success = false, message = "ID không hợp lệ" });

            var msg = _bus.Delete(id);
            return Ok(new { success = true, message = msg });
        }

        //  Đăng nhập
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Dữ liệu đăng nhập không hợp lệ" });

            var result = _bus.Login(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
