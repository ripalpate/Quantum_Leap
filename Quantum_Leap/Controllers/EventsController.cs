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
    public class EventsController : ControllerBase
    {
        readonly EventRepository _eventRepository;
        readonly CreateEventRequestValidator _validator;
        public EventsController()
        {
            _validator = new CreateEventRequestValidator();
            _eventRepository = new EventRepository();

        }
        [HttpPost]
        public ActionResult AddEvent(CreateEventRequest createRequest)
        {
            if (_validator.Validate(createRequest))
            {
                return BadRequest(new { error = "event must have a name"});
            }

            var newEvent = _eventRepository.AddEvent(
                createRequest.EventName, 
                createRequest.Description, 
                createRequest.Date, 
                createRequest.Location,
                createRequest.IsCorrected,
                createRequest.LeapeeId
                );

            return Created($"api/events/{newEvent.Id}", newEvent);

        }

        [HttpGet]
        public ActionResult GetAllEvents()
        {
            var events = _eventRepository.GetAllEvents();
            return Ok(events);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEvent(int id)
        {
            _eventRepository.DeleteEvent(id);
            return Ok($"Event with id {id} is deleted");
        }
    }
}