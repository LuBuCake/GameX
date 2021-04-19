using DevExpress.XtraEditors;
using GameX.Base.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace GameX.Base.Modules
{
    public class ClientConnected
    {
        public string IP { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
    }

    public class ServerConnected
    {
        public string IP { get; set; }
        public WatsonTcpClient Connector {get;set;}
    }

    public class Network
    {
        public static bool ModuleStarted { get; set; }
        private static App Main { get; set; }
        public static WatsonTcpServer Server { get; set; }
        public static ServerConnected BuddyServer { get; set; }
        public static ClientConnected[] BuddyClients { get; private set; }
        public static string PublicIPv4 { get; private set; }
        public static string PrivateIPv4 { get; private set; }
        public static bool HasConnection { get; private set; }

        #region Utility

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

        #endregion

        #region Module Manager

        public static async Task StartModule(App GameXRef)
        {
            Main = GameXRef;
            HasConnection = await Task.Run(() => TestConnection("google.com"));

            if (HasConnection)
            {
                BuddyClients = new ClientConnected[3];
                PublicIPv4 = MachinePublicIP();
                PrivateIPv4 = MachinePrivateIP();
                ModuleStarted = true;
                Terminal.WriteLine("[Network] Module started successfully.");
                return;
            }

            ModuleStarted = false;
        }

        public static void FinishModule()
        {
            if (Server != null)
            {
                StopServer(null, null);
            }

            if (BuddyServer != null)
            {
                BuddyServer.Connector?.Dispose();
                BuddyServer.Connector = null;
                BuddyServer = null;
            }

            Main = null;
            HasConnection = false;
            PublicIPv4 = "";
            PrivateIPv4 = "";
            ModuleStarted = false;
            Terminal.WriteLine("[Network] Module finished successfully.");
        }

        #endregion

        #region Server

        public static void StartServer_Click(object sender, EventArgs e)
        {
            if (!ModuleStarted)
            {
                Terminal.WriteLine("[App] The Network module hasn't been enabled, enable it to start a server.");
                return;
            }

            if (Server == null)
                StartServer(sender, e);
            else
                StopServer(sender, e);
        }

        public static void StartServer(object sender, EventArgs e)
        {
            if (Main.ServerPortTextEdit.Text == "")
            {
                Terminal.WriteLine("[App] There is not server port specified, aborting.");
                return;
            }

            bool PortParsed = int.TryParse(Main.ServerPortTextEdit.Text, out int Port);

            if (!PortParsed || Port > 65535 || Port < 0)
            {
                Terminal.WriteLine("[App] Invalid port, please check your inputs.");
                return;
            }

            Main.ServerPortTextEdit.Enabled = false;

            try
            {
                Server = new WatsonTcpServer(PrivateIPv4, Port);
                Server.Events.ClientConnected += Server_ClientConnected;
                Server.Events.ClientDisconnected += Server_ClientDisconnected;
                Server.Events.MessageReceived += Server_MessageReceived;
                Server.Events.ServerStarted += Server_ServerStarted;
                Server.Events.ServerStopped += Server_ServerStopped;
                Server.Callbacks.SyncRequestReceived = Server_SyncRequestReceived;

                Server.Settings.Logger = Logger;
                Server.Settings.DebugMessages = true;
                Server.Settings.MaxConnections = 3;

                Server.Keepalive.EnableTcpKeepAlives = true;
                Server.Keepalive.TcpKeepAliveInterval = 1;
                Server.Keepalive.TcpKeepAliveTime = 1;
                Server.Keepalive.TcpKeepAliveRetryCount = 3;
                Server.Start();
            }
            catch (Exception Ex)
            {
                Server?.Dispose();
                Server = null;
                Main.ServerPortTextEdit.Enabled = true;
                Terminal.WriteLine($"[Server] {Ex.Message}");
                return;
            }

            Main.StartServerButton.Text = "Close";
            Main.ServerStatusTextEdit.Text = $"Server sucessfully hosted at: {PrivateIPv4}:{Port}";
        }

        public static void StopServer(object sender, EventArgs e)
        {
            Server?.DisconnectClients();
            Server?.Dispose();
            Server = null;

            Main.ServerPortTextEdit.Enabled = true;
            Main.StartServerButton.Text = "Open";
            Main.ServerStatusTextEdit.Text = "Server offline";
        }

        private static async void Server_ClientConnected(object sender, ConnectionEventArgs args)
        {
            Terminal.WriteLine($"[Server] New client connected: {args.IpPort}");

            string PlayerName = args.IpPort;

            try
            {
                Terminal.WriteLine($"[Server] Sending PlayerName request to {args.IpPort}, waiting response.");
                SyncResponse PlayerNameRequest = await Task.Run(() => Server.SendAndWait(5000, args.IpPort, "[NICKNAME]"));
                PlayerName = Encoding.UTF8.GetString(PlayerNameRequest.Data);
                Terminal.WriteLine($"[Server] PlayerName received from {args.IpPort}.");
            }
            catch(Exception)
            {
                Terminal.WriteLine($"[Server] PlayerName response from {args.IpPort} timed out, going with defaults.");
            }

            LabelControl[] ClientNames =
            {
                Main.P1ClientLabelControl,
                Main.P2ClientLabelControl,
                Main.P3ClientLabelControl
            };

            SimpleButton[] DropButtons =
            {
                Main.P1DropSimpleButton,
                Main.P2DropSimpleButton,
                Main.P3DropSimpleButton,
            };

            for (int i = 0; i < 3; i++)
            {
                if (BuddyClients[i] == null)
                {
                    BuddyClients[i] = new ClientConnected() { IP = args.IpPort, Index = i };
                    ClientNames[i].Text = PlayerName;
                    DropButtons[i].Enabled = true;
                    break;
                }
            }
        }

        private static void Server_ClientDisconnected(object sender, DisconnectionEventArgs args)
        {
            LabelControl[] ClientNames =
            {
                Main.P1ClientLabelControl,
                Main.P2ClientLabelControl,
                Main.P3ClientLabelControl
            };

            SimpleButton[] DropButtons =
            {
                Main.P1DropSimpleButton,
                Main.P2DropSimpleButton,
                Main.P3DropSimpleButton,
            };

            for (int i = 0; i < 3; i++)
            {
                if (BuddyClients[i] != null && BuddyClients[i].IP == args.IpPort)
                {
                    BuddyClients[i] = null;
                    ClientNames[i].Text = "No client connected";
                    DropButtons[i].Enabled = false;
                }
            }

            Terminal.WriteLine($"[Server] The client {args.IpPort} has been disconnected, reason: {args.Reason}");
        }

        private static void Server_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            if (args.Data != null)
            {
                string Decoded = Encoding.UTF8.GetString(args.Data);

                if (Decoded.Contains("[CHAT]"))
                {
                    Server_BroadcastMessage(Decoded, args.IpPort, true);
                }
                else if (Decoded.Contains("[NAME]"))
                {
                    Decoded = Decoded.Replace("[NAME]", "");

                    LabelControl[] ClientNames =
                    {
                        Main.P1ClientLabelControl,
                        Main.P2ClientLabelControl,
                        Main.P3ClientLabelControl
                    };

                    for (int i = 0; i < 3; i++)
                    {
                        if (BuddyClients[i] != null && BuddyClients[i].IP == args.IpPort)
                        {
                            ClientNames[i].Text = Decoded;
                        }
                    }
                }
                else
                {
                    Terminal.WriteLine($"[Server] Message received from {args.IpPort}: {Decoded}");
                }

                return;
            }

            Terminal.WriteLine($"[Server] Empty or unknown message received from {args.IpPort}.");
        }

        private static SyncResponse Server_SyncRequestReceived(SyncRequest req)
        {
            Terminal.WriteLine($"[Server] Empty or unknown request received from {req.IpPort}, sending empty response back.");
            return new SyncResponse(req, "");
        }

        private static void Server_ServerStarted(object sender, EventArgs args)
        {
            Terminal.WriteLine("[Server] Successfully started.");
        }

        private static void Server_ServerStopped(object sender, EventArgs args)
        {
            Terminal.WriteLine("[Server] Successfully shutted down.");
        }

        public static void Server_DropClient(object sender, EventArgs e)
        {
            if (Server == null)
                return;

            SimpleButton SB = sender as SimpleButton;
            int index = int.Parse(SB.Name[1].ToString()) - 1;

            Server.DisconnectClient(BuddyClients[index].IP);
            Terminal.WriteLine($"[Server] Dropping client {BuddyClients[index].IP}.");

            LabelControl[] ClientNames =
            {
                Main.P1ClientLabelControl,
                Main.P2ClientLabelControl,
                Main.P3ClientLabelControl
            };

            SimpleButton[] DropButtons =
            {
                Main.P1DropSimpleButton,
                Main.P2DropSimpleButton,
                Main.P3DropSimpleButton,
            };

            BuddyClients[index] = null;
            ClientNames[index].Text = "No client connected";
            DropButtons[index].Enabled = false;
        }

        #endregion

        #region Client

        public static async void StartClient_Click(object sender, EventArgs e)
        {
            if (!ModuleStarted)
            {
                Terminal.WriteLine("[App] The Network module hasn't been enabled, enable it to start a server.");
                return;
            }

            if (BuddyServer == null)
                await StartClient(sender, e);
            else
                StopClient(sender, e);
        }

        public static async Task StartClient(object sender, EventArgs e)
        {
            SimpleButton CB = sender as SimpleButton;

            if (Main.BuddyServerIPTextEdit.Text == "")
            {
                Terminal.WriteLine("[App] No IP address specified, aborting.");
                return;
            }

            if (!IPAddress.TryParse(Main.BuddyServerIPTextEdit.Text, out IPAddress IP))
            {
                Terminal.WriteLine("[App] Invalid IP specified, please check your inputs.");
                return;
            }

            bool PortParsed = int.TryParse(Main.BuddyServerPortTextEdit.Text, out int Port);

            if (!PortParsed || Port > 65535 || Port < 0)
            {
                Terminal.WriteLine("[App] Invalid port, please check your inputs.");
                return;
            }

            if (Server != null)
            {
                if (Main.BuddyServerIPTextEdit.Text == PrivateIPv4 && Main.BuddyServerPortTextEdit.Text == Main.ServerPortTextEdit.Text)
                {
                    Terminal.WriteLine("[App] You cannot join yourself, please specify another IP:Port combination.");
                    return;
                }
            }

            try
            {
                BuddyServer = new ServerConnected() { Connector = new WatsonTcpClient(IP.ToString(), Port), IP = $"{IP}:{Port}" };
                BuddyServer.Connector.Events.ServerConnected += Client_ServerConnected;
                BuddyServer.Connector.Events.ServerDisconnected += Client_ServerDisconnected;
                BuddyServer.Connector.Events.MessageReceived += Client_MessageReceived;
                BuddyServer.Connector.Callbacks.SyncRequestReceived = Client_SyncRequestReceived;

                BuddyServer.Connector.Settings.DebugMessages = true;
                BuddyServer.Connector.Settings.Logger = Logger;

                BuddyServer.Connector.Keepalive.EnableTcpKeepAlives = true;
                BuddyServer.Connector.Keepalive.TcpKeepAliveInterval = 1;
                BuddyServer.Connector.Keepalive.TcpKeepAliveTime = 1;
                BuddyServer.Connector.Keepalive.TcpKeepAliveRetryCount = 3;

                CB.Enabled = false;
                CB.Text = "Connecting";

                await Task.Run(() => BuddyServer.Connector.Connect());
            }
            catch (Exception Ex)
            {
                CB.Enabled = true;
                CB.Text = "Connect";
                BuddyServer?.Connector?.Dispose();
                BuddyServer = null;
                Terminal.WriteLine($"[Client] {Ex.Message}");
                return;
            }

            CB.Enabled = true;
            CB.Text = "Disconnect";
        }

        public static void StopClient(object sender, EventArgs e)
        {
            SimpleButton CB = sender as SimpleButton;

            string IPPort = BuddyServer?.IP;

            CB.Enabled = false;

            BuddyServer?.Connector?.Dispose();
            BuddyServer = null;

            CB.Text = "Connect";
            CB.Enabled = true;

            Terminal.WriteLine($"[Client] Sucessfully disconnected from {IPPort}.");
        }

        private static void Client_ServerConnected(object sender, ConnectionEventArgs args)
        {
            Terminal.WriteLine($"[Client] Sucessfully connected to {args.IpPort}.");
        }

        private static void Client_ServerDisconnected(object sender, DisconnectionEventArgs args)
        {
            if (BuddyServer != null && BuddyServer.Connector != null && !BuddyServer.Connector.Connected)
            {
                BuddyServer?.Connector?.Dispose();
                BuddyServer = null;
                Main.BuddyServerConnectionButton.Text = "Connect";
            }

            Terminal.WriteLine($"[Client] Disconnected from {args.IpPort}, reason: {args.Reason}");
        }

        private static void Client_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            if (args.Data != null)
            {
                string Decoded = Encoding.UTF8.GetString(args.Data);

                if (Decoded.Contains("[CHAT]"))
                {
                    Decoded = Decoded.Replace("[CHAT]", "");
                    Terminal.WriteMessage(Decoded, (int)Terminal.ConsoleInterface.Client);
                    return;
                }
            }

            Terminal.WriteLine($"[Client] Empty or unknown message received from {args.IpPort}.");
        }

        private static SyncResponse Client_SyncRequestReceived(SyncRequest req)
        {
            if (req.Data != null)
            {
                string Decoded = Encoding.UTF8.GetString(req.Data);

                if (Decoded == "[NICKNAME]")
                {
                    Terminal.WriteLine($"[Client] PlayerName request received from {req.IpPort}, sending response back.");
                    return new SyncResponse(req, Main.PlayerNameTextEdit.Text);
                }
            }

            Terminal.WriteLine($"[Client] Empty or unknown request received from {req.IpPort}, sending empty response back.");
            return new SyncResponse(req, "");
        }

        #endregion

        #region Shared

        private static void Logger(Severity sev, string msg)
        {
            
        }

        #endregion

        #region Senders

        public static void Server_BroadcastMessage(string Message, string IPException = "", bool WriteInTheChat = false, bool SuppressTerminal = false)
        {
            if (!ModuleStarted || Server == null)
            {
                Terminal.WriteLine("[App] The server is offline, send request ignored.");
                return;
            }

            List<string> clients = Server.ListClients().ToList();

            if (clients.Count == 0)
            {
                Terminal.WriteLine("[App] No clients connected, send request ignored.");
                return;
            }

            byte[] Encoded = Encoding.UTF8.GetBytes(Message);
            int MessagesSent = 0;

            foreach (string Cli in clients)
            {
                if (IPException != "" && Cli == IPException)
                    continue;

                Server.Send(Cli, Encoded);
                MessagesSent++;
            }

            if (WriteInTheChat)
            {
                if (Message.Contains("[CHAT]"))
                    Message = Message.Replace("[CHAT]", "");

                Terminal.WriteMessage(Message, (int)Terminal.ConsoleInterface.Server);
            }

            if (SuppressTerminal || MessagesSent == 0)
                return;

            Terminal.WriteLine($"[Server] Message sucessfully sent to {clients.Count} client{(clients.Count > 1 ? "s" : "")}.");
        }

        public static void Server_SendMessage(string IPPort, string Message, bool SuppressTerminal = false)
        {
            if (!ModuleStarted || Server == null)
            {
                Terminal.WriteLine("[App] The server is offline, send request ignored.");
                return;
            }

            byte[] Encoded = Encoding.UTF8.GetBytes(Message);
            Server.Send(IPPort, Encoded);

            if (SuppressTerminal)
                return;

            Terminal.WriteLine($"[Server] Message sucessfully sent to {IPPort}.");
        }

        public static void Client_SendMessage(string Message, bool WriteInTheChat = false, bool SuppressTerminal = false)
        {
            if (!ModuleStarted)
            {
                Terminal.WriteLine("[App] The Network module isn't running, send request ignored.");
                return;
            }

            if (BuddyServer == null)
            {
                Terminal.WriteLine("[App] No connection found at the specified server.");
                return;
            }

            byte[] Encoded = Encoding.UTF8.GetBytes(Message);
            BuddyServer.Connector.Send(Encoded);

            if (WriteInTheChat)
            {
                if (Message.Contains("[CHAT]"))
                    Message = Message.Replace("[CHAT]", "");

                Terminal.WriteMessage(Message, (int)Terminal.ConsoleInterface.Client);
            }

            if (SuppressTerminal)
                return;

            Terminal.WriteLine($"[Client] Message sucessfully sent to the Server.");
        }

        #endregion
    }
}
