using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using restfulserviceplaygroundproject.DatabaseContext;
using restfulserviceplaygroundproject.Infrastructure;

namespace restfulserviceplaygroundproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrawlerApiController : ControllerBase
    {
        WebDriver driver;

        CarsDbContext _carDbContext;

        public CrawlerApiController(
            CarsDbContext carsDbContext)
        {
            driver = new FirefoxDriver();
            this._carDbContext = carsDbContext;
        }

        [HttpGet("ListCarBrands")]
        public ActionResult ListTotalCarBrands(string? cssSelector)
        {
            if (string.IsNullOrEmpty(cssSelector))
            {
                cssSelector = "div#car_make a";
            }
            List<string> brandList = new List<string>();
            driver.Navigate().GoToUrl("https://www.ultimatespecs.com/car-specs");
            var aTags = driver.FindElements(By.CssSelector(cssSelector));
            foreach (IWebElement tag in aTags)
            {
                var brandName = tag.Text;
                var brandUrl = tag.GetAttribute("href");

                bool isBrandExists = this._carDbContext.WorldCarBrand?.Any(x => x.Name == brandName) ?? false;

                Console.WriteLine(brandName);
                if (!isBrandExists && !string.IsNullOrEmpty(brandName) && !string.IsNullOrEmpty(brandUrl))
                {
                    brandList.Add(brandName);
                    this._carDbContext.WorldCarBrand?.Add(new Model.CarBrand() { Name = brandName, Url = brandUrl });
                }
            }
            driver.Close();
            this._carDbContext.SaveChanges();

            return Ok(Result.Ok(brandList));
        }
    }
}