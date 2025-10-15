using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.Model;

namespace QLCF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToppingController : ControllerBase
    {
        private readonly ToppingBUS _bus;

        public ToppingController(ToppingBUS bus)
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
        public IActionResult Add([FromBody] Topping topping)
        {
            return Ok(new { message = _bus.Add(topping) });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] Topping topping)
        {
            return Ok(new { message = _bus.Update(topping) });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(new { message = _bus.Delete(id) });
        }
    }
}
