#nullable enable
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.InGame
{
    [RequireComponent(typeof(ShadowCaster2D))]
    // ReSharper disable once UnusedMember.Global
    public class ShadowCasterCircleShapeMaker : MonoBehaviour
    {
        public int Sides = 15;
        public float Radius = 1f;

        public void BuildPath(int sides, float radius)
        {
            var points = new Vector3[sides];
            var angle = Mathf.Deg2Rad * 360 / sides;
            for (var i = 0; i < sides; i++)
            {
                points[i] = new Vector2(
                    radius * Mathf.Cos(angle * i + Mathf.PI / 2),
                    radius * Mathf.Sin(angle * i + Mathf.PI / 2)
                );
            }

            var field = typeof(ShadowCaster2D).GetField(
                "m_ShapePath",
                BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            field?.SetValue(GetComponent<ShadowCaster2D>(), points);
        }
    }
}