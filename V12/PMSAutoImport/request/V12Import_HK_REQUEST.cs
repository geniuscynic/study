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
using System.Text.RegularExpressions;
using System.Threading;

namespace PMSAutoImport
{
    public abstract class V12Import_HK_REQUEST : V12Import_REQUEST_BASE
    {
        
        //string importFolder = ConfigurationManager.AppSettings["importFolder"];
        
        private string domain = "https://v12.instantsoftware.com/";
        protected CookieContainer myCookieContainer = new CookieContainer();

       
        public V12Import_HK_REQUEST(string companyId)
            : base(companyId)
        {
           if (companyId == "20452")
            {
                domain = "https://v12migration.instantsoftware.com/";
            }
         }

        public override void login()
        {
            var accountKey = companyId + "Account";
            string account = ConfigurationManager.AppSettings[accountKey];
            string companyCode = account.Split(',')[0]; // "0637";//importInfo.ClientSiteLogin.Split('-')[0];
            string login = account.Split(',')[1].Split('|')[0];//"EZinspections";//importInfo.ClientSiteLogin.Split('-')[1];
            string password = account.Split('|')[1].Replace("@", "%40");//importInfo.ClientSitePassword.Replace("@", "%40");

            string responseString = GetRequest("/V12login/V12Login.aspx");


            //string eventtarget = GetHtmlHiddenVaule(responseString, "__EVENTTARGET");
            // string eventargument = GetHtmlHiddenVaule(responseString, "__EVENTARGUMENT");
            string viewstate = GetHtmlHiddenVaule(responseString, "__VIEWSTATE");
            string viewstategenerator = GetHtmlHiddenVaule(responseString, "__VIEWSTATEGENERATOR");
            string eventvalidation = GetHtmlHiddenVaule(responseString, "__EVENTVALIDATION");

           
            string content = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__EVENTVALIDATION={2}&txtCompanyCode={3}&txtLogin={4}&txtPassword={5}&SubmitButton=Login&hdnClear=&hdnRedirect=";
            content = string.Format(content, viewstate, viewstategenerator, eventvalidation, companyCode, login, password);

            responseString = PostRequest("/v12login/V12Login.aspx?Authenticate", content);

            var temp_dynamicUrl = GetDynamicUrl(responseString);
            dynamicUrl = temp_dynamicUrl;

            string userdata = GetHtmlHiddenVaule(responseString, "userdata");
            string redirect = GetHtmlHiddenVaule(responseString, "redirect");
            content = string.Format("userdata={0}&redirect={1}", userdata, redirect);

            responseString = PostRequest("/Web_Page/SecondaryLoginPage.aspx", content);

           
            //responseString = GetRequest(myCookieContainer, domain +"/V12_12-7-07-000_B/Web_Page/index.aspx");
            Console.WriteLine("login successfully ");
        }

        //private string buildUrl(string url)
        //{
        //    return domain  + dynamicUrl + url;
        //}

        protected string GetRequest(string url)
        {
            url = buildUrl(url);

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


        protected override string PostRequest(string url, string content)
        {
            return PostRequest(url, new Dictionary<string, string>(), content, true);
        }

        protected override string PostRequest(string url,Dictionary<string ,string > headers,  string content)
        {
            return PostRequest(url, headers, content, true);
        }

        protected string PostRequest(string url, Dictionary<string ,string > headers, string content, bool allowAutoRedirect)
        {
            url = buildUrl(url);

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


        protected string GetHtmlHiddenVaule(string responseString, string name)
        {
            var matchString = GetHtmlHiddenVaule1(responseString, name);
            if (String.IsNullOrEmpty(matchString))
            {
                return GetHtmlHiddenVaule2(responseString, name);
            }

            return matchString;
        }

        protected string GetHtmlHiddenVaule1(string responseString, string name)
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

        protected string GetHtmlHiddenVaule2(string responseString, string name)
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

        protected string GetDynamicUrl(string responseString)
        {
            //Match m = Regex.Match(responseString, "<form.*?action=.*?>");
            //if (m.Length <= 0)
            //{
            //    return string.Empty;
            //    //throw new EziFormFillerJobLevelException(string.Format("Uploading photos to Altisource site failed, {0} can't be found.", name.Replace("__", "")));
            //}
            string value = Regex.Match(responseString, domain + ".*?/").ToString();

            return value.Replace(domain + "", "").Replace("/", "");

        }


    }
}
