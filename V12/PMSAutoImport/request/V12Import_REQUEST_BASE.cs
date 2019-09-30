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
    public abstract class V12Import_REQUEST_BASE:V12Import
    {
        protected const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        protected const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;


        public V12Import_REQUEST_BASE()
        {

        }

        public V12Import_REQUEST_BASE(string companyId)
        {

            this.companyId = companyId;

            importFolder = ConfigurationManager.AppSettings[companyId + "importFolder"];

            if (!string.IsNullOrEmpty (importFolder ) && Directory.Exists(importFolder))
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
            return "application/json";
        }


        protected virtual string PostRequest(string url,  string content)
        {
           return  PostRequest(url, new Dictionary<string, string>(), content);
        }

        protected virtual  string PostRequest(string url, Dictionary<string ,string > headers, string content)
        {


            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = GetContentType();
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64)";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");

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

        protected virtual string GetRequest(string url, Dictionary<string ,string > headers)
        {


            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = GetContentType();
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64)";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
         
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

    }
}
