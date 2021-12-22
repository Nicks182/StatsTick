using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibreHardwareMonitor.Hardware;

namespace StatsTick
{
    public partial class MainWindow
    {
        ST_Processes G_ST_Processes = null;

        private void _InitGui(ST_MonitorInfo P_ST_MonitorInfo)
        {
            _InitGui_Processes();
            _InitGui_CPUMainGraph(P_ST_MonitorInfo);
            _InitGui_RAM(P_ST_MonitorInfo);
            _InitGui_SWAP(P_ST_MonitorInfo);

            _InitGui_GPU(P_ST_MonitorInfo);
            _InitGui_Storage(P_ST_MonitorInfo);
        }

        private void _InitGui_Processes()
        {
            G_ST_Processes = new ST_Processes();
            G_ST_Processes.G_OnProcessClick += G_ST_Processes_G_OnProcessClick;
            Grid_CPUMainProcesses.Children.Add(G_ST_Processes);
        }

        private void G_ST_Processes_G_OnProcessClick(ST_MonitorInfo_PR P_ST_MonitorInfo_PR)
        {
            Win_ProcessView L_Win_ProcessView = new Win_ProcessView(P_ST_MonitorInfo_PR);
            L_Win_ProcessView.Show();
        }

        private void _InitGui_CPUMainGraph(ST_MonitorInfo P_ST_MonitorInfo)
        {
            ST_MonitorInfo_HW_Item L_HW = P_ST_MonitorInfo.HWInfo.Items.Where(l => l.Type == HardwareType.Cpu).FirstOrDefault();
            if (L_HW != null)
            {
                ST_MonitorInfo_HW_Item_Sensor L_Sensor = L_HW.Sensors.Where(s => s.Type == SensorType.Load && s.Name.Contains("CPU Total")).FirstOrDefault();
                if (L_Sensor != null)
                {
                    ST_Graph L_ST_Graph = new ST_Graph(new ST_Graph_Info
                    {
                        Caption = "CPU Total",
                        HWIdentifier = L_HW.HWIdentifier,
                        HWName = L_HW.Name,
                        SNName = L_Sensor.Name,
                        Type = L_Sensor.Type,
                        Symbol = "%",
                        Color = GUI_Colors.CPU,
                        Step = 100,
                        IsDynamicWidth = true
                    });
                    G_ST_Graphs.Add(L_ST_Graph);
                    Grid_CPUMainGraph.Children.Add(L_ST_Graph);
                }

                _InitGui_CPUCores(L_HW);
                _InitGui_CPUTemp(L_HW);
            }
        }


        private void _InitGui_CPUCores(ST_MonitorInfo_HW_Item P_ST_MonitorInfo_HW_Item)
        {
            ST_Graph L_ST_Graph = null;
            foreach (ST_MonitorInfo_HW_Item_Sensor L_Sensor in P_ST_MonitorInfo_HW_Item.Sensors.Where(s => s.Type == SensorType.Load && s.Name.Contains("Total") == false))
            {
                L_ST_Graph = new ST_Graph(new ST_Graph_Info
                {
                    Caption = L_Sensor.Name,
                    HWIdentifier = P_ST_MonitorInfo_HW_Item.HWIdentifier,
                    HWName = P_ST_MonitorInfo_HW_Item.Name,
                    SNName = L_Sensor.Name,
                    Type = L_Sensor.Type,
                    Symbol = "%",
                    Color = GUI_Colors.CPU
                });
                G_ST_Graphs.Add(L_ST_Graph);
                Panel_CPUCores.Children.Add(L_ST_Graph);
            }
        }

