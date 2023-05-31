using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CustomMath.Collisions
{
    public class PointHolder : MonoBehaviour
    {
        [Header("Set Values")]
        [SerializeField] int poinsPerLine;
        [SerializeField, Range(0, 1)] float pointDis;
        //[Header("Run Values")]
        Vec3[][][] points;
        List<Vec3> pointsList;

        [Header("DEBUGGER")]
        [SerializeField, Range(0,.25f)] float gizmoRadius;
        [SerializeField] Color gizmoColor;
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
            
            for (int i = 0; i < poinsPerLine; i++)
            {
                for (int j = 0; j < poinsPerLine; j++)
                {
                    for (int k = 0; k < poinsPerLine; k++)
                    {
                        Gizmos.DrawCube(points[i][j][k], Vector3.one * gizmoRadius);
                    }
                }
            }
        }

        //Methods
        public void GeneratePoints()
        {
            if (points == null)
            {
                GeneratePointArray();
            }
            
            float x, y, z;
            for (int i = 0; i < poinsPerLine; i++)
            {
                for (int j = 0; j < poinsPerLine; j++)
                {
                    for (int k = 0; k < poinsPerLine; k++)
                    {
                        x = i * pointDis - i * pointDis / 2;
                        y = j * pointDis - j * pointDis / 2;
                        z = k * pointDis - k * pointDis / 2;
                        points[i][j][k] = new Vec3(x, y, z);
                    }
                }
            }
        }
        void GeneratePointArray()
        {
            points = new Vec3[poinsPerLine][][];

            for (int i = 0; i < poinsPerLine; i++)
            {
                points[i] = new Vec3[poinsPerLine][];

                for (int j = 0; j < poinsPerLine; j++)
                {
                    points[i][j] = new Vec3[poinsPerLine];
                }
            }
        }
        int GetPointByPos(Vec3 pos)
        {
            return 0;
        }
    }
}