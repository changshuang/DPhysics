using UnityEngine;
using FixedPointMath;

namespace FixedPointMath {
    public struct Vector2f {
        public intf x;
        public intf y;

        private static Vector2f zero = new Vector2f((intf)0, (intf)0);
        private static Vector2f one = new Vector2f((intf)1, (intf)1);

        public Vector2f(intf x, intf y) {
            this.x = x;
            this.y = y;
        }

        public Vector2f(int x, int y) {
            this.x = (intf)x;
            this.y = (intf)y;
        }

        public Vector2f(intf value) {
            this.x = value;
            this.y = value;
        }

        public Vector2f(Vector2 v) {
            this.x = intf.Create(v.x);
            this.y = intf.Create(v.y);
        }

        public Vector2f(Vector3 v) {
            this.x = intf.Create(v.x);
            this.y = intf.Create(v.z);
        }

        public static Vector2f Zero {
            get { return zero; }
        }

        public static Vector2f One {
            get { return one; }
        }

        public intf Magnitude {
            get {
                return FixedMath.Sqrt(SqrtMagnitude);
            }
        }

        public intf SqrtMagnitude {
            get {
                return DistanceSquared(this, zero);
            }
        }

        public Vector2f Normalized {
            get {
                this.Normalize();
                return this;
            }
        }

        public static bool operator ==(Vector2f v1, Vector2f v2) {
            return (v1.x == v2.x && v1.y == v2.y);
        }

        public static bool operator !=(Vector2f v1, Vector2f v2) {
            return !(v1 == v2);
        }

        public static Vector2f operator +(Vector2f v1, Vector2f v2) {
            v1.x += v2.x;
            v1.y += v2.y;
            return v1;
        }

        public static Vector2f operator -(Vector2f v1, Vector2f v2) {
            v1.x -= v2.x;
            v1.y -= v2.y;
            return v1;
        }

        public static Vector2f operator -(Vector2f v) {
            return new Vector2f(-v.x, -v.y);
        }

        public static Vector2f operator *(Vector2f v1, intf scale) {
            v1.x *= scale;
            v1.y *= scale;
            return v1;
        }

        public static Vector2f operator *(Vector2f v1, int scale) {
            v1.x *= scale;
            v1.y *= scale;
            return v1;
        }

        public static Vector2f operator *(Vector2f v1, float scale) {
            intf s = intf.Create(scale);
            v1.x *= s;
            v1.y *= s;
            return v1;
        }

        public static Vector2f operator *(Vector2f v1, Vector2f v2) {
            v1.x *= v2.x;
            v1.y *= v2.y;
            return v1;
        }

        public static Vector2f operator /(Vector2f v1, intf scale) {
            v1.x /= scale;
            v1.y /= scale;
            return v1;
        }

        public static Vector2f operator /(Vector2f v1, int scale) {
            v1.x /= scale;
            v1.y /= scale;
            return v1;
        }

        public static Vector2f operator /(Vector2f v1, Vector2f v2) {
            v1.x /= v2.x;
            v1.y /= v2.y;
            return v1;
        }

        public static explicit operator Vector2f(Vector2 v) {
            return new Vector2f(
                intf.Create(v.x),
                intf.Create(v.y)
                );
        }

        public static Vector2f Abs(Vector2f v) {
            return new Vector2f(FixedMath.Abs(v.x), FixedMath.Abs(v.y));
        }

        public static Vector2f Clamp(Vector2f value, Vector2f min, Vector2f max) {
            value.x = FixedMath.Clamp(value.x, min.x, max.x);
            value.y = FixedMath.Clamp(value.y, min.y, max.y);
            return value;
        }

        public static intf Distance(Vector2f v1, Vector2f v2) {
            return FixedMath.Sqrt(DistanceSquared(v1, v2));
        }

        public static intf DistanceSquared(Vector2f v1, Vector2f v2) {
            intf x = v1.x - v2.x;
            intf y = v1.y - v2.y;
            return (x * x + y * y);
        }

        public static intf Dot(Vector2f v1, Vector2f v2) {
            return v1.x * v2.x + v1.y * v2.y;
        }
        
        public static Vector2f Max(Vector2f v1, Vector2f v2) {
            return new Vector2f(
                FixedMath.Max(v1.x, v2.x),
                FixedMath.Max(v1.y, v2.y)
                );
        }

        public static Vector2f Min(Vector2f v1, Vector2f v2) {
            return new Vector2f(
                FixedMath.Min(v1.x, v2.x),
                FixedMath.Min(v1.y, v2.y)
                );
        }

        public void Normalize() {
            intf length = this.Magnitude;
            if (length == 0) {
                this = Vector2f.Zero;
            }
            this.x /= length;
            this.y /= length;
        }

        public Vector2 ToVector2() {
            float a = x.ToFloat();
            float b = y.ToFloat();
            return new Vector2(a, b);
        }

        public Vector3 ToVector3() {
            float a = x.ToFloat();
            float b = y.ToFloat();
            return new Vector3(a, 0, b);
        }

        public override bool Equals(object obj) {
            return (obj is Vector2f) ? (this == (Vector2f)obj) : false;
        }

        public override int GetHashCode() {
            return (x.GetHashCode() + y.GetHashCode());
        }

        public override string ToString() {
            return "(" + x + ", " + y + ")";
        }
    }
}
