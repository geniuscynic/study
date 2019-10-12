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

        private string exportWorkOrders()
        {
            

            var jsonObject = new
            {
                methodName = "GetWorkOrders",
                @params = new
                {
                    token_key = TokenKey,
                    token_secret = TokenSecretKey,

                    unit_id = 154138,
                    maintenance_worker_id = 7016659,
                    show_notes = 1

                }

            };

            string json = JsonConvert.SerializeObject(jsonObject);

            var result = PostRequest(url, json);

            var fileName = string.Format("{0}workorder_{1}.txt", importFolder, Guid.NewGuid().ToString());
            File.WriteAllText(fileName, result);

            return fileName;

        }

        private string exportMaintenanceStatuses()
        {


            var jsonObject = new
            {
                methodName = "GetMaintenanceStatuses",
                @params = new
                {
                    token_key = TokenKey,
                    token_secret = TokenSecretKey

                }

            };

            string json = JsonConvert.SerializeObject(jsonObject);

            var result = PostRequest(url, json);

            var fileName = string.Format("{0}workorder_{1}.txt", importFolder, Guid.NewGuid().ToString());
            File.WriteAllText(fileName, result);

            return fileName;

        }

        private string UpdateWorkOrderDescription()
        {


            var jsonObject = new
            {
                methodName = "UpdateWorkOrderDescription",
                @params = new
                {
                    token_key = TokenKey,
                    token_secret = TokenSecretKey,
                    processor_id = 1,
                    work_order_id = 7016659,
                    description = "Update Description Example 2"

                }

            };

            string json = JsonConvert.SerializeObject(jsonObject);

            var result = PostRequest(url, json);

            var fileName = string.Format("{0}workorder_{1}.txt", importFolder, Guid.NewGuid().ToString());
            File.WriteAllText(fileName, result);

            return fileName;

        }

        private string AddHKNoteToWorkOrder()
        {


            var jsonObject = new
            {
                methodName = "AddHKNoteToWorkOrder",
                @params = new
                {
                    token_key = TokenKey,
                    token_secret = TokenSecretKey,
                    processor_id = 1,
                    work_order_id = 7016659,
                    message = "test AddHKNoteToWorkOrder3"

                }

            };

            string json = JsonConvert.SerializeObject(jsonObject);

            var result = PostRequest(url, json);

            var fileName = string.Format("{0}workorder_{1}.txt", importFolder, Guid.NewGuid().ToString());
            File.WriteAllText(fileName, result);

            return fileName;

        }

        private string AddPhotoToWorkOrder()
        {


            var jsonObject = new
            {
                methodName = "AddPhotoToWorkOrder",
                @params = new
                {
                    token_key = TokenKey,
                    token_secret = TokenSecretKey,
                    processor_id = 1,
                    work_order_id = 7016659,
                    photo_name = "test.jpg",
                    photo_description= "dd",
                    photo_data = "iVBORw0KGgoAAAANSUhEUgAAAFAAAABQCAYAAACOEfKtAAACpklEQVR42u3bPVIDMQwF4BwhJSVXoKTkChyBMi0lJVehpEzLVVJScgUTJyvPW621P2GwvOu3MxogCWHyIXllbbILIewYtwcRCEhAAhKQQUACEpCADAISkIAEZBCQgAQkIIOABCRgLg5PdwGDgAvgji/3Ibw/9iLetmbIYnin14cBHsYtiDVkcjHAMTzJxL9ksxdkUTyrjJdk4dhzxCwvjVg8++LP+/3eBIz36ZjKZr1ElET89z+AWAiEEDGjuhLuHT+fz5forXUZPPl9ycxNAWLWaEB50RFp7LjcLzBGxspzxUwsWcpFAPEFyosWDI0nWJJ96ZhR8oioS3/1faBGHMPDSI9ZABhjc4CCOCf7lgRmdGk8t63cILsMRCv75DliqWLWNQeYO/OOgWLorMP1dbPDBN1UxyN+tTLRal3wH2D1i5sH1O2MnHUlJDt1z5gDbGacpTNQzpzh+yMcDm9XjPP3MTC7CGjtYzssK1IJy20KrzlAREyN8gQihpR2LXgugFK2es2TLZgFh2tj04A5RJjlXde883qogTVgDXiuF5X0nnjQ3mQQa8NzBUxnYGN7ZpVsTXjuGWg1yLnpjWRrbReg3PBwAKr7Ohyg5va6NUG6lm+uPHNgOezSk+eqATHblo6zvIYI1ZxErOzKja9w4txDPH61Bzj3erF18QghPc/MrnhT71bIXfMdlP+5X5QGvDnAuWuenjxDP9jWPNBqZaxMtEb2vat71wxs8/2BVp83J6SUvVuZ1QN6I7p38lPbtim8ZgHj2iUzQL3WCUgOU8M2N5FOeKcTvoclISKIWeJd49z0QBVbkTGQNPbqHndBddx5VLUGzu3haur7Vv0xBw5U+TkRBgEJSEACMghIQAISkEFAAhKQgAQkAgEJSEACMghIwFXGL4pi66jNesPAAAAAAElFTkSuQmCC"
                 }

            };

            string json = JsonConvert.SerializeObject(jsonObject);

            var result = PostRequest(url, json);

            var fileName = string.Format("{0}workorder_{1}.txt", importFolder, Guid.NewGuid().ToString());
            File.WriteAllText(fileName, result);

            return fileName;

        }

        public override string exportFile()
        {
            //AddPhotoToWorkOrder();
            //AddHKNoteToWorkOrder();
            //exportMaintenanceStatuses();
            exportWorkOrders();
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
