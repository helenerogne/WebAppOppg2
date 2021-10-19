using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAppOppg2.OrderController
{
    [Route("api/[controller]")]
    public class OrderController
    {
        /*
        private ILogger<OrderController> _log;
        private readonly ITicketRepository _db;

        public OrderController(ITicketRepository db, ILogger<OrderController> log)
        {
            _db = db;
            _log = log;
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrder(Ticket inTicket)
        {
            bool returOK = await _db.saveOrder(inTicket);
            if (!returOK)
            {
                _log.LogInformation("Bestilling ble ikke lagret");
                return BadRequest("Bestilling ble ikke lagret");
            }
            return Ok(inTicket);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetOne(int id)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = await _db.GetOne(id);
                if (ticket == null)
                {
                    _log.LogInformation("Fant ikke kunden");
                    return NotFound("Fant ikke kunden");
                }
                return Ok(ticket);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        */

    }
}
