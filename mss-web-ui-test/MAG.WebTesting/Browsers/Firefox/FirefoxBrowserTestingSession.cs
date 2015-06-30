using Castle.Windsor;

namespace MAG.WebTesting.Browsers.Firefox
{
    public class FirefoxBrowserTestingSession : BaseBrowserTestingSession
    {
        
        public FirefoxBrowserTestingSession(IWindsorContainer container) : base(container)
        {
        }

        protected override IBrowserInstance GetInstance()
        {
            var instance = new FirefoxBrowserInstance(Container);
            return instance;
        }

    }
}