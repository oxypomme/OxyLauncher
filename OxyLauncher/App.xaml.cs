using OxyLauncher.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Text.RegularExpressions;
using OxyNuggets;
using OxyNuggets.JSON;

namespace OxyLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static LogStream logstream = new LogStream(Path.Combine(Path.GetDirectoryName(Settings.path), "logs.log"));
        public static Settings settings { get; set; }

        public static List<Applet> Applications { get; set; }

        public App()
        {
            try
            {
                logstream.Log("LogStream initialized");

                // Reading or creating the settings
                if (File.Exists(Settings.path))
                    settings = (Settings)JSONSerializer.Deserialize<Settings>(Settings.path);
                else
                {
                    settings = new Settings(Directory.GetParent(Path.GetDirectoryName(Settings.path)).FullName);
                }
                JSONSerializer.Serialize(Settings.path, settings);
                logstream.Log("Settings initialized");

                LoadApplets();
                logstream.Log("Ready");
            }
            catch (Exception e) { logstream.Error(e); }
        }

        public static void LoadApplets()
        {
            Applications = new List<Applet>();
            var appFolder = new DirectoryInfo(settings.AppFolder);
            foreach (var appDir in appFolder.GetDirectories())
                try
                {
                    string appDirName = Regex.Replace(appDir.Name, "[^a-z]+", string.Empty, RegexOptions.IgnoreCase);
                    if (File.Exists(Path.Combine(appDir.FullName, appDirName + ".exe")))
                        Applications.Add(new Applet(Path.Combine(appDir.FullName, appDirName + ".exe")));
                    else
                        foreach (var appPath in appDir.GetFiles("*.exe"))
                        {
                            string appName = Regex.Replace(appPath.Name, "(\\.exe)|[^a-z]+", string.Empty, RegexOptions.IgnoreCase);
                            if (appDirName.Contains(appName) || appName.Contains(appDirName))
                            {
                                Applications.Add(new Applet(appPath.FullName));
                                break;
                            }
                        }
                }
                catch (UnauthorizedAccessException) { logstream.Error($"Access to the path \"{appDir.Name}\" is denied."); }

            //Applications.Sort(Comparer<Applet>.Create((a1, a2) => a1.Name[0].CompareTo(a2.Name[0])));

            settings = (Settings)JSONSerializer.Deserialize<Settings>(Settings.path);
            foreach (var appEx in settings.Exceptions)
                Applications.Remove(Applications.Find(a => string.Equals(a.Name, appEx, StringComparison.CurrentCultureIgnoreCase)));

            foreach (var appCus in settings.CustomApplications)
                try
                {
                    int index = Applications.FindIndex(a => string.Equals(a.Name, appCus.Name, StringComparison.CurrentCultureIgnoreCase));
                    if (index == -1)
                        index = Applications.FindIndex(a => string.Equals(a.ExePath, appCus.ExePath, StringComparison.CurrentCultureIgnoreCase));
                    Applications[index] = appCus;
                }
                catch (ArgumentOutOfRangeException) { logstream.Error($"Custom app \"{appCus.Name}\" not found in {appCus.ExePath}"); }
        }
    }
}