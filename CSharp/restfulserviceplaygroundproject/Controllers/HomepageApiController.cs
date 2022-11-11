using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restfulserviceplaygroundproject.DatabaseContext;
using restfulserviceplaygroundproject.Infrastructure;
using restfulserviceplaygroundproject.Model;

namespace restfulserviceplaygroundproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomepageApiController : ControllerBase
    {
        private readonly HomeDbContext _homeDbContext;
        public HomepageApiController(HomeDbContext homeDbContext)
        {
            _homeDbContext = homeDbContext;
        }

        [HttpGet("Navigation")]
        public async Task<ActionResult<Result>> HomepageNaviationList()
        {
            List<HomeNavigation> homeNavigationList = new List<HomeNavigation>();
            var dbHomeNavigationList = _homeDbContext.HomeNavigation;

            if (dbHomeNavigationList != null)
            {
                homeNavigationList = await dbHomeNavigationList.ToListAsync();
            }

            return Result.Ok(homeNavigationList);
        }
    }
}