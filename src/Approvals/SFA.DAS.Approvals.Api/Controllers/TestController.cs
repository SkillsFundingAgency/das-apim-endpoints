﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TestController : Controller
    {
        
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(this.User.Identity);
        }

        [HttpGet("auth")]
        [Authorize]
        public IActionResult Auth()
        {
            return Ok(this.User.Identity.IsAuthenticated);
        }
    }
}
