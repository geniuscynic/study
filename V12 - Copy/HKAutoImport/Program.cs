using HKAutoImport;
using HKAutoImport.ServiceReference1;
using PMSAutoImport;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;


namespace HKAutoImport1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
               

                var companyId = args[0];
                //var companyId = "21810";

                ezUpdaterServicesSoapClient ezClient = new ezUpdaterServicesSoapClient();
                var ds = ezClient.GetV12PassWord(int.Parse(companyId));
                
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var account = row["loginAccount"].ToString();
                    var password = row["loginPassWord"].ToString();
                    var productCode = row["productCode"].ToString();
                    var status = row["v12status"].ToString();

                    var formCompanyId = row["formCompanyId"].ToString();
                    var ownerCompanyId = row["owenCompanyId"].ToString();
                    companyId = row["companyId"].ToString();

                    //var parse = "";
                    if (status == "0") //to-do
                    {
                        continue;
                    }

                    if (args.Length >= 2)
                    {
                        if (productCode != args[1])
                        {
                            continue;
                        }
                    }

                    //var codes = productCode.Split('|');
                   // productCode = codes[0];

                    //if (codes.Length >= 2)
                    //{
                       // parse = codes[1];
                    //}



                    V12Import v12 = null;
                    switch (productCode)
                    {
                        case "CFR":
                            v12 = new V12Import_CFR(companyId, account, password, productCode);
                            break;

                        case "LINE":
                            v12 = new V12Import_LINE(companyId, account, password, productCode);
                            break;

                        case "VRMAIN":
                            v12 = new V12Import_VRMAIN(companyId, account, password, productCode);
                            break;

                        case "HK":
                            v12 = new V12Import_HK(companyId, account, password, productCode);
                            break;

                        case "STREAMLINE":
                            v12 = new V12Import_STREAMLINE(companyId, account, password, productCode);
                            break;

                        case "ESCAPIA":
                            v12 = new SITE_ESCAPIA(companyId, account, password, productCode);
                            break;

                        case "ESCAPIA_VRMAIN":
                            v12 = new SITE_ESCAPIA(companyId, account, password, productCode);
                            break;
                    }


                    Console.WriteLine("begin autoImport.ImportJob " + companyId);
                    v12.setOwnerCoid(ownerCompanyId);
                    v12.setFormCoid(formCompanyId);
                    v12.login();
                    v12.exportAndUpload();


                    Console.WriteLine("end autoImport.ImportJob " + companyId);

                }

            }
            catch (WebException ex2)
            {
                using (StreamReader reader = new StreamReader(ex2.Response.GetResponseStream()))
                {
                    var message = reader.ReadToEnd();
                    File.AppendAllText("Exception_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + message + "\r\n" + ex2.StackTrace);

                }


            }
            catch (Exception ex)
            {
                File.AppendAllText("Exception_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + ex.Message + "\r\n" + ex.StackTrace);

            }

        }

        public static void Run(Action<string> action, string companyid)
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    action(companyid);
                    return;
                }
                catch (Exception ex)
                {
                    File.AppendAllText("Exception_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + ex.Message + "\r\n" + ex.StackTrace + "\r\n");
                    Thread.Sleep(10000);
                }
            }


            throw new Exception("Run failed");
        }

        private static string _publicIP = string.Empty;
        public static string GetPublicIP()
        {
            //return "52.12.202.205";
            //return "54.189.29.234 IP-0AD92F0C";
            //return "52.12.58.68 IP-0AD619DE";

            if (_publicIP == string.Empty)
            {
                //string publicIPFilePath = Application.StartupPath + "\\PublicIP.txt";
                string publicIPFilePath = "c:\\PublicIP.txt";

                if (File.Exists(publicIPFilePath))
                {
                    _publicIP = File.ReadAllText(publicIPFilePath);
                }

                if (_publicIP == string.Empty)
                {
                    Exception ex2BeLogged = null;
                    int i = 0;

                    // retry 3 times to get public IP if failed.
                    while (_publicIP == string.Empty && i < 3)
                    {
                        i++;
                        try
                        {

                            using (WebClient wc = new System.Net.WebClient())
                            {
                                _publicIP = System.Text.Encoding.ASCII.GetString((wc.DownloadData("https://www.ezinspections.com/info/requestIP.aspx"))) + " " + System.Environment.MachineName;
                            }
                        }
                        catch (Exception ex)
                        {
                            ex2BeLogged = ex;
                        }
                    }

                    if (_publicIP == string.Empty && ex2BeLogged != null)
                    {
                        throw new Exception("GetPublicIP failed");
                    }
                    else
                    {
                        File.WriteAllText(publicIPFilePath, _publicIP);
                    }
                }
            }
            return _publicIP;
        }
    }
}
