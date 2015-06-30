using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MAG.WebTesting.BasicElements;
using MAG.WebTesting.Browsers.Chrome;
using MAG.WebTesting.Browsers.Firefox;
using MAG.WebTesting.Browsers.Ie;
using MAG.WebTesting.Browsers.Phantom;
using MAG.WebTesting.Pages;

namespace MAG.WebTesting.Browsers
{
    public class BrowserTestingSessionFactory : IBrowserTestingSessionFactory
    {
        private readonly IWindsorContainer _container;
        //logging
        private IBrowserTestingSession _testingSession;

        public BrowserTestingSessionFactory()
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<IWindsorContainer>().Instance(_container));


            _container.Install(
                new BrowserInstaller(),
                new BasicElementsInstaller()
                );
            //new up logging    
        }

        public IBrowserTestingSession CreateSession<TMarker>(
            SupportedBrowserType browserType = SupportedBrowserType.Firefox)
        {
            var session = CreateTestSession(browserType);
            session.Start();
            _container.Register(Classes.FromAssemblyContaining<TMarker>().BasedOn<IAppPage>());
            _container.Register(Classes.FromAssemblyContaining<TMarker>().BasedOn<IAppWidget>());

            _container.Register(Component.For<IBrowserTestingSession>().Instance(session));
            return session;
        }

        private IBrowserTestingSession CreateTestSession(SupportedBrowserType browserType)
        {
            switch (browserType)
            {
                case SupportedBrowserType.Firefox:
                    return new FirefoxBrowserTestingSession(_container);
                case SupportedBrowserType.Ie:
                    return new IeBrowserTestingSession(_container);
                case SupportedBrowserType.Phantom:
                    return new PhantomBrowserTestingSession(_container);
                default:
                    return new ChromeBrowserTestingSession(_container);
            }
        }
    }
}