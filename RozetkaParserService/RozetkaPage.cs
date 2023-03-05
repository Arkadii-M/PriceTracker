using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RozetkaDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozetkaParserService
{
    public class RozetkaProductPage
    {

        IWebDriver _driver;
        private readonly By title_selector;
        //private readonly By seller_name_selector;
        private readonly By price_selector;
        private readonly By in_stock_selector;
        const string prodcut_in_stock_str = "Є в наявності";
        public RozetkaProductPage(IWebDriver driver)
        {
            _driver = driver;
            title_selector = By.ClassName("product__title");
            //seller_name_selector = By.ClassName("product__title");
            price_selector = By.CssSelector("p.product-price__big");
            in_stock_selector = By.ClassName("status-label");
        }
        public string GetTitle(uint wait_sec = 10)
        {
            return new WebDriverWait(_driver, TimeSpan.FromSeconds(wait_sec))
                .Until(drv => drv.FindElement(By.ClassName("product__title")))
                .Text;
        }
        public string GetSellerName(uint wait_sec = 10)
        {
            return new WebDriverWait(_driver, TimeSpan.FromSeconds(wait_sec))
                .Until(drv => drv
                .FindElement(By.ClassName("product-seller__title"))
                .FindElement(By.CssSelector("rz-marketplace-link"))
                .FindElement(By.ClassName("ng-star-inserted"))).Text;
        }

        public decimal GetPrice(uint wait_sec = 10)
        {
            string price_str = new WebDriverWait(_driver, TimeSpan.FromSeconds(wait_sec))
                .Until(drv => drv.FindElement(By.CssSelector("p.product-price__big")))
                .Text;
            price_str = price_str.Replace(" ", "");// remove spaces
            price_str = price_str.Remove(price_str.Length - 1);// remove currency symbol

            return decimal.Parse(price_str, System.Globalization.CultureInfo.InvariantCulture);
        }
        public bool IsInStock(uint wait_sec = 10)
        {
            string in_stock_str = new WebDriverWait(_driver, TimeSpan.FromSeconds(30))
                .Until(drv => drv.FindElement(By.ClassName("status-label")))
                .Text;
            return in_stock_str == prodcut_in_stock_str;
        }


        public RozetkaPageResult ParsePage()
        {
            return new RozetkaPageResult() 
            {
                datetime = DateTime.Now,
                product_title = GetTitle(),
                price = GetPrice(),
                in_stock = IsInStock(),
                seller_name = GetSellerName()
            };
        }
    }
}
