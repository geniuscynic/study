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
    public class V12Import_CFR : V12Import
    {
        public V12Import_CFR(string companyId)
            : base(companyId)
        {

        }

        private string exportArrivalsByUnit()
        {
            var startDate = DateTime.Now.AddDays(-29).ToShortDateString();
            var endDate = DateTime.Now.AddDays(7).ToShortDateString();

            driver.Navigate().GoToUrl(buildUrl("/Web_Page/Reports/Report.aspx?reportName=ArrivalsByUnit"));

            //Folios From
            var from = driver.FindElement(By.Id("ctl00_MainBody_par0TodayFromDate_NoNewRow_txtDate"));
            from.Clear();
            from.SendKeys(startDate);

            //Folios Through
            var through = driver.FindElement(By.Id("ctl00_MainBody_par0TodayThruDate_NoNewRow_txtDate"));
            through.Clear();
            through.SendKeys(endDate);

            //Folio Type
            driver.FindElement(By.Id("ctl00_MainBody_par0ChoiceListFolioType_NoNewRow")).SendKeys("Regular");

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

           
            if (File.Exists(fileName))
            {
                var downloadFileName = "CFR_" + Guid.NewGuid().ToString() + ".xls";
                var newFileName = importFolder + downloadFileName;
                File.Move(fileName, newFileName);

                return newFileName;
            }

            return "";
        }
        public override string exportFile()
        {
            var file = addFile(() => exportArrivalsByUnit());

            return file;
            //*[@id="StiWebViewer1_JsViewerMainPanel"]/div[2]/div/table/tbody/tr/td[1]/table/tbody/tr/td[3]/div
        }

        public override void uploadFile(string fileName)
        {

            var fileInfo = new FileInfo(fileName);

                UploadJobs("E", fileInfo.Name, 0, "CFR");

            
        }
    }
}
