using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Threading;

namespace PMSAutoImport
{
    public class V12Import_STREAMLINE : V12Import_REQUEST_BASE
    {
        String url = "https://web.streamlinevrs.com/api/json";

        String TokenKey = "";
        String TokenSecretKey = "";

      
      
       
        public V12Import_STREAMLINE(string companyId):base(companyId)
            
        {
            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            TokenKey = account.Split('|')[0];
            TokenSecretKey = account.Split('|')[1];

           
         }

        public V12Import_STREAMLINE(string companyId, string importFolder)
            :this(companyId)        
        {
            this.importFolder = importFolder;
        }


        private string RenewExpiredToken()
        {
            var jsonObject = new
            {
                methodName = "RenewExpiredToken",
                @params = new
                {
                    token_key = TokenKey,
                    token_secret = TokenSecretKey
                }

            };

            string json = JsonConvert.SerializeObject(jsonObject);

            var result = PostRequest(url, json);

            return result;
        }

        private string exportPropertyList()
        {
            var jsonObject = new
            {
                methodName = "GetPropertyList",
                @params = new
                {
                    token_key = TokenKey,
                    token_secret = TokenSecretKey
                }

            };

            string json = JsonConvert.SerializeObject(jsonObject);

            var result = PostRequest(url, json);

            var fileName = string.Format("{0}property_{1}.txt", importFolder, Guid.NewGuid ().ToString ());
            File.WriteAllText(fileName, result);

            return fileName;
           
        }

        private string exportHousekeepingCleaning()
        {
            var startDate = DateTime.Now .ToShortDateString();
            var endDate = DateTime.Now.AddDays(7).ToShortDateString();

            if (companyId == "21388")
            {
                endDate = DateTime.Now.AddDays(14).ToShortDateString();
            }

            var jsonObject = new
            {
                methodName = "GetHousekeepingCleaningReport",
                @params = new
                {
                    token_key = TokenKey,
                    token_secret = TokenSecretKey,
                    report_startdate = startDate,
                    report_enddate = endDate

                }

            };

            string json = JsonConvert.SerializeObject(jsonObject);

            var result = PostRequest(url, json);

            var fileName = string.Format("{0}housekeepingCleaningReport_{1}.txt", importFolder, Guid.NewGuid().ToString());
            File.WriteAllText(fileName, result);

            return fileName;

        }

       
        public override string exportFile()
        {
            //RenewExpiredToken();
            addFile(() => exportPropertyList());
            addFile(() => exportHousekeepingCleaning());
           // addFile(() => exportHousekeepingServicesSchedule());
            

            String fileName = "Job_" + Guid.NewGuid().ToString() + ".zip";
            CompressFile(fileNameList, importFolder + fileName);

            return fileName;
            //*[@id="StiWebViewer1_JsViewerMainPanel"]/div[2]/div/table/tbody/tr/td[1]/table/tbody/tr/td[3]/div
        }

        public override void uploadFile(string fileName)
        {

            UploadJobs("T", fileName, 0, "StreamlineImport");
        }
        

    }
}
