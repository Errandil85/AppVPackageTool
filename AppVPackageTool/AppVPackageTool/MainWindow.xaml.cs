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

        /// <summary>
        /// Close application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Load UI with localhost content
        /// </summary>
        private async void loadListLocalhost()
        {
            //List<ListAppvPackages> appvPackages = WmiQuery.GetAppvPackagesLocalHost();
            List<ListAppvPackages> appvPackages = await GetListAppvLocalhostAsync();
            if (appvPackages.Count > 0)
            {
                wmiAppvListBox.ItemsSource = appvPackages;
                Connection.status = "";
                conectedToLabel.Content = "Connected to: Localhost";
                conectedToLabel.Visibility = Visibility.Visible;
                refreshButton.IsEnabled = true;
                removePackageButton.IsEnabled = true;
            }  
        }


        /// <summary>
        /// Query async for localhost appv packages
        /// </summary>
        /// <returns></returns>
        private async Task<List<ListAppvPackages>> GetListAppvLocalhostAsync()
        {
            List<ListAppvPackages> appvPackages = await Task.Run(() => WmiQuery.GetAppvPackagesLocalHost());
            return appvPackages;
        }


        /// <summary>
        /// Load UI with remote host content
        /// </summary>
        private async void loadListRemote()
        {
            Connection.status = hostnameTextBox.Text;
            //List<ListAppvPackages> appvPackages = WmiQuery.GetAppvPackagesRemote(Connection.status);
            List<ListAppvPackages> appvPackages = await GetListAppvRemoteAsync();
            if (appvPackages.Count > 0)
            {
                wmiAppvListBox.ItemsSource = appvPackages;
                conectedToLabel.Content = $"Connected to: {Connection.status}";
                conectedToLabel.Visibility = Visibility.Visible;
                refreshButton.IsEnabled = true;
                removePackageButton.IsEnabled = true;
            }
        }


        /// <summary>
        /// Query async for remote host appv packages
        /// </summary>
        /// <returns></returns>
        private async Task<List<ListAppvPackages>> GetListAppvRemoteAsync()
        {
            List<ListAppvPackages> appvPackages = await Task.Run(() => WmiQuery.GetAppvPackagesRemote(Connection.status));
            return appvPackages;
        }


        /// <summary>
        /// Louad localhost button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void localHostButton_Click(object sender, RoutedEventArgs e)
        {
            loadListLocalhost();
        }


        /// <summary>
        /// Remove packages button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void removePackageButton_Click(object sender, RoutedEventArgs e)
        {
            ListAppvPackages item = wmiAppvListBox.SelectedItem as ListAppvPackages;
            await RemovePackageCallAsync(item);
        }

        /// <summary>
        /// Remove packages async call for button method
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task RemovePackageCallAsync(ListAppvPackages item)
        {
            if (item != null)
            {
                var dialogResult = MessageBox.Show($"Are you sure you want to remove: {item.Name}?", "Warning", MessageBoxButton.OKCancel);
                if (dialogResult == MessageBoxResult.OK)
                {
                    if (item.InUse != true)
                    {
                        await RemovePackageAsync(item);
                    }
                    else
                    {
                        await StopAppvClientPackageAsync(item);
                        await Task.Delay(2000);
                        await RemovePackageAsync(item);
                    }
                }
                else if (dialogResult == MessageBoxResult.Cancel)
                {
                    // do nothing
                }
            }
            else
            {
                MessageBox.Show("Please select a package", "Warning");
            }
        }

        /// <summary>
        /// Method to remove packages async
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task RemovePackageAsync(ListAppvPackages item)
        {
            if (Connection.status.Length > 0)
            {
                if (item.IsPublishedGlobally)
                {
                    await Task.Run(() => WmiQuery.RemoveAppvPackageRemote(item.PackageID, item.VersionID, Connection.status));
                }
                else
                {
                    await Task.Run(() => WmiQuery.RemoveAppvPackageRemoteUser(item.PackageID, item.VersionID, Connection.status));
                }
                //Thread.Sleep(5000);
                await Task.Delay(5000);
                loadListRemote();
            }
            else
            {
                if (item.IsPublishedGlobally)
                {
                    await Task.Run(() => WmiQuery.RemoveAppvPackageLocalHost(item.PackageID, item.VersionID));
                }
                else
                {
                    await Task.Run(() => WmiQuery.RemoveAppvPackageLocalHostUser(item.PackageID, item.VersionID));
                }
                //Thread.Sleep(5000);
                await Task.Delay(5000);
                loadListLocalhost();
            }
        }

        /// <summary>
        /// Method to stop running appv packages
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task StopAppvClientPackageAsync(ListAppvPackages item)
        {
            if (Connection.status.Length > 0)
            {
                await Task.Run(() => WmiQuery.StopAppvClientPackageRemote(item.PackageID, item.VersionID, Connection.status));
            }
            else
            {
                await Task.Run(() => WmiQuery.StopAppvClientPackageLocalhost(item.PackageID, item.VersionID));
            }
        }

        /// <summary>
        /// Load UI remote button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            loadListRemote();
        }

        /// <summary>
        /// Refresh ui
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Loud remote on enter key in textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hostnameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                loadListRemote();
            }
        }
    }

    /// <summary>
    /// Connection status class
    /// </summary>
    static class Connection
    {
        public static string status = "";
    }
}
