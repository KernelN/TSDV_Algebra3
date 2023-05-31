using CustomMath.Collisions;
using UnityEditor;
using UnityEngine;

namespace CustomMath.Collision
{
    [CustomEditor(typeof(PointHolder))]
    public class PointHolderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            PointHolder myScript = (PointHolder)target;
            
            if(GUILayout.Button("Generate Points"))
            {
                myScript.GeneratePoints();
            }
        }
    }
}
