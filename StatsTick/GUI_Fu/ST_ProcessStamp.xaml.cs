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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StatsTick
{
    /// <summary>
    /// Interaction logic for ST_ProcessStamp.xaml
    /// </summary>
    public partial class ST_ProcessStamp : UserControl
    {
        ST_MonitorInfo_PR G_ST_MonitorInfo_PR = null;
        StringBuilder G_Text = new StringBuilder();
        string G_StampId = null;
        bool G_ShowText = false;

        public delegate void G_ProcessClickHandler(ST_MonitorInfo_PR P_ST_MonitorInfo_PR);
        public event G_ProcessClickHandler G_OnProcessClick;

        public ST_ProcessStamp(ST_MonitorInfo_PR P_ST_MonitorInfo_PR, bool P_ShowText)
        {
            G_ST_MonitorInfo_PR = P_ST_MonitorInfo_PR;
            G_ShowText = P_ShowText;
            InitializeComponent();

        }

        

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < G_ST_MonitorInfo_PR.Items.Count; i++)
            {
                if(i > 4)
                {
                    break;
                }
                G_Text.Append(G_ST_MonitorInfo_PR.Items[i].Name + Environment.NewLine);
            }

            _SetVisible();
            _SetWidth();
            //Lbl_Text.Content = G_Text.ToString();
        }

        public void _SetTextVisible(bool P_ShowText)
        {
            G_ShowText = P_ShowText;
            _SetVisible();
        }


        private void _SetVisible()
        {
            if(G_ShowText == false)
            {
                Btn_Text.Content = null;
            }
            else
            {
                Btn_Text.Content = G_Text.ToString();
            }
        }

        private void _SetWidth()
        {
            if(G_ShowText == true)
            {
                object sb = this.FindResource("AniWidthKey");
                if(sb != null)
                {
                    (sb as Storyboard).Begin();
                }
            }
            else
            {
                this.Width = 100;
            }
        }

        private void Btn_Text_Click(object sender, RoutedEventArgs e)
        {
            G_OnProcessClick?.Invoke(G_ST_MonitorInfo_PR);
        }
    }
}
