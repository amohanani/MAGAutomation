namespace MAG.WebTesting.Browsers
{
    public interface IBrowserTestingSessionFactory
    {
        IBrowserTestingSession CreateSession<TMarker>(SupportedBrowserType browserType = SupportedBrowserType.Chrome);
    }
}