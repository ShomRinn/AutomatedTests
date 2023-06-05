using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;

namespace SeleniumTests
{
    [TestFixture]
    public class Tests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://svyatoslav.biz/testlab/wt/");
        }

        [Test]
        public void TestMenuAndBanners()
        {
            String bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.IsTrue(bodyText.Contains("menu") && bodyText.Contains("banners"));
        }

        [Test]
        public void TestTableFooter()
        {
            String cellText = driver.FindElement(By.XPath("//table/tbody/tr[last()]/td[last()]")).Text;
            Assert.AreEqual("CoolSoft by Somebody", cellText);
        }

        [Test]
        public void TestEmptyFormFields()
        {
            IWebElement genderField = driver.FindElement(By.Name("gender"));
            ReadOnlyCollection<IWebElement> textFields = driver.FindElements(By.CssSelector("input[type='text']"));

            foreach (IWebElement textField in textFields)
            {
                Assert.IsTrue(string.IsNullOrEmpty(textField.GetAttribute("value")));
            }

            Assert.IsTrue(string.IsNullOrEmpty(genderField.GetAttribute("value")));
        }

        [Test]
        public void TestFormSubmission()
        {
            IWebElement heightField = driver.FindElement(By.Name("height"));
            IWebElement weightField = driver.FindElement(By.Name("weight"));

            heightField.SendKeys("50");
            weightField.SendKeys("3");

            driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            Assert.IsFalse(driver.FindElement(By.Id("form-id")).Displayed);
            Assert.IsTrue(driver.FindElement(By.Id("warning-id")).Text.Contains("Too large body weight"));
        }

        [Test]
        public void TestFormStructure()
        {
            IWebElement form = driver.FindElement(By.Id("form-id"));
            ReadOnlyCollection<IWebElement> textFields = form.FindElements(By.CssSelector("input[type='text']"));
            ReadOnlyCollection<IWebElement> radioButtons = form.FindElements(By.CssSelector("input[type='radio']"));

            IWebElement button = form.FindElement(By.CssSelector("input[type='submit']"));

            Assert.AreEqual(3, textFields.Count);
            Assert.AreEqual(2, radioButtons.Count);
            Assert.IsNotNull(button);
        }

        [Test]
        public void TestHeightWeightValidation()
        {
            IWebElement heightField = driver.FindElement(By.Name("height"));
            IWebElement weightField = driver.FindElement(By.Name("weight"));

            heightField.SendKeys("0");
            weightField.SendKeys("0");

            driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            Assert.IsTrue(driver.FindElement(By.Id("height-error-id")).Text.Contains("50-300 cm"));
            Assert.IsTrue(driver.FindElement(By.Id("weight-error-id")).Text.Contains("3-500 kg"));
        }

        [Test]
        public void TestCurrentDateDisplay()
        {
            String currentDate = driver.FindElement(By.Id("date-id")).Text;
            DateTime dateValue;

            bool parseResult = DateTime.TryParseExact(currentDate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out dateValue);

            Assert.IsTrue(parseResult);
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}

