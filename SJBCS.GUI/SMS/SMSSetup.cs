using SJBCS.SMS;
using SJBCS.Data;
using SJBCS.Services.Repository;
using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace SJBCS.GUI.SMS
{
    public class SMSSetup
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static SMSSetup _instance;
        private WebServiceHost serviceHost;
        private const int port = 54000;

        private SMSSetup() { }

        public static SMSSetup Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SMSSetup();
                }
                return _instance;
            }
        }

        public void StartSMSService()
        {
            try
            {
                string ipAddress = GetIPAddress();
                Uri baseAddress = new Uri(String.Format("http://{0}:54000/SMSService/", ipAddress));
                serviceHost = new WebServiceHost(typeof(RESTService), baseAddress);
                serviceHost.AddServiceEndpoint(typeof(IRESTService), new WebHttpBinding(), "");
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true
                };
                serviceHost.Description.Behaviors.Add(smb);
                serviceHost.Description.Namespace = "http://SJBCS.SMS";
                serviceHost.Open();
                Logger.Info("SMS Service started");
            }
            catch (CommunicationException ce)
            {
                Logger.Error("Error encountered when starting SMS Service", ce);
                serviceHost.Abort();
            }
            catch (Exception e)
            {
                Logger.Error("Error encountered when starting SMS Service", e);
                serviceHost.Abort();
            }
        }

        public void StopSMSService()
        {
            try
            {
                serviceHost?.Close();
                Logger.Info("SMS Service stopped");
            }
            catch (Exception e)
            {
                Logger.Error("Error encountered when stopping SMS Service", e);
                serviceHost.Abort();
            }
        }

        private string GetIPAddress()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 80);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint.Address.ToString();
            }
        }
    }
}
