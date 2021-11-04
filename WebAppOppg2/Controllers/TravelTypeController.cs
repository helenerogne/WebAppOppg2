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

    [Route ("api/[controller]")]

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

        //SaveTicket
        [HttpPost]
        public async Task<ActionResult> SaveTravelType(TravelType travelType)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.AddTravelType(travelType);
                if (!returOK)
                {
                    _log.LogInformation("Traveltype kunne ikke lagres!");
                    return BadRequest("");
                }
                return Ok("");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("");
        }


        //GetOne
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOneTravelType(int id)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                TravelType travelType = await _db.GetOneTravelType(id);
                if (travelType == null)
                {
                    _log.LogInformation("Fant ikke traveltype");
                    return NotFound();
                }
                return Ok(travelType);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        //GetALl
        [HttpGet]
        public async Task<ActionResult> GetAllTravelTypes()
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            */
            List<TravelType> travelTypes = await _db.GetAllTravelTypes();
            return Ok(travelTypes);
        }

        //DeleteTicket
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTravelType(int id)
        {
            bool returOK = await _db.DeleteTravelType(id);
            if (!returOK)
            {
                _log.LogInformation("Sletting av traveltype ble ikke utført");
                return NotFound();
            }
            return Ok();

        }

        //EditTicket
        [HttpPut]
        public async Task<ActionResult> EditTravelType(TravelType travelType)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.EditTravelType(travelType);
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
