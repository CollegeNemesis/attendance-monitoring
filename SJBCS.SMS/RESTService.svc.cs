using SJBCS.SMS.Implementation;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SJBCS.SMS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RESTService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RESTService.svc or RESTService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class RESTService : IRESTService
    {
        private SMSImpl smsImpl = new SMSImpl();

        public bool AcknowledgeSMS(AcknowledgeRequestData rData)
        {
            bool ret = smsImpl.UpdateSMSStatus(rData.id, rData.state);
            return ret;
        }

        public async Task<bool> SendSMS(SendRequestData rData)
        {
            bool ret = await smsImpl.SendSMS(rData);
            return ret;
        }

        public string TestServer()
        {
            return "Service is running.";
        }
    }
}
