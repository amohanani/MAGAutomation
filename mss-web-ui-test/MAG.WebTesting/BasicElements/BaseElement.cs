using MAG.WebTesting.Browsers;
using OpenQA.Selenium;

namespace MAG.WebTesting.BasicElements
{
    public class BaseElement
    {
        private readonly By _selector;
        public IBrowserTestingSession TestingSession;

        public BaseElement(By selector, IBrowserTestingSession testingSession)
        {
            _selector = selector;
            TestingSession = testingSession;
        }

        //Visibility testingSession
        public bool IsVisible()
        {
            try
            {
                return TestingSession.Browser.FindElement(_selector).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }
    }
}