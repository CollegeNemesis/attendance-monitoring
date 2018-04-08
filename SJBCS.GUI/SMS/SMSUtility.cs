using RestSharp;
using SJBCS.Data;
using System;
using System.Net;

namespace SJBCS.GUI.SMS
{
    public class SMSUtility
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void SendSMS(string text, string number, string attendaceID, bool isTimeIn, Action<string> success, Action<string> fail)
        {
            SendSMSAsync(text, number, attendaceID, isTimeIn, success, fail);
        }

        public static void SendSMS(string text, string number, Action<string> success, Action<string> fail)
        {
            SendSMSAsync(text, number, null, false, success, fail);
        }

        private static void SendSMSAsync(string text, string number, string attendaceID, bool isTimeIn, Action<string> success, Action<string> fail)
        {
            string SMSURL = ConnectionHelper.Config.AppConfiguration.Settings.SmsService.Url;

            try
            {
                RestClient client = new RestClient("http://localhost:54000/SMSService/SendSMS");
                RestRequest request = new RestRequest(Method.POST);
                request.AddJsonBody(new
                {
                    AttendanceID = attendaceID,
                    IsTimeIn = isTimeIn,
                    Number = number,
                    Text = text,
                    URL = SMSURL
                });

                client.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        bool responseData = Boolean.Parse(response.Content);
                        if (responseData)
                        {
                            success?.Invoke("Successfully sent SMS to Android Phone");
                        }
                        else
                        {
                            fail?.Invoke("Failed to send SMS");
                        }
                    }
                    else
                    {
                        fail?.Invoke(response.ErrorMessage);
                        Logger.Error("Failed to send SMS: " + response.ErrorMessage);
                    }
                });
            }
            catch (Exception error)
            {
                fail?.Invoke(error.Message);
                Logger.Error("Error encountered when sending SMS: ", error);
            }
        }
    }
}
