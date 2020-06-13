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
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for Graph1.xaml
    /// </summary>
    public partial class Graph1 : UserControl
    {
        ConnectionToDB connection = new ConnectionToDB();

        public Graph1()
        {
            InitializeComponent();

            int NrCamp = connection.GetNrCamp();
            int NrChecks = connection.GetNrChecksIn();
            int NrLeft = connection.GetNrLeft();
            int NrTicket = connection.GetNrTicket();
            int NrRegistered = connection.GetNrRegistered();


            NrCamplb = chartPoint =>
                string.Format("{0} ({1:P})", NrCamp, chartPoint.Participation);

            NrCheckslb = ChartPoint =>
                 string.Format("{0} ({1:P})", NrChecks, ChartPoint.Participation);

            NrLeftlb = ChartPoint =>
                string.Format("{0} ({1:P})", NrLeft, ChartPoint.Participation);

            NrTicketlb = ChartPoint =>
               string.Format("{0} ({1:P})", NrTicket, ChartPoint.Participation);

            NrRegisteredlb = ChartPoint =>
              string.Format("{0} ({1:P})", NrRegistered, ChartPoint.Participation);

            DataContext = this;


        }
        public Func<ChartPoint, string> NrCamplb { get; set; }
        public Func<ChartPoint, string> NrCheckslb { get; set; }
        public Func<ChartPoint, string> NrLeftlb { get; set; }
        public Func<ChartPoint, string> NrTicketlb { get; set; }
        public Func<ChartPoint, string> NrRegisteredlb { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
