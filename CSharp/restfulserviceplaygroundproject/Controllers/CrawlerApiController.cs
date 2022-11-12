using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using restfulserviceplaygroundproject.DatabaseContext;
using restfulserviceplaygroundproject.Infrastructure;
using restfulserviceplaygroundproject.Model;

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

        [HttpGet("ListCarModelFromBrand")]
        public ActionResult ListModelsFromBrand(string? brandName)
        {
            if (string.IsNullOrEmpty(brandName))
            {
                return Ok();
            }

            List<CarModel> modelList = new List<CarModel>();
            var brandList = this._carDbContext?.WorldCarBrand?.Where(x => x.Name.IndexOf(brandName) == 0).ToList();
            foreach (CarBrand brand in brandList)
            {
                int brandId = brand.ID;
                driver = new FirefoxDriver(FirefoxDriverService.CreateDefaultService(), new FirefoxOptions(), new TimeSpan(0,3,0));
                ITimeouts timeouts = driver.Manage().Timeouts();
                timeouts.ImplicitWait = new TimeSpan(0, 0, 10);
                timeouts.PageLoad = new TimeSpan(0, 3, 0);
                timeouts.AsynchronousJavaScript = new TimeSpan(0, 1, 0);

                //driver.Navigate().GoToUrl(brand.Url);
                driver.ExecuteScript("window.location.href='" + brand.Url.Replace("-models", "") + "'");
                Thread.Sleep(10000);
                
                var aTags = driver.FindElements(By.CssSelector("div.home_models_line a"));

                if (aTags?.Count > 0)
                {
                    for (var ind = 0; ind < aTags.Count; ind++)
                    {
                        IWebElement x = aTags[ind];
                        string modelName = x.FindElement(By.TagName("h2")).Text;
                        string caption = x.FindElement(By.TagName("p")).Text.Trim();
                        string fromYearPattern = @"From\s(\d\d\d\d)";
                        string generationPattern = @"(\d\d*) Generation";
                        string modelPattern = @"(\d\d*) Model";
                        string versionPattern = @"(\d\d*) Version";
                        string imageUrl = x.FindElement(By.TagName("img")).GetAttribute("src");
                        int yearFrom = 0;
                        var matchResult = Regex.Matches(caption, fromYearPattern);
                        if (matchResult.Count > 0)
                        {
                        int.TryParse(matchResult.First()?.Groups[1].Value, out yearFrom);
                        }

                        int generation = 0;
                        matchResult = Regex.Matches(caption, generationPattern);
                        if (matchResult.Count > 0)
                        {
                            int.TryParse(matchResult.First()?.Groups[1].Value, out generation);                        
                        }

                        int model = 0;
                        matchResult = Regex.Matches(caption, modelPattern);
                        if (matchResult.Count > 0)
                        {
                            int.TryParse(matchResult.First()?.Groups[1].Value, out model);                        
                        }
                        
                        int version = 0;
                        matchResult = Regex.Matches(caption, versionPattern);
                        if (matchResult.Count > 0)
                        {
                            int.TryParse(matchResult.First()?.Groups[1].Value, out version);                        
                        }
                        
                        string modelHref = x.GetAttribute("href");
                        CarModel input = new CarModel(){ 
                            BrandId = brandId,
                            Name = modelName, 
                            ListingYear = yearFrom,
                            Generation = generation,
                            Version = version,
                            Model = model,
                            ImageUrl = imageUrl,
                            Url = modelHref };
                        modelList.Add(input);

                        if (!this._carDbContext.WorldCarModel.Any(x => x.Name == input.Name && x.BrandId == input.BrandId))
                        {
                            this._carDbContext.WorldCarModel.Add(input);
                            this._carDbContext.SaveChanges();
                        }
                    }
                }

                driver.Close();
            }

            return Ok(modelList);
        }
    
    
        [HttpGet("ListCarVersionFromBrand")]
        public ActionResult ListVersionsFromBrand(string brandName)
        {
            int brandId = 0;
            var targetBrand = this._carDbContext.WorldCarBrand.FirstOrDefault(x => x.Name == brandName);
            if (targetBrand != null)
            {
                brandId = targetBrand.ID;
            }

            if (brandId == 0)
            {
                return Ok();
            }

            var modelList = this._carDbContext.WorldCarModel.Where(x => x.BrandId == brandId).ToList();
            if (modelList != null && modelList.Count > 0)
            {
                return Ok(Result.Ok(modelList));
            }

            return Ok();
        }
    }
}