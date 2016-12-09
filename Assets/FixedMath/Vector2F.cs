using UnityEngine;
using FixedMath;

namespace FixedMath {

    /// <summary>
    /// Struct representing a point in 2D space, using fixed point math.
    /// </summary>
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
                Vector2F v = new Vector2F(this.x, this.y);
                v.Normalize();
                return v;
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

        /// <summary>
        /// Returns a vector containing the absolute value of its components.
        /// </summary>
        public static Vector2F Abs(Vector2F v) {
            return new Vector2F(Fix32.Abs(v.x), Fix32.Abs(v.y));
        }

        /// <summary>
        /// Clamps the x and y components of the given vector inside the min and max components.
        /// </summary>
        /// <param name="value">the vector to clamp</param>
        /// <param name="min">min x and y</param>
        /// <param name="max">max x and y</param>
        /// <returns>clamped vector</returns>
        public static Vector2F Clamp(Vector2F value, Vector2F min, Vector2F max) {
            value.x = Fix32.Clamp(value.x, min.x, max.x);
            value.y = Fix32.Clamp(value.y, min.y, max.y);
            return value;
        }

        /// <summary>
        /// Returns the euclidean distance between two coordinates.
        /// Note: Heavier than DistanceSquared, because of the Sqrt call.
        /// </summary>
        /// <param name="v1">first position</param>
        /// <param name="v2">second position</param>
        /// <returns>distance between the points</returns>
        public static Fix32 Distance(Vector2F v1, Vector2F v2) {
            return Fix32.Sqrt(DistanceSquared(v1, v2));
        }

        /// <summary>
        /// Returns the squared distance between the points.
        /// Use this function when the actual distance value is not important.
        /// </summary>
        /// <param name="v1">first position</param>
        /// <param name="v2">second position</param>
        /// <returns>squared distance between the two</returns>
        public static Fix32 DistanceSquared(Vector2F v1, Vector2F v2) {
            Fix32 x = v1.x - v2.x;
            Fix32 y = v1.y - v2.y;
            return (x * x + y * y);
        }

        /// <summary>
        /// Calculates the dot product between the two vectors.
        /// </summary>
        /// <param name="v1">first vector</param>
        /// <param name="v2">second vector</param>
        /// <returns>value indicating the dot product.</returns>
        public static Fix32 Dot(Vector2F v1, Vector2F v2) {
            return v1.x * v2.x + v1.y * v2.y;
        }

        /// <summary>
        /// Creates a new vector using the maximum value components of the given vectors.
        /// </summary>
        /// <param name="v1">first vector</param>
        /// <param name="v2">second vector</param>
        /// <returns>vector containing max x and y from the other two</returns>
        public static Vector2F Max(Vector2F v1, Vector2F v2) {
            return new Vector2F(
                Fix32.Max(v1.x, v2.x),
                Fix32.Max(v1.y, v2.y)
                );
        }

        /// <summary>
        /// Creates a new vector using the minimum value components of the given vectors.
        /// </summary>
        /// <param name="v1">first vector</param>
        /// <param name="v2">second vector</param>
        /// <returns>vector containing min x and y from the other two</returns>
        public static Vector2F Min(Vector2F v1, Vector2F v2) {
            return new Vector2F(
                Fix32.Min(v1.x, v2.x),
                Fix32.Min(v1.y, v2.y)
                );
        }

        /// <summary>
        /// Normalizes this vector, using the inverse square root for faster calculations.
        /// </summary>
        public void Normalize() {
            Fix32 sqrMag = this.SqrtMagnitude;
            Fix32 invMag = (sqrMag > Fix32.Zero) ? Fix32.InvSqrt(sqrMag) : Fix32.Zero;
            this.x *= invMag;
            this.y *= invMag;
        }

        /// <summary>
        /// Returns the Vector2 (float) equivalent of this vector.
        /// </summary>
        /// <returns>Vector2 converted</returns>
        public Vector2 ToVector2() {
            float a = (float)x;
            float b = (float)y;
            return new Vector2(a, b);
        }

        /// <summary>
        /// Returns the Vector3 (float) equivalent of this vector, using x and z as axis.
        /// </summary>
        /// <returns>A Vector3</returns>
        public Vector3 ToVector3() {
            float a = (float)x;
            float b = (float)y;
            return new Vector3(a, 0, b);
        }

        /// <summary>
        /// Checks whether the given object is a vector2F and if it has the same component values
        /// of this one.
        /// </summary>
        /// <param name="obj">the given object</param>
        /// <returns>true if it's a vector with the same component values, false otherwise</returns>
        public override bool Equals(object obj) {
            return (obj is Vector2F) ? (this == (Vector2F)obj) : false;
        }

        /// <summary>
        /// Gets the hashcode of this vector.
        /// </summary>
        /// <returns>the sum of the x and y hashcodes.</returns>
        public override int GetHashCode() {
            return (x.GetHashCode() + y.GetHashCode());
        }

        /// <summary>
        /// Returns a string containing the x and y components of this vector.
        /// </summary>
        /// <returns>string of the vector</returns>
        public override string ToString() {
            return "(" + x + ", " + y + ")";
        }
    }
}