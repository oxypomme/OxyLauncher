﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Notifications.Wpf.Core;
using OxyLauncher.Controllers;

namespace OxyLauncher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Ressources/logo.ico", UriKind.RelativeOrAbsolute));

                /* RELOAD SHORTCUT (F5) */
                RoutedCommand keyShortcut = new RoutedCommand();
                keyShortcut.InputGestures.Add(new KeyGesture(Key.F5));
                CommandBindings.Add(new CommandBinding(keyShortcut, Reload_Click));

                LoadApplets();
            }
            catch (Exception e) { App.logstream.Error(e); }
        }

        private void LoadApplets()
        {
            MainList.Children.Clear();

            var btn = new Button()
            {
                Content = new Image()
                {
                    Source = BitmapFrame.Create(new Uri("pack://application:,,,/Ressources/settings.png", UriKind.RelativeOrAbsolute))
                },
                ToolTip = "Paramètres"
            };

            btn.Click += SettingsClicked;
            btn.MouseRightButtonDown += SettingsSaved;
            MainList.Children.Add(btn);

            foreach (var applet in App.Applications)
                MainList.Children.Add(AppletController.CreateAppletButton(applet));
        }

        private void SettingsClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(App.settings.Editor)
                {
                    Arguments = "\"" + Models.Settings.path + "\"",
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                });
            }
            catch (Exception ex) { App.logstream.Error(ex); }
        }

        private void SettingsSaved(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.LoadApplets();
                LoadApplets();

                App.logstream.Log("List reloaded");

                Task.Run(() =>
                {
                    var notificationManager = new NotificationManager();
                    notificationManager.ShowAsync(
                    new NotificationContent { Title = "Applets rechargés", Message = "La liste des applets est maintenant à jour.", Type = NotificationType.Information },
                    areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(2));
                });
            }
            catch (Exception ex) { App.logstream.Error(ex); }
        }
    }
}