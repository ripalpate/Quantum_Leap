using System;
using System.Collections.Generic;
using System.Linq;
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
                return BadRequest(new { error = "leaper must have a name" });
            }

            var newLeaper = _leaperRepository.AddLeaper(createRequest.LeaperName, createRequest.Age, createRequest.BudgetAmount);

            return Created($"api/leapers/{newLeaper.Id}", newLeaper);
        }

        [HttpGet]
        public ActionResult GetAllLeapers()
        {
            var leapers = _leaperRepository.GetAllLeapers();
            return Ok(leapers);
        }
    }
}