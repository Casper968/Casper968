using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CSharp.Routing
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        [Route("/articles/{page}")]
        public async ActionResult ArticlesList(int page)
        {
            return page;
            // this action cause error
        }

        // the following keywords are reserved in the context of a Razor view or a Razor page:
        /*
        page

        using

        namespace

        inject

        section

        inherits

        model

        addTagHelper

        removeTagHelper
        */
    }
}