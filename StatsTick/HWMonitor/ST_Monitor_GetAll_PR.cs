using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace StatsTick
{
    partial class ST_Monitor
    {
        const string G_ProcessQuery = "SELECT * FROM Win32_PerfFormattedData_PerfProc_Process WHERE Name <> '_Total' AND Name <> 'Idle'";
        int G_ProcessorCount = Environment.ProcessorCount;

        private ST_MonitorInfo_PR _GetAll_PR(DateTime P_TimeStamp)
        {
            ST_MonitorInfo_PR L_Info = new ST_MonitorInfo_PR();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", G_ProcessQuery);

            foreach (ManagementObject queryObj in searcher.Get())
            {

                //StreamWriter SW;
                //SW = File.CreateText("c:\\MyTextFile.txt");//I can't get the StreamWriter to work

                //SW.WriteLine("Name: ", queryObj["Name"]);//This fails in that it does not write to the file and I don't understand why
                //Console.WriteLine("ProcessID: {0}", queryObj["IDProcess"]);
                //Console.WriteLine("Handles: {0}", queryObj["HandleCount"]);
                //Console.WriteLine("Threads: {0}", queryObj["ThreadCount"]);
                //Console.WriteLine("Memory: {0}", queryObj["WorkingSetPrivate"]);
                //Console.WriteLine("CPU%: {0}", queryObj["PercentProcessorTime"]);
                //Console.Read();
                //SW.Close();

                L_Info.Items.Add(new ST_MonitorInfo_PR_Item()
                {
                    ProcesId = Convert.ToDouble(queryObj["IDProcess"]),
                    Handles = Convert.ToDouble(queryObj["HandleCount"]),
                    Threads = Convert.ToDouble(queryObj["ThreadCount"]),
                    Memory = Convert.ToDouble(queryObj["WorkingSetPrivate"]),
                    MemoryMB = Convert.ToDouble(queryObj["WorkingSetPrivate"]) / 1024 / 1024,
                    Name = queryObj["Name"].ToString(),
                    //CommandLine = queryObj["CommandLine"].ToString(),
                    Value = Convert.ToDouble(queryObj["PercentProcessorTime"]) / G_ProcessorCount // Have to devide by thread count or we get values way above 100%. This is how windows stores the usage value.
                });

            }

            return L_Info;
        }

    }
}
