using UnityEditor;
using UnityEngine;

namespace CustomMath
{
    [CustomEditor(typeof(QuatTester))]
    public class QuatTesterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            QuatTester myScript = (QuatTester)target;
            
            //Title
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Results");
            
            //Vector of 3 components
            EditorGUILayout.LabelField("Quat: ", myScript.QuatCalcQ().ToString());
            EditorGUILayout.LabelField("Quaternion: ", myScript.QuaternionCalcQ().ToString());
            
            //Float
            EditorGUILayout.LabelField("Quat: ", myScript.QuatCalcFloat().ToString());
            EditorGUILayout.LabelField("Quaternion: ", myScript.QuaternionCalcFloat().ToString());
        }
    }
}