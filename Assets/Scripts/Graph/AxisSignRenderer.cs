#nullable enable
using UnityEngine;

namespace Assets.Scripts.Graph
{
    [ExecuteInEditMode]
    // ReSharper disable once UnusedMember.Global
    public class AxisSignRenderer : MonoBehaviour
    {
        public GameObject? XAxisSign;
        public GameObject? YAxisSign;

        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            RenderAxisSigns();
        }

        private void RenderAxisSigns()
        {
            if (XAxisSign == null) return;
            if (YAxisSign == null) return;

            XAxisSign.transform.localPosition = (transform.localScale.x + 0.35f) * Vector3.right;
            YAxisSign.transform.localPosition = (transform.localScale.y + 0.5f) * Vector3.up;
        }
    }
}
