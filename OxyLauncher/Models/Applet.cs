using Newtonsoft.Json;
using System;
using System.IO;

namespace OxyLauncher.Models
{
    [Serializable()]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2235:Mark all non-serializable fields", Justification = "Depuis quand un string est pas sérializable ?")]
    public class Applet
    {
        private string _name;

        [JsonProperty("exe_path")]
        private string _exepath;

        [JsonProperty("name")]
        public string Name { get => char.ToUpper(_name[0]) + _name.Substring(1); set => _name = value; }

        public string arguments { get; set; }

        [JsonIgnore]
        public string ExePath { get => Path.Combine(App.settings.AppFolder, _exepath); set => _exepath = value; }

        public Applet(string path, string name = "", string args = "")
        {
            _exepath = path;
            _name = !string.IsNullOrEmpty(name) ? name : System.Text.RegularExpressions.Regex.Replace(Path.GetRelativePath(App.settings.AppFolder, Path.GetDirectoryName(path)), @"[^a-zA-Z]+", "");
            arguments = args;
        }

        public override string ToString() => _name;

        public override bool Equals(object obj)
        {
            if (obj is Applet)
            {
                if (((Applet)obj)._name.ToLower() == _name.ToLower())
                    return true;
            }
            return false;
        }

        public static bool Equals(Applet app1, Applet app2)
        {
            if (app1._name.ToLower() == app2._name.ToLower())
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}