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

    [Route("api/[controller]")]

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

        [HttpPost]
        public async Task<ActionResult> SavePort(Port port)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Havn ikke lagret - status: ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.AddPort(port);
                if (!returOK)
                {
                    _log.LogInformation("Havn kunne ikke lagres!");
                    return BadRequest("Havn ikke lagret, feil i DB - status: innlogget");
                }
                return Ok("Havn lagret - status: innlogget");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Havn ikke lagret, feil ved inputvalideringen på server - status: innlogget");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOnePort(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Havn ikke hentet, feil i DB - status: ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                Port port = await _db.GetOnePort(id);
                if (port == null)
                {
                    _log.LogInformation("Fant ikke port");
                    return NotFound("Havn ikke hentet, feil i DB - status: innlogget");
                }
                return Ok(port);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPorts()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Havner ikke hentet, feil i DB - status: ikke innlogget");
            }
            List<Port> ports = await _db.GetAllPorts();
            return Ok(ports);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePort(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Havn ikke slettet, feil i DB - status: ikke innlogget");
            }
            bool returOK = await _db.DeletePort(id);
            if (!returOK)
            {
                _log.LogInformation("Sletting av port ble ikke utført");
                return NotFound("Havn ikke slettet, feil i DB - status: innlogget");
            }
            return Ok("Havn slettet - status: innlogget");

        }

        [HttpPut]
        public async Task<ActionResult> EditPort(Port port)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Havn ikke endret - status: ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.EditPort(port);
                if (!returOK)
                {
                    _log.LogInformation("Endringen kunne ikke utføres");
                    return NotFound("Havn ikke endret, feil i DB - status: innlogget");
                }
                return Ok("Havn endret - status: innlogget");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Havn ikke endret, feil ved inputvalideringen på server - status: innlogget");
        }
    }
}
