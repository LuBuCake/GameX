using DevExpress.XtraEditors;
using GameX.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace GameX.Modules
{
    public class Peer
    {
        public TcpClient Client { get; set; }
        public IPAddress IP { get; set; }
        public int Port { get; set; }
        public int Index { get; set; }
        public int Player { get; set; }
    }

    public class Network
    {
        private static App Main { get; set; }
        private static TcpListener Server { get; set; }
        public static Peer[] Peers { get; private set; }
        public static string PublicIPv4 { get; private set; }
        public static string PrivateIPv4 { get; private set; }
        public static bool HasConnection { get; private set; }

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

        public static string[] MachinePrivateIP()
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
            return ipAddrList.ToArray();
        }

        public static async Task Start(App GameXRef)
        {
            Main = GameXRef;
            HasConnection = await Task.Run(() => TestConnection("google.com"));

            if (HasConnection)
            {
                Peers = new Peer[3];
                PublicIPv4 = MachinePublicIP();
                PrivateIPv4 = MachinePrivateIP().FirstOrDefault();
                return;
            }

            Terminal.WriteLine("No connection found, check your internet connection and restart GameX.");
        }

        public static async Task Update()
        {
            if (!HasConnection || Server == null)
                return;

            TcpClient Client = Server.AcceptTcpClient();
            await Task.Run(() => HandleClient(Client));
        }

        public static void StartServer(int Port)
        {
            Server = new TcpListener(IPAddress.Parse(PrivateIPv4), Port);
            Server.Start();

            Terminal.WriteLine($"Server started at {PrivateIPv4}:{Port}");
        }

        public static void StopServer()
        {
            if (!HasConnection || Server == null)
            {
                Terminal.WriteLine("There is no server to stop, skipping.");
                return;
            }

            Server.Stop();
            Server = null;

            foreach (Peer peer in Peers)
            {
                if (peer != null && peer.Client != null)
                    peer.Client.Close();
            }

            Terminal.WriteLine("Server closed sucessfully.");
        }

        public static async Task HandleClient(TcpClient Client)
        {
            NetworkStream DataStream = Client.GetStream();
        }

        public static async void Connect_Click(object sender, EventArgs e)
        {
            if (!HasConnection || Server == null)
            {
                Terminal.WriteLine("Server is not up, start one using the command: StartServer:Port");
                return;
            }

            TextEdit[] IPBoxes =
            {
                Main.P1IPTextEdit,
                Main.P2IPTextEdit,
                Main.P3IPTextEdit
            };

            SimpleButton[] ConnectButtons =
            {
                Main.P1ConnectionButton,
                Main.P2ConnectionButton,
                Main.P3ConnectionButton
            };

            ComboBoxEdit[] PlayerIndexes =
            {
                Main.P1PlayerIndexComboBoxEdit,
                Main.P2PlayerIndexComboBoxEdit,
                Main.P3PlayerIndexComboBoxEdit
            };

            SimpleButton SB = sender as SimpleButton;
            int Index = int.Parse(SB.Name[1].ToString()) - 1;

            string[] Address = IPBoxes[Index].Text.Split(':');

            if (!IPAddress.TryParse(Address[0], out IPAddress IPv4))
            {
                Terminal.WriteLine($"Peer {Index + 1} has a invalid IP address, your input should follow IPv4:Port.");
                return;
            }

            if (!int.TryParse(Address[1], out int Port))
            {
                Terminal.WriteLine($"Peer {Index + 1} has a invalid port, your input should follow IPv4:Port.");
                return;
            }

            ConnectButtons[Index].Text = "Pinging";
            ConnectButtons[Index].Enabled = false;

            Terminal.WriteLine($"Pinging {IPv4} for response.");

            bool ConnectionSucess = await Task.Run(() => TestConnection(IPv4.ToString()));

            if (ConnectionSucess)
                Terminal.WriteLine($"Response received from {IPv4}, trying connection now.");
            else
            {
                Terminal.WriteLine($"No response received from {IPv4}, check if the IP address is correct.");
                ConnectButtons[Index].Text = "Connect";
                ConnectButtons[Index].Enabled = true;
                return;
            }

            ConnectButtons[Index].Text = "Connecting";

            try
            {
                TcpClient Client = new TcpClient();
                await Task.Run(() => Client.Connect(IPv4, Port));

                Peers[Index] = new Peer() { Client = Client, IP = IPv4, Port = Port, Index = Index, Player = (PlayerIndexes[Index].SelectedItem as ListItem).Value };

                Terminal.WriteLine($"Sucessfully connected to {IPv4}:{Port}");

                ConnectButtons[Index].Text = "Disconnect";
                ConnectButtons[Index].Click += Disconnect_Click;
                ConnectButtons[Index].Enabled = true;
            }
            catch(SocketException Ex)
            {
                Terminal.WriteLine($"{Ex.Message}");
                ConnectButtons[Index].Text = "Connect";
                ConnectButtons[Index].Enabled = true;
            }
        }

        public static async void Disconnect_Click(object sender, EventArgs e)
        {

        }
    }
}
