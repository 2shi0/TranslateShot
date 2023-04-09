using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace TranslateShot
{
    internal class SeleniumControl
    {
        KeySend keySend = new KeySend();
        DirectoryControl directoryControl = new DirectoryControl();
        

        public void Init()
        {
            //https://teratail.com/questions/227899

            Console.WriteLine("ChromeDriverを取得中");

            //PCにインストールされているのと同じバージョン
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
        }

        public void OpenWebPage()
        {
            //ダウンロード先指定
            //https://gazee.net/develop/csharp-selenium-chrome-driver/
            ChromeOptions options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + directoryControl.folderPath);
            options.AddUserProfilePreference("download.prompt_for_download", "false");
            options.AddUserProfilePreference("download.directory_upgrade", "true");
            //options.AddArgument("--headless");

            //コンソール非表示
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            //driver.exeのパス指定
            var driverVersion = new ChromeConfig().GetMatchingBrowserVersion();
            var driverPath = $"./Chrome/{driverVersion}/X64/";

            using (var driver = new ChromeDriver(service, options))
            {
                //画面最大化
                driver.Manage().Window.Size = new System.Drawing.Size(1920,1080);

                //FindElement時、読み込まれるまで待機させる設定
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                //URL
                string url = "https://www.google.com";

                try
                {
                    driver.Navigate().GoToUrl(url);
                }
                catch
                {
                    MessageBox.Show("urlへのアクセスに失敗しました。");
                    return;
                }

                string imgButtonPath = "/html/body/div[1]/div[3]/form/div[1]/div[1]/div[1]/div/div[3]/div[3]";
                string isOpenedXPath = "/html/body/div[1]/div[3]/form/div[1]/div[1]/div[3]/c-wiz/div[2]/div/div[3]/div[2]/div/div[2]";
                string translateButtonPath = "/html/body/c-wiz/div/div[2]/div/c-wiz/div/div[1]/div/div[3]/div/div/span[3]/span/button/span[1]";
                string isTranslatedPath = "/html/body/c-wiz/div/div[2]/div/c-wiz/div/div[2]/div/div/div/div[1]/div/div[3]/div/div";

                driver.FindElement(By.XPath(imgButtonPath)).Click();

                driver.FindElement(By.XPath(isOpenedXPath)).Click();

                keySend.CtrlV();

                driver.FindElement(By.XPath(translateButtonPath)).Click();

                driver.FindElement(By.XPath(translateButtonPath)).Click();

                driver.FindElement(By.XPath(translateButtonPath)).Click();

                driver.FindElement(By.XPath(isTranslatedPath));

                Screenshot screenshot = (driver as ITakesScreenshot).GetScreenshot();

                string pngName = "result.png";
                screenshot.SaveAsFile(pngName, ScreenshotImageFormat.Png);

                /*
                using (Form dummyForm = new Form())
                {
                    dummyForm.TopMost = true;
                    MessageBox.Show(dummyForm, "Finished");
                    dummyForm.TopMost = false;
                }
                */

                System.Diagnostics.Process.Start(pngName);
            }

            
        }
    }
}
