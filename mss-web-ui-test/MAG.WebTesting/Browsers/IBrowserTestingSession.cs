using System;
using OpenQA.Selenium;

namespace MAG.WebTesting.Browsers
{
    public interface IBrowserTestingSession : IDisposable
    {
        void Start();
        IBrowserInstance Browser { get; }
        T GetDriver<T>(By selector);
        
        bool IsRunningOnBuildServer();
    }
}