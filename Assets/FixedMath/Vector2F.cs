using UnityEngine;
using FixedMath;

namespace FixedMath {
    public struct Vector2F {
        public Fix32 x;
        public Fix32 y;

        private static Vector2F zero = new Vector2F(0, 0);
        private static Vector2F one = new Vector2F(1, 1);
        private static Vector2F down = new Vector2F(0, -1);
        private static Vector2F up = new Vector2F(0, 1);

        public Vector2F(Fix32 x, Fix32 y) {
            this.x = x;
            this.y = y;
        }

        public Vector2F(int x, int y) {
            this.x = (Fix32)x;
            this.y = (Fix32)y;
        }

        public Vector2F(Fix32 value) {
            this.x = value;
            this.y = value;
        }

        public Vector2F(Vector2 v) {
            this.x = (Fix32)v.x;
            this.y = (Fix32)v.y;
        }

        public Vector2F(Vector3 v) {
            this.x = (Fix32)v.x;
            this.y = (Fix32)v.z;
        }

        public static Vector2F Zero {
            get { return zero; }
        }

        public static Vector2F One {
            get { return one; }
        }

        public static Vector2F Down {
            get { return down; }
        }

        public static Vector2F Up {
            get { return up; }
        }

        public Fix32 Magnitude {
            get {
                return Fix32.Sqrt(SqrtMagnitude);
            }
        }

        public Fix32 SqrtMagnitude {
            get {
                return DistanceSquared(this, zero);
            }
        }

        public Vector2F Normalized {
            get {
                this.Normalize();
                return this;
            }
        }

        public static bool operator ==(Vector2F v1, Vector2F v2) {
            return (v1.x == v2.x && v1.y == v2.y);
        }

        public static bool operator !=(Vector2F v1, Vector2F v2) {
            return !(v1 == v2);
        }

        public static Vector2F operator +(Vector2F v1, Vector2F v2) {
            v1.x += v2.x;
            v1.y += v2.y;
            return v1;
        }

        public static Vector2F operator -(Vector2F v1, Vector2F v2) {
            v1.x -= v2.x;
            v1.y -= v2.y;
            return v1;
        }

        public static Vector2F operator -(Vector2F v) {
            return new Vector2F(-v.x, -v.y);
        }

        public static Vector2F operator *(Vector2F v1, Fix32 scale) {
            v1.x *= scale;
            v1.y *= scale;
            return v1;
        }

        public static Vector2F operator *(Vector2F v1, int scale) {
            v1.x *= (Fix32)scale;
            v1.y *= (Fix32)scale;
            return v1;
        }

        public static Vector2F operator *(Vector2F v1, float scale) {
            Fix32 s = (Fix32)scale;
            v1.x *= s;
            v1.y *= s;
            return v1;
        }

        public static Vector2F operator *(Vector2F v1, Vector2F v2) {
            v1.x *= v2.x;
            v1.y *= v2.y;
            return v1;
        }

        public static Vector2F operator /(Vector2F v1, Fix32 scale) {
            v1.x /= scale;
            v1.y /= scale;
            return v1;
        }

        public static Vector2F operator /(Vector2F v1, int scale) {
            v1.x /= (Fix32)scale;
            v1.y /= (Fix32)scale;
            return v1;
        }

        public static Vector2F operator /(Vector2F v1, Vector2F v2) {
            v1.x /= v2.x;
            v1.y /= v2.y;
            return v1;
        }

        public static explicit operator Vector2F(Vector2 v) {
            return new Vector2F(
                (Fix32)v.x,
                (Fix32)v.y
                );
        }

        public static Vector2F Abs(Vector2F v) {
            return new Vector2F(Fix32.Abs(v.x), Fix32.Abs(v.y));
        }

        public static Vector2F Clamp(Vector2F value, Vector2F min, Vector2F max) {
            value.x = Fix32.Clamp(value.x, min.x, max.x);
            value.y = Fix32.Clamp(value.y, min.y, max.y);
            return value;
        }

        public static Fix32 Distance(Vector2F v1, Vector2F v2) {
            return Fix32.Sqrt(DistanceSquared(v1, v2));
        }

        public static Fix32 DistanceSquared(Vector2F v1, Vector2F v2) {
            Fix32 x = v1.x - v2.x;
            Fix32 y = v1.y - v2.y;
            return (x * x + y * y);
        }

        public static Fix32 Dot(Vector2F v1, Vector2F v2) {
            return v1.x * v2.x + v1.y * v2.y;
        }

        public static Vector2F Max(Vector2F v1, Vector2F v2) {
            return new Vector2F(
                Fix32.Max(v1.x, v2.x),
                Fix32.Max(v1.y, v2.y)
                );
        }

        public static Vector2F Min(Vector2F v1, Vector2F v2) {
            return new Vector2F(
                Fix32.Min(v1.x, v2.x),
                Fix32.Min(v1.y, v2.y)
                );
        }

        public void Normalize() {
            Fix32 length = this.Magnitude;
            if (length == Fix32.Zero) {
                this = Vector2F.Zero;
            }
            this.x /= length;
            this.y /= length;
        }

        public Vector2 ToVector2() {
            float a = (float)x;
            float b = (float)y;
            return new Vector2(a, b);
        }

        public Vector3 ToVector3() {
            float a = (float)x;
            float b = (float)y;
            return new Vector3(a, 0, b);
        }

        public override bool Equals(object obj) {
            return (obj is Vector2F) ? (this == (Vector2F)obj) : false;
        }

        public override int GetHashCode() {
            return (x.GetHashCode() + y.GetHashCode());
        }

        public override string ToString() {
            return "(" + x + ", " + y + ")";
        }
    }
}