using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;

namespace WebAppOppg2.PassengerTypeController
{
    [ApiController]

    [Route("api/[controller]")]

    public class PassengerTypeController : ControllerBase
    {

        private ITicketRepository _db;
        private ILogger<PassengerTypeController> _log;
        private const string _loggetInn = "loggetInn";

        public PassengerTypeController(ITicketRepository db, ILogger<PassengerTypeController> log)
        {
            _db = db;
            _log = log;
        }

        [HttpPost]
        public async Task<ActionResult> SavePassenger(PassengerType passengerType)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Passasjertype ikke lagret - status: ikke innlogget");
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.AddPassengerType(passengerType);
                if (!returOK)
                {
                    _log.LogInformation("passengertype kunne ikke lagres!");
                    return BadRequest("Passasjertype ikke lagret, feil i DB - status: innlogget");
                }
                return Ok("Passasjertype lagret - status: innlogget");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Passasjertype ikke lagret, feil ved inputvalideringen på server - status: innlogget");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOnePassengerType(int id)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Passasjertype ikke hentet, feil i DB - status: ikke innlogget");
            }*/
            if (ModelState.IsValid)
            {
                PassengerType passengerType = await _db.GetOnePassengerType(id);
                if (passengerType == null)
                {
                    _log.LogInformation("Fant ikke passengertype");
                    return NotFound("Passasjertype ikke hentet, feil i DB - status: innlogget");
                }
                return Ok(passengerType);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPassengerTypes()
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Passasjertyper ikke hentet, feil i DB - status: ikke innlogget");
            }*/
            List<PassengerType> passengerTypes = await _db.GetAllPassengerTypes();
            return Ok(passengerTypes);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePassengerType(int id)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Passasjertype ikke slettet, feil i DB - status: ikke innlogget");
            }*/
            bool returOK = await _db.DeletePassengerType(id);
            if (!returOK)
            {
                _log.LogInformation("Sletting av passengertype ble ikke utført");
                return NotFound("Passasjertype ikke slettet, feil i DB - status: innlogget");
            }
            return Ok("Passasjertype slettet - status: innlogget");
        }

        [HttpPut]
        public async Task<ActionResult> EditPassengerType(PassengerType passengerType)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Passasjertype ikke endret - status: ikke innlogget");
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.EditPassengerType(passengerType);
                if (!returOK)
                {
                    _log.LogInformation("Endringen kunne ikke utføres");
                    return NotFound("Passasjertype ikke endret, feil i DB - status: innlogget");
                }
                return Ok("Passasjertype endret - status: innlogget");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Passasjertype ikke endret, feil ved inputvalideringen på server - status: innlogget");
        }
    }
}
