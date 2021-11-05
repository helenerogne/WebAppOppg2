using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;

namespace WebAppOppg2.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private ITicketRepository _db;
        private ILogger<AdminController> _log;
        private const string _loggetInn = "_loggetInn";
        private const string _ikkeLoggetInn = "";

        public AdminController(ITicketRepository db, ILogger<AdminController> log)
        {
            _db = db;
            _log = log;
        }

        [HttpPut]
        public async Task<ActionResult> EditAdmin(Admin admin)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Admin ikke endret - status: ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.EditAdmin(admin);
                if (!returOK)
                {
                    _log.LogInformation("Endringen kunne ikke utføres");
                    return NotFound("Admin ikke endret, feil i DB - status: innlogget");
                }
                return Ok("Admin endret - status: innlogget");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Admin ikke endret, feil ved inputvalideringen på server - status: innlogget");
        }

        [HttpPost]
        public async Task<ActionResult> LogIn(Admin admin)
        {
            if (ModelState.IsValid)
            {
                bool returnOK = await _db.LogIn(admin);
                if (!returnOK)
                {
                    _log.LogInformation("Innloggingen feilet for bruker");
                    HttpContext.Session.SetString(_loggetInn, _ikkeLoggetInn);
                    return Ok(false);
                }
                HttpContext.Session.SetString(_loggetInn, _loggetInn);
                return Ok(true);
            }
            _log.LogInformation("Feil ved inputvalideringen");
            return BadRequest("Feil ved inputvalideringen på server - status: innlogget");
        }

        [HttpDelete]
        public void LogOut()
        {
            HttpContext.Session.SetString(_loggetInn, "");
        }
    }
}
