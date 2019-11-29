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
    public class V12Import_HK : V12Import
    {
        public V12Import_HK(string companyId)
            : base(companyId)
        {

         }

        private string exportArrivals()
        {
            var startDate = DateTime.Now.AddDays(0).ToShortDateString();
            var endDate = DateTime.Now.AddDays(7).ToShortDateString();


            driver.Navigate().GoToUrl(buildUrl("/Web_Page/Reports/Report.aspx?reportName=Arrivals"));

            //From Date
            var from = driver.FindElement(By.Id("ctl00_MainBody_par0TodayFromDate_NoNewRow_txtDate"));
            from.Clear();
            from.SendKeys(startDate);

            //Through Date
            var through = driver.FindElement(By.Id("ctl00_MainBody_par0TodayThruDate_NoNewRow_txtDate"));
            through.Clear();
            through.SendKeys(endDate);

           
            //Complex
            driver.FindElement(By.Id("ctl00_MainBody_par0ComplexID")).SendKeys("CI");

            //Report Format
            driver.FindElement(By.Id("ctl00_MainBody_par0ChoiceListFormat_NoNewRow")).SendKeys("Export");

            //click run button
            driver.FindElement(By.Id("ctl00_MainBody_submit")).Click();

            var fileName = importFolder + "Report.xls";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            this.downFile("excel");

           // var fileName = importFolder + "AddOnsSold-20190426-010258679.xlsx";
            
            if (File.Exists(fileName))
            {
                var downloadFileName = "Arrivals_" + Guid.NewGuid().ToString() + ".xls";
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
            addFile(() => exportArrivals());

            tmp_HKImport hk = new tmp_HKImport();
            hk.ImportJob(companyId, fileNameList);
           // addFile(() => exportHousekeepingServicesSchedule());


            String fileName = Guid.NewGuid().ToString() + ".zip";
            if (companyId == "15418" || companyId == "18980" || companyId == "18935")
            {
                //if (DateTime.Now.Hour == 7)
                //{
                //    //fileName = "PM12_" + fileName;
                //    fileName = "AM9_" + fileName;
                //}
                //else
                //{
                fileName = "PM12_" + fileName;
                // fileName = "AM9_" + fileName;
                //}
            }
            else
            {
                if (DateTime.Now.Hour == 5)
                {
                    //fileName = "PM12_" + fileName;
                    fileName = "AM9_" + fileName;
                }
                else
                {
                    fileName = "PM12_" + fileName;
                    // fileName = "AM9_" + fileName;
                }
            }
           // CompressFile(fileNameList, importFolder + fileName);

            

            //String fileName = "Job_" + Guid.NewGuid().ToString() + ".zip";
            CompressFile(fileNameList, importFolder + fileName);
            //*[@id="StiWebViewer1_JsViewerMainPanel"]/div[2]/div/table/tbody/tr/td[1]/table/tbody/tr/td[3]/div

            return fileName;
            //UploadJobs("T", fileName, 0, "HK");

        }

        public override void uploadFile(string fileName)
        {

            UploadJobs("T", fileName, 0, "HK");
        }
    }
}
