//Comment this definition to avoid overflow checking.
//#define OVERFLOW

using System;

namespace FixedMath {

    /// <summary>
    /// Structure for fixed point calculations.
    /// The representation uses 64 bits to store the number, with a shift of 16 bits.
    /// The number is stored as  1 (sign) | 15 + 16 + 16 = 47 (integer part) | 16 (decimal part)
    /// Sum and subtraction don't need shifting, so they can use up to 47 bits without problems (see RealMaxVal),
    /// while multiplication and division require an extra shifting to maintain the scale, reducing the range to 31
    /// (see SafeMinVal).
    /// </summary>
    public struct Fix32 : IEquatable<Fix32>, IComparable<Fix32> {

        public const int SHIFT = 16;
        public const int NBITS = 64;
        public const int NSQRT = 8;
        public const long ONE = 1L << SHIFT;
        public const long RealMaxValue = 140737488355327L;  //max number reachable with sums  (2^37-1)
        public const long RealMinValue = -140737488355328L; //min number reachable with subs -(2^37)
        public const long SafeMaxValue = 2147483647L;       //max number reachable with mul/div  (2^31-1)
        public const long SafeMinValue = -2147483648L;      //min number reachable with mul/div -(2^31)

        public static readonly Fix32 MaxValue = new Fix32(long.MaxValue);
        public static readonly Fix32 MinValue = new Fix32(long.MinValue);
        public static readonly Fix32 One = new Fix32(ONE);
        public static readonly Fix32 Zero = new Fix32(0L);
        private static readonly Fix32 Half = (One >> 1);
        private static readonly Fix32 ThreeHalfs = (One + Half);

        private long rawValue;

        #region Constructors

        public Fix32(int value) {
            this.rawValue = ((long)value) << SHIFT;
        }

        public Fix32(float value) {
            this.rawValue = (long)(value * ONE);
        }

        public Fix32(double value) {
            this.rawValue = (long)(value * ONE);
        }

        private Fix32(long value) {
            this.rawValue = value;
        }

        #endregion

        #region Operators

        public static Fix32 operator +(Fix32 a, Fix32 b) {
            long sum = a.rawValue + b.rawValue;
#if (OVERFLOW)
            if (((a.rawValue ^ sum) & (b.rawValue ^ sum)) >> (NBITS - 1) != 0) {
                sum = a.rawValue > 0 ? long.MaxValue : long.MinValue;
            }
#endif
            return new Fix32(sum);
        }

        public static Fix32 operator -(Fix32 a, Fix32 b) {
            long diff = a.rawValue - b.rawValue;
#if (OVERFLOW)
            if ((~(a.rawValue ^ b.rawValue) & (diff & a.rawValue)) < 0) {
                diff = a.rawValue > 0 ? long.MaxValue : long.MinValue;
            }
#endif
            return new Fix32(diff);
        }

        public static Fix32 operator -(Fix32 val) {
            return val == MinValue ? MaxValue : new Fix32(-val.rawValue);
        }

        public static Fix32 operator *(Fix32 a, Fix32 b) {
#if (OVERFLOW)
            if (b.rawValue > ONE && a.rawValue > SafeMaxValue / b.rawValue)
                throw new System.OverflowException();
#endif
            return new Fix32((a.rawValue * b.rawValue) >> SHIFT);
        }

        public static Fix32 operator /(Fix32 a, Fix32 b) {
            return new Fix32((a.rawValue << SHIFT) / b.rawValue);
        }

        public static Fix32 operator %(Fix32 a, Fix32 b) {
            return new Fix32(a.rawValue % b.rawValue);
        }

        public static bool operator ==(Fix32 a, Fix32 b) {
            return a.rawValue == b.rawValue;
        }

        public static bool operator !=(Fix32 a, Fix32 b) {
            return !(a.rawValue == b.rawValue);
        }

        public static bool operator >(Fix32 a, Fix32 b) {
            return a.rawValue > b.rawValue;
        }

        public static bool operator <(Fix32 a, Fix32 b) {
            return a.rawValue < b.rawValue;
        }

        public static bool operator >=(Fix32 a, Fix32 b) {
            return a.rawValue >= b.rawValue;
        }

        public static bool operator <=(Fix32 a, Fix32 b) {
            return a.rawValue <= b.rawValue;
        }

        public static Fix32 operator <<(Fix32 n, int shift) {
            return new Fix32(n.rawValue << shift);
        }

