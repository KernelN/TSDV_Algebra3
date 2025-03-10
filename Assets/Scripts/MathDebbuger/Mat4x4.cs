using System;
using UnityEngine;

namespace CustomMath
{
    public struct Mat4x4 : IEquatable<Mat4x4>, IFormattable
    {
        //Variables
        public float m00, m01, m02, m03; //m00 = row 0 column 0
        public float m10, m11, m12, m13; //m10 = row 1 column 0
        public float m20, m21, m22, m23; //m20 = row 2 column 0
        public float m30, m31, m32, m33; //m30 = row 3 column 0
        
        #region Properties

        public float determinant => GetDeterminant(this);
        public Mat4x4 inverse => GetInverse(this);
        public bool isIdentity => this == identity;
        public Vec3 lossyScale => GetLossyScale(this);
        public Quat rotation => GetRotation(this); 
        public float this[int index]
        { 
          get
          { switch (index)
          {
              default:
              case 0:
                  return m00;
              case 1:
                  return m10;
              case 2:
                  return m20;
              case 3:
                  return m30;
              case 4:
                  return m01;
              case 5:
                  return m11;
              case 6:
                  return m21;
              case 7:
                  return m31;
              case 8:
                  return m02;
              case 9:
                  return m12;
              case 10:
                  return m22;
              case 11:
                  return m32;
              case 12:
                  return m03;
              case 13:
                  return m13;
              case 14:
                  return m23;
              case 15:
                  return m33;
          } }
          set
          { switch (index)
          {
              case 0:
                  m00 = value;
                  break;
              case 1:
                  m10 = value;
                  break;
              case 2:
                  m20 = value;
                  break;
              case 3:
                  m30 = value;
                  break;
              case 4:
                  m01 = value;
                  break;
              case 5:
                  m11 = value;
                  break;
              case 6:
                  m21 = value;
                  break;
              case 7:
                  m31 = value;
                  break;
              case 8:
                  m02 = value;
                  break;
              case 9:
                  m12 = value;
                  break;
              case 10:
                  m22 = value;
                  break;
              case 11:
                  m32 = value;
                  break;
              case 12:
                  m03 = value;
                  break;
              case 13:
                  m13 = value;
                  break;
              case 14:
                  m23 = value;
                  break;
              case 15:
                  m33 = value;
                  break;
          } } 
        }
        public float this[int row, int column] 
        { get => GetRow(row)[column]; set => this[row + column * 4] = value; } 
        public Mat4x4 transpose => GetTranspose(this);
        
            
        #endregion
        
        //Static Properties
        public static Mat4x4 identity { get { return GetIdentity(); } }
        public static Mat4x4 zero { get { return new Mat4x4(); } }

        //Constructors
        
        #region Operators

