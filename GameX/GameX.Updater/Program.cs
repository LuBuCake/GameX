using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameX.Updater.Database.Type;
using GameX.Updater.Helpers;
using Spectre.Console;

namespace GameX.Updater
{
    internal class Program
    {
        static string _log = "";

        static void CreateTestRequest()
        {
            string AppDirectory = Directory.GetCurrentDirectory();
            string UpdaterDirectory = AppDirectory + "/updater/";

            if (!Directory.Exists(UpdaterDirectory))
                Directory.CreateDirectory(UpdaterDirectory);

            AppVersion _version = new AppVersion()
            {
                FileRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Launcher.x86/latest.zip",
                StartLauncher = "x86"
            };

            Serializer.WriteDataFile(UpdaterDirectory + "updateapp.json", Serializer.Serialize(_version));
        }

        static bool TestConnection(string HostNameOrAddress, int Timeout = 1000)
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

        static void WriteLine(string Message)
        {
            string newline = "[grey]LOG:[/] " + Message;
            string newlogline = $"LOG: {Message}";

            AnsiConsole.MarkupLine(newline);

            _log = string.IsNullOrEmpty(_log) ? newlogline : _log + Environment.NewLine + newlogline;
        }

        static void SaveLog()
        {
            Serializer.WriteDataFile(Directory.GetCurrentDirectory() + "/Updater.log.txt", _log);
        }

        static void Main(string[] args)
        {
            CreateTestRequest();

            AnsiConsole.Status().Start("FETCH", status =>
            {
                status.Spinner(Spinner.Known.SimpleDotsScrolling);
                status.SpinnerStyle(Style.Parse("aqua"));

                status.Status("Initializing");
                Thread.Sleep(500);

                WriteLine("Initialized");

                status.Status("Testing connection");
                Thread.Sleep(500);

                bool HasConnection = TestConnection("8.8.8.8");

                if (!HasConnection)
                {
                    WriteLine("Connection not found, exiting");
                    Thread.Sleep(500);

                    SaveLog();
                    Environment.Exit(0);
                }

                WriteLine("Connection found");

                status.Status("Searching for update requests");
                Thread.Sleep(500);

                string AppDirectory = Directory.GetCurrentDirectory();
                string UpdaterDirectory = AppDirectory + "/updater/";

                if (!Directory.Exists(UpdaterDirectory) || !File.Exists(UpdaterDirectory + "updateapp.json"))
                {
                    WriteLine("No update request found, exiting");
                    Thread.Sleep(500);

                    SaveLog();
                    Environment.Exit(0);
                }

                WriteLine("Update request found");

                status.Status("Building route");
                Thread.Sleep(500);

                status.Status("Fetching data");
                Thread.Sleep(2000);
            });

            Thread.Sleep(2000);
        }
    }
}
