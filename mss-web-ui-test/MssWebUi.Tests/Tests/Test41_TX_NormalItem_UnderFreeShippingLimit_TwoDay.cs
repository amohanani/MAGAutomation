using System.Diagnostics;
using MAG.WebTesting;
using MssWebUi.Tests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace MssWebUiTest.Tests
{
    public class Test41_TX_NormalItem_UnderFreeShippingLimit_TwoDay : BaseBrowserFixture
    {
        private const string ProductSize = "Medium", LastName = "Normal Under Two Day";
        private readonly char[] _splitChar = {'$'};
        private string _estimatedArrivalDate;
        private string _productPrice, _rewardPoints, _shippingInfo, _billingInfo;
        private decimal _productSavings, _shippingPrice, _productGrandTotalPrice, _taxes;
        private int _quantity;

        [Test]
        public void TestMyUseCaseFortyOne()
        {
            var productInfoPage = Session.Browser.NavigateTo<ProductInformationPage>(new {styleId = 61359}); //43644
            productInfoPage.SelectColor("Orange"); //Charcoal
            productInfoPage.SelectSize(ProductSize);
            _quantity = productInfoPage.GetQuantity();
            if (_quantity == 0)
            {
                productInfoPage.IncrementQuantity();
            }
            _productPrice = productInfoPage.GetPrice();
            _productSavings = productInfoPage.GetSavings(3);
            _rewardPoints = productInfoPage.GetRewardPoints();

            Debug.WriteLine("\nInformation on Product Page: Product Price is:" + _productPrice);
            Debug.WriteLine("Product savings is:" + _productSavings);
            Debug.WriteLine("Product reward Points is:" + _rewardPoints);

            /* Verifications on cart model */
            var cartPage = productInfoPage.ClickAddToCart();
            cartPage.NavigateToCartModal();

            Assert.True(cartPage.GetPriceInCartModal(_productSavings).Contains(_productPrice),
                "Product price on product information page and cart modal is not matching.");
            if (_productSavings != 0)
            Assert.AreEqual(cartPage.GetSavingsInCartModal(), _productSavings,
                "Product savings on product information page and cart modal  is not matching..");
            Assert.True(cartPage.GetRewardPointsInCartModal().Contains(_rewardPoints), "Reward points are not present.");

            /* Verifications on Cart Page */
            cartPage.ClickViewCart();
            cartPage.WaitforBeginSecureButton();
            Assert.AreEqual(_productPrice, cartPage.GetPriceInCart(),
                "Product price on product information page and cart page is not matching.");
            if (_productSavings != 0)
            {
                Assert.AreEqual(_productSavings, cartPage.GetSavingsInCartPage(),
                    "Product savings on product information page and cart page is not matching.");
                Assert.AreEqual(_productSavings, cartPage.GetTotalSavingsInCartPage(),
                    "Product total savings on product information page and cart page is not matching.");
            }
            Assert.AreEqual(_rewardPoints, cartPage.GetTotalRewardPointsInCartPage(_rewardPoints), "Reward points are not matching.");

            Assert.AreEqual(_productPrice, cartPage.GetSubTotalPriceInCart(),
                "Product sub total on product information page and cart page is not matching.");
            _shippingPrice = cartPage.GetShippingExpectedInCartPage();
            var productSubTotalPrice = SplitAndConvert(_productPrice, 1);
            _productGrandTotalPrice = cartPage.GetGrandTotalInCartPage();

            Debug.WriteLine("Shipping Price:" + _shippingPrice + " SubTotal:" + productSubTotalPrice + " GrandTotal: " +
                            _productGrandTotalPrice);
            Assert.AreEqual((_shippingPrice + productSubTotalPrice), _productGrandTotalPrice,
                "Grand total is not correct.");

            var shippingBillingPage = cartPage.ClickBeginSecureCheckout();

            /* Verifications on ShippingBillingInformationPage */
            Assert.AreEqual("1", shippingBillingPage.GetQuantity(), "Order quantity on ShippingBilling page is wrong.");
            Assert.AreEqual(productSubTotalPrice, shippingBillingPage.GetPrice(),
                "Order sub total on ShippingBilling page is wrong.");
            _shippingPrice = shippingBillingPage.GetShippingExpected(); 
            Debug.WriteLine("Shipping Price:" + _shippingPrice + " SubTotal:" + shippingBillingPage.GetPrice() +
                            " GrandTotal: " +
                            shippingBillingPage.GetGrandTotal());
            Assert.AreEqual((_shippingPrice + shippingBillingPage.GetPrice()), shippingBillingPage.GetGrandTotal(),
                "Order Grand total on ShippingBilling page is wrong.");
            if (_productSavings != 0)
            Assert.AreEqual(_productSavings, shippingBillingPage.GetSavings(),
                "Order Savings on ShippingBilling page is wrong.");
            Assert.AreEqual(_rewardPoints, shippingBillingPage.GetTotalRewardPoints(_rewardPoints),
                "Reward points on ShippingBilling page is wrong.");

            /* Shipping Information */
            shippingBillingPage.TypeFirstName(Configuration.GetTXFirstName);
            shippingBillingPage.TypeLastName(LastName);
            shippingBillingPage.TypeAddressLine1(Configuration.GetTXAddressLine1);
            shippingBillingPage.TypeCity(Configuration.GetTXCity);
            shippingBillingPage.SelectState(Configuration.GetTXState);
            shippingBillingPage.TypePostalCode(Configuration.GetTXPostalCode);
            shippingBillingPage.TypePhone(Configuration.GetPhone);
            shippingBillingPage.TypeEmail(Configuration.GetEmail);

            /* Provide Billing details */
            shippingBillingPage.TypeCardNumber(Configuration.GetCardNumber);
            shippingBillingPage.SelectCardExpirationMonth(Configuration.GetExpiryMonth);
            shippingBillingPage.SelectCardExpirationYear(Configuration.GetExpiryYear);
            shippingBillingPage.TypeCVV(Configuration.GetCVV);

            shippingBillingPage.WaitForElement(By.Id("shipping-option-Ground"), 60);
            shippingBillingPage.ClickTwoDayShipping();
            Assert.True(shippingBillingPage.IsGroundShippingPresent(), "Ground Shipping option is not present.");
            Assert.True(shippingBillingPage.IsOvernightShippingPresent(), "Overnight Shipping option is not present.");
            Assert.True(shippingBillingPage.IsTwoDayShippingPresent(), "Two Day Shipping option is not present.");
            Assert.False(shippingBillingPage.IsOvernightFreeShippingNotPresent(), "Two Day Free Shipping option is present.");//As taxes are taking time.

            Assert.AreEqual("1", shippingBillingPage.GetQuantity(),
                "Order quantity on ShippingBilling page is wrong after providing postal code.");
            Assert.AreEqual(productSubTotalPrice, shippingBillingPage.GetPrice());
            if (_productSavings != 0)
                Assert.AreEqual(_productSavings, shippingBillingPage.GetSavings(),
                    "Order Savings on ShippingBilling page is wrong after proving postal code.");
            Assert.AreEqual(_rewardPoints, shippingBillingPage.GetTotalRewardPoints(_rewardPoints),
                "Reward points on ShippingBilling page is wrong after proving postal code.");
            Assert.True(shippingBillingPage.IsShipToSameBillingAddressChecked(),
                "Same as Billing address is not checked.");
            Assert.True(shippingBillingPage.IsShipToDifferentAddressPresent(),
                "Ship to different address is not present.");

            _shippingPrice = shippingBillingPage.GetShippingExpected();
            _taxes = shippingBillingPage.GetTaxes();
            Assert.True(_taxes != 0, "Taxes are not present");
            Debug.WriteLine("Shipping Price:" + _shippingPrice + " SubTotal:" + shippingBillingPage.GetPrice() +
                            " GrandTotal: " + shippingBillingPage.GetGrandTotal() + " Taxes: " + _taxes);
           
            Assert.AreEqual((_shippingPrice + shippingBillingPage.GetPrice() + _taxes), shippingBillingPage.GetGrandTotal(),
                "Order sub total on ShippingBilling page is wrong after proving postal code.");
            _estimatedArrivalDate = shippingBillingPage.EstimatedArrivalDate();
            _shippingPrice = shippingBillingPage.GetTwoDayShippingCharges();
            var orderSummaryPage = shippingBillingPage.ClickPlaceOrder();

            /* Order Summary Page validations */
            _shippingInfo = orderSummaryPage.GetShippingInformation();
            Assert.True(_shippingInfo.ToLower().Contains(Configuration.GetTXFirstName.ToLower()),
                "Shipping To - First name is not present on Order Summary page.");
            Assert.True(_shippingInfo.ToLower().Contains(LastName.ToLower()),
                "Shipping To - Last name is not present on Order Summary page.");
            Assert.True(_shippingInfo.ToLower().Contains(Configuration.GetTXAddressLine1.ToLower()),
                "Shipping To - Address line1 is not present on Order Summary page.");
            Assert.True(_shippingInfo.Contains(Configuration.GetTXCity),
                "Shipping To - City is not present on Order Summary page.");
            Assert.True(_shippingInfo.Contains(Configuration.GetTXStateCode),
                "Shipping To - State code is not present on Order Summary page.");
            Assert.True(_shippingInfo.Contains(Configuration.GetTXPostalCode),
                "Shipping To - Postal code is not present on Order Summary page.");
            Assert.True(orderSummaryPage.GetShippingMethod().Contains("Two"),
                "Shipping method is not present on Order Summary page.");

            _billingInfo = orderSummaryPage.GetBillingInformation();
            Assert.True(_billingInfo.ToLower().Contains(Configuration.GetTXFirstName.ToLower()),
                "Billing To - First name is not present on Order Summary page.");
            Assert.True(_billingInfo.ToLower().Contains(LastName.ToLower()),
                "Billing To - Last name is not present on Order Summary page.");
            Assert.True(_billingInfo.ToLower().Contains(Configuration.GetTXAddressLine1.ToLower()),
                "Billing To - Address line1 is not present on Order Summary page.");
            Assert.True(_billingInfo.Contains(Configuration.GetTXCity),
                "Billing To - City is not present on Order Summary page.");
            Assert.True(_billingInfo.Contains(Configuration.GetTXStateCode),
                "Billing To - State code is not present on Order Summary page.");
            Assert.True(_billingInfo.Contains(Configuration.GetTXPostalCode),
                "Billing To - Postal code is not present on Order Summary page.");

            Assert.True(orderSummaryPage.GetPaymentDetails().Contains("Visa"),
                "Payment method is not present on Order Summary page.");
            Assert.AreEqual(productSubTotalPrice, orderSummaryPage.GetProductPrice(),
                "Product price is wrong on Order Summary page.");
            Assert.AreEqual(productSubTotalPrice, orderSummaryPage.GetSubTotal(),
                "Product sub total is wrong on Order Summary page.");
            Assert.AreEqual(_shippingPrice, orderSummaryPage.GetShippingPrice(),
                "Product shipping price is wrong on Order Summary page.");
            Assert.AreEqual((_shippingPrice + productSubTotalPrice + _taxes), orderSummaryPage.GetTotalOrderAmount(),
                "Product grand total is wrong on Order Summary page.");

            Debug.WriteLine("Order Number is:" + orderSummaryPage.GetOrderNumber());
            Assert.AreEqual(_estimatedArrivalDate, orderSummaryPage.GetExpectedArrivalDate(),
                "Estimated arrival date is not matching.");
            Assert.True(orderSummaryPage.GetPaymentDetails().Contains("**** **** **** 1111"));
        }

        private decimal SplitAndConvert(string strToSplit, int index)
        {
            var strArr = strToSplit.Split(_splitChar);
            return decimal.Parse(strArr[index]);
        }
    }
}