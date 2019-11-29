using OpenQA.Selenium;
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
    public class V12Import_VRMAIN : V12Import
    {
        public V12Import_VRMAIN(string companyId)
            : base(companyId)
        {

         }

        private string exportWorkOrderAssignments(string type)
        {
            var startDate = DateTime.Now.AddDays(-3).ToShortDateString();
            var endDate = DateTime.Now.ToShortDateString();


            driver.Navigate().GoToUrl(buildUrl("/Web_Page/Reports/Report.aspx?reportName=WorkOrderAssignments"));

            //From Date
            var from = driver.FindElement(By.Id("ctl00_MainBody_par0YearStart_NoNewRow_txtDate"));
            from.Clear();
            from.SendKeys(startDate);

            //Through Date
            var through = driver.FindElement(By.Id("ctl00_MainBody_par0Today_NoNewRow_txtDate"));
            through.Clear();
            through.SendKeys(endDate);

            //Assignment Status
            SelectItemsByValue( driver.FindElement(By.Id("ctl00_MainBody_par0ChoiceListAssignmentStatus_NoNewRow")), type );
           
            //Complex
            driver.FindElement(By.Id("ctl00_MainBody_par0Complex_NoNewRow")).SendKeys("CI");

            //Assigned to Vendor
            driver.FindElement(By.Id("ctl00_MainBody_par0WOVendor")).SendKeys("-1");

            //WO Status
            //driver.FindElement(By.Id("ctl00_MainBody_par0ChoiceListProgress_NoNewRow")).SendKeys("Started");

            //Limit to Work Orders 
            driver.FindElement(By.Id("ctl00_MainBody_par0WODateFilter_NoNewRow")).SendKeys("Entered");


            //Report Format
           // driver.FindElement(By.Id("ctl00_MainBody_par0ChoiceListFormat")).SendKeys("Export");

            //click run button
            driver.FindElement(By.Id("ctl00_MainBody_submit")).Click();

            var fileName = importFolder + "Report.pdf";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            this.downFile("pdf");

           // var fileName = importFolder + "AddOnsSold-20190426-010258679.xlsx";
            int i = 0;
            while (!File.Exists(fileName) && i<10)
            {
                i++;
                Thread.Sleep(1000);
            }

            if (File.Exists(fileName))
            {
                var downloadFileName = "VRMAIN_" + Guid.NewGuid().ToString() + ".pdf";
                var newFileName = importFolder + downloadFileName;
                File.Move(fileName, newFileName);

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

        public override string exportFile(string type)
        {
            addFile(() => exportWorkOrderAssignments("Started"));
            addFile(() => exportWorkOrderAssignments("Not Started"));
           // addFile(() => exportHousekeepingServicesSchedule());
            

            String fileName = "Job_" + Guid.NewGuid().ToString() + ".zip";
            CompressFile(fileNameList, importFolder + fileName);
            //*[@id="StiWebViewer1_JsViewerMainPanel"]/div[2]/div/table/tbody/tr/td[1]/table/tbody/tr/td[3]/div


            return fileName;

        }

        public override void uploadFile(string fileName)
        {

            UploadJobs("T", fileName, 0, "VR-MAIN");
        }
    }
}
