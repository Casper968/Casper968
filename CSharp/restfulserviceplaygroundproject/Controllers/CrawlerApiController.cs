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
    
    
        [HttpGet("ListCarSeriesFromBrand")]
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

            List<CarSeries> resultList = new List<CarSeries>();
            var modelList = this._carDbContext.WorldCarModel.Where(x => x.BrandId == brandId).ToList();
            if (modelList != null && modelList.Count > 0)
            {
                foreach (var model in modelList)
                {
                    driver = new FirefoxDriver(FirefoxDriverService.CreateDefaultService(), new FirefoxOptions(), new TimeSpan(0,3,0));
                    string gotoUrl = model.Url;
                    //gotoUrl = "https://www.ultimatespecs.com/car-specs/BMW-models/BMW-2-Series";
                    driver.Navigate().GoToUrl(gotoUrl);

                    int modelId = model.ID;
                    string cssSelector = "div.home_models_line.gene a";
                    var seriesList = driver.FindElements(By.CssSelector(cssSelector));
                    foreach (var series in seriesList)
                    {
                        string seriesHref = series.GetAttribute("href");
                        bool isSeriesElement = !string.IsNullOrEmpty(seriesHref);
                        if (isSeriesElement) 
                        {
                            string seriesName = series.FindElement(By.CssSelector("div.centered h3")).Text.Trim();
                            string seriesImage = series.FindElement(By.TagName("img")).GetAttribute("src");

                            CarSeries newSeries = new CarSeries(){ ModelId = modelId, Name = seriesName, Url = seriesHref, ImageUrl = seriesImage };
                            
                            this._carDbContext.WorldCarSeries.Add(newSeries);
                            resultList.Add(newSeries);
                        }
                    }
                    //driver.FindElement(By.TagName("a")).Text.Trim();
                    this._carDbContext.SaveChanges();
                    driver.Close();
                }
                
                return Ok(Result.Ok(modelList));
            }

            return Ok();
        }
    
        [HttpGet("GetCarVersionFromSeries")]
        public ActionResult GetCarVersionFromSeries()
        {
            CarBrand bmw = this._carDbContext.WorldCarBrand.First(x => x.Name.Equals("Audi"));
            List<CarModel> bmwModelList = this._carDbContext.WorldCarModel.Where(x => x.BrandId.Equals(bmw.ID)).ToList();
            List<CarSeries> carSeriesList = this._carDbContext.WorldCarSeries.ToList();
            
            if (carSeriesList != null && carSeriesList.Count > 0)
            {
                foreach (var series in carSeriesList)
                {
                    if (!bmwModelList.Any(y => y.ID.Equals(series.ModelId)))
                    {
                        break;
                    }

                    int seriesId = series.ID;
                    driver = new FirefoxDriver(FirefoxDriverService.CreateDefaultService(), new FirefoxOptions(), new TimeSpan(0,3,0));
                    string gotoUrl = series.Url;
                    //gotoUrl = "https://www.ultimatespecs.com/car-specs/BMW/M7819/F20-LCI-1-Series-5-Doors";
                    driver.Navigate().GoToUrl(gotoUrl);

                    string cssSelector = "table.table_versions tr[class*='row']";
                    var versionList = driver.FindElements(By.CssSelector(cssSelector));

                    foreach (var versionRow in versionList)
                    {
                        IWebElement versionLink = versionRow.FindElement(By.TagName("a"));
                        bool hasSpecs = versionLink != null && !string.IsNullOrEmpty(versionLink.GetAttribute("href"));

                        if (hasSpecs)
                        {
                            string versionUrl = versionLink.GetAttribute("href");
                            string versionName = versionLink.Text;
                            int listingYear = 0;
                            int.TryParse( versionRow.FindElement(By.CssSelector("td:nth-child(2)")).Text.Trim(), out listingYear);

                            string rawPowOutput = versionRow.FindElement(By.CssSelector("td:nth-child(3)")).Text.Trim();
                            int power = 0;
                            int output = 0;
                            Regex rxColHoursePow = new Regex(@"(?<hourse>\d+) hp\s/\s(?<output>\d+)\skW", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            MatchCollection colPowOutputPow = rxColHoursePow.Matches(rawPowOutput);
                            var firstMatchGroupPower =  colPowOutputPow.FirstOrDefault();
                            if (firstMatchGroupPower != null)
                            {
                                int.TryParse(firstMatchGroupPower.Groups["hourse"].Value, out power);
                                int.TryParse(firstMatchGroupPower.Groups["output"].Value, out output);
                            }

                            string rawSizeCapacity = versionRow.FindElement(By.CssSelector("td:nth-child(4)")).Text.Trim();
                            int engineSize = 0;
                            decimal engineCapacity = 0;
                            Regex rxColEngine = new Regex(@"(?<size>\d+)\s[\w]+\d*\s\((?<capacity>\d+\.*\d*)\s\w*-*\w*\)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            MatchCollection colPowOutputEngine = rxColEngine.Matches(rawSizeCapacity);
                            var firstMatchGroupEngine =  colPowOutputEngine.FirstOrDefault();
                            if (firstMatchGroupEngine != null)
                            {
                                int.TryParse(firstMatchGroupEngine.Groups["size"].Value, out engineSize);
                                decimal.TryParse(firstMatchGroupEngine.Groups["capacity"].Value, out engineCapacity);
                            }

                            CarVersion currentVersion = new CarVersion() 
                            {
                                SeriesId = seriesId,
                                Name = versionName,
                                ListingYear = listingYear,
                                HorsePower = power,
                                MaximumPower = output,
                                EngineSize = engineSize,
                                EngineCapacity = engineCapacity,
                                Url = versionUrl
                            };

                            this._carDbContext.WorldCarVersion.Add(currentVersion);
                            this._carDbContext.SaveChanges();
                        }
                    }
                                
                    driver.Close();
                }
            }

            return Ok(carSeriesList);
        }
    
        [HttpGet("GetCarSpecFromVersion")]
        public ActionResult GetCarSpecFromVersion()
        {
            CarBrand bmw = this._carDbContext.WorldCarBrand.First(x => x.Name.Equals("BMW"));
            List<CarModel> bmwModel = this._carDbContext.WorldCarModel.Where(x => x.BrandId.Equals(bmw.ID)).ToList();
            List<CarSeries> bmwSeries = this._carDbContext.WorldCarSeries.ToList();
            bmwSeries = bmwSeries.Where(x => bmwModel.Any(y => y.ID.Equals(x.ModelId))).ToList();

            List<CarVersion> versionList = new List<CarVersion>();
            versionList = this._carDbContext.WorldCarVersion.ToList();
            versionList = versionList.Where(x => bmwSeries.Any(y => y.ID.Equals(x.SeriesId))).ToList();

            if (versionList != null && versionList.Count > 0)
            {
                foreach (var version in versionList)
                {
                    driver = new FirefoxDriver(FirefoxDriverService.CreateDefaultService(), new FirefoxOptions(), new TimeSpan(0,3,0));
                    string gotoUrl = version.Url;
                    //gotoUrl = "https://www.ultimatespecs.com/car-specs/BMW/70979/BMW-F20-LCI-1-Series-5-Doors-116i.html";
                    driver.Navigate().GoToUrl(gotoUrl);
                    Thread.Sleep(15);

                    string cssSelector = "table.content_text";
                    var specInfos = driver.FindElements(By.CssSelector(cssSelector));
                    if (specInfos != null && specInfos.Count >= 7)
                    {
                        IWebElement engineSpec = specInfos[0];
                        var engineSpecRow = engineSpec.FindElements(By.TagName("tr"));
                        foreach (var row in engineSpecRow)
                        {
                            var cell = row.FindElements(By.TagName("td"));
                            if (cell != null && cell.Count == 2)
                            {
                                CarEngineSpec spec = new CarEngineSpec() {
                                    VersionId = version.ID,
                                    SpecName = cell[0].Text,
                                    SpecValue = cell[1].Text
                                };

                                this._carDbContext.WorldCarEngineSpec.Add(spec);
                                this._carDbContext.SaveChanges();
                            }
                        }
                        
                        IWebElement fuelSpec = specInfos[1];
                        foreach (var row in fuelSpec.FindElements(By.TagName("tr")))
                        {
                            var cell = row.FindElements(By.TagName("td"));
                            if (cell != null && cell.Count == 2)
                            {
                                CarFuelSpec spec = new CarFuelSpec() {
                                    VersionId = version.ID,
                                    SpecName = cell[0].Text,
                                    SpecValue = cell[1].Text
                                };

                                this._carDbContext.WorldCarFuelSpec.Add(spec);
                                this._carDbContext.SaveChanges();
                            }
                        }

                        IWebElement perfSpec = specInfos[4];
                        foreach (var row in perfSpec.FindElements(By.TagName("tr")))
                        {
                            var cell = row.FindElements(By.TagName("td"));
                            if (cell != null && cell.Count == 2)
                            {
                                CarPrefSpec spec = new CarPrefSpec() {
                                    VersionId = version.ID,
                                    SpecName = cell[0].Text,
                                    SpecValue = cell[1].Text
                                };

                                this._carDbContext.WorldCarPrefSpec.Add(spec);
                                this._carDbContext.SaveChanges();
                            }
                        }

                        IWebElement sizeSpec = specInfos[6];
                        foreach (var row in sizeSpec.FindElements(By.TagName("tr")))
                        {
                            var cell = row.FindElements(By.TagName("td"));
                            if (cell != null && cell.Count == 2)
                            {
                                CarSizeSpec spec = new CarSizeSpec() {
                                    VersionId = version.ID,
                                    SpecName = cell[0].Text,
                                    SpecValue = cell[1].Text
                                };

                                this._carDbContext.WorldCarSizeSpec.Add(spec);
                                this._carDbContext.SaveChanges();
                            }
                        }
                    }

                    driver.Close();
                }
            }

            return Ok(versionList);
        }
    }
}