        public static Mat4x4 operator *(Mat4x4 lhs, Mat4x4 rhs)
        {
            //https://byjus.com/maths/matrix-multiplication/
            //(assuming it's the same logic of 3x3, and testing on tester)
            //m01 = row 0 column 1
            
            //remplazar por mat * vec4 (4 veces) porque si no lean me mata
            Mat4x4 m4x4;
            m4x4.m00 =  lhs.m00 *  rhs.m00 +  lhs.m01 *  rhs.m10 +  lhs.m02 *  rhs.m20 +  lhs.m03 *  rhs.m30;
            m4x4.m01 =  lhs.m00 *  rhs.m01 +  lhs.m01 *  rhs.m11 +  lhs.m02 *  rhs.m21 +  lhs.m03 *  rhs.m31;
            m4x4.m02 =  lhs.m00 *  rhs.m02 +  lhs.m01 *  rhs.m12 +  lhs.m02 *  rhs.m22 +  lhs.m03 *  rhs.m32;
            m4x4.m03 =  lhs.m00 *  rhs.m03 +  lhs.m01 *  rhs.m13 +  lhs.m02 *  rhs.m23 +  lhs.m03 *  rhs.m33;
            m4x4.m10 =  lhs.m10 *  rhs.m00 +  lhs.m11 *  rhs.m10 +  lhs.m12 *  rhs.m20 +  lhs.m13 *  rhs.m30;
            m4x4.m11 =  lhs.m10 *  rhs.m01 +  lhs.m11 *  rhs.m11 +  lhs.m12 *  rhs.m21 +  lhs.m13 *  rhs.m31;
            m4x4.m12 =  lhs.m10 *  rhs.m02 +  lhs.m11 *  rhs.m12 +  lhs.m12 *  rhs.m22 +  lhs.m13 *  rhs.m32;
            m4x4.m13 =  lhs.m10 *  rhs.m03 +  lhs.m11 *  rhs.m13 +  lhs.m12 *  rhs.m23 +  lhs.m13 *  rhs.m33;
            m4x4.m20 =  lhs.m20 *  rhs.m00 +  lhs.m21 *  rhs.m10 +  lhs.m22 *  rhs.m20 +  lhs.m23 *  rhs.m30;
            m4x4.m21 =  lhs.m20 *  rhs.m01 +  lhs.m21 *  rhs.m11 +  lhs.m22 *  rhs.m21 +  lhs.m23 *  rhs.m31;
            m4x4.m22 =  lhs.m20 *  rhs.m02 +  lhs.m21 *  rhs.m12 +  lhs.m22 *  rhs.m22 +  lhs.m23 *  rhs.m32;
            m4x4.m23 =  lhs.m20 *  rhs.m03 +  lhs.m21 *  rhs.m13 +  lhs.m22 *  rhs.m23 +  lhs.m23 *  rhs.m33;
            m4x4.m30 =  lhs.m30 *  rhs.m00 +  lhs.m31 *  rhs.m10 +  lhs.m32 *  rhs.m20 +  lhs.m33 *  rhs.m30;
            m4x4.m31 =  lhs.m30 *  rhs.m01 +  lhs.m31 *  rhs.m11 +  lhs.m32 *  rhs.m21 +  lhs.m33 *  rhs.m31;
            m4x4.m32 =  lhs.m30 *  rhs.m02 +  lhs.m31 *  rhs.m12 +  lhs.m32 *  rhs.m22 +  lhs.m33 *  rhs.m32;
            m4x4.m33 =  lhs.m30 *  rhs.m03 +  lhs.m31 *  rhs.m13 +  lhs.m32 *  rhs.m23 +  lhs.m33 *  rhs.m33;
            return m4x4;
        }

        public static Vector4 operator *(Mat4x4 lhs, Vector4 vector)
        {
            Vector4 vec4;
            vec4.x =  lhs.m00 *  vector.x +  lhs.m01 *  vector.y +  lhs.m02 *  vector.z +  lhs.m03 *  vector.w;
            vec4.y =  lhs.m10 *  vector.x +  lhs.m11 *  vector.y +  lhs.m12 *  vector.z +  lhs.m13 *  vector.w;
            vec4.z =  lhs.m20 *  vector.x +  lhs.m21 *  vector.y +  lhs.m22 *  vector.z +  lhs.m23 *  vector.w;
            vec4.w =  lhs.m30 *  vector.x +  lhs.m31 *  vector.y +  lhs.m32 *  vector.z +  lhs.m33 *  vector.w;
            return vec4;
        }

        public static bool operator ==(Mat4x4 lhs, Mat4x4 rhs) => lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) && lhs.GetColumn(2) == rhs.GetColumn(2) && lhs.GetColumn(3) == rhs.GetColumn(3);

        public static bool operator !=(Mat4x4 lhs, Mat4x4 rhs) => !(lhs == rhs);

        #endregion

        #region IEquatable

        public bool Equals(Mat4x4 other)
        {
            return this == other;
        }

        #endregion

        #region Public Methods

