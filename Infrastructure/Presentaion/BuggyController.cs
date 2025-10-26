using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Presentaion
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            return NotFound(); // Returns a 404 Not Found response
        }
        [HttpGet("servererror")]
        public ActionResult GetServerErrorRequest()
        {
            throw new Exception(); // returns a 500 Internal Server Error response
            return Ok();
        }
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(); // Returns a 400 Bad Request response
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return BadRequest(); // Returns a 400 Bad Request response
        }

        [HttpGet("unauthorized")]
        public ActionResult GetUnauthorizedRequest()
        {
            return Unauthorized(); // Returns a 401 Unauthorized response
        }

    }
}
