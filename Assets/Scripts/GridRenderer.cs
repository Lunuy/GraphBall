#nullable enable
using UnityEngine;

namespace Assets.Scripts
{
    public class GridRenderer : MonoBehaviour
    {
        // When added to an object, draws colored rays from the
        // transform position.
        public int LineCount = 100;
        public float Radius = 3.0f;

        private static Material? _lineMaterial;

        private static void CreateLineMaterial()
        {
            if (_lineMaterial) return;
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            _lineMaterial = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            // Turn on alpha blending
            _lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn back_face culling off
            _lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            _lineMaterial.SetInt("_ZWrite", 0);
        }

        // Will be called after all regular rendering is done
        // ReSharper disable once UnusedMember.Global
        public void OnRenderObject()
        {
            Debug.Log("A");
            CreateLineMaterial();
            // Apply the line material
            _lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.MultMatrix(transform.localToWorldMatrix);

            // Draw lines
            GL.Begin(GL.LINES);
            for (var i = 0; i < LineCount; ++i)
            {
                var a = i / (float)LineCount;
                var angle = a * Mathf.PI * 2;
                // Vertex colors change from red to green
                GL.Color(new Color(a, 1 - a, 0, 0.8F));
                // One vertex at transform position
                GL.Vertex3(0, 0, 0);
                // Another vertex at edge of circle
                GL.Vertex3(Mathf.Cos(angle) * Radius, Mathf.Sin(angle) * Radius, 0);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}
