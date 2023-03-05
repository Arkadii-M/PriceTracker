using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozetkaParserService
{
    public class RozetkaParser
    {
        IWebDriver _driver;
        public RozetkaParser(IWebDriver driver)
        {
            _driver = driver;
        }
        public RozetkaProductPage GetProductPageByUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
            return new RozetkaProductPage(_driver);
        }
    }
}
