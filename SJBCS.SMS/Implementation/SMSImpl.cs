using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SJBCS.SMS.Implementation
{
    public class SMSImpl
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Task<bool> SendSMS(SendRequestData requestData)
        {
            TaskCompletionSource<bool> taskCompletion = new TaskCompletionSource<bool>();
            bool ret = false;

            if (String.IsNullOrWhiteSpace(requestData.URL))
            {
                Logger.Error("Failed to send SMS: URL is not setup.");
                taskCompletion.SetResult(ret);
                return taskCompletion.Task;
            }

            RestClient client = new RestClient(requestData.URL);
            RestRequest request = new RestRequest(Method.POST);
            request.AddJsonBody(new {
                number = requestData.Number,
                text = requestData.Text
            });

            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
                        string smsID = values["id"];

                        if (!string.IsNullOrEmpty(requestData.AttendanceID))
                        {
                            DatabaseImpl dbImpl = new DatabaseImpl();
                            ret = dbImpl.UpdateAttendanceSMSID(requestData.AttendanceID, requestData.IsTimeIn, smsID);
                            Logger.Debug("SMS ID update: " + ret);
                        }
                        else
                        {
                            // For bulk sending
                            ret = true;
                        }
                    }
                    else
                    {
                        Logger.Error("Failed to send SMS: " + response.ErrorMessage);
                    }
                    taskCompletion.SetResult(ret);
                });
            }
            catch (Exception error)
            {
                taskCompletion.SetResult(ret);
                Logger.Error("Error encountered when sending SMS: ", error);
            }

            return taskCompletion.Task;
        }

        public bool UpdateSMSStatus(string smsID, string status)
        {
            DatabaseImpl dbImpl = new DatabaseImpl();
            bool ret = dbImpl.UpdateAttendaceSMSStatus(smsID, status);
            Logger.Debug("SMS status update: " + ret);
            return ret;
        }
    }
}