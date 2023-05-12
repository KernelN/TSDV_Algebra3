using UnityEngine;

namespace CustomMath
{
    public class CustomPlaneTester : MonoBehaviour
    {
        [System.Serializable]
        public struct P
        {
            public Vector3 n;
            public float d;

            #region constructors

            public P(Vector3 n, float d)
            {
                this.n = n;
                this.d = d;
            }

            public P(P p)
            {
                this.n = p.n;
                this.d = p.d;
            }

            #endregion

            #region equal operators

            //Plane myPlane = new P(V3,0)
            public static implicit operator Plane(P p)
            {
                return new Plane(p.n, p.d);
            }

            //P myPlane = new Plane(V3,0)
            public static implicit operator P(Plane p)
            {
                return new P(p.normal, p.distance);
            }

            //CustomPlane myPlane = new P(V3,0)
            public static implicit operator CustomPlane(P p)
            {
                return new CustomPlane(p.n, p.d);
            }

            //P myPlane = new CustomPlane(V3,0)
            public static implicit operator P(CustomPlane p)
            {
                return new P(p.normal, p.distance);
            }

            #endregion
        }

        [SerializeField] P[] inputs = new P[1];
        [SerializeField] Vector3[] vec3s = new Vector3[3];
        [SerializeField] float[] floats;

        public Plane CustomPlaneCalcP()
        {
            P newP = new CustomPlane(vec3s[0], vec3s[1]);
            ((CustomPlane)newP).Flip();
            return newP;
        }
        public Plane PlaneCalcP()
        {
            P newP = new Plane(vec3s[0], vec3s[1]);
            ((Plane)newP).Flip();
            return newP;
        }
        public Vector3 CustomPlaneCalcV3()
        {
            return ((CustomPlane)inputs[0]).ClosestPointOnPlane(vec3s[0]);
        }
        public Vector3 PlaneCalcV3()
        {
            return ((Plane)inputs[0]).ClosestPointOnPlane(vec3s[0]);
        }
        public float CustomPlaneCalcFloat()
        {
            return ((CustomPlane)inputs[0]).GetDistanceToPoint(vec3s[0]);
        }
        public float PlaneCalcFloat()
        {
            return ((Plane)inputs[0]).GetDistanceToPoint(vec3s[0]);
        }
        public bool CustomPlaneCalcBool()
        {
            return ((CustomPlane)inputs[0]).GetSide(vec3s[0]);
        }
        public bool PlaneCalcBool()
        {
            return ((Plane)inputs[0]).GetSide(vec3s[0]);
        }
    }
}