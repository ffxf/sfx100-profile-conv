using System;

namespace SFB_Profile_Converter
{
    class ScaleParams {
        private int scaleGeneric(int input, int srcLo, int scrHi, int destLo, int destHi)
        {
            return destLo + (Int32)((double)(input - srcLo)
                                   * (double)(destHi - destLo)
                                   / (double)(scrHi - srcLo));
        }

        public int scaleToNewAccel(int input)
        {
            return scaleGeneric(input, 1, 200, 0, 100);
        }

        public int scaleToNewSpeed(int input)
        {
            return scaleGeneric(input, 63514, 65504, 0, 100);
        }

        public int scaleToNewIntensity(int input)
        {
            return scaleGeneric(input, 0, 200, 0, 100);
        }

        public int scaleToNewSmoothness(int input)
        {
            return scaleGeneric(input, 30, 60, 0, 100);
        }
    }
}