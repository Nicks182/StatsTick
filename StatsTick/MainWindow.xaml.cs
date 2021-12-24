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
using System.Windows.Threading;

namespace StatsTick
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer G_Timer = new DispatcherTimer();

        ST_MonitorInfo G_ST_MonitorInfo = null;
        List<ST_Graph> G_ST_Graphs = new List<ST_Graph>();

        ST_Monitor G_ST_Monitor = null;
        ST_Settings G_ST_Settings = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            G_ST_Settings = new ST_Settings();
            G_ST_Settings._LoadDefault();

            this.Width = G_ST_Settings.G_Info.AppWidth;
            this.Height = G_ST_Settings.G_Info.AppHeight;
            Col_Left.Width = new GridLength(G_ST_Settings.G_Info.LeftPanelWidth);
            Col_Right.Width = new GridLength(G_ST_Settings.G_Info.RightPanelWidth);

            G_Timer.Interval = TimeSpan.FromMilliseconds(1000);
            G_Timer.Tick += G_Timer_Tick;
            G_ST_Monitor = new ST_Monitor();

            G_ST_MonitorInfo = G_ST_Monitor._GetAll();

            _InitGui(G_ST_MonitorInfo);

            this.SizeChanged += Window_SizeChanged;
            Split_Left.PreviewMouseLeftButtonUp += Split_Left_MouseLeftButtonUp;
            Split_Right.PreviewMouseLeftButtonUp += Split_Right_MouseLeftButtonUp;
        }

        private async void G_Timer_Tick(object sender, EventArgs e)
        {
            G_ST_MonitorInfo = await Task.Run(() => G_ST_Monitor._GetAll());
            _UpdateGui();
        }

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            G_Timer.Start();
        }

        private void Btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            G_Timer.Stop();
        }

        private void Check_ShowGraphs_Checked(object sender, RoutedEventArgs e)
        {
            for (int g = 0; g < G_ST_Graphs.Count; g++)
            {
                G_ST_Graphs[g]._SetGraphVisibillity(Check_ShowGraphs.IsChecked.GetValueOrDefault(false));
            }
        }

        private void Check_Processes_Checked(object sender, RoutedEventArgs e)
        {
            G_ST_Monitor.G_TrackProcess = Check_Processes.IsChecked.GetValueOrDefault(false);
        }

        private void Check_ProcesPreview_Checked(object sender, RoutedEventArgs e)
        {
            G_ST_Processes._SetTextVisible(Check_ProcesPreview.IsChecked.GetValueOrDefault(false));
        }

        private void Btn_DumpAll_Click(object sender, RoutedEventArgs e)
        {
            //StringBuilder L_Data = new StringBuilder();

            //foreach (var L_HW in G_ST_MonitorInfo.HWInfo.Items)
            //{
            //    L_Data.Append(L_HW.Type + "-" + L_HW.Name + ": " + Environment.NewLine);
            //    foreach (var L_Sen in L_HW.Sensors)
            //    {
            //        L_Data.Append("\t" + L_Sen.Type + "-" + L_Sen.Name + ": " + L_Sen.Value + Environment.NewLine);
            //    }
            //}

            Clipboard.SetText(G_ST_Monitor._GetDataDump().ToString());

            MessageBox.Show("Hardware data copied to clipboard. Paste in text editor.");
        }

        private void Btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            G_Timer.Stop();
            Win_Settings L_Win_Settings = new Win_Settings(G_ST_Monitor);
            L_Win_Settings.G_OnSettingsSave += L_Win_Settings_G_OnSettingsSave;
            L_Win_Settings.ShowDialog();
        }

        private void L_Win_Settings_G_OnSettingsSave(ST_Settings P_ST_Settings)
        {
            G_ST_Settings.G_Info.HWItems = P_ST_Settings.G_Info.HWItems;
            G_ST_Settings._SaveSettings();
            _InitGui(G_ST_MonitorInfo);
        }

        private void Btn_ResetMinMax_Click(object sender, RoutedEventArgs e)
        {
            foreach(ST_Graph L_Graph in G_ST_Graphs)
            {
                L_Graph._ResetMinMax();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            G_ST_Settings.G_Info.AppHeight = this.ActualHeight;
            G_ST_Settings.G_Info.AppWidth = this.ActualWidth;
            G_ST_Settings._SaveSettings();
        }

        private void Split_Left_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            G_ST_Settings.G_Info.LeftPanelWidth = Col_Left.Width.Value;
            G_ST_Settings._SaveSettings();
        }

        private void Split_Right_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            G_ST_Settings.G_Info.RightPanelWidth = Col_Right.Width.Value;
            G_ST_Settings._SaveSettings();
        }
    }
}
