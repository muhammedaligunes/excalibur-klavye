using System;

namespace ExcaliburKlavye
{
    public class ExcaliburKlavye
    {
        public enum MOD { STATIK = 1, FLASH = 2, NEFES_ALMA = 3, KALP_ATISI = 4, GOKKUSAGI = 5, RASTGELE_GOKKUSAGI = 6 };

        public enum BOLGE { SOL = 3, ORTA = 4, SAG = 5, HEPSI = 6 };

        public enum PARLAKLIK { KAPALI = 0, DUSUK = 1, YUKSEK = 2 };

        public static ExcaliburKlavye instance;

        private LedControl ledControl = new LedControl();

        public ExcaliburKlavye()
        {
            
        }

        public static ExcaliburKlavye getInstance()
        {
            if (instance == null)
                instance = new ExcaliburKlavye();
            return instance;
        }


        public void renkAyarla(BOLGE bolge, MOD mod, PARLAKLIK parlaklik, byte r, byte g, byte b)
        {
            uint color = CombineLEDData((byte)mod, (byte)parlaklik, r, g, b);
            ledControl.SetSingleDevice((byte)bolge, color);
        }

        public void renkAyarla(BOLGE bolge, MOD mod, PARLAKLIK parlaklik, string rgbKod)
        {
            uint rgb = Convert.ToUInt32(rgbKod, 16);
            byte b = (byte)(rgb & 0xFF);
            byte g = (byte)(rgb >> 8 & 0xFF);
            byte r = (byte)(rgb >> 16 & 0xFF);
            uint color = CombineLEDData((byte)mod, (byte)parlaklik, r, g, b);
            ledControl.SetSingleDevice((byte)bolge, color);

        }


        public static uint CombineLEDData(byte mod, byte alpha, byte r, byte g, byte b)
        {

            uint DataArg3 = mod * 16U + alpha;
            DataArg3 *= 16777216U;
            DataArg3 += (uint)r * 65536U;
            DataArg3 += (uint)g * 256U;
            DataArg3 += (uint)b;
            return DataArg3;
        }

    }
}
