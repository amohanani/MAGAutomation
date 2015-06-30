using MAG.WebTesting.Browsers;
using MAG.WebTesting.Pages;
using MssWebUiTest.Widgets;
using OpenQA.Selenium;

namespace MssWebUiTest.Pages
{
    public class MssHomePage : BasePage
    {
        private readonly YmmWidget _widget;
        private readonly SearchWidget _searchWidget;

        public MssHomePage(IBrowserTestingSession testingSession) : base(testingSession, "")
        {
            _widget = TestingSession.GetDriver<YmmWidget>(By.ClassName("fitment-drop-down"));
            _searchWidget = TestingSession.GetDriver<SearchWidget>(By.ClassName("abc"));
        }

        public void SelectAMotorcycle(string year, string make, string model)
        {
            _widget.SelectYear(year);
            _widget.SelectMake(make);
            _widget.SelectModel(model);
            _widget.Submit();
        }

        public void Search(string searchString)
        {
            _searchWidget.Type(searchString);
            _searchWidget.Submit();
        }
    }
}