using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Lab6Tests
{
    public class Selenium
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void CrearNuevoPais_Test()
        {
            // ARRANGE
            var url = "http://localhost:8081/";
            _driver.Manage().Window.Maximize();

            Console.WriteLine("Navegando a: " + url);
            _driver.Navigate().GoToUrl(url);

            // ASSERT 1: Verificar página principal
            var h1Element = _wait.Until(d => d.FindElement(By.TagName("h1")));
            Console.WriteLine("h1 encontrado: " + h1Element.Text);
            Assert.That(h1Element.Text, Is.EqualTo("Lista de países"));

            // ACT - Ir al formulario
            var agregarBoton = _wait.Until(d => d.FindElement(By.LinkText("Agregar país")));
            agregarBoton.Click();

            // ASSERT 2: Verificar formulario
            var h3Element = _wait.Until(d => d.FindElement(By.TagName("h3")));
            Assert.That(h3Element.Text, Is.EqualTo("Formulario de creación de países"));

            // ACT - Llenar formulario
            var nombreInput = _wait.Until(d => d.FindElement(By.CssSelector("input[type='text']")));
            nombreInput.SendKeys("País Selenium Test");

            var selectContinente = _wait.Until(d => d.FindElement(By.TagName("select")));
            selectContinente.SendKeys("América");

            var allTextInputs = _driver.FindElements(By.CssSelector("input[type='text']"));
            allTextInputs[1].SendKeys("Idioma Test");

            // ACT - Enviar formulario
            var guardarBoton = _wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']")));
            guardarBoton.Click();

            // ASSERT 3: Verificar que volvió a la lista
            var h1ElementFinal = _wait.Until(d => d.FindElement(By.TagName("h1")));
            Assert.That(h1ElementFinal.Text, Is.EqualTo("Lista de países"));

            // ASSERT 4: Verificar que el país aparece en la tabla
            var tabla = _wait.Until(d => d.FindElement(By.TagName("table")));
            Assert.That(tabla.Text, Contains.Substring("País Selenium Test"));
        }

        [TearDown]
        public void Teardown()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}