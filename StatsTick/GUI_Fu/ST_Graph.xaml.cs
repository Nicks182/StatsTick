using LibreHardwareMonitor.Hardware;
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

namespace StatsTick
{
    /// <summary>
    /// Interaction logic for ST_Graph.xaml
    /// </summary>
    public partial class ST_Graph : UserControl
    {
        public ST_Graph_Info G_ST_Graph_Info = null;

        List<ST_Graph_Point> G_ST_Graph_Points = null;

        bool G_ShowGraph = true;
        double G_ValueMin = 9999;
        double G_ValueMax = 0;

        public delegate void G_ShowGraphClickHandler();
        public event G_ShowGraphClickHandler G_OnShowGraphClick;

        public ST_Graph(ST_Graph_Info P_ST_Graph_Info)
        {
            G_ST_Graph_Info = P_ST_Graph_Info;
            G_ST_Graph_Points = new List<ST_Graph_Point>();
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _Init();
            
        }

        private void Lbl_Name_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _SetGraphVisibillity(!G_ShowGraph);
            G_OnShowGraphClick?.Invoke();
        }

        private void _Init()
        {
            if(G_ST_Graph_Info.IsDynamicWidth == true)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch;
            }
            else
            {
                this.Width = 160;
            }

            Lbl_Name.Text = G_ST_Graph_Info.Caption + " " + G_ST_Graph_Info.Symbol;
            Border_Main.BorderBrush = new SolidColorBrush(G_ST_Graph_Info.Color);
            Border.BorderBrush = new SolidColorBrush(G_ST_Graph_Info.Color);
            Graph_Line.Stroke = new SolidColorBrush(G_ST_Graph_Info.Color);

            if(G_ST_Graph_Info.Setting != null)
            {
                _SetGraphVisibillity(G_ST_Graph_Info.Setting.ShowGraph);
            }
        }

        public void _ResetMinMax()
        {
            G_ValueMax = 0;
            G_ValueMin = 100;
        }

        public void _Update(double P_Value)
        {
            _AddPoint(P_Value);
            _UpdateGui(P_Value);
        }

        public void _SetGraphVisibillity(bool P_ShowGraph)
        {
            G_ShowGraph = P_ShowGraph;

            if (G_ShowGraph == true)
            {
                Border.Visibility = Visibility.Visible;
            }
            else
            {
                Border.Visibility = Visibility.Collapsed;
            }

            if (G_ST_Graph_Info.Setting != null)
            {
                G_ST_Graph_Info.Setting.ShowGraph = G_ShowGraph;
            }
        }

        private void _UpdateGui(double P_Value)
        {
            if (P_Value < G_ValueMin)
            {
                G_ValueMin = P_Value;
            }

            if (P_Value > G_ValueMax)
            {
                G_ValueMax = P_Value;
            }

            Lbl_Value.Text = "↕" + P_Value.ToString("#0");
            Lbl_ValueMin.Text = "↓" + G_ValueMin.ToString("#0");
            Lbl_ValueMax.Text = "↑" + G_ValueMax.ToString("#0");
        }

        
        private void _AddPoint(double P_Value)
        {
            for (int p = G_ST_Graph_Points.Count - 1; p >= 0; p--)
            {
                G_ST_Graph_Points[p].X = G_ST_Graph_Points[p].X + G_ST_Graph_Info.Step;

                if(G_ST_Graph_Points[p].X > (Can_Graph.ActualWidth + G_ST_Graph_Info.Step))
                {
                    G_ST_Graph_Points.RemoveAt(p);
                }
            }

            G_ST_Graph_Points.Add(new ST_Graph_Point() 
            { 
                X = G_ST_Graph_Info.OffSet, 
                Y = P_Value
            });

            Graph_Line.Points.Clear();

            if (G_ShowGraph == true)
            {
                for (int p = 0; p < G_ST_Graph_Points.Count; p++)
                {
                    Graph_Line.Points.Add(new Point(G_ST_Graph_Points[p].X, Can_Graph.ActualHeight - _CalcReletiveValue(G_ST_Graph_Points[p].Y)));
                }
            }
            

        }

        private double _CalcReletiveValue(double P_Value)
        {
            return (Can_Graph.ActualHeight / 100) * P_Value;
        }
        
        

        
    }

    public class ST_Graph_Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class ST_Graph_Info
    {
        public string HWName { get; set; }
        public string SNName { get; set; }
        public string HWIdentifier { get; set; }
        public SensorType Type { get; set; }
        public string Caption { get; set; }
        public string Symbol { get; set; }
        public Color Color { get; set; }
        public double OffSet { get; set; }
        public double Step = 2; // Default
        public bool IsDynamicWidth = false; // Default
        public ST_Settings_Info_HWItem Setting { get; set; }
    }
}
