using WatsonTcp;

namespace GameX.Base.Types
{
    public class Server
    {
        public string IP { get; set; }
        public WatsonTcpClient Connector { get; set; }
    }
}