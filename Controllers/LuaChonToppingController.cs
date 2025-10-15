using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.Model;

namespace QLCF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LuaChonToppingController : ControllerBase
    {
        private readonly LuaChonToppingBUS _bus;

        public LuaChonToppingController(LuaChonToppingBUS bus)
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
        public IActionResult Add([FromBody] LuaChonTopping obj)
        {
            var msg = _bus.Add(obj);
            return Ok(msg);
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] LuaChonTopping obj)
        {
            var msg = _bus.Update(obj);
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
