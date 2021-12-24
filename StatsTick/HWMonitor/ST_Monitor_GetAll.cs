using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsTick
{
    public partial class ST_Monitor
    {
        public ST_MonitorInfo _GetAll()
        {
            ST_MonitorInfo L_Info = new ST_MonitorInfo();
            DateTime L_TimeStamp = DateTime.Now;
            
            L_Info.HWInfo = _GetAll_HW(L_TimeStamp);

            if (G_TrackProcess == true)
            {
                L_Info.PRInfo = _GetAll_PR(L_TimeStamp);
            }
            else
            {
                L_Info.PRInfo = null;
            }
            return L_Info;
        }


        public StringBuilder _GetDataDump()
        {
            StringBuilder L_HWData = new StringBuilder();
            foreach (IHardware hardware in G_Computer.Hardware)
            {
                L_HWData.Append(hardware.Identifier + " - " + hardware.HardwareType.ToString() + " - " + hardware.Name + Environment.NewLine);

                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    L_HWData.Append("\t" + hardware.Identifier + " - " + hardware.HardwareType.ToString() + " - " + hardware.Name + Environment.NewLine);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        L_HWData.Append("\t\t" + sensor.Identifier + "-" + sensor.SensorType.ToString() + " - " + sensor.Name + ": " + sensor.Value + Environment.NewLine);
                    }
                }

                foreach (ISensor sensor in hardware.Sensors)
                {
                    L_HWData.Append("\t" + sensor.Identifier + "-" + sensor.SensorType.ToString() + " - " + sensor.Name + ": " + sensor.Value + Environment.NewLine);
                }
            }

            return L_HWData;
        }

    }
}
