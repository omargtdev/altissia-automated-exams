using OpenQA.Selenium;

namespace IdiomaExtranjeroAutomatedExams
{
    internal class WebElements
    {
        internal class Altissia
        {
            public static readonly By GOT_IT_BTN = By.XPath("//button[contains(string(), ' Got it ')]");
            public static readonly By REJECT_ALL_BTN = By.Id("ppms_cm_reject-all");

            public static readonly By EXAM_TITLE = By.CssSelector("//h1[contains(string(), 'Summary Test')]");
            public static readonly By EXAM_QUESTION_NUMBER = By.CssSelector("p.progress-bar-numbers");
            public static readonly By EXAM_START_BTN = By.CssSelector("button[data-test='main-button']");
            public static readonly By EXAM_VALIDATE_BTN = By.CssSelector("button.btn.footer-button-bar-btn"); // Next btn is the same
            public static readonly By EXAM_QUESTION_INPUTS = By.CssSelector("input.input.question-input");
            public static readonly By EXAM_CORRECT_ANSWERS = By.CssSelector("span.input-answer.input-answer-is-correct");
            public static readonly By EXAM_RESULT = By.CssSelector("p.user-result-box");
            public static readonly By EXAM_RETRY = By.XPath("//div[@class=\"result-button-box\"]/*[1]/button"); 

        }

        internal class ISIL
        {
            // Login
            public static readonly By LOGIN_INPUT = By.Id("usernameUserInput");
            public static readonly By LOGIN_PASSWORD = By.Id("password");
            public static readonly By LOGIN_BTN = By.CssSelector("button[tabindex=\"4\"]");

            // ISIL+
            public static readonly By COURSE_DESCRIPTION_SECTION = By.CssSelector("ul.tiles");
            public static readonly By COURSE_CONTENT_SECTION = By.Id("sectionlink-2");
            public static readonly By COURSE_ALTISSIA_LINK = By.XPath("//li[@id='module-572349']/div/div/a");

            public static readonly By PAGE_IFRAME = By.CssSelector("iframe");
        }
    }
}
