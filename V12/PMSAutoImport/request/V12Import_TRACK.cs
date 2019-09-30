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
    public class V12Import_TRACK : V12Import_REQUEST_BASE
    {
        String url = "https://emily.stg-3.trackstaging.info";

        String UserName = "";
        String Password = "";

      
      
       
        public V12Import_TRACK(string companyId):base(companyId)
            
        {
            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            UserName = account.Split('|')[0];
            Password = account.Split('|')[1];

           
         }

        public V12Import_TRACK(string companyId, string importFolder)
            :this(companyId)        
        {
            this.importFolder = importFolder;
        }

      
        private string getToken()
        {

            string authorization = string.Format("{0}:{1}", UserName, Password);

            //var jsonObject = new
            //{
            //    Username = UserName,
            //    Password = Password,
            //    LocationID = 1

            //};

            //string json = JsonConvert.SerializeObject(jsonObject);

            //var postUrl = url + "/Authentication/AuthorizeUser";
            //var result = PostRequest(postUrl, json);

           // var fileName = string.Format("{0}property_{1}.txt", importFolder, Guid.NewGuid ().ToString ());
           // File.WriteAllText(fileName, result);

            return "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(authorization));
        
           
        }

        public void getUnits(string token)
        {
            var postUrl = url + "/api/pms/units/";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getReservations(string token)
        {
            var postUrl = url + "/api/pms/reservations/?updatedSince=2019-09-01";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        } 

        public Dictionary<string, string> buildHeader(string token)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("Authorization", token);

            return dict;
        }
        public override string exportFile()
        {
           var token = getToken();
           // getProperties(token);
           // getUnits(token);
           // getLeases(token);
           // getInspections(token);
           // getServiceManagerIssues(token);
           // getServiceManagerIssueWorkOrders(token);
           // getTasks(token);
           //getUnits(token);
           getReservations(token);
           // getVendors(token);
            //addFile(() => exportPropertyList());
            //addFile(() => exportHousekeepingCleaning());
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
