using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMath.Collisions
{
    public class PointHolder : MonoBehaviour
    {
        [Header("Set Values")]
        [SerializeField] int pointsPerLine;
        [SerializeField, Range(0, 1)] float pointDis;
        [Header("Run Values")]
        Vec3[][][] points;
        [SerializeField] List<Vec3> pointsList;

        [Header("DEBUGGER")]
        [SerializeField, Range(0,.25f)] float gizmoRadius;
        [SerializeField] Vector3 debugPointbyPos;
        [SerializeField] Color gizmoColor;
        [SerializeField] Color gizmoDebugColor;
        [SerializeField] bool enableGizmos;

        //Unity Events
        void Start()
        {
        }
        void Update()
        {
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
            
            Gizmos.color = gizmoDebugColor;
            
            Vector3 debugPoint;
            debugPoint = pointsList[GetIDByPoint(debugPointbyPos)];
            
            Gizmos.DrawCube(debugPoint, Vector3.one * gizmoRadius);
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
                        //Set position
                        x = i * pointDis - i * pointDis / 2;
                        y = j * pointDis - j * pointDis / 2;
                        z = k * pointDis - k * pointDis / 2;
                        points[i][j][k] = new Vec3(x, y, z);
                        
                        //Add to list
                        pointsList.Add(points[i][j][k]);
                    }
                }
            }
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
            return x * pointsPerLine * pointsPerLine + y * pointsPerLine + z;
        }
        Vec3 GetPointByID(int id)
        {
            if (id < 0) id = 0;
            else if(id >= pointsList.Count) id = pointsList.Count - 1;
            return pointsList[id];
        }
    }
}