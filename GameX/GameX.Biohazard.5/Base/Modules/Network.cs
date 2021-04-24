using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using GameX.Base.Helpers;
using GameX.Base.Types;
using GameX.Game.Types;
using WatsonTcp;

namespace GameX.Base.Modules
{
    public static class Network
    {
        public static bool ModuleStarted { get; set; }
        private static App Main { get; set; }
        public static WatsonTcpServer TCPServer { get; set; }
        public static Server TCPClient { get; set; }
        public static Client[] BuddyClients { get; private set; }
        public static string[] PrivateIPv4 { get; private set; }
        public static bool HasConnection { get; private set; }

        #region Utility

        public static bool TestConnection(string HostNameOrAddress, int Timeout = 1000)
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

        public static string[] MachinePrivateIP()
        {
            return (from item in NetworkInterface.GetAllNetworkInterfaces()
                where item.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                      item.OperationalStatus == OperationalStatus.Up
                from ip in item.GetIPProperties().UnicastAddresses
                where ip.Address.AddressFamily == AddressFamily.InterNetwork
                select ip.Address.ToString()).ToArray();
        }

        #endregion

        #region Module Manager

        public static async Task StartModule(App GameXRef)
        {
            Main = GameXRef;
            HasConnection = await Task.Run(() => TestConnection("8.8.8.8"));

            if (HasConnection)
            {
                BuddyClients = new Client[3];
                PrivateIPv4 = MachinePrivateIP();
                ModuleStarted = true;
                Terminal.WriteLine("[Network] Module started successfully.");
                return;
            }

            ModuleStarted = false;
        }

        public static void FinishModule()
        {
            if (TCPServer != null)
            {
                StopServer(null, null);
            }

            if (TCPClient != null)
            {
                StopClient(Main.BuddyServerConnectionButton, null);
            }

            Main = null;
            HasConnection = false;
            PrivateIPv4 = null;
            ModuleStarted = false;
            Terminal.WriteLine("[Network] Module finished successfully.");
        }

        #endregion

        #region Server

        public static void StartServer_Click(object sender, EventArgs e)
        {
            if (!ModuleStarted)
            {
                Terminal.WriteLine("[App] The Network module hasn't been enabled, enable it to start a server.",
                    Enums.MessageBoxType.Error);
                return;
            }

            if (TCPServer == null)
                StartServer(sender, e);
            else
                StopServer(sender, e);
        }

        public static void StartServer(object sender, EventArgs e)
        {
            if (Main.ServerPortTextEdit.Text == "")
            {
                Terminal.WriteLine("[App] Please specify a server port.", Enums.MessageBoxType.Error);
                return;
            }

            bool PortParsed = int.TryParse(Main.ServerPortTextEdit.Text, out int Port);

            if (!PortParsed || Port > 65535 || Port < 0)
            {
                Terminal.WriteLine("[App] Invalid port, specify a port between 1 and 65535.",
                    Enums.MessageBoxType.Error);
                return;
            }

            Main.ServerPortTextEdit.Enabled = false;

            try
            {
                TCPServer = new WatsonTcpServer(null, Port);
                TCPServer.Events.ClientConnected += Server_ClientConnected;
                TCPServer.Events.ClientDisconnected += Server_ClientDisconnected;
                TCPServer.Events.MessageReceived += Server_MessageReceived;
                TCPServer.Events.ServerStarted += Server_ServerStarted;
                TCPServer.Events.ServerStopped += Server_ServerStopped;
                TCPServer.Callbacks.SyncRequestReceived = Server_SyncRequestReceived;

                TCPServer.Settings.Logger = Logger;
                TCPServer.Settings.DebugMessages = true;
                TCPServer.Settings.MaxConnections = 3;

                TCPServer.Keepalive.EnableTcpKeepAlives = true;
                TCPServer.Keepalive.TcpKeepAliveInterval = 1;
                TCPServer.Keepalive.TcpKeepAliveTime = 1;
                TCPServer.Keepalive.TcpKeepAliveRetryCount = 3;
                TCPServer.Start();
            }
            catch (Exception Ex)
            {
                TCPServer?.Dispose();
                TCPServer = null;
                Main.ServerPortTextEdit.Enabled = true;
                Terminal.WriteLine($"[Server] {Ex.Message}");
                return;
            }

            Main.BuddyServerConnectionButton.Enabled = false;

            Main.StartServerButton.Text = "Close";
            Main.ServerStatusTextEdit.Text = $"Server sucessfully hosted at the port: {Port}";
        }

