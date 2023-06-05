using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            // Установка кодировки консоли на UTF-8
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Создание экземпляра веб-драйвера Chrome
            IWebDriver driver = new ChromeDriver();

            // Навигация к тестируемому веб-сайту
            driver.Navigate().GoToUrl("http://svyatoslav.biz/testlab/wt/");

            // Тест 1 - Проверка наличия слов: "menu" и "banners" на главной странице
            Console.WriteLine("\nТест 1: Проверка наличия слов: \"menu\" и \"banners\" на главной странице");
            bool isMenuPresent = driver.PageSource.Contains("menu");
            bool isBannerPresent = driver.PageSource.Contains("banners");
            Console.WriteLine("Содержит \"menu\": " + isMenuPresent);
            Console.WriteLine("Содержит \"banners\": " + isBannerPresent);

            // Тест 2 - Поиск нижней ячейки таблицы и проверка наличия текста "CoolSoft by Somebody"
            Console.WriteLine("\nТест 2: Поиск нижней ячейки таблицы, проверка наличия текста \"CoolSoft by Somebody\"");
            string pageSource = driver.PageSource;
            string searchedText = "© CoolSoft by Somebody";
            bool isSearchedTextPresent = pageSource.Contains(searchedText);
            Console.WriteLine("Наличие текста \"CoolSoft by Somebody\": " + isSearchedTextPresent);

            // Тест 3 - Проверка, что по умолчанию все текстовые поля формы пусты, а поле «Пол» не выбрано
            Console.WriteLine("\nТест 3: Проверка, что по умолчанию все текстовые поля формы пусты, а поле «Пол» не выбрано");
            IWebElement heightElement = driver.FindElement(By.Name("height"));
            IWebElement weightElement = driver.FindElement(By.Name("weight"));
            IWebElement genderElement = driver.FindElement(By.Name("gender"));
            bool isHeightFieldEmpty = string.IsNullOrEmpty(heightElement.GetAttribute("value"));
            bool isWeightFieldEmpty = string.IsNullOrEmpty(weightElement.GetAttribute("value"));
            bool isGenderNotSelected = !genderElement.Selected;
            Console.WriteLine("Поле \"Рост\" пустое: " + isHeightFieldEmpty);
            Console.WriteLine("Поле \"Вес\" пустое: " + isWeightFieldEmpty);
            Console.WriteLine("Поле \"Пол\" не выбрано: " + isGenderNotSelected);

            // Тест 4 - Проверка исчезновения формы и появления надписи после ввода значений в поля и нажатия на кнопку отправки
            Console.WriteLine("\nТест 4: Проверка исчезновения формы и появления надписи после ввода значений и нажатия на кнопку отправки");
            heightElement.SendKeys("50");
            weightElement.SendKeys("3");
            IWebElement submitButton = driver.FindElement(By.XPath("//input[@type='submit']"));
            submitButton.Click();
            bool isMessageDisplayed = driver.PageSource.Contains("Слишком большая масса тела");
            bool isFormDisappeared = !driver.FindElement(By.TagName("form")).Displayed;
            Console.WriteLine("Форма исчезает: " + isFormDisappeared);
            Console.WriteLine("Надпись «Слишком большая масса тела» отображается: " + isMessageDisplayed);

            // Тест 5 - Проверка содержания главной страницы после повторного открытия
            Console.WriteLine("\nТест 5: Проверка содержания главной страницы после повторного открытия");
            driver.Navigate().Back();
            bool isFormPresent = driver.FindElement(By.TagName("form")).Displayed;
            bool areTextFieldsPresent = driver.FindElements(By.XPath("//input[@type='text']")).Count == 3;
            bool areRadioButtonsPresent = driver.FindElements(By.XPath("//input[@type='radio']")).Count == 2;
            bool isSubmitButtonPresent = driver.FindElement(By.XPath("//input[@type='submit']")).Displayed;
            Console.WriteLine("Присутствует форма: " + isFormPresent);
            Console.WriteLine("Присутствуют 3 текстовых поля: " + areTextFieldsPresent);
            Console.WriteLine("Присутствуют 2 радиокнопки: " + areRadioButtonsPresent);
            Console.WriteLine("Присутствует кнопка отправки: " + isSubmitButtonPresent);

            // Тест 6 - Проверка наличия сообщений об ошибке при вводе неверных значений в поля веса и роста
            Console.WriteLine("\nТест 6: Проверка наличия сообщений об ошибке при вводе неверных значений в поля веса и роста");
            heightElement.Clear();
            heightElement.SendKeys("600");
            weightElement.Clear();
            weightElement.SendKeys("600");
            submitButton.Click();
            bool isHeightErrorMessagePresent = driver.PageSource.Contains("Рост должен быть в диапазоне 50-300 см.");
            bool isWeightErrorMessagePresent = driver.PageSource.Contains("Вес должен быть в диапазоне 3-500 кг.");
            Console.WriteLine("Содержит сообщение об ошибке для поля рост: " + isHeightErrorMessagePresent);
            Console.WriteLine("Содержит сообщение об ошибке для поля вес: " + isWeightErrorMessagePresent);

            // Тест 7 - Проверка наличия текущей даты на главной странице
            Console.WriteLine("\nТест 7: Проверка наличия текущей даты в формате «DD.MM.YYYY» на главной странице");
            string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
            bool isCurrentDatePresent = driver.PageSource.Contains(currentDate);
            Console.WriteLine("Содержит текущую дату (" + currentDate + "): " + isCurrentDatePresent);

            driver.Quit();
        }
    }
}
