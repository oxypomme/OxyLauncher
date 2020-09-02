using OxyLauncher.Models;
using OxyLauncher.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OxyLauncher.Controllers
{
    internal static class AppletController
    {
        public static Button CreateAppletButton(Applet app)
        {
            var btn = new Button()
            {
                Content = new Image()
                {
                    Source = App.GetIconFromExe(app.ExePath)
                },
                Tag = app,
                ToolTip = app.Name
            };

            btn.Click += AppletClicked;
            btn.MouseRightButtonDown += AppletSaved;
            return btn;
        }

        private static void AppletSaved(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (App.settings.CustomApplications.FindIndex(a => a.Equals((Applet)((FrameworkElement)sender).Tag)) == -1)
                {
                    App.settings.CustomApplications.Add((Applet)((FrameworkElement)sender).Tag);
                    OxyNuggets.JSON.JSONSerializer.Serialize(Settings.path, App.settings);
                    App.logstream.Log($"Custom config added for \"{(Applet)((FrameworkElement)sender).Tag}\"");
                }
            }
            catch (Exception ex) { App.logstream.Error(ex); }
        }

        public static void AppletClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(((Applet)((FrameworkElement)sender).Tag).ExePath)
                {
                    Arguments = ((Applet)((FrameworkElement)sender).Tag).arguments,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                });
                App.logstream.Log($"\"{(Applet)((FrameworkElement)sender).Tag}\" started with \"{((Applet)((FrameworkElement)sender).Tag).arguments}\"");
            }
            catch (Exception ex)
            {
                App.logstream.Error(ex);
            }
        }
    }
}