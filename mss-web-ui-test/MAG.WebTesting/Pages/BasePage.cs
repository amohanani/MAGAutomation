using MAG.WebTesting.Browsers;

namespace MAG.WebTesting.Pages
{
    public abstract class BasePage : IAppPage
    {
        private readonly string _path;

        public BasePage(IBrowserTestingSession testingSession, string path)
        {
            TestingSession = testingSession;
            _path = path;
        }

        public IBrowserTestingSession TestingSession { get; set; }

        public void Navigate()
        {
            var fullPath = Configuration.BaseUrl + _path;
            TestingSession.Browser.NavigateTo(fullPath);
        }
    }
}