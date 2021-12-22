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
    /// Interaction logic for ST_Processes.xaml
    /// </summary>
    public partial class ST_Processes : UserControl
    {
        ST_MonitorInfo G_ST_MonitorInfo = null;

        List<UIElement> G_ToRemove = new List<UIElement>();

        bool G_ShowText = false;

        public delegate void G_ProcessClickHandler(ST_MonitorInfo_PR P_ST_MonitorInfo_PR);
        public event G_ProcessClickHandler G_OnProcessClick;
        

        public ST_Processes()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Stack_ProcessStamps.MaxHeight = 10;
        }

        public void _SetTextVisible(bool P_ShowText)
        {
            G_ShowText = P_ShowText;

            if(G_ShowText == true)
            {
                Stack_ProcessStamps.MaxHeight = 90;
            }
            else
            {
                Stack_ProcessStamps.MaxHeight = 10;
            }

            for(int i = 0; i < Stack_ProcessStamps.Children.Count; i++)
            {
                (Stack_ProcessStamps.Children[i] as ST_ProcessStamp)._SetTextVisible(G_ShowText);
            }
        }

        public void _SetProcesses(ST_MonitorInfo P_ST_MonitorInfo)
        {
            G_ST_MonitorInfo = P_ST_MonitorInfo;
            if (G_ST_MonitorInfo.PRInfo != null)
            {
                for (int p = Stack_ProcessStamps.Children.Count - 1; p >= 0; p--)
                {
                    if (IsUserVisible(Stack_ProcessStamps.Children[p] as FrameworkElement, this) == false)
                    {
                        Stack_ProcessStamps.Children.RemoveAt(p);
                    }
                }

                ST_ProcessStamp L_ST_ProcessStamp = new ST_ProcessStamp(P_ST_MonitorInfo.PRInfo, G_ShowText);
                L_ST_ProcessStamp.G_OnProcessClick += L_ST_ProcessStamp_G_OnProcessClick;

                Stack_ProcessStamps.Children.Insert(0, L_ST_ProcessStamp);
            }
            else
            {
                Stack_ProcessStamps.Children.Clear();
            }
        }

        private void L_ST_ProcessStamp_G_OnProcessClick(ST_MonitorInfo_PR P_ST_MonitorInfo_PR)
        {
            G_OnProcessClick?.Invoke(P_ST_MonitorInfo_PR);
        }

        private bool IsUserVisible(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
                return false;

            Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
            //return rect.Contains(bounds.TopLeft);
            //return rect.Contains(bounds.BottomRight);
            //return bounds.X > rect.Width;
        }
    }
}
