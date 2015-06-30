using MAG.WebTesting.BasicElements;
using MAG.WebTesting.Browsers;
using MAG.WebTesting.Pages;
using OpenQA.Selenium;

namespace MssWebUiTest.Widgets
{
    public class SearchWidget : IAppWidget
    {
        private readonly IBrowserTestingSession _testingSession;

        public SearchWidget(IBrowserTestingSession testingSession)
        {
            _testingSession = testingSession;
        }

        public void Type(string input)
        {
            _testingSession.Browser.WaitFor(By.Name("Ntt"));


            _testingSession.GetDriver<TextBox>(By.Name("Ntt"))
                .EnterText(input);
        }

        public void Submit()
        {
            _testingSession.GetDriver<Button>(By.CssSelector(".searchfield button")).Click();
            _testingSession.Browser.WaitFor(By.CssSelector("#searchRefinements"));
        }
    }
}