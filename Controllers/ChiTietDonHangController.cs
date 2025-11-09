using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.Model;

namespace QLCF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietDonHangController : ControllerBase
    {
        private readonly ChiTietDonHangBUS _bus;

        public ChiTietDonHangController(ChiTietDonHangBUS bus)
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
        public IActionResult Add([FromBody] ChiTietDonHang ctdh)
        {
            var msg = _bus.Add(ctdh);
            return Ok(msg);
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] ChiTietDonHang ctdh)
        {
            var msg = _bus.Update(ctdh);
            return Ok(msg);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int maDH, int maMon)
        {
            var msg = _bus.Delete(maDH, maMon);
            return Ok(msg);
        }
        [HttpGet("get-by-donhang/{maDH}")]
        public IActionResult GetByDonHang(int maDH)
        {
            var result = _bus.GetByDonHang(maDH);
            if (!result.Item1) return BadRequest(result.Item2);
            return Ok(result.Item3);
        }
    }
}
