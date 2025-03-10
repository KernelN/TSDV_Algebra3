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
                mat.SetRow(0, m.m0);
                mat.SetRow(1, m.m1);
                mat.SetRow(2, m.m2);
                mat.SetRow(3, m.m3);
                return mat;
            }

            //M myMat = new Matrix4x4(0,0,0,0)
            public static implicit operator M(Matrix4x4 m)
            {
                M mat = new M();
                mat.m0 = m.GetRow(0);
                mat.m1 = m.GetRow(1);
                mat.m2 = m.GetRow(2);
                mat.m3 = m.GetRow(3);
                return mat;
            }

            //Matrix4x4 myMat = new M(0,0,0,0)
            public static implicit operator Mat4x4(M m)
            {
                Mat4x4 mat = new Mat4x4();
                mat.SetRow(0, m.m0);
                mat.SetRow(1, m.m1);
                mat.SetRow(2, m.m2);
                mat.SetRow(3, m.m3);
                return mat;
            }

            //M myMat = new Matrix4x4(0,0,0,0)
            public static implicit operator M(Mat4x4 m)
            {
                M mat = new M();
                mat.m0 = m.GetRow(0);
                mat.m1 = m.GetRow(1);
                mat.m2 = m.GetRow(2);
                mat.m3 = m.GetRow(3);
                return mat;
            }

            #endregion
        }

        [SerializeField] M[] inputs = new M[1];
        [SerializeField] QuatTester.Q[] optionalQuats;
        [SerializeField] Vec3[] optionalVec3s;
        [SerializeField] float[] optionalFloats;

        public Matrix4x4 MatCalcM()
        {
            //M m = ((Mat4x4)inputs[0]).inverse;
            //M m = Mat4x4.TRS(optionalVec3s[0], optionalQuats[0], optionalVec3s[1]);
            M m = Mat4x4.LookAt(optionalVec3s[0], optionalVec3s[1], optionalVec3s[2]);
            //M m = Mat4x4.Rotate(optionalQuats[0]);
            //M m = Mat4x4.Perspective(optionalFloats[0], optionalFloats[1], optionalFloats[2], optionalFloats[3]);
            //M m = Mat4x4.identity;
            // Mat4x4 m1 = m;
            // m1.SetTRS(optionalVec3s[0], optionalQuats[0], optionalVec3s[1]);
            // m = m1;
            return m;
        }
        public Matrix4x4 MatrixCalcM()
        {
            //M m = ((Matrix4x4)inputs[0]).inverse;
            //M m = Matrix4x4.TRS(optionalVec3s[0], optionalQuats[0], optionalVec3s[1]);
            M m = Matrix4x4.LookAt(optionalVec3s[0], optionalVec3s[1], optionalVec3s[2]);
            //M m = Matrix4x4.Rotate(optionalQuats[0]);
            //M m = Matrix4x4.Perspective(optionalFloats[0], optionalFloats[1], optionalFloats[2], optionalFloats[3]);
            //M m = Matrix4x4.identity;
            // Matrix4x4 m1 = m;
            // m1.SetTRS(optionalVec3s[0], optionalQuats[0], optionalVec3s[1]);
            // m = m1;
            return m;
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
            Quat q = (QuatTester.Q)((Mat4x4)inputs[0]).rotation;
            return q;
        }
        public Quat MatrixCalcQ()
        {
            Quat q = (QuatTester.Q)((Matrix4x4)inputs[0]).rotation;
            return q;
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
            Vec3 v = ((Mat4x4)inputs[0]).lossyScale;
            return v;
        }
        public Vec3 MatrixCalcVec3()
        {
            Vec3 v = ((Matrix4x4)inputs[0]).lossyScale;
            return v;
        }
        public float MatCalcFloat()
        {
            return ((Mat4x4)inputs[0]).determinant;
        }
        public float MatrixCalcFloat()
        {
            return ((Matrix4x4)inputs[0]).determinant;
        }
        public bool MatCalcBool()
        {
            return ((Mat4x4)inputs[0]).ValidTRS();
            return Mat4x4.identity.isIdentity;
        }
        public bool MatrixCalcBool()
        {
            return ((Matrix4x4)inputs[0]).ValidTRS();;
            return Matrix4x4.identity.isIdentity;
        }
    }
}