#nullable enable
using Assets.Scripts;
using UnityEditor;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Assets.Editor
{
    [CustomEditor(typeof(GraphController))]
    [CanEditMultipleObjects]
    public class GraphControllerEditor : UnityEditor.Editor
    {
        // ReSharper disable once UnusedMember.Local
        public override void OnInspectorGUI()
        {
            var graphController = (GraphController) target;

            var minX = EditorGUILayout.DoubleField("Min X", graphController.MinX);
            var maxX = EditorGUILayout.DoubleField("Max X", graphController.MaxX);
            var minY = EditorGUILayout.DoubleField("Min Y", graphController.MinY);
            var step = EditorGUILayout.DoubleField("Step", graphController.Step);

            if (minX != graphController.MinX) graphController.MinX = minX;
            if (maxX != graphController.MaxX) graphController.MaxX = maxX;
            if (minY != graphController.MinY) graphController.MinY = minY;
            if (step != graphController.Step) graphController.Step = step;
        }
    }
}
