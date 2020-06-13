using LiveCharts;
using LiveCharts.Wpf;
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

namespace SypnosisApp
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class Graph : UserControl
    {
        ConnectionToDB connection = new ConnectionToDB();


        public Graph()
        {
            InitializeComponent();
            

            int ICCamp = connection.GetCampIncome();
            int ICTicket = connection.GetTicketIncome();
            double ICShop = connection.GetShopIncome();
            double ICRental = connection.GetRentalIncome() + connection.GetRentalIncome1();


            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", ICCamp, chartPoint.Participation);

            ICTicketlb = ChartPoint =>
                string.Format("{0} ({1:P})", ICTicket, ChartPoint.Participation);

            ICShoplb = ChartPoint =>
                string.Format("{0} ({1:P})", ICShop, ChartPoint.Participation);

            ICLoaning = ChartPoint =>
               string.Format("{0} ({1:P})", ICRental, ChartPoint.Participation);


            DataContext = this;
        }

        public Func<ChartPoint, string> PointLabel { get; set; }
        public Func<ChartPoint, string> ICTicketlb { get; set; }
        public Func<ChartPoint, string> ICShoplb { get; set; }
        public Func<ChartPoint, string> ICLoaning { get; set; }


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
