using DevExpress.XtraEditors;
using GameX.Types;
using WatsonTcp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;

namespace GameX.Modules
{
    public class ConnectedClient
    {
        public string IP { get; set; }
        public int Index { get; set; }
        public int Player { get; set; }
    }

    public class Network
    {
        public static bool ModuleStarted { get; set; }
        private static App Main { get; set; }
        public static WatsonTcpServer Server { get; set; }
        public static WatsonTcpClient Client { get; set; }
        public static ConnectedClient[] Clients { get; private set; }
        public static string PublicIPv4 { get; private set; }
        public static string PrivateIPv4 { get; private set; }
        public static bool HasConnection { get; private set; }

        /* Utils */

        public static bool TestConnection(string HostNameOrAddress, int Timeout = 100)
        {
            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send(HostNameOrAddress, Timeout, new byte[32]);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string MachinePublicIP()
        {
            string address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }

        public static string MachinePrivateIP()
        {
            List<string> ipAddrList = new List<string>();

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddrList.Add(ip.Address.ToString());
                        }
                    }
                }
            }

            return ipAddrList.ToArray().FirstOrDefault();
        }

        /* Module Manager */

        public static async Task StartModule(App GameXRef)
        {
            Main = GameXRef;
            HasConnection = await Task.Run(() => TestConnection("google.com"));

            if (HasConnection)
            {
                Clients = new ConnectedClient[3];
                PublicIPv4 = MachinePublicIP();
                PrivateIPv4 = MachinePrivateIP();
                ModuleStarted = true;
                return;
            }

            ModuleStarted = false;
        }

        public static void FinishModule()
        {
            Main = null;
            HasConnection = false;
            Clients = null;
            PublicIPv4 = "";
            PrivateIPv4 = "";

            if (Server != null)
            {
                Server.Dispose();
                Server = null;
            }

            if (Client != null)
            {
                Client.Dispose();
                Client = null;
            }

            ModuleStarted = false;
        }

        /* Server */

        #region SERVER

        public static void StartServer_Click(object sender, EventArgs e)
        {
            if (!ModuleStarted)
            {
                Terminal.WriteLine("The Network module hasn't been enabled, enable it to start a server.");
                return;
            }

            if (Server == null)
                StartServer(sender, e);
            else
                StopServer(sender, e);
        }

        public static void StartServer(object sender, EventArgs e)
        {
            CheckButton CB = sender as CheckButton;

            if (Main.ServerPortTextEdit.Text == "")
            {
                Terminal.WriteLine("There is not server port specified, aborting.");
                return;
            }

            bool PortParsed = int.TryParse(Main.ServerPortTextEdit.Text, out int Port);

            if (!PortParsed || Port > 65535 || Port < 0)
            {
                Terminal.WriteLine("Invalid port, please check your inputs.");
                return;
            }

            try
            {
                Server = new WatsonTcpServer("127.0.0.1", Port);
                Server.Events.ClientConnected += Server_ClientConnected;
                Server.Events.ClientDisconnected += Server_ClientDisconnected;
                Server.Events.MessageReceived += Server_MessageReceived;
                Server.Events.ServerStarted += Server_ServerStarted;
                Server.Events.ServerStopped += Server_ServerStopped;
                Server.Callbacks.SyncRequestReceived = Server_SyncRequestReceived;

                Server.Settings.Logger = Server_Logger;
                Server.Settings.DebugMessages = true;
                Server.Settings.MaxConnections = 3;

                Server.Keepalive.EnableTcpKeepAlives = true;
                Server.Keepalive.TcpKeepAliveInterval = 1;
                Server.Keepalive.TcpKeepAliveTime = 1;
                Server.Keepalive.TcpKeepAliveRetryCount = 3;
            }
            catch (Exception Ex)
            {
                Server?.Dispose();
                Server = null;
                Terminal.WriteLine(Ex.Message);
                return;
            }

            Server.Start();

            CB.Text = "Close";
            Main.ServerStatusTextEdit.Text = $"Server sucessfully hosted at: 127.0.0.1:{Port}";
        }

        public static void StopServer(object sender, EventArgs e)
        {
            CheckButton CB = sender as CheckButton;

            Server.DisconnectClients();
            Server.Dispose();
            Server = null;

            CB.Text = "Open";
            Main.ServerStatusTextEdit.Text = "Server offline";
        }

        private static void Server_ClientConnected(object sender, ConnectionEventArgs args)
        {
            ComboBoxEdit[] PlayerIndexes =
            {
                Main.P1PlayerIndexComboBoxEdit,
                Main.P2PlayerIndexComboBoxEdit,
                Main.P3PlayerIndexComboBoxEdit
            };

            for (int i = 0; i < 3; i++)
            {
                if (Clients[i] == null)
                    Clients[i] = new ConnectedClient() { IP = args.IpPort, Index = i, Player = (PlayerIndexes[i].SelectedItem as ListItem).Value };
            }

            Terminal.WriteLine("Client connected: " + args.IpPort);
        }
        private static void Server_ClientDisconnected(object sender, DisconnectionEventArgs args)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Clients[i] != null && Clients[i].IP == args.IpPort)
                    Clients[i] = null;
            }

            Terminal.WriteLine("Client disconnected: " + args.IpPort + ": " + args.Reason.ToString());
        }

        private static void Server_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Terminal.WriteLine("Message from " + args.IpPort + ": " + Encoding.UTF8.GetString(args.Data));
        }

        private static void Server_ServerStarted(object sender, EventArgs args)
        {
            Terminal.WriteLine("Server started.");
        }

        private static void Server_ServerStopped(object sender, EventArgs args)
        {
            Terminal.WriteLine("Server closed.");
        }

        private static SyncResponse Server_SyncRequestReceived(SyncRequest req)
        {
            return new SyncResponse(req, req.Data);
        }

        private static void Server_Logger(Severity sev, string msg)
        {
            Terminal.WriteLine("[" + sev.ToString().PadRight(9) + "] " + msg);
        }

        public static void ServerLoop()
        {
            if (!ModuleStarted || Server == null)
                return;


        }

        #endregion

        /* Client */

        #region CLIENT

        public static void ConnectToServer_Click(object sender, EventArgs e)
        {
            
        }

        public static void DisconnectFromServer_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
