using ColorPicker;
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
using System.Windows.Shapes;

namespace StatsTick
{
    /// <summary>
    /// Interaction logic for Win_Settings.xaml
    /// </summary>
    public partial class Win_Settings : Window
    {
        ST_Monitor G_ST_Monitor = null;

        ST_Settings G_ST_Settings = null;

        List<ST_Settings_Info_HWItem> G_HWItems = null;

        public delegate void G_SettingsSaveHandler(ST_Settings P_ST_Settings);
        public event G_SettingsSaveHandler G_OnSettingsSave;

        public Win_Settings(ST_Monitor P_ST_Monitor)
        {
            G_ST_Monitor = P_ST_Monitor;
            G_HWItems = new List<ST_Settings_Info_HWItem>();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            G_ST_Settings = new ST_Settings();
            G_ST_Settings._LoadDefault();
            _Get_Dropdowns_Info();
        }

        

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            G_OnSettingsSave?.Invoke(G_ST_Settings);
            this.Close();
        }

        private void Drop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox L_Combo = (sender as ComboBox);
            ST_HWItem_Type L_ST_HWItem_Type = _ToEnum<ST_HWItem_Type>(L_Combo.Tag);
            ST_Settings_Info_HWItem L_ST_Settings_Info_HWItem = G_ST_Settings.G_Info.HWItems.Where(i => i.Type == L_ST_HWItem_Type).FirstOrDefault();
            if(L_ST_Settings_Info_HWItem != null)
            {
                L_ST_Settings_Info_HWItem.SensorName = (L_Combo.SelectedItem as ST_Monitor_SettingsItemSensor).Name;
            }
        }

        private T _ToEnum<T>(object o)
        {
            T enumVal = (T)Enum.Parse(typeof(T), o.ToString());
            return enumVal;
        }


        private void Check_Checked_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox L_Check = (sender as CheckBox);
            ST_HWItem_Type L_ST_HWItem_Type = _ToEnum<ST_HWItem_Type>(L_Check.Tag);
            ST_Settings_Info_HWItem L_ST_Settings_Info_HWItem = G_ST_Settings.G_Info.HWItems.Where(i => i.Type == L_ST_HWItem_Type).FirstOrDefault();
            if (L_ST_Settings_Info_HWItem != null)
            {
                L_ST_Settings_Info_HWItem.DoOverrride = L_Check.IsChecked.GetValueOrDefault(false);
            }
        }



        private void Color_ColorChanged(object sender, RoutedEventArgs e)
        {
            PortableColorPicker L_Color = (sender as PortableColorPicker);
            ST_HWItem_Type L_ST_HWItem_Type = _ToEnum<ST_HWItem_Type>(L_Color.Tag);
            ST_Settings_Info_HWItem L_ST_Settings_Info_HWItem = G_ST_Settings.G_Info.HWItems.Where(i => i.Type == L_ST_HWItem_Type).FirstOrDefault();
            if (L_ST_Settings_Info_HWItem != null)
            {
                L_ST_Settings_Info_HWItem.Color = L_Color.SelectedColor;
            }
        }

        private ST_Settings_Info_HWItem _GetSetting(ST_HWItem_Type P_ST_HWItem_Type)
        {
            return G_ST_Settings.G_Info.HWItems.Where(s => s.Type == P_ST_HWItem_Type).FirstOrDefault();
        }

        private ST_Monitor_SettingsItemSensor _GetDropSelected(ST_Settings_Info_HWItem P_Setting, List<ST_Monitor_SettingsItemSensor> P_Items)
        {
            if(P_Setting != null && P_Items != null)
            {
                return P_Items.Where(i => i.Name.Equals(P_Setting.SensorName)).FirstOrDefault();
            }

            return null;
        }

        private bool _GetCheckValue(ST_Settings_Info_HWItem P_Setting)
        {
            if (P_Setting != null)
            {
                return P_Setting.DoOverrride;
            }

            return false;
        }

        private Color _GetColorValue(ST_Settings_Info_HWItem P_Setting, Color P_Default)
        {
            if (P_Setting != null)
            {
                return P_Setting.Color;
            }

            return P_Default;
        }

        private void _Get_Dropdowns_Info()
        {
            ST_Settings_Info_HWItem L_Setting = null;

            // CPU Load
            L_Setting = _GetSetting(ST_HWItem_Type.CPULoad);
            Check_CPULoad.Tag = ST_HWItem_Type.CPULoad;
            Check_CPULoad.IsChecked = _GetCheckValue(L_Setting);
            Check_CPULoad.Checked += Check_Checked_Changed;
            Check_CPULoad.Unchecked += Check_Checked_Changed;

            Color_CPULoad.Tag = ST_HWItem_Type.CPULoad;
            Color_CPULoad.SelectedColor = _GetColorValue(L_Setting, GUI_Colors.CPU);
            Color_CPULoad.ColorChanged += Color_ColorChanged;

            Drop_CPULoad.Tag = ST_HWItem_Type.CPULoad;
            Drop_CPULoad.DisplayMemberPath = "Name";
            Drop_CPULoad.SelectedValuePath = "Identifier";
            Drop_CPULoad.ItemsSource = G_ST_Monitor._Get_Settings_Sensors(HardwareType.Cpu, SensorType.Load);
            Drop_CPULoad.SelectedItem = _GetDropSelected(L_Setting, Drop_CPULoad.ItemsSource as List<ST_Monitor_SettingsItemSensor>);
            Drop_CPULoad.SelectionChanged += Drop_SelectionChanged;

            // CPU Temp
            L_Setting = _GetSetting(ST_HWItem_Type.CPUTemp);
            Check_CPUTemp.Tag = ST_HWItem_Type.CPUTemp;
            Check_CPUTemp.IsChecked = _GetCheckValue(L_Setting);
            Check_CPUTemp.Checked += Check_Checked_Changed;
            Check_CPUTemp.Unchecked += Check_Checked_Changed;

            Color_CPU.Tag = ST_HWItem_Type.CPUTemp;
            Color_CPU.SelectedColor = _GetColorValue(L_Setting, GUI_Colors.CPU);
            Color_CPU.ColorChanged += Color_ColorChanged;

            Drop_CPUTemp.Tag = ST_HWItem_Type.CPUTemp;
            Drop_CPUTemp.DisplayMemberPath = "Name";
            Drop_CPUTemp.SelectedValuePath = "Identifier";
            Drop_CPUTemp.ItemsSource = G_ST_Monitor._Get_Settings_Sensors(HardwareType.Cpu, SensorType.Temperature);
            Drop_CPUTemp.SelectedItem = _GetDropSelected(L_Setting, Drop_CPUTemp.ItemsSource as List<ST_Monitor_SettingsItemSensor>);
            Drop_CPUTemp.SelectionChanged += Drop_SelectionChanged;

            // GPU Hot Spot
            List<ST_Monitor_SettingsItemSensor> L_GPUSensors = G_ST_Monitor._Get_Settings_Sensors(HardwareType.GpuAmd, SensorType.Temperature);
            if (L_GPUSensors == null || L_GPUSensors.Count == 0)
            {
                L_GPUSensors = G_ST_Monitor._Get_Settings_Sensors(HardwareType.GpuNvidia, SensorType.Temperature);
            }

            L_Setting = _GetSetting(ST_HWItem_Type.GPUHotSpot);
            Check_GPUHotSpot.Tag = ST_HWItem_Type.GPUHotSpot;
            Check_GPUHotSpot.IsChecked = _GetCheckValue(L_Setting);
            Check_GPUHotSpot.Checked += Check_Checked_Changed;
            Check_GPUHotSpot.Unchecked += Check_Checked_Changed;

            Color_GPUHotSpot.Tag = ST_HWItem_Type.GPUHotSpot;
            Color_GPUHotSpot.SelectedColor = _GetColorValue(L_Setting, GUI_Colors.GPU);
            Color_GPUHotSpot.ColorChanged += Color_ColorChanged;

            Drop_GPUHotSpot.Tag = ST_HWItem_Type.GPUHotSpot;
            Drop_GPUHotSpot.DisplayMemberPath = "Name";
            Drop_GPUHotSpot.SelectedValuePath = "Identifier";
            Drop_GPUHotSpot.ItemsSource = L_GPUSensors;
            Drop_GPUHotSpot.SelectionChanged += Drop_SelectionChanged;

            // GPU Temp
            L_Setting = _GetSetting(ST_HWItem_Type.GPUTemp);
            Check_GPUTemp.Tag = ST_HWItem_Type.GPUTemp;
            Check_GPUTemp.IsChecked = _GetCheckValue(L_Setting);
            Check_GPUTemp.Checked += Check_Checked_Changed;
            Check_GPUTemp.Unchecked += Check_Checked_Changed;

            Color_GPUTemp.Tag = ST_HWItem_Type.GPUTemp;
            Color_GPUTemp.SelectedColor = _GetColorValue(L_Setting, GUI_Colors.GPU);
            Color_GPUTemp.ColorChanged += Color_ColorChanged;

            Drop_GPUTemp.Tag = ST_HWItem_Type.GPUTemp;
            Drop_GPUTemp.DisplayMemberPath = "Name";
            Drop_GPUTemp.SelectedValuePath = "Identifier";
            Drop_GPUTemp.ItemsSource = L_GPUSensors;
            Drop_GPUTemp.SelectionChanged += Drop_SelectionChanged;

            // GPU VRAM Total
            L_GPUSensors = G_ST_Monitor._Get_Settings_Sensors(HardwareType.GpuAmd, SensorType.SmallData);
            if (L_GPUSensors == null || L_GPUSensors.Count == 0)
            {
                L_GPUSensors = G_ST_Monitor._Get_Settings_Sensors(HardwareType.GpuNvidia, SensorType.SmallData);
            }

            L_Setting = _GetSetting(ST_HWItem_Type.GPUHotVRAMTotal);
            Check_GPUVRAMTotal.Tag = ST_HWItem_Type.GPUHotVRAMTotal;
            Check_GPUVRAMTotal.IsChecked = _GetCheckValue(L_Setting);
            Check_GPUVRAMTotal.Checked += Check_Checked_Changed;
            Check_GPUVRAMTotal.Unchecked += Check_Checked_Changed;

            Color_GPUVRAMTotal.Tag = ST_HWItem_Type.GPUHotVRAMTotal;
            Color_GPUVRAMTotal.SelectedColor = _GetColorValue(L_Setting, GUI_Colors.GPU);
            Color_GPUVRAMTotal.ColorChanged += Color_ColorChanged;

            Drop_GPUVramTotal.Tag = ST_HWItem_Type.GPUHotVRAMTotal;
            Drop_GPUVramTotal.DisplayMemberPath = "Name";
            Drop_GPUVramTotal.SelectedValuePath = "Identifier";
            Drop_GPUVramTotal.ItemsSource = L_GPUSensors;
            Drop_GPUVramTotal.SelectionChanged += Drop_SelectionChanged;

            // GPU VRAM Used
            L_Setting = _GetSetting(ST_HWItem_Type.GPUHotVRAMUsed);
            Check_GPUVRAMUsed.Tag = ST_HWItem_Type.GPUHotVRAMUsed;
            Check_GPUVRAMUsed.IsChecked = _GetCheckValue(L_Setting);
            Check_GPUVRAMUsed.Checked += Check_Checked_Changed;
            Check_GPUVRAMUsed.Unchecked += Check_Checked_Changed;

            Color_GPUVRAMUsed.Tag = ST_HWItem_Type.GPUHotVRAMUsed;
            Color_GPUVRAMUsed.SelectedColor = _GetColorValue(L_Setting, GUI_Colors.GPU);
            Color_GPUVRAMUsed.ColorChanged += Color_ColorChanged;

            Drop_GPUVramUsed.Tag = ST_HWItem_Type.GPUHotVRAMUsed;
            Drop_GPUVramUsed.DisplayMemberPath = "Name";
            Drop_GPUVramUsed.SelectedValuePath = "Identifier";
            Drop_GPUVramUsed.ItemsSource = L_GPUSensors;
            Drop_GPUVramUsed.SelectionChanged += Drop_SelectionChanged;

            // GPU Load
            L_GPUSensors = G_ST_Monitor._Get_Settings_Sensors(HardwareType.GpuAmd, SensorType.Load);
            if (L_GPUSensors == null || L_GPUSensors.Count == 0)
            {
                L_GPUSensors = G_ST_Monitor._Get_Settings_Sensors(HardwareType.GpuNvidia, SensorType.Load);
            }

            L_Setting = _GetSetting(ST_HWItem_Type.GPULoad);
            Check_GPULoad.Tag = ST_HWItem_Type.GPULoad;
            Check_GPULoad.IsChecked = _GetCheckValue(L_Setting);
            Check_GPULoad.Checked += Check_Checked_Changed;
            Check_GPULoad.Unchecked += Check_Checked_Changed;

            Color_GPULoad.Tag = ST_HWItem_Type.GPULoad;
            Color_GPULoad.SelectedColor = _GetColorValue(L_Setting, GUI_Colors.GPU);
            Color_GPULoad.ColorChanged += Color_ColorChanged;

            Drop_GPULoad.Tag = ST_HWItem_Type.GPULoad;
            Drop_GPULoad.DisplayMemberPath = "Name";
            Drop_GPULoad.SelectedValuePath = "Identifier";
            Drop_GPULoad.ItemsSource = L_GPUSensors;
            Drop_GPULoad.SelectionChanged += Drop_SelectionChanged;

            // Memory
            L_Setting = _GetSetting(ST_HWItem_Type.MemoryUsed);
            Check_Memory.Tag = ST_HWItem_Type.MemoryUsed;
            Check_Memory.IsChecked = _GetCheckValue(L_Setting);
            Check_Memory.Checked += Check_Checked_Changed;
            Check_Memory.Unchecked += Check_Checked_Changed;

            Color_MemoryUsed.Tag = ST_HWItem_Type.MemoryUsed;
            Color_MemoryUsed.SelectedColor = _GetColorValue(L_Setting, GUI_Colors.RAM);
            Color_MemoryUsed.ColorChanged += Color_ColorChanged;

            Drop_MemoryUsed.Tag = ST_HWItem_Type.MemoryUsed;
            Drop_MemoryUsed.DisplayMemberPath = "Name";
            Drop_MemoryUsed.SelectedValuePath = "Identifier";
            Drop_MemoryUsed.ItemsSource = G_ST_Monitor._Get_Settings_Sensors(HardwareType.Memory, SensorType.Load);
            Drop_MemoryUsed.SelectionChanged += Drop_SelectionChanged;

            // Network
            L_Setting = _GetSetting(ST_HWItem_Type.Network);
            Check_NetworkDown.Tag = ST_HWItem_Type.Network;
            Check_NetworkDown.IsChecked = _GetCheckValue(L_Setting);
            Check_NetworkDown.Checked += Check_Checked_Changed;
            Check_NetworkDown.Unchecked += Check_Checked_Changed;

            Color_NetworkDown.Tag = ST_HWItem_Type.Network;
            Color_NetworkDown.SelectedColor = _GetColorValue(L_Setting, GUI_Colors.Network);
            Color_NetworkDown.ColorChanged += Color_ColorChanged;

            Drop_NetworkDown.Tag = ST_HWItem_Type.Network;
            Drop_NetworkDown.DisplayMemberPath = "Name";
            Drop_NetworkDown.SelectedValuePath = "Identifier";
            Drop_NetworkDown.ItemsSource = G_ST_Monitor._Get_Settings_Sensors(HardwareType.Network, SensorType.Load);
            Drop_NetworkDown.SelectionChanged += Drop_SelectionChanged;

            // Storage Temp
            L_Setting = _GetSetting(ST_HWItem_Type.StorageTemp);
            Check_Storage.Tag = ST_HWItem_Type.StorageTemp;
            Check_Storage.IsChecked = _GetCheckValue(L_Setting);
            Check_Storage.Checked += Check_Checked_Changed;
            Check_Storage.Unchecked += Check_Checked_Changed;

            Color_Storage.Tag = ST_HWItem_Type.StorageTemp;
            Color_Storage.SelectedColor = _GetColorValue(L_Setting, GUI_Colors.Drive);
            Color_Storage.ColorChanged += Color_ColorChanged;

            Drop_StorageTemp.Tag = ST_HWItem_Type.StorageTemp;
            Drop_StorageTemp.DisplayMemberPath = "Name";
            Drop_StorageTemp.SelectedValuePath = "Identifier";
            Drop_StorageTemp.ItemsSource = G_ST_Monitor._Get_Settings_Sensors(HardwareType.Storage, SensorType.Temperature);
            Drop_StorageTemp.SelectionChanged += Drop_SelectionChanged;

            // Swap
            L_Setting = _GetSetting(ST_HWItem_Type.SWAPUsed);
            Check_Swap.Tag = ST_HWItem_Type.SWAPUsed;
            Check_Swap.IsChecked = _GetCheckValue(L_Setting);
            Check_Swap.Checked += Check_Checked_Changed;
            Check_Swap.Unchecked += Check_Checked_Changed;

            Color_Swap.Tag = ST_HWItem_Type.SWAPUsed;
            Color_Swap.SelectedColor = _GetColorValue(L_Setting, GUI_Colors.VirMem);
            Color_Swap.ColorChanged += Color_ColorChanged;

            Drop_SWAPUsed.Tag = ST_HWItem_Type.SWAPUsed;
            Drop_SWAPUsed.DisplayMemberPath = "Name";
            Drop_SWAPUsed.SelectedValuePath = "Identifier";
            Drop_SWAPUsed.ItemsSource = Drop_MemoryUsed.ItemsSource;
            Drop_SWAPUsed.SelectionChanged += Drop_SelectionChanged;

        }
    }
}
