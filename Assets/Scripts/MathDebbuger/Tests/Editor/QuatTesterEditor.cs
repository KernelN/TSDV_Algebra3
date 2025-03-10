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
            EditorGUILayout.LabelField("Results", EditorStyles.whiteLargeLabel);
            
            //Quaternion
            EditorGUILayout.LabelField("Quat: ", myScript.QuatCalcQ().ToString());
            EditorGUILayout.LabelField("Quaternion: ", myScript.QuaternionCalcQ().ToString());
            
            //Vector of 3 components
            EditorGUILayout.LabelField("Quat: ", myScript.QuatCalcVec3().ToString());
            EditorGUILayout.LabelField("Quaternion: ", myScript.QuaternionCalcVec3().ToString());
            
            //Float
            EditorGUILayout.LabelField("Quat: ", myScript.QuatCalcFloat().ToString());
            EditorGUILayout.LabelField("Quaternion: ", myScript.QuaternionCalcFloat().ToString());
        }
    }
}