using System;
using System.Runtime.Serialization;

namespace ClienteMercado.Utils.Sms
{
    [DataContract]
    public class MensagemSMS
    {
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public int data { get; set; }
        [DataMember]
        public string msg { get; set; }
    }
}
