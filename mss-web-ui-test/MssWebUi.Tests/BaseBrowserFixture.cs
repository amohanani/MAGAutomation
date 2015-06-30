using MAG.WebTesting;
using MAG.WebTesting.Browsers;
using NUnit.Framework;

namespace MssWebUiTest
{
    /// <summary>
    /// Provides a base test that has the session already initialized
    /// </summary>
    public abstract class BaseBrowserFixture
    {
        public string GetPath(string path = "")
        {
            return Configuration.BaseUrl + path;
        }

        private bool _open;
        protected IBrowserTestingSession Session { get; private set; }

        protected void LeaveTheBrowserOpen()
        {
            _open = true;
        }
        
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var factory = new BrowserTestingSessionFactory();
            Session = factory.CreateSession<TestAssemblyMarker>(); 
            
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            if (!_open)
                Session.Dispose();
        }

        [SetUp]
        protected void SetUp()
        {
            BeforeEach();
        }

        [TearDown]
        public void TearDown()
        {
            AfterEach();
        }

        protected virtual void BeforeEach()
        {
            
        }

        protected virtual void AfterEach()
        {
            
        }
    }
}