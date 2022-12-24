using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace ExcaliburKlavye
{
    internal class MyWMIBase
    {
        public static bool m_GamingWmi = true;
        private const string WMINamespace = "root\\wmi";
        private const string WMIClassRW_GMWMI = "RW_GMWMI";
        private const int BUFFER_BYTES_SIZE = 32;
        public SMI_STRUCT_S data;
        public static byte[] BufferBytes_output_check;
        public static byte[] BufferBytes_input_check;

        public static bool Init()
        {
            MyWMIBase.check_gaming_wmi();
            return MyWMIBase.m_GamingWmi;
        }

        private static void check_gaming_wmi()
        {
            if (MyWMIBase.ReadGMWMI() == null)
                MyWMIBase.m_GamingWmi = false;
            else
                MyWMIBase.m_GamingWmi = true;
        }

        public bool ACPIPerformSMI() => this.ACPIPerformSMI(ref this.data);


        private static byte[] ReadGMWMI()
        {
            try
            {
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher(new ManagementScope("root\\wmi"), (ObjectQuery)new SelectQuery("RW_GMWMI")).Get().GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        byte[] data;
                        MyWMIBase.BufferBytes_output_check = data = (byte[])enumerator.Current["BufferBytes"];
                        byte[] numArray = new byte[32];
                        ref byte[] local = ref numArray;
                        SMI_STRUCT_S.byte_swap(data, ref local);
                        return numArray;
                    }
                }
            }
            catch (ManagementException ex)
            {
                Console.WriteLine((object)ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine((object)ex);
            }
            return (byte[])null;
        }

        private static bool WriteGMWMI(ref byte[] BufferBytes)
        {
            try
            {
                if (BufferBytes.GetLength(0) != 32)
                {
                    Console.WriteLine("BufferBytes.GetLength(0) != BufferBytesSize");
                    return false;
                }
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher(new ManagementScope("root\\wmi"), (ObjectQuery)new SelectQuery("RW_GMWMI")).Get().GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        ManagementObject current = (ManagementObject)enumerator.Current;
                        byte[] data2 = new byte[32];
                        SMI_STRUCT_S.byte_swap(BufferBytes, ref data2);
                        MyWMIBase.BufferBytes_input_check = data2;
                        current.SetPropertyValue(nameof(BufferBytes), (object)data2);
                        current.Put();
                        return true;
                    }
                }
            }
            catch (ManagementException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public bool ACPIPerformSMI(ref SMI_STRUCT_S a)
        {
            if (!MyWMIBase.m_GamingWmi)
                return false;
            //MyDebug.show(a.debug_arg);
            bool flag = false;
            byte[] BufferBytes = new byte[32];
            int num1 = Marshal.SizeOf<SMI_STRUCT_S>(a);
            if (num1 != 32)
                Console.WriteLine("size error");
            IntPtr num2 = Marshal.AllocHGlobal(num1);
            Marshal.StructureToPtr<SMI_STRUCT_S>(a, num2, true);
            Marshal.Copy(num2, BufferBytes, 0, num1);
            MyWMIBase.WriteGMWMI(ref BufferBytes);
            byte[] source = MyWMIBase.ReadGMWMI();
            if (source != null)
            {
                Marshal.Copy(source, 0, num2, num1);
                a = (SMI_STRUCT_S)Marshal.PtrToStructure(num2, typeof(SMI_STRUCT_S));
                flag = true;
            }
            //MyDebug.show(a.debug_res);
            Marshal.FreeHGlobal(num2);
            return flag;
        }
    }
}
