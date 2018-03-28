using SJBCS.SMS.Implementation;

namespace SJBCS.SMS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RESTService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RESTService.svc or RESTService.svc.cs at the Solution Explorer and start debugging.
    public class RESTService : IRESTService
    {
        private SMSImpl smsImpl = new SMSImpl();

        public bool acknowledgeSMS(AcknowledgeRequestData rData)
        {
            bool ret = smsImpl.UpdateSMSStatus(rData.id, rData.state);
            return ret;
        }

        public bool sendSMS(SendRequestData rData)
        {
            bool ret = smsImpl.SendSMS(rData);
            return ret;
        }

        public string testServer()
        {
            return "Service is running.";
        }
    }
}
