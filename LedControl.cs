using System;
using System.Collections.Generic;
using System.Text;

namespace ExcaliburKlavye
{
    internal class LedControl
    {
        private MyWMIBase _WMI = new MyWMIBase();

        public void SetSingleDevice(uint SltZone, uint LEDData32)
        {
            _WMI.data.clear();
            _WMI.data.a0 = (ushort)64256;
            _WMI.data.a1 = (ushort)256;
            _WMI.data.a2 = SltZone;
            _WMI.data.a3 = LEDData32;
            _WMI.ACPIPerformSMI();
        }

    }
}
