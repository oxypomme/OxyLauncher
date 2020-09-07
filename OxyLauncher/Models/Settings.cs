using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace OxyLauncher.Models
{
    [Serializable()]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2235:Mark all non-serializable fields", Justification = "Depuis quand un string est pas sérializable ?")]
    public class Settings
    {
        [NonSerialized()]
        public readonly static string path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "settings.json");

        public string AppFolder { get; set; }

        public List<string> Exceptions { get; set; }

        public List<Applet> CustomApplications { get; set; }

        public string Editor { get; set; }

        public Settings(string appPath)
        {
            AppFolder = appPath;
            //Exceptions = new List<string>();
            //CustomApplications = new List<Applet>();
            Editor = Environment.GetEnvironmentVariable("windir") + @"\system32\notepad.exe";
        }
    }
}