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
        public int Player { get; set; }
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
                BuddyClients = new ClientConnected[3];
                PublicIPv4 = MachinePublicIP();
                PrivateIPv4 = MachinePrivateIP();
                ModuleStarted = true;
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
                Server = new WatsonTcpServer(PrivateIPv4, Port);
                Server.Events.ClientConnected += Server_ClientConnected;
                Server.Events.ClientDisconnected += Server_ClientDisconnected;
                Server.Events.MessageReceived += Server_MessageReceived;
                Server.Events.ServerStarted += Server_ServerStarted;
                Server.Events.ServerStopped += Server_ServerStopped;
                Server.Callbacks.SyncRequestReceived = SyncRequestReceived;

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
                Terminal.WriteLine(Ex.Message);
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

            Main.StartServerButton.Text = "Open";
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
                    BuddyClients[i] = new ClientConnected() { IP = args.IpPort, Index = i, Player = (PlayerIndexes[i].SelectedItem as ListItem).Value };
                    ClientNames[i].Text = args.IpPort;
                    DropButtons[i].Enabled = true;
                    break;
                }
            }

            Terminal.WriteLine($"[SERVER] Client connected: {args.IpPort}");
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

            Terminal.WriteLine($"[SERVER] Client disconnected: {args.IpPort} - Reason: {args.Reason}");
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
                    Terminal.WriteLine($"[SERVER] Message received from {args.IpPort}: {Decoded}");
                }

                return;
            }

            Terminal.WriteLine($"[SERVER] Empty message received from {args.IpPort}.");
        }

        private static void Server_ServerStarted(object sender, EventArgs args)
        {
            Terminal.WriteLine("Server started.");
        }

        private static void Server_ServerStopped(object sender, EventArgs args)
        {
            Terminal.WriteLine("Server closed.");
        }

        public static void DropClient_Click(object sender, EventArgs e)
        {
            if (Server == null)
                return;

            SimpleButton SB = sender as SimpleButton;
            int index = int.Parse(SB.Name[1].ToString()) - 1;

            Server.DisconnectClient(BuddyClients[index].IP);
            Terminal.WriteLine($"[SERVER] Dropping client: {BuddyClients[index].IP}");

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

        /* Client */

        #region CLIENT

        public static async void StartClient_Click(object sender, EventArgs e)
        {
            if (!ModuleStarted)
            {
                Terminal.WriteLine("The Network module hasn't been enabled, enable it to start a server.");
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
                Terminal.WriteLine("No IP address specified, aborting.");
                return;
            }

            if (!IPAddress.TryParse(Main.BuddyServerIPTextEdit.Text, out IPAddress IP))
            {
                Terminal.WriteLine("Invalid IP specified, please check your inputs.");
                return;
            }

            bool PortParsed = int.TryParse(Main.BuddyServerPortTextEdit.Text, out int Port);

            if (!PortParsed || Port > 65535 || Port < 0)
            {
                Terminal.WriteLine("Invalid port, please check your inputs.");
                return;
            }

            try
            {
                BuddyServer = new ServerConnected() { Connector = new WatsonTcpClient(IP.ToString(), Port), IP = $"{IP}:{Port}" };
                BuddyServer.Connector.Events.ServerConnected += Client_ServerConnected;
                BuddyServer.Connector.Events.ServerDisconnected += Client_ServerDisconnected;
                BuddyServer.Connector.Events.MessageReceived += Client_MessageReceived;
                BuddyServer.Connector.Callbacks.SyncRequestReceived = SyncRequestReceived;

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
                Terminal.WriteLine(Ex.Message);
                return;
            }

            CB.Enabled = true;
            CB.Text = "Disconnect";
        }

        public static void StopClient(object sender, EventArgs e)
        {
            SimpleButton CB = sender as SimpleButton;

            BuddyServer?.Connector?.Dispose();
            BuddyServer = null;

            CB.Text = "Connect";
        }

        private static void Client_ServerConnected(object sender, ConnectionEventArgs args)
        {
            Client_SendMessage($"[NAME]{Main.PlayerNameTextEdit.Text}");
            Terminal.WriteLine($"[CLIENT] Sucessfully connected to: {args.IpPort}");
        }

        private static void Client_ServerDisconnected(object sender, DisconnectionEventArgs args)
        {
            if (BuddyServer != null && BuddyServer.Connector != null && !BuddyServer.Connector.Connected)
            {
                BuddyServer?.Connector?.Dispose();
                BuddyServer = null;
                Main.BuddyServerConnectionButton.Text = "Connect";
            }

            Terminal.WriteLine($"[CLIENT] Disconnected from: {args.IpPort} - Reason: {args.Reason}");
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
                }
                else
                {
                    Terminal.WriteLine($"Message received from {args.IpPort}: {Decoded}");
                }

                return;
            }

            Terminal.WriteLine($"[CLIENT] Empty message received from {args.IpPort}.");
        }


        #endregion

        /* Shared */

        #region SHARED

        private static void Logger(Severity sev, string msg)
        {
            
        }

        private static SyncResponse SyncRequestReceived(SyncRequest req)
        {
            Terminal.WriteLine("Synchronous request received from " + req.IpPort + ": ");
            if (req.Data != null) Terminal.WriteLine(Encoding.UTF8.GetString(req.Data));
            else Terminal.WriteLine("[null]");

            if (req.Metadata != null && req.Metadata.Count > 0)
            {
                Terminal.WriteLine("Metadata:");
                foreach (KeyValuePair<object, object> curr in req.Metadata)
                {
                    Terminal.WriteLine("  " + curr.Key.ToString() + ": " + curr.Value.ToString());
                }
            }

            Dictionary<object, object> retMetadata = new Dictionary<object, object>();
            retMetadata.Add("foo", "bar");
            retMetadata.Add("bar", "baz");

            return new SyncResponse(req, retMetadata, "Here is your response!");
        }

        #endregion

        /* Senders */

        #region SENDERS

        public static void Server_BroadcastMessage(string Message, string IPException = "", bool WriteInTheChat = false, bool SuppressTerminal = false)
        {
            if (!ModuleStarted || Server == null)
            {
                Terminal.WriteLine("The server isn't running, send request ignored.");
                return;
            }

            List<string> clients = Server.ListClients().ToList();

            if (clients.Count == 0)
            {
                Terminal.WriteLine("No clients connected, send request ignored.");
                return;
            }

            byte[] Encoded = Encoding.UTF8.GetBytes(Message);

            foreach (string Cli in clients)
            {
                if (IPException != "" && Cli == IPException)
                    continue;

                Server.Send(Cli, Encoded);
            }

            if (WriteInTheChat)
            {
                if (Message.Contains("[CHAT]"))
                    Message = Message.Replace("[CHAT]", "");

                Terminal.WriteMessage(Message, (int)Terminal.ConsoleInterface.Server);
            }

            if (SuppressTerminal)
                return;

            Terminal.WriteLine($"[SERVER] Message sucessfully sent to {clients.Count} client{(clients.Count > 1 ? "s" : "")}.");
        }

        public static void Server_SendMessage(string IPPort, string Message, bool SuppressTerminal = false)
        {
            if (!ModuleStarted || Server == null)
            {
                Terminal.WriteLine("The server isn't running, send request ignored.");
                return;
            }

            byte[] Encoded = Encoding.UTF8.GetBytes(Message);
            Server.Send(IPPort, Encoded);

            if (SuppressTerminal)
                return;

            Terminal.WriteLine($"[SERVER] Message sucessfully sent to: {IPPort}");
        }

        public static void Client_SendMessage(string Message, bool WriteInTheChat = false, bool SuppressTerminal = false)
        {
            if (!ModuleStarted)
            {
                Terminal.WriteLine("The Network module isn't running, send request ignored.");
                return;
            }

            if (BuddyServer == null)
            {
                Terminal.WriteLine("No connection found at the specified index.");
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

            Terminal.WriteLine($"[CLIENT] Message sucessfully sent to the Server.");
        }

        #endregion
    }
}
