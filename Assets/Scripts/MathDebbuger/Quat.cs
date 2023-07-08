using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMath
{
    public struct Quat : IEquatable<Quat>
    {
        //Variables
        public float x;
        public float y;
        public float z;
        public float w;

        //Static Properties
        public static readonly Quat identity = new Quat(0.0f, 0.0f, 0.0f, 1f);

        //Constants
        public const float epsilon = 1e-05f;
        
        //Properties
        public Vec3 eulerAngles => Vec3.Zero; //Quat.EulerAngles(this); //NOT IMPLEMENTED
        public Quat normalized => Normalize(this);

        //Constructors
        public Quat(float x, float y, float z, float w)
        {
            //https://docs.unity3d.com/ScriptReference/Quaternion.html
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        #region Operators

        public static Quat operator*(Quat q, Quat q2)
        {
            //complex multiplication ref
            //https://upload.wikimedia.org/wikipedia/commons/9/90/Quaternion-multiplication-table.png
            //https://i.ytimg.com/vi/3Ki14CsP_9k/maxresdefault.jpg
            
            //Calculate Components
            float x = q.w * q2.x + q.x * q2.w + q.y * q2.z - q.z * q2.y;
            float y = q.w * q2.y - q.x * q2.z + q.y * q2.w + q.z * q2.x;
            float z = q.w * q2.z + q.x * q2.y - q.y * q2.x + q.z * q2.w;
            float w = q.w * q2.w - q.x * q2.x - q.y * q2.y - q.z * q2.z;
            
            //Return Quaternion
            return new Quat(x, y, z, w);
        }
        public static Vec3 operator*(Quat q, Vec3 v)
        {
            //https://gamedev.stackexchange.com/questions/28395/rotating-vector3-by-a-quaternion
            //Sincerely, I got the first part, and that the second one is more optimal

            //Remove scalar from quaternion
            Vec3 qUnit = new Vec3(q.x, q.y, q.z);
            
            // Scale vector by quaternion scalar 
            Vec3 scaledVec = q.w * v;
            
            // Get rotation axis
            Vec3 cross1 = Vec3.Cross(qUnit, v); //perpendicular of v (direction) and qUnit (og rotation axis)
            Vec3 cross2 = Vec3.Cross(qUnit, cross1); //perpendicular of cross1 (X direction) and qUnit (og rotation axis)
            Vec3 axis = cross2 * 2.0f; //multiply by 2 to compensate for quaternion scalar 

            // Apply the rotation formula
            Vec3 rotatedVector = v + scaledVec + axis;

            return rotatedVector;
        }
        public static bool operator==(Quat q1, Quat q2)
        {
            throw new NotImplementedException();
        }
        public static bool operator!=(Quat q1, Quat q2)
        {
            throw new NotImplementedException();
        }

        #endregion
        
        #region IEquatable

        public bool Equals(Quat other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return obj is Quat other && Equals(other);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Methods

        public void Set(float newX, float newY, float newZ, float newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }
        public void SetFromToRotation(Vec3 from, Vec3 to)
        {
            this = FromToRotation(from, to);
        }
        public void SetLookRotation(Vec3 view)
        {
            Vec3 up = Vec3.Up; //Can't use static read only as defaults
            SetLookRotation(view, up);
        }
        public void SetLookRotation(Vec3 view, Vec3 up)
        {
            this = LookRotation(view, up);
        }
        public void ToAngleAxis(out float angle, out Vec3 axis)
        {
            //https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToAngle/index.htm

            Quat q = Normalize(this);

            float sqrt = Mathf.Sqrt(1 - q.w * q.w);

            if (sqrt < epsilon)
                axis = new Vec3(q.x, q.y, q.z);
            else
                axis = new Vec3(q.x, q.y, q.z) / sqrt;
            
            axis.Normalize();
            
            angle = 2 * Mathf.Acos(q.w) * Mathf.Rad2Deg;
        }
        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns the angle in degrees between two rotations a and b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Angle(Quat a, Quat b)
        {
            //Similar to Vec3.Angle
            //https://stackoverflow.com/questions/21513637/dot-product-of-two-quaternion-rotations

            //Dot gets the cos(angle) * magA * magB between the two quaternions (value can be a positive or negative number)
            
            //Acos needs a value between -1 and 1 and returns the angle in radians (0 to pi) (or 0 to 180 degrees)
            
            //Abs gets the absolute value of the dot product (so it's always positive)
            //This is possible because quaternions are always positive (w is always positive)
            
            //Min makes sure value is never bigger than 1 (and thanks to abs it's never smaller than 0)
            
            //Multiply by 2 to get the full angle (0 to 2pi) (or 0 to 360 degrees)
            
            //Convert to degrees

            return Mathf.Acos(Mathf.Min(Mathf.Abs(Dot(a, b)), 1)) * 2 * Mathf.Rad2Deg;
        }
        
        /// <summary>
        /// Creates a rotation which rotates 'angle' degrees around axis.
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static Quat AngleAxis(float angle, Vec3 axis)
        {
            //https://www.euclideanspace.com/maths/geometry/rotations/conversions/angleToQuaternion/index.htm
                //(see "Derivation of Equations" for proof)
            
            //Make sure axis is normalized
            axis.Normalize();
            
            //Get sin of half angle in radians
                //angle is divided by 2 because radians go from 0 to pi (or 0 to 180 degrees)
            float sin = Mathf.Sin(Mathf.Deg2Rad * angle / 2);
            
            float x = axis.x * sin;
            float y = axis.y * sin;
            float z = axis.z * sin;
            float w = Mathf.Cos(Mathf.Deg2Rad * angle / 2);
            
            return new Quat(x, y, z, w);
        }

        /// <summary>
        /// Dot returns: magA * magB * cos(angle) OR sum of product of each component
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Dot(Quat a, Quat b)
        {
            //Same as Vec3.Dot
            //https://stackoverflow.com/questions/21513637/dot-product-of-two-quaternion-rotations
            
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public static Quat Euler(Vec3 euler)
        {
            return Euler(euler.x, euler.y, euler.z);
        }
        public static Quat Euler(float _x, float _y, float _z)
        {
            //Calculate Radians
            float radX = Mathf.Deg2Rad * _x / 2;
            float radY = Mathf.Deg2Rad * _y / 2;
            float radZ = Mathf.Deg2Rad * _z / 2;
            
            //Calculate Components
            Quat qX = new Quat(Mathf.Sin(radX), 0, 0, Mathf.Cos(radX));
            Quat qY = new Quat(0, Mathf.Sin(radY), 0, Mathf.Cos(radY));
            Quat qZ = new Quat(0, 0, Mathf.Sin(radZ), Mathf.Cos(radZ));
            
            //Combine components and return Quaternion
            return qY * qX * qZ;
        }

        /// <summary>
        /// Creates a rotation which rotates from fromDirection to toDirection.
        /// </summary>
        /// <param name="fromDirection"></param>
        /// <param name="toDirection"></param>
        /// <returns></returns>
        public static Quat FromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            //Normalize both vectors, just in case
            fromDirection.Normalize();
            toDirection.Normalize();

            //Get angle between two directions
            float angle = Vec3.Angle(fromDirection, toDirection);

            //Get rotation axis
            //(Rotation axis is the perpendicular vector between the two directions)
            Vec3 rotationAxis = Vec3.Cross(fromDirection, toDirection);
            rotationAxis.Normalize();

            return Quat.AngleAxis(angle, rotationAxis);
        }

        /// <summary>
        /// Returns the Inverse of rotation
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Quat Inverse(Quat rotation)
        {
            //Invert all components except w (as w should always be positive)
            return new Quat(-rotation.x, -rotation.y, -rotation.z, rotation.w);
        }

        /// <summary>
        /// Interpolates between a and b by t and normalizes the result afterwards.
        /// The parameter t is clamped to the range [0, 1].
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Quat Lerp(Quat a, Quat b, float t)
        {
            //https://stackoverflow.com/questions/46156903/how-to-lerp-between-two-quaternions
            
            //Clamp t between 0 and 1
            t = Mathf.Clamp01(t);
            
            //Invert second quat if dot product is negative
            float dot = Quat.Dot(a, b);
            if(dot < epsilon) 
            {
                b = Quat.Inverse(b);
            }
            
            Quat quat;
            
            //Linear Interpolation
            // Similar to vec3, but must be done component-wise and be normalized afterwards
            // quat = a + t(b - a)  -->   quat = a - t(a - b)
            // the latter is slightly better on x64
            quat.x = a.x - t*(a.x - b.x);
            quat.y = a.y - t*(a.y - b.y);
            quat.z = a.z - t*(a.z - b.z);
            quat.w = a.w - t*(a.w - b.w);
            
            return Quat.Normalize(quat);
        }

        /// <summary>
        /// Interpolates between a and b by t and normalizes the result afterwards.
        /// The parameter t is not clamped.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Quat LerpUnclamped(Quat a, Quat b, float t)
        {
            //https://stackoverflow.com/questions/46156903/how-to-lerp-between-two-quaternions
            
            //Invert second quat if dot product is negative
            float dot = Quat.Dot(a, b);
            if(dot < epsilon) 
            {
                b = Quat.Inverse(b);
            }
            
            Quat quat;
            
            //Linear Interpolation
            // Similar to vec3, but must be done component-wise and be normalized afterwards
            // quat = a + t(b - a)  -->   quat = a - t(a - b)
            // the latter is slightly better on x64
            quat.x = a.x - t*(a.x - b.x);
            quat.y = a.y - t*(a.y - b.y);
            quat.z = a.z - t*(a.z - b.z);
            quat.w = a.w - t*(a.w - b.w);
            
            return Quat.Normalize(quat);
        }
        
        /// <summary>
        /// Creates a rotation with the specified forward and upwards directions.
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="upwards"></param>
        /// <returns></returns>
        public static Quat LookRotation(Vec3 forward, Vec3 upwards)
        {
            //https://stackoverflow.com/questions/52413464/look-at-quaternion-using-up-vector
            //Third answer, by nilpunch
            
            forward.Normalize();
    
            // First matrix column
            Vec3 sideAxis = Vec3.Cross(upwards, forward).normalized;
            // Second matrix column
            Vec3 rotatedUp = Vec3.Cross(forward, sideAxis);
            // Third matrix column

            // Sums of matrix main diagonal elements
            float trace1 = 1.0f + sideAxis.x - rotatedUp.y - forward.z;
            float trace2 = 1.0f - sideAxis.x + rotatedUp.y - forward.z;
            float trace3 = 1.0f - sideAxis.x - rotatedUp.y + forward.z;

            // If orthonormal vectors forms identity matrix, then return identity rotation
            if (trace1 + trace2 + trace3 < epsilon)
            {
                return identity;
            }

            // Choose largest diagonal
            if (trace1 + epsilon > trace2 && trace1 + epsilon > trace3)
            { 
                float s = Mathf.Sqrt(trace1) * 2.0f;
                return new Quat(
                    0.25f * s,
                    (rotatedUp.x + sideAxis.y) / s,
                    (forward.x + sideAxis.z) / s,
                    (rotatedUp.z - forward.y) / s);
            }
            else if (trace2 + epsilon > trace1 && trace2 + epsilon > trace3)
            { 
                float s = Mathf.Sqrt(trace2) * 2.0f;
                return new Quat(
                    (rotatedUp.x + sideAxis.y) / s,
                    0.25f * s,
                    (forward.y + rotatedUp.z) / s,
                    (forward.x - sideAxis.z) / s);
            }
            else
            { 
                float s = Mathf.Sqrt(trace3) * 2.0f;
                return new Quat(
                    (forward.x + sideAxis.z) / s,
                    (forward.y + rotatedUp.z) / s,
                    0.25f * s,
                    (sideAxis.y - rotatedUp.x) / s);
            }
        } //SOMETIMES SKIPS NEGATIVE, BUT ABS IS GOOD
        public static Quat LookRotation(Vec3 forward)
        {
            Vec3 upwards = Vec3.Up; //Can't use static read only as defaults
            return LookRotation(forward, upwards);
        }

        /// <summary>
        /// Rotates a rotation from towards to, with a maximum of maxDegreesDelta.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="maxDegreesDelta"></param>
        /// <returns></returns>
        public static Quat RotateTowards(Quat from, Quat to, float maxDegreesDelta)
        {
            // Convert maxDegreesDelta to radians
            //float maxRadiansDelta = Mathf.Deg2Rad * maxDegreesDelta;

            // Calculate the angle between the quaternions
            float angle = Quat.Angle(from, to);

            // Check if the angle is within the maximum delta
            if (angle <= maxDegreesDelta)
            {
                // The quaternions are already within the desired range
                return to;
            }

            // Interpolate the rotation using Slerp
            float t = maxDegreesDelta / angle;
            return Quat.Slerp(from, to, t);
        }

        /// <summary>
        /// Spherically interpolates between quaternions a and b by ratio t.
        /// The parameter t is clamped to the range [0, 1].
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Quat Slerp(Quat a, Quat b, float t)
        {
            //https://www.euclideanspace.com/maths/algebra/realNormedAlgebra/quaternions/slerp/index.htm
            
            if (t < epsilon) return a;
            if(t > 1) return b;

            // Calculate angle between them.
            float abDot = a.w * b.w + a.x * b.x + a.y * b.y + a.z * b.z;
            
            // Adjust the sign of b if necessary to take the shortest path
            if (abDot < epsilon)
            {
                b = Quat.Inverse(b);
                abDot = -abDot;
            }
            
            // Check if the quaternions are close to each other
            if (abDot > 0.9995f)
            {
                // Linearly interpolate if the quaternions are very close
                return Lerp(a, b, t);
            }
            
            // quaternion to return
            Quat qm = new Quat();
            
            // Calculate temporary values.
            float halfTheta = Mathf.Acos(abDot);
            float sinHalfTheta = Mathf.Sqrt(1.0f - abDot*abDot);
            
            // if theta = 180 degrees then result is not fully defined
            // we could rotate around any axis normal to qa or qb
            if (Mathf.Abs(sinHalfTheta) < epsilon)
            {
                qm.w = (a.w * 0.5f + b.w * 0.5f);
                qm.x = (a.x * 0.5f + b.x * 0.5f);
                qm.y = (a.y * 0.5f + b.y * 0.5f);
                qm.z = (a.z * 0.5f + b.z * 0.5f);
                return qm;
            }
            
            float ratioA = Mathf.Sin((1 - t) * halfTheta) / sinHalfTheta;
            float ratioB = Mathf.Sin(t * halfTheta) / sinHalfTheta; 
            
            //calculate Quaternion.
            qm.w = (a.w * ratioA + b.w * ratioB);
            qm.x = (a.x * ratioA + b.x * ratioB);
            qm.y = (a.y * ratioA + b.y * ratioB);
            qm.z = (a.z * ratioA + b.z * ratioB);
            
            return qm;
        }

        /// <summary>
        /// Spherically interpolates between a and b by t.
        /// The parameter t is not clamped.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Quat SlerpUnclamped(Quat a, Quat b, float t)
        {
            //https://www.euclideanspace.com/maths/algebra/realNormedAlgebra/quaternions/slerp/index.htm
            
            // Calculate angle between them.
            float abDot = a.w * b.w + a.x * b.x + a.y * b.y + a.z * b.z;
            
            // Adjust the sign of b if necessary to take the shortest path
            if (abDot < epsilon)
            {
                b = Quat.Inverse(b);
                abDot = -abDot;
            }
            
            // Check if the quaternions are close to each other
            if (abDot > 0.9995f)
            {
                // Linearly interpolate if the quaternions are very close
                return LerpUnclamped(a, b, t);
            }
            
            // quaternion to return
            Quat qm = new Quat();
            
            // Calculate temporary values.
            float halfTheta = Mathf.Acos(abDot);
            float sinHalfTheta = Mathf.Sqrt(1.0f - abDot*abDot);
            
            // if theta = 180 degrees then result is not fully defined
            // we could rotate around any axis normal to qa or qb
            if (Mathf.Abs(sinHalfTheta) < epsilon)
            {
                qm.w = (a.w * 0.5f + b.w * 0.5f);
                qm.x = (a.x * 0.5f + b.x * 0.5f);
                qm.y = (a.y * 0.5f + b.y * 0.5f);
                qm.z = (a.z * 0.5f + b.z * 0.5f);
                return qm;
            }
            
            float ratioA = Mathf.Sin((1 - t) * halfTheta) / sinHalfTheta;
            float ratioB = Mathf.Sin(t * halfTheta) / sinHalfTheta; 
            
            //calculate Quaternion.
            qm.w = (a.w * ratioA + b.w * ratioB);
            qm.x = (a.x * ratioA + b.x * ratioB);
            qm.y = (a.y * ratioA + b.y * ratioB);
            qm.z = (a.z * ratioA + b.z * ratioB);
            
            return qm;
        }
        
        /// <summary>
        /// This is not public, as it is not in Unity default
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        static float GetMagnitude(Quat q)
        {
            //https://www.cprogramming.com/tutorial/3d/quaternions.html
            //Calculate Magnitude
            return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
        }
        
        /// <summary>
        /// This is not public, as it is not in Unity default
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        static Quat Normalize(Quat q)
        {
            //https://www.cprogramming.com/tutorial/3d/quaternions.html
            //Calculate Magnitude
            float magnitude = GetMagnitude(q);
            
            //Normalize
            q.x /= magnitude;
            q.y /= magnitude;
            q.z /= magnitude;
            q.w /= magnitude;
            
            //Return Quaternion
            return q;
        }

        /// <summary>
        /// This is not public, as it is not in Unity default
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        static Quat Pow(Quat q, float p)
        {
            Vec3 axis;
            float angle;
            
            q.ToAngleAxis(out angle, out axis);
            
            angle *= p;

            angle /= 2;
            
            //angle *= Mathf.Deg2Rad;
            
            return Quat.AngleAxis(Mathf.Cos(angle), Mathf.Sin(angle) * axis);
        }

        /// <summary>
        /// This is not public, as it is not in Unity default
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        static Vec3 EulerAngles(Quat q)
        {
            // static float NormalizeAngle(float angle)
            // {
            //     while (angle < 0)
            //         angle += 360f;
            //
            //     while (angle >= 360f)
            //         angle -= 360f;
            //
            //     return angle;
            // }
            //
            // // Convert quaternion to euler angles
            // double pitch = Math.Atan2(2 * (q.y * q.z + q.w * q.x), q.w * q.w - q.x * q.x - q.y * q.y + q.z * q.z);
            // double yaw = Math.Asin(-2 * (q.x * q.z - q.w * q.y));
            // double roll = Math.Atan2(2 * (q.x * q.y + q.w * q.z), q.w * q.w + q.x * q.x - q.y * q.y - q.z * q.z);
            //
            // // Convert radians to degrees
            // float pitchDeg = (float)((180.0 / Math.PI) * pitch);
            // float yawDeg = (float)((180.0 / Math.PI) * yaw);
            // float rollDeg = (float)((180.0 / Math.PI) * roll);
            //
            // pitchDeg = NormalizeAngle(pitchDeg);
            // yawDeg = NormalizeAngle(yawDeg);
            // rollDeg = NormalizeAngle(rollDeg);
            //
            // return new Vector3(pitchDeg, yawDeg, rollDeg);
            
            // //https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/index.htm
            
            Vec3 euler = new Vec3();
            
            float sqw = q.w * q.w;
            float sqx = q.x * q.x;
            float sqy = q.y * q.y;
            float sqz = q.z * q.z;
            float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            
            float test = q.x * q.y + q.z * q.w;
            
            //
            
            euler.z = Mathf.Atan2(2 * (q.x * q.y + q.z * q.w), sqx - sqy - sqz + sqw);
            euler.y = Mathf.Asin(2 * test / unit);
            euler.x = Mathf.Atan2(2 * q.x * q.w - 2 * q.y * q.z, -sqx + sqy - sqz + sqw);
            
            euler *= Mathf.Rad2Deg;
            
            if(euler.x < 0)
                euler.x += 360;
            if (euler.y < 0)
                euler.y += 360;
            if (euler.z < 0)
                euler.z += 360;
            
            return euler; //Works with only one component, breaks with more than one OR with values < 0 | >= 1

            
            // // singularity at north pole
            // if (test > 0.499f * unit)
            // {
            //     euler.z = 2 * Mathf.Atan2(q.x, q.w);
            //     euler.y = Mathf.PI / 2;
            //     euler.x = 0;
            //     return euler * Mathf.Rad2Deg;
            // }
            //
            // // singularity at south pole
            // if (test < -0.499f * unit)
            // {
            //     euler.z = -2 * Mathf.Atan2(q.x, q.w);
            //     euler.y = -Mathf.PI / 2;
            //     euler.x = 0;
            //     return euler * Mathf.Rad2Deg;
            // }

            bool space;


            // float test = q.x*q.y + q.z*q.w;
            // if (test > 0.499) 
            // { // singularity at north pole
            //     euler.z = 2 * Mathf.Atan2(q.x,q.w);
            //     euler.y = Mathf.PI/2;
            //     euler.x = 0;
            //     return euler;
            // }
            // if (test < -0.499) 
            // { // singularity at south pole
            //     euler.z = -2 * Mathf.Atan2(q.x,q.w);
            //     euler.y = - Mathf.PI/2;
            //     euler.x = 0;
            //     return euler;
            // }
            // float sqx = q.x*q.x;
            // float sqy = q.y*q.y;
            // float sqz = q.z*q.z;
            // euler.z = Mathf.Atan2(2*q.y*q.w-2*q.x*q.z , 1 - 2*sqy - 2*sqz);
            // euler.y = Mathf.Asin(2*test);
            // euler.x = Mathf.Atan2(2 * q.x * q.w - 2 * q.y * q.z, 1 - 2 * sqx - 2 * sqz);
            // return euler;
        }

        #endregion
    }
}