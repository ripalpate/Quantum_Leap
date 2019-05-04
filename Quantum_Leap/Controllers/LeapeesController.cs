using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quantum_Leap.Data;
using Quantum_Leap.Models;
using Quantum_Leap.Validators;

namespace Quantum_Leap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeapeesController : ControllerBase
    {
        readonly LeapeeRepository _leaperRepository;
        readonly CreateLeapeeRequestValidator _validator;
        public LeapeesController()
        {
            _validator = new CreateLeapeeRequestValidator();
            _leaperRepository = new LeapeeRepository();

        }
        [HttpPost]
        public ActionResult AddLeaper(CreateLeapeeRequest createRequest)
        {
            if (_validator.Validate(createRequest))
            {
                return BadRequest(new { error = "leapee must have a name and profession" });
            }

            var newLeapee = _leaperRepository.AddLeapee(createRequest.Name, createRequest.Profession, createRequest.Gender, createRequest.LeaperId);

            return Created($"api/leapees/{newLeapee.Id}", newLeapee);

        }
    }
}