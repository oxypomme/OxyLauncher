using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;

namespace OxyLauncher.Models
{
    [Serializable()]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2235:Mark all non-serializable fields", Justification = "Depuis quand un string est pas sérializable ?")]
    public class Applet
    {
        [JsonIgnore]
        private string _name;

        [DefaultValue("")]
        [JsonProperty("exe_path", Required = Required.Always)]
        public string _exepath;

        [DefaultValue("")]
        [JsonProperty("name")]
        public string Name { get => char.ToUpper(_name[0]) + _name.Substring(1); set => _name = value; }

        [DefaultValue("")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string arguments { get; set; }

        [JsonIgnore]
        public string ExePath { get => Path.Combine(App.settings.AppFolder, _exepath); set => _exepath = value; }

        [DefaultValue("")]
        [JsonProperty("work_path", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private string _work;

        [JsonIgnore]
        public string Work_Path { get => !string.IsNullOrEmpty(_work) ? _work : Path.GetDirectoryName(ExePath); set => _work = value; }

        public Applet(string path = "", string name = "", string args = "", string work = "")
        {
            try
            {
                _exepath = Path.GetRelativePath(App.settings.AppFolder, path);
            }
            catch (Exception e) when (e is NullReferenceException || e is ArgumentNullException)
            {
                _exepath = "";
                App.logstream.Warning(e);
            }
            _name = !string.IsNullOrEmpty(name) ? name : System.Text.RegularExpressions.Regex.Replace(Path.GetDirectoryName(_exepath), @"[^a-zA-Z]+", "");
            arguments = args;
            _work = work;
        }

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            if (obj is Applet)
                if (((Applet)obj)._name.ToLower() == _name.ToLower())
                    return true;
            return false;
        }

        public static bool Equals(Applet app1, Applet app2)
        {
            if (app1._name.ToLower() == app2._name.ToLower())
                return true;
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}