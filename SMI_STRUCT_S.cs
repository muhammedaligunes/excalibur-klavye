using System;
using System.Collections.Generic;
using System.Text;

namespace ExcaliburKlavye
{
    internal struct SMI_STRUCT_S
    {
        public ushort a0;
        public ushort a1;
        public uint a2;
        public uint a3;
        public uint a4;
        public uint a5;
        public uint a6;
        public uint rev0;
        public uint rev1;


        public static void byte_swap(byte[] data, ref byte[] data2, bool swap = false)
        {
            if (swap)
            {
                data2[0] = data[1];
                data2[1] = data[0];
                data2[2] = data[3];
                data2[3] = data[2];
                for (int index = 0; index < 7; ++index)
                {
                    int num = index * 4;
                    data2[num + 4] = data[num + 7];
                    data2[num + 5] = data[num + 6];
                    data2[num + 6] = data[num + 5];
                    data2[num + 7] = data[num + 4];
                }
            }
            else
            {
                data2[0] = data[0];
                data2[1] = data[1];
                data2[2] = data[2];
                data2[3] = data[3];
                for (int index = 0; index < 7; ++index)
                {
                    int num = index * 4;
                    data2[num + 4] = data[num + 4];
                    data2[num + 5] = data[num + 5];
                    data2[num + 6] = data[num + 6];
                    data2[num + 7] = data[num + 7];
                }
            }
        }

        public void clear()
        {
            this.a0 = this.a1 = (ushort)0;
            this.a2 = this.a3 = this.a4 = this.a5 = this.a6 = 0U;
            this.rev0 = this.rev1 = 0U;
        }
    }
}
