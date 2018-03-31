using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace SJBCS.SMS.Implementation
{
    public class SMSImpl
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool SendSMS(SendRequestData requestData)
        {
            bool ret = false;
            var client = new RestClient(requestData.URL);
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(new {
                number = requestData.Number,
                text = requestData.Text
            });

            try
            {
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
                    var smsID = values["id"];

                    if (!string.IsNullOrEmpty(requestData.AttendanceID))
                    {
                        int rowsAffected = DatabaseImpl.updateAttendanceSMSID(requestData.AttendanceID, requestData.IsTimeIn, smsID);
                        ret = rowsAffected > 0;
                        Logger.Debug("Update SMS ID rows affected: " + rowsAffected);
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
            }
            catch (Exception error)
            {
                Logger.Error("Error encountered when sending SMS: ", error);
            }

            return ret;
        }

        public bool UpdateSMSStatus(string smsID, string status)
        {
            bool ret;
            int rowsAffected = DatabaseImpl.updateAttendaceSMSStatus(smsID, status);
            ret = rowsAffected > 0;
            Logger.Debug("Update status rows affected: " + rowsAffected);

            return ret;
        }
    }
}