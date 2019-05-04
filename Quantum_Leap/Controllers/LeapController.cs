using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quantum_Leap.Data;
using Quantum_Leap.Models;

namespace Quantum_Leap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeapController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetLeap()
        {
            var repository = new LeapRepository();
            var getLeap = repository.GetLeap();
            return Ok(getLeap);
        }
    }
}