        public static void StopServer(object sender, EventArgs e)
        {
            TCPServer?.DisconnectClients();
            TCPServer?.Dispose();
            TCPServer = null;

            Main.BuddyServerConnectionButton.Enabled = true;

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
                Terminal.WriteLine($"[Server] Sending Player Name request to {args.IpPort}, waiting response.");
                SyncResponse PlayerNameRequest =
                    await Task.Run(() => TCPServer.SendAndWait(5000, args.IpPort, "[PLAYERNAME]"));
                PlayerName = Encoding.UTF8.GetString(PlayerNameRequest.Data);
                Terminal.WriteLine($"[Server] Player Name received from {args.IpPort}.");
            }
            catch (Exception)
            {
                Terminal.WriteLine($"[Server] Player Name response from {args.IpPort} timed out, going with defaults.");
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

            ComboBoxEdit[] CharacterCombos =
            {
                Main.P1CharComboBox,
                Main.P2CharComboBox,
                Main.P3CharComboBox,
                Main.P4CharComboBox
            };

            ComboBoxEdit[] CostumeCombos =
            {
                Main.P1CosComboBox,
                Main.P2CosComboBox,
                Main.P3CosComboBox,
                Main.P4CosComboBox
            };

            for (int i = 0; i < 3; i++)
            {
                if (BuddyClients[i] != null)
                    continue;

                BuddyClients[i] = new Client {IP = args.IpPort, Name = PlayerName, Index = i};

                Threading.SetControlPropertyThreadSafe(ClientNames[i], "Text", PlayerName);
                Threading.SetControlPropertyThreadSafe(DropButtons[i], "Enabled", true);

                for (int j = 0; j < 4; j++)
                {
                    Main.Character_SendChange(j, CharacterCombos[j].SelectedIndex, CostumeCombos[j].SelectedIndex);
                }

                break;
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
                if (BuddyClients[i] == null || BuddyClients[i].IP != args.IpPort)
                    continue;

                BuddyClients[i] = null;
                Threading.SetControlPropertyThreadSafe(ClientNames[i], "Text", "No client connected");
                Threading.SetControlPropertyThreadSafe(DropButtons[i], "Enabled", false);
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
                else if (Decoded.Contains("[CHARCHANGE]"))
                {
                    Decoded = Decoded.Replace("[CHARCHANGE]", "");
                    Client ClientSender = null;
                    NetCharacterChange Change = Serializer.DeserializeCharacterChanged(Decoded);

                    foreach (Client Client in BuddyClients)
                    {
                        if (Client.IP != args.IpPort)
                            continue;

                        ClientSender = Client;
                        break;
                    }

                    Main.Character_ReceiveChange(Change, ClientSender);
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
            Terminal.WriteLine(
                $"[Server] Empty or unknown request received from {req.IpPort}, sending empty response back.");
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
            if (TCPServer == null)
                return;

            SimpleButton SB = sender as SimpleButton;
            int index = int.Parse(SB.Name[1].ToString()) - 1;

            TCPServer.DisconnectClient(BuddyClients[index].IP);
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
            Threading.SetControlPropertyThreadSafe(ClientNames[index], "Text", "No client connected");
            Threading.SetControlPropertyThreadSafe(DropButtons[index], "Enabled", false);
        }

        #endregion

        #region Client

        public static async void StartClient_Click(object sender, EventArgs e)
        {
            if (!ModuleStarted)
            {
                Terminal.WriteLine("[App] The Network module hasn't been enabled, enable it to start a server.",
                    Enums.MessageBoxType.Error);
                return;
            }

            if (TCPClient == null)
                await StartClient(sender, e);
            else
                StopClient(sender, e);
        }

        public static async Task StartClient(object sender, EventArgs e)
        {
            SimpleButton CB = sender as SimpleButton;

            if (Main.BuddyServerIPTextEdit.Text == "")
            {
                Terminal.WriteLine("[App] No IP address specified.", Enums.MessageBoxType.Error);
                return;
            }

            if (Main.BuddyServerPortTextEdit.Text == "")
            {
                Terminal.WriteLine("[App] Please specify a server port.", Enums.MessageBoxType.Error);
                return;
            }

            if (!IPAddress.TryParse(Main.BuddyServerIPTextEdit.Text, out IPAddress IP))
            {
                Terminal.WriteLine("[App] The specified IP is invalid.", Enums.MessageBoxType.Error);
                return;
            }

            if (IP.ToString() == "0.0.0.0" || IP.ToString() == "::0")
            {
                Terminal.WriteLine("[App] The specified IP is invalid.", Enums.MessageBoxType.Error);
                return;
            }

            bool PortParsed = int.TryParse(Main.BuddyServerPortTextEdit.Text, out int Port);

            if (!PortParsed || Port > 65535 || Port < 1)
            {
                Terminal.WriteLine("[App] The specified port is invalid, please specify one between 1 and 65535.",
                    Enums.MessageBoxType.Error);
                return;
            }

            if (TCPServer != null)
            {
                if (Array.Exists(PrivateIPv4, AvailableIP => AvailableIP == Main.BuddyServerIPTextEdit.Text) &&
                    Main.BuddyServerPortTextEdit.Text == Main.ServerPortTextEdit.Text)
                {
                    Terminal.WriteLine(
                        "[Network] You cannot join yourself, please specify another IP:Port combination.",
                        Enums.MessageBoxType.Error);
                    return;
                }
            }

            try
            {
                TCPClient = new Server {Connector = new WatsonTcpClient(IP.ToString(), Port), IP = $"{IP}:{Port}"};
                TCPClient.Connector.Events.ServerConnected += Client_ServerConnected;
                TCPClient.Connector.Events.ServerDisconnected += Client_ServerDisconnected;
                TCPClient.Connector.Events.MessageReceived += Client_MessageReceived;
                TCPClient.Connector.Callbacks.SyncRequestReceived = Client_SyncRequestReceived;

                TCPClient.Connector.Settings.DebugMessages = true;
                TCPClient.Connector.Settings.Logger = Logger;

                TCPClient.Connector.Keepalive.EnableTcpKeepAlives = true;
                TCPClient.Connector.Keepalive.TcpKeepAliveInterval = 1;
                TCPClient.Connector.Keepalive.TcpKeepAliveTime = 1;
                TCPClient.Connector.Keepalive.TcpKeepAliveRetryCount = 3;

                CB.Enabled = false;
                CB.Text = "Connecting";

                await Task.Run(() => TCPClient.Connector.Connect());
            }
            catch (Exception Ex)
            {
                CB.Enabled = true;
                CB.Text = "Connect";
                TCPClient?.Connector?.Dispose();
                TCPClient = null;
                Terminal.WriteLine($"[Client] {Ex.Message}");
                Utility.MessageBox_Information("No connection found at the specified IP:Port.");
                return;
            }

            Main.StartServerButton.Enabled = false;

            CB.Enabled = true;
            CB.Text = "Disconnect";
        }

        public static void StopClient(object sender, EventArgs e)
        {
            SimpleButton CB = sender as SimpleButton;

            string IPPort = TCPClient?.IP;

            CB.Enabled = false;

            TCPClient?.Connector?.Dispose();
            TCPClient = null;

            Main.StartServerButton.Enabled = true;

            CB.Text = "Connect";
            CB.Enabled = true;

            Terminal.WriteLine($"[Client] Sucessfully disconnected from {IPPort}.");
        }

        private static void Client_ServerConnected(object sender, ConnectionEventArgs args)
        {
            Terminal.WriteLine($"[Client] Sucessfully connected to {args.IpPort}.", Enums.MessageBoxType.Information);
        }

        private static void Client_ServerDisconnected(object sender, DisconnectionEventArgs args)
        {
            if (TCPClient != null && TCPClient.Connector != null && !TCPClient.Connector.Connected)
            {
                TCPClient?.Connector?.Dispose();
                TCPClient = null;
                Threading.SetControlPropertyThreadSafe(Main.BuddyServerConnectionButton, "Text", "Connect");
                Threading.SetControlPropertyThreadSafe(Main.StartServerButton, "Enabled", true);
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
                    Terminal.WriteMessage(Decoded, (int) Enums.ConsoleInterface.Client);
                    return;
                }

                if (Decoded.Contains("[CHARCHANGE]"))
                {
                    Decoded = Decoded.Replace("[CHARCHANGE]", "");
                    NetCharacterChange Change = Serializer.DeserializeCharacterChanged(Decoded);
                    Main.Character_ReceiveChange(Change);
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

                if (Decoded == "[PLAYERNAME]")
                {
                    Terminal.WriteLine(
                        $"[Client] Player Name request received from {req.IpPort}, sending response back.");
                    return new SyncResponse(req, Main.PlayerNameTextEdit.Text);
                }
            }

            Terminal.WriteLine(
                $"[Client] Empty or unknown request received from {req.IpPort}, sending empty response back.");
            return new SyncResponse(req, "");
        }

        #endregion

        #region Shared

        private static void Logger(Severity sev, string msg)
        {
        }

        #endregion

        #region Senders

        public static void Server_BroadcastMessage(string Message, string IPException = "", bool WriteInTheChat = false,
            bool SuppressTerminal = false)
        {
            if (!ModuleStarted || TCPServer == null)
            {
                Terminal.WriteLine("[App] The server is offline, send request ignored.");
                return;
            }

            List<string> clients = TCPServer.ListClients().ToList();

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

                TCPServer.Send(Cli, Encoded);
                MessagesSent++;
            }

            if (WriteInTheChat)
            {
                if (Message.Contains("[CHAT]"))
                    Message = Message.Replace("[CHAT]", "");

                Terminal.WriteMessage(Message, (int) Enums.ConsoleInterface.Server);
            }

            if (SuppressTerminal || MessagesSent == 0)
                return;

            Terminal.WriteLine(
                $"[Server] Message sucessfully broadcasted to {clients.Count} client{(clients.Count > 1 ? "s" : "")}.");
        }

        public static void Server_SendMessage(string IPPort, string Message, bool SuppressTerminal = false)
        {
            if (!ModuleStarted || TCPServer == null)
            {
                Terminal.WriteLine("[App] The server is offline, send request ignored.");
                return;
            }

            byte[] Encoded = Encoding.UTF8.GetBytes(Message);
            TCPServer.Send(IPPort, Encoded);

            if (SuppressTerminal)
                return;

            Terminal.WriteLine($"[Server] Message sucessfully sent to {IPPort}.");
        }

        public static void Client_SendMessage(string Message, bool WriteInTheChat = false,
            bool SuppressTerminal = false)
        {
            if (!ModuleStarted)
            {
                Terminal.WriteLine("[App] The Network module isn't running, send request ignored.");
                return;
            }

            if (TCPClient == null)
            {
                Terminal.WriteLine("[App] No connection found at the specified server.");
                return;
            }

            byte[] Encoded = Encoding.UTF8.GetBytes(Message);
            TCPClient.Connector.Send(Encoded);

            if (WriteInTheChat)
            {
                if (Message.Contains("[CHAT]"))
                    Message = Message.Replace("[CHAT]", "");

                Terminal.WriteMessage(Message, (int) Enums.ConsoleInterface.Client);
            }

            if (SuppressTerminal)
                return;

            Terminal.WriteLine("[Client] Message sucessfully sent to the Server.");
        }

        #endregion
    }
}