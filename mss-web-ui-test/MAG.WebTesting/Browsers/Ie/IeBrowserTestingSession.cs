using Castle.Windsor;

namespace MAG.WebTesting.Browsers.Ie
{
    public class IeBrowserTestingSession : BaseBrowserTestingSession
    {
        private readonly IWindsorContainer _container;

        public IeBrowserTestingSession(IWindsorContainer container) : base(container)
        {
            _container = container;
        }


        protected override IBrowserInstance GetInstance()
        {
            return new IeBrowserInstance(_container);
        }
    }
}