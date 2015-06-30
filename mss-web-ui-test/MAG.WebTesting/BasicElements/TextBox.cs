using MAG.WebTesting.Browsers;
using OpenQA.Selenium;

namespace MAG.WebTesting.BasicElements
{
    public class TextBox
    {
        private readonly By _selector;
        private readonly IBrowserTestingSession _testingSession;

        public TextBox(By selector, IBrowserTestingSession testingSession)
        {
            _selector = selector;
            _testingSession = testingSession;
        }

        public void EnterText(string text)
        {
            _testingSession.Browser.FindElement(_selector).Clear();
            _testingSession.Browser.FindElement(_selector).SendKeys(text);
        }

        public string GetValue()
        {
            return _testingSession.Browser.FindElement(_selector).GetAttribute("value");
        }

        public string GetText()
        {
            return _testingSession.Browser.FindElement(_selector).Text;
        }
    }
}