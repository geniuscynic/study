﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace PMSAutoImport
{
    public class V12Import_LINE : V12Import
    {
        public V12Import_LINE(string companyId)
            : base(companyId)
        {

         }

        private string exportAddOnsSold() {
            var startDate = "9/27/2017"; //new DateTime(DateTime.Today.Year, 1, 1).ToShortDateString().Replace("/", "%2F");
            var throughDate = DateTime.Today.AddDays(0).ToShortDateString();
            var staysFromDate = DateTime.Today.AddDays(3).ToShortDateString();
            var staysThroughDate = DateTime.Today.AddDays(4).ToShortDateString();
            //var throughDate = DateTime.Today.AddDays(1).ToShortDateString();
            //var staysFromDate = DateTime.Today.AddDays(4).ToShortDateString();
            //var staysThroughDate = DateTime.Today.AddDays(5).ToShortDateString();

            driver.Navigate().GoToUrl(buildUrl("/Web_Page/Reports/Report.aspx?reportName=AddOnsSold"));

            //From Date
            var from = driver.FindElement(By.Id("ctl00_MainBody_par0MonthStart_txtDate"));
            from.Clear();
            from.SendKeys(startDate);

            //Through Date
            var through = driver.FindElement(By.Id("ctl00_MainBody_par0Today_txtDate"));
            through.Clear();
            through.SendKeys(throughDate);

            //Stays From
            var staysFrom = driver.FindElement(By.Id("ctl00_MainBody_par0MonthStart2_txtDate"));
            staysFrom.Clear();
            staysFrom.SendKeys(staysFromDate);

            //Stays Through
            var staysThrouge = driver.FindElement(By.Id("ctl00_MainBody_par0FarFuture_txtDate"));
            staysThrouge.Clear();
            staysThrouge.SendKeys(staysThroughDate);

          
            //Report Format
            driver.FindElement(By.Id("ctl00_MainBody_par0ChoiceListFormat")).SendKeys("Export");

            //click run button
            driver.FindElement(By.Id("ctl00_MainBody_submit")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElement(By.Id("runForeground")));

            Thread.Sleep(1000);
            driver.FindElement(By.Id("runForeground")).Click();

            Thread.Sleep(10000);
           // var fileName = importFolder + "AddOnsSold-20190426-010258679.xlsx";
            var files = Directory.GetFiles(importFolder, "AddOnsSold-" + DateTime .Now .ToString("yyyyMMdd") + "-*");
            Console.WriteLine(importFolder);
            if (files.Count() > 0)
            {
                string newFileName = importFolder + "AddOnsSold_" + Guid.NewGuid().ToString() + ".xlsx";
                File.Move(files[0], newFileName);
                return newFileName;
            }

            return "";
            //if (File.Exists(fileName))
            //{
            //    var downloadFileName = "CFR_" + Guid.NewGuid().ToString() + ".xls";
            //    var newFileName = importFolder + downloadFileName;
            //    File.Move(fileName, newFileName);


            //    Console.WriteLine("===================Begin upload jobs==================");
            //    UploadJobs("E", downloadFileName, 0, companyId, "CFR");

            //    Console.WriteLine("===================End upload jobs==================");
            //}
        }

        private string exportHousekeepingServicesSchedule()
        {
            var startDate = DateTime.Today.AddDays(-2).ToShortDateString();  //new DateTime(DateTime.Today.Year, 1, 1).ToShortDateString().Replace("/", "%2F");
            var throughDate = DateTime.Today.AddDays(5).ToShortDateString();
            //var startDate = DateTime.Today.AddDays(0).ToShortDateString();  //new DateTime(DateTime.Today.Year, 1, 1).ToShortDateString().Replace("/", "%2F");
            //var throughDate = DateTime.Today.AddDays(6).ToShortDateString();

            driver.Navigate().GoToUrl(buildUrl("/Web_Page/Reports/Report.aspx?reportName=HousekeepingServicesSchedule"));

            //From Date
            var from = driver.FindElement(By.Id("ctl00_MainBody_par0Today_NoNewRow_txtDate"));
            from.Clear();
            from.SendKeys(startDate);

            //Through Date
            var through = driver.FindElement(By.Id("ctl00_MainBody_par0Today2_txtDate"));
            through.Clear();
            through.SendKeys(throughDate);

            //Service Type or blank for all
            //.se("B");
            SelectItemsByValue(driver.FindElement(By.Id("ctl00_MainBody_par0CleanCode_NoNewRow")), "B");


            //	Optional second Service Type	
            SelectItemsByValue(driver.FindElement(By.Id("ctl00_MainBody_par0CleanCode2_NoNewRow")), "L");

            //	Optional third Service Type	
            SelectItemsByValue(driver.FindElement(By.Id("ctl00_MainBody_par0CleanCode3")), "T");


            //Report Format
            driver.FindElement(By.Id("ctl00_MainBody_par0ChoiceListFormat")).SendKeys("Export");


            //click run button
            driver.FindElement(By.Id("ctl00_MainBody_submit")).Click();


            Thread.Sleep(10000);
            // var fileName = importFolder + "AddOnsSold-20190426-010258679.xlsx";
            var files = Directory.GetFiles(importFolder, "HousekeepingServicesSchedule-" + DateTime.Now.ToString("yyyyMMdd") + "-*");
            if (files.Count() > 0)
            {
                string newFileName = importFolder + "HousekeepingServicesSchedule_" + Guid.NewGuid().ToString() + ".xlsx";
                File.Move(files[0], newFileName);
                return newFileName;
            }

            return "";
        }
        public override string exportFile()
        {
            addFile(() => exportAddOnsSold());
            addFile(() => exportHousekeepingServicesSchedule());
            

            String fileName = "line_" + Guid.NewGuid().ToString() + ".zip";
            CompressFile(fileNameList, importFolder + fileName);
            //*[@id="StiWebViewer1_JsViewerMainPanel"]/div[2]/div/table/tbody/tr/td[1]/table/tbody/tr/td[3]/div

            return fileName;

        }

        public override void uploadFile(string fileName)
        {

            UploadJobs("T", fileName, 0, "Lin-Rep");
        }
    }
}