        public static Fix32 operator >>(Fix32 n, int shift) {
            return new Fix32(n.rawValue >> shift);
        }

        #endregion

        #region Casting

        public static explicit operator Fix32(int value) {
            return new Fix32(value);
        }

        public static explicit operator int(Fix32 value) {
            return (int) (value.rawValue >> SHIFT);
        }

        public static explicit operator Fix32(long value) {
            return new Fix32(value << SHIFT);
        }

        public static explicit operator long(Fix32 value) {
            return value.rawValue >> SHIFT;
        }

        public static explicit operator Fix32(float value) {
            return new Fix32(value);
        }

        public static explicit operator float(Fix32 value) {
            return (float)value.rawValue / ONE;
        }

        public static explicit operator Fix32(double value) {
            return new Fix32(value);
        }

        public static explicit operator double(Fix32 value) {
            return (double)value.rawValue / ONE;
        }

        #endregion

        #region Inherited

        public bool Equals(Fix32 other) {
            return this == other;
        }

        public int CompareTo(Fix32 other) {
            return (int)(this - other);
        }

        public override bool Equals(object obj) {
            return (obj is Fix32) ? (this == ((Fix32)obj)) : false;
        }

        public override int GetHashCode() {
            return rawValue.GetHashCode();
        }

        public override string ToString() {
            return rawValue.ToString();
        }

        #endregion

        #region MathFunctions

        /// <summary>
        /// Returns the absolute value of the given fixed point number.
        /// </summary>
        /// <param name="val">the given value</param>
        /// <returns>the absolute value</returns>
        public static Fix32 Abs(Fix32 val) {
            long mask = val.rawValue >> (NBITS-1);
            return new Fix32((val.rawValue + mask) ^ mask);
        }

        /// <summary>
        /// Clamps the number between the given maximum and minimum.
        /// </summary>
        /// <param name="val">the given value</param>
        /// <returns>the clamped value</returns>
        public static Fix32 Clamp(Fix32 val, Fix32 min, Fix32 max) {
            return (val > max) ? max : (val < min) ? min : val;
        }

        /// <summary>
        /// Gets the maximum value between the given ones.
        /// </summary>
        /// <param name="a">first value</param>
        /// <param name="b">second value</param>
        /// <returns>max value</returns>
        public static Fix32 Max(Fix32 a, Fix32 b) {
            return (a > b) ? a : b;
        }

        /// <summary>
        /// Gets the minimum value between the given ones.
        /// </summary>
        /// <param name="a">first value</param>
        /// <param name="b">second value</param>
        /// <returns>min value</returns>
        public static Fix32 Min(Fix32 a, Fix32 b) {
            return (a < b) ? a : b;
        }

        /// <summary>
        /// Approximates the squared root, with a precision determined by the 
        /// amount of iterations.
        /// </summary>
        /// <param name="val">the given value</param>
        /// <param name="iterations">number of iterations</param>
        /// <returns>Approximation of the squared root.</returns>
        public static Fix32 Sqrt(Fix32 val, int iterations) {
            if (val.rawValue < 0)
                throw new ArithmeticException("Negative value");

            if (val.rawValue == 0)
                return Fix32.Zero;

            Fix32 res = val + Fix32.One >> 1;
            for (uint i = 0; i < iterations; i++) {
                res = (res + (val / res)) >> 1;
            }
            if (res.rawValue < 0) {
                throw new ArithmeticException("Overflow");
            }
            return res;
        }

        /// <summary>
        /// Computes the squared root with a fixed amount of iterations.
        /// </summary>
        /// <param name="val">the given value</param>
        /// <returns>(Approximation of) the squared root of the value</returns>
        public static Fix32 Sqrt(Fix32 val) {
            return Sqrt(val, NSQRT);
        }

        /// <summary>
        /// "relatively fast" inverse square root, inspired by John Carmack's
        /// Fast Inverse Sqrt from Quake.
        /// It uses the Newton's method of approximation to guess a value pretty close to the exact one.
        /// </summary>
        /// <param name="val">the given value</param>
        /// <returns>approximation of the inverse square root</returns>
        public static Fix32 InvSqrt(Fix32 val) {
            Fix32 halfval = val >> 1;
            Fix32 g = halfval;
            for (ushort i = 0; i < 8; i++) {
                g = g * (ThreeHalfs - halfval * g * g);
            }
            return g;
        }

        #endregion
    }
}
