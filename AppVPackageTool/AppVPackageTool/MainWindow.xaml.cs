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
            MessageBox.Show(Connection.status.Length.ToString());
        }

        private void loadListRemote()
        {
            Connection.status = hostnameTextBox.Text;
            List<ListAppvPackages> appvPackages = WmiQuery.GetAppvPackagesRemote(hostnameTextBox.Text);
            wmiAppvListBox.ItemsSource = appvPackages;
            MessageBox.Show(Connection.status.Length.ToString());
        }

        private void localHostButton_Click(object sender, RoutedEventArgs e)
        {
            loadListLocalhost();
        }

        private void removePackageButton_Click(object sender, RoutedEventArgs e)
        {
            ListAppvPackages item = wmiAppvListBox.SelectedItem as ListAppvPackages;
            MessageBox.Show($"{item.Name} {item.PackageID}");
            if (Connection.status.Length > 0)
            {
                WmiQuery.RemoveAppvPackageRemote(item.PackageID, item.VersionID, hostnameTextBox.Text);
                loadListRemote();
            }
            else
            {
                WmiQuery.RemoveAppvPackageLocalHost(item.PackageID, item.VersionID);
                loadListLocalhost();
            }
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            loadListRemote();
        }
    }
    static class Connection
    {
        public static string status = "";
    }
}
