using UnityEngine;

namespace CustomMath
{
    public class Vec3Tester : MonoBehaviour
    {
        [System.Serializable]
        public struct V3
        {
            public float x;
            public float y;
            public float z;

            #region constructors

            public V3(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public V3(V3 v3)
            {
                this.x = v3.x;
                this.y = v3.y;
                this.z = v3.z;
            }

            #endregion

            #region equal operators

            //Vector3 myVec = new V3(0,0,0)
            public static implicit operator Vector3(V3 v3)
            {
                return new Vector3(v3.x, v3.y, v3.z);
            }

            //V3 myVec = new Vector3(0,0,0)
            public static implicit operator V3(Vector3 v3)
            {
                return new V3(v3.x, v3.y, v3.z);
            }

            //Vector3 myVec = new V3(0,0,0)
            public static implicit operator Vec3(V3 v3)
            {
                return new Vec3(v3.x, v3.y, v3.z);
            }

            //V3 myVec = new Vector3(0,0,0)
            public static implicit operator V3(Vec3 v3)
            {
                return new V3(v3.x, v3.y, v3.z);
            }

            #endregion
        }

        [SerializeField] V3[] inputs = new V3[1];
        [SerializeField] float optionalFloat;

        public Vector3 Vec3CalcV3()
        {
            return Vec3.Reflect(inputs[0], inputs[1]);
            //return Vec3.ClampMagnitude(inputs[0], optionalFloat);
            //return Vec3.LerpUnclamped(inputs[0], inputs[1], optionalFloat);
        }
        public Vector3 Vector3CalcV3()
        {
            return Vector3.Reflect(inputs[0], inputs[1]);
            //return Vector3.ClampMagnitude(inputs[0], optionalFloat);
            //return Vector3.LerpUnclamped(inputs[0], inputs[1], optionalFloat);
        }
        public float Vec3CalcFloat()
        {
            return Vec3.SqrMagnitude(inputs[0]);
            //return Vec3.Distance(inputs[0], inputs[1]);
        }
        public float Vector3CalcFloat()
        {
            return Vector3.SqrMagnitude(inputs[0]);
            //return Vector3.Distance(inputs[0], inputs[1]);
        }
    }
}