using ICSharpCode.SharpZipLib.Zip;
using PMSAutoImport.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace PMSAutoImport
{
    public class tmp_HKImport
    {
        int waitMinute = 5;
        //string importFolder = ConfigurationManager.AppSettings["importFolder"];
        string dynamicUrl = "V12_12-7-07-000_B";
        string domain = "https://v12.instantsoftware.com";
        public void SetDomain(string companyId)
        {
            if (companyId == "20452")
            {
                domain = "https://v12migration.instantsoftware.com";
            }
        }

        public void ImportJob(string companyId, List<string> fileNameList)
        {

            string importFolder = ConfigurationManager.AppSettings[companyId + "importFolder"];

            var startDate = DateTime.Now.AddDays(0).ToShortDateString().Replace("/", "%2F");
            var endDate = DateTime.Now.AddDays(1).ToShortDateString().Replace("/", "%2F");

            if (companyId == "17519")
            {
                if (DateTime.Now.Hour == 5)
                {
                    startDate = DateTime.Now.AddDays(-4).ToShortDateString().Replace("/", "%2F");
                    endDate = DateTime.Now.ToShortDateString().Replace("/", "%2F");
                }
                else
                {
                    startDate = DateTime.Now.AddDays(0).ToShortDateString().Replace("/", "%2F");
                    endDate = DateTime.Now.AddDays(1).ToShortDateString().Replace("/", "%2F");

                }
            }
            else if (companyId == "18167")
            {
                if (DateTime.Now.Hour == 5)
                {
                    startDate = DateTime.Now.AddDays(-4).ToShortDateString().Replace("/", "%2F");
                    endDate = DateTime.Now.ToShortDateString().Replace("/", "%2F");
                }
                else
                {
                    startDate = DateTime.Now.AddDays(0).ToShortDateString().Replace("/", "%2F");
                    endDate = DateTime.Now.AddDays(2).ToShortDateString().Replace("/", "%2F");

                }
            }
            else if (companyId == "18302")
            {
                startDate = DateTime.Now.AddDays(0).ToShortDateString().Replace("/", "%2F");
                endDate = DateTime.Now.AddDays(7).ToShortDateString().Replace("/", "%2F");
            }
            else if (companyId == "15418" || companyId == "18980")
            {
                startDate = DateTime.Now.AddDays(0).ToShortDateString().Replace("/", "%2F");
                endDate = DateTime.Now.AddDays(7).ToShortDateString().Replace("/", "%2F");
            }
            else if (companyId == "18935")
            {
                startDate = DateTime.Now.AddDays(0).ToShortDateString().Replace("/", "%2F");
                endDate = DateTime.Now.AddDays(6).ToShortDateString().Replace("/", "%2F");
            }
            else if (companyId == "20452")
            {
                startDate = "03%2F06%2F2019"; //DateTime.Now.AddDays(0).ToShortDateString().Replace("/", "%2F");
                endDate = "03%2F16%2F2019"; DateTime.Now.AddDays(6).ToShortDateString().Replace("/", "%2F");
            }

            //var fileNameList = new List<string>();

            CookieContainer myCookieContainer = new CookieContainer();
            //PostJifen(myCookieContainer);

            string responseString = GetRequest(myCookieContainer, domain + "/V12login/V12Login.aspx");


            //string eventtarget = GetHtmlHiddenVaule(responseString, "__EVENTTARGET");
            // string eventargument = GetHtmlHiddenVaule(responseString, "__EVENTARGUMENT");
            string viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            string viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            string eventvalidation = GetHtmlHiddenVaule(responseString, "__EVENTVALIDATION");

            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            string companyCode = account.Split(',')[0]; // "0637";//importInfo.ClientSiteLogin.Split('-')[0];
            string login = account.Split(',')[1].Split('|')[0];//"EZinspections";//importInfo.ClientSiteLogin.Split('-')[1];
            string password = account.Split('|')[1].Replace("@", "%40");//importInfo.ClientSitePassword.Replace("@", "%40");

            string content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__EVENTVALIDATION={2}&txtCompanyCode={3}&txtLogin={4}&txtPassword={5}&SubmitButton=Login&hdnClear=&hdnRedirect=";
            content = string.Format(content, viewstate, viewstategenerator, eventvalidation, companyCode, login, password);

            responseString = PostRequest(myCookieContainer, domain + "/v12login/V12Login.aspx?Authenticate", content);

            dynamicUrl = GetDynamicUrl(responseString);
            string userdata = GetHtmlHiddenVaule(responseString, "userdata");
            string redirect = GetHtmlHiddenVaule(responseString, "redirect");
            content = string.Format("userdata={0}&redirect={1}", userdata, redirect);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/SecondaryLoginPage.aspx", content);

            //responseString = GetRequest(myCookieContainer, domain +"/V12_12-7-07-000_B/Web_Page/index.aspx");
            Console.WriteLine("login successfully ");

            responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Housekeeping/AssignHousekeepingTask.aspx");


            content = "__EVENTTARGET=ctl00%24MainBody%24btnExport&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24knockoutViewModel%24viewStateStorage=%7B%7D&Date={2}&DateThrough={3}&RevenueGroupID=&HousekeepingSizeID=&HousekeepingGroupID=&CleaningTypeCode=&CleanStatusID=&UnitName=&HousekeeperType=&AssignedToID=";

            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            content = string.Format(content, viewstate, viewstategenerator, startDate, endDate);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Housekeeping/AssignHousekeepingTask.aspx", content);


            Match m = Regex.Match(responseString, "open\\('.*?\\'");
            var downloadUrl = m.ToString().Replace("open(", "").Replace("'", "");

            //File.AppendAllText("Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\ndownloadUrl:" + downloadUrl);


            string excelPath = importFolder + Guid.NewGuid().ToString() + ".xls";
            responseString = DownFile(myCookieContainer, downloadUrl, excelPath);
            fileNameList.Add(excelPath);
            Console.WriteLine("down file 1 ");


            //
            //Reservation Type
            responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Housekeeping/HousekeepingDashboard.aspx");
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
            content = "{\"Date\":\"" + startDate + "\",\"DateThrough\":\"" + endDate + "\",\"UnitName\":\"\",\"HasOpenWorkOrders\":false,\"ShowUnitsWithoutFolios\":false}";
            // content = string.Format(content, startDate, endDate);

            responseString = PostJson(myCookieContainer, domain + "/V12_12-7-07-000_B/Api/HousekeepingDashboard", content, xid, uid, coid);
            excelPath = importFolder + "FolioNumber_" + Guid.NewGuid().ToString() + ".txt";
            File.WriteAllText(excelPath, responseString);
            fileNameList.Add(excelPath);

            excelPath = importFolder + Guid.NewGuid().ToString() + ".txt";
            File.WriteAllText(excelPath, responseString);
            fileNameList.Add(excelPath);

            //if (companyId == "15418" || companyId == "18980")
            //{

            //    startDate = DateTime.Now.AddDays(0).ToShortDateString().Replace("/", "%2F");
            //    endDate = DateTime.Now.AddDays(7).ToShortDateString().Replace("/", "%2F");
            //    string complexId = "1099";
            //    if (companyId == "18935")
            //    {
            //        startDate = DateTime.Now.AddDays(0).ToShortDateString().Replace("/", "%2F");
            //        endDate = DateTime.Now.AddDays(6).ToShortDateString().Replace("/", "%2F");
            //        complexId = "0";
            //    }

            //    responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=Arrivals");


            //    content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24par0ChoiceListReportType_NoNewRow=Arrivals&ctl00%24MainBody%24par0UnitID=0&ctl00%24MainBody%24par0TodayFromDate_NoNewRow%24txtDate={2}&ctl00%24MainBody%24par0TodayThruDate_NoNewRow%24txtDate={3}&ctl00%24MainBody%24par0RevenueGroup_NoNewRow=0&ctl00%24MainBody%24par0ComplexID={4}&ctl00%24MainBody%24par0ChoiceListFolioType_NoNewRow=All&ctl00%24MainBody%24par0RateUnitType_NoNewRow=0&ctl00%24MainBody%24par0PropertyManager_NoNewRow=0&ctl00%24MainBody%24par0FilterUnitFeature=&ctl00%24MainBody%24par0FilterUnitFeatureChoice=0&ctl00%24MainBody%24par0ChoiceListCheckIn_NoNewRow=All&ctl00%24MainBody%24par0ChoiceListActive_NoNewRow=All&ctl00%24MainBody%24par0PackageID_NoNewRow=0&ctl00%24MainBody%24parStayLength=0&ctl00%24MainBody%24par0ZoneID=0&ctl00%24MainBody%24par0ChoiceListFD_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListHK_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListAD_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListR=Default&ctl00%24MainBody%24par0ChoiceListM_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListS_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListP_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListC=Default&ctl00%24MainBody%24par0ChoiceListPI_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListOW_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListOF_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListWOB=Default&ctl00%24MainBody%24par0ChoiceListFormat_NoNewRow=Export&ctl00%24MainBody%24par0ChoiceListAllGuests_NoNewRow=Default&ctl00%24MainBody%24par0UnitFeature_NoNewRow=&ctl00%24MainBody%24par0ChoiceListSort=Unit+Name&ctl00%24MainBody%24par0YesNoOverrideExclude_NoNewRow=N&ctl00%24MainBody%24par0YesNoShowUnitAddress=N&ctl00%24MainBody%24submit=Run";

            //    viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            //    viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            //    content = string.Format(content, viewstate, viewstategenerator, startDate, endDate, complexId);

            //    responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=Arrivals", content);

            //    viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            //    viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            //    content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__LASTFOCUS=&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24StiWebViewer1%24SaveTypeList=Microsoft+Excel+2007+File...&ctl00%24MainBody%24StiWebViewer1%24Save.x=9&ctl00%24MainBody%24StiWebViewer1%24Save.y=12&ctl00%24MainBody%24StiWebViewer1%24ZoomList=100%25&ctl00%24MainBody%24StiWebViewer1%24ViewModeList=Whole+Report&ctl00%24MainBody%24StiWebViewer1%24PagesRange=ctl00_MainBody_StiWebViewer1All&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1Zoom=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageFormat=Jpeg&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ExportMode=Table&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageResolution=10&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageCompressionMethod=Jpeg&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1AllowEditable=No&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageQuality=10&ctl00%24MainBody%24StiWebViewer1%24BorderTypeGroupBox=ctl00_MainBody_StiWebViewer1Simple&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ZoomX=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ZoomY=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingTextOrCsvFile=1252&ctl00%24MainBody%24StiWebViewer1%24ImageTypeGroupBox=ctl00_MainBody_StiWebViewer1Color&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingDifFile=437&ctl00%24MainBody%24StiWebViewer1%24ExportModeGroupBox=ctl00_MainBody_StiWebViewer1Table&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1Separator=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingDbfFile=Default&ctl00%24MainBody%24StiWebViewer1%24SaveReportGroupBox=ctl00_MainBody_StiWebViewer1SaveReportMdc&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1PasswordSaveReport=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1UserPassword=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1OwnerPassword=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncryptionKeyLength=Bit40&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1SubjectNameString=";
            //    content = string.Format(content, viewstate, viewstategenerator);

            //    excelPath = importFolder + "Arrivals_" + Guid.NewGuid().ToString() + ".xlsx";
            //    responseString = DownFile(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=Arrivals", content, excelPath);
            //    fileNameList.Add(excelPath);
            //}

            responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=FolioNotes");

            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            startDate = DateTime.Now.AddDays(-14).ToShortDateString().Replace("/", "%2F");
            endDate = DateTime.Now.ToShortDateString().Replace("/", "%2F");

            content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24par0ChoiceListRangeType=Notes+Created&ctl00%24MainBody%24par0Today0%24txtDate={2}&ctl00%24MainBody%24par0Today%24txtDate={3}&ctl00%24MainBody%24par0FolioNoteType=H&ctl00%24MainBody%24par0UserCode=0&ctl00%24MainBody%24par0RevenueGroup=0&ctl00%24MainBody%24par0Complex=&ctl00%24MainBody%24par0Units=&ctl00%24MainBody%24parFolio=0&ctl00%24MainBody%24parPhrase=&ctl00%24MainBody%24par0ChoiceListSort=Folio&ctl00%24MainBody%24par0ChoiceListFormat=Print&ctl00%24MainBody%24parReportGrouping=&ctl00%24MainBody%24par0UnitFeature=&ctl00%24MainBody%24par0YesNoCancelled=N&ctl00%24MainBody%24submit=Run";
            content = string.Format(content, viewstate, viewstategenerator, startDate, endDate);

            if (companyId == "15418" || companyId == "18980" || companyId == "20452")
            {
                String pdfPath = importFolder + "FolioNotes_" + Guid.NewGuid() + ".xls";

                responseString = DownFile(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=FolioNotes", content, pdfPath);

                responseString = DownFile(myCookieContainer, downloadUrl, pdfPath);
                fileNameList.Add(pdfPath);
            }
            else
            {
                responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=FolioNotes", content, true);

                // <iframe src=domain +"/CustomerData/0637/ReportsOutput/FolioNotes-20170527-022657528.pdf" id="ctl00_MainBody_FramedPage" width="100%" frameborder="0" style="height: 100%;"></iframe>
                m = Regex.Match(responseString, "<iframe src=\".*?\"");

                downloadUrl = m.ToString().Replace("\"", "").Replace("<iframe src=", "");
                String pdfPath = importFolder + "FolioNotes_" + Guid.NewGuid() + ".pdf";
                responseString = DownFile(myCookieContainer, downloadUrl, pdfPath);
                fileNameList.Add(pdfPath);
            }
            Console.WriteLine("down file 2 ");
            
        }

        public void ImportJobLinens(string companyId)
        {
            string importFolder = ConfigurationManager.AppSettings[companyId + "importFolder"];

            if (!Directory.Exists(importFolder))
            {
                //File.AppendAllText("Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\nCan't find folder");

                Directory.CreateDirectory(importFolder);
            }

            //if (!Directory.Exists(importFolder))
            //{
            //    File.AppendAllText("Log2_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\nCan't find folder");

            //    //Directory.CreateDirectory(importFolder);
            //}

            //if (!Directory.Exists("z:\\17519\\"))
            //{
            //    File.AppendAllText("Log3_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\nCan't find folder");

            //    //Directory.CreateDirectory(importFolder);
            //}

            //if (!Directory.Exists("z:\\17519"))
            //{
            //    File.AppendAllText("Log4_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\nCan't find folder");

            //    //Directory.CreateDirectory(importFolder);
            //}

            var startDate = new DateTime(DateTime.Today.Year, 1, 1).ToShortDateString().Replace("/", "%2F");
            var throughDate = DateTime.Today.AddDays(0).ToShortDateString().Replace("/", "%2F");
            var staysFrom = DateTime.Today.AddDays(3).ToShortDateString().Replace("/", "%2F");
            var staysThroughDate = DateTime.Today.AddDays(4).ToShortDateString().Replace("/", "%2F");


            var fileNameList = new List<string>();

            CookieContainer myCookieContainer = new CookieContainer();
            //PostJifen(myCookieContainer);

            string responseString = GetRequest(myCookieContainer, domain + "/V12login/V12Login.aspx");


            //string eventtarget = GetHtmlHiddenVaule(responseString, "__EVENTTARGET");
            // string eventargument = GetHtmlHiddenVaule(responseString, "__EVENTARGUMENT");
            string viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            string viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            string eventvalidation = GetHtmlHiddenVaule(responseString, "__EVENTVALIDATION");

            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            string companyCode = account.Split(',')[0]; // "0637";//importInfo.ClientSiteLogin.Split('-')[0];
            string login = account.Split(',')[1].Split('|')[0];//"EZinspections";//importInfo.ClientSiteLogin.Split('-')[1];
            string password = account.Split('|')[1].Replace("@", "%40");//importInfo.ClientSitePassword.Replace("@", "%40");

            string content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__EVENTVALIDATION={2}&txtCompanyCode={3}&txtLogin={4}&txtPassword={5}&SubmitButton=Login&hdnClear=&hdnRedirect=";
            content = string.Format(content, viewstate, viewstategenerator, eventvalidation, companyCode, login, password);

            responseString = PostRequest(myCookieContainer, domain + "/v12login/V12Login.aspx?Authenticate", content);

            dynamicUrl = GetDynamicUrl(responseString);
            string userdata = GetHtmlHiddenVaule(responseString, "userdata");
            string redirect = GetHtmlHiddenVaule(responseString, "redirect");
            content = string.Format("userdata={0}&redirect={1}", userdata, redirect);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/SecondaryLoginPage.aspx", content);

            //responseString = GetRequest(myCookieContainer, domain +"/V12_12-7-07-000_B/Web_Page/index.aspx");
            Console.WriteLine("login successfully ");

            responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=AddOnsSold");
            content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24par0MonthStart%24txtDate={2}&ctl00%24MainBody%24par0Today%24txtDate={3}&ctl00%24MainBody%24par0MonthStart2%24txtDate={4}&ctl00%24MainBody%24par0FarFuture%24txtDate={5}&ctl00%24MainBody%24par0ChoiceListRange=Folios+with+Occupancy&ctl00%24MainBody%24par0RevenueGroup=0&ctl00%24MainBody%24par0Complex=&ctl00%24MainBody%24par0AddOnDescription=&ctl00%24MainBody%24par0AddOnName=&ctl00%24MainBody%24par0User=&ctl00%24MainBody%24par0YesNoC=N&ctl00%24MainBody%24parFolio=0&ctl00%24MainBody%24par0YesNoIncidentals=N&ctl00%24MainBody%24par0ChoiceListFormat=Export&ctl00%24MainBody%24par0ChoiceListUAUN=Default&ctl00%24MainBody%24queueReport=No&ctl00%24MainBody%24submit=Run";

            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            content = string.Format(content, viewstate, viewstategenerator, startDate, throughDate, staysFrom, staysThroughDate);

            string excelPath = importFolder + "AddOnsSold_" + Guid.NewGuid().ToString() + ".xlsx";
            responseString = DownFile(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=AddOnsSold", content, excelPath);

            fileNameList.Add(excelPath);
            Console.WriteLine("down file 1 ");


            responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=HousekeepingServicesSchedule");
            content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24par0Today_NoNewRow%24txtDate={2}&ctl00%24MainBody%24par0Today2%24txtDate={3}&ctl00%24MainBody%24par0LongAgo_NoNewRow%24txtDate=1%2F1%2F2000&ctl00%24MainBody%24par0FarFuture%24txtDate=12%2F31%2F2078&ctl00%24MainBody%24par0HousekeepingGroup_NoNewRow=&ctl00%24MainBody%24par0Housekeeper=0&ctl00%24MainBody%24par0CleanCode_NoNewRow=B&ctl00%24MainBody%24par0CleanCode2_NoNewRow=L&ctl00%24MainBody%24par0CleanCode3=T&ctl00%24MainBody%24par0ChoiceList=All&ctl00%24MainBody%24par0CleanStatusID=0&ctl00%24MainBody%24par0ChoiceListNotes=Housekeeping&ctl00%24MainBody%24par0ChoiceListUN=Yes&ctl00%24MainBody%24par0YesNoShowMN=N&ctl00%24MainBody%24par0ChoiceListNS=No&ctl00%24MainBody%24par0YesNoShowVacant=N&ctl00%24MainBody%24par0ChoiceListPackageInfo=No&ctl00%24MainBody%24par0RevenueGroup=0&ctl00%24MainBody%24par0Complex=&ctl00%24MainBody%24par0PropertyManager=0&ctl00%24MainBody%24parReportGrouping=&ctl00%24MainBody%24par0ChoiceListSort=Clean+Date&ctl00%24MainBody%24par0ChoiceListFormat=Export&ctl00%24MainBody%24queueReport=No&ctl00%24MainBody%24submit=Run";

            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            startDate = DateTime.Today.AddDays(0).ToShortDateString().Replace("/", "%2F");
            throughDate = DateTime.Today.AddDays(7).ToShortDateString().Replace("/", "%2F");

            content = string.Format(content, viewstate, viewstategenerator, startDate, throughDate);

            excelPath = importFolder + "HousekeepingServicesSchedule_" + Guid.NewGuid().ToString() + ".xlsx";
            fileNameList.Add(excelPath);

            responseString = DownFile(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=HousekeepingServicesSchedule", content, excelPath);

            Console.WriteLine("down file 2 ");
            String fileName = "line_" + Guid.NewGuid().ToString() + ".zip";


            CompressFile(fileNameList, importFolder + fileName);

            //File.Copy(importFolder + fileName, fileName);
            // LogInfo("===================Begin upload jobs==================");
            Console.WriteLine("===================Begin upload jobs==================");
            UploadJobs("T", fileName, 0, companyId);

            Console.WriteLine("===================End upload jobs==================");
            // LogInfo("===================End upload jobs==================");
        }

        //report and BBCC sweep
        public void ImportJob2(string companyId)
        {
            string importFolder = ConfigurationManager.AppSettings[companyId + "importFolder"];
            if (!Directory.Exists(importFolder))
            {
                Directory.CreateDirectory(importFolder);
            }
            var startDate = DateTime.Now.AddDays(-7).ToShortDateString();
            var endDate = DateTime.Now.ToShortDateString();



            var fileNameList = new List<string>();

            CookieContainer myCookieContainer = new CookieContainer();
            //PostJifen(myCookieContainer);

            string responseString = GetRequest(myCookieContainer, domain + "/V12login/V12Login.aspx");


            //string eventtarget = GetHtmlHiddenVaule(responseString, "__EVENTTARGET");
            // string eventargument = GetHtmlHiddenVaule(responseString, "__EVENTARGUMENT");
            string viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            string viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            string eventvalidation = GetHtmlHiddenVaule(responseString, "__EVENTVALIDATION");

            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            string companyCode = account.Split(',')[0]; // "0637";//importInfo.ClientSiteLogin.Split('-')[0];
            string login = account.Split(',')[1].Split('|')[0];//"EZinspections";//importInfo.ClientSiteLogin.Split('-')[1];
            string password = account.Split('|')[1].Replace("@", "%40");//importInfo.ClientSitePassword.Replace("@", "%40");

            string content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__EVENTVALIDATION={2}&txtCompanyCode={3}&txtLogin={4}&txtPassword={5}&SubmitButton=Login&hdnClear=&hdnRedirect=";
            content = string.Format(content, viewstate, viewstategenerator, eventvalidation, companyCode, login, password);

            responseString = PostRequest(myCookieContainer, domain + "/v12login/V12Login.aspx?Authenticate", content);

            dynamicUrl = GetDynamicUrl(responseString);
            string userdata = GetHtmlHiddenVaule(responseString, "userdata");
            string redirect = GetHtmlHiddenVaule(responseString, "redirect");
            content = string.Format("userdata={0}&redirect={1}", userdata, redirect);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/SecondaryLoginPage.aspx", content);

            //responseString = GetRequest(myCookieContainer, domain +"/V12_12-7-07-000_B/Web_Page/index.aspx");

            responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Housekeeping/AssignHousekeepingTask.aspx");


            content = "__EVENTTARGET=ctl00%24MainBody%24btnExport&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24knockoutViewModel%24viewStateStorage=%7B%7D&Date={2}&DateThrough={3}&RevenueGroupID=&HousekeepingSizeID=&HousekeepingGroupID=&CleaningTypeCode=&CleanStatusID=&UnitName=&HousekeeperType=&AssignedToID=";

            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            content = string.Format(content, viewstate, viewstategenerator, startDate, endDate);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Housekeeping/AssignHousekeepingTask.aspx", content);


            Match m = Regex.Match(responseString, "open\\('.*?\\'");
            var downloadUrl = m.ToString().Replace("open(", "").Replace("'", "");

            string excelName = downloadUrl.Substring(downloadUrl.LastIndexOf("/") + 1);

            string excelPath = importFolder + excelName;

            responseString = DownFile(myCookieContainer, downloadUrl, excelPath);
            fileNameList.Add(excelPath);
            //sweep task end

            //UnitSummaryExport report 
            responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=UnitSummaryExport");

            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");


            content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24par0RevenueGroup=0&ctl00%24MainBody%24par0Complex=&ctl00%24MainBody%24par0FilterUnitFeature=&ctl00%24MainBody%24par0FilterUnitFeatureChoice=0&ctl00%24MainBody%24par0PropertyManager=0&ctl00%24MainBody%24par0Units=&ctl00%24MainBody%24par0ChoiceListStatus=Active&ctl00%24MainBody%24par0ChoiceListShowExcluded=Both&ctl00%24MainBody%24par0YesNo=N&ctl00%24MainBody%24submit=Run";
            content = string.Format(content, viewstate, viewstategenerator);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=UnitSummaryExport", content, false);

            // <iframe src=domain +"/CustomerData/0637/ReportsOutput/FolioNotes-20170527-022657528.pdf" id="ctl00_MainBody_FramedPage" width="100%" frameborder="0" style="height: 100%;"></iframe>
            m = Regex.Match(responseString, "<a href=\".*?\"");

            downloadUrl = m.ToString().Replace("\"", "").Replace("<a href=", "");
            String pdfPath = "";
            if (downloadUrl == "#" || downloadUrl == "")
            {
                viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
                viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

                content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__LASTFOCUS=&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24StiWebViewer1%24SaveTypeList=Microsoft+Excel+2007+File...&ctl00%24MainBody%24StiWebViewer1%24Save.x=20&ctl00%24MainBody%24StiWebViewer1%24Save.y=12&ctl00%24MainBody%24StiWebViewer1%24ZoomList=100%25&ctl00%24MainBody%24StiWebViewer1%24ViewModeList=Whole+Report&ctl00%24MainBody%24StiWebViewer1%24PagesRange=ctl00_MainBody_StiWebViewer1All&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1Zoom=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageFormat=Jpeg&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ExportMode=Table&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageResolution=10&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageCompressionMethod=Jpeg&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1AllowEditable=No&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageQuality=10&ctl00%24MainBody%24StiWebViewer1%24BorderTypeGroupBox=ctl00_MainBody_StiWebViewer1Simple&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ZoomX=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ZoomY=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingTextOrCsvFile=1252&ctl00%24MainBody%24StiWebViewer1%24ImageTypeGroupBox=ctl00_MainBody_StiWebViewer1Color&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingDifFile=437&ctl00%24MainBody%24StiWebViewer1%24ExportModeGroupBox=ctl00_MainBody_StiWebViewer1Table&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1Separator=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingDbfFile=Default&ctl00%24MainBody%24StiWebViewer1%24SaveReportGroupBox=ctl00_MainBody_StiWebViewer1SaveReportMdc&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1PasswordSaveReport=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1UserPassword=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1OwnerPassword=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncryptionKeyLength=Bit40&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1SubjectNameString=";
                content = string.Format(content, viewstate, viewstategenerator);

                excelName = "UnitSummaryExport_" + Guid.NewGuid().ToString() + ".xlsx";
                pdfPath = importFolder + excelName;
                responseString = DownFile(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=UnitSummaryExport", content, pdfPath);
                fileNameList.Add(pdfPath);
            }
            else
            {
                excelName = downloadUrl.Substring(downloadUrl.LastIndexOf("/") + 1);
                pdfPath = importFolder + excelName;
                responseString = DownFile(myCookieContainer, downloadUrl, pdfPath);
                fileNameList.Add(pdfPath);
            }

            //excelName = downloadUrl.Substring(downloadUrl.LastIndexOf("/") + 1);
            //String pdfPath = importFolder + excelName;
            //responseString = DownFile(myCookieContainer, downloadUrl, pdfPath);
            //fileNameList.Add(pdfPath);



            //UnitFeaturesExport report 
            responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=UnitFeaturesExport");

            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");


            content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24par0RevenueGroup=0&ctl00%24MainBody%24par0Complex=&ctl00%24MainBody%24par0PropertyManager=0&ctl00%24MainBody%24par0YesNo1=Y&ctl00%24MainBody%24par0YesNo2=N&ctl00%24MainBody%24submit=Run";
            content = string.Format(content, viewstate, viewstategenerator);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=UnitFeaturesExport", content, false);

            // <iframe src=domain +"/CustomerData/0637/ReportsOutput/FolioNotes-20170527-022657528.pdf" id="ctl00_MainBody_FramedPage" width="100%" frameborder="0" style="height: 100%;"></iframe>
            m = Regex.Match(responseString, "<a href=\".*?\"");

            downloadUrl = m.ToString().Replace("\"", "").Replace("<a href=", "");
            if (downloadUrl == "#" || downloadUrl == "")
            {
                viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
                viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

                content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__LASTFOCUS=&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24StiWebViewer1%24SaveTypeList=Microsoft+Excel+2007+File...&ctl00%24MainBody%24StiWebViewer1%24Save.x=5&ctl00%24MainBody%24StiWebViewer1%24Save.y=9&ctl00%24MainBody%24StiWebViewer1%24ZoomList=100%25&ctl00%24MainBody%24StiWebViewer1%24ViewModeList=Whole+Report&ctl00%24MainBody%24StiWebViewer1%24PagesRange=ctl00_MainBody_StiWebViewer1All&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1Zoom=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageFormat=Jpeg&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ExportMode=Table&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageResolution=10&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageCompressionMethod=Jpeg&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1AllowEditable=No&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageQuality=10&ctl00%24MainBody%24StiWebViewer1%24BorderTypeGroupBox=ctl00_MainBody_StiWebViewer1Simple&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ZoomX=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ZoomY=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingTextOrCsvFile=1252&ctl00%24MainBody%24StiWebViewer1%24ImageTypeGroupBox=ctl00_MainBody_StiWebViewer1Color&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingDifFile=437&ctl00%24MainBody%24StiWebViewer1%24ExportModeGroupBox=ctl00_MainBody_StiWebViewer1Table&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1Separator=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingDbfFile=Default&ctl00%24MainBody%24StiWebViewer1%24SaveReportGroupBox=ctl00_MainBody_StiWebViewer1SaveReportMdc&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1PasswordSaveReport=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1UserPassword=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1OwnerPassword=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncryptionKeyLength=Bit40&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1SubjectNameString=";
                content = string.Format(content, viewstate, viewstategenerator);

                excelName = "UnitFeaturesExport_" + Guid.NewGuid().ToString() + ".xlsx";
                pdfPath = importFolder + excelName;
                responseString = DownFile(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=UnitFeaturesExport", content, pdfPath);
                fileNameList.Add(pdfPath);
            }
            else
            {
                excelName = downloadUrl.Substring(downloadUrl.LastIndexOf("/") + 1);
                pdfPath = importFolder + excelName;
                responseString = DownFile(myCookieContainer, downloadUrl, pdfPath);
                fileNameList.Add(pdfPath);
            }
            //excelName = downloadUrl.Substring(downloadUrl.LastIndexOf("/") + 1);
            //pdfPath = importFolder + excelName;
            //responseString = DownFile(myCookieContainer, downloadUrl, pdfPath);
            //fileNameList.Add(pdfPath);

            String fileName = "Property_" + Guid.NewGuid().ToString() + ".zip";

            CompressFile(fileNameList, importFolder + fileName);

            //File.Copy(importFolder + fileName, fileName);
            // LogInfo("===================Begin upload jobs==================");
            Console.WriteLine("===================Begin upload jobs==================");
            UploadJobs("T", fileName, 0, companyId);

            Console.WriteLine("===================End upload jobs==================");
            // LogInfo("===================End upload jobs==================");
        }

        //note
        public void ImportJob3(string companyId)
        {
            string importFolder = ConfigurationManager.AppSettings[companyId + "importFolder"];
            if (!Directory.Exists(importFolder))
            {
                Directory.CreateDirectory(importFolder);
            }
            var startDate = DateTime.Now.ToShortDateString();
            var endDate = DateTime.Now.AddDays(1).ToShortDateString();
            if (DateTime.Now.Hour == 12)
            {
                startDate = DateTime.Now.ToShortDateString().Replace("/", "%2F");
                endDate = DateTime.Now.AddDays(1).ToShortDateString().Replace("/", "%2F");

            }
            else if (DateTime.Now.Hour == 7)
            {
                startDate = DateTime.Now.AddDays(-4).ToShortDateString().Replace("/", "%2F");
                endDate = DateTime.Now.ToShortDateString().Replace("/", "%2F");
            }

            var fileNameList = new List<string>();

            CookieContainer myCookieContainer = new CookieContainer();
            //PostJifen(myCookieContainer);

            string responseString = GetRequest(myCookieContainer, domain + "/V12login/V12Login.aspx");


            //string eventtarget = GetHtmlHiddenVaule(responseString, "__EVENTTARGET");
            // string eventargument = GetHtmlHiddenVaule(responseString, "__EVENTARGUMENT");
            string viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            string viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            string eventvalidation = GetHtmlHiddenVaule(responseString, "__EVENTVALIDATION");

            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            string companyCode = account.Split(',')[0]; // "0637";//importInfo.ClientSiteLogin.Split('-')[0];
            string login = account.Split(',')[1].Split('|')[0];//"EZinspections";//importInfo.ClientSiteLogin.Split('-')[1];
            string password = account.Split('|')[1].Replace("@", "%40");//importInfo.ClientSitePassword.Replace("@", "%40");


            string content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__EVENTVALIDATION={2}&txtCompanyCode={3}&txtLogin={4}&txtPassword={5}&SubmitButton=Login&hdnClear=&hdnRedirect=";
            content = string.Format(content, viewstate, viewstategenerator, eventvalidation, companyCode, login, password);

            responseString = PostRequest(myCookieContainer, domain + "/v12login/V12Login.aspx?Authenticate", content);

            dynamicUrl = GetDynamicUrl(responseString);
            string userdata = GetHtmlHiddenVaule(responseString, "userdata");
            string redirect = GetHtmlHiddenVaule(responseString, "redirect");
            content = string.Format("userdata={0}&redirect={1}", userdata, redirect);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/SecondaryLoginPage.aspx", content);

            //responseString = GetRequest(myCookieContainer, domain +"/V12_12-7-07-000_B/Web_Page/index.aspx");

            responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Property%20Management/UnitSearch.aspx");


            content = "__EVENTTARGET=ctl00%24TopHeaderBar%24btnSearch&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24captFindFirst=5000&ctl00%24MainBody%24tbunit_name=&ctl00%24MainBody%24captRateUnTyp=0&ctl00%24MainBody%24txtAddress1=&ctl00%24MainBody%24txtAddress2=&ctl00%24MainBody%24county=&ctl00%24MainBody%24city=&ctl00%24MainBody%24state=&ctl00%24MainBody%24zip_code=&ctl00%24MainBody%24captStatus=Active&ctl00%24MainBody%24captBeedroms=&ctl00%24MainBody%24captBathroms=&ctl00%24MainBody%24drdComplex=&ctl00%24MainBody%24captRating=&ctl00%24MainBody%24captSmokingAllowed=&ctl00%24MainBody%24captPetsAllowed=&ctl00%24MainBody%24drOwnerName=0&ctl00%24MainBody%24captTaxDistrinct=&ctl00%24MainBody%24captOccupancy=&ctl00%24MainBody%24captCleanStatus=&ctl00%24MainBody%24captHoseKepping=&ctl00%24MainBody%24captContractType=&ctl00%24MainBody%24captLockOff=&ctl00%24MainBody%24tbkeyCode=&ctl00%24MainBody%24ddlFeatures=";

            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            content = string.Format(content, viewstate, viewstategenerator);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Property%20Management/UnitSearch.aspx", content);

            int i = 0;
            DataTable tbNotes = new DataTable();
            tbNotes.Columns.Add("UnitName");
            tbNotes.Columns.Add("HousekeepingNotes");
            tbNotes.Columns.Add("MaintenanceNotes");
            tbNotes.Columns.Add("HousekeeperFirst");
            tbNotes.Columns.Add("HousekeeperLast");
            tbNotes.Columns.Add("AdditionalInfo");

            //List<HKNote> listNotes = new List<HKNote>();
            var matchs = Regex.Matches(responseString, "<a id=\"ctl00_MainBody_DataGridUnit_ct.*?_nameButton.*?unit_id=(.*?)\">(.*?)</a>");
            foreach (Match m in matchs)
            {
                try
                {
                    i++;


                    //HKNote note = new HKNote();

                    DataRow r = tbNotes.NewRow();
                    r["UnitName"] = m.Groups[2].ToString();

                    Console.WriteLine(string.Format("{0} - {1} - {2}", i, matchs.Count, r["UnitName"]));

                    //note.UnitID = m.Groups[1].ToString();
                    //note.UnitName = m.Groups[2].ToString();

                    responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Property%20Management/UnitInfo.aspx?unit_id=" + m.Groups[1].ToString());

                    responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Property%20Management/UnitMaintHskp.aspx");

                    Match m1 = Regex.Match(responseString, "<textarea.*?id=\"ctl00_MainBody_txtNotes.*?>([\\s\\S]*?)</textarea>");
                    //note.MaintenanceNotes = m1.Groups[1].ToString().Trim();
                    r["MaintenanceNotes"] = m1.Groups[1].ToString().Trim();
                    m1 = Regex.Match(responseString, "<textarea.*?id=\"ctl00_MainBody_txtHousekeepingNotes.*?>([\\s\\S]*?)</textarea>");
                    //note.HousekeepingNotes = m1.Groups[1].ToString().Trim();
                    r["HousekeepingNotes"] = m1.Groups[1].ToString().Trim();

                    r["HousekeeperFirst"] = "";
                    r["HousekeeperLast"] = "";


                    responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Property%20Management/UnitAdditionalInfo.aspx");
                    m1 = Regex.Match(responseString, "<input.*?id=\"ctl00_MainBody_knockoutViewModel_viewStateStorage.*? value=\"([\\s\\S]*?)\"");
                    r["AdditionalInfo"] = m1.Groups[1].ToString().Trim();
                    //ctl00_MainBody_knockoutViewModel_viewStateStorage
                    // listNotes.Add(note);
                    tbNotes.Rows.Add(r);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //string[] mapping = new string[5];

            //mapping[0] = "UnitName|UnitName";
            //mapping[1] = "HousekeepingNotes|HousekeepingNotes";
            //mapping[2] = "MaintenanceNotes|MaintenanceNotes";
            //mapping[3] = "HousekeeperFirst|HousekeeperFirst";
            //mapping[4] = "HousekeeperLast|HousekeeperLast";

            string path = importFolder + "Notes_" + Guid.NewGuid().ToString() + ".xls";
            //ExportToCSV(tbNotes.DefaultView, mapping, path);
            ExportExcelWithAspose(tbNotes, path);
            fileNameList.Add(path);

            String fileName = "Notes_" + tbNotes.Rows.Count + "_" + Guid.NewGuid().ToString() + ".zip";

            CompressFile(fileNameList, importFolder + fileName);

            Console.WriteLine("===================Begin upload jobs==================");
            UploadJobs("T", fileName, 0, companyId);

            Console.WriteLine("===================End upload jobs==================");
        }

        //note
        public void ImportJob4(string companyId)
        {
            string importFolder = ConfigurationManager.AppSettings[companyId + "importFolder"];
            if (!Directory.Exists(importFolder))
            {
                Directory.CreateDirectory(importFolder);
            }
            var startDate = DateTime.Now.AddDays(-29).ToShortDateString();
            var endDate = DateTime.Now.AddDays(7).ToShortDateString();


            var fileNameList = new List<string>();

            CookieContainer myCookieContainer = new CookieContainer();
            //PostJifen(myCookieContainer);

            string responseString = GetRequest(myCookieContainer, domain + "/V12login/V12Login.aspx");


            //string eventtarget = GetHtmlHiddenVaule(responseString, "__EVENTTARGET");
            // string eventargument = GetHtmlHiddenVaule(responseString, "__EVENTARGUMENT");
            string viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            string viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            string eventvalidation = GetHtmlHiddenVaule(responseString, "__EVENTVALIDATION");

            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            string companyCode = account.Split(',')[0]; // "0637";//importInfo.ClientSiteLogin.Split('-')[0];
            string login = account.Split(',')[1].Split('|')[0];//"EZinspections";//importInfo.ClientSiteLogin.Split('-')[1];
            string password = account.Split('|')[1].Replace("@", "%40");//importInfo.ClientSitePassword.Replace("@", "%40");


            string content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__EVENTVALIDATION={2}&txtCompanyCode={3}&txtLogin={4}&txtPassword={5}&SubmitButton=Login&hdnClear=&hdnRedirect=";
            content = string.Format(content, viewstate, viewstategenerator, eventvalidation, companyCode, login, password);

            responseString = PostRequest(myCookieContainer, domain + "/v12login/V12Login.aspx?Authenticate", content);

            dynamicUrl = GetDynamicUrl(responseString);
            string userdata = GetHtmlHiddenVaule(responseString, "userdata");
            string redirect = GetHtmlHiddenVaule(responseString, "redirect");
            content = string.Format("userdata={0}&redirect={1}", userdata, redirect);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/SecondaryLoginPage.aspx", content);

            //responseString = GetRequest(myCookieContainer, domain +"/V12_12-7-07-000_B/Web_Page/index.aspx");

            responseString = GetRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=ArrivalsByUnit");


            content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24par0ChoiceListReportType_NoNewRow=Arrivals+By+Unit&ctl00%24MainBody%24par0UnitID=0&ctl00%24MainBody%24par0TodayFromDate_NoNewRow%24txtDate={2}&ctl00%24MainBody%24par0TodayThruDate_NoNewRow%24txtDate={3}&ctl00%24MainBody%24par0RevenueGroup_NoNewRow=0&ctl00%24MainBody%24par0ComplexID=0&ctl00%24MainBody%24par0ChoiceListFolioType_NoNewRow=All&ctl00%24MainBody%24par0RateUnitType_NoNewRow=0&ctl00%24MainBody%24par0PropertyManager_NoNewRow=0&ctl00%24MainBody%24par0FilterUnitFeature=&ctl00%24MainBody%24par0FilterUnitFeatureChoice=0&ctl00%24MainBody%24par0ChoiceListCheckIn_NoNewRow=All&ctl00%24MainBody%24par0ChoiceListActive_NoNewRow=All&ctl00%24MainBody%24par0PackageID_NoNewRow=0&ctl00%24MainBody%24parStayLength=0&ctl00%24MainBody%24par0ZoneID=0&ctl00%24MainBody%24par0ChoiceListFD_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListHK_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListAD_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListR=Default&ctl00%24MainBody%24par0ChoiceListM_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListS_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListP_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListC=Default&ctl00%24MainBody%24par0ChoiceListPI_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListOW_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListOF_NoNewRow=Default&ctl00%24MainBody%24par0ChoiceListWOB=Default&ctl00%24MainBody%24par0ChoiceListFormat_NoNewRow=Export&ctl00%24MainBody%24par0ChoiceListAllGuests_NoNewRow=Default&ctl00%24MainBody%24par0UnitFeature_NoNewRow=&ctl00%24MainBody%24par0ChoiceListSort=Unit+Name&ctl00%24MainBody%24par0YesNoOverrideExclude_NoNewRow=N&ctl00%24MainBody%24par0YesNoShowUnitAddress=N&ctl00%24MainBody%24par0ChoiceListUAUN=Default&ctl00%24MainBody%24submit=Run";

            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            content = string.Format(content, viewstate, viewstategenerator, startDate, endDate);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=ArrivalsByUnit", content);


            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__LASTFOCUS=&ctl00%24universalSearch%24universalSearchViewModel%24viewStateStorage=%7B%7D&ctl00%24TopHeaderBar%24customReportId=&ctl00%24MainBody%24StiWebViewer1%24SaveTypeList=Microsoft+Excel+File...&ctl00%24MainBody%24StiWebViewer1%24Save.x=11&ctl00%24MainBody%24StiWebViewer1%24Save.y=14&ctl00%24MainBody%24StiWebViewer1%24ZoomList=100%25&ctl00%24MainBody%24StiWebViewer1%24ViewModeList=Whole+Report&ctl00%24MainBody%24StiWebViewer1%24PagesRange=ctl00_MainBody_StiWebViewer1All&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1Zoom=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageFormat=Jpeg&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ExportMode=Table&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageResolution=10&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageCompressionMethod=Jpeg&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1AllowEditable=No&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ImageQuality=10&ctl00%24MainBody%24StiWebViewer1%24BorderTypeGroupBox=ctl00_MainBody_StiWebViewer1Simple&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ZoomX=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1ZoomY=0.25&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingTextOrCsvFile=1252&ctl00%24MainBody%24StiWebViewer1%24ImageTypeGroupBox=ctl00_MainBody_StiWebViewer1Color&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingDifFile=437&ctl00%24MainBody%24StiWebViewer1%24ExportModeGroupBox=ctl00_MainBody_StiWebViewer1Table&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1Separator=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncodingDbfFile=Default&ctl00%24MainBody%24StiWebViewer1%24SaveReportGroupBox=ctl00_MainBody_StiWebViewer1SaveReportMdc&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1PasswordSaveReport=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1UserPassword=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1OwnerPassword=&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1EncryptionKeyLength=Bit40&ctl00%24MainBody%24StiWebViewer1%24ctl00_MainBody_StiWebViewer1SubjectNameString=";
            content = string.Format(content, viewstate, viewstategenerator);

            String excelPath = importFolder + "CFR_" + Guid.NewGuid().ToString() + ".xlsx";
            responseString = DownFile(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/Reports/Report.aspx?reportName=Arrivals", content, excelPath);
            fileNameList.Add(excelPath);


            Console.WriteLine("down file 1 ");




            String fileName = "CFR_" + Guid.NewGuid().ToString() + ".zip";

            CompressFile(fileNameList, importFolder + fileName);

            Console.WriteLine("===================Begin upload jobs==================");
            UploadJobs("T", fileName, 0, companyId);

            Console.WriteLine("===================End upload jobs==================");
        }



        private string GetRequest(CookieContainer myCookieContainer, string url)
        {
            if (!string.IsNullOrEmpty(dynamicUrl))
            {
                url = url.Replace("V12_12-7-07-000_B", dynamicUrl);
            }

            //var reqUrl = domain +"/V12login/V12Login.aspx";
            Thread.Sleep(500);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/x-www-form-urlencoded;";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64)";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.CookieContainer = myCookieContainer;


            string resultStr = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    resultStr = reader.ReadToEnd();
                }
            }
            return resultStr;
        }

        private string DownFile(CookieContainer myCookieContainer, string url, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            if (!string.IsNullOrEmpty(dynamicUrl))
            {
                url = url.Replace("V12_12-7-07-000_B", dynamicUrl);
            }

            //var reqUrl = domain +"/V12login/V12Login.aspx";
            Thread.Sleep(500);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/x-www-form-urlencoded;";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64)";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.CookieContainer = myCookieContainer;



            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    Stream stream = new FileStream(path, FileMode.Create);
                    byte[] bArr = new byte[1024];
                    int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    while (size > 0)
                    {
                        stream.Write(bArr, 0, size);
                        size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    }
                    stream.Close();

                }
            }

            return path;
        }

        private string DownFile(CookieContainer myCookieContainer, string url, string content, string path)
        {
            if (!string.IsNullOrEmpty(dynamicUrl))
            {
                url = url.Replace("V12_12-7-07-000_B", dynamicUrl);
            }

            //var reqUrl = ;
            Thread.Sleep(500);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded;";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64)";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.CookieContainer = myCookieContainer;
            request.AllowAutoRedirect = true;

            using (Stream requestStream = request.GetRequestStream())
            {
                byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(content);

                requestStream.Write(contentBytes, 0, contentBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    Stream stream = new FileStream(path, FileMode.Create);
                    byte[] bArr = new byte[1024];
                    int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    while (size > 0)
                    {
                        stream.Write(bArr, 0, size);
                        size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    }
                    stream.Close();

                }
            }

            return path;
        }

        private string PostRequest(CookieContainer myCookieContainer, string url, string content, bool allowAutoRedirect = true)
        {
            if (!string.IsNullOrEmpty(dynamicUrl))
            {
                url = url.Replace("V12_12-7-07-000_B", dynamicUrl);
            }

            //var reqUrl = ;
            Thread.Sleep(500);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded;";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64)";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.CookieContainer = myCookieContainer;
            request.AllowAutoRedirect = allowAutoRedirect;

            using (Stream requestStream = request.GetRequestStream())
            {
                byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(content);

                requestStream.Write(contentBytes, 0, contentBytes.Length);
            }

            string resultStr = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    resultStr = reader.ReadToEnd();
                }
            }
            return resultStr;
        }

        private string PostJson(CookieContainer myCookieContainer, string url, string content, string xid, string uid, string coid)
        {
            if (!string.IsNullOrEmpty(dynamicUrl))
            {
                url = url.Replace("V12_12-7-07-000_B", dynamicUrl);
            }

            //var reqUrl = ;
            Thread.Sleep(500);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/json";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64)";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("X-NewRelic-ID", xid);
            request.Headers.Add("UserId", uid);
            request.Headers.Add("COID", coid);
            request.CookieContainer = myCookieContainer;
            request.AllowAutoRedirect = true;

            using (Stream requestStream = request.GetRequestStream())
            {
                byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(content);

                requestStream.Write(contentBytes, 0, contentBytes.Length);
            }

            string resultStr = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    resultStr = reader.ReadToEnd();
                }
            }
            return resultStr;
        }

        private string PostFile(CookieContainer myCookieContainer, string url, string xid, string uid, string coid,
           Dictionary<string, string> nameValues, string filePath)
        {
            if (!string.IsNullOrEmpty(dynamicUrl))
            {
                url = url.Replace("V12_12-7-07-000_B", dynamicUrl);
            }

            var exitname = filePath.Substring(filePath.LastIndexOf(".") + 1).ToLower();
            var mimeName = "";

            if (exitname == "pdf")
            {
                mimeName = "application/pdf";
            }
            else if (exitname == "jpg")
            {
                mimeName = "image/jpeg";
            }

            //var reqUrl = ;
            Thread.Sleep(500);

            var boundary = "----" + Guid.NewGuid().ToString();

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.Method = "Post";
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64)";
            httpWebRequest.Accept = "application/json, text/javascript, */*";
            httpWebRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            httpWebRequest.Headers.Add("X-NewRelic-ID", xid);
            httpWebRequest.Headers.Add("UserId", uid);
            httpWebRequest.Headers.Add("COID", coid);
            httpWebRequest.CookieContainer = myCookieContainer;
            httpWebRequest.AllowAutoRedirect = true;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback
                                                                              (
                                                                                 delegate { return true; }
                                                                              );

            Stream requestStream = httpWebRequest.GetRequestStream();
            String totalContent = "";
            string content = string.Empty;
            byte[] contentBytes;

            // content = "--" + boundary + "\r\n";
            //totalContent += content;

            foreach (string key in nameValues.Keys)
            {
                content = "--" + boundary + "\r\n";
                content += string.Format("Content-Disposition: form-data; name=\"{0}\" \r\n\r\n{1}\r\n", key, nameValues[key]);
                contentBytes = System.Text.Encoding.UTF8.GetBytes(content);
                requestStream.Write(contentBytes, 0, contentBytes.Length);
                totalContent += content;
            }


            var fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);

            content = "--" + boundary + "\r\n";
            content += "Content-Disposition: form-data; name=\"files[]\"; filename=\"" + fileName + "\"\r\n";
            content += string.Format("Content-type: {0}\r\n\r\n", mimeName);
            contentBytes = System.Text.Encoding.UTF8.GetBytes(content);
            requestStream.Write(contentBytes, 0, contentBytes.Length);

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                requestStream.Write(buffer, 0, bytesRead);
                requestStream.Flush();
            }
            fileStream.Close();

            totalContent += content;
            content = "\r\n--" + boundary + "--\r\n";
            contentBytes = System.Text.Encoding.UTF8.GetBytes(content);
            requestStream.Write(contentBytes, 0, contentBytes.Length);

            requestStream.Close();
            totalContent += content;


            string resultStr = "";
            using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    resultStr = reader.ReadToEnd();
                }
            }
            return resultStr;
        }


        private string GetHtmlHiddenVaule(string responseString, string name)
        {
            var matchString = GetHtmlHiddenVaule1(responseString, name);
            if (String.IsNullOrEmpty(matchString))
            {
                return GetHtmlHiddenVaule2(responseString, name);
            }

            return matchString;
        }

        private string GetHtmlHiddenVaule1(string responseString, string name)
        {
            if (name.IndexOf("$") > -1)
            {
                name = name.Replace("$", "\\$");
            }
            Match m = Regex.Match(responseString, string.Format("name=\"{0}.*value.*\".*\"", name));
            if (m.Length <= 0)
            {
                return string.Empty;
                //throw new EziFormFillerJobLevelException(string.Format("Uploading photos to Altisource site failed, {0} can't be found.", name.Replace("__", "")));
            }
            string value = Regex.Match(m.ToString(), "value=\".*\"").ToString();

            return value.Replace("value=", "").Replace("\"", "").Replace("/", "%2F").Replace("+", "%2B").Replace("=", "%3D");
        }

        private string GetHtmlHiddenVaule2(string responseString, string name)
        {
            if (name.IndexOf("$") > -1)
            {
                name = name.Replace("$", "\\$");
            }
            Match m = Regex.Match(responseString, string.Format("name='{0}.*?value.*?'.*?'", name));
            if (m.Length <= 0)
            {
                return string.Empty;
                //throw new EziFormFillerJobLevelException(string.Format("Uploading photos to Altisource site failed, {0} can't be found.", name.Replace("__", "")));
            }
            string value = Regex.Match(m.ToString(), "value='.*'").ToString();

            return value.Replace("value=", "").Replace("'", "").Replace("/", "%2F").Replace("+", "%2B").Replace("=", "%3D");
        }

        private string GetDynamicUrl(string responseString)
        {
            //Match m = Regex.Match(responseString, "<form.*?action=.*?>");
            //if (m.Length <= 0)
            //{
            //    return string.Empty;
            //    //throw new EziFormFillerJobLevelException(string.Format("Uploading photos to Altisource site failed, {0} can't be found.", name.Replace("__", "")));
            //}
            string value = Regex.Match(responseString, domain + "/.*?/").ToString();

            return value.Replace(domain + "", "").Replace("/", "");

        }
        private void CompressFile(List<string> fileNames, string zipFileName)
        {
            using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFileName)))
            {
                foreach (string fileName in fileNames)
                {
                    using (FileStream fs = File.OpenRead(fileName))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        fs.Close();

                        string newfileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(newfileName);
                        s.PutNextEntry(entry);
                        s.SetLevel(1);

                        s.Write(buffer, 0, buffer.Length);
                    }
                }

                s.Finish();
                s.Close();
            }
        }


        public void UploadJobs(string fileType, string fileName, int toBeImported, string companyId)
        {
            UploadJobs(fileType, fileName, toBeImported, companyId, "HK");
        }

        public void UploadJobs(string fileType, string fileName, int toBeImported, string companyId, string productCode)
        {
            int succeeded = 0;
            int duplicated = 0;
            int failed = 0;
            ezUpdaterServicesSoapClient client = new ezUpdaterServicesSoapClient();

            string importationResult = client.ImportFiles(0, int.Parse(ConfigurationManager.AppSettings[companyId + "CompanyId"]), int.Parse(ConfigurationManager.AppSettings[companyId + "OwnerCoId"]), int.Parse(ConfigurationManager.AppSettings[companyId + "FormCoid"]), productCode, fileType, fileName, true, true, 0);
            XmlDocument importationResultXml = new XmlDocument();
            importationResultXml.LoadXml(importationResult);
            string status = importationResultXml.SelectSingleNode("/ImportLogDataSet/ImportLog/Status").InnerText;
            int.TryParse(importationResultXml.SelectSingleNode("/ImportLogDataSet/ImportLog/Succeeded").InnerText, out succeeded);
            int.TryParse(importationResultXml.SelectSingleNode("/ImportLogDataSet/ImportLog/Duplicated").InnerText, out duplicated);
            int.TryParse(importationResultXml.SelectSingleNode("/ImportLogDataSet/ImportLog/Failed").InnerText, out failed);


            File.AppendAllText(DateTime.Now.ToString("yyyyMMdd") + ".txt", importationResult);


            //client.UpdateJobProgress(importInfo.CompanyId, importInfo.FormCoId, importInfo.ProductCode, importInfo.OwnerCoId, CommonUtil.GetPublicIP(), 1);
        }

        public class HKNote
        {
            public string UnitID { get; set; }
            public string UnitName { get; set; }
            public string MaintenanceNotes { get; set; }
            public string HousekeepingNotes { get; set; }
        }


        private bool ExportToCSV(DataView dv, string[] mapping, string fileName)
        {
            try
            {
                string STRING_FORMAT = "\"{0}\"";
                FileInfo fi = new FileInfo(fileName);
                string strSubfolder = fi.Directory.FullName;

                if (!Directory.Exists(strSubfolder))
                {
                    Directory.CreateDirectory(strSubfolder);
                }

                StreamWriter writer = File.CreateText(fileName);


                foreach (string title in mapping)
                {
                    if ((title != null))
                    {
                        writer.Write(title.Split('|')[1]);
                        writer.Write(",");
                    }

                }
                writer.WriteLine();


                foreach (DataRowView row in dv)
                {

                    foreach (string title in mapping)
                    {
                        if ((title != null))
                        {
                            string data = string.Empty;
                            if (title.Split('|').Length > 2)
                            {
                                if (title.Split('|')[2].ToUpper().IndexOf("D2") >= 0)
                                {
                                    try
                                    {
                                        data = ((DateTime)row[title.Split('|')[0]]).ToString("dd/MM/yyyy hh:mm:ss tt PST");
                                    }
                                    catch (Exception ex)
                                    {
                                        data = row[title.Split('|')[0]].ToString();
                                    }
                                }
                                else if (title.Split('|')[2].ToUpper().IndexOf("D") >= 0)
                                {
                                    try
                                    {
                                        data = ((DateTime)row[title.Split('|')[0]]).ToShortDateString();
                                    }
                                    catch (Exception ex)
                                    {
                                        data = row[title.Split('|')[0]].ToString();
                                    }

                                }
                                else if (title.Split('|')[2].ToUpper().IndexOf("P") >= 0)
                                {
                                    try
                                    {
                                        data = decimal.Parse(row[title.Split('|')[0]].ToString()).ToString("p0");
                                    }
                                    catch (Exception ex)
                                    {
                                        data = row[title.Split('|')[0]].ToString();
                                    }
                                }
                                else if (title.Split('|')[2].ToUpper().IndexOf("C") >= 0)
                                {
                                    string value = row[title.Split('|')[0]].ToString();
                                    if (value.StartsWith("$"))
                                    {
                                        value = value.Remove(0, 1);
                                    }
                                    try
                                    {
                                        //data = FormatCurrency(value);
                                    }
                                    catch (Exception ex)
                                    {
                                        data = value;
                                    }
                                }
                                else
                                {
                                    data = string.Format(title.Split('|')[2], row[title.Split('|')[0]].ToString());
                                }

                            }
                            else
                            {
                                data = row[title.Split('|')[0]].ToString();
                            }


                            if (data.IndexOf(',') != -1)
                            {
                                data = string.Format(STRING_FORMAT, data);
                            }

                            writer.Write(data);
                            writer.Write(",");
                        }

                    }
                    writer.WriteLine();

                }
                writer.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }

        public static bool ExportExcelWithAspose(System.Data.DataTable dt, string path)
        {
            bool succeed = false;
            if (dt != null)
            {
                try
                {
                    Aspose.Cells.License li = new Aspose.Cells.License();
                    string str = System.Environment.CurrentDirectory;
                    string lic = @"<License>
  <Data>
    <SerialNumber>aed83727-21cc-4a91-bea4-2607bf991c21</SerialNumber>
    <EditionType>Enterprise</EditionType>
    <Products>
      <Product>Aspose.Total</Product>
    </Products>
  </Data>
 <Signature>CxoBmxzcdRLLiQi1kzt5oSbz9GhuyHHOBgjTf5w/wJ1V+lzjBYi8o7PvqRwkdQo4tT4dk3PIJPbH9w5Lszei1SV/smkK8SCjR8kIWgLbOUFBvhD1Fn9KgDAQ8B11psxIWvepKidw8ZmDmbk9kdJbVBOkuAESXDdtDEDZMB/zL7Y=</Signature>
</License>";
                    Stream s = new MemoryStream(ASCIIEncoding.Default.GetBytes(lic));
                    li.SetLicense(s);

                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                    Aspose.Cells.Worksheet cellSheet = workbook.Worksheets[0];

                    cellSheet.Name = dt.TableName;

                    int rowIndex = 0;
                    int colIndex = 0;
                    int colCount = dt.Columns.Count;
                    int rowCount = dt.Rows.Count;

                    //列名的处理
                    for (int i = 0; i < colCount; i++)
                    {
                        cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Columns[i].ColumnName);
                        cellSheet.Cells[rowIndex, colIndex].Style.Font.IsBold = true;
                        cellSheet.Cells[rowIndex, colIndex].Style.Font.Name = "宋体";
                        colIndex++;
                    }

                    Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
                    style.Font.Name = "Arial";
                    style.Font.Size = 10;
                    Aspose.Cells.StyleFlag styleFlag = new Aspose.Cells.StyleFlag();
                    cellSheet.Cells.ApplyStyle(style, styleFlag);

                    rowIndex++;

                    for (int i = 0; i < rowCount; i++)
                    {
                        colIndex = 0;
                        for (int j = 0; j < colCount; j++)
                        {
                            cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Rows[i][j].ToString());
                            colIndex++;
                        }
                        rowIndex++;
                    }
                    cellSheet.AutoFitColumns();

                    path = Path.GetFullPath(path);
                    workbook.Save(path);
                    succeed = true;
                }
                catch (Exception ex)
                {
                    succeed = false;
                }
            }

            return succeed;
        }

        public bool UpdatePDf(string companyId, string folioId, string filePath)
        {
            string importFolder = ConfigurationManager.AppSettings[companyId + "importFolder"];

            if (!Directory.Exists(importFolder))
            {
                //File.AppendAllText("Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\nCan't find folder");

                Directory.CreateDirectory(importFolder);
            }




            var fileNameList = new List<string>();

            CookieContainer myCookieContainer = new CookieContainer();
            //PostJifen(myCookieContainer);

            string responseString = GetRequest(myCookieContainer, domain + "/V12login/V12Login.aspx");


            //string eventtarget = GetHtmlHiddenVaule(responseString, "__EVENTTARGET");
            // string eventargument = GetHtmlHiddenVaule(responseString, "__EVENTARGUMENT");
            string viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            string viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            string eventvalidation = GetHtmlHiddenVaule(responseString, "__EVENTVALIDATION");

            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            string companyCode = account.Split(',')[0]; // "0637";//importInfo.ClientSiteLogin.Split('-')[0];
            string login = account.Split(',')[1].Split('|')[0];//"EZinspections";//importInfo.ClientSiteLogin.Split('-')[1];
            string password = account.Split('|')[1].Replace("@", "%40");//importInfo.ClientSitePassword.Replace("@", "%40");

            string content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__EVENTVALIDATION={2}&txtCompanyCode={3}&txtLogin={4}&txtPassword={5}&SubmitButton=Login&hdnClear=&hdnRedirect=";
            content = string.Format(content, viewstate, viewstategenerator, eventvalidation, companyCode, login, password);

            responseString = PostRequest(myCookieContainer, domain + "/v12login/V12Login.aspx?Authenticate", content);

            dynamicUrl = GetDynamicUrl(responseString);
            string userdata = GetHtmlHiddenVaule(responseString, "userdata");
            string redirect = GetHtmlHiddenVaule(responseString, "redirect");
            content = string.Format("userdata={0}&redirect={1}", userdata, redirect);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/SecondaryLoginPage.aspx", content);

            //responseString = GetRequest(myCookieContainer, domain +"/V12_12-7-07-000_B/Web_Page/index.aspx");
            Console.WriteLine("login successfully ");

            responseString = GetRequest(myCookieContainer, string.Format(domain + "/V12_12-7-07-000_B/Web_Page/Reservation/FolioGeneral.aspx?id={0}&page=folio", folioId));

            responseString = GetRequest(myCookieContainer, string.Format(domain + "/V12_12-7-07-000_B/Web_Page/Reservation/FolioFiles.aspx{0}", ""));


            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            var arrivalDateForCheckin = GetHtmlHiddenVaule(responseString, "ctl00$TopHeaderBar$FolioHeaderBar$arrivalDateForCheckin");
            var departureDateForCheckout = GetHtmlHiddenVaule(responseString, "ctl00$TopHeaderBar$FolioHeaderBar$departureDateForCheckout");
            var viewStateStorage = GetHtmlHiddenVaule(responseString, "ctl00$MainBody$knockoutViewModel$viewStateStorage");
            var entityName = "Folios";
            var entityId = folioId;


            var pattern = "xpid:\"(.*?)\"";
            var match = Regex.Match(responseString, pattern);

            var xid = match.Groups[1].Value;

            pattern = "coid.*?'(.*?)'.*?userid.*?'(.*?)'";
            match = Regex.Match(responseString, pattern);

            var coid = match.Groups[1].Value;
            var uid = match.Groups[2].Value;



            var nameValues = new Dictionary<string, string>();
            nameValues.Add("__EVENTTARGET", "");
            nameValues.Add("__EVENTARGUMENT", "");
            nameValues.Add("__VIEWSTATE", viewstate);
            nameValues.Add("__VIEWSTATEGENERATOR", viewstategenerator);
            nameValues.Add("ctl00$universalSearch$universalSearchViewModel$viewStateStorage", "{}");
            nameValues.Add("ctl00$TopHeaderBar$FolioHeaderBar$folioNumber", folioId);
            nameValues.Add("ctl00$TopHeaderBar$FolioHeaderBar$arrivalDateForCheckin", arrivalDateForCheckin);
            nameValues.Add("ctl00$TopHeaderBar$FolioHeaderBar$departureDateForCheckout", departureDateForCheckout);
            nameValues.Add("ctl00$MainBody$knockoutViewModel$viewStateStorage", viewStateStorage);
            nameValues.Add("entityName", entityName);
            nameValues.Add("entityId", entityId);
            nameValues.Add("description[]", "");

            responseString = PostFile(myCookieContainer, domain + "/V12_12-7-07-000_B/API/Files/upload", xid, uid, coid, nameValues, filePath);

            if (responseString.Contains("PublicUrl"))
            {
                return true;
            }
            return false;
        }

        public bool UpdateVRImage(string companyId, string workId, string[] filePaths)
        {
            string importFolder = ConfigurationManager.AppSettings[companyId + "importFolder"];

            if (!Directory.Exists(importFolder))
            {
                //File.AppendAllText("Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\nCan't find folder");

                Directory.CreateDirectory(importFolder);
            }




            var fileNameList = new List<string>();

            CookieContainer myCookieContainer = new CookieContainer();
            //PostJifen(myCookieContainer);

            string responseString = GetRequest(myCookieContainer, domain + "/V12login/V12Login.aspx");


            //string eventtarget = GetHtmlHiddenVaule(responseString, "__EVENTTARGET");
            // string eventargument = GetHtmlHiddenVaule(responseString, "__EVENTARGUMENT");
            string viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            string viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            string eventvalidation = GetHtmlHiddenVaule(responseString, "__EVENTVALIDATION");

            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            string companyCode = account.Split(',')[0]; // "0637";//importInfo.ClientSiteLogin.Split('-')[0];
            string login = account.Split(',')[1].Split('|')[0];//"EZinspections";//importInfo.ClientSiteLogin.Split('-')[1];
            string password = account.Split('|')[1].Replace("@", "%40");//importInfo.ClientSitePassword.Replace("@", "%40");

            string content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__EVENTVALIDATION={2}&txtCompanyCode={3}&txtLogin={4}&txtPassword={5}&SubmitButton=Login&hdnClear=&hdnRedirect=";
            content = string.Format(content, viewstate, viewstategenerator, eventvalidation, companyCode, login, password);

            responseString = PostRequest(myCookieContainer, domain + "/v12login/V12Login.aspx?Authenticate", content);

            dynamicUrl = GetDynamicUrl(responseString);
            string userdata = GetHtmlHiddenVaule(responseString, "userdata");
            string redirect = GetHtmlHiddenVaule(responseString, "redirect");
            content = string.Format("userdata={0}&redirect={1}", userdata, redirect);

            responseString = PostRequest(myCookieContainer, domain + "/V12_12-7-07-000_B/Web_Page/SecondaryLoginPage.aspx", content);

            //responseString = GetRequest(myCookieContainer, domain +"/V12_12-7-07-000_B/Web_Page/index.aspx");
            Console.WriteLine("login successfully ");

            responseString = GetRequest(myCookieContainer, string.Format(domain + "/V12_12-7-07-000_B/Web_Page/WorkOrders/WorkOrderInfo.aspx?id={0}", workId));

            responseString = GetRequest(myCookieContainer, string.Format(domain + "/V12_12-7-07-000_B/Web_Page/WorkOrders/WorkOrderImages.aspx{0}", ""));


            viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");

            var arrivalDateForCheckin = GetHtmlHiddenVaule(responseString, "ctl00$TopHeaderBar$FolioHeaderBar$arrivalDateForCheckin");
            var departureDateForCheckout = GetHtmlHiddenVaule(responseString, "ctl00$TopHeaderBar$FolioHeaderBar$departureDateForCheckout");
            var viewStateStorage = GetHtmlHiddenVaule(responseString, "ctl00$MainBody$knockoutViewModel$viewStateStorage");
            var entityName = "WorkOrders";
            var entityId = workId;


            var pattern = "xpid:\"(.*?)\"";
            var match = Regex.Match(responseString, pattern);

            var xid = match.Groups[1].Value;

            pattern = "coid.*?'(.*?)'.*?userid.*?'(.*?)'";
            match = Regex.Match(responseString, pattern);

            var coid = match.Groups[1].Value;
            var uid = match.Groups[2].Value;



            var nameValues = new Dictionary<string, string>();
            nameValues.Add("__EVENTTARGET", "");
            nameValues.Add("__EVENTARGUMENT", "");
            nameValues.Add("__VIEWSTATE", viewstate);
            nameValues.Add("__VIEWSTATEGENERATOR", viewstategenerator);
            nameValues.Add("ctl00$universalSearch$universalSearchViewModel$viewStateStorage", "{}");
            nameValues.Add("ctl00$MainBody$knockoutViewModel$viewStateStorage", "{}");
            nameValues.Add("entityName", entityName);
            nameValues.Add("entityId", entityId);
            nameValues.Add("description[]", "");

            bool flag = true;
            foreach (var filePath in filePaths)
            {
                responseString = PostFile(myCookieContainer, domain + "/V12_12-7-07-000_B/API/Files/upload", xid, uid, coid, nameValues, filePath);

                if (!responseString.Contains("PublicUrl"))
                {
                    flag = false;
                    // return true;
                }
                //return false;
            }

            return flag;
        }



    }
}
