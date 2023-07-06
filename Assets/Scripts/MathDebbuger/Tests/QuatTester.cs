using UnityEngine;

namespace CustomMath
{
    public class QuatTester : MonoBehaviour
    {
        [System.Serializable]
        public struct Q
        {
            public float x;
            public float y;
            public float z;
            public float w;

            #region constructors

            public Q(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public Q(Q q)
            {
                this.x = q.x;
                this.y = q.y;
                this.z = q.z;
                this.w = q.w;
            }

            #endregion

            #region equal operators

            //Quaternion myQuat = new Q(0,0,0)
            public static implicit operator Quaternion(Q q)
            {
                return new Quaternion(q.x, q.y, q.z, q.w);
            }

            //Q myQuat = new Quaternion(0,0,0)
            public static implicit operator Q(Quaternion q)
            {
                return new Q(q.x, q.y, q.z, q.w);
            }

            //Quaternion myQuat = new Q(0,0,0)
            public static implicit operator Quat(Q q)
            {
                return new Quat(q.x, q.y, q.z, q.w);
            }

            //Q myQuat = new Quaternion(0,0,0)
            public static implicit operator Q(Quat q)
            {
                return new Q(q.x, q.y, q.z, q.w);
            }

            #endregion
        }

        [SerializeField] Q[] inputs = new Q[1];
        [SerializeField] float optionalFloat;

        public Quaternion QuatCalcQ()
        {
            return (Q)Quat.identity;
        }
        public Quaternion QuaternionCalcQ()
        {
            return Quaternion.identity;
        }
        public float QuatCalcFloat()
        {
            return Quat.identity.w;
        }
        public float QuaternionCalcFloat()
        {
            return Quaternion.identity.w;
        }
    }
}