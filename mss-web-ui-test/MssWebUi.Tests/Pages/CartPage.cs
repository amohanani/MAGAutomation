using MAG.WebTesting.BasicElements;
using MAG.WebTesting.Browsers;
using MAG.WebTesting.Pages;
using MssWebUi.Tests.Utilities;
using OpenQA.Selenium;

namespace MssWebUi.Tests.Pages
{
    public class CartPage : BasePage
    {
        private readonly CommonMethods commonFunctions;

        public CartPage(IBrowserTestingSession testingSession) : base(testingSession, "cart")
        {
            commonFunctions = new CommonMethods(TestingSession);
        }

        public bool IsEmpty()
        {
            TestingSession.Browser.WaitFor(By.CssSelector(".footer"));

            return TestingSession
                .Browser
                .IsElementVisible(By.CssSelector("#emptyCartNotficationDiv"));
        }

        /* Methods to be verified in Cart Modal */

        public void NavigateToCartModal()
        {
            TestingSession.Browser.SwitchToActiveElement();
            TestingSession.Browser.WaitFor(By.CssSelector(".cartAddAnnounce"));
        }

        public string GetPriceInCartModal(decimal savings)
        {
            if
                (savings == 0)
            {
                return TestingSession.GetDriver<TextBox>(By.CssSelector(".itempricing div")).GetText();
            }
            return TestingSession.GetDriver<TextBox>(By.XPath("//li[@id='liOnSalePrice']/div/strong")).GetText();
        }

        public decimal GetSavingsInCartModal()
        {
            return commonFunctions.GetSavings(By.CssSelector("#liSavingsAmount > div"), 1); //dd.savings
        }

        public string GetSubTotalInCartModal()
        {
            return TestingSession.GetDriver<TextBox>(By.CssSelector("div.subtotalamount > strong")).GetText();
        }

        public string GetRewardPointsInCartModal()
        {
            return TestingSession.GetDriver<TextBox>(By.CssSelector("li.riderrewards > div")).GetText();
        }

        public void ClickViewCartPage()
        {
            TestingSession.GetDriver<Button>(By.XPath("(//a[contains(text(),'VIEW CART')])[2]")).Click();
            TestingSession.Browser.WaitForPageLoad();
        }

        /* Methods to be verified in View Cart Page */

        public void ClickViewCart()
        {
            TestingSession.GetDriver<Button>(By.XPath("(//a[contains(text(),'VIEW CART')])[2]")).Click();
            TestingSession.Browser.WaitForPageLoad();
            TestingSession.Browser.SwitchToDefaultContent();
        }

        public string GetPriceInCart()
        {
            //return TestingSession.GetDriver<TextBox>(By.CssSelector("dd.price")).GetText();
            ////div[@id='cartitems_ko']/article/div/div[4]/div/dl/dd - Mapping changed
            return
                TestingSession.GetDriver<TextBox>(By.XPath("//div[@id='cartitems']/article/div/div[4]/div/dl/dd"))
                    .GetText();
        }

        public decimal GetSavingsInCartPage()
        {
            return commonFunctions.GetSavings(By.CssSelector("dd.savings"), 1);
        }


        public decimal GetSavingsForOilChemicalInCartPage()
        {
            return commonFunctions.GetSavings(By.Id("savingsLabel"), 1);
        }

        public decimal GetTotalSavingsInCartPage()
        {
            return commonFunctions.GetSavings(By.XPath("//div[@id='totalsWrapper']/div[2]/div/div[2]/label"), 1);
            //id=savingsLabel
        }

        public string GetTotalRewardPointsInCartPage(string rewardPoints)
        {
            if (! rewardPoints.Equals("0"))
                return TestingSession.GetDriver<TextBox>(By.CssSelector("span.rewardPoints")).GetText();
            else
                return rewardPoints;
        }

        public string GetSubTotalPriceInCart()
        {
            return TestingSession.GetDriver<TextBox>(By.Id("subTotalLabel")).GetText();
        }

        public decimal GetShippingExpectedInCartPage() //decimal
        {
            decimal shipping = 0;
            if (!TestingSession.GetDriver<TextBox>(By.Id("shippingLabel")).GetText().Contains("FREE"))
            {
                shipping = commonFunctions.GetSavings(By.Id("shippingLabel"), 1);
            }
            return shipping;
        }

        public decimal GetGrandTotalInCartPage()
        {
            return commonFunctions.GetSavings(By.Id("grandTotalLabel"), 1);
        }

        public ShippingBillingInformationPage ClickBeginSecureCheckout()
        {
            TestingSession.GetDriver<Button>(By.XPath("(//a[contains(text(),'Begin Secure Checkout')])[2]")).Click();
            TestingSession.Browser.WaitForPageLoad();
            return new ShippingBillingInformationPage(TestingSession);
        }

        public void IncrementQuantity(int totalQuantity)
        {
            for (var i = 1; i <= totalQuantity; i++)
            {
                TestingSession.GetDriver<Button>(By.CssSelector("input.add.wait-spinner")).Click();
                TestingSession.Browser.WaitForPageLoad();
                TestingSession.Browser.ImplicitWait();
            }
        }

        public string GetCartMessage()
        {
            return TestingSession.GetDriver<TextBox>(By.CssSelector("h1")).GetText();
        }

        public void WaitForPriceToGetUpdated(decimal price)
        {
            TestingSession.Browser.WaitForTextToPresent(By.Id("subTotalLabel"), price.ToString(), 10);
        }

        public void WaitforBeginSecureButton()
        {
            TestingSession.Browser.WaitForElement(By.CssSelector("a.button.primary.big.checkoutbutton"), 10);
        }

        public void WaitForViewCartButton()
        {
            TestingSession.Browser.WaitForElement(By.XPath("(//a[contains(text(),'VIEW CART')])[2]"), 10);
        }

    }
}