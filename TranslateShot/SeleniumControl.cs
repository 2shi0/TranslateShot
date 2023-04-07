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

            // 最新版
            //new DriverManager().SetUpDriver(new ChromeConfig());

            // インストールされているバージョン
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

            var driverVersion = new ChromeConfig().GetMatchingBrowserVersion();
            var driverPath = $"./Chrome/{driverVersion}/X64/";

            using (var driver = new ChromeDriver(driverPath, options))
            {
                //FindElement時、読み込まれるまで待機させる設定
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

                //URL
                string url = "https://translate.yandex.com/ocr?";

                //押すボタンのXPATH
                string russian = "/html/body/div[1]/main/div[1]/div[1]/div[2]/div/button[3]";
                string japanese = "/html/body/div[1]/main/div[1]/div[1]/div[4]/div/div[2]/div/div[43]";
                string download = "/html/body/div[1]/main/div[1]/div[1]/div[3]/button[2]";

                // URLへ遷移
                try
                {
                    driver.Navigate().GoToUrl(url);
                    while (driver.Url != url) ;
                }
                catch
                {
                    return;
                }
                
                /*
                //読み込まれるまで待機（旧）
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                wait.Until(drv => drv.FindElement(By.XPath(russian)));

                
                var element = driver.FindElement(By.XPath(russian));
                Actions actions = new Actions(driver);
                actions.MoveToElement(element);
                actions.Perform();
                */

                driver.FindElement(By.XPath(russian)).Click();
                driver.FindElement(By.XPath(japanese)).Click();

                keySend.CtrlV();

                driver.FindElement(By.XPath(download)).Click();

                MessageBox.Show("Finished");
            }
        }
    }
}
