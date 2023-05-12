using UnityEditor;
using UnityEngine;

namespace CustomMath
{
    [CustomEditor(typeof(Vec3Tester))]
    public class Vec3TesterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            Vec3Tester myScript = (Vec3Tester)target;
            
            //Title
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Results");
            
            //Vector of 3 components
            EditorGUILayout.LabelField("Vec3: ", myScript.Vec3CalcV3().ToString());
            EditorGUILayout.LabelField("Vector3: ", myScript.Vector3CalcV3().ToString());
            
            //Float
            EditorGUILayout.LabelField("Vec3: ", myScript.Vec3CalcFloat().ToString());
            EditorGUILayout.LabelField("Vector3: ", myScript.Vector3CalcFloat().ToString());
        }
    }
}