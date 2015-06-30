using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using Castle.Windsor;
using MAG.WebTesting.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace MAG.WebTesting.Browsers
{
    public abstract class BaseBrowserInstance : IBrowserInstance
    {
        private readonly IWindsorContainer _container;

        protected BaseBrowserInstance(IWindsorContainer container)
        {
            _container = container;
        }

        public abstract void Dispose();
        public abstract RemoteWebDriver WebDriver { get; }
        public abstract SupportedBrowserType BrowserType { get; }

        public bool IsElementVisible(By selector)
        {
            try
            {
                return FindElement(selector).Displayed;
            }
            catch (NullReferenceException)
            {
                return false;
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

        public bool IsTextPresent(string textToFind)
        {
            return WebDriver.IsTextPresent(textToFind);
        }

        public string GetLocation()
        {
            return WebDriver.Url ?? String.Empty;
        }

        public void NavigateTo(string url)
        {
            try
            {
                WebDriver.Navigate().GoToUrl(url);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to navigate to url '{0}'. Exception:", url);
                Debug.WriteLine(e);
            }
        }

        public TPage NavigateTo<TPage>() where TPage : IAppPage
        {
            var page = _container.Resolve<TPage>();
            page.Navigate();
            WebDriver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(50000));
            return page;
        }

        public TPage NavigateTo<TPage>(object args) where TPage : IAppPage
        {
            var page = _container.Resolve<TPage>(args);
            page.Navigate();
            WebDriver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(50000));
            return page;
        }

        public string GetPageSource()
        {
            try
            {
                return WebDriver.PageSource;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to fetch page source. Exception:");
                Debug.WriteLine(e);
                return string.Empty;
            }
        }

        public void TakeScreenshot(string location)
        {
            var screenshotResponse = ((ITakesScreenshot) WebDriver).GetScreenshot();
            screenshotResponse.SaveAsFile(location, ImageFormat.Jpeg);
        }

        public bool IsElementPresent(By selector)
        {
            var presence = false;
            try
            {
                WebDriver.FindElement(selector);
                presence = true;
            }
            catch (Exception e)
            {
                presence = false;
            }
            return presence;
        }

        public bool IsElementNotPresent(By selector)
        {
            int size = 0;
            size = WebDriver.FindElements(selector).Count();
            if (size > 0)
                return true;
            else
                return false;
        }

        public bool IsElementDisabled(By selector)
        {
            return WebDriver.IsElementDisabled(selector);
        }

        public IWebElement FindElement(By selector)
        {
            //System.Diagnostics.Debug.WriteLine("Total frames:" + FindElements(selector).Count());
            return FindElements(selector).FirstOrDefault();
        }

        public IWebElement FindElementsWithIndex(By selector, int index)
        {
            return WebDriver.FindElements(selector)[index];
        }

        public IEnumerable<IWebElement> FindElements(By selector)
        {
            //return WebDriver.FindElements(selector);

            var w = new DefaultWait<IWebDriver>(WebDriver);

            w.Timeout = TimeSpan.FromSeconds(2);
            w.PollingInterval = TimeSpan.FromMilliseconds(100);
            w.IgnoreExceptionTypes(typeof (NoSuchElementException), typeof (StaleElementReferenceException));

            //waits for true or a non-null value
            return w.Until(b => { return b.FindElements(selector); });
        }

        public void WaitFor(By selector)
        {
            WaitFor(b =>
            {
                var el = b.FindElement(selector);
                return el != null && el.Displayed;
            });
        }

        public void WaitFor(Func<IBrowserInstance, bool> condition)
        {
            var w = new DefaultWait<IBrowserInstance>(this);
            w.Timeout = TimeSpan.FromSeconds(5);
            w.PollingInterval = TimeSpan.FromMilliseconds(100);

            //waiting for a true or non-null value
            w.Until(condition);
        }

        public void WaitForElement(By by, int timeOutSecs) 
        {		
          var counter = 0;
            while(counter<timeOutSecs) {						
             if (IsElementPresent(by))  {
                    Debug.WriteLine("Waited for " + counter + " secs for element to get displayed");
            	return;
             } else  {
                 Thread.Sleep(1000);
                counter++;
            }			
            Debug.WriteLine("Counter Count"+counter);
        }
        Debug.WriteLine("TimeOut Error: Element did not load in time");
        }


        public void WaitForPageLoad()
        {
            WebDriver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(9000));
        }

        public void ImplicitWait()
        {
            WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
        }

        public void SwitchToActiveElement()
        {
            WaitForPageLoad();
            ImplicitWait();
            WebDriver.SwitchTo().ActiveElement();
        }

        public void SwitchToFrameBy(By selector)
        {
            WebDriver.SwitchTo().Frame(WebDriver.FindElement(selector));
        }

        public void SwitchToDefaultContent()
        {
            WebDriver.SwitchTo().DefaultContent();
        }

        public void WaitForDropDownValues(By selector)
        {
            int size = 1;
            WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(5));
            do
            {
                Thread.Sleep(5);
                size = WebDriver.FindElements(selector).Count();
            } while (size < 1);

        }

        public void WaitForTextToPresent(By selector, string textToSearch, int timeToWait)
        {
            WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(timeToWait));
             wait.Until(d => d.FindElement(selector).Text.Contains(textToSearch));
        }
    }
}