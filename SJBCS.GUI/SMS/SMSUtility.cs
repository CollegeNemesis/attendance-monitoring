using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace SJBCS.GUI.SMS
{
    public class SMSUtility
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string SMSURL = "http://192.168.43.1:8080/";

        public static void SendSMS(string text, string number, string attendaceID, Action<string[]> callback)
        {
            SendSMSAsync(text, number, response =>
            {
                string[] responseData = new string[2];
                responseData[0] = response;
                responseData[1] = attendaceID;
                callback(responseData);
            });
        }

        public static void SendSMS(string text, string number, Action<string[]> callback)
        {
            SendSMSAsync(text, number, response =>
            {
                string[] responseData = new string[1];
                responseData[0] = response;
                callback(responseData);
            });
        }

        private static void SendSMSAsync(string text, string number, Action<string> callback)
        {
            try
            {
                RestClient client = new RestClient(SMSURL);
                RestRequest request = new RestRequest(Method.POST);
                request.AddJsonBody(new
                {
                    number,
                    text
                });

                client.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
                        string smsID = values["id"];
                        callback(smsID);
                    }
                    else
                    {
                        Logger.Error("Failed to send SMS: " + response.ErrorMessage);
                    }
                });
            }
            catch (Exception error)
            {
                Logger.Error("Error encountered when sending SMS: ", error);
            }
        }
    }
}
