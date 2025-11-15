using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace UIAutomationTests
{
    public class Selenium
    {
        IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
        }

        [Test]
        public void Enter_To_List_Of_Countries_Test()
        {
            // Arrange
            // Abre una nueva ventana
            var URL = "http://localhost:8081/";
            // Maximiza la pantalla
            _driver.Manage().Window.Maximize();

            // Act
            // Navega a la página que se necesita probar
            _driver.Navigate().GoToUrl(URL);

            // Assert
            // No es un buen ejemplo de assert, use uno diferente
            Assert.IsNotNull(_driver);
        }
    }
}
