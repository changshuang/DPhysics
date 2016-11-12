namespace FixedPointMath {
    public struct intf {
        public long rawValue;
        public const int SHIFT_AMOUNT = 12; //12 is 4096

        public const long oneL = 1 << SHIFT_AMOUNT;
        public const int oneI = 1 << SHIFT_AMOUNT;
        public static intf oneF = intf.Create(1, true);

        #region Constructors
        public static intf Create(long rawValue, bool useMultiple) {
            intf fInt;
            fInt.rawValue = rawValue;
            if (useMultiple)
                fInt.rawValue = fInt.rawValue << SHIFT_AMOUNT;
            return fInt;
        }

        public static intf Create(double value) {
            intf fInt;
            value *= (double)oneL;
            fInt.rawValue = (int) value;
            return fInt;
        }

        public static intf Create(float value) {
            intf fInt;
            value *= (float)oneL;
            fInt.rawValue = (int)value;
            return fInt;
        }
        #endregion

        public int IntValue {
            get { return (int)(this.rawValue >> SHIFT_AMOUNT); }
        }

        public int ToInt() {
            return (int)(this.rawValue >> SHIFT_AMOUNT);
        }

        public double ToDouble() {
            return (double)this.rawValue / (double)oneL;
        }

        public float ToFloat() {
            return (float)this.ToDouble();
        }

        public intf Inverse {
            get { return intf.Create(-this.rawValue, false); }
        }

        #region FromParts
        /// <summary>
        /// Create a fixed-int number from parts.  For example, to create 1.5 pass in 1 and 500.
        /// </summary>
        /// <param name="PreDecimal">The number above the decimal.  For 1.5, this would be 1.</param>
        /// <param name="PostDecimal">The number below the decimal, to three digits.  
        /// For 1.5, this would be 500. For 1.005, this would be 5.</param>
        /// <returns>A fixed-int representation of the number parts</returns>
        public static intf FromParts(int PreDecimal, int PostDecimal) {
            intf f = intf.Create(PreDecimal, true);
            if (PostDecimal != 0)
                f.rawValue += (intf.Create(PostDecimal) / 1000).rawValue;

            return f;
        }
        #endregion

        #region *
        public static intf operator *(intf one, intf other) {
            intf fInt;
            fInt.rawValue = (one.rawValue * other.rawValue) >> SHIFT_AMOUNT;
            return fInt;
        }

        public static intf operator *(intf one, int multi) {
            return one * (intf)multi;
        }

        public static intf operator *(int multi, intf one) {
            return one * (intf)multi;
        }
        #endregion

        #region /
        public static intf operator /(intf one, intf other) {
            intf fInt;
            fInt.rawValue = (one.rawValue << SHIFT_AMOUNT) / (other.rawValue);
            return fInt;
        }

        public static intf operator /(intf one, int divisor) {
            return one / (intf)divisor;
        }

        public static intf operator /(int divisor, intf one) {
            return (intf)divisor / one;
        }
        #endregion

        #region %
        public static intf operator %(intf one, intf other) {
            intf fInt;
            fInt.rawValue = (one.rawValue) % (other.rawValue);
            return fInt;
        }

        public static intf operator %(intf one, int divisor) {
            return one % (intf)divisor;
        }

        public static intf operator %(int divisor, intf one) {
            return (intf)divisor % one;
        }
        #endregion

        #region +
        public static intf operator +(intf one, intf other) {
            intf fInt;
            fInt.rawValue = one.rawValue + other.rawValue;
            return fInt;
        }

        public static intf operator +(intf one, int other) {
            return one + (intf)other;
        }

        public static intf operator +(int other, intf one) {
            return one + (intf)other;
        }
        #endregion

        #region -
        public static intf operator -(intf one, intf other) {
            intf fInt;
            fInt.rawValue = one.rawValue - other.rawValue;
            return fInt;
        }

        public static intf operator -(intf one, int other) {
            return one - (intf)other;
        }

        public static intf operator -(int other, intf one) {
            return (intf)other - one;
        }

        public static intf operator -(intf value) {
            return (intf)0 - value;
        }
        #endregion

        #region ==
        public static bool operator ==(intf one, intf other) {
            return one.rawValue == other.rawValue;
        }

        public static bool operator ==(intf one, int other) {
            return one == (intf)other;
        }

        public static bool operator ==(int other, intf one) {
            return (intf)other == one;
        }
        #endregion

        #region !=
        public static bool operator !=(intf one, intf other) {
            return one.rawValue != other.rawValue;
        }

        public static bool operator !=(intf one, int other) {
            return one != (intf)other;
        }

        public static bool operator !=(int other, intf one) {
            return (intf)other != one;
        }
        #endregion

        #region >=
        public static bool operator >=(intf one, intf other) {
            return one.rawValue >= other.rawValue;
        }

        public static bool operator >=(intf one, int other) {
            return one >= (intf)other;
        }

        public static bool operator >=(int other, intf one) {
            return (intf)other >= one;
        }
        #endregion

        #region <=
        public static bool operator <=(intf one, intf other) {
            return one.rawValue <= other.rawValue;
        }

        public static bool operator <=(intf one, int other) {
            return one <= (intf)other;
        }

        public static bool operator <=(int other, intf one) {
            return (intf)other <= one;
        }
        #endregion

        #region >
        public static bool operator >(intf one, intf other) {
            return one.rawValue > other.rawValue;
        }

        public static bool operator >(intf one, int other) {
            return one > (intf)other;
        }

        public static bool operator >(int other, intf one) {
            return (intf)other > one;
        }
        #endregion

        #region <
        public static bool operator <(intf one, intf other) {
            return one.rawValue < other.rawValue;
        }

        public static bool operator <(intf one, int other) {
            return one < (intf)other;
        }

        public static bool operator <(int other, intf one) {
            return (intf)other < one;
        }
        #endregion

        public static explicit operator int(intf src) {
            return (int)(src.rawValue >> SHIFT_AMOUNT);
        }

        public static explicit operator intf(int src) {
            return intf.Create(src, true);
        }

        public static explicit operator intf(long src) {
            return intf.Create(src, true);
        }

        public static explicit operator intf(ulong src) {
            return intf.Create((long)src, true);
        }

        public static intf operator <<(intf one, int Amount) {
            return intf.Create(one.rawValue << Amount, false);
        }

        public static intf operator >>(intf one, int Amount) {
            return intf.Create(one.rawValue >> Amount, false);
        }

        public override bool Equals(object obj) {
            if (obj is intf)
                return ((intf)obj).rawValue == this.rawValue;
            else
                return false;
        }

        public override int GetHashCode() {
            return rawValue.GetHashCode();
        }

        public override string ToString() {
            return this.rawValue.ToString();
        }
    }
}
