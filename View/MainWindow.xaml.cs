using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;

namespace View
{
    public partial class MainWindow : Window
    {
        MainViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            Chart chart = new Chart();
            winFormsHost.Child = chart;

            MyAppUIServices myAppUIServices = new MyAppUIServices(chart);
            mainViewModel = new MainViewModel(myAppUIServices);

            DataContext = mainViewModel;
            mainViewModel.UpdateCollectionReference += UpdateReferenceHandler;

            chart.ChartAreas.Add(new ChartArea("chartArea1"));
            chart.ChartAreas.Add(new ChartArea("chartArea2"));

        }

        public void UpdateReferenceHandler() 
        {
            listBox.ItemsSource = mainViewModel.ObservableData;
        }

        public class MyAppUIServices : IUIServices
        {
            private Chart chart;

            public MyAppUIServices(Chart chart)
            {
                this.chart = chart;
            }

            public void DrawGraphic(double[] x, double[] y, Property property = null)
            {
                chart.Series.Add("Series" + chart.Series.Count);
                int number = chart.Series.Count - 1;
                chart.Series[number].Points.DataBindXY(x, y);
                chart.Series[number].ChartType = SeriesChartType.Spline;
                chart.Series[number].LegendText = property.name;
                chart.Series[number].MarkerSize = 9;

                switch (property.metadata)
                {
                    case "Area 1":
                        chart.ChartAreas[0].AxisX.LabelStyle.Format = property.format;
                        chart.ChartAreas[0].AxisY.LabelStyle.Format = property.format;
                        chart.Series[number].MarkerStyle = MarkerStyle.Circle;
                        chart.Series[number].ChartType = SeriesChartType.Spline;
                        chart.Series[number].ChartArea = "chartArea1";
                        break;
                    case "Area 2":
                        for (int j = 0; j < chart.Series[number].Points.Count; j++)
                            chart.Series[number].Points[j].ToolTip =
                             "p = " + chart.Series[number].Points[j].XValue.ToString() +
                             "\nF(x) = " + chart.Series[number].Points[j].YValues[0].ToString();

                        chart.ChartAreas[1].AxisX.LabelStyle.Format = property.format;
                        chart.ChartAreas[1].AxisY.LabelStyle.Format = property.format;
                        chart.Series[number].Sort(PointSortOrder.Ascending, "X");
                        chart.Series[number].MarkerStyle = MarkerStyle.Square;
                        chart.Series[number].ChartType = SeriesChartType.Line;
                        chart.Series[number].IsVisibleInLegend = false;
                        chart.Series[number].ChartArea = "chartArea2";
                        break;
                    default:
                        break;

                }
            }

            public string ConfirmOpen()
            {
                Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
                if (ofd.ShowDialog() == true)
                    return ofd.FileName;
                return null;
            }

            public string ConfirmSave(bool useMessageBox)
            {
                if (useMessageBox)
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Вы изменили данные. Без сохранения они будут утеряны\nСохранить?", "Сохранение", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return null;
                    }
                }
                
                Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                if (sfd.ShowDialog() == true)
                    return sfd.FileName;

                return null;
            }

            public void ClearChart()
            {
                chart.Series.Clear();
                chart.Legends.Clear();
                chart.Legends.Add("legend");
            }
        }
    }
}

