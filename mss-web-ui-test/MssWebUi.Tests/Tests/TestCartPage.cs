using MssWebUi.Tests.Pages;
using MssWebUiTest;
using NUnit.Framework;

namespace MssWebUi.Tests.Tests
{
    public class TestCartPage : BaseBrowserFixture
    {
        [Test]
        public void WhenEmpty()
        {
            var page = Session.Browser.NavigateTo<CartPage>();

            var empty = page.IsEmpty();

            Assert.AreEqual(true, empty);
        }
    }
}
