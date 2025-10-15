using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.Model;

namespace QLCF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeMonController : ControllerBase
    {
        private readonly SizeMonBUS _bus;

        public SizeMonController(SizeMonBUS bus)
        {
            _bus = bus;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var result = _bus.GetAll();
            if (!result.Item1)
                return BadRequest(result.Item2);

            return Ok(new
            {
                success = result.Item1,
                message = result.Item2,
                data = result.Item3
            });
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] SizeMon size)
        {
            return Ok(new { message = _bus.Add(size) });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] SizeMon size)
        {
            return Ok(new { message = _bus.Update(size) });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(new { message = _bus.Delete(id) });
        }
    }
}
