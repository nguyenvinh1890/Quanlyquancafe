using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.Model;

namespace QLCF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaiTroController : ControllerBase
    {
        private readonly VaiTroBUS _bus;

        public VaiTroController(VaiTroBUS bus)
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
        public IActionResult Add([FromBody] VaiTro vt)
        {
            var msg = _bus.Add(vt);
            return Ok(msg);
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] VaiTro vt)
        {
            var msg = _bus.Update(vt);
            return Ok(msg);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var msg = _bus.Delete(id);
            return Ok(msg);
        }
    }
}
