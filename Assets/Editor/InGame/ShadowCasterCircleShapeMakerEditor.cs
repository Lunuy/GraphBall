#nullable enable
using Assets.Scripts.InGame;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.InGame
{
    [CustomEditor(typeof(ShadowCasterCircleShapeMaker))]
    [CanEditMultipleObjects]
    // ReSharper disable once UnusedMember.Global
    public class ShadowCasterCircleShapeMakerEditor : UnityEditor.Editor
    {
        private ShadowCasterCircleShapeMaker? _shadowCasterCircleShapeMaker;

        // ReSharper disable once UnusedMember.Local
        private void OnEnable()
        {
            _shadowCasterCircleShapeMaker = (ShadowCasterCircleShapeMaker) target;
        }

        public override void OnInspectorGUI()
        {
            {
                EditorGUI.BeginChangeCheck();
                var sides = EditorGUILayout.IntField("sides", _shadowCasterCircleShapeMaker!.Sides);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_shadowCasterCircleShapeMaker, "change sides");
                    _shadowCasterCircleShapeMaker.Sides = sides;
                    EditorUtility.SetDirty(_shadowCasterCircleShapeMaker);
                }
            }

            {
                EditorGUI.BeginChangeCheck();
                var radius = EditorGUILayout.FloatField("radius", _shadowCasterCircleShapeMaker!.Radius);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_shadowCasterCircleShapeMaker, "change radius");
                    _shadowCasterCircleShapeMaker.Radius = radius;
                    EditorUtility.SetDirty(_shadowCasterCircleShapeMaker);
                }
            }


            var buildShadowPath = GUILayout.Button("build shadow path");

            if (!buildShadowPath) return;
            if (_shadowCasterCircleShapeMaker != null)
            {
                _shadowCasterCircleShapeMaker.BuildPath(
                    _shadowCasterCircleShapeMaker.Sides,
                    _shadowCasterCircleShapeMaker.Radius);
            }
        }
    }
}
