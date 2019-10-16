using HKAutoImport;
using HKAutoImport.ServiceReference1;
using PMSAutoImport;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            //    string startDate = "05/14/2019 12:00 AM";
            //    startDate = DateTime.Parse(startDate).ToShortDateString();
            //    startDate = DateTime.Parse(startDate).AddDays(-5).ToString("MM/dd/yy");
                //UnitFeaturesParse arrivalParse = new UnitFeaturesParse();
                //arrivalParse.parse("002", "");


                V12Import v13 = null;
                v13 = new SITE_ESCAPIA("21144");
                v13.login();
                v13.exportAndUpload();

                var companyId = "";
               


                if (args[0] == "17519")
                {
                    companyId = args[0];
                }
                else if (args[0] == "18155")
                {
                    companyId = args[0];
                }
                else if (args[0] == "18167")
                {
                    companyId = args[0];
                }
                else if (args[0] == "18302")
                {
                    companyId = args[0];
                }
                else if (args[0] == "15418")
                {
                    companyId = args[0];
                }
                else if (args[0] == "18980")
                {
                    companyId = args[0];
                }
                else if (args[0] == "18935")
                {
                    companyId = args[0];

                    //if (DateTime.Now.Hour == 5)
                    //{
                    //    return;
                    //}
                }
                else if (args[0] == "20942")
                {
                    companyId = args[0];
                }


               
                

                ezUpdaterServicesSoapClient ezClient = new ezUpdaterServicesSoapClient();
                var companyId2 = int.Parse(ConfigurationManager.AppSettings[companyId + "CompanyId"]);
                var clientId = int.Parse(ConfigurationManager.AppSettings[companyId + "OwnerCoId"]);
                var formCoid = int.Parse(ConfigurationManager.AppSettings[companyId + "FormCoid"]);
                var productCode = "HK";
                string ipAddress = GetPublicIP();
                ezClient.UpdateJobProgress(companyId2, 25, productCode, companyId2, ipAddress, 1);

                var executionId = Guid.NewGuid().ToString();
                ezClient.LogAutomationProgramStarted(executionId, 1, companyId2, 25, productCode, companyId2, ipAddress);
                
                try
                {
                    V12Import v12 = null;
                    if (args[1] == "import")
                    {   //autoImport.ImportJob(companyId);
                        if (args[2] == "CFR")
                        {
                            v12 = new V12Import_CFR(companyId);
                        }
                        else if (args[2] == "LINE")
                        {
                            v12 = new V12Import_LINE(companyId);
                        }
                        else if (args[2] == "VRMAIN")
                        {
                            v12 = new V12Import_VRMAIN(companyId);
                        }
                        else if (args[2] == "HK")
                        {
                            v12 = new V12Import_HK(companyId);
                        }
                        else if (args[2] == "STREAMLINE")
                        {
                            v12 = new V12Import_STREAMLINE(companyId);
                        }

                        Console.WriteLine("begin autoImport.ImportJob " + companyId);
                       // File.AppendAllText("Exception_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "begin autoImport.ImportJob " + companyId);
                        v12.login();
                        v12.exportFile();
                       
                        // Run(autoImport.ImportJob4, companyId);
                       // File.AppendAllText("Exception_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "end autoImport.ImportJob " + companyId);

                        Console.WriteLine("end autoImport.ImportJob " + companyId);
                    }
                    
                    else if (args[1] == "excelreport")
                    {
                        //Run(autoImport.ImportJob2, companyId);
                    }
                    else if (args[1] == "note")
                    {
                        //Run(autoImport.ImportJob3, companyId);
                    }

                   

                }
                catch (Exception ex)
                {
                    File.AppendAllText("Exception_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + ex.Message + "\r\n" + ex.StackTrace);
                    ezClient.LogAutomationProgramResult(executionId, "F", ex.Message);
                }
                ezClient.LogAutomationProgramResult(executionId, "S", "");
                ezClient.UpdateJobProgress(companyId2, 25, productCode, companyId2, ipAddress, 1);


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
