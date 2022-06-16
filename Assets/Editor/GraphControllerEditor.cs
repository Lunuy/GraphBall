#nullable enable
using Assets.Scripts;
using UnityEditor;

namespace Assets.Editor
{
    [CustomEditor(typeof(GraphController))]
    [CanEditMultipleObjects]
    // ReSharper disable once UnusedMember.Global
    public class GraphControllerEditor : UnityEditor.Editor
    {
        private GraphController? _graphController;

        // ReSharper disable once UnusedMember.Local
        private void OnEnable()
        {
            _graphController = (GraphController) target;
        }

        // ReSharper disable once UnusedMember.Local
        public override void OnInspectorGUI()
        {
            {
                EditorGUI.BeginChangeCheck();
                var minX = EditorGUILayout.DoubleField("Min X", _graphController!.MinX);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_graphController, "change Min X");
                    _graphController.MinX = minX;
                    EditorUtility.SetDirty(_graphController);
                }
            }

            {
                EditorGUI.BeginChangeCheck();
                var maxX = EditorGUILayout.DoubleField("Max X", _graphController.MaxX);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_graphController, "change Max X");
                    _graphController.MaxX = maxX;
                    EditorUtility.SetDirty(_graphController);
                }
            }

            {
                EditorGUI.BeginChangeCheck();
                var minY = EditorGUILayout.DoubleField("Min Y", _graphController.MinY);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_graphController, "change Min Y");
                    _graphController.MinY = minY;
                    EditorUtility.SetDirty(_graphController);
                }
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.DoubleField("Max Y", _graphController.MaxY);
            EditorGUI.EndDisabledGroup();
            
            {
                EditorGUI.BeginChangeCheck();
                var step = EditorGUILayout.DoubleField("Step", _graphController.Step);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_graphController, "change Step");
                    _graphController.Step = step;
                    EditorUtility.SetDirty(_graphController);
                }
            }
        }
    }
}
