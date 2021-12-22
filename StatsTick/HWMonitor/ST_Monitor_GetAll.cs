using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsTick
{
    partial class ST_Monitor
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



    }
}
