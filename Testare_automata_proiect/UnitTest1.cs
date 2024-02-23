using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;



namespace Testare_automata_proiect
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLogin()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            IWebDriver driver = new ChromeDriver(chromeOptions);
            try
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

                IWebElement usernameField = driver.FindElement(By.Id("username"));
                IWebElement passwordField = driver.FindElement(By.Id("password"));
                IWebElement loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));

                usernameField.SendKeys("tomsmith");
                passwordField.SendKeys("SuperSecretPassword!");

                loginButton.Click();

                IWebElement successMessage = driver.FindElement(By.CssSelector(".flash.success"));

                Assert.IsTrue(successMessage.Displayed);
            }
            finally
            {
                driver.Quit();
            }
        }

        [TestMethod]
        public void TestBasicAuthentication()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                string url = "https://the-internet.herokuapp.com/basic_auth";
                string username = "admin";
                string password = "admin";

                string urlWithCredentials = $"https://{username}:{password}@the-internet.herokuapp.com/basic_auth";

                driver.Navigate().GoToUrl(urlWithCredentials);

                IWebElement successMessage = driver.FindElement(By.XPath("//div[@id='content']/div[@class='example']/p"));

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => successMessage.Displayed);

                Assert.IsTrue(successMessage.Text.Contains("Congratulations!"));

                System.Threading.Thread.Sleep(3000);
            }
        }

        [TestMethod]
        public void TestDropdownSelection()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized");

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dropdown");

                IWebElement dropdown = driver.FindElement(By.Id("dropdown"));

                SelectElement select = new SelectElement(dropdown);

                select.SelectByIndex(1);

                Assert.AreEqual("Option 1", select.SelectedOption.Text);

                select.SelectByValue("2");

                Assert.AreEqual("Option 2", select.SelectedOption.Text);

                System.Threading.Thread.Sleep(3000);
            }
        }

        [TestMethod]
        public void TestIframeTitle()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/iframe");

                IWebElement iframeElement = driver.FindElement(By.TagName("iframe"));

                driver.SwitchTo().Frame(iframeElement);

                try
                {
                    IWebElement titleInsideIframe = driver.FindElement(By.TagName("h1"));

                    Assert.AreEqual("An iFrame containing the TinyMCE WYSIWYG Editor", titleInsideIframe.Text.Trim());

                    System.Threading.Thread.Sleep(5000);
                }
                catch (NoSuchElementException ex)
                {
                    Console.WriteLine("Nu s-a găsit niciun element de titlu în interiorul iframe-ului.");
                }

                finally
                {
                    driver.SwitchTo().DefaultContent();
                }
            }
        }

        [TestMethod]
        public void TestIframeInteraction()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/iframe");

                driver.SwitchTo().Frame(0);

                IWebElement textInsideIframe = driver.FindElement(By.Id("tinymce"));

                textInsideIframe.Clear();

                textInsideIframe.SendKeys("Hello, this is a test message inside the iframe!");

                driver.SwitchTo().DefaultContent();

                System.Threading.Thread.Sleep(5000);
            }
        }
        [TestMethod]
        public void TestIframeTextContent()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/iframe");

                
                IWebElement iframeElement = driver.FindElement(By.TagName("iframe"));

                driver.SwitchTo().Frame(iframeElement);

                try
                {
                    IWebElement textInsideIframe = driver.FindElement(By.TagName("p"));

                    Assert.IsTrue(textInsideIframe.Text.Contains("Your content goes here."));

                    System.Threading.Thread.Sleep(5000);
                }
                catch (NoSuchElementException ex)
                {
                    Console.WriteLine("Nu s-a găsit niciun element de text în interiorul iframe-ului.");
                }
                finally
                {
                    driver.SwitchTo().DefaultContent();
                }
            }
        }

        [TestMethod]
        public void TestDragAndDrop()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized");

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/drag_and_drop");

                IWebElement sourceElement = driver.FindElement(By.Id("column-a"));
                IWebElement targetElement = driver.FindElement(By.Id("column-b"));

                Actions builder = new Actions(driver);
                builder.DragAndDrop(sourceElement, targetElement).Perform();

                Assert.AreEqual("A", targetElement.Text);
            }
        }

        [TestMethod]
        public void TestCheckbox()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/checkboxes");

                IWebElement checkbox1 = driver.FindElement(By.XPath("//input[@type='checkbox'][1]"));
                IWebElement checkbox2 = driver.FindElement(By.XPath("//input[@type='checkbox'][2]"));

                Assert.IsFalse(checkbox1.Selected);
                Assert.IsTrue(checkbox2.Selected);

                checkbox1.Click();

                Assert.IsTrue(checkbox1.Selected);
            }
        }
        [TestMethod]
        public void TestJavaScriptAlert()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized");

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            { 
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");

                driver.FindElement(By.XPath("//button[text()='Click for JS Alert']")).Click();

                IAlert alert = driver.SwitchTo().Alert();
                Assert.AreEqual("I am a JS Alert", alert.Text);
                alert.Accept();

                Assert.AreEqual("You successfully clicked an alert", driver.FindElement(By.Id("result")).Text);
            }
        }
        [TestMethod]
        public void TestJavaScriptConfirm()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 
            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");

                driver.FindElement(By.XPath("//button[text()='Click for JS Confirm']")).Click();

                
                IAlert alert = driver.SwitchTo().Alert();
                Assert.AreEqual("I am a JS Confirm", alert.Text);
                alert.Dismiss(); 

                IWebElement resultMessage = driver.FindElement(By.Id("result"));
                Assert.AreEqual("You clicked: Cancel", resultMessage.Text);
            }
        }
        [TestMethod]
        public void TestJavaScriptPrompt()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");

                driver.FindElement(By.XPath("//button[text()='Click for JS Prompt']")).Click();

                IAlert alert = driver.SwitchTo().Alert();
                Assert.AreEqual("I am a JS prompt", alert.Text);

                string inputText = "Hello, Selenium!";
                alert.SendKeys(inputText);
                alert.Accept();

                IWebElement resultMessage = driver.FindElement(By.Id("result"));
                Assert.AreEqual("You entered: " + inputText, resultMessage.Text);
            }
        }
        [TestMethod]
        public void TestHover()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/hovers");

                IWebElement image1 = driver.FindElement(By.CssSelector(".figure:nth-of-type(1)"));
                IWebElement image2 = driver.FindElement(By.CssSelector(".figure:nth-of-type(2)"));
                IWebElement image3 = driver.FindElement(By.CssSelector(".figure:nth-of-type(3)"));

                Actions builder = new Actions(driver);

                builder.MoveToElement(image1).Perform();
                Assert.IsTrue(image1.FindElement(By.CssSelector(".figcaption")).Text.Contains("name: user1"));
                builder.MoveToElement(image2).Perform();
                Assert.IsTrue(image2.FindElement(By.CssSelector(".figcaption")).Text.Contains("name: user2"));
                builder.MoveToElement(image3).Perform();
                Assert.IsTrue(image3.FindElement(By.CssSelector(".figcaption")).Text.Contains("name: user3"));

            }
        }

        [TestMethod]
        public void TestGeolocation()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/geolocation");

                driver.FindElement(By.CssSelector("#content button")).Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                try
                {
                    IWebElement locationElement = wait.Until(d => driver.FindElement(By.Id("lat-value")));
                    Assert.IsTrue(locationElement.Text.Length > 0);
                }
                finally
                {
                    driver.Quit();
                }
            }


        }
        [TestMethod]
        public void TestHorizontalSlider()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/horizontal_slider");

                IWebElement slider = driver.FindElement(By.CssSelector(".sliderContainer input"));
                Actions builder = new Actions(driver);
                builder.ClickAndHold(slider).MoveByOffset(50, 0).Release().Perform();

                IWebElement sliderValue = driver.FindElement(By.Id("range"));
                Assert.AreEqual("4.5", sliderValue.Text);
            }
        }

        [TestMethod]
        public void TestLargeDeepDOM()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/large");

                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                jsExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                Assert.AreEqual(driver.FindElement(By.Id("page-footer")).Text, "Powered by Elemental Selenium");
            }

        }

        [TestMethod]
        public void TestNotificationMessages()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 

            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/notification_message_rendered");

                driver.FindElement(By.CssSelector("a[href*='notification_message']")).Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement notificationMessage = wait.Until(d => driver.FindElement(By.Id("flash")));

                Assert.IsTrue(notificationMessage.Text.Contains("Action successful") || notificationMessage.Text.Contains("Action unsuccesful"));
            }

        }



        [TestMethod]
        public void TestInputIncrement()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized"); 
            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/inputs");

                IWebElement inputElement = driver.FindElement(By.TagName("input"));

                string initialValue = inputElement.GetAttribute("value");

                inputElement.SendKeys(Keys.ArrowUp);

                IWebElement body = driver.FindElement(By.TagName("body"));
                body.Click();

                Assert.AreEqual((initialValue + 1), inputElement.GetAttribute("value"));
                
            }
        }
        [TestMethod]
        public void TestNavigation()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized");
            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/");
                Assert.AreEqual("The Internet", driver.Title);
            }
        }
        [TestMethod]
        public void TestLoginButton()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized");
            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

                IWebElement loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));
                Assert.IsTrue(loginButton.Displayed);
            }
        }

        [TestMethod]
        public void TestAddAndDeleteElement()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized");
            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {  
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/add_remove_elements/");

                IWebElement addButton = driver.FindElement(By.CssSelector("button[onclick='addElement()']"));
                Assert.IsTrue(addButton.Displayed);
                addButton.Click();

                IWebElement addedElement = driver.FindElement(By.CssSelector(".added-manually"));
                Assert.IsTrue(addedElement.Displayed);

                IWebElement deleteButton = driver.FindElement(By.CssSelector("button[onclick='deleteElement()']"));
                Assert.IsTrue(deleteButton.Displayed);
                deleteButton.Click();

                IReadOnlyCollection<IWebElement> elements = driver.FindElements(By.CssSelector(".added-manually"));
                Assert.AreEqual(0, elements.Count);
            }
        }

    }
}