        private void _InitGui_CPUTemp(ST_MonitorInfo_HW_Item P_ST_MonitorInfo_HW_Item)
        {
            ST_Graph L_ST_Graph = null;
            ST_MonitorInfo_HW_Item_Sensor L_Sensor = P_ST_MonitorInfo_HW_Item.Sensors.Where(s => s.Type == SensorType.Temperature && (s.Name.Contains(" (Tctl/Tdie)") || s.Name.Contains("CPU Package"))).FirstOrDefault();
            if (L_Sensor != null)
            {
                L_ST_Graph = new ST_Graph(new ST_Graph_Info
                {
                    Caption = "CPU Temp",
                    HWIdentifier = P_ST_MonitorInfo_HW_Item.HWIdentifier,
                    HWName = P_ST_MonitorInfo_HW_Item.Name,
                    SNName = L_Sensor.Name,
                    Type = L_Sensor.Type,
                    Symbol = "°C",
                    Color = GUI_Colors.CPU
                });
                G_ST_Graphs.Add(L_ST_Graph);
                Panel_Right.Children.Add(L_ST_Graph);
            }
        }

        private void _InitGui_RAM(ST_MonitorInfo P_ST_MonitorInfo)
        {
            ST_MonitorInfo_HW_Item L_HW = P_ST_MonitorInfo.HWInfo.Items.Where(l => l.Type == HardwareType.Memory).FirstOrDefault();
            if (L_HW != null)
            {
                ST_MonitorInfo_HW_Item_Sensor L_Sensor = L_HW.Sensors.Where(s => s.Type == SensorType.Load && s.Name.Equals("Memory")).FirstOrDefault();
                if (L_Sensor != null)
                {
                    ST_Graph L_ST_Graph = new ST_Graph(new ST_Graph_Info
                    {
                        Caption = "RAM Used",
                        HWIdentifier = L_HW.HWIdentifier,
                        HWName = L_HW.Name,
                        SNName = L_Sensor.Name,
                        Type = L_Sensor.Type,
                        Symbol = "%",
                        Color = GUI_Colors.RAM
                    });
                    G_ST_Graphs.Add(L_ST_Graph);
                    Panel_Left.Children.Add(L_ST_Graph);
                }
            }
        }

        private void _InitGui_SWAP(ST_MonitorInfo P_ST_MonitorInfo)
        {
            ST_MonitorInfo_HW_Item L_HW = P_ST_MonitorInfo.HWInfo.Items.Where(l => l.Type == HardwareType.Memory).FirstOrDefault();
            if (L_HW != null)
            {
                ST_MonitorInfo_HW_Item_Sensor L_Sensor = L_HW.Sensors.Where(s => s.Type == SensorType.Load && s.Name.Equals("Virtual Memory")).FirstOrDefault();
                if (L_Sensor != null)
                {
                    ST_Graph L_ST_Graph = new ST_Graph(new ST_Graph_Info
                    {
                        Caption = "SWAP Used",
                        HWIdentifier = L_HW.HWIdentifier,
                        HWName = L_HW.Name,
                        SNName = L_Sensor.Name,
                        Type = L_Sensor.Type,
                        Symbol = "%",
                        Color = GUI_Colors.VirMem
                    });
                    G_ST_Graphs.Add(L_ST_Graph);
                    Panel_Left.Children.Add(L_ST_Graph);
                }
            }
        }

        private void _InitGui_GPU(ST_MonitorInfo P_ST_MonitorInfo)
        {
            ST_MonitorInfo_HW_Item L_HW = P_ST_MonitorInfo.HWInfo.Items.Where(l => l.Type == HardwareType.GpuAmd || l.Type == HardwareType.GpuNvidia).FirstOrDefault();
            if (L_HW != null)
            {
                ST_MonitorInfo_HW_Item_Sensor L_Sensor = L_HW.Sensors.Where(s => s.Type == SensorType.Load && s.Name.Equals("GPU Core")).FirstOrDefault();
                ST_Graph L_ST_Graph = null;
                if (L_Sensor != null)
                {
                    L_ST_Graph = new ST_Graph(new ST_Graph_Info
                    {
                        Caption = "GPU Load",
                        HWIdentifier = L_HW.HWIdentifier,
                        HWName = L_HW.Name,
                        SNName = L_Sensor.Name,
                        Type = L_Sensor.Type,
                        Symbol = "%",
                        Color = GUI_Colors.GPU
                    });
                    G_ST_Graphs.Add(L_ST_Graph);
                    Panel_GPU.Children.Add(L_ST_Graph);
                }
                _InitGui_GPUTemp(L_HW);
                _InitGui_GPUVRAM(L_HW);
            }
        }

