using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

using IdiomaExtranjeroAutomatedExams;
using IdiomaExtranjeroAutomatedExams.Configuration;

// Settings (json)
IConfiguration appSettings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string? courseUrl = appSettings.GetSection("CourseUrl").Get<string>();
Credentials? credentials = appSettings.GetSection("Credentials").Get<Credentials>();

if (courseUrl is null || credentials is null)
{
    Console.WriteLine("Please, defined your credentials of ISIL and the course url in appsettings.json");
    return;
}

IWebDriver driver = new EdgeDriver();
// Driver Settings 
IOptions driverOptions = driver.Manage();
driverOptions.Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
driverOptions.Window.Maximize();
var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

var utils = new Util(driver, wait);

// Start
driver.Navigate().GoToUrl(courseUrl);

// Login
driver.FindElement(WebElements.ISIL.LOGIN_INPUT).SendKeys(credentials.Username);
driver.FindElement(WebElements.ISIL.LOGIN_PASSWORD).SendKeys(credentials.Password);
driver.FindElement(WebElements.ISIL.LOGIN_BTN).Click();

EnterToAltissia(true);

if (driver.WindowHandles.Count > 1)
{

    driver.SwitchTo().Window(driver.WindowHandles[1]);

    if (driver.Title == "Error") // Sometimes, the request to login in altissia fails
    {
        driver.Close();
        driver.SwitchTo().Window(driver.WindowHandles[0]); // Move to the first tab
        driver.Navigate().GoToUrl(courseUrl);
        // Only, do it again, but without open the content section
        EnterToAltissia(false);

    }
    // Altissia platform
    driver.SwitchTo().Window(driver.WindowHandles[1]);
}
else
{
    utils.ExplicitWait(5); // Wait to "charge" the page
    string altissiaPage = driver.FindElement(WebElements.ISIL.PAGE_IFRAME).GetAttribute("src");
    driver.Navigate().GoToUrl(altissiaPage);
}

wait.Until(e => e.FindElement(WebElements.Altissia.REJECT_ALL_BTN).Displayed);
driver.FindElement(WebElements.Altissia.REJECT_ALL_BTN).Click();

wait.Until(e => e.FindElement(WebElements.Altissia.GOT_IT_BTN).Displayed);
driver.FindElement(WebElements.Altissia.GOT_IT_BTN).Click();

// Make exams
while (true)
{
    Console.Clear();
    utils.ShowAlert("Select your exam to resolve. Then go to console and write `start`");
    Console.Write("Put command: ");
    string? command = Console.ReadLine();
    if (command is null || command.Trim() == string.Empty)
        continue;

    if (command.Trim().ToLower() == "exit")
        break;

    if(command.Trim().ToLower() == "start")
    {
        // Start exam
        driver.FindElement(WebElements.Altissia.EXAM_START_BTN).Click();

        int numberOfQuestions = Convert.ToInt32(driver.FindElement(WebElements.Altissia.EXAM_QUESTION_NUMBER)
            .Text
            .Split('/')[1]
            .Trim());

        var allAnswers = new List<string[]>();
        for (int i = 0; i < numberOfQuestions; i++)
        {
            // Show answers
            driver.FindElement(WebElements.Altissia.EXAM_VALIDATE_BTN).Click();
            // Get them
            var answers = driver.FindElements(WebElements.Altissia.EXAM_CORRECT_ANSWERS).Select(e => e.Text.Trim()).ToArray();
            // Store them
            allAnswers.Add(answers);
            // Go to next
            driver.FindElement(WebElements.Altissia.EXAM_VALIDATE_BTN).Click();
        }

        // Retry the exam
        wait.Until(e => e.FindElement(WebElements.Altissia.EXAM_RETRY).Displayed);
        driver.FindElement(WebElements.Altissia.EXAM_RETRY).Click();
        wait.Until(e => e.FindElement(WebElements.Altissia.EXAM_START_BTN).Displayed);
        driver.FindElement(WebElements.Altissia.EXAM_START_BTN).Click();

        foreach (var answers in allAnswers)
        {
            var inputs = driver.FindElements(WebElements.Altissia.EXAM_QUESTION_INPUTS);
            // Every question can be one, two or more inputs to complete
            for (int j = 0; j < answers.Length; j++)
                inputs[j].SendKeys(answers[j]);
            driver.FindElement(WebElements.Altissia.EXAM_VALIDATE_BTN).Click(); // Validate
            driver.FindElement(WebElements.Altissia.EXAM_VALIDATE_BTN).Click(); // Continue to next
        }
        utils.ExplicitWait(3);
    }
}

// End
driver.Quit();

// Functions
void EnterToAltissia(bool openContentSection)
{
    if (openContentSection)
    {
        wait.Until(e => e.FindElement(WebElements.ISIL.COURSE_DESCRIPTION_SECTION).Displayed);
        utils.ScrollToEnd();

        driver.FindElement(WebElements.ISIL.COURSE_CONTENT_SECTION).Click();
    }

    // Wait until the link of altissia displayed
    utils.ExplicitWait(5);
    utils.ScrollToEnd();
    driver.FindElement(WebElements.ISIL.COURSE_ALTISSIA_LINK).Click();
}