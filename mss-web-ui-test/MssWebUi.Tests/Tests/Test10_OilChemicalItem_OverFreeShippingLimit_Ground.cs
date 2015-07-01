using System;
using System.Diagnostics;
using MAG.WebTesting;
using MssWebUi.Tests.Pages;
using NUnit.Framework;

namespace MssWebUiTest.Tests
{
    public class Test10_OilChemicalItem_OverFreeShippingLimit_Ground : BaseBrowserFixture
    {
        private const string LastName = "Oil Over Ground";
        private const int TotalQuantity = 1;
        private readonly char[] _splitChar = {'$'};
        private string _estimatedArrivalDate;
        private string _productPrice, _rewardPoints, _shippingInfo, _billingInfo;
        private decimal _productSavings, _shippingPrice, _productGrandTotalPrice;
        private int _quantity;

        [Test]
        public void TestMyUseCaseTen()
        {
            var productInfoPage = Session.Browser.NavigateTo<ProductInformationPage>(new {styleId = 7388});
            productInfoPage.SelectOilChemicalSize("5W - 16 Ounce");
            _quantity = productInfoPage.GetQuantity();
            if (_quantity == 0)
            {
                productInfoPage.IncrementQuantity();
            }
            _productPrice = productInfoPage.GetPrice();
            _productSavings = productInfoPage.GetSavings(5);
            _rewardPoints = productInfoPage.GetRewardPoints();

            Debug.WriteLine("\nInformation on Product Page: Product Price is:" + _productPrice);
            Debug.WriteLine("Product savings is:" + _productSavings);
            Debug.WriteLine("Product reward Points is:" + _rewardPoints);

            /* Verifications on cart model */
            var cartPage = productInfoPage.ClickAddToCart();
            cartPage.NavigateToCartModal();

            Assert.True(cartPage.GetPriceInCartModal(_productSavings).Contains(_productPrice),
                "Product price on product information page and cart modal is not matching."); 
            Debug.WriteLine("Savings on cart model is:" + cartPage.GetSavingsInCartModal());
            Assert.True(cartPage.GetRewardPointsInCartModal().Contains(_rewardPoints), "Reward points are not present.");

            /* Took savings from cart model page as it is not matching with the product information page */
            _productSavings = cartPage.GetSavingsInCartModal();

            /* Verifications on Cart Page by increasing quantity */
            cartPage.ClickViewCart();
            cartPage.WaitforBeginSecureButton();

            Assert.AreEqual(_productPrice, cartPage.GetPriceInCart(),
                "Product price on product information page and cart page is not matching.");
            Debug.WriteLine("Savings on cart model page is:" + cartPage.GetSavingsInCartModal());
            if (_productSavings != 0) 
                Assert.AreEqual((TotalQuantity * _productSavings), cartPage.GetTotalSavingsInCartPage(),
                "Product total savings on product information page and cart page is not matching.");
            Assert.AreEqual((SplitAndConvert(_productPrice, 1)*TotalQuantity),
                SplitAndConvert(cartPage.GetSubTotalPriceInCart(), 1),
                "Product sub total on product information page and cart page is not matching.");
            Assert.AreEqual((TotalQuantity*Int32.Parse(_rewardPoints)),
                Int32.Parse(cartPage.GetTotalRewardPointsInCartPage(_rewardPoints)),
                "Reward points are not matching.");

            _shippingPrice = cartPage.GetShippingExpectedInCartPage();
            var productSubTotalPrice = SplitAndConvert(_productPrice, 1);
            _productGrandTotalPrice = cartPage.GetGrandTotalInCartPage();

            Debug.WriteLine("Shipping Price:" + _shippingPrice + " SubTotal:" + productSubTotalPrice + " GrandTotal: "
                            + _productGrandTotalPrice);
            Assert.AreEqual((_shippingPrice + (TotalQuantity*productSubTotalPrice)), _productGrandTotalPrice,
                "Grand total is not correct.");

            /* Verifications on ShippingBillingInformationPage */
            var shippingBillingPage = cartPage.ClickBeginSecureCheckout();
            Assert.AreEqual((TotalQuantity*Int32.Parse(_rewardPoints)),
                Int32.Parse(shippingBillingPage.GetTotalRewardPoints(_rewardPoints)),
                "Reward points on ShippingBilling page is wrong.");

            Assert.AreEqual(TotalQuantity.ToString(), shippingBillingPage.GetQuantity(),
                "Order quantity on ShippingBilling page is wrong.");
            Assert.AreEqual(TotalQuantity*productSubTotalPrice, shippingBillingPage.GetPrice(),
                "Order sub total on ShippingBilling page is wrong.");
            _shippingPrice = shippingBillingPage.GetShippingExpected();
            Debug.WriteLine("Shipping Price:" + _shippingPrice + " SubTotal:" + shippingBillingPage.GetPrice() +
                            " GrandTotal: " + shippingBillingPage.GetGrandTotal());
            Assert.AreEqual((_shippingPrice + shippingBillingPage.GetPrice()), shippingBillingPage.GetGrandTotal(),
                "Order Grand total on ShippingBilling page is wrong.");
            if (_productSavings != 0) 
                Assert.AreEqual((TotalQuantity * _productSavings), shippingBillingPage.GetSavings(),
                "Order Savings on ShippingBilling page is wrong.");
            Assert.AreEqual((TotalQuantity*Int32.Parse(_rewardPoints)),
                Int32.Parse(shippingBillingPage.GetTotalRewardPoints(_rewardPoints)),
                "Reward points on ShippingBilling page is wrong.");

            /* Shipping Information */
            shippingBillingPage.TypeFirstName(Configuration.GetFirstName);
            shippingBillingPage.TypeLastName(LastName);
            shippingBillingPage.TypeAddressLine1(Configuration.GetAddressLine1);
            shippingBillingPage.TypeCity(Configuration.GetCity);
            shippingBillingPage.SelectState(Configuration.GetState);
            shippingBillingPage.TypePostalCode(Configuration.GetPostalCode);
            shippingBillingPage.TypePhone(Configuration.GetPhone);
            shippingBillingPage.TypeEmail(Configuration.GetEmail);

            Assert.AreEqual(TotalQuantity, Int32.Parse(shippingBillingPage.GetQuantity()),
                "Order quantity on ShippingBilling page is wrong after providing postal code.");
            Assert.AreEqual(TotalQuantity*productSubTotalPrice, shippingBillingPage.GetPrice());
            _shippingPrice = shippingBillingPage.GetShippingExpected();
            Debug.WriteLine("Shipping Price:" + _shippingPrice + " SubTotal:" + shippingBillingPage.GetPrice() +
                            " GrandTotal: " + shippingBillingPage.GetGrandTotal());
            Assert.AreEqual((_shippingPrice + shippingBillingPage.GetPrice()), shippingBillingPage.GetGrandTotal(),
                "Order sub total on ShippingBilling page is wrong after proving postal code.");
            if (_productSavings != 0) 
                Assert.AreEqual((TotalQuantity * _productSavings), shippingBillingPage.GetSavings(),
                "Order Savings on ShippingBilling page is wrong after proving postal code.");
            Assert.AreEqual((TotalQuantity*Int32.Parse(_rewardPoints)),
                Int32.Parse(shippingBillingPage.GetTotalRewardPoints(_rewardPoints)),
                "Reward points on ShippingBilling page is wrong after proving postal code.");

            Assert.True(shippingBillingPage.IsShipToSameBillingAddressChecked(),
                "Same as Billing address is not checked.");
            Assert.True(shippingBillingPage.IsShipToDifferentAddressPresent(),
                "Ship to different address is not present.");

            /* Provide Billing details */

            shippingBillingPage.TypeCardNumber(Configuration.GetCardNumber);
            shippingBillingPage.SelectCardExpirationMonth(Configuration.GetExpiryMonth);
            shippingBillingPage.SelectCardExpirationYear(Configuration.GetExpiryYear);
            shippingBillingPage.TypeCVV(Configuration.GetCVV);
            Assert.True(shippingBillingPage.IsGroundShippingPresent(), "Ground Shipping option is not present.");
            Assert.False(shippingBillingPage.IsOvernightShippingNotPresent(),
                "Overnight Shipping option is present.");
            _estimatedArrivalDate = shippingBillingPage.EstimatedArrivalDate();
            var orderSummaryPage = shippingBillingPage.ClickPlaceOrder();

            /* Order Summary Page validations */
            _shippingInfo = orderSummaryPage.GetShippingInformation();
            Assert.True(_shippingInfo.ToLower().Contains(Configuration.GetFirstName.ToLower()),
                "Shipping To - First name is not present on Order Summary page.");
            Assert.True(_shippingInfo.ToLower().Contains(LastName.ToLower()),
                "Shipping To - Last name is not present on Order Summary page.");
            Assert.True(_shippingInfo.Contains(Configuration.GetAddressLine1),
                "Shipping To - Address line1 is not present on Order Summary page.");
            Assert.True(_shippingInfo.Contains(Configuration.GetCity),
                "Shipping To - City is not present on Order Summary page.");
            Assert.True(_shippingInfo.Contains(Configuration.GetStateCode),
                "Shipping To - State code is not present on Order Summary page.");
            Assert.True(_shippingInfo.Contains(Configuration.GetPostalCode),
                "Shipping To - Postal code is not present on Order Summary page.");
            Assert.True(orderSummaryPage.GetShippingMethod().Contains("Ground"),
                "Shipping method is not present on Order Summary page.");

            _billingInfo = orderSummaryPage.GetBillingInformation();
            Assert.True(_billingInfo.ToLower().Contains(Configuration.GetFirstName.ToLower()),
                "Billing To - First name is not present on Order Summary page.");
            Assert.True(_billingInfo.ToLower().Contains(LastName.ToLower()),
                "Billing To - Last name is not present on Order Summary page.");
            Assert.True(_billingInfo.Contains(Configuration.GetAddressLine1),
                "Billing To - Address line1 is not present on Order Summary page.");
            Assert.True(_billingInfo.Contains(Configuration.GetCity),
                "Billing To - City is not present on Order Summary page.");
            Assert.True(_billingInfo.Contains(Configuration.GetStateCode),
                "Billing To - State code is not present on Order Summary page.");
            Assert.True(_billingInfo.Contains(Configuration.GetPostalCode),
                "Billing To - Postal code is not present on Order Summary page.");

            Assert.True(orderSummaryPage.GetPaymentDetails().Contains("Visa"),
                "Payment method is not present on Order Summary page.");
            Assert.AreEqual(productSubTotalPrice, orderSummaryPage.GetProductPrice(),
                "Product price is wrong on Order Summary page.");
            Assert.AreEqual(TotalQuantity*productSubTotalPrice, orderSummaryPage.GetSubTotal(),
                "Product sub total is wrong on Order Summary page.");
            Assert.AreEqual(_shippingPrice, orderSummaryPage.GetShippingPrice(),
                "Product shipping price is wrong on Order Summary page.");
            Assert.AreEqual((_shippingPrice + (TotalQuantity*productSubTotalPrice)),
                orderSummaryPage.GetTotalOrderAmount(),
                "Product grand total is wrong on Order Summary page.");

            Debug.WriteLine("Product size:" + orderSummaryPage.GetProductSize());
            Debug.WriteLine("Order Number is:" + orderSummaryPage.GetOrderNumber());
            Debug.WriteLine("Expected arrival date is:" + orderSummaryPage.GetExpectedArrivalDate());
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