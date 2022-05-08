using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Metrics_app.Services;
using Microsoft.OpenApi.Models;

namespace Metrics_app
{
    /// <summary>
    /// Логика взаимодействия для RamControl.xaml
    /// </summary>
    public partial class RamControl : UserControl, INotifyPropertyChanged
    {
        private MetricsAgentClient metricsAgentClient;
        private int _lastLecture;
        private double _trend = 1;
        int Id = 1;

        public event PropertyChangedEventHandler? PropertyChanged;


        public RamControl()
        {
            InitializeComponent();
            metricsAgentClient = new MetricsAgentClient(new HttpClient());
            LastHourSeries = new SeriesCollection
            {
                new LineSeries
                {
                    AreaLimit = -10,
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0),
                        new ObservableValue(0)
                    }
                }
            };
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(200);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LastHourSeries[0].Values.Add(new ObservableValue(_trend));
                        LastHourSeries[0].Values.RemoveAt(0);
                        Set();
                    });
                }
            });

            DataContext = this;
        }


        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public SeriesCollection LastHourSeries { get; set; }

        public int LastLecture
        {
            get { return _lastLecture; }
            set
            {
                _lastLecture = value;
                OnPropertyChanged("LastLecture");
            }
        }
        private async void Set()
        {
            RamMetrics ramMetrics = await metricsAgentClient.GetRamMetrics(new GetAllRamMetricsApiRequest()
            {
                ClientBaseAddress = "http://localhost:5000",
                id = Id
            });

            try
            {
                if (ramMetrics != null)
                {
                    Task.Run(() =>
                    {
                        LastLecture = ramMetrics.Value;
                    });
                    _trend = Convert.ToDouble(ramMetrics.Value);
                    Id++;
                }
            }
            catch (Exception ex) { }
        }
    }
}
