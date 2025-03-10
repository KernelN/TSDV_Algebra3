using System;
using UnityEngine;

namespace CustomMath
{
    [Serializable]
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
        public Vec3 eulerAngles => Quat.EulerAngles(this);
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
            
            //como cada complejo representa un eje, * entre sí da producto cruz
            
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
            // /2 because W must be divided among all axis | imaginary numbers 
            // universally it would be "/ number of dimensions above 1"
            float radX = Mathf.Deg2Rad * _x / 2;  
            float radY = Mathf.Deg2Rad * _y / 2;
            float radZ = Mathf.Deg2Rad * _z / 2;
            
            //https://www.mathsisfun.com/algebra/trig-sin-cos-tan-graphs.html
            //https://www.mathsisfun.com/algebra/images/cosine-graph.svg
            //as higher is each angle, lower will be w
            
            //https://www.mathsisfun.com/algebra/trig-sin-cos-tan-graphs.html
            //https://www.mathsisfun.com/algebra/images/sine-graph.svg
            //highest point of sine is 1, so highest point of x y z will be at 90 degrees (pi/2 radians)
            
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
            //based on
            //http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/
            
            forward.Normalize();
    
            // First matrix column
            Vec3 sideAxis = Vec3.Cross(upwards, forward).normalized;
            // Second matrix column
            Vec3 rotatedUp = Vec3.Cross(forward, sideAxis);
            // Third matrix column: forward

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
            if (trace2 + epsilon > trace1 && trace2 + epsilon > trace3)
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
        } //TODO: USING UP VECTOR AS (000) OR (100) NOT WORKING
        public static Quat LookRotation(Vec3 forward)
        {
            Vec3 upwards = Vec3.Up; //Can't use static read only as defaults
            return LookRotation(forward, upwards);
        } //Work, as uses Vec3.Up

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
            //UNITY Multiplication order is:
            //(qY * qX) * qZ | X right, Y up, Z forward
            //Heading = y axis, Attitude = x axis, Bank z axis
            //(Heading*Attitude)*Bank
            
            //EUCLIDEAN SPACE 
            //Heading = y axis, Attitude = z axis, Bank x axis
            //(Heading*Attitude)*Bank

            #region MyFrankenstein
            static double NormalizeRadians(double rad)
            {
                while(rad < 0)
                    rad += 2 * Math.PI;
                while (rad > 2 * Math.PI)
                    rad -= 2 * Math.PI;
                return rad;
            }
            
            // //https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/index.htm
            //converts a quaternion in 6 triangles
            
            Vec3 euler = new Vec3();
            
            double sqw = q.w * q.w;
            double sqx = q.x * q.x;
            double sqy = q.y * q.y;
            double sqz = q.z * q.z;
            //double unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            
            //double test = q.x * q.y + q.z * q.w; //euclideanspace
            //double test = q.x * q.z - q.y * q.w; //me (this is the order unity uses, probably)
            
            // singularity at north pole
            //if (test > 0.499 * unit) //ES
            // if (test > 0.5 * unit) //me
            // {
            //     euler.z = 2 * Mathf.Atan2(q.x, q.w);
            //     euler.y = Mathf.PI / 2;
            //     euler.x = 0;
            //     return euler * Mathf.Rad2Deg;
            // }
            
            // singularity at south pole
            //if (test < -0.499 * unit) //ES
            // if (test < -0.5 * unit) //me
            // {
            //     euler.z = -2 * Mathf.Atan2(q.x, q.w);
            //     euler.y = -Mathf.PI / 2;
            //     euler.x = 0;
            //     return euler * Mathf.Rad2Deg;
            // }
            
            //ATAN2 Calcula el angulo dependiendo de en que cuadrante esta el vector
            //Chequea por x e y, en vez de un solo numero
            
            //double z = Math.Atan2(2 * (q.x * q.y + q.z * q.w), sqx - sqy - sqz + sqw); //ES
            //double z = Math.Atan2(2 * q.z * q.w + 2 * q.x * q.y, -sqx + sqy - sqz + sqw); //me
            double z = Math.Atan2(2 * q.z * q.w + 2 * q.x * q.y, -sqx + sqy - sqz + sqw); //me 2
            //double z = Math.Atan2(2 * q.x * q.w - 2 * q.y * q.z, 1 - 2 * sqx - 2 * sqz); //ES 2
            
            //double y = Math.Asin(2 * test / unit); //euclideanspace
            //double y = Math.Atan2(2 * (q.y * q.w - q.x * q.z), 1 - 2 * sqy - 2 * sqz); //ES2
            double y = Math.Atan2(2 * (q.y * q.w + q.x * q.z), -sqx - sqy + sqz + sqw); //me
            //double y = Math.Atan2(2 * (q.y * q.w + q.x * q.z), 1 - 2 * (q.z * q.z + q.z * q.z)); //gpt
            //double y = Math.Asin(-2 * (q.x * q.z - q.w * q.z)); //gpt
            
            //double x = Math.Atan2(2 * q.x * q.w - 2 * q.y * q.z, -sqx + sqy - sqz + sqw); //ES
            //double x = Math.Asin(2 * q.x * q.y + 2 * q.z * q.w); //ES2
            //double x = Math.Atan2(2 * q.x * q.w - 2 * q.y * q.z, -sqx + sqy - sqz + sqw); //me
            double x = Math.Atan2(2 * q.x * q.w - 2 * q.y * q.z, -sqx + sqy - sqz + sqw); //me
            
            float zDeg = Mathf.Rad2Deg * (float)z;
            float yDeg = Mathf.Rad2Deg * (float)y;
            float xDeg = Mathf.Rad2Deg * (float)x;
            
            euler.z = zDeg; //rotation is inverted while manipulating w
            euler.y = yDeg;
            euler.x = xDeg; //rotation is inverted while manipulating w
            
            return euler; //breaks with more than one component
            #endregion
            
            //https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/index.htm
            #region EuclideanSpaceNonNormalized
            // Vec3 euler;
            //
            // double sqw = q.w*q.w;
            // double sqx = q.x*q.x;
            // double sqy = q.y*q.y;
            // double sqz = q.z*q.z;
            //
            // // if q is normalised, unit = 1, otherwise unit = correction factor
            // //double unit = sqx + sqy + sqz + sqw;
            // double unit = sqz + sqy + sqx + sqw;
            // //double test = q.x*q.y + q.z*q.w;
            // double gimbalTest = q.z*q.y + q.x*q.w;
            //
            // //Avoids gimbal lock
            // if (gimbalTest > 0.5*unit) { // singularity at north pole
            //     // heading = 2 * atan2(q.x,q.w);
            //     // attitude = Math.PI/2;
            //     // bank = 0;
            //     euler.y = 2 * (float)Math.Atan2(q.z,q.w); //Atan2 is less precise
            //     euler.x = Mathf.PI/2;
            //     euler.z = 0;
            //     return euler * Mathf.Rad2Deg;
            // }
            // if (gimbalTest < -0.5*unit) { // singularity at south pole
            //     // heading = -2 * atan2(q.x,q.w);
            //     // attitude = -Math.PI/2;
            //     // bank = 0;
            //     euler.y = -2 * (float)Math.Atan2(q.z,q.w);
            //     euler.x = -Mathf.PI/2;
            //     euler.z = 0;
            //     return euler * Mathf.Rad2Deg;
            // }
            // // heading = atan2(2*q.y*q.w-2*q.x*q.z , sqx - sqy - sqz + sqw);
            // // attitude = asin(2*test/unit);
            // // bank = atan2(2*q.x*q.w-2*q.y*q.z , -sqx + sqy - sqz + sqw);
            // euler.y = (float)Math.Atan2(2*q.y*q.w-2*q.z*q.x , sqz - sqy - sqx + sqw);
            // euler.x = (float)Math.Asin(2*gimbalTest/unit);
            // euler.z = (float)Math.Atan2(2*q.z*q.w-2*q.y*q.x , -sqz + sqy - sqx + sqw);
            //
            // return euler * Mathf.Rad2Deg;
            #endregion

            //https://stackoverflow.com/questions/12088610/conversion-between-euler-quaternion-like-in-unity3d-engine
            #region StackOverflow

            // static float NormalizeAngle (float angle)
            // {
            //     while (angle>360)
            //         angle -= 360;
            //     while (angle<0)
            //         angle += 360;
            //     return angle;
            // }
            //
            // static Vector3 NormalizeAngles (Vector3 angles)
            // {
            //     angles.x = NormalizeAngle (angles.x);
            //     angles.y = NormalizeAngle (angles.y);
            //     angles.z = NormalizeAngle (angles.z);
            //     return angles;
            // }
            //
            // float sqw = q1.w * q1.w;
            // float sqx = q1.x * q1.x;
            // float sqy = q1.y * q1.y;
            // float sqz = q1.z * q1.z;
            // float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            // float test = q1.x * q1.w - q1.y * q1.z;
            // Vector3 v;
            //
            // if (test>0.4995f*unit) { // singularity at north pole
            //     v.y = 2f * Mathf.Atan2 (q1.y, q1.x);
            //     v.x = Mathf.PI / 2;
            //     v.z = 0;
            //     return NormalizeAngles (v * Mathf.Rad2Deg);
            // }
            // if (test<-0.4995f*unit) { // singularity at south pole
            //     v.y = -2f * Mathf.Atan2 (q1.y, q1.x);
            //     v.x = -Mathf.PI / 2;
            //     v.z = 0;
            //     return NormalizeAngles (v * Mathf.Rad2Deg);
            // }
            // Quaternion q = new Quaternion (q1.w, q1.z, q1.x, q1.y);
            // v.y = (float)Math.Atan2 (2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));     // Yaw
            // v.x = (float)Math.Asin (2f * (q.x * q.z - q.w * q.y));                             // Pitch
            // v.z = (float)Math.Atan2 (2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));      // Roll
            // return NormalizeAngles (v * Mathf.Rad2Deg);

            #endregion

            //https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles#Quaternion_to_Euler_angles_(in_3-2-1_sequence)_conversion
            #region Wikipedia

            // Vec3 angles;
            // // roll (x-axis rotation)
            // float sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
            // float cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
            // //angles.x = Mathf.Atan2(sinr_cosp, cosr_cosp);
            // angles.x = Mathf.Atan2(sinr_cosp, cosr_cosp) - Mathf.PI / 2;
            //
            // // pitch (z-axis rotation)
            // float sinp = Mathf.Sqrt(1 + 2 * (q.w * q.y - q.x * q.z));
            // float cosp = Mathf.Sqrt(1 - 2 * (q.w * q.y - q.x * q.z));
            // //angles.z = 2 * Mathf.Atan2(sinp, cosp) - Mathf.PI / 2;
            // angles.z = 2 * Mathf.Atan2(sinp, cosp);
            //
            // // yaw (y-axis rotation)
            // float siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            // float cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            // angles.y = Mathf.Atan2(siny_cosp, cosy_cosp);
            // return angles * Mathf.Rad2Deg;

            #endregion

            #region Lean
            // // this implementation assumes normalized quaternion
            // // converts to Euler angles in 3-2-1 sequence
            // Vec3 angles;
            //
            // #region EuclideanGimbalTest
            // //double unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            // double unit = q.z*q.z + q.y*q.y + q.x*q.x + q.w*q.w; // if normalised is one, otherwise is correction factor
            // //double test = q.x*q.y + q.z*q.w;
            // double gimbalTest = q.z*q.y + q.x*q.w; 
            //
            // if (gimbalTest > 0.5*unit) { // singularity at north pole
            //     // heading = 2 * atan2(q.x,q.w);
            //     // attitude = Math.PI/2;
            //     // bank = 0;
            //     angles.y = 2 * (float)Math.Atan2(q.z,q.w); //Atan2 is less precise
            //     angles.x = Mathf.PI/2;
            //     angles.z = 0;
            //     return angles * Mathf.Rad2Deg;
            // }
            // if (gimbalTest < -0.5*unit) { // singularity at south pole
            //     // heading = -2 * atan2(q.x,q.w);
            //     // attitude = -Math.PI/2;
            //     // bank = 0;
            //     angles.y = -2 * (float)Math.Atan2(q.z,q.w);
            //     angles.x = -Mathf.PI/2;
            //     angles.z = 0;
            //     return angles * Mathf.Rad2Deg;
            // }
            // #endregion
            //
            // // roll (x-axis rotation) sin r 
            // double sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
            // double cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
            // //angles.roll = std::atan2(sinr_cosp, cosr_cosp);
            // angles.x = (float)Math.Atan2(sinr_cosp, cosr_cosp);
            //
            // // pitch (y-axis rotation)
            // double sinp = Math.Sqrt(1 + 2 * (q.w * q.y - q.x * q.z));
            // double cosp = Math.Sqrt(1 - 2 * (q.w * q.y - q.x * q.z));
            // //angles.pitch = 2 * std::atan2(sinp, cosp) - M_PI / 2;
            // angles.y = (float)(2 * Math.Atan2(sinp, cosp) - Math.PI / 2);
            //
            // // yaw (z-axis rotation)
            // double siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            // double cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            // //angles.yaw = std::atan2(siny_cosp, cosy_cosp);
            // angles.z = (float)(Math.Atan2(siny_cosp, cosy_cosp));
            //
            // return angles * Mathf.Rad2Deg;
            #endregion

            #region Lean2

            // float sinPitch = 2.0f * (q.w * q.x + q.y * q.z);
            // float cosPitch = 1.0f - 2.0f * (q.x * q.x + q.y * q.y);
            // float pitch = Mathf.Atan2(sinPitch, cosPitch);
            //
            // float sinYaw = 2.0f * (q.w * q.y - q.z * q.x);
            // float yaw;
            // if (Mathf.Abs(sinYaw) >= 1.0f)
            //     yaw = Mathf.PI / 2 * Mathf.Sign(sinYaw); 
            // else
            //     yaw = Mathf.Asin(sinYaw);
            //
            // float sinRoll = 2.0f * (q.w * q.z + q.x * q.y);
            // float cosRoll = 1.0f - 2.0f * (q.y * q.y + q.z * q.z);
            // float roll = Mathf.Atan2(sinRoll, cosRoll);
            //
            // return new Vec3(pitch, yaw, roll) * Mathf.Rad2Deg; //in radians
            #endregion

            #region Nacho

            // static float NormalizeAngle(float angle)
            // {
            //     while (angle > 360)
            //         angle -= 360;
            //     while (angle < 0)
            //         angle += 360;
            //     return angle;
            // }
            //
            // static Vector3 NormalizeAngles(Vector3 angles)
            // {
            //     angles.x = NormalizeAngle(angles.x);
            //     angles.y = NormalizeAngle(angles.y);
            //     angles.z = NormalizeAngle(angles.z);
            //     return angles;
            // }
            //
            // float sqw = rotation.w * rotation.w;
            // float sqx = rotation.x * rotation.x;
            // float sqy = rotation.y * rotation.y;
            // float sqz = rotation.z * rotation.z;
            // float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            // float test = rotation.x * rotation.w - rotation.y * rotation.z;
            // Vector3 v;
            //
            // if (test > 0.4999f * unit)   // singularity at north pole
            // { 
            //     v.y = 2f * Mathf.Atan2(rotation.y, rotation.x);
            //     v.x = Mathf.PI / 2;
            //     v.z = 0;
            //     return NormalizeAngles(v * Mathf.Rad2Deg);
            // }
            // if (test < -0.4999f * unit)  // singularity at south pole
            // { 
            //     v.y = -2f * Mathf.Atan2(rotation.y, rotation.x);
            //     v.x = -Mathf.PI / 2;
            //     v.z = 0;
            //     return NormalizeAngles(v * Mathf.Rad2Deg);
            // }
            //
            // Quat q = new Quat(rotation.w, rotation.z, rotation.x, rotation.y);
            // v.y = Mathf.Atan2(2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));     // Yaw
            // v.x = Mathf.Asin(2f * (q.x * q.z - q.w * q.y));                                           // Pitch
            // v.z = Mathf.Atan2(2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));      // Roll
            // return NormalizeAngles(v * Mathf.Rad2Deg);

            #endregion
        }

        #endregion
    }
}