        public Vector4 GetColumn(int index)
        {
            //m01 = row 0 column 1
            
            switch (index)
            {
                default:
                case 0: return new Vector4(m00, m10, m20, m30);
                case 1: return new Vector4(m01, m11, m21, m31);
                case 2: return new Vector4(m02, m12, m22, m32);
                case 3: return new Vector4(m03, m13, m23, m33);
            }
        }
        public Vec3 GetPosition()
        {
            //https://learnopengl.com/Getting-started/Transformations
            //m03 = row 0 column 3
            return new Vec3(m03, m13, m23);
        }
        public Vector4 GetRow(int index)
        {
            //m01 = row 0 column 1
            
            switch (index)
            {
                default:
                case 0: return new Vector4(m00, m01, m02, m03);
                case 1: return new Vector4(m10, m11, m12, m13);
                case 2: return new Vector4(m20, m21, m22, m23);
                case 3: return new Vector4(m30, m31, m32, m33);
            }
        }
        
        /// <summary>
        /// Transforms a position by this matrix (generic).
        /// Returns a position v transformed by the current fully arbitrary matrix.
        /// If the matrix is a regular 3D transformation matrix, it is much faster to use MultiplyPoint3x4 instead.
        /// MultiplyPoint is slower, but can handle projective transformations as well.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vec3 MultiplyPoint(Vec3 point)
        {
            Vec3 result = MultiplyPoint3x4(point);
            float scalar = 1.0f / (m30 * point.x + m31 * point.y + m32 * point.z + m33);
            return result * scalar;
        }
        
        /// <summary>
        /// Transforms a position by this matrix (fast).
        /// Returns a position v transformed by the current transformation matrix.
        /// This function is a faster version of MultiplyPoint; but it can only handle regular 3D transformations.
        /// MultiplyPoint is slower, but can handle projective transformations as well.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vec3 MultiplyPoint3x4(Vec3 point)
        {
            Vec3 result;
            
            //(projection of point into matrix's row axis) * scalar
            result.x = (m00 * point.x + m01 * point.y + m02 * point.z) + m03;
            result.y = (m10 * point.x + m11 * point.y + m12 * point.z) + m13;
            result.z = (m20 * point.x + m21 * point.y + m22 * point.z) + m23;
            
            return result;
        }
        
        /// <summary>
        /// Transforms a direction by this matrix.
        /// This function is similar to MultiplyPoint; but it transforms directions and not positions.
        /// When transforming a direction, only the rotation part of the matrix is taken into account.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Vec3 MultiplyVector(Vec3 source)
        {
            //m01 = row 0 column 1
            //Dot product of row and source
            source.x = m00 * source.x + m01 * source.y + m02 * source.z;
            source.y = m10 * source.x + m11 * source.y + m12 * source.z;
            source.z = m20 * source.x + m21 * source.y + m22 * source.z;
            return source;
        }
        public void SetColumn(int index, Vector4 column)
        {
            //m01 = row 0 column 1
            
            switch (index)
            {
                case 0: 
                    m00 = column.x;
                    m10 = column.y;
                    m20 = column.z;
                    m30 = column.w;
                    break;
                case 1:
                    m01 = column.x;
                    m11 = column.y;
                    m21 = column.z;
                    m31 = column.w;
                    break;
                case 2: 
                    m02 = column.x;
                    m12 = column.y;
                    m22 = column.z;
                    m32 = column.w;
                    break;
                case 3:
                    m03 = column.x;
                    m13 = column.y;
                    m23 = column.z;
                    m33 = column.w;
                    break;
            }
        }
        public void SetRow(int index, Vector4 row)
        {
            //m01 = row 0 column 1
            
            switch (index)
            {
                case 0: 
                    m00 = row.x;
                    m01 = row.y;
                    m02 = row.z;
                    m03 = row.w;
                    break;
                case 1:
                    m10 = row.x;
                    m11 = row.y;
                    m12 = row.z;
                    m13 = row.w;
                    break;
                case 2: 
                    m20 = row.x;
                    m21 = row.y;
                    m22 = row.z;
                    m23 = row.w;
                    break;
                case 3:
                    m30 = row.x;
                    m31 = row.y;
                    m32 = row.z;
                    m33 = row.w;
                    break;
            }
        }
        public void SetTRS(Vec3 translation, Quat rotation, Vec3 scale)
        {
            this = TRS(translation, rotation, scale);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            string str = "";

            str += m00.ToString(format, formatProvider) + " " + m01.ToString(format, formatProvider) + " " + m02.ToString(format, formatProvider) + " " + m03.ToString(format, formatProvider) + "\n";
            str += m10.ToString(format, formatProvider) + " " + m11.ToString(format, formatProvider) + " " + m12.ToString(format, formatProvider) + " " + m13.ToString(format, formatProvider) + "\n";
            str += m20.ToString(format, formatProvider) + " " + m21.ToString(format, formatProvider) + " " + m22.ToString(format, formatProvider) + " " + m23.ToString(format, formatProvider) + "\n";
            str += m30.ToString(format, formatProvider) + " " + m31.ToString(format, formatProvider) + " " + m32.ToString(format, formatProvider) + " " + m33.ToString(format, formatProvider) + "\n";

            return str;
        }

