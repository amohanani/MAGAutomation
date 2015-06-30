using System;
using MAG.WebTesting.BasicElements;
using MAG.WebTesting.Browsers;
using MAG.WebTesting.Pages;
using MssWebUi.Tests.Utilities;
using OpenQA.Selenium;

namespace MssWebUi.Tests.Pages
{
    public class ProductInformationPage : BasePage
    {
        private readonly CommonMethods _commonFunctions;

        public ProductInformationPage(IBrowserTestingSession testingSession, int styleId)
            : base(testingSession, styleId + "/i/ignored")
        {
            _commonFunctions = new CommonMethods(TestingSession);
        }

        public void SelectColor(string colorCode)
        {
            TestingSession.GetDriver<Button>(By.XPath("//*[@title='" + colorCode + "']")).Click();
        }

        public void SelectSize(string size)
        {
            TestingSession.GetDriver<Button>(By.XPath("//*[@value='" + size + "']")).Click();
        }

        public void IncrementQuantity()
        {
            TestingSession.GetDriver<Button>(By.Id("btnAdd")).Click();
        }

        public int GetQuantity()
        {
            var strVal = TestingSession.GetDriver<TextBox>(By.Id("quantity")).GetValue();
            return int.Parse(strVal);
        }

        public void TypeQuantityInTextBox(int quantity)
        {
            TestingSession.GetDriver<TextBox>(By.Id("quantity")).EnterText(quantity.ToString());
        }

        public string GetPrice()
        {
            return TestingSession.GetDriver<TextBox>(By.CssSelector("span.price > span")).GetText();
        }

        public string GetRewardPoints()
        {
            var fetchedRewards =
                TestingSession.GetDriver<TextBox>(By.CssSelector("div.rewardsText > p > span")).GetText();
            if (String.IsNullOrEmpty(fetchedRewards))
            {
                fetchedRewards = "0";
            }
            return fetchedRewards;
        }

        public CartPage ClickAddToCart()
        {
            TestingSession.GetDriver<Button>(By.Id("btnSubmit")).Click();
            TestingSession.Browser.WaitForPageLoad();
            TestingSession.Browser.ImplicitWait();
            TestingSession.Browser.SwitchToActiveElement();
            TestingSession.Browser.SwitchToFrameBy(By.ClassName("jackbox-content"));
            return new CartPage(TestingSession);
        }

        public decimal GetSavings(int index)
        {
            decimal savings = 0;
            if (! TestingSession.Browser.IsElementNotPresent(By.CssSelector("span.savings"))) {
                savings=_commonFunctions.GetSavings(By.CssSelector("span.savings"), index);
            }
            return savings;
        }

        public bool SelectStyleYears(string year)
        {
            if (TestingSession.Browser.IsElementNotPresent(By.Id("itemStyleSelectorYears")))
            {
                TestingSession.GetDriver<SelectBox>(By.Id("itemStyleSelectorYears")).SelectByDisplay(year);
                return true;
            }
            else return false;
        }

        public void SelectStyleSelector(string style)
        {
            if (TestingSession.Browser.IsElementPresent(By.Id("itemStyleSelectorMakes")))
            {
                TestingSession.GetDriver<SelectBox>(By.Id("itemStyleSelectorMakes")).SelectByDisplay(style);
            }
        }

        public void SelectStyleSelectorModel(string model)
        {
            if (TestingSession.Browser.IsElementPresent(By.Id("itemStyleSelectorModels")))
            {
                TestingSession.GetDriver<SelectBox>(By.Id("itemStyleSelectorModels")).SelectByDisplay(model);
            }
        }

        public void SelectOverSizeItemSize(string size)
        {
            TestingSession.GetDriver<Button>(By.XPath("//*[@value='" + size + "']")).Click();
        }

        public void SelectOverSizeItemColor(string color)
        {
            TestingSession.GetDriver<Button>(By.XPath("//*[@title='" + color + "']")).Click();
        }

        public void SelectOilChemicalSize(string size)
        {
            TestingSession.GetDriver<Button>(By.XPath("//*[@value='" + size + "']")).Click();
        }
    }
}