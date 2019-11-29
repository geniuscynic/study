
using ICSharpCode.SharpZipLib.Zip;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PMSAutoImport.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace PMSAutoImport
{
    public abstract class V12Import
    {
        protected IWebDriver driver;
        protected string companyId;
        protected string importFolder;
        protected string domain = "https://v12.instantsoftware.com/";
        protected string dynamicUrl = "";
        protected List<string> fileNameList = new List<string>();

        public V12Import()
        {

        }

        public V12Import(string companyId)
        {

            this.companyId = companyId;

            importFolder = ConfigurationManager.AppSettings[companyId + "importFolder"];

            if (!Directory.Exists(importFolder))
            {
                //File.AppendAllText("Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\nCan't find folder");

                Directory.CreateDirectory(importFolder);
            }

            ChromeOptions options = new ChromeOptions();
            options.AddUserProfilePreference("profile.default_content_settings.popups", 0);
            options.AddUserProfilePreference("download.default_directory", importFolder);


            driver = new ChromeDriver("./", options, TimeSpan.FromSeconds(180));


            //OpenQA.Selenium.Support.UI.
            //SelectElement s=new SelectElement
        }

        public virtual void login()
        {
           

            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            string companyCode = account.Split(',')[0]; // "0637";//importInfo.ClientSiteLogin.Split('-')[0];
            string login = account.Split(',')[1].Split('|')[0];//"EZinspections";//importInfo.ClientSiteLogin.Split('-')[1];
            string password = account.Split('|')[1];//importInfo.ClientSitePassword.Replace("@", "%40");


            driver.Navigate().GoToUrl(buildUrl("V12login/V12Login.aspx"));
            driver.FindElement(By.Id("txtCompanyCode")).SendKeys(companyCode);
            driver.FindElement(By.Id("txtLogin")).SendKeys(login);
            driver.FindElement(By.Id("txtPassword")).SendKeys(password);
            driver.FindElement(By.Name("SubmitButton")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.Id("ctl00_lblCOID")));

            dynamicUrl = GetDynamicUrl(driver.Url);
        }


        
        public abstract string exportFile(string type);

       

        public abstract void uploadFile(string fileName);

        public void exportAndUpload(string type)
        {
            var fileName = exportFile(type);
            if (fileName != "")
            {
                uploadFile(fileName);
            }
        }

        protected string addFile(Func<string> func)
        {
            Console.WriteLine("Begin download file");
            var file = func();
            if (file != "")
            {
                fileNameList.Add(file);
            }
            Console.WriteLine("End download file " + file);

            return file;
        }

        protected List<string> addFile(Func<List<string>> func)
        {
            Console.WriteLine("Begin download file");
            var files = func();
            if (files != null)
            {
                fileNameList.AddRange(files);
            }
            Console.WriteLine("End download file " + files.Count());

            return files;
        }

        //protected string addFile(Func<string, string> func, string arg1)
        //{
        //    Console.WriteLine("Begin download file");
        //    var file = func(arg1);
        //    if (file != "")
        //    {
        //        fileNameList.Add(file);
        //    }
        //    Console.WriteLine("End download file");

        //    return file;
        //}

        protected void UploadJobs(string fileType, string fileName, int toBeImported)
        {
            UploadJobs(fileType, fileName, toBeImported, "HK");

        }
        protected void UploadJobs(string fileType, string fileName, int toBeImported, string productCode)
        {
            Console.WriteLine("===================Begin upload jobs==================");

            int succeeded = 0;
            int duplicated = 0;
            int failed = 0;
            ezUpdaterServicesSoapClient client = new ezUpdaterServicesSoapClient();

            string importationResult = client.ImportFiles(0, int.Parse(ConfigurationManager.AppSettings[companyId + "CompanyId"]), int.Parse(ConfigurationManager.AppSettings[companyId + "OwnerCoId"]), int.Parse(ConfigurationManager.AppSettings[companyId + "FormCoid"]), productCode, fileType, fileName, true, false, 0);
            XmlDocument importationResultXml = new XmlDocument();
            importationResultXml.LoadXml(importationResult);
            string status = importationResultXml.SelectSingleNode("/ImportLogDataSet/ImportLog/Status").InnerText;
            int.TryParse(importationResultXml.SelectSingleNode("/ImportLogDataSet/ImportLog/Succeeded").InnerText, out succeeded);
            int.TryParse(importationResultXml.SelectSingleNode("/ImportLogDataSet/ImportLog/Duplicated").InnerText, out duplicated);
            int.TryParse(importationResultXml.SelectSingleNode("/ImportLogDataSet/ImportLog/Failed").InnerText, out failed);


            File.AppendAllText(DateTime.Now.ToString("yyyyMMdd") + ".txt", importationResult);

            Console.WriteLine("===================End upload jobs==================");

            driver.Quit();
            //driver.Close();


            //client.UpdateJobProgress(importInfo.CompanyId, importInfo.FormCoId, importInfo.ProductCode, importInfo.OwnerCoId, CommonUtil.GetPublicIP(), 1);
        }

        protected string buildUrl(string page)
        {
            return domain + dynamicUrl + page;
        }
        private string GetDynamicUrl(string responseString)
        {

            var value = Regex.Match(responseString, domain + "(.*?)/.*");

            return value.Groups[1].Value;

        }

        protected void CompressFile(List<string> fileNames, string zipFileName)
        {
            using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFileName)))
            {
                foreach (string fileName in fileNames)
                {
                    using (FileStream fs = File.OpenRead(fileName))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        fs.Close();

                        string newfileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(newfileName);
                        s.PutNextEntry(entry);
                        s.SetLevel(1);

                        s.Write(buffer, 0, buffer.Length);
                    }
                }

                s.Finish();
                s.Close();
            }
        }

        protected void downFile(string exportType)
        {
            //Thread.Sleep(5000);

            //wait load ok
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(180));
            wait.Until(d => d.FindElement(By.ClassName("stiJsViewerPage")));

            Thread.Sleep(1000);
            //save
            var elemEments = driver.FindElements(By.CssSelector(".stiJsViewerToolBar .stiJsViewerStandartSmallButton.stiJsViewerStandartSmallButtonDefault"));
            foreach (IWebElement ele in elemEments)
            {
                //#StiWebViewer1_JsViewerMainPanel > div:nth-child(4) > div > table > tbody > tr > td:nth-child(1) > table > tbody > tr > td:nth-child(3) > div > table > tbody > tr > td:nth-child(2)
                try
                {
                    var saveElement = ele.FindElement(By.CssSelector("td.stiJsViewerClearAllStyles:nth-child(2)"));
                    if (saveElement != null && saveElement.Text.ToLower() == "save")
                    {
                        ele.Click();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    //ele.
                }
            }

            Thread.Sleep(1000);
            //pdf
            elemEments = driver.FindElements(By.CssSelector("div.stiJsViewerMenu .stiJsViewerMenuStandartItem"));
            foreach (IWebElement ele in elemEments)
            {
                try
                {
                    var saveElement = ele.FindElement(By.CssSelector("td.stiJsViewerClearAllStyles:nth-child(2)"));
                    if (saveElement != null && saveElement.Text.ToLower().Contains(exportType))
                    {
                        ele.Click();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    //ele.
                }
            }

            Thread.Sleep(1000);
            //erport
            elemEments = driver.FindElements(By.CssSelector("div.stiJsViewerForm div.stiJsViewerFormButtonsPanel .stiJsViewerFormButton.stiJsViewerFormButtonDefault"));
            foreach (IWebElement ele in elemEments)
            {
                try
                {
                    var saveElement = ele.FindElement(By.CssSelector("td.stiJsViewerClearAllStyles"));
                    if (saveElement != null && saveElement.Text.ToLower().Contains("ok"))
                    {
                        ele.Click();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    //ele.
                }
            }

            Thread.Sleep(5000);
        }


        protected void SelectItemsByValue(IWebElement element, string value)
        {
            var selectElement = new SelectElement(element);
            selectElement.SelectByValue(value);
        }


        #region request
       
        #endregion
    }
}