        public bool ValidTRS()
        {
            //https://learnopengl.com/Getting-started/Transformations
            
            bool valid = true;
            
            //This part is NOT performant (but it works)
            valid &= Math.Abs(m30) < Double.Epsilon; //4th column should be 0,0,0,1
            valid &= Math.Abs(m31) < Double.Epsilon; //4th column should be 0,0,0,1
            valid &= Math.Abs(m32) < Double.Epsilon; //4th column should be 0,0,0,1
            
            valid &= Math.Abs(m33 - 1.0f) < Double.Epsilon; //4th column should be 0,0,0,1
            
            return valid;
        }

        #endregion

        #region Static Methods

        //Public Methods
        public static Mat4x4 LookAt(Vec3 from, Vec3 to, Vec3 up)
        {
            //conseguir vector posicion relativa / from >> to
            //obtener angulos correspondientes a las coordenadas esfericas
            //definir rotacion en cada eje para apuntar en la direccion del vector from>to
            
            Quat rotation = Quat.LookRotation(to - from, up); //this has to be cheating, Lean will kill me
            return Rotate(rotation);
        } //TODO: NOT WORKING
        public static Mat4x4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            //https://www.scratchapixel.com/lessons/3d-basic-rendering/perspective-and-orthographic-projection-matrix/orthographic-projection-matrix.html
            //This engine runs on right major notation
            //Unity and openGL use column major notation, so it's manually transposed (to avoid transpose of 0 values)
            
            Mat4x4 m = new Mat4x4();
            
            // 2 / (right - left) is a way to clamp the values between -1 and 1
            m[0,0] = 2 / (right - left); 
            m[0,1] = 0; 
            m[0,2] = 0; 
            m[0,3] = -(right + left) / (right - left);
 
            //same with top and bottom
            m[1,0] = 0; 
            m[1,1] = 2 / (top - bottom); 
            m[1,2] = -(top + bottom) / (top - bottom);
            m[1,3] = 0; 
 
            //This TOO clamps the values between -1 and 1
            //But as how unity works, it could clamp the values between 0 and 1
            //Review with more time
            m[2,0] = 0; 
            m[2,1] = 0; 
            m[2,2] = -2 / (zFar - zNear); 
            m[2,3] = -(zFar + zNear) / (zFar - zNear);

            m[3, 0] = 0;
            m[3, 1] = 0;
            m[3, 2] = 0;
            m[3,3] = 1;

