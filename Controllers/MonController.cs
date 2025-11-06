using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.Model;

namespace QLCF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonController : ControllerBase
    {
        private readonly MonBUS _monBUS;

        public MonController(MonBUS monBUS)
        {
            _monBUS = monBUS;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var result = _monBUS.GetAll();
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            
            return Ok(new
            {
                success = result.IsSuccess,
                message = result.Message,
                data = result.Data
            });
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] Mon mon)
        {
            var msg = _monBUS.Add(mon);
            return Ok(new { message = msg });
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] Mon mon)
        {
            var msg = _monBUS.Update(mon);
            return Ok(new { message = msg });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var msg = _monBUS.Delete(id);
            return Ok(new { message = msg });
        }
        [HttpGet("check-sold/{id}")]
        public IActionResult CheckSold(int id)
        {
            var hasSold = _monBUS.CheckHasSold(id);
            return Ok(new { hasSold = hasSold });
        }
    }
}
