using Microsoft.Win32;
using System;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeoTunes
{
    public partial class MainWindow : Window
    {
        ETSConnection connection;
        List<City> cities;
        Placement placement;
        CityDistance[] cityDistances;
        RadioHandler radioHandler;
        string radioPath = "D:\\GitRepo\\GeoTunes\\Radios\\";

        private System.Timers.Timer timer;
        public MainWindow()
        {
            InitializeComponent();
            string url = Interaction.InputBox("Enter the Telemetry API URL!");
            if (string.IsNullOrEmpty(url))
            {
                Application.Current.Shutdown();
                return;
            }
            connection = new ETSConnection(url);
            radioHandler = new RadioHandler();
            cities = connection.GetCities();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += async (sender, e) => await OnTimerElapsed();
            timer.AutoReset = true;
            timer.Enabled = true;
        }


        private double Euklid3D(double x, double y, double z)
        {
            return Math.Sqrt(x*x + y*y + z*z);
        }

        private double getDistance(Placement Truck, double x, double y, double z)
        {
            return Euklid3D(Truck.x - x, Truck.y - y, Truck.z - z);
        }

        private CityDistance[] SortedCityDistance(Placement truck)
        {
            return cities
            .Select(c => new CityDistance(c.realName, getDistance(truck, c.x, c.y, c.z)))
            .OrderBy(cd => cd.Distance)
            .ToArray();
        }

        private async Task OnTimerElapsed()
        {
            try
            {
                if(connection.isShuttingDown)
                {
                    Environment.Exit(0);
                }
                placement = await connection.GetPlacement();
                cityDistances = SortedCityDistance(placement);
                Dispatcher.Invoke(() =>
                {
                    LBClosestCities.ItemsSource = cityDistances;
                    LBLCurrentPos.Content = "Current Pos: x: " + placement.x.ToString() + " y: " + placement.y.ToString() + " z: " + placement.z.ToString();
                });
                await radioHandler.PlayRadio(cityDistances, radioPath);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void CopyCurrentCoordinatesToClipboard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(connection.GetCoordinatesForJson());
        }
    }
}
