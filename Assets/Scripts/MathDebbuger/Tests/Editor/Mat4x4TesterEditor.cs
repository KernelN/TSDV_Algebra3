using UnityEditor;
using UnityEngine;

namespace CustomMath
{
    [CustomEditor(typeof(Mat4x4Tester))]
    public class Mat4x4TesterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            Mat4x4Tester myScript = (Mat4x4Tester)target;
            
            //Title
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Results", EditorStyles.whiteLargeLabel);

            GUILayoutOption[] matOp =
            { GUILayout.MaxHeight(68.0f), GUILayout.MinHeight(50.0f) };

            //Matrix 4 x 4
            EditorGUILayout.LabelField("Matrix 4 x 4", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Mat: ", myScript.MatCalcM().ToString(), matOp);
            EditorGUILayout.LabelField("Matrix: ", myScript.MatrixCalcM().ToString(), matOp);
            EditorGUILayout.Separator();
            
            GUILayoutOption[] plOp =
            { GUILayout.MaxHeight(40.0f), GUILayout.MinHeight(10.0f) };
            
            //Plane
            EditorGUILayout.LabelField("Plane", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Mat: ", myScript.MatCalcCPlane().ToString(), plOp);
            EditorGUILayout.LabelField("Matrix: ", myScript.MatrixCalcCPlane().ToString(), plOp);
            EditorGUILayout.Separator();
            
            //Quaternion
            EditorGUILayout.LabelField("Quaternion", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Mat: ", myScript.MatCalcQ().ToString());
            EditorGUILayout.LabelField("Matrix: ", myScript.MatrixCalcQ().ToString());
            EditorGUILayout.Separator();
            
            //Vector of 4 components
            EditorGUILayout.LabelField("Vector of 4 components", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Mat: ", myScript.MatCalcVector4().ToString());
            EditorGUILayout.LabelField("Matrix: ", myScript.MatrixCalcVector4().ToString());
            EditorGUILayout.Separator();
            
            //Vector of 3 components
            EditorGUILayout.LabelField("Vector of 3 components", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Mat: ", myScript.MatCalcVec3().ToString());
            EditorGUILayout.LabelField("Matrix: ", myScript.MatrixCalcVec3().ToString());
            EditorGUILayout.Separator();
            
            //Float
            EditorGUILayout.LabelField("Float", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Mat: ", myScript.MatCalcFloat().ToString());
            EditorGUILayout.LabelField("Matrix: ", myScript.MatrixCalcFloat().ToString());
            EditorGUILayout.Separator();
            
            //Bool
            EditorGUILayout.LabelField("Bool", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Mat: ", myScript.MatCalcBool().ToString());
            EditorGUILayout.LabelField("Matrix: ", myScript.MatrixCalcBool().ToString());
        }
    }
}