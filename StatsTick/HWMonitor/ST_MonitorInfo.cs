using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibreHardwareMonitor.Hardware;

namespace StatsTick
{
    public class ST_MonitorInfo
    {
        public ST_MonitorInfo_HW HWInfo { get; set; }
        public ST_MonitorInfo_PR PRInfo { get; set; }
    }


    public class ST_MonitorInfo_HW
    {
        public ST_MonitorInfo_HW()
        {
            Items = new List<ST_MonitorInfo_HW_Item>();
        }
        public DateTime TimeStamp { get; set; }
        public List<ST_MonitorInfo_HW_Item> Items { get; set; }
    }

    public class ST_MonitorInfo_HW_Item
    {
        public long Id { get; set; }
        public string HWIdentifier { get; set; }
        public string Name { get; set; }
        public HardwareType Type { get; set; }
        public DateTime TimeStamp { get; set; }

        public List<ST_MonitorInfo_HW_Item_Sensor> Sensors { get; set; }
    }

    public partial class ST_MonitorInfo_HW_Item_Sensor
    {
        public string HWIdentifier { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public SensorType Type { get; set; }
    }

    public partial class ST_MonitorInfo_PR
    {
        public ST_MonitorInfo_PR()
        {
            Items = new List<ST_MonitorInfo_PR_Item>();
        }
        public DateTime TimeStamp { get; set; }
        public List<ST_MonitorInfo_PR_Item> Items { get; set; }
    }

    public partial class ST_MonitorInfo_PR_Item
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public double Memory { get; set; }
        public double MemoryMB { get; set; }
        public double ProcesId { get; set; }
        public double Handles { get; set; }
        public double Threads { get; set; }
        public string CommandLine { get; set; }
        
    }

}
