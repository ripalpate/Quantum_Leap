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
            var getLeap = repository.GetAllLeap();
            return Ok(getLeap);
        }

        [HttpPost]
        public ActionResult AddLeapee(CreateLeapRequest createRequest)
        {
            var repository = new LeapRepository();
            var randomLeaper = repository.getRandomLeaper();
            int @leaperId = randomLeaper.Id;
            int @leapeeId = repository.getRandomLeapee().Id;
            int @eventId = 0;
            var eventAssociatedWithLeapee = repository.getEventAssociatedWithLeapee(leapeeId);

            if (eventAssociatedWithLeapee != null)
            {
                eventId = eventAssociatedWithLeapee.Id;
            } else
            {
                return NotFound("Event already exist in another leap for that leapee. Please try again");
            }
            
            if (randomLeaper.BudgetAmount > createRequest.Cost)
            {
                var newLeap = repository.insertLeapAndUpdateBudget(leaperId, leapeeId, eventId, createRequest.Cost);
                return Created($"api/leapees/{newLeap.Id}", newLeap);
            }
            else
            {
                return BadRequest("Sorry, you can not leap because you don't have enough budget. Better luck next time");
            }

           
           
           

            

        }
    }
}