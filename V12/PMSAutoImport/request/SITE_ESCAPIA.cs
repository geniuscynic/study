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
    public class SITE_ESCAPIA : SITE_REQUEST_BASE
    {
        //String domain = "";

        String UserName = "";
        String Password = "";

      
      
       
        public SITE_ESCAPIA(string companyId):base(companyId)
            
        {
            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            UserName = account.Split('|')[0];
            Password = account.Split('|')[1];

            switch (companyId)
            {
                case "21144":
                    domain = "https://beachprosrealty.escapia.com";
                    break;
            }
         }

       
        public override void login()
        {
            string content = string.Format("UserName={0}&Password={1}&Persist=false", UserName, Password);

            // string content = string.Format("UserName={0}&Password={1}&Persist=false", account, password);
            // content = string.Format(content, viewstate, viewstategenerator, eventvalidation, companyCode, login, password);

            string responseString = PostRequest(buildUrl("/WebAgency/Account/LogOn?ReturnUrl=/webagency/"), content);

            var answer = "";
            if (responseString.Contains("What is the first name"))
            {
                answer = "Christine";
            }
            else if (responseString.Contains("What was the name of your first pet"))
            {
                answer = "Coco";
            }
            else
            {
                answer = "Beijing";

            }
            string requestVerificationToken = GetHtmlHiddenVaule(responseString, "__RequestVerificationToken");
            content = string.Format("__RequestVerificationToken={0}&Answer=" + answer, requestVerificationToken);
            responseString = PostRequest(buildUrl("/WebAgency/Account/SecurityQuestionChallenge?id=8831294&ReturnURL=/WebAgency/Default.aspx"), content);


            responseString = GetRequest(buildUrl("/WebAgency/Housekeeping/Home"));
        }


        private string exportMaintenance()
        {
            var responseString = GetRequest(buildUrl("/WebAgency/ServiceOrder/ManageServiceOrders.aspx"));

            var startDate = DateTime.Now.AddDays(-10).ToShortDateString().Replace("/", "%2F");
            var endDate = DateTime.Now.AddDays(0).ToShortDateString().Replace("/", "%2F");

            string viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            string viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            string eventvalidation = GetHtmlHiddenVaule(responseString, "__EVENTVALIDATION");
            string sid = GetHtmlHiddenVaule(responseString, "SID");
            string defaultBusinessEntityID = GetHtmlHiddenVaule(responseString, "slBillingEntity:defaultBusinessEntityID");
            string defaultBusinessEntity = GetHtmlHiddenVaule(responseString, "slBillingEntity:defaultBusinessEntity");


            string content = string.Format("SID={4}&__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}"+
                "&txtOrderNumber=&lstOrderPriority=255&lstSortBy=0&hdnSortDir=0&lstOrderStatus=0"+
                "&lstOrderStatus=1&lstOrderStatus=2&lstProcessedStatus=255&lstTechnicianType=255"+
                "&lstSortBy2=-1&hdnSortDir2=0&slBillingEntity%3AdefaultBusinessEntityID={5}"+
                "&slBillingEntity%3AdefaultBusinessEntity={6}"+
                "&slBillingEntity%3Atype=255&slBillingEntity=0&slBillingEntity%3AentityName=&txtMaxRows=500"+
                "&lstDateSearchMethod=0&dpStartDate={2}&dpEndDate={3}&btnExport="
                , viewstate, viewstategenerator, startDate, startDate, sid, defaultBusinessEntityID, defaultBusinessEntity);
            
            responseString = PostRequest(buildUrl("/WebAgency/ServiceOrder/ManageServiceOrders.aspx"), content);


            var fileName = string.Format("{0}Maintenance_{1}.txt", importFolder, Guid.NewGuid().ToString());
            File.WriteAllText(fileName, responseString);

            return fileName;
        }
      
        public override string exportFile()
        {

            addFile(() => exportMaintenance());
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
