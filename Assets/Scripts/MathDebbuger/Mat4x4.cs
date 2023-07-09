using System;
using System.Collections;
using System.Collections.Generic;
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

        public string ToString(string format, IFormatProvider formatProvider)
        {
            string str = "";

            str += m00.ToString(format, formatProvider) + " " + m01.ToString(format, formatProvider) + " " + m02.ToString(format, formatProvider) + " " + m03.ToString(format, formatProvider) + "\n";
            str += m10.ToString(format, formatProvider) + " " + m11.ToString(format, formatProvider) + " " + m12.ToString(format, formatProvider) + " " + m13.ToString(format, formatProvider) + "\n";
            str += m20.ToString(format, formatProvider) + " " + m21.ToString(format, formatProvider) + " " + m22.ToString(format, formatProvider) + " " + m23.ToString(format, formatProvider) + "\n";
            str += m30.ToString(format, formatProvider) + " " + m31.ToString(format, formatProvider) + " " + m32.ToString(format, formatProvider) + " " + m33.ToString(format, formatProvider) + "\n";

            return str;
        }

        #endregion

        #region Static Methods

        static Mat4x4 GetIdentity()
        {
            Mat4x4 m = new Mat4x4();
            
            for (int i = 0; i < 4; i++)
            {
                m[0, i] = 1;
            }

            return m;
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
        determinant //https://docs.unity3d.com/ScriptReference/Matrix4x4-determinant.html
        inverse	//https://docs.unity3d.com/ScriptReference/Matrix4x4-inverse.html
        isIdentity	//https://docs.unity3d.com/ScriptReference/Matrix4x4-isIdentity.html
        lossyScale	//https://docs.unity3d.com/ScriptReference/Matrix4x4-lossyScale.html
        rotation    //https://docs.unity3d.com/ScriptReference/Matrix4x4-rotation.html
        this[int,int] //https://docs.unity3d.com/ScriptReference/Matrix4x4.Index_operator.html
        transpose	//https://docs.unity3d.com/ScriptReference/Matrix4x4-transpose.html
        */

        /*
        Operators
        operator*	//https://docs.unity3d.com/ScriptReference/Matrix4x4-operator_multiply.html
        */

        /*
        Public Methods
        ***GetColumn	//https://docs.unity3d.com/ScriptReference/Matrix4x4.GetColumn.html
        GetPosition	//https://docs.unity3d.com/ScriptReference/Matrix4x4.GetPosition.html
        ***GetRow	//https://docs.unity3d.com/ScriptReference/Matrix4x4.GetRow.html
        MultiplyPoint	//https://docs.unity3d.com/ScriptReference/Matrix4x4.MultiplyPoint.html
        MultiplyPoint3x4	//https://docs.unity3d.com/ScriptReference/Matrix4x4.MultiplyPoint3x4.html
        MultiplyVector  //https://docs.unity3d.com/ScriptReference/Matrix4x4.MultiplyVector.html
        ***SetColumn //https://docs.unity3d.com/ScriptReference/Matrix4x4.SetColumn.html
        ***SetRow //https://docs.unity3d.com/ScriptReference/Matrix4x4.SetRow.html
        SetTRS //https://docs.unity3d.com/ScriptReference/Matrix4x4.SetTRS.html
        ToString //https://docs.unity3d.com/ScriptReference/Matrix4x4.ToString.html
        TransformPlane //https://docs.unity3d.com/ScriptReference/Matrix4x4.TransformPlane.html
        ValidTRS	//https://docs.unity3d.com/ScriptReference/Matrix4x4.ValidTRS.html
        */

        /*
        Static Methods
        Frustum	//https://docs.unity3d.com/ScriptReference/Matrix4x4.Frustum.html
        Inverse3DAffine //https://docs.unity3d.com/ScriptReference/Matrix4x4.Inverse3DAffine.html
        LookAt //https://docs.unity3d.com/ScriptReference/Matrix4x4.LookAt.html
        Ortho //https://docs.unity3d.com/ScriptReference/Matrix4x4.Ortho.html
        Perspective //https://docs.unity3d.com/ScriptReference/Matrix4x4.Perspective.html
        Rotate //https://docs.unity3d.com/ScriptReference/Matrix4x4.Rotate.html
        Scale //https://docs.unity3d.com/ScriptReference/Matrix4x4.Scale.html
        Translate //https://docs.unity3d.com/ScriptReference/Matrix4x4.Translate.html
        TRS //https://docs.unity3d.com/ScriptReference/Matrix4x4.TRS.html
         */

        #endregion
    }
}