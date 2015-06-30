using OpenQA.Selenium;

namespace MAG.WebTesting.Browsers
{
    public static class WebDriverExtensions
    {
        public static bool IsTextPresent(this IWebDriver driver, string textToFind)
        {
            return driver.PageSource.Contains(textToFind);
        }

        public static bool IsElementDisabled(this IWebDriver driver, By selector)
        {
            var element = driver.FindElement(selector);
            if (element == null)
            {
                return false;
            }

            return !element.Enabled;
        }

        public static bool IsElementPresent(this IWebDriver driver, By selector)
        {
            try
            {
                driver.FindElement(selector);
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }
    }
}