using WatsonTcp;

namespace GameX.Base.Types
{
    public class ConnectedServer
    {
        public string IP { get; set; }
        public WatsonTcpClient Connector { get; set; }
    }
}