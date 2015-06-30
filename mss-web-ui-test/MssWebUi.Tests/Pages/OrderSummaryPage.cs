using System;
using MAG.WebTesting.BasicElements;
using MAG.WebTesting.Browsers;
using MAG.WebTesting.Pages;
using MssWebUi.Tests.Utilities;
using OpenQA.Selenium;

namespace MssWebUi.Tests.Pages
{
    public class OrderSummaryPage : BasePage
    {
        private readonly CommonMethods _commonFunctions;

        public OrderSummaryPage(IBrowserTestingSession testingSession) : base(testingSession, "")
        {
            _commonFunctions = new CommonMethods(TestingSession);
        }

        public string GetShippingInformation()
        {
            return TestingSession.GetDriver<TextBox>(By.Id("ContentPlaceHolder1_shippedToInformation")).GetText();
        }

        public string GetBillingInformation()
        {
            return
                TestingSession.GetDriver<TextBox>(
                    By.XPath("//section[@id='ContentPlaceHolder1_orderResults']/div/div/div[2]/div/section[2]/p"))
                    .GetText();
        }

        public string GetShippingMethod()
        {
            return
                TestingSession.GetDriver<TextBox>(
                    By.XPath("//section[@id='ContentPlaceHolder1_orderResults']//section/p[2]")).GetText();
        }

        public string GetPaymentDetails()
        {
            return
                TestingSession.GetDriver<TextBox>(
                    By.XPath("//section[@id='ContentPlaceHolder1_orderResults']//section[2]/p[2]")).GetText();
        }

        public string GetExpectedArrivalDate()
        {
            return
                TestingSession.GetDriver<TextBox>(
                    By.XPath("//section[@id='ContentPlaceHolder1_orderResults']//div[2]/div/section[2]/p[3]/span"))
                    .
                    GetText();
        }

        public string GetOrderNumber()
        {
            return
                TestingSession.GetDriver<TextBox>(By.XPath("//section[@id='ContentPlaceHolder1_orderResults']//strong/a"))
                    .GetText();
        }

        public decimal GetProductPrice()
        {
            if
                (TestingSession.GetDriver<TextBox>(
                    By.XPath("//section[@id='ContentPlaceHolder1_orderResults']//section/div/article/div[2]/div/p[2]"))
                    .GetText()
                    .Contains("You saved"))
            {
                return
                    _commonFunctions.GetSavings(
                        By.XPath(
                            "//section[@id='ContentPlaceHolder1_orderResults']//section/div/article/div[2]/div/p[3]"), 1);
            }
            return _commonFunctions.GetSavings(
                By.XPath(
                    "//section[@id='ContentPlaceHolder1_orderResults']//section/div/article/div[2]/div/p[2]"), 1);
        }

        public decimal GetSubTotal()
        {
            decimal total = 0;
            try
            {
                if (TestingSession.Browser.IsElementPresent(By.Id("ContentPlaceHolder1_lblDiscountsText")))
                {
                    total = _commonFunctions.GetSavings(By.XPath("//dd[2]"), 1);
                }
                else
                {
                    total = _commonFunctions.GetSavings(By.CssSelector("dd"), 1);
                }
            }
            catch (Exception e)
            {
            }
            return total;
        }

        public decimal GetTotalOrderAmount()
        {
            return _commonFunctions.GetSavings(By.Id("orderTotalValue"), 1);
        }

        public decimal GetShippingPrice()
        {
            decimal shipping = 0;
            string selector;
            if
                (TestingSession.GetDriver<TextBox>(
                    By.XPath("//section[@id='ContentPlaceHolder1_orderResults']//section/div/article/div[2]/div/p[2]"))
                    .GetText()
                    .Contains("You saved"))
            {
                selector = "//div[@id='ContentPlaceHolder1_divTotals']/dl/dd[3]";
            }
            else
            {
                selector = "//div[@id='ContentPlaceHolder1_divTotals']/dl/dd[2]";
            }

            if (TestingSession.GetDriver<TextBox>(
                By.XPath(selector)).GetText().Contains("FREE"))
            {
                shipping = 0;
            }
            else
            {
                shipping = _commonFunctions.GetSavings(By.XPath(selector), 1);
            }
            return shipping;
        }

        public string GetProductSize()
        {
            return
                TestingSession.GetDriver<TextBox>(By.Id("pItemSizeName"))
                    .GetText();
        }

        public decimal GetTaxes()
        {
            return _commonFunctions.GetSavings(By.XPath("//div[@id='ContentPlaceHolder1_divTotals']/dl/dt[contains(text(),'Taxes')]/following::dd"), 1);
        }
    }
}