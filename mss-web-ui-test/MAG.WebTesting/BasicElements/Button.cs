using MAG.WebTesting.Browsers;
using OpenQA.Selenium;

namespace MAG.WebTesting.BasicElements
{
    public class Button
    {
        private readonly By _selector;
        private readonly IBrowserTestingSession _testSession;

        public Button(By selector, IBrowserTestingSession testSession)
        {
            _selector = selector;
            _testSession = testSession;
        }

        public void Click()
        {
            var el = _testSession.Browser.FindElement(_selector);
            el.Click();
        }
    }
}