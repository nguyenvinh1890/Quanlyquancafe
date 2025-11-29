using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.Model;

namespace QLCF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly DonHangBUS _bus;

        public DonHangController(DonHangBUS bus)
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
        public IActionResult Add([FromBody] DonHang donHang)
        {
            var message = _bus.Add(donHang);
            return Ok(message);
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] DonHang donHang)
        {
            var message = _bus.Update(donHang);
            return Ok(message);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var message = _bus.Delete(id);
            return Ok(message);
        }

        [HttpGet("get-tong-tien/{maDH}")]
        public IActionResult GetTongTien(int maDH)
        {
            var result = _bus.GetTongTien(maDH);
            if (!result.Item1) return BadRequest(result.Item2);
            return Ok(new { tongTien = result.Item3 });
        }
    }
}
