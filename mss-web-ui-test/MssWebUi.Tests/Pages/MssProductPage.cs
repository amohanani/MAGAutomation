using MAG.WebTesting.BasicElements;
using MAG.WebTesting.Browsers;
using MAG.WebTesting.Pages;
using OpenQA.Selenium;

namespace MssWebUiTest.Pages
{
    public class MssProductPage : BasePage
    {
        public MssProductPage(IBrowserTestingSession testingSession, string sku) : base(testingSession, sku)
        {
        }

        public void SetQty(int qty)
        {
            TestingSession.Browser.WaitFor(By.Name("quantity"));
            TestingSession.Browser.FindElement(By.Name("quantity")).SendKeys(qty.ToString());
        }

        public void AddToCart()
        {
            TestingSession.GetDriver<Button>(By.Id("addToCartButton")).Click();
        }
    }
}