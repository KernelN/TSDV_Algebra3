using UnityEngine;

namespace CustomMath
{
    public class Mat4x4Tester : MonoBehaviour
    {
        [System.Serializable]
        public struct M
        {
            public Vector4 m0;
            public Vector4 m1;
            public Vector4 m2;
            public Vector4 m3;

            #region constructors

            public M(Vector4 m0, Vector4 m1, Vector4 m2, Vector4 m3)
            {
                this.m0 = m0;
                this.m1 = m1;
                this.m2 = m2;
                this.m3 = m3;
            }

            public M(M m)
            {
                m0 = m.m0;
                m1 = m.m1;
                m2 = m.m2;
                m3 = m.m3;
            }

            #endregion

            #region equal operators

            //Matrix4x4 myMat = new M(0,0,0,0)
            public static implicit operator Matrix4x4(M m)
            {
                Matrix4x4 mat = new Matrix4x4();
                mat.SetColumn(0, m.m0);
                mat.SetColumn(1, m.m1);
                mat.SetColumn(2, m.m2);
                mat.SetColumn(3, m.m3);
                return mat;
            }

            //M myMat = new Matrix4x4(0,0,0,0)
            public static implicit operator M(Matrix4x4 m)
            {
                M mat = new M();
                mat.m0 = m.GetColumn(0);
                mat.m1 = m.GetColumn(1);
                mat.m2 = m.GetColumn(2);
                mat.m3 = m.GetColumn(3);
                return mat;
            }

            //Matrix4x4 myMat = new M(0,0,0,0)
            public static implicit operator Mat4x4(M m)
            {
                Mat4x4 mat = new Mat4x4();
                mat.SetColumn(0, m.m0);
                mat.SetColumn(1, m.m1);
                mat.SetColumn(2, m.m2);
                mat.SetColumn(3, m.m3);
                return mat;
            }

            //M myMat = new Matrix4x4(0,0,0,0)
            public static implicit operator M(Mat4x4 m)
            {
                M mat = new M();
                mat.m0 = m.GetColumn(0);
                mat.m1 = m.GetColumn(1);
                mat.m2 = m.GetColumn(2);
                mat.m3 = m.GetColumn(3);
                return mat;
            }

            #endregion
        }

        [SerializeField] M[] inputs = new M[1];
        [SerializeField] Vec3[] optionalVec3s;
        [SerializeField] float optionalFloat;

        public Matrix4x4 MatCalcM()
        {
            return (M)Mat4x4.identity;
        }
        public Matrix4x4 MatrixCalcM()
        {
            return Matrix4x4.identity;
        }
        public CustomPlane MatCalcCPlane()
        {
            return new CustomPlane();
        }
        public CustomPlane MatrixCalcCPlane()
        {
            return new CustomPlane();
        }
        public Quat MatCalcQ()
        {
            return Quat.identity;
        }
        public Quat MatrixCalcQ()
        {
            return Quat.identity;
        }
        public Vector4 MatCalcVector4()
        {
            return Vector4.zero;
        }
        public Vector4 MatrixCalcVector4()
        {
            return Vector4.zero;
        }
        public Vec3 MatCalcVec3()
        {
            return Vec3.Zero;
        }
        public Vec3 MatrixCalcVec3()
        {
            return Vec3.Zero;
        }
        public float MatCalcFloat()
        {
            return 0;
        }
        public float MatrixCalcFloat()
        {
            return 0;
        }
        public bool MatCalcBool()
        {
            return false;
        }
        public bool MatrixCalcBool()
        {
            return false;
        }
    }
}