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

    [Route ("api/[controller]")]

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

        //SaveTicket
        [HttpPost]
        public async Task<ActionResult> SavePassenger(PassengerType passengerType)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.AddPassengerType(passengerType);
                if (!returOK)
                {
                    _log.LogInformation("passengertype kunne ikke lagres!");
                    return BadRequest("");
                }
                return Ok("");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("");
        }


        //GetOne
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOnePassengerType(int id)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                PassengerType passengerType = await _db.GetOnePassengerType(id);
                if (passengerType == null)
                {
                    _log.LogInformation("Fant ikke passengertype");
                    return NotFound();
                }
                return Ok(passengerType);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        //GetALl
        [HttpGet]
        public async Task<ActionResult> GetAllPassengerTypes()
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            */
            List<PassengerType> passengerTypes = await _db.GetAllPassengerTypes();
            return Ok(passengerTypes);
        }

        //DeleteTicket
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePassengerType(int id)
        {
            bool returOK = await _db.DeletePassengerType(id);
            if (!returOK)
            {
                _log.LogInformation("Sletting av passengertype ble ikke utført");
                return NotFound();
            }
            return Ok();

        }

        //EditTicket
        [HttpPut]
        public async Task<ActionResult> EditPassengerType(PassengerType passengerType)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.EditPassengerType(passengerType);
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
