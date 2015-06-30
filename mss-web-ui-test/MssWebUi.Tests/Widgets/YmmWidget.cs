using MAG.WebTesting.BasicElements;
using MAG.WebTesting.Browsers;
using MAG.WebTesting.Pages;
using OpenQA.Selenium;

namespace MssWebUiTest.Widgets
{
    public class YmmWidget : IAppWidget
    {
        private By _root;
        private readonly IBrowserTestingSession _testingSession;

        public YmmWidget(IBrowserTestingSession testingSession, By selector)
        {
            _testingSession = testingSession;
            _root = selector;
        }


        public void SelectYear(string year)
        {
            WaitForOverlay();

            var yearElement = _testingSession.GetDriver<SelectBox>(By.Name("dd_year"));
            yearElement.SelectByDisplay(year);

        }

        public void SelectMake(string make)
        {
            WaitForOverlay();

            var makeElement = _testingSession.GetDriver<SelectBox>(By.Name("ddMake"));
            makeElement.SelectByDisplay(make);

        }

        public void SelectModel(string model)
        {
            WaitForOverlay();

            _testingSession.GetDriver<SelectBox>(By.Name("ddModel"))
                .SelectByDisplay(model);
        }

        public void Submit()
        {
            WaitForOverlay();

            _testingSession.GetDriver<Button>(By.Name("btn_fitment")).Click();
        }

        private void WaitForOverlay()
        {
            _testingSession.Browser.WaitFor(b =>
            {
                return !b.IsElementVisible(By.CssSelector("#loader"));
            });
        }
    }
}