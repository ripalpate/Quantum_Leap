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
    public class LeapersController : ControllerBase
    {
        readonly LeaperRepository _leaperRepository;
        readonly CreateLeaperRequestValidator _validator;
        public LeapersController()
        {
            _validator = new CreateLeaperRequestValidator();
            _leaperRepository = new LeaperRepository();

        }
        [HttpPost]
        public ActionResult AddLeaper(CreateLeaperRequest createRequest)
        {
            if (_validator.Validate(createRequest))
            {
                return BadRequest(new { error = "users must have a name and age" });
            }

            var newLeaper = _leaperRepository.AddLeaper(createRequest.Name, createRequest.Age);

            return Created($"api/leapers/{newLeaper.Id}", newLeaper);

        }
    }
}