using UnityEngine;
using System;

namespace CustomMath
{
    [System.Serializable]
    public struct Vec3 : IEquatable<Vec3>
    {
        #region Variables
        public float x;
        public float y;
        public float z;

        //get {} is calculated every time the values change, instead of lambda. Think later best/optimal option
        //ref: https://www.tabsoverspaces.com/233844-back-to-csharp-basics-difference-between-and-get-for-properties
        public float sqrMagnitude { get { return x * x + y * y + z * z; } }
        public Vec3 normalized { get { return this / magnitude; } } 
        public float magnitude { get { return Mathf.Sqrt(sqrMagnitude); } } 

        #endregion

        #region constants
        public const float epsilon = 1e-05f;
        #endregion

        #region Default Values
        public static Vec3 Zero { get { return new Vec3(0.0f, 0.0f, 0.0f); } }
        public static Vec3 One { get { return new Vec3(1.0f, 1.0f, 1.0f); } }
        public static Vec3 Forward { get { return new Vec3(0.0f, 0.0f, 1.0f); } }
        public static Vec3 Back { get { return new Vec3(0.0f, 0.0f, -1.0f); } }
        public static Vec3 Right { get { return new Vec3(1.0f, 0.0f, 0.0f); } }
        public static Vec3 Left { get { return new Vec3(-1.0f, 0.0f, 0.0f); } }
        public static Vec3 Up { get { return new Vec3(0.0f, 1.0f, 0.0f); } }
        public static Vec3 Down { get { return new Vec3(0.0f, -1.0f, 0.0f); } }
        public static Vec3 PositiveInfinity { get { return new Vec3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity); } }
        public static Vec3 NegativeInfinity { get { return new Vec3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity); } }
        #endregion                                                                                                                                                                               

        #region Constructors
        public Vec3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0.0f;
        }

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(Vec3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector2 v2)
        {
            this.x = v2.x;
            this.y = v2.y;
            this.z = 0.0f;
        }
        #endregion

        #region Operators
        public static bool operator ==(Vec3 left, Vec3 right)
        {
            float diff_x = left.x - right.x;
            float diff_y = left.y - right.y;
            float diff_z = left.z - right.z;
            float sqrmag = diff_x * diff_x + diff_y * diff_y + diff_z * diff_z;
            return sqrmag < epsilon * epsilon;
        }
        public static bool operator !=(Vec3 left, Vec3 right)
        {
            return !(left == right);
        }

        public static Vec3 operator +(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x + rightV3.x, leftV3.y + rightV3.y, leftV3.z + rightV3.z);
        }

        public static Vec3 operator -(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x - rightV3.x, leftV3.y - rightV3.y, leftV3.z - rightV3.z);
        }

        public static Vec3 operator -(Vec3 v3)
        {
            return new Vec3(-v3.x, -v3.y, -v3.z);
        }

        public static Vec3 operator *(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x * scalar, v3.y  * scalar, v3.z * scalar);
        }
        public static Vec3 operator *(float scalar, Vec3 v3)
        {
            //sames as (Vec3 v3, float scalar)
            return new Vec3(v3.x * scalar, v3.y  * scalar, v3.z * scalar);
        }
        public static Vec3 operator /(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x / scalar, v3.y  / scalar, v3.z / scalar);
        }

        public static implicit operator Vector3(Vec3 v3)
        {
            return new Vector3(v3.x, v3.y, v3.z);
        }

        public static implicit operator Vector2(Vec3 v2)
        {
            return new Vector2(v2.x, v2.y);
        }
        #endregion

        #region Functions
        public override string ToString()
        {
            return "X = " + x.ToString() + "   Y = " + y.ToString() + "   Z = " + z.ToString();
        }
        public static float Angle(Vec3 from, Vec3 to)
        {
            //https://www.cuemath.com/geometry/angle-between-vectors/
            //Using dot formula because has 3 less multiplications, so it's more efficient
            //The formula is in radians, so it must be converted to degrees (credits to ChatGPT 3.5) 
            return Mathf.Acos(Vec3.Dot(from, to) / (from.magnitude * to.magnitude)) * Mathf.Rad2Deg;
        }
        public static Vec3 ClampMagnitude(Vec3 vector, float maxLength)
        {
            //get {} property is calculated every time the vector changes,
            //so using magnitude multiple times doesn't bring performance issues
            return vector.magnitude > maxLength ? vector.normalized * maxLength : vector;
        }
        /// <summary>
        /// Returns the length of the vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float Magnitude(Vec3 vector)
        {
            //https://www.cuemath.com/magnitude-of-a-vector-formula/ (the symbol for mag is |x|)
            return Mathf.Sqrt(vector.magnitude);
        }
        /// <summary>
        /// Cross returns the perpendicular vector to the 2 input vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            //https://www.cuemath.com/geometry/cross-product/
            return new Vec3(a.y * b.z - a.z * b.y, -(a.x * b.z - a.z * b.x), a.x * b.y - a.y * b.x);
        }
        public static float Distance(Vec3 a, Vec3 b)
        {
            //https://www.cuemath.com/geometry/distance-between-two-points/
            //Formula into code:
            //Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2));
            //As this is basically substracting each component and calculating an individual magnitude...
            //...we can just substract each vector and calculate magnitude
            //(order is not important because magnitude is absolute)
            return (a - b).magnitude;
        }
        /// <summary>
        /// Dot returns the sum of the product of the components of the 2 vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Dot(Vec3 a, Vec3 b)
        {
            //https://www.cuemath.com/algebra/scalar-product/
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }
        /// <summary>
        /// Linearly interpolates the two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vec3 Lerp(Vec3 a, Vec3 b, float t)
        {
            //Both a & b are scaled to get a whole of 1 using t
            //(if t is .5, it will be 50% a and 50% b, but t .25 it will be 75% a and 25% b)
            //(this happens because:
            //1-.25 = .75, so it scales a by .75
            //then scales b by .25
            //and when it adds them together you get .75 from a & .25 from b
            //formula: https://www.mathworks.com/matlabcentral/answers/259773-linear-interpolation-in-3d-space
            t = Mathf.Clamp01(t);
            return (1.0f-t) * a + t * b;
        }
        public static Vec3 LerpUnclamped(Vec3 a, Vec3 b, float t)
        {
            //Same as lerp, but without clamping T
            return (1.0f-t) * a + t * b;
        }
        /// <summary>
        /// Returns a combination of the biggest components of the 2 vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vec3 Max(Vec3 a, Vec3 b)
        {
            float x = a.x > b.x ? a.x : b.x;
            float y = a.y > b.y ? a.y : b.y;
            float z = a.z > b.z ? a.z : b.z;
            return new Vec3(x, y, z);
        }
        /// <summary>
        /// Returns a combination of the smallest components of the 2 vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vec3 Min(Vec3 a, Vec3 b)
        {
            float x = a.x < b.x ? a.x : b.x;
            float y = a.y < b.y ? a.y : b.y;
            float z = a.z < b.z ? a.z : b.z;
            return new Vec3(x, y, z);
        }
        public static float SqrMagnitude(Vec3 vector)
        {
            return vector.sqrMagnitude;
        }
        /// <summary>
        /// Projects a vector onto another vector (expands normal vector until it reaches vector "height")
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="onNormal"></param>
        /// <returns></returns>
        public static Vec3 Project(Vec3 vector, Vec3 onNormal) 
        {
            //https://www.cuemath.com/geometry/projection-vector/
            //Projection gives a scalar, multiply that scalar by the normal to get the projected vector 
            return (Dot(vector, onNormal) / onNormal.magnitude) * onNormal.normalized;
        }
        /// <summary>
        /// Reflects the vector off a plane defined by the normal.
        /// </summary>
        /// <param name="inDirection"></param>
        /// <param name="inNormal"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Vec3 Reflect(Vec3 inDirection, Vec3 inNormal) 
        {
            //http://www.sunshine2k.de/articles/coding/vectorreflection/vectorreflection.html
            return inDirection - 2 * Dot(inDirection, inNormal) * inNormal;
        }
        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }
        public void Scale(Vec3 scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }
        public void Normalize()
        {
            this = normalized;
        }
        #endregion

        #region Internals
        public override bool Equals(object other)
        {
            if (!(other is Vec3)) return false;
            return Equals((Vec3)other);
        }

        public bool Equals(Vec3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }
        #endregion
    }
}