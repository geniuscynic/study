using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PMSAutoImport.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace PMSAutoImport
{
    public class SITE_HK : V12Import_HK_REQUEST
    {
        
        //string importFolder = ConfigurationManager.AppSettings["importFolder"];
        private string dynamicUrl = "";
        private string domain = "https://v12.instantsoftware.com/";
        protected CookieContainer myCookieContainer = new CookieContainer();

        private string startDate = DateTime.Now.AddDays(0).ToShortDateString().Replace("/", "%2F");
        private string endDate = DateTime.Now.AddDays(6).ToShortDateString().Replace("/", "%2F");

        public SITE_HK(string companyId, string account, string password, string parse)
            : base(companyId, account, password, parse)
        {
           
        }


        public string exportAssignHouseKeeper()
        {
           

            var responseString = GetRequest("/Web_Page/Housekeeping/AssignHousekeepingTask.aspx");


            var content = "__EVENTTARGET=ctl00%24MainBody%24btnExport&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24knockoutViewModel%24viewStateStorage=%7B%7D&Date={2}&DateThrough={3}&RevenueGroupID=&HousekeepingSizeID=&HousekeepingGroupID=&CleaningTypeCode=&CleanStatusID=&UnitName=&HousekeeperType=&AssignedToID=";

            var viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            var viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            content = string.Format(content, viewstate, viewstategenerator, startDate, endDate);

            responseString = PostRequest("/Web_Page/Housekeeping/AssignHousekeepingTask.aspx", content);


            Match m = Regex.Match(responseString, "open\\('.*?\\'");
            var downloadUrl = m.ToString().Replace("open(", "").Replace("'", "");

            //File.AppendAllText("Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\ndownloadUrl:" + downloadUrl);


            string excelPath = importFolder + Guid.NewGuid().ToString() + ".xls";
            responseString = DownFile(downloadUrl, excelPath);
           // fileNameList.Add(excelPath);
            Console.WriteLine("down file 1 ");

            return excelPath;
        }

        public string exportHousekeepingDashboard()
        {
            var responseString = GetRequest("/Web_Page/Housekeeping/HousekeepingDashboard.aspx");
            //viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            //viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            var pattern = "xpid:\"(.*?)\"";
            var match = Regex.Match(responseString, pattern);

            var xid = match.Groups[1].Value;

            pattern = "coid.*?'(.*?)'.*?userid.*?'(.*?)'";
            match = Regex.Match(responseString, pattern);

            var coid = match.Groups[1].Value;
            var uid = match.Groups[2].Value;

            startDate = startDate.Replace("%2F", "/");
            endDate = endDate.Replace("%2F", "/");
            var content = "{\"Date\":\"" + startDate + "\",\"DateThrough\":\"" + endDate + "\",\"UnitName\":\"\",\"HasOpenWorkOrders\":false,\"ShowUnitsWithoutFolios\":false}";
            // content = string.Format(content, startDate, endDate);

            responseString = PostJson("/Api/HousekeepingDashboard", content, xid, uid, coid);
            var excelPath = importFolder + "FolioNumber_" + Guid.NewGuid().ToString() + ".txt";
            File.WriteAllText(excelPath, responseString);
            fileNameList.Add(excelPath);

            excelPath = importFolder + Guid.NewGuid().ToString() + ".txt";
            File.WriteAllText(excelPath, responseString);

            return excelPath;
            
        }

        public string exportFolioNotes()
        {
            String pdfPath = "";

            var responseString = GetRequest("/Web_Page/Reports/Report.aspx?reportName=FolioNotes");

            var viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            var viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            startDate = DateTime.Now.AddDays(-14).ToShortDateString().Replace("/", "%2F");
            endDate = DateTime.Now.ToShortDateString().Replace("/", "%2F");

            var content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24par0ChoiceListRangeType=Notes+Created&ctl00%24MainBody%24par0Today0%24txtDate={2}&ctl00%24MainBody%24par0Today%24txtDate={3}&ctl00%24MainBody%24par0FolioNoteType=H&ctl00%24MainBody%24par0UserCode=0&ctl00%24MainBody%24par0RevenueGroup=0&ctl00%24MainBody%24par0Complex=&ctl00%24MainBody%24par0Units=&ctl00%24MainBody%24parFolio=0&ctl00%24MainBody%24parPhrase=&ctl00%24MainBody%24par0ChoiceListSort=Folio&ctl00%24MainBody%24par0ChoiceListFormat=Print&ctl00%24MainBody%24parReportGrouping=&ctl00%24MainBody%24par0UnitFeature=&ctl00%24MainBody%24par0YesNoCancelled=N&ctl00%24MainBody%24submit=Run";
            content = string.Format(content, viewstate, viewstategenerator, startDate, endDate);

            if (companyId == "15418" || companyId == "18980" || companyId == "20452")
            {
                pdfPath = importFolder + "FolioNotes_" + Guid.NewGuid() + ".xls";

                responseString = DownFile("/Web_Page/Reports/Report.aspx?reportName=FolioNotes", content, pdfPath);

                //responseString = DownFile(downloadUrl, pdfPath);
                //fileNameList.Add(pdfPath);
            }
            else if (companyId != "21304")
            {
                responseString = PostRequest("/Web_Page/Reports/Report.aspx?reportName=FolioNotes", new Dictionary<string, string>(), content, true);

                // <iframe src=domain +"/CustomerData/0637/ReportsOutput/FolioNotes-20170527-022657528.pdf" id="ctl00_MainBody_FramedPage" width="100%" frameborder="0" style="height: 100%;"></iframe>
                var m = Regex.Match(responseString, "<iframe src=\".*?\"");

                var downloadUrl = m.ToString().Replace("\"", "").Replace("<iframe src=", "");
                pdfPath = importFolder + "FolioNotes_" + Guid.NewGuid() + ".pdf";
                responseString = DownFile(downloadUrl, pdfPath);
                //fileNameList.Add(pdfPath);
            }
            Console.WriteLine("down file 2 ");

            return pdfPath;
        }

        public void startExportUnitFeature()
        {

            var responseString = GetRequest("/Web_Page/Property%20Management/UnitSearch.aspx");


            var content = "__EVENTTARGET=ctl00%24TopHeaderBar%24btnSearch&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24captFindFirst=5000&ctl00%24MainBody%24tbunit_name=&ctl00%24MainBody%24captRateUnTyp=0&ctl00%24MainBody%24txtAddress1=&ctl00%24MainBody%24txtAddress2=&ctl00%24MainBody%24county=&ctl00%24MainBody%24city=&ctl00%24MainBody%24state=&ctl00%24MainBody%24zip_code=&ctl00%24MainBody%24captStatus=Active&ctl00%24MainBody%24captBeedroms=&ctl00%24MainBody%24captBathroms=&ctl00%24MainBody%24drdComplex=&ctl00%24MainBody%24captRating=&ctl00%24MainBody%24captSmokingAllowed=&ctl00%24MainBody%24captPetsAllowed=&ctl00%24MainBody%24drOwnerName=0&ctl00%24MainBody%24captTaxDistrinct=&ctl00%24MainBody%24captOccupancy=&ctl00%24MainBody%24captCleanStatus=&ctl00%24MainBody%24captHoseKepping=&ctl00%24MainBody%24captContractType=&ctl00%24MainBody%24captLockOff=&ctl00%24MainBody%24tbkeyCode=&ctl00%24MainBody%24ddlFeatures=";

            var viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            var viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            content = string.Format(content, viewstate, viewstategenerator);

            responseString = PostRequest("/Web_Page/Property%20Management/UnitSearch.aspx", content);

            int i = 0;
            //DataTable tbNotes = new DataTable();
            //tbNotes.Columns.Add("UnitName");
           
            var matchs = Regex.Matches(responseString, "<a id=\"ctl00_MainBody_DataGridUnit_ct.*?_nameButton.*?unit_id=(.*?)\">(.*?)</a>");
            foreach (Match m in matchs)
            {
                try
                {
                    i++;

                    var unitId = m.Groups[1].ToString();
                    var unitName = m.Groups[2].ToString();

                    Console.WriteLine(string.Format("{0} - {1} - {2}", i, matchs.Count, unitName));


                    addFile(() => exportUnitFeature(unitId, unitName));
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        private string exportUnitFeature(string unitId, string unitName)
        {
            try
            {
                var responseString = GetRequest("/Web_Page/Property%20Management/UnitInfo.aspx?unit_id=" + unitId);

                responseString = GetRequest("/Web_Page/Property%20Management/UnitAttributes.aspx");

                var parse = new UnitFeaturesParse(unitId, unitName, responseString, importFolder);
                parse.parse();

                //addFile(parse.GetFileName());
                return parse.GetFileName();
            }
            catch (Exception ex)
            {
                Thread.Sleep(3000);
            }

            return "";
        }



        public override string exportFile()
        {
            //startExportUnitFeature();
            addFile(() => exportAssignHouseKeeper());
            addFile(() => exportHousekeepingDashboard());
            addFile(() => exportFolioNotes());

            String fileName = "PM12_" + Guid.NewGuid().ToString() + ".zip";
            CompressFile(fileNameList, importFolder + fileName);
            //*[@id="StiWebViewer1_JsViewerMainPanel"]/div[2]/div/table/tbody/tr/td[1]/table/tbody/tr/td[3]/div


            return fileName;

        }

        public override void uploadFile(string fileName)
        {

            UploadJobs("T", fileName, 0, "UnitFeature");
        }
    }
}
