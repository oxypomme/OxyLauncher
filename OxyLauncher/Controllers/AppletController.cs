using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;
using OxyLauncher.Models;
using OxyLauncher.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                    Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(System.Drawing.Icon.ExtractAssociatedIcon(app.ExePath).Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
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
                var app = (Applet)((FrameworkElement)sender).Tag;
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    if (App.settings.CustomApplications.FindIndex(a => a.Equals(app)) == -1)
                    {
                        App.settings.CustomApplications.Add(app);
                        OxyNuggets.JSON.JSONSerializer.Serialize(Settings.path, App.settings);
                        Task.Run(() =>
                        {
                            var notificationManager = new NotificationManager();
                            notificationManager.ShowAsync(
                            new NotificationContent { Title = "Configuration sauvegardée", Message = $"Vous pouvez modifier la configuration de \"{app}\" depuis le fichier settings.json.", Type = NotificationType.Success },
                            areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(2));
                        });
                        App.logstream.Log($"Custom config added for \"{app}\"");
                    }
                    else
                        Task.Run(() =>
                        {
                            var notificationManager = new NotificationManager();
                            notificationManager.ShowAsync(
                            new NotificationContent { Title = "Configuration déjà existante", Message = $"La configuration de \"{app}\" existe déjà !", Type = NotificationType.Error },
                            areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(2));
                        });
                else
                {
                    Process.Start("explorer.exe", Path.GetDirectoryName(app.ExePath));
                    App.logstream.Log($"Opening {app} folder");
                }
            }
            catch (Exception ex) { App.logstream.Error(ex); }
        }

        public static void AppletClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var app = (Applet)((FrameworkElement)sender).Tag;
                Process.Start(new ProcessStartInfo(app.ExePath)
                {
                    Arguments = app.arguments,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                });

                Task.Run(() =>
                {
                    var notificationManager = new NotificationManager();
                    notificationManager.ShowAsync(
                    new NotificationContent { Title = "Applet lancée", Message = $"L'applet \"{app}\" est maintenant lancée.", Type = NotificationType.Success },
                    areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(2));
                });

                App.logstream.Log($"\"{app}\" started with \"{app.arguments}\"");
            }
            catch (Exception ex)
            {
                App.logstream.Error(ex);
                Task.Run(() =>
                {
                    var notificationManager = new NotificationManager();
                    notificationManager.ShowAsync(
                    new NotificationContent { Title = "Une erreur est survenue", Message = "", Type = NotificationType.Error },
                    areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(2));
                });
            }
        }
    }
}