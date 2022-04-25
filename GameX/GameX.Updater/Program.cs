﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using GameX.Updater.Database.Type;
using GameX.Updater.Helpers;
using Spectre.Console;

namespace GameX.Updater
{
    internal class Program
    {
        static AppVersion _version = null;

        static string _log = "";
        static bool _isdownloading = false;
        static int _downloadprogresspercentage = 0;

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
                    WriteLine("Connection not found");
                    status.Status("Exiting");
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
                    WriteLine("No update request found");
                    status.Status("Exiting");
                    Thread.Sleep(500);

                    SaveLog();
                    Environment.Exit(0);
                }

                WriteLine("Update request found");

                status.Status("Building route");
                Thread.Sleep(500);

                _version = Serializer.Deserialize<AppVersion>(Serializer.ReadDataFile(UpdaterDirectory + "updateapp.json"));

                WriteLine("Route built");

                status.Status("Fetching data");
                Thread.Sleep(500);

                WebClient Downloader = new WebClient();
                Downloader.DownloadProgressChanged += DownloadProgressChanged;
                Downloader.DownloadFileCompleted += DownloadFileCompleted;
                Downloader.DownloadFileAsync(new Uri(_version.FileRoute), UpdaterDirectory + "latest.zip");

                WriteLine("Data fetched");
            });

            _isdownloading = true;

            ProcessDownload().GetAwaiter().GetResult();
        }

        static async Task ProcessDownload()
        {
            await AnsiConsole.Status().StartAsync("DOWNLOAD", async status =>
            {
                status.Spinner(Spinner.Known.SimpleDotsScrolling);
                status.SpinnerStyle(Style.Parse("aqua"));

                await Task.Run(() =>
                {
                    while (_isdownloading)
                    {
                        status.Status($"Downloading update {_downloadprogresspercentage}%");
                    }
                });

                WriteLine("Download finished");

                string AppDirectory = Directory.GetCurrentDirectory();
                string UpdaterDirectory = AppDirectory + "/updater/";

                if (!File.Exists(UpdaterDirectory + "latest.zip"))
                {
                    WriteLine("Download finished but the file has gone missing");
                    status.Status("Exiting");
                    await Task.Delay(500);

                    SaveLog();
                    Environment.Exit(0);
                }

                status.Status("Extracting update");
                await Task.Delay(500);

                await ExtractLatestPackage();

                WriteLine("Finished unpacking");
                WriteLine("All files have been updated");

                status.Status("Exiting");
                await Task.Delay(500);

                SaveLog();

                if (!string.IsNullOrWhiteSpace(_version.StartLauncher))
                {
                    switch (_version.StartLauncher)
                    {
                        case "x86":
                            Process.Start(AppDirectory + "/GameX-x86.exe");
                            break;
                        case "x64":
                            Process.Start(AppDirectory + "/GameX-x64.exe");
                            break;
                    }
                }
            });
        }

        static void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            _downloadprogresspercentage = e.ProgressPercentage;
        }

        static async void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            await Task.Run(() => { _isdownloading = false; });
        }

        static async Task ExtractLatestPackage()
        {
            string AppDirectory = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
            string ZipPath = AppDirectory + "/updater/latest.zip";

            using (ZipArchive archive = ZipFile.OpenRead(ZipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.GetFullPath(Path.Combine(AppDirectory, entry.FullName));

                    if (destinationPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    {
                        if (!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);

                        continue;
                    }

                    try
                    {
                        await Task.Run(() => entry.ExtractToFile(destinationPath, true));
                        WriteLine($"{entry.FullName} extracted");
                    }
                    catch (Exception Ex)
                    {
                        WriteLine($"{entry.FullName} skipped: {Ex.Message}");
                    }
                }
            }

            Directory.Delete(AppDirectory + "/updater/", true);
        }
    }
}