        private void _InitGui_GPUVRAM(ST_MonitorInfo_HW_Item P_HW)
        {
            ST_MonitorInfo_HW_Item_Sensor L_Sensor = P_HW.Sensors.Where(s => s.Type == SensorType.SmallData && s.Name.Equals("GPU Memory Used")).FirstOrDefault();
            if (L_Sensor != null)
            {
                ST_Graph L_ST_Graph = new ST_Graph(new ST_Graph_Info
                {
                    Caption = "GPU VRAM Load",
                    HWIdentifier = P_HW.HWIdentifier,
                    HWName = P_HW.Name,
                    SNName = L_Sensor.Name,
                    Type = L_Sensor.Type,
                    Symbol = "%",
                    Color = GUI_Colors.GPU
                });
                G_ST_Graphs.Add(L_ST_Graph);
                Panel_GPU.Children.Add(L_ST_Graph);
            }
        }

        private void _InitGui_GPUTemp(ST_MonitorInfo_HW_Item P_HW)
        {
            ST_MonitorInfo_HW_Item_Sensor L_Sensor = P_HW.Sensors.Where(s => s.Type == SensorType.Temperature && s.Name.Equals("GPU Core")).FirstOrDefault();
            ST_Graph L_ST_Graph = null;
            if (L_Sensor != null)
            {
                L_ST_Graph = new ST_Graph(new ST_Graph_Info
                {
                    Caption = "GPU Temp",
                    HWIdentifier = P_HW.HWIdentifier,
                    HWName = P_HW.Name,
                    SNName = L_Sensor.Name,
                    Type = L_Sensor.Type,
                    Symbol = "°C",
                    Color = GUI_Colors.GPU
                });
                G_ST_Graphs.Add(L_ST_Graph);
                Panel_GPU.Children.Add(L_ST_Graph);
            }

            L_Sensor = P_HW.Sensors.Where(s => s.Type == SensorType.Temperature && s.Name.Equals("GPU Hot Spot")).FirstOrDefault();
            if (L_Sensor != null)
            {
                L_ST_Graph = new ST_Graph(new ST_Graph_Info
                {
                    Caption = "GPU Hot Spot",
                    HWIdentifier = P_HW.HWIdentifier,
                    HWName = P_HW.Name,
                    SNName = L_Sensor.Name,
                    Type = L_Sensor.Type,
                    Symbol = "°C",
                    Color = GUI_Colors.GPU
                });
                G_ST_Graphs.Add(L_ST_Graph);
                Panel_GPU.Children.Add(L_ST_Graph);
            }
        }

        

        private void _InitGui_Storage(ST_MonitorInfo P_ST_MonitorInfo)
        {
            ST_Graph L_ST_Graph = null;
            ST_MonitorInfo_HW_Item_Sensor L_Sensor = null;
            foreach (ST_MonitorInfo_HW_Item L_HW in P_ST_MonitorInfo.HWInfo.Items.Where(l => l.Type == HardwareType.Storage))
            {
                L_Sensor = L_HW.Sensors.Where(s => s.Type == SensorType.Temperature && s.Name.Equals("Temperature")).FirstOrDefault();
                if (L_Sensor != null)
                {
                    L_ST_Graph = new ST_Graph(new ST_Graph_Info
                    {
                        Caption = L_HW.Name,
                        HWIdentifier = L_HW.HWIdentifier,
                        HWName = L_HW.Name,
                        SNName = L_Sensor.Name,
                        Type = L_Sensor.Type,
                        Symbol = "°C",
                        Color = GUI_Colors.Drive
                    });
                    G_ST_Graphs.Add(L_ST_Graph);
                    Panel_Right.Children.Add(L_ST_Graph);
                }

            }
        }


    }
}
