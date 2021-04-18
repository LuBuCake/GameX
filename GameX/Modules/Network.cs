using DevExpress.XtraEditors;
using GameX.Types;
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
        public static WatsonTcpClient[] Client { get; set; }
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
                Client = new WatsonTcpClient[3];
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
            if (Server != null)
            {
                StopServer(null, null);
            }

            for (int i = 0; i < 3; i++)
            {
                if (Client[i] != null)
                {
                    Client[i].Dispose();
                    Client[i] = null;
                }
            }

            Main = null;
            HasConnection = false;
            Clients = null;
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
                Server = new WatsonTcpServer("127.0.0.1", Port);
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
            }
            catch (Exception Ex)
            {
                Server?.Dispose();
                Server = null;
                Terminal.WriteLine(Ex.Message);
                return;
            }

            Server.Start();

            Main.StartServerButton.Text = "Close";
            Main.ServerStatusTextEdit.Text = $"Server sucessfully hosted at: 127.0.0.1:{Port}";
        }

        public static void StopServer(object sender, EventArgs e)
        {
            Server.DisconnectClients();
            Server.Dispose();
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
                if (Clients[i] == null)
                {
                    Clients[i] = new ConnectedClient() { IP = args.IpPort, Index = i, Player = (PlayerIndexes[i].SelectedItem as ListItem).Value };
                    ClientNames[i].Text = args.IpPort;
                    DropButtons[i].Enabled = true;
                    break;
                }
            }

            Terminal.WriteLine("[SERVER] Client connected: " + args.IpPort);
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
                if (Clients[i] != null && Clients[i].IP == args.IpPort)
                {
                    Clients[i] = null;
                    ClientNames[i].Text = "No client connected";
                    DropButtons[i].Enabled = false;
                }
            }

            Terminal.WriteLine($"[SERVER] Client disconnected: {args.IpPort}");
            Terminal.WriteLine($"[SERVER] Disconnect reason: {args.Reason}");
        }

        private static void Server_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Terminal.WriteLine("Message from " + args.IpPort + ": ");
            if (args.Data != null) Terminal.WriteLine(Encoding.UTF8.GetString(args.Data));
            else Terminal.WriteLine("[null]");

            if (args.Metadata != null && args.Metadata.Count > 0)
            {
                Terminal.WriteLine("Metadata:");
                foreach (KeyValuePair<object, object> curr in args.Metadata)
                {
                    Terminal.WriteLine("  " + curr.Key.ToString() + ": " + curr.Value.ToString());
                }
            }
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

            Server.DisconnectClient(Clients[index].IP);
            Terminal.WriteLine($"[SERVER] Dropping client: {Clients[index].IP}");

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

            Clients[index] = null;
            ClientNames[index].Text = "No client connected";
            DropButtons[index].Enabled = false;
        }

        #endregion

        /* Client */

        #region CLIENT

        public static void StartClient_Click(object sender, EventArgs e)
        {
            if (!ModuleStarted)
            {
                Terminal.WriteLine("The Network module hasn't been enabled, enable it to start a server.");
                return;
            }

            SimpleButton CB = sender as SimpleButton;
            int index = int.Parse(CB.Name[1].ToString()) - 1;

            if (Client[index] == null)
                StartClient(sender, e);
            else
                StopClient(sender, e);
        }

        public static void StartClient(object sender, EventArgs e)
        {
            SimpleButton CB = sender as SimpleButton;
            int index = int.Parse(CB.Name[1].ToString()) - 1;

            TextEdit[] IPs =
            {
                Main.P1IPTextEdit,
                Main.P2IPTextEdit,
                Main.P3IPTextEdit
            };

            TextEdit[] Ports =
            {
                Main.P1PortTextEdit,
                Main.P2PortTextEdit,
                Main.P3PortTextEdit
            };

            if (IPs[index].Text == "")
            {
                Terminal.WriteLine("No IP address specified, aborting.");
                return;
            }

            if (!IPAddress.TryParse(IPs[index].Text, out IPAddress IP))
            {
                Terminal.WriteLine("Invalid IP specified, please check your inputs.");
                return;
            }

            bool PortParsed = int.TryParse(Ports[index].Text, out int Port);

            if (!PortParsed || Port > 65535 || Port < 0)
            {
                Terminal.WriteLine("Invalid port, please check your inputs.");
                return;
            }

            try
            {
                Client[index] = new WatsonTcpClient(IP.ToString(), Port);
                Client[index].Events.ServerConnected += Client_ServerConnected;
                Client[index].Events.ServerDisconnected += Client_ServerDisconnected;
                Client[index].Events.MessageReceived += Client_MessageReceived;
                Client[index].Callbacks.SyncRequestReceived = SyncRequestReceived;

                Client[index].Settings.DebugMessages = true;
                Client[index].Settings.Logger = Logger;

                Client[index].Keepalive.EnableTcpKeepAlives = true;
                Client[index].Keepalive.TcpKeepAliveInterval = 1;
                Client[index].Keepalive.TcpKeepAliveTime = 1;
                Client[index].Keepalive.TcpKeepAliveRetryCount = 3;

            }
            catch (Exception Ex)
            {
                Client[index]?.Dispose();
                Client[index] = null;
                Terminal.WriteLine(Ex.Message);
                return;
            }

            Client[index].Connect();
            CB.Text = "Disconnect";
        }

        public static void StopClient(object sender, EventArgs e)
        {
            SimpleButton CB = sender as SimpleButton;
            int index = int.Parse(CB.Name[1].ToString()) - 1;

            Client[index].Dispose();
            Client[index] = null;

            CB.Text = "Connect";
        }

        private static void Client_ServerConnected(object sender, ConnectionEventArgs args)
        {
            Terminal.WriteLine($"[CLIENT] Connected to: {args.IpPort}");
        }

        private static void Client_ServerDisconnected(object sender, DisconnectionEventArgs args)
        {
            SimpleButton[] Connectbuttons =
            {
                Main.P1ConnectionButton,
                Main.P2ConnectionButton,
                Main.P3ConnectionButton
            };

            for (int i = 0; i < 3; i++)
            {
                if (Client[i] != null && !Client[i].Connected)
                {
                    Client[i].Dispose();
                    Client[i] = null;
                    Connectbuttons[i].Text = "Connect";
                }
            }

            Terminal.WriteLine($"[CLIENT] Disconnected from: {args.IpPort}");
            Terminal.WriteLine($"[CLIENT] Disconnect reason: {args.Reason}");
        }

        private static void Client_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Terminal.WriteLine("Message from " + args.IpPort + ": ");
            if (args.Data != null) Terminal.WriteLine(Encoding.UTF8.GetString(args.Data));
            else Terminal.WriteLine("[null]");

            if (args.Metadata != null && args.Metadata.Count > 0)
            {
                Terminal.WriteLine("Metadata:");
                foreach (KeyValuePair<object, object> curr in args.Metadata)
                {
                    Terminal.WriteLine("  " + curr.Key.ToString() + ": " + curr.Value.ToString());
                }
            }
        }


        #endregion

        /* Shared */

        #region SHARED

        private static void Logger(Severity sev, string msg)
        {
            /*
            if (sev.ToString().ToLower() == "error")
                return;

            Terminal.WriteLine("[ " + sev.ToString().ToUpper() + " ] " + msg);
            */
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
    }
}
