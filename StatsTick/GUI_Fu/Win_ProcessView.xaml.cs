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

namespace StatsTick
{
    /// <summary>
    /// Interaction logic for Win_ProcessView.xaml
    /// </summary>
    public partial class Win_ProcessView : Window
    {
        ST_MonitorInfo_PR G_ST_MonitorInfo_PR = null;

        public Win_ProcessView(ST_MonitorInfo_PR P_ST_MonitorInfo_PR)
        {
            G_ST_MonitorInfo_PR = P_ST_MonitorInfo_PR;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Grid_Processes.ItemsSource = G_ST_MonitorInfo_PR.Items.OrderByDescending(i => i.Value);
            
        }
    }
}