            return m;
        }
        public static Mat4x4 Perspective(float fov, float aspect, float zNear, float zFar)
        {
            //https://www.scratchapixel.com/lessons/3d-basic-rendering/perspective-and-orthographic-projection-matrix/opengl-perspective-projection-matrix.html
            //This engine runs on right major notation
            //Unity and openGL use column major notation, so it's manually transposed (to avoid transpose of 0 values)
            
            //left & bottom are the opposite of right and top, so they're just top & right * -1
            float scale = Mathf.Tan(fov * 0.5f * Mathf.PI / 180) * zNear;
            float right = aspect * scale;
            float left = -right;
            float top = scale;
            float bottom = -top; 
            
            Mat4x4 m = new Mat4x4();
            
            m[0,0] = 2 * zNear / (right - left); 
            m[0,1] = 0; 
            m[0,2] = 0; 
            m[0,3] = 0; 
 
            m[1,0] = 0; 
            m[1,1] = 2 * zNear / (top - bottom); 
            m[1,2] = 0; 
            m[1,3] = 0; 
 
            m[2,0] = (right + left) / (right - left); 
            m[2,1] = (top + bottom) / (top - bottom); 
            m[2,2] = -(zFar + zNear) / (zFar - zNear); 
            m[2,3] = -2 * zFar * zNear / (zFar - zNear); 
 
            m[3,0] = 0; 
            m[3,1] = 0; 
            m[3,2] = -1; 
            m[3,3] = 0;

            return m;
        }
        public static Mat4x4 Rotate(Quat q)
        {
            //https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToMatrix/index.htm
            
            Mat4x4 m = new Mat4x4();
            
            float xx      = q.x * q.x;
            float xy      = q.x * q.y;
            float xz      = q.x * q.z;
            float xw      = q.x * q.w;

            float yy      = q.y * q.y;
            float yz      = q.y * q.z;
            float yw      = q.y * q.w;

            float zz      = q.z * q.z;
            float zw      = q.z * q.w;

            m.m00  = 1 - 2 * ( yy + zz );
            m.m01  =     2 * ( xy - zw );
            m.m02 =     2 * ( xz + yw );

            m.m10  =     2 * ( xy + zw );
            m.m11  = 1 - 2 * ( xx + zz );
            m.m12  =     2 * ( yz - xw );

            m.m20  =     2 * ( xz - yw );
            m.m21  =     2 * ( yz + xw );
            m.m22 = 1 - 2 * ( xx + yy );

            m.m03  = m.m13 = m.m23 = m.m30 = m.m31 = m.m32 = 0;
            m.m33 = 1;
            return m;
        }
        public static Mat4x4 Scale(Vec3 scale)
        {
            //https://learnopengl.com/Getting-started/Transformations
            
            Mat4x4 m = identity;
            m.m00 = scale.x;
            m.m11 = scale.y;
            m.m22 = scale.z;
            return m;
        }
        public static Mat4x4 Translate(Vec3 translation)
        {
            //https://learnopengl.com/Getting-started/Transformations
            
            Mat4x4 m = identity;
            m.m03 = translation.x;
            m.m13 = translation.y;
            m.m23 = translation.z;
            return m;
        }
        public static Mat4x4 TRS(Vec3 translation, Quat rotation, Vec3 scale)
        {
            //https://learnopengl.com/Getting-started/Transformations
            
            Mat4x4 t = Translate(translation);
            Mat4x4 r = Rotate(rotation);
            Mat4x4 s = Scale(scale);
            
            
            return t*r*s;
        }
        
        //Methods for properties
        static float GetDeterminant(Mat4x4 m)
        {
            //https://byjus.com/maths/determinant-of-a-3x3-matrix/
            //https://byjus.com/maths/determinant-of-4x4-matrix/

            float det = 0;

            det += m.m00 * (m.m11 * (m.m22 * m.m33 - m.m23 * m.m32)
                         - m.m12 * (m.m21 * m.m33 - m.m23 * m.m31)
                         + m.m13 * (m.m21 * m.m32 - m.m22 * m.m31));
            det -= m.m01 * (m.m10 * (m.m22 * m.m33 - m.m23 * m.m32)
                         - m.m12 * (m.m20 * m.m33 - m.m23 * m.m30)
                         + m.m13 * (m.m20 * m.m32 - m.m22 * m.m30));
            det += m.m02 * (m.m10 * (m.m21 * m.m33 - m.m23 * m.m31)
                         - m.m11 * (m.m20 * m.m33 - m.m23 * m.m30)
                         + m.m13 * (m.m20 * m.m31 - m.m21 * m.m30));
            det -= m.m03 * (m.m10 * (m.m21 * m.m32 - m.m22 * m.m31)
                         - m.m11 * (m.m20 * m.m32 - m.m22 * m.m30)
                         + m.m12 * (m.m20 * m.m31 - m.m21 * m.m30));
            return det;
        }
        static Mat4x4 GetInverse(Mat4x4 m)
        {
            //https://byjus.com/maths/find-inverse-of-matrix/
            
            Mat4x4 inv = new Mat4x4();
            
            float det = GetDeterminant(m);
            
            if (Mathf.Abs(det) < Mathf.Epsilon)
            {
                // Matrix is not invertible, return identity matrix
                return Mat4x4.identity;
            }

            float invDet = 1f / det;
            
            inv = Mat4x4.GetTranspose(GetCofactor(m)); //Adjoint: transpose of cofactor matrix
            
            inv.SetColumn(0, inv.GetColumn(0) * invDet);
            inv.SetColumn(1, inv.GetColumn(1) * invDet);
            inv.SetColumn(2, inv.GetColumn(2) * invDet);
            inv.SetColumn(3, inv.GetColumn(3) * invDet);

            return inv;
        }
        static Mat4x4 GetIdentity()
        {
            Mat4x4 m = new Mat4x4();

            m.m00 = m.m11 = m.m22 = m.m33 = 1; 

            return m;
        }

        static Vec3 GetLossyScale(Mat4x4 m)
        {
            //https://discussions.unity.com/t/how-to-decompose-a-trs-matrix/63681/4
            Vec3 scale = new Vec3();
            scale.x = m.GetColumn(0).magnitude;
            scale.y = m.GetColumn(1).magnitude;
            scale.z = m.GetColumn(2).magnitude;
            return scale;
        }
        static Quat GetRotation(Mat4x4 m)
        {
            //https://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/
            Quat q = new Quat();
            
            float tr1 = 1.0f + m.m00 - m.m11 - m.m22;
            float tr2 = 1.0f - m.m00 + m.m11 - m.m22;
            float tr3 = 1.0f - m.m00 - m.m11 + m.m22;
            
            if (tr1 > tr2 && tr1 > tr3) { 
                float S = Mathf.Sqrt(tr1) * 2; // S=4*qx 
                q.w = (m.m21 - m.m12) / S;
                q.x = 0.25f * S;
                q.y = (m.m01 + m.m10) / S; 
                q.z = (m.m02 + m.m20) / S; 
            } else if (tr2 > tr1&&tr2 > tr3) { 
                float S = Mathf.Sqrt(tr2) * 2; // S=4*qy
                q.w = (m.m02 - m.m20) / S;
                q.x = (m.m01 + m.m10) / S; 
                q.y = 0.25f * S;
                q.z = (m.m12 + m.m21) / S; 
            } else { 
                float S = Mathf.Sqrt(tr3) * 2; // S=4*qz
                q.w = (m.m10 - m.m01) / S;
                q.x = (m.m02 + m.m20) / S;
                q.y = (m.m12 + m.m21) / S;
                q.z = 0.25f * S;
            }

            // //Trace = sum of diagonal elements
            // float tr = m.m00 + m.m11 + m.m22; 
            //
            // if (tr > 0)
            // {
            //     float S = Mathf.Sqrt(tr+1.0f) * 2; // S=4*qw 
            //     q.w = 0.25f * S;
            //     q.x = (m.m21 - m.m12) / S;
            //     q.y = (m.m02 - m.m20) / S; 
            //     q.z = (m.m10 - m.m01) / S; 
            //
            //     // Clamp w component to [0, 1] range
            //     //q.w = Mathf.Clamp(q.w, 0f, 1f);
            //
            //     // Normalize the quaternion
            //     q = q.normalized;
            //     
            // }
            
            //  else if ((m.m00 > m.m11)&(m.m00 > m.m22)) { 
            //     return new Quat();
            //     float S = Mathf.Sqrt(1.0f + m.m00 - m.m11 - m.m22) * 2; // S=4*qx 
            //     q.w = (m.m21 - m.m12) / S;
            //     q.x = 0.25f * S;
            //     q.y = (m.m01 + m.m10) / S; 
            //     q.z = (m.m02 + m.m20) / S; 
            // } else if (m.m11 > m.m22) { 
            //     return new Quat();
            //     float S = Mathf.Sqrt(1.0f + m.m11 - m.m00 - m.m22) * 2; // S=4*qy
            //     q.w = (m.m02 - m.m20) / S;
            //     q.x = (m.m01 + m.m10) / S; 
            //     q.y = 0.25f * S;
            //     q.z = (m.m12 + m.m21) / S; 
            // } else { 
            //     return new Quat();
            //     float S = Mathf.Sqrt(1.0f + m.m22 - m.m00 - m.m11) * 2; // S=4*qz
            //     q.w = (m.m10 - m.m01) / S;
            //     q.x = (m.m02 + m.m20) / S;
            //     q.y = (m.m12 + m.m21) / S;
            //     q.z = 0.25f * S;
            // }

            return q;
        }
        static Mat4x4 GetTranspose(Mat4x4 m)
        {
            //Transpose: change rows to columns & columns to rows
            //https://www.cuemath.com/algebra/transpose-of-a-matrix/
            
            Mat4x4 t = new Mat4x4();
            t.SetColumn(0, m.GetRow(0));
            t.SetColumn(1, m.GetRow(1));
            t.SetColumn(2, m.GetRow(2));
            t.SetColumn(3, m.GetRow(3));
            
            return t;
        }
        
        
        //Methods for operations
        static Mat4x4 Add(Mat4x4 a, Mat4x4 b)
        {
            Mat4x4 sum = new Mat4x4();
            sum.SetRow(0, a.GetRow(0) + b.GetRow(0));
            sum.SetRow(1, a.GetRow(1) + b.GetRow(1));
            sum.SetRow(2, a.GetRow(2) + b.GetRow(2));
            sum.SetRow(3, a.GetRow(3) + b.GetRow(3));
            return sum;
        }
        static Mat4x4 Substract(Mat4x4 a, Mat4x4 b)
        {
            Mat4x4 diff = new Mat4x4();
            diff.SetRow(0, a.GetRow(0) - b.GetRow(0));
            diff.SetRow(1, a.GetRow(1) - b.GetRow(1));
            diff.SetRow(2, a.GetRow(2) - b.GetRow(2));
            diff.SetRow(3, a.GetRow(3) - b.GetRow(3));
            return diff;
        }
        static Mat4x4 ScaleByScalar (Mat4x4 m, float scalar)
        {
            Mat4x4 scaled = new Mat4x4();
            scaled.SetRow(0, m.GetRow(0) * scalar);
            scaled.SetRow(1, m.GetRow(1) * scalar);
            scaled.SetRow(2, m.GetRow(2) * scalar);
            scaled.SetRow(3, m.GetRow(3) * scalar);
            return scaled;
        }
        static Mat4x4 GetCofactor(Mat4x4 m)
        {
            //Cofactor Matrix: the matrix of determinants of the minors obtained from the square matrix
            //https://www.cuemath.com/algebra/cofactor-matrix/
            
            Mat4x4 cofactor = Mat4x4.zero;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    int sign = ((row + col) % 2 == 0) ? 1 : -1;
                    cofactor[row, col] = sign * GetMinorDet(m, row, col);
                }
            }

            return cofactor;
        }
        static float GetMinorDet(Mat4x4 m, int rowToRemove, int colToRemove)
        {
            //Minor: the square matrix formed by deleting one row and one column from some larger square matrix.
            //https://www.cuemath.com/algebra/minor-of-matrix/
            
            float[,] minorMatrix = new float[3, 3]; //as we're using a 4x4, the minor is a 3x3
            int minorRow = 0;
            int minorCol;

            for (int row = 0; row < 4; row++)
            {
                if (row == rowToRemove)
                    continue;

                minorCol = 0;

                for (int col = 0; col < 4; col++)
                {
                    if (col == colToRemove)
                        continue;

                    minorMatrix[minorRow, minorCol] = m[row, col];
                    minorCol++;
                }

                minorRow++;
            }

            // Calculate the determinant of the minor matrix (for 3x3)
            float minorDet = minorMatrix[0, 0] * (minorMatrix[1, 1] * minorMatrix[2, 2] - minorMatrix[1, 2] * minorMatrix[2, 1]) -
                             minorMatrix[0, 1] * (minorMatrix[1, 0] * minorMatrix[2, 2] - minorMatrix[1, 2] * minorMatrix[2, 0]) +
                             minorMatrix[0, 2] * (minorMatrix[1, 0] * minorMatrix[2, 1] - minorMatrix[1, 1] * minorMatrix[2, 0]);

            return minorDet;
        }

        #endregion

        #region Func List

        /*
        Static Properties
        ***identity //https://docs.unity3d.com/ScriptReference/Matrix4x4-identity.html
        ***zero //https://docs.unity3d.com/ScriptReference/Matrix4x4-zero.html
        */

        /* Properties            
        decomposeProjection //https://docs.unity3d.com/ScriptReference/Matrix4x4-decomposeProjection.html
        ***determinant //https://docs.unity3d.com/ScriptReference/Matrix4x4-determinant.html
        ***inverse	//https://docs.unity3d.com/ScriptReference/Matrix4x4-inverse.html
        ***isIdentity	//https://docs.unity3d.com/ScriptReference/Matrix4x4-isIdentity.html
        ***lossyScale	//https://docs.unity3d.com/ScriptReference/Matrix4x4-lossyScale.html
        rotation    //https://docs.unity3d.com/ScriptReference/Matrix4x4-rotation.html
        ***this[int,int] //https://docs.unity3d.com/ScriptReference/Matrix4x4.Index_operator.html
        **transpose	//https://docs.unity3d.com/ScriptReference/Matrix4x4-transpose.html
        */

        /*
        Operators
        operator*	//https://docs.unity3d.com/ScriptReference/Matrix4x4-operator_multiply.html
        */

        /*
        Public Methods
        ***GetColumn	//https://docs.unity3d.com/ScriptReference/Matrix4x4.GetColumn.html
        **GetPosition	//https://docs.unity3d.com/ScriptReference/Matrix4x4.GetPosition.html
        ***GetRow	//https://docs.unity3d.com/ScriptReference/Matrix4x4.GetRow.html
        ***MultiplyPoint	//https://docs.unity3d.com/ScriptReference/Matrix4x4.MultiplyPoint.html
        ***MultiplyPoint3x4	//https://docs.unity3d.com/ScriptReference/Matrix4x4.MultiplyPoint3x4.html
        ***MultiplyVector  //https://docs.unity3d.com/ScriptReference/Matrix4x4.MultiplyVector.html
        ***SetColumn //https://docs.unity3d.com/ScriptReference/Matrix4x4.SetColumn.html
        ***SetRow //https://docs.unity3d.com/ScriptReference/Matrix4x4.SetRow.html
        **SetTRS //https://docs.unity3d.com/ScriptReference/Matrix4x4.SetTRS.html
        **ToString //https://docs.unity3d.com/ScriptReference/Matrix4x4.ToString.html
        TransformPlane //https://docs.unity3d.com/ScriptReference/Matrix4x4.TransformPlane.html
        ***ValidTRS	//https://docs.unity3d.com/ScriptReference/Matrix4x4.ValidTRS.html
        */

        /*
        Static Methods
        Frustum	//https://docs.unity3d.com/ScriptReference/Matrix4x4.Frustum.html
        Inverse3DAffine //https://docs.unity3d.com/ScriptReference/Matrix4x4.Inverse3DAffine.html
        ***LookAt //https://docs.unity3d.com/ScriptReference/Matrix4x4.LookAt.html
        ***Ortho //https://docs.unity3d.com/ScriptReference/Matrix4x4.Ortho.html
        ***Perspective //https://docs.unity3d.com/ScriptReference/Matrix4x4.Perspective.html
        ***Rotate //https://docs.unity3d.com/ScriptReference/Matrix4x4.Rotate.html
        ***Scale //https://docs.unity3d.com/ScriptReference/Matrix4x4.Scale.html
        ***Translate //https://docs.unity3d.com/ScriptReference/Matrix4x4.Translate.html
        **TRS //https://docs.unity3d.com/ScriptReference/Matrix4x4.TRS.html
         */

        #endregion
    }
}