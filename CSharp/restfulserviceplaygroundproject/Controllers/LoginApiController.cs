using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using restfulserviceplaygroundproject.Authentication;
using restfulserviceplaygroundproject.Infrastructure;

namespace restfulserviceplaygroundproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginApiController : ControllerBase
    {
        [HttpGet("GenerateJwt")]
        public Result GetJwtString(string username)
        {
            return Result.Ok(JwtGenerator.GenerateJwt(username));
        }
    }
}