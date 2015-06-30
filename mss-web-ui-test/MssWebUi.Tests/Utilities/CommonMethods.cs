using System;
using MAG.WebTesting.Browsers;
using MAG.WebTesting.Pages;
using OpenQA.Selenium;

namespace MssWebUi.Tests.Utilities
{
    public class CommonMethods : BasePage
    {
        public CommonMethods(IBrowserTestingSession testingSession) : base(testingSession, "")
        {
        }

        public decimal GetSavings(By selector, int index)
        {
            char[] splitChar = {'$', ' '};
            decimal savings = 0;
                if (TestingSession.Browser.IsElementPresent(selector))
                {
                    var strArr = TestingSession.Browser.FindElement(selector).Text.Split(splitChar);
                    savings = decimal.Parse(strArr[index]);
                }else
                savings = 0;
            return savings;
        }

        public decimal GetTax(By selector, int index)
        {
            decimal tax = 0;
            try
            {
                tax = GetSavings(selector, index);
             }catch (Exception e){
                tax = 0;
             }
            return tax;
        }
    }
}