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
    public class V12Import_HK_UnitFeature : V12Import_HK_REQUEST
    {
        
        //string importFolder = ConfigurationManager.AppSettings["importFolder"];
        private string dynamicUrl = "";
        private string domain = "https://v12.instantsoftware.com/";
        protected CookieContainer myCookieContainer = new CookieContainer();


        public V12Import_HK_UnitFeature(string companyId)
            : base(companyId)
        {
           
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



        public override string exportFile(string type)
        {
            startExportUnitFeature();
            // addFile(() => exportHousekeepingServicesSchedule());


            String fileName = "UnitFeature_" + Guid.NewGuid().ToString() + ".zip";
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
