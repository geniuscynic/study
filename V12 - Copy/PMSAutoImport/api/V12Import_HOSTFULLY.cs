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
    public class V12Import_HOSTFULLY : V12Import_REQUEST_BASE
    {
        String url = "https://sandbox.hostfully.com/v1";

        String UserName = "";
        String Password = "";




        public V12Import_HOSTFULLY(string companyId, string account, string password, string parse)
            : base(companyId, account, password, parse)
            
        {
            //var accountKey = companyId + "Account";
            //string account = ConfigurationManager.AppSettings[accountKey];
            //UserName = account.Split('|')[0]; 
            //Password = account.Split('|')[1]; 
            UserName = account;
            Password = password;

           
         }

        //public V12Import_HOSTFULLY(string companyId, string importFolder)
        //    :this(companyId)        
        //{
        //    this.importFolder = importFolder;
        //}

      
        private string getToken()
        {

            //string authorization = string.Format("{0}:{1}", UserName, Password);

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

            return UserName;
        
           
        }

        public void getAgencies(string token)
        {
            var postUrl = url + "/agencies/";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getProperties(string token)
        {
            var postUrl = url + "/properties?agencyUid=" + Password ;
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
            dict.Add("X-ORBIRENTAL-APIKEY", token);

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
           getProperties(token);
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
