using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace StatsTick
{
    public class ST_Settings
    {
        const string G_SettingsFile = "settings";
        public ST_Settings_Info G_Info = new ST_Settings_Info();

        public void _LoadDefault()
        {
            try
            {
                if (File.Exists(_GetFilePath()) == true)
                {
                    _LoadSettings();
                }
                else
                {
                    File.Create(_GetFilePath()).Close();

                    G_Info.AppHeight = 600;
                    G_Info.AppWidth = 800;
                    G_Info.LeftPanelWidth = 185;
                    G_Info.RightPanelWidth = 185;

                    G_Info.HWItems.Add(new ST_Settings_Info_HWItem()
                    {
                        Color = GUI_Colors.CPU,
                        DoOverrride = false,
                        SensorName = string.Empty,
                        ShowGraph = true,
                        Type = ST_HWItem_Type.CPULoad
                    });

                    G_Info.HWItems.Add(new ST_Settings_Info_HWItem()
                    {
                        Color = GUI_Colors.CPU,
                        DoOverrride = false,
                        SensorName = string.Empty,
                        ShowGraph = true,
                        Type = ST_HWItem_Type.CPUTemp
                    });

                    G_Info.HWItems.Add(new ST_Settings_Info_HWItem()
                    {
                        Color = GUI_Colors.GPU,
                        DoOverrride = false,
                        SensorName = string.Empty,
                        ShowGraph = true,
                        Type = ST_HWItem_Type.GPULoad
                    });

                    G_Info.HWItems.Add(new ST_Settings_Info_HWItem()
                    {
                        Color = GUI_Colors.GPU,
                        DoOverrride = false,
                        SensorName = string.Empty,
                        ShowGraph = true,
                        Type = ST_HWItem_Type.GPUHotSpot
                    });

                    G_Info.HWItems.Add(new ST_Settings_Info_HWItem()
                    {
                        Color = GUI_Colors.GPU,
                        DoOverrride = false,
                        SensorName = string.Empty,
                        ShowGraph = true,
                        Type = ST_HWItem_Type.GPUHotVRAMTotal
                    });

                    G_Info.HWItems.Add(new ST_Settings_Info_HWItem()
                    {
                        Color = GUI_Colors.GPU,
                        DoOverrride = false,
                        SensorName = string.Empty,
                        ShowGraph = true,
                        Type = ST_HWItem_Type.GPUHotVRAMUsed
                    });

                    G_Info.HWItems.Add(new ST_Settings_Info_HWItem()
                    {
                        Color = GUI_Colors.GPU,
                        DoOverrride = false,
                        SensorName = string.Empty,
                        ShowGraph = true,
                        Type = ST_HWItem_Type.GPUTemp
                    });

                    G_Info.HWItems.Add(new ST_Settings_Info_HWItem()
                    {
                        Color = GUI_Colors.RAM,
                        DoOverrride = false,
                        SensorName = string.Empty,
                        ShowGraph = true,
                        Type = ST_HWItem_Type.MemoryUsed
                    });

                    G_Info.HWItems.Add(new ST_Settings_Info_HWItem()
                    {
                        Color = GUI_Colors.Network,
                        DoOverrride = false,
                        SensorName = string.Empty,
                        ShowGraph = true,
                        Type = ST_HWItem_Type.Network
                    });

                    G_Info.HWItems.Add(new ST_Settings_Info_HWItem()
                    {
                        Color = GUI_Colors.Drive,
                        DoOverrride = false,
                        SensorName = string.Empty,
                        ShowGraph = true,
                        Type = ST_HWItem_Type.StorageTemp
                    });

                    G_Info.HWItems.Add(new ST_Settings_Info_HWItem()
                    {
                        Color = GUI_Colors.VirMem,
                        DoOverrride = false,
                        SensorName = string.Empty,
                        ShowGraph = true,
                        Type = ST_HWItem_Type.SWAPUsed
                    });

                    _SaveSettings();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Message: " + ex.Message + Environment.NewLine + Environment.NewLine + ex.ToString());
            }
        }

        public void _LoadSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ST_Settings_Info));
            using (StreamReader sr = new StreamReader(_GetFilePath()))
            {
                G_Info = (ST_Settings_Info)serializer.Deserialize(sr);
            }
        }

        public void _SaveSettings()
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(typeof(ST_Settings_Info));
                serializer.Serialize(stringwriter, G_Info);
                File.WriteAllText(_GetFilePath(), stringwriter.ToString());
            }
        }


        private string _GetFilePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), G_SettingsFile);
        }
    }

    public class ST_Settings_Info
    {
        public ST_Settings_Info()
        {
            HWItems = new List<ST_Settings_Info_HWItem>();
        }

        public double AppWidth { get; set; }
        public double AppHeight { get; set; }
        public double LeftPanelWidth { get; set; }
        public double RightPanelWidth { get; set; }

        public List<ST_Settings_Info_HWItem> HWItems { get; set; }
    }

    public class ST_Settings_Info_HWItem
    {
        public ST_HWItem_Type Type { get; set; }
        public string SensorName { get; set; }
        public Color Color { get; set; }
        public bool ShowGraph { get; set; }
        public bool DoOverrride { get; set; }
    }


    public enum ST_HWItem_Type
    {
        CPUTemp,
        CPULoad,
        GPULoad,
        GPUTemp,
        GPUHotSpot,
        GPUHotVRAMTotal,
        GPUHotVRAMUsed,
        MemoryUsed,
        SWAPUsed,
        StorageTemp,
        Network,
    }

}
