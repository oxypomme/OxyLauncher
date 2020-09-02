using System;
using System.Collections.Generic;
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
            foreach (var applet in App.Applications)
                MainList.Children.Add(AppletController.CreateAppletButton(applet));
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