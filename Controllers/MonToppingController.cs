using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.Model;

namespace QLCF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonToppingController : ControllerBase
    {
        private readonly MonToppingBUS _bus;

        public MonToppingController(MonToppingBUS bus)
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
        public IActionResult Add([FromBody] MonTopping mt)
        {
            var msg = _bus.Add(mt);
            return Ok(msg);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int maMon, int maTopping)
        {
            var msg = _bus.Delete(maMon, maTopping);
            return Ok(msg);
        }
    }
}
