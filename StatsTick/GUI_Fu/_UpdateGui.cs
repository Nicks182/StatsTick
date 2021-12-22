using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsTick
{
    public partial class MainWindow
    {
        private void _UpdateGui()
        {
            G_ST_Processes._SetProcesses(G_ST_MonitorInfo);

            ST_Graph L_ST_Graph = null;
            foreach (var L_HW in G_ST_MonitorInfo.HWInfo.Items)
            {
                foreach (var L_Sensor in L_HW.Sensors)
                {
                    L_ST_Graph = G_ST_Graphs.Where(g => g.G_ST_Graph_Info.HWIdentifier == L_HW.HWIdentifier && g.G_ST_Graph_Info.SNName == L_Sensor.Name && g.G_ST_Graph_Info.Type == L_Sensor.Type).FirstOrDefault();
                    if (L_ST_Graph != null)
                    {
                        if (L_Sensor.Name == "D3D Dedicated Memory Used")
                        {
                            L_ST_Graph._Update(_Get_GPU_Meme_Used(L_HW));
                        }
                        else
                        {
                            L_ST_Graph._Update(L_Sensor.Value);
                        }
                    }
                }
            }
        }

        private double _Get_GPU_Meme_Used(ST_MonitorInfo_HW_Item P_HW)
        {
            ST_MonitorInfo_HW_Item_Sensor L_MemTotal = P_HW.Sensors.Where(s => s.Type == SensorType.SmallData && s.Name == "GPU Memory Total").FirstOrDefault();
            ST_MonitorInfo_HW_Item_Sensor L_MemUsed = P_HW.Sensors.Where(s => s.Type == SensorType.SmallData && s.Name == "GPU Memory Used").FirstOrDefault();

            if (L_MemTotal != null && L_MemUsed != null)
            {
                return (L_MemUsed.Value / L_MemTotal.Value) * 100;
            }


            return 0;
        }

    }
}
