using ICSharpCode.SharpZipLib.Zip;
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
using System.Xml;

namespace PMSAutoImport
{
    public abstract class SITE_REQUEST_BASE:V12Import
    {
        protected const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        protected const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
        protected String domain = "";

        protected CookieContainer myCookieContainer = new CookieContainer();

        public SITE_REQUEST_BASE()
        {

        }

        public SITE_REQUEST_BASE(string companyId)
        {

            this.companyId = companyId;

            importFolder = ConfigurationManager.AppSettings[companyId + "importFolder"];

            if (!string.IsNullOrEmpty (importFolder ) && !Directory.Exists(importFolder))
            {
                //File.AppendAllText("Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\nCan't find folder");

                Directory.CreateDirectory(importFolder);
            }


            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback
                                                                                    (
                                                                                       delegate { return true; }
                                                                                    );


            ServicePointManager.SecurityProtocol = Tls12;
            //OpenQA.Selenium.Support.UI.
            //SelectElement s=new SelectElement
        }

        public override void login()
        {
           
        }

       
        protected virtual string GetContentType() {
            return "application/x-www-form-urlencoded";
        }

        protected virtual string PostRequest(string url, string content)
        {
            return PostRequest(url, content, true );
        }

        protected virtual string PostRequest(string url, string content, bool allowAutoRedirect)
        {
           return  PostRequest(url, new Dictionary<string, string>(), content, allowAutoRedirect);
        }

        protected virtual string PostRequest(string url, Dictionary<string, string> headers, string content)
        {
            return PostRequest(url, headers, content, true);
        }

        protected virtual string PostRequest(string url, Dictionary<string, string> headers, string content, bool allowAutoRedirect)
        {


            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = GetContentType();
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64)";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.CookieContainer = myCookieContainer;
            request.AllowAutoRedirect = allowAutoRedirect;

            foreach (string key in headers.Keys)
            {
                request.Headers.Add(key, headers[key]);
            }
            //request.Proxy = new WebProxy("127.0.0.1", 8848);

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

        protected virtual string GetRequest(string url)
        {
            return GetRequest(url, new Dictionary<string, string>());
        }

        protected virtual string GetRequest(string url, Dictionary<string ,string > headers)
        {


            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = GetContentType();
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64)";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.CookieContainer = myCookieContainer;

            foreach(string key in headers.Keys ) {
                request.Headers.Add(key, headers[key]);
            }
            //request.Proxy = new WebProxy("127.0.0.1", 8848);

            request.AllowAutoRedirect = true;

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

        protected string DownFile(CookieContainer myCookieContainer, string url, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            if (!string.IsNullOrEmpty(dynamicUrl))
            {
                url = url.Replace("V12_12-7-07-000_B", dynamicUrl);
            }

            //var reqUrl = "https://v12.instantsoftware.com/V12login/V12Login.aspx";
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
            Match m = Regex.Match(responseString, string.Format("name=\"{0}\".*?value.*?\".*?\"", name));
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

        protected string buildUrl(string url)
        {
            return domain + url;
        }
    }
}
