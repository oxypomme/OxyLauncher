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
            /*
            var sp = new StackPanel();
            sp.Children.Add(new Image()
            {
                Source = App.GetIconFromExe(app.ExePath),
                MaxWidth = 32,
                MaxHeight = 32
            });
            sp.Children.Add(new TextBlock()
            {
                Text = app.Name,
                TextAlignment = TextAlignment.Center
            });

            var btn = new Button()
            {
                Content = sp,
                Tag = app
            };
            */

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
                    App.notificationManager.Show(new Notifications.Wpf.NotificationContent
                    {
                        Title = "Configuration sauvegardée",
                        Message = $"Vous pouvez modifier la configuration de \"{(Applet)((FrameworkElement)sender).Tag}\" depuis le fichier settings.json.",
                        Type = Notifications.Wpf.NotificationType.Success
                    });
                    App.logstream.Log($"Custom config added for \"{(Applet)((FrameworkElement)sender).Tag}\"");
                }
                else
                    App.notificationManager.Show(new Notifications.Wpf.NotificationContent
                    {
                        Title = "Erreur",
                        Message = $"La configuration de \"{(Applet)((FrameworkElement)sender).Tag}\" existe déjà !",
                        Type = Notifications.Wpf.NotificationType.Error
                    });
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

                App.notificationManager.Show(new Notifications.Wpf.NotificationContent
                {
                    Title = "Applet lancée",
                    Message = $"L'applet \"{(Applet)((FrameworkElement)sender).Tag}\" est maintenant lancée.",
                    Type = Notifications.Wpf.NotificationType.Success
                });
                App.logstream.Log($"\"{(Applet)((FrameworkElement)sender).Tag}\" started with \"{((Applet)((FrameworkElement)sender).Tag).arguments}\"");
            }
            catch (Exception ex)
            {
                App.logstream.Error(ex);
                App.notificationManager.Show(new Notifications.Wpf.NotificationContent
                {
                    Title = "Une erreur est survenue",
                    Message = "",
                    Type = Notifications.Wpf.NotificationType.Error
                });
            }
        }
    }
}