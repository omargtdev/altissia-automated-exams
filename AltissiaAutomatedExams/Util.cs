using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace IdiomaExtranjeroAutomatedExams
{
    internal class Util
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private IJavaScriptExecutor _jsExecutor;

        public Util(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
            _jsExecutor = (IJavaScriptExecutor)_driver;
        }

        public bool WaitForDocumentLoaded()
        {
            return _jsExecutor.ExecuteScript("return document.readyState").Equals("complete");
        }

        public void ScrollToEnd()
        {
            _jsExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        }

        public void ExplicitWait(int secondsToWait)
        {
            int counter = 0;
            if(secondsToWait == 0)
                return;

            _wait.Until(e => {
                counter++;
                return counter == secondsToWait;
            });
        }

        internal void ShowAlert(string message)
        {
            _jsExecutor.ExecuteScript($"window.alert('{message}');");
        }
    }
}
