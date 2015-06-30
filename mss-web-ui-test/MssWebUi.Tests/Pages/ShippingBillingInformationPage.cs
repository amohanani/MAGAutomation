using System.Diagnostics;
using MAG.WebTesting.BasicElements;
using MAG.WebTesting.Browsers;
using MAG.WebTesting.Pages;
using MssWebUi.Tests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MssWebUi.Tests.Pages
{
    public class ShippingBillingInformationPage : BasePage
    {
        private readonly CommonMethods _commonFunctions;

        public ShippingBillingInformationPage(IBrowserTestingSession testingSession) : base(testingSession, "")
        {
            _commonFunctions = new CommonMethods(TestingSession);
        }

        public string GetQuantity()
        {
            return TestingSession.GetDriver<TextBox>(By.Name("quantity")).GetValue();
        }

        public decimal GetPrice()
        {
            return _commonFunctions.GetSavings(By.CssSelector("dd"), 1);
        }

        public decimal GetSavings()
        {
            return _commonFunctions.GetSavings(By.CssSelector("dd.savings"), 1);
        }

        public string GetTotalRewardPoints(string rewardPoints)
        {
            if (!rewardPoints.Equals("0"))
                return TestingSession.GetDriver<TextBox>(By.CssSelector("span.rewardPoints")).GetText();
            else
                return rewardPoints;
        }

        public decimal GetShippingExpected()
        {
            decimal shipping = 0;
            if (
                !TestingSession.GetDriver<TextBox>(By.XPath("//div[@id='OrderSummary-Totals']/dl/dd[2]"))
                    .GetText()
                    .Contains("FREE"))
            {
                //shipping =
                //    decimal.Parse(
                //        TestingSession.GetDriver<TextBox>(By.XPath("//div[@id='OrderSummary-Totals']/dl/dd[2]"))
                //            .GetText());
                shipping = _commonFunctions.GetSavings(By.XPath("//div[@id='OrderSummary-Totals']/dl/dd[2]"), 1);
            }
            return shipping;
        }

        public decimal GetShippingFromShippingOptions(int shippingOption)
        {
            return
                _commonFunctions.GetSavings(By.XPath("//ul[@id='shipping-methods-list']/li[" + shippingOption + "]/label/span[2]"),1);
        }

        public void WaitForShippingPriceToGetUpdated(int shippingOption)
        {
            TestingSession.Browser.WaitForTextToPresent(By.XPath("//div[@id='OrderSummary-Totals']/dl/dd[2]"),
                TestingSession.Browser.FindElement(By.XPath("//ul[@id='shipping-methods-list']/li[" + shippingOption + "]/label/span[2]")).Text, 20);
        }

        public decimal GetGrandTotal()
        {
            TestingSession.Browser.ImplicitWait();
            return _commonFunctions.GetSavings(By.CssSelector("dd.ordertotal"), 1);
        }

        public bool IsShipToSameBillingAddressChecked()
        {
            return
                TestingSession.Browser.IsElementPresent(
                    By.XPath("//*[@id='shipToSameAddressAsBilling'][@checked='checked']"));
        }

        public bool IsShipToDifferentAddressPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipToDifferentAddress"));
        }

        public void TypeFirstName(string name)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Shipping_FirstName")).EnterText(name);
        }

        public void TypeLastName(string name)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Shipping_LastName")).EnterText(name);
        }

        public void TypeAddressLine1(string address)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Shipping_AddressLine1")).EnterText(address);
        }

        public void TypeCity(string city)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Shipping_City")).EnterText(city);
        }

        public void TypePostalCode(string code)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Shipping_PostalCode")).EnterText(code);
        }

        public void TypePhone(string phone)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Phone")).EnterText(phone);
        }

        public void TypeEmail(string email)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Email")).EnterText(email);
        }

        public void SelectCountry(string country)
        {
            TestingSession.GetDriver<SelectBox>(By.Id("Shipping_CountryId")).SelectByDisplay(country);
            TestingSession.Browser.ImplicitWait();
        }

        public void SelectState(string state)
        {
            TestingSession.GetDriver<SelectBox>(By.Id("Shipping_TerritoryId")).SelectByDisplay(state);
        }

        public string EstimatedArrivalDate()
        {
            return TestingSession.GetDriver<TextBox>(By.Id("estDate")).GetText();
        }

        public void TypeCardNumber(string card)
        {
            TestingSession.GetDriver<TextBox>(By.Id("PaymentInformation_Number")).EnterText(card);
        }

        public void SelectCardExpirationMonth(string month)
        {
            TestingSession.GetDriver<SelectBox>(By.Id("PaymentInformation_ExpirationMonth")).SelectByDisplay(month);
        }

        public void SelectCardExpirationYear(string year)
        {
            TestingSession.GetDriver<SelectBox>(By.Id("PaymentInformation_ExpirationYear")).SelectByDisplay(year);
        }

        public void TypeCVV(string cvv)
        {
            TestingSession.GetDriver<TextBox>(By.Id("PaymentInformation_CVV")).EnterText(cvv);
        }

        public bool IsGroundShippingPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Ground"));
        }

        public bool IsGroundShippingNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-Ground"));
        }

        public void ClickGroundShipping()
        {
            TestingSession.GetDriver<Button>(By.Id("shipping-option-Free Ground")).Click();
        }

        public void ClickOvernightShipping()
        {
            TestingSession.GetDriver<Button>(By.XPath("//ul[@id='shipping-methods-list']/li[3]/input")).Click();
        }

        public decimal GetOvernightShippingCharges()
        {
             return _commonFunctions.GetSavings(By.XPath("//ul[@id='shipping-methods-list']/li[3]/label/span[2]"), 1);
        }

        public bool IsGroundFreeShippingPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Free Ground"));
        }

        public bool IsTwoDayShippingPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Two Day"));
        }

        public void ClickTwoDayShipping()
        {
            TestingSession.GetDriver<Button>(By.XPath("//ul[@id='shipping-methods-list']/li[2]/input")).Click();
        }

        public decimal GetTwoDayShippingCharges()
        {
            return _commonFunctions.GetSavings(By.XPath("//ul[@id='shipping-methods-list']/li[2]/label/span[2]"), 1);
        }

        public bool IsTwoDayFreeShippingPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Two Day Upgrade"));
        }

        public bool IsTwoDayFreeShippingNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-Two Day Upgrade"));
        }

        public void ClickTwoDayFreeShipping()
        {
            TestingSession.GetDriver<Button>(By.XPath("//ul[@id='shipping-methods-list']/li[2]/input")).Click();
        }

        public bool IsOvernightShippingPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Overnight Saver"));
        }

        public bool IsOvernightShippingNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-Overnight Saver"));
        }

        public bool IsOvernightFreeShippingPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Overnight Saver Upgrade"));
        }

        public bool IsOvernightFreeShippingNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-Overnight Saver Upgrade"));
        }
        
        public void ClickOvernightFreeShipping()
        {
            TestingSession.GetDriver<Button>(By.XPath("//ul[@id='shipping-methods-list']/li[3]/input")).Click();
        }

        public OrderSummaryPage ClickPlaceOrder()
        {
            TestingSession.GetDriver<Button>(By.Id("btnPlaceOrder")).Click();
            TestingSession.Browser.WaitForPageLoad();
            TestingSession.Browser.WaitForElement(By.XPath("//input[@value='Submit Answers']"), 60000);
            return new OrderSummaryPage(TestingSession);
        }

        public void WaitForElement(By selector, int timeout)
        {
            TestingSession.Browser.WaitForElement(selector, timeout);
        }

        public decimal GetTaxes()
        {
           return _commonFunctions.GetTax(By.XPath("//div[@id='OrderSummary-Totals']/dl/dd[3]"), 1);
        }

        public bool IsAPOShippingPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-APO"));
        }

        public bool IsAPOShippingNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-APO"));
        }

        public void ClickAPOShipping()
        {
            TestingSession.GetDriver<Button>(By.Id("shipping-option-APO")).Click();
        }

        public CartPage ClickReduceQuantityToZero(int quantity)
        {
            for (var i = 1; i <= quantity; i++)
            {
                TestingSession.GetDriver<Button>(By.CssSelector("input.subtract.wait-spinner")).Click();
            }
            return new CartPage(TestingSession);
        }

        public bool IsAKGroundShippingPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-AK Ground"));
        }

        public bool IsAKGroundShippingNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-AK Ground"));
        }

        public bool IsPuertoRicoShippingPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Puerto Rico Int."));
        }

        public bool IsPuertoRicoShippingNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-Puerto Rico Int."));
        }

        public bool IsInvalidShippingOptionsPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Invalid Shipping Options"));
        }

        public void ClickInvalidShippingOptions()
        {
            TestingSession.GetDriver<Button>(By.XPath("//ul[@id='shipping-methods-list']/li[2]/input")).Click();
        }

        public bool IsHIGroundShippingPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Hawaii Ground"));
        }

        public bool IsHIGroundShippingNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-Hawaii Ground"));
        }

        public bool IsShipToPreferredInstallerPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("cbShipToPreferredInstaller"));
        }

        public bool IsUKExpeditedPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-United Kingdom Expedited"));
        }

        public bool IsUKExpeditedNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-United Kingdom Expedited"));
        }

        public bool IsAUExpeditedPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Australia Expedited"));
        }

        public bool IsAUExpeditedNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-Australia Expedited"));
        }

        public bool IsCAExtendedPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Canadian Int Standard"));
        }

        public bool IsCAExtendedNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-Canadian Int Standard"));
        }

        public void ClickAKGroundShipping()
        {
            TestingSession.GetDriver<Button>(By.XPath("//ul[@id='shipping-methods-list']/li/input")).Click();
        }

        public bool IsCAInternationalShippingPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("shipping-option-Canadian Int Standard"));
        }

        public bool IsCAInternationalShippingNotPresent()
        {
            return TestingSession.Browser.IsElementNotPresent(By.Id("shipping-option-Canadian Int Standard"));
        }

        public void ClickCAInternationalShipping()
        {
            TestingSession.GetDriver<Button>(By.XPath("//ul[@id='shipping-methods-list']/li/input")).Click();
        }

        public bool IsRestrictionsPresent()
        {
            return TestingSession.Browser.IsElementPresent(By.Id("restrictionClose"));
        }

        public void ClickDifferentBillingAddress()
        {
            TestingSession.GetDriver<Button>(By.Id("shipToDifferentAddress")).Click();
        }

        public void TypeBillingFirstName(string name)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Billing_FirstName")).EnterText(name);
        }

        public void TypeBillingAddressLine1(string address)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Billing_AddressLine1")).EnterText(address);
        }

        public void TypeBillingCity(string city)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Billing_City")).EnterText(city);
        }

        public void TypeBillingPostalCode(string code)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Billing_PostalCode")).EnterText(code);
        }

        public void SelectBillingState(string state)
        {
            TestingSession.GetDriver<SelectBox>(By.Id("Billing_TerritoryId")).SelectByDisplay(state);
        }

        public void TypeBillingLastName(string name)
        {
            TestingSession.GetDriver<TextBox>(By.Id("Billing_LastName")).EnterText(name);
        }

        public void SelectBillingCountry(string country)
        {
            TestingSession.GetDriver<SelectBox>(By.Id("Billing_CountryId")).SelectByDisplay(country);
        }

        public string GetCustomsNoticeMessage()
        {
            return TestingSession.GetDriver<TextBox>(By.Id("customsNotice")).GetText();
        }

        public void WaitForStateDropDownToGetLoaded(By selector){
             TestingSession.Browser.WaitForDropDownValues(selector);
        }

        public void WaitForRestrictionMessage()
        {
            TestingSession.Browser.WaitForElement(By.Id("restrictionClose"), 30);
        }

        public void WaitForTaxToPresent()
        {
            TestingSession.Browser.WaitForTextToPresent(By.XPath("//div[@id='OrderSummary-Totals']/dl/dd[3]"), "$", 30);
        }
    }
}