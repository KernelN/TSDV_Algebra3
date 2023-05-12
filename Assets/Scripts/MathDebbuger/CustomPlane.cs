using UnityEngine;
using System;

namespace CustomMath
{
    [Serializable]
    public struct CustomPlane : IFormattable
    {
        #region Properties

        public float distance;
        public CustomPlane flipped => new(-normal, distance);
        public Vec3 normal;

        #endregion

        #region Constructors

        public CustomPlane(Vec3 inNormal, Vec3 inPoint)
        {
            //https://www.cuemath.com/geometry/equation-of-plane/ (given vector and through a Point)
           
            normal = inNormal.normalized;
            
            //As each compoment is multiplied by the one in the other vector...
            //...is the same to send a negative vector or make negative the whole multiplication
            //(it's more efficient to make negative a float than a V3 though
            distance = -Vec3.Dot(normal, inPoint);
        }
        
        public CustomPlane(Vec3 inNormal, float d)
        {
            normal = inNormal.normalized;
            distance = d;
        }

        public CustomPlane(Vec3 a, Vec3 b, Vec3 c)
        {
            //https://www.cuemath.com/geometry/equation-of-plane/ (Three Non Collinear Points)

            normal = Vec3.Cross(b - a, c - a).normalized;
            
            //As each compoment is multiplied by the one in the other vector...
            //...is the same to send a negative vector or make negative the whole multiplication
            //(it's more efficient to make negative a float than a V3 though
            distance = -Vec3.Dot(normal, a);
        }

        #endregion

        #region Methods
        
        public override string ToString()
        {
            return "Distance = " + distance + "\nNormal = " + normal;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "Distance = " + distance + "\nNormal = " + normal;
        }

        public Vec3 ClosestPointOnPlane(Vec3 point)
        {
            //Multiply distanceToPoint by normal to get vector from plane to point
            //substract point to that calc to get the point in the plane
            //credits to ChatGPT
            return point - GetDistanceToPoint(point) * normal;
        }
        
        public void Flip()
        {
            this = flipped;
        }
        
        public float GetDistanceToPoint(Vec3 point)
        {
            //https://www.cuemath.com/geometry/distance-between-point-and-plane/
            return (Vec3.Dot(normal, point) + distance) / normal.magnitude;
        }

        public bool GetSide(Vec3 point)
        {
            return GetDistanceToPoint(point) > Mathf.Epsilon;
        }
        
        public bool Raycast(Ray ray, out float enter)
        {
            throw new NotImplementedException();
        }
        
        public bool SameSide(Vec3 inPt0, Vec3 inPt1)
        {
            return GetSide(inPt0) == GetSide(inPt1);
        }

        public void Set3Points(Vec3 a, Vec3 b, Vec3 c)
        {
            normal = Vec3.Cross(b - a, c - a).normalized;
            distance = -Vec3.Dot(normal, a);
        }

        public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
        {
            normal = inNormal.normalized;
            distance = -Vec3.Dot(normal, inPoint);
        }

        public static void Translate(ref CustomPlane plane, Vec3 translation)
        {
            //This is similar to the projection of the normal into the translation
            //It gives the distance from the old point to the new point
            //credits to ChatGPT, check cuemath projection for reference
            //https://www.cuemath.com/geometry/projection-vector/
            
            //No te voy a mentir lean, esta si que no la entiendo del todo, pero comprobe con el tester y funciona
            plane.distance += Vector3.Dot(plane.normal, translation);
        }
        #endregion

        #region Operators

        public static implicit operator CustomPlane(Plane plane)
        {
            return new CustomPlane(plane.normal, plane.distance);
        }
        
        public static implicit operator Plane(CustomPlane plane)
        {
            return new Plane(plane.normal, plane.distance);
        }

        #endregion
    }
}