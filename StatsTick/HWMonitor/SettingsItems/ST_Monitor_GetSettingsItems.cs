using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibreHardwareMonitor.Hardware;

namespace StatsTick
{
    public partial class ST_Monitor
    {
        public List<ST_Monitor_SettingsItemSensor> _Get_Settings_Sensors(HardwareType P_HardwareType, SensorType L_SensorType)
        {
            List<ST_Monitor_SettingsItemSensor> L_Sensors = new List<ST_Monitor_SettingsItemSensor>();
            IHardware L_Hardware = G_Computer.Hardware.Where(h => h.HardwareType == P_HardwareType).FirstOrDefault();
            if (L_Hardware != null)
            {
                foreach (var L_Sensor in L_Hardware.Sensors.Where(s => s.SensorType == L_SensorType))
                {
                    L_Sensors.Add(new ST_Monitor_SettingsItemSensor()
                    {
                        Identifier = L_Sensor.Identifier.ToString(),
                        Name = L_Sensor.Name
                    });
                }
            }
            return L_Sensors;
        }

    }


    public class ST_Monitor_SettingsItemSensor
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
    }
}
