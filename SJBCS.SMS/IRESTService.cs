using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SJBCS.SMS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRESTService" in both code and config file together.
    [ServiceContract]
    public interface IRESTService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "Test")]
        string testServer();

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "AcknowledgeSMS")]
        ResponseData acknowledgeSMS(AcknowledgeRequestData rData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "SendSMS")]
        ResponseData sendSMS(SendRequestData AttendanceID);
    }

    [DataContract]
    public class AcknowledgeRequestData
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string frag { get; set; }
        [DataMember]
        public string text { get; set; }
    }

    [DataContract]
    public class SendRequestData
    {
        [DataMember]
        public string AttendanceID { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public string URL { get; set; }
    }

    [DataContract]
    public class ResponseData
    {
        [DataMember]
        public string Response { get; set; }
    }
}
