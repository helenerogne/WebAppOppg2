using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;

namespace WebAppOppg2.TravelTypeController
{
    [ApiController]

    [Route("api/[controller]")]

    public class TravelTypeController : ControllerBase
    {

        private ITicketRepository _db;
        private ILogger<TravelTypeController> _log;
        private const string _loggetInn = "loggetInn";

        public TravelTypeController(ITicketRepository db, ILogger<TravelTypeController> log)
        {
            _db = db;
            _log = log;
        }

        [HttpPost]
        public async Task<ActionResult> SaveTravelType(TravelType travelType)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Reisetype ikke lagret - status: ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.AddTravelType(travelType);
                if (!returOK)
                {
                    _log.LogInformation("Traveltype kunne ikke lagres!");
                    return BadRequest("Reisetype ikke lagret, feil i DB - status: innlogget");
                }
                return Ok("Reisetype lagret - status: innlogget");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Reisetype ikke lagret, feil ved inputvalideringen på server - status: innlogget");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOneTravelType(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Reisetype ikke hentet, feil i DB - status: ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                TravelType travelType = await _db.GetOneTravelType(id);
                if (travelType == null)
                {
                    _log.LogInformation("Fant ikke traveltype");
                    return NotFound("Reisetype ikke hentet, feil i DB - status: innlogget");
                }
                return Ok(travelType);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTravelTypes()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Reisetype ikke hentet, feil i DB - status: ikke innlogget");
            }
            List<TravelType> travelTypes = await _db.GetAllTravelTypes();
            return Ok(travelTypes);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTravelType(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Reisetype ikke slettet, feil i DB - status: ikke innlogget");
            }
            bool returOK = await _db.DeleteTravelType(id);
            if (!returOK)
            {
                _log.LogInformation("Sletting av traveltype ble ikke utført");
                return NotFound("Reisetype ikke slettet, feil i DB - status: innlogget");
            }
            return Ok("Reisetype slettet - status: innlogget");

        }

        [HttpPut]
        public async Task<ActionResult> EditTravelType(TravelType travelType)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Reisetype ikke endret - status: ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.EditTravelType(travelType);
                if (!returOK)
                {
                    _log.LogInformation("Endringen kunne ikke utføres");
                    return NotFound("Reisetype ikke endret, feil i DB - status: innlogget");
                }
                return Ok("Reisetype endret - status: innlogget");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Reisetype ikke endret, feil ved inputvalideringen på server - status: innlogget");
        }
    }
}
