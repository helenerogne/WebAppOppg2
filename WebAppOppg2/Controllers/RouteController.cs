using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;

namespace WebAppOppg2.RouteController
{
    [ApiController]

    [Route ("api/[controller]")] 

    public class RouteController : ControllerBase
    {

        private ITicketRepository _db;
        private ILogger<RouteController> _log;
        private const string _loggetInn = "loggetInn";

        public RouteController(ITicketRepository db, ILogger<RouteController> log)
        {
            _db = db;
            _log = log;
        }

        //SaveTicket
        [HttpPost]
        public async Task<ActionResult> SaveRoute(Route route)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.AddRoute(route);
                if (!returOK)
                {
                    _log.LogInformation("Rute kunne ikke lagres!");
                    return BadRequest("");
                }
                return Ok("");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("");
        }


        //GetOne
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOneRoute(int id)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                Route route = await _db.GetOneRoute(id);
                if (route == null)
                {
                    _log.LogInformation("Fant ikke rute");
                    return NotFound();
                }
                return Ok(route);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        //GetALl
        [HttpGet]
        public async Task<ActionResult> GetAllRoutes()
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            */
            List<Route> routes = await _db.GetAllRoutes();
            return Ok(routes);
        }

        //DeleteTicket
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoute(int id)
        {
            bool returOK = await _db.DeleteRoute(id);
            if (!returOK)
            {
                _log.LogInformation("Sletting av ruten ble ikke utført");
                return NotFound();
            }
            return Ok();

        }

        //EditTicket
        [HttpPut]
        public async Task<ActionResult> EditRoute(Route route)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.EditRoute(route);
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
