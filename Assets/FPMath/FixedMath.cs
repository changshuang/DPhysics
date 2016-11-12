using System;

namespace FixedPointMath {
    public class FixedMath {

        public static intf Abs(intf value) {
            return (value > 0) ? value : -value;
        }

        public static intf Clamp(intf value, intf min, intf max) {
            if (value < min) value = min;
            if (value > max) value = max;
            return value;
        }

        public static intf Max(intf a, intf b) {
            return (a >= b) ? a : b;
        }

        public static intf Min(intf a, intf b) {
            return (a < b) ? a : b;
        }

        public static int Sign(intf n) {
            return (n > 0) ? 1 : -1;
        }

        public static intf Sqrt(intf f, int NumberOfIterations) {
            if (f.rawValue < 0) //NaN in Math.Sqrt
                throw new ArithmeticException("Input Error");
            if (f.rawValue == 0)
                return (intf)0;
            intf k = f + intf.oneF >> 1;
            for (int i = 0; i < NumberOfIterations; i++)
                k = (k + (f / k)) >> 1;

            if (k.rawValue < 0)
                throw new ArithmeticException("Overflow");
            else
                return k;
        }

        public static intf Sqrt(intf f) {
            byte numberOfIterations = 8;
            if (f.rawValue > 0x64000)
                numberOfIterations = 12;
            if (f.rawValue > 0x3e8000)
                numberOfIterations = 16;
            return Sqrt(f, numberOfIterations);
        }
    }
}
