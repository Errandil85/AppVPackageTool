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
using AppVPackageTool.Library;
using System.Threading;

namespace AppVPackageTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void loadListLocalhost()
        {
            List<ListAppvPackages> appvPackages = WmiQuery.GetAppvPackagesLocalHost();
            wmiAppvListBox.ItemsSource = appvPackages;
            Connection.status = "";
            conectedToLabel.Content = "Connected to: Localhost";
            conectedToLabel.Visibility = Visibility.Visible;
            refreshButton.IsEnabled = true;
        }

        private void loadListRemote()
        {
            Connection.status = hostnameTextBox.Text;
            List<ListAppvPackages> appvPackages = WmiQuery.GetAppvPackagesRemote(Connection.status);
            if (appvPackages.Count > 0)
            {
                wmiAppvListBox.ItemsSource = appvPackages;
                conectedToLabel.Content = $"Connected to: {Connection.status}";
                conectedToLabel.Visibility = Visibility.Visible;
                refreshButton.IsEnabled = true;
            }
        }

        private void localHostButton_Click(object sender, RoutedEventArgs e)
        {
            loadListLocalhost();
        }

        private void removePackageButton_Click(object sender, RoutedEventArgs e)
        {
            ListAppvPackages item = wmiAppvListBox.SelectedItem as ListAppvPackages;
            //MessageBox.Show($"{Connection.status} {item.Name} {item.IsPublishedGlobally}");
            var dialogResult = MessageBox.Show($"Are you sure you want to remove: {item.Name}?", "Warning", MessageBoxButton.OKCancel);
            if (dialogResult == MessageBoxResult.OK)
            {
                if (item.InUse != true)
                {
                    RemovePackage(item);
                }
                else
                {
                    StopAppvClientPackage(item);
                    Thread.Sleep(2000);
                    RemovePackage(item);
                }
            }
            else if (dialogResult == MessageBoxResult.Cancel)
            {
                // do nothing
            }
            
        }

        private void RemovePackage(ListAppvPackages item)
        {
            if (Connection.status.Length > 0)
            {
                if (item.IsPublishedGlobally)
                {
                    WmiQuery.RemoveAppvPackageRemote(item.PackageID, item.VersionID, Connection.status);
                }
                else
                {
                    WmiQuery.RemoveAppvPackageRemoteUser(item.PackageID, item.VersionID, Connection.status);
                }
                Thread.Sleep(5000);
                loadListRemote();
            }
            else
            {
                if (item.IsPublishedGlobally)
                {
                    WmiQuery.RemoveAppvPackageLocalHost(item.PackageID, item.VersionID);
                }
                else
                {
                    WmiQuery.RemoveAppvPackageLocalHostUser(item.PackageID, item.VersionID);
                }
                Thread.Sleep(5000);
                loadListLocalhost();
            }
        }

        private void StopAppvClientPackage(ListAppvPackages item)
        {
            if (Connection.status.Length > 0)
            {
                WmiQuery.StopAppvClientPackageRemote(item.PackageID, item.VersionID, Connection.status);
            }
            else
            {
                WmiQuery.StopAppvClientPackageLocalhost(item.PackageID, item.VersionID);
            }
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            loadListRemote();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (Connection.status.Length > 0)
            {
                loadListRemote();
            }
            else
            {
                loadListLocalhost();
            }
        }

        private void hostnameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                loadListRemote();
            }
        }
    }
    static class Connection
    {
        public static string status = "";
    }
}
