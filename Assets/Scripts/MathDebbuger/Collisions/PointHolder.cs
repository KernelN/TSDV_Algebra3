using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMath.Collisions
{
    public class PointHolder : MonoBehaviour
    {
        [SerializeField] CollisionDetector collA, collB;
        [SerializeField] int pointsPerLine;
        [SerializeField, Range(0, 1)] float pointDis;
        [Header("Run Values")]
        Vec3[][][] points;
        [SerializeField] List<Vec3> pointsList;

        [Header("DEBUGGER")]
        [SerializeField, Range(0,.25f)] float gizmoRadius;
        [SerializeField] Vec3 debugPointByPos;
        [SerializeField] int debugPointById;
        [SerializeField] Color gizmoColor;
        [SerializeField] Color gizmoDebugPosColor;
        [SerializeField] Color gizmoDebugIdColor;
        [SerializeField] Color gizmoDebugAMeshColor;
        [SerializeField] Color gizmoDebugBMeshColor;
        [SerializeField] Color gizmoDebugCollisionColor;
        [SerializeField] bool enableGizmos;
        int[] aGizmoIDs;
        int[] bGizmoIDs;
        int collGizmoID;

        //Unity Events
        void Awake()
        {
            if(points == null || points.Length < 1) GeneratePoints();
        }
        void Update()
        {
            if (collA && collB)
                AreMeshesColliding(collA, collB, true);
        }
        void OnDrawGizmos()
        {
            if(!enableGizmos) return;
            if(points == null || points.Length < 1) return;
            
            Gizmos.color = gizmoColor;
            for (int i = 0; i < pointsPerLine; i++)
            {
                for (int j = 0; j < pointsPerLine; j++)
                {
                    for (int k = 0; k < pointsPerLine; k++)
                    {
                        Gizmos.DrawCube(points[i][j][k], Vector3.one * gizmoRadius);
                    }
                }
            }
            
            //Draw point by position
            Vector3 debugPoint;
            debugPoint = pointsList[GetIDByPoint(debugPointByPos)];
            Gizmos.color = gizmoDebugPosColor;
            Gizmos.DrawCube(debugPoint, Vector3.one * gizmoRadius);
            
            //Draw point by ID
            debugPoint = GetPointByID(debugPointById);
            Gizmos.color = gizmoDebugIdColor;
            Gizmos.DrawCube(debugPoint, Vector3.one * gizmoRadius);
            
            //Draw mesh A points
            Gizmos.color = gizmoDebugAMeshColor;
            for (int i = 0; i < aGizmoIDs.Length; i++)
                Gizmos.DrawCube(GetPointByID(aGizmoIDs[i]), Vector3.one * gizmoRadius);
            
            //Draw mesh B points
            Gizmos.color = gizmoDebugBMeshColor;
            for (int i = 0; i < bGizmoIDs.Length; i++)
                Gizmos.DrawCube(GetPointByID(bGizmoIDs[i]), Vector3.one * gizmoRadius);
            
            //Draw collision points
            if (collGizmoID > -1)
            {
                Gizmos.color = gizmoDebugCollisionColor;
                Gizmos.DrawCube(GetPointByID(collGizmoID), Vector3.one * gizmoRadius);
            }
        }

        //Methods
        public void GeneratePoints()
        {
            if (points == null || Math.Abs(points.Length - Mathf.Pow(pointsPerLine,3)) > .1f)
            {
                GeneratePointArray();
            }
            
            float x, y, z;
            for (int i = 0; i < pointsPerLine; i++)
            {
                for (int j = 0; j < pointsPerLine; j++)
                {
                    for (int k = 0; k < pointsPerLine; k++)
                    {
                        ////This was made to have the points centered, depracted
                        // x = i * pointDis - i * pointDis / 2;
                        // y = j * pointDis - j * pointDis / 2;
                        // z = k * pointDis - k * pointDis / 2;
                        //Set position
                        x = i * pointDis;
                        y = j * pointDis;
                        z = k * pointDis;
                        points[i][j][k] = new Vec3(x, y, z);
                        
                        //Add to list
                        pointsList.Add(points[i][j][k]);
                    }
                }
            }
        }
        public bool AreMeshesColliding(CollisionDetector a, CollisionDetector b, bool drawGizmos = false)
        {
            Vec3 dist = b.Center - a.Center;
            
            //Small optimization | if AABB are too far, no need to check for mesh
            if (dist.magnitude > a.Extents.magnitude + b.Extents.magnitude)
            {
                //If drawGizmos is on, bake faces anyway
                if (drawGizmos)
                {
                    a.BakeFaces();
                    b.BakeFaces();

                    aGizmoIDs = GetPoints(a);
                    bGizmoIDs = GetPoints(b);

                    collGizmoID = -1;
                }
                return false;
            }

            a.BakeFaces();
            b.BakeFaces();
            
            List<int> aPoints = new List<int>(GetPoints(a));

            if (drawGizmos)
            {
                aGizmoIDs = aPoints.ToArray();
                bGizmoIDs = GetPoints(b);
                collGizmoID = -1;
            }
            
            Vec3 start = a.Center + a.Extents.magnitude * dist.normalized;
            Vector3Int sIndex = GetIndexByPoint(start);
            Vec3 end = a.Center - a.Extents.magnitude * dist.normalized;
            Vector3Int eIndex = GetIndexByPoint(end);
            
            int xInc = dist.x > 0 ? 1 : -1;
            int yInc = dist.y > 0 ? 1 : -1;
            int zInc = dist.z > 0 ? 1 : -1;
            
            //Run through all points in bounding box, if any is inside the mesh, return true
            //If ran through all mesh A points without order, triple for wouldn't be needed
            for (int i = sIndex.x; i != eIndex.x; i+=xInc)
                for (int j = sIndex.y; j != eIndex.y; j+=yInc)
                    for (int k = sIndex.z; k != eIndex.z; k+=zInc)
                    {
                        // if(!aPoints.Contains(GetIDByPoint(points[i][j][k])))
                        //     continue;
                        if (b.IsPointInside(points[i][j][k]))
                        {
                            if(drawGizmos)
                                collGizmoID = GetIDByPoint(points[i][j][k]);
                            return true;
                        }
                    }
            
            return false;
        }
        public int[] GetPoints(CollisionDetector collider)
        {
            List<int> pointsInside = new List<int>();
                
            Vector3Int minPoint = GetIndexByPoint(collider.Min);
            Vector3Int maxPoint = GetIndexByPoint(collider.Max);
            
            //Run through all the bounding box, checking which points are inside the mesh
            for (int i = minPoint.x; i <= maxPoint.x; i++)
                for (int j = minPoint.y; j <= maxPoint.y; j++)
                    for (int k = minPoint.z; k <= maxPoint.z; k++)
                        if (collider.IsPointInside(points[i][j][k]))
                            pointsInside.Add(GetIDByPoint(points[i][j][k]));
            //El siguiente algoritmo toma el punto mas lejano del centro y se va acercando
            //Una vez que encontro un punto que esta adentro del 2do objeto, devuelve true
            return pointsInside.ToArray();
        }
        void GeneratePointArray()
        {
            points = new Vec3[pointsPerLine][][];
    
            if(pointsList == null)
                pointsList = new List<Vec3>();
            else
                pointsList.Clear();

            for (int i = 0; i < pointsPerLine; i++)
            {
                points[i] = new Vec3[pointsPerLine][];

                for (int j = 0; j < pointsPerLine; j++)
                {
                    points[i][j] = new Vec3[pointsPerLine];
                }
            }
        }
        int GetIDByPoint(Vec3 pos)
        {
            int x = Mathf.RoundToInt(pos.x / pointDis);
            int y = Mathf.RoundToInt(pos.y / pointDis);
            int z = Mathf.RoundToInt(pos.z / pointDis);
            
            int id = x * pointsPerLine * pointsPerLine + y * pointsPerLine + z;
            
            if(id < 0) id = 0;
            else if(id >= pointsList.Count) id = pointsList.Count - 1;
            
            return id;
        }
        Vec3 GetPointByID(int id)
        {
            if (id < 0) id = 0;
            else if(id >= pointsList.Count) id = pointsList.Count - 1;
            return pointsList[id];
        }
        Vector3Int GetIndexByPoint(Vec3 pos)
        {
            int x = Mathf.RoundToInt(pos.x / pointDis);
            int y = Mathf.RoundToInt(pos.y / pointDis);
            int z = Mathf.RoundToInt(pos.z / pointDis);
            
            if(x < 0) x = 0;
            else if(x >= pointsPerLine) x = pointsPerLine - 1;
            
            if(y < 0) y = 0;
            else if(y >= pointsPerLine) y = pointsPerLine - 1;
            
            if(z < 0) z = 0;
            else if(z >= pointsPerLine) z = pointsPerLine - 1;
            
            return new Vector3Int(x, y, z);
        }
    }
}