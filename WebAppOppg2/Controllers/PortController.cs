using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;

namespace WebAppOppg2.PortController
{
    [ApiController]

    [Route ("api/[controller]")] 

    public class PortController : ControllerBase
    {

        private ITicketRepository _db;
        private ILogger<PortController> _log;
        private const string _loggetInn = "loggetInn";

        public PortController(ITicketRepository db, ILogger<PortController> log)
        {
            _db = db;
            _log = log;
        }

        //SaveTicket
        [HttpPost]
        public async Task<ActionResult> SavePort(Port port)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.AddPort(port);
                if (!returOK)
                {
                    _log.LogInformation("port kunne ikke lagres!");
                    return BadRequest("");
                }
                return Ok("");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("");
        }


        //GetOne
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOnePort(int id)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                Port port = await _db.GetOnePort(id);
                if (port == null)
                {
                    _log.LogInformation("Fant ikke port");
                    return NotFound();
                }
                return Ok(port);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        //GetALl
        [HttpGet]
        public async Task<ActionResult> GetAllPorts()
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            */
            List<Port> ports = await _db.GetAllPorts();
            return Ok(ports);
        }

        //DeleteTicket
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePort(int id)
        {
            bool returOK = await _db.DeletePort(id);
            if (!returOK)
            {
                _log.LogInformation("Sletting av port ble ikke utført");
                return NotFound();
            }
            return Ok();

        }

        //EditTicket
        [HttpPut]
        public async Task<ActionResult> EditPort(Port port)
        {
            /*
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }*/
            if (ModelState.IsValid)
            {
                bool returOK = await _db.EditPort(port);
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
