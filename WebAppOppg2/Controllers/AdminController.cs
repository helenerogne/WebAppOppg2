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
        private IAdminRepository _db;
        private ILogger<AdminController> _log;
        private const string _loggetInn = "loggetInn";

        public AdminController(IAdminRepository db, ILogger<AdminController> log)
        {
            _db = db;
            _log = log;
        }

        //EditAdmin
        [HttpPut]
        public async Task<ActionResult> EditAdmin(Admin admin)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.EditAdmin(admin);
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

        //LogIn
        //ikke sikker på om det er riktig Http som benyttes  
        [HttpPost]
        public async Task<ActionResult> LogIn(Admin admin)
        {
            if (ModelState.IsValid)
            {
                bool returnOK = await _db.LogIn(admin);
                if (!returnOK)
                {
                    _log.LogInformation("Innloggingen feilet for bruker" + admin.Username);
                    HttpContext.Session.SetString(_loggetInn, "");
                    return Ok(false);
                }
                HttpContext.Session.SetString(_loggetInn, "LoggetInn");
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        //LogOut
        //ikke sikker på om det er riktig Http benyttes  
        [HttpDelete]
        public void LogOut()
        {
            HttpContext.Session.SetString(_loggetInn, "");
        }
    }
}
