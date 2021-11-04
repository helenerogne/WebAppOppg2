using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;

namespace WebAppOppg2.PassengerController
{
    [ApiController]

    [Route ("api/[controller]")] 

    public class PassengerController : ControllerBase
    {

        private ITicketRepository _db;
        private ILogger<PassengerController> _log;
        private const string _loggetInn = "loggetInn";

        public PassengerController(ITicketRepository db, ILogger<PassengerController> log)
        {
            _db = db;
            _log = log;
        }

        //SaveTicket
        [HttpPost]
        public async Task<ActionResult> SavePassenger(Passenger passenger)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.AddPassenger(passenger);
                if (!returOK)
                {
                    _log.LogInformation("Passasjer kunne ikke lagres!");
                    return BadRequest("");
                }
                return Ok("");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("");
        }


        //GetOne
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOnePassenger(int id)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                Passenger passenger = await _db.GetOnePassenger(id);
                if (passenger == null)
                {
                    _log.LogInformation("Fant ikke passasjer");
                    return NotFound();
                }
                return Ok(passenger);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        //GetALl
        [HttpGet]
        public async Task<ActionResult> GetAllPassengers()
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            */
            List<Passenger> passengers = await _db.GetAllPassengers();
            return Ok(passengers);
        }

        //DeleteTicket
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePassenger(int id)
        {
            bool returOK = await _db.DeletePassenger(id);
            if (!returOK)
            {
                _log.LogInformation("Sletting av passasjer ble ikke utført");
                return NotFound();
            }
            return Ok();

        }

        //EditTicket
        [HttpPut]
        public async Task<ActionResult> EditPassenger(Passenger passenger)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.EditPassenger(passenger);
                if (!returOK)
                {
                    _log.LogInformation("Endringen kunne ikke utføres");
                    return NotFound();
                }
                return Ok();
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }
    }
}
