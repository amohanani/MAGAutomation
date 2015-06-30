using MssWebUiTest.Pages;
using NUnit.Framework;

namespace MssWebUiTest.Tests
{
    public class TestHomePage : BaseBrowserFixture
    {
        [Test]
        public void TestYMMWidget()
        {
            LeaveTheBrowserOpen();
            var page = Session.Browser.NavigateTo<MssHomePage>();

            page.SelectAMotorcycle("2010", "Honda", "Fury VT1300CX");

            Assert.AreEqual(
                "http://www.jpcycles.com/2010-honda-fury-vt1300cx",
                Session.Browser.GetLocation()
                );
        }

        [Test]
        public void TestSearch()
        {
            Session.Browser.NavigateTo<MssHomePage>()
                .Search("Honda");

            var url = Session.Browser.GetLocation();
            var expected = "http://alpha.demo.motorcycle-superstore.com/search/go?Ntt=Honda";
            url = url.Substring(0, expected.Length);
            Assert.AreEqual(expected, url);
            //Session.Browser.TakeScreenshot("bob.jpg");
        }

        [Test]
        public void AddToCart()
        {
            var page = Session.Browser.NavigateTo<MssProductPage>(new
            {
                Sku = "711-571"
            });
            page.SetQty(22);
            page.AddToCart();

            //Application.Resolve<IOrderRepository>().GetCart(Session.SomeId).Count().ShouldBe(22); // actual database hit
            //UI should have 22 items in the cart icon
        }
    }
}
