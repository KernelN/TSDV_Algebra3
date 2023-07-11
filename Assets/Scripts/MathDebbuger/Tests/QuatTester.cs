using UnityEngine;

namespace CustomMath
{
    public class QuatTester : MonoBehaviour
    {
        [System.Serializable]
        public struct Q
        {
            [Range(-1,1)] public float x;
            [Range(-1,1)] public float y;
            [Range(-1,1)] public float z;
            [Range(-1,1)] public float w;

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

            //Quaternion myQuat = new Q(0,0,0,0)
            public static implicit operator Quaternion(Q q)
            {
                return new Quaternion(q.x, q.y, q.z, q.w);
            }

            //Q myQuat = new Quaternion(0,0,0,0)
            public static implicit operator Q(Quaternion q)
            {
                return new Q(q.x, q.y, q.z, q.w);
            }

            //Quaternion myQuat = new Q(0,0,0,0)
            public static implicit operator Quat(Q q)
            {
                return new Quat(q.x, q.y, q.z, q.w);
            }

            //Q myQuat = new Quaternion(0,0,0,0)
            public static implicit operator Q(Quat q)
            {
                return new Q(q.x, q.y, q.z, q.w);
            }

            #endregion
        }

        [SerializeField] Q[] inputs = new Q[1];
        [SerializeField] Vec3[] optionalVec3s;
        [SerializeField] float optionalFloat;

        public Quaternion QuatCalcQ()
        {
            //Q quat= ((Quat)inputs[0]).normalized;
            
            //Q quat = Quat.RotateTowards(inputs[0], inputs[1], optionalFloat);
            
            //Q quat = Quat.Inverse(inputs[0]);
            
            Q quat = Quat.Euler(optionalVec3s[0]);
            
            //Q quat = Quat.LookRotation(optionalVec3s[0], optionalVec3s[1]);
            
            //Q quat = Quat.AngleAxis(optionalFloat, optionalVec3s[0]);
            
            return quat;
        }
        public Quaternion QuaternionCalcQ()
        {
           // Q quat= ((Quaternion)inputs[0]).normalized;
            
            //Q quat = Quaternion.RotateTowards(inputs[0], inputs[1], optionalFloat);

            //Q quat = Quaternion.Inverse(inputs[0]);
            
            Q quat = Quaternion.Euler(optionalVec3s[0]);
            
            //Q quat = Quaternion.LookRotation(optionalVec3s[0], optionalVec3s[1]);
            
            //Q quat = Quaternion.AngleAxis(optionalFloat, optionalVec3s[0]);
            
            return quat;
        }
        public Vec3 QuatCalcVec3()
        {
            Vec3 v = ((Quat)inputs[0]).eulerAngles;
            //float angle;
            //((Quat)inputs[0]).ToAngleAxis(out angle, out v);
            return v;
        }
        public Vec3 QuaternionCalcVec3()
        {
            Vector3 v = ((Quaternion)inputs[0]).eulerAngles;
            //float angle;
            //((Quaternion)inputs[0]).ToAngleAxis(out angle, out v);
            return v;
        }
        public float QuatCalcFloat()
        {
            Quat q = (Quat)inputs[0];
            return q.x * q.y + q.z * q.w;
            //return Quat.Angle(inputs[0], inputs[1]);
        }
        public float QuaternionCalcFloat()
        {
            Vector3 v;
            float angle;
            ((Quaternion)inputs[0]).ToAngleAxis(out angle, out v);
            //return angle;
            
            //Temp
            Quat q = (Quat)inputs[0];
            return 0.5f * (q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
            
            //return Quaternion.Angle(inputs[0], inputs[1]);;
        }
    }
}