
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibreHardwareMonitor.Hardware;

namespace StatsTick
{
    partial class ST_Monitor
    {

        private ST_MonitorInfo_HW _GetAll_HW(DateTime P_TimeStamp)
        {
            ST_MonitorInfo_HW L_Info = new ST_MonitorInfo_HW();
            L_Info.TimeStamp = P_TimeStamp;

            G_Computer.Accept(new UpdateVisitor());

            foreach (IHardware L_Hardware in G_Computer.Hardware)
            {
                L_Info.Items.Add(_GetHardware(L_Hardware, P_TimeStamp));
            }

            return L_Info;
        }


        private ST_MonitorInfo_HW_Item _GetHardware(IHardware P_Hardware, DateTime P_Timestamp)
        {
            return new ST_MonitorInfo_HW_Item()
            {
                Name = P_Hardware.Name,
                HWIdentifier = P_Hardware.Identifier.ToString(),
                Type = P_Hardware.HardwareType,
                TimeStamp = P_Timestamp,
                Sensors = _GetHardware_Sensors(P_Hardware, P_Timestamp)
            };
        }

        private List<ST_MonitorInfo_HW_Item_Sensor> _GetHardware_Sensors(IHardware P_Hardware, DateTime P_Timestamp)
        {
            List<ST_MonitorInfo_HW_Item_Sensor> L_HW_SensorLogs = new List<ST_MonitorInfo_HW_Item_Sensor>();

            foreach (ISensor sensor in P_Hardware.Sensors)
            {
                L_HW_SensorLogs.Add(new ST_MonitorInfo_HW_Item_Sensor()
                {
                    Name = sensor.Name,
                    HWIdentifier = sensor.Identifier.ToString(),
                    Type = sensor.SensorType,
                    Value = _GetHardware_Sensor_Value(sensor.Value)
                });
            }

            return L_HW_SensorLogs;
        }

        private double _GetHardware_Sensor_Value(float? P_Value)
        {
            if (P_Value == null || P_Value.ToString().ToLower() == "nan")
            {
                return 0;
            }

            return Convert.ToDouble(P_Value);
        }

    }
}
