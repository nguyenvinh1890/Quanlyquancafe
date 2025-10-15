using Microsoft.AspNetCore.Mvc;
using QLCF.BUS;
using QLCF.DAL;
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
            if (!result.IsSuccess) return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] Mon mon)
        {
            var msg = _monBUS.Add(mon);
            return Ok(msg);
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] Mon mon)
        {
            var msg = _monBUS.Update(mon);
            return Ok(msg);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var msg = _monBUS.Delete(id);
            return Ok(msg);
        }
    }
}
