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
    public class V12Import_RENT : V12Import_REQUEST_BASE
    {
        String url = "https://harmoni.api.rentmanager.com";

        String UserName = "";
        String Password = "";




        public V12Import_RENT(string companyId, string account, string password, string parse)
            : base(companyId, account, password, parse)
            
        {
            //var accountKey = companyId + "Account";
            //string account = ConfigurationManager.AppSettings[accountKey];
            UserName = account;
            Password = password;

           
         }

        public V12Import_RENT(string companyId, string account, string password, string parse, string importFolder)
            : this(companyId, account, password, parse)        
        {
            this.importFolder = importFolder;
        }

      
        private string getToken()
        {
            var jsonObject = new
            {
                Username = UserName,
                Password = Password,
                LocationID = 1

            };

            string json = JsonConvert.SerializeObject(jsonObject);

            var postUrl = url + "/Authentication/AuthorizeUser";
            var result = PostRequest(postUrl, json);

           // var fileName = string.Format("{0}property_{1}.txt", importFolder, Guid.NewGuid ().ToString ());
           // File.WriteAllText(fileName, result);

            return result.Trim('"');
           
        }


        public void getTenants(string token)
        {
            var postUrl = url + "/tenants";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getProperties(string token)
        {
            var postUrl = url + "/properties";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getUnits(string token)
        {
            var postUrl = url + "/units";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getLeases(string token)
        {
            var postUrl = url + "/Leases";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getLeasesFilter(string token)
        {
            var postUrl = url + "/Leases?filters=MoveOutDate,bt,(2012-9-12,2019-9-18);MoveInDate,gt,(2015-01-01)";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getMoveInMoveOutReort(string token)
        {
            var postUrl = url + "/Reports/77/RunReport?parameters=StartDate,2015-9-12;EndDate,2019-9-18";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getInspections(string token)
        {
            var postUrl = url + "/Inspections";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getServiceManagerIssues(string token)
        {
            var postUrl = url + "/ServiceManagerIssues";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void saveServiceManagerIssues(string token)
        {
            var postUrl = url + "/ServiceManagerIssues";
            var content = @"[
              {
                
                'Title': 'sample string 4',
                'Description': 'test desc4',
                'Resolution': 'test res4',
                'AssignedOpenDate': '2019-09-09T12:00:39',
                'CustomerDescription': 'test custom',
                'NoteText': 'testc note',
                'StatusID': '1'

              }
            ]";

            var result = PostRequest(postUrl, buildHeader(token), content);

            var a = "";

         }

        public void saveServiceManagerIssues2(string token)
        {
            var postUrl = url + "/ServiceManagerIssues?fields=Issue,AssignedCloseDate,IsClosed,Resolution,ServiceManagerIssueID,StatusID";
            var content = @"[
              {
                'ServiceManagerIssueID': 1,
                'Issue': 'sample string 2',
                'Resolution': 'test res2',
                'StatusID': '4',
                'IsClosed': true,
                'AssignedCloseDate': '2019-09-13'

              }
            ]";

            var result = PostRequest(postUrl, buildHeader(token), content);

            var a = "";

        }

        public void getServiceManagerIssueWorkOrders(string token)
        {
            var postUrl = url + "/ServiceManagerIssueWorkOrders";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getLeaseRenewals(string token)
        {
            var postUrl = url + "/LeaseRenewals";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getTasks(string token)
        {
            var postUrl = url + "/Tasks";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getUsers(string token)
        {
            var postUrl = url + "/Users";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getVendors(string token)
        {
            var postUrl = url + "/Vendors";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

        public void getRecurringCharges(string token)
        {
            var postUrl = url + "/RecurringCharges";
            var result = GetRequest(postUrl, buildHeader(token));

            var a = "";
        }

       
        public Dictionary<string, string> buildHeader(string token)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("X-RM12Api-ApiToken", token);

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
            saveServiceManagerIssues(token);
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
