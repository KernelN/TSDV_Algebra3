using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMath
{
    public class CollisionDetector : MonoBehaviour
    {
        struct Tri
        {
            public Vec3 a, b, c;
            public float area;
            
            public Tri (Vec3 a, Vec3 b, Vec3 c)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                //area = Vec3.Cross(b - a, c - a).magnitude; //REVISAR
                area = Mathf.Abs((b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y));
            }
        }
        int[][] triangles;
        CustomPlane[] faces;
        Tri[] tris;
        Vec3 cachedCenter;
        Mesh m;

        public Vec3 Extents {
            get
            {
                Vec3 e = m.bounds.extents;
                e.x *= transform.lossyScale.x;
                e.y *= transform.lossyScale.y;
                e.z *= transform.lossyScale.z;
                return e;
            } 
        }
        public Vec3 Max => transform.TransformPoint(m.bounds.max);
        public Vec3 Min => transform.TransformPoint(m.bounds.min);
        public Vec3 Center => transform.TransformPoint(m.bounds.center);
        
        [Header("DEBUG - BUTTONS")] [SerializeField]
        bool setButton;

        void Awake()
        {
            if (TryGetComponent<MeshFilter>(out var mf)) m = mf.mesh;
            else if (TryGetComponent<SkinnedMeshRenderer>(out var smr)) m = smr.sharedMesh;
            else
            {
                Debug.LogError("No mesh found");
                return;
            }

            //
            List<int[]> tempTris = new List<int[]>();
            for (int i = 0; i < m.subMeshCount; i++)
            {
                int[] smIndices = m.GetTriangles(i);
                for (int j = 0; j < smIndices.Length; j += 3)
                    tempTris.Add(new[] { smIndices[j], smIndices[j + 1], smIndices[j + 2] });
            }
            
            triangles = tempTris.ToArray();
            faces = new CustomPlane[tempTris.Count];
            tris = new Tri[tempTris.Count];
            
            BakeFaces(true);
        }
        void OnDrawGizmos()
        {
            if (setButton)
            {
                Awake();
                setButton = false;
            }
        }

        public void BakeFaces(bool forceBake = false)
        {
            //This prevents baking more than once per position
            if(!forceBake && cachedCenter == Center) return;
            cachedCenter = Center;
            
            for (int i = 0; i < triangles.Length; i++)
            {
                //Use t.TransformPoint(point) instead of point + t.position
                    //To take into account the rotation of the object
                tris[i] = new Tri(transform.TransformPoint(m.vertices[triangles[i][0]]),
                                    transform.TransformPoint(m.vertices[triangles[i][1]]),
                                    transform.TransformPoint(m.vertices[triangles[i][2]]));
                faces[i].Set3Points(tris[i].a, tris[i].b, tris[i].c);
                faces[i].normal *= -1f;
            }
        }
        public bool IsPointInside(Vec3 point)
        {
            Vec3 dir = Vec3.Forward * m.bounds.size.z * 1.1f;
            List<int> collidedPlanes = new List<int>();
            int facesCollided = 0;
            bool skip;
            for (int i = 0; i < faces.Length; i++)
            {
                //Check if already collided with this plane on another face
                skip = false;
                for (int j = 0; j < facesCollided; j++)
                {
                    skip = Mathf.Abs(faces[i].distance - faces[collidedPlanes[j]].distance)
                           < Mathf.Epsilon;
                    skip &= faces[i].normal == faces[collidedPlanes[j]].normal;
                }
                if(skip) continue;
                
                if (IsPointInPlane(faces[i], point, dir, out Vec3 collisionPoint))
                {
                    if (IsValidPlane(tris[i], collisionPoint))
                    {
                        collidedPlanes.Add(i);
                        facesCollided++;
                    }
                }
            }
            return facesCollided % 2 == 1;
        }
        private bool IsValidPlane(Tri tri, Vec3 point)
        {
            // http://www.jeffreythompson.org/collision-detection/tri-point.php
            // Triangle Point Collision
            float x1 = tri.a.x;
            float x2 = tri.b.x;
            float x3 = tri.c.x;

            float y1 = tri.a.y;
            float y2 = tri.b.y;
            float y3 = tri.c.y;
            
            float px = point.x;
            float py = point.y;
        
            // get the area of 3 triangles made between the point
            // and the corners of the triangle
            float area1 = Mathf.Abs((x1 - px) * (y2 - py) - (x2 - px) * (y1 - py));
            float area2 = Mathf.Abs((x2 - px) * (y3 - py) - (x3 - px) * (y2 - py));
            float area3 = Mathf.Abs((x3 - px) * (y1 - py) - (x1 - px) * (y3 - py));
        
            // if the sum of the three areas equals the original,
            // we're inside the triangle! (it means that each area is a part of the triangle)
            return Mathf.Abs(area1 + area2 + area3 - tri.area) < Vec3.epsilon;
        }
        bool IsPointInPlane(CustomPlane plane, Vec3 rOrigin, Vec3 rDist, out Vec3 collPoint)
        {
            collPoint = Vec3.Zero;

            float denom = Vec3.Dot(plane.normal, rDist);

            //If denom is 0, the ray is parallel to the plane
            if (Mathf.Abs(denom) <= Vec3.epsilon) return false;

            float t = Vec3.Dot(plane.normal * plane.distance - rOrigin, plane.normal) / denom;
            if (t < Vec3.epsilon) return false;

            collPoint = rOrigin + rDist * t;
            return true;
        }
    }
}