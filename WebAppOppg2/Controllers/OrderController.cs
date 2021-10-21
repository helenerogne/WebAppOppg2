using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.DAL;


namespace WebAppOppg2.OrderController
{
    [ApiController]

    [Route ("api/[controller]")] 

    public class OrderController : ControllerBase
    {

        private ITicketRepository _db;
        private ILogger<OrderController> _log;
        private const string _loggetInn = "loggetInn";

        public OrderController(ITicketRepository db, ILogger<OrderController> log)
        {
            _db = db;
            _log = log;
        }

        //SaveTicket
        [HttpPost]
        public async Task<ActionResult> SaveTicket(Ticket ticket)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.SaveTicket(ticket);
                if (!returOK)
                {
                    _log.LogInformation("Billett kunne ikke lagres!");
                    return BadRequest("");
                }
                return Ok("");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("");
        }


        //GetOne
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOne(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                Ticket ticket = await _db.GetOne(id);
                if (ticket == null)
                {
                    _log.LogInformation("Fant ikke bilett");
                    return NotFound();
                }
                return Ok(ticket);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        //GetALl
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            List<Ticket> tickets = await _db.GetAll();
            return Ok(tickets);
        }

        //DeleteTicket
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTicket(int id)
        {
            bool returOK = await _db.DeleteTicket(id);
            if (!returOK)
            {
                _log.LogInformation("Sletting av biletten ble ikke utført");
                return NotFound();
            }
            return Ok();

        }

        //EditTicket
        [HttpPut]
        public async Task<ActionResult> EditTicket(Ticket ticket)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.EditTicket(ticket);
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
