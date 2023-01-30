using UnityEditor;
using UnityEngine;

namespace MergeSystem.Cells
{
    [CustomEditor(typeof(PointGenerator))]
    public class EditPointGenerator : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);

            PointGenerator pointGenerator = target as PointGenerator;

            if (GUILayout.Button("Generate Points"))
                pointGenerator.Awake();

            if (GUILayout.Button("Delete Points"))
                pointGenerator.DeletePoints();
        }
    }
}
