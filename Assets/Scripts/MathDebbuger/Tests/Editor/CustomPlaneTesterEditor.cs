using UnityEditor;

namespace CustomMath
{
    [CustomEditor(typeof(CustomPlaneTester))]
    public class CustomPlaneTesterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            CustomPlaneTester myScript = (CustomPlaneTester)target;
            
            //Title
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Results");
            
            //Plane
            EditorGUILayout.LabelField("CustomPlane: ", myScript.CustomPlaneCalcP().ToString());
            EditorGUILayout.LabelField("Plane: ", myScript.PlaneCalcP().ToString());
            
            //Vector of 3 components
            EditorGUILayout.LabelField("CustomPlane: ", myScript.CustomPlaneCalcV3().ToString());
            EditorGUILayout.LabelField("Plane: ", myScript.PlaneCalcV3().ToString());
            
            //Float
            EditorGUILayout.LabelField("CustomPlane: ", myScript.CustomPlaneCalcFloat().ToString());
            EditorGUILayout.LabelField("Plane: ", myScript.PlaneCalcFloat().ToString());
            
            //Bool
            EditorGUILayout.LabelField("CustomPlane: ", myScript.CustomPlaneCalcBool().ToString());
            EditorGUILayout.LabelField("Plane: ", myScript.PlaneCalcBool().ToString());
        }
    }
}