#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Graph
{
    public class AxisSignRenderer : MonoBehaviour
    {
        public GameObject? XAxisSign;
        public GameObject? YAxisSign;

        void Update()
        {
            RenderAxisSigns();
        }

        void RenderAxisSigns() {
            if(XAxisSign == null) return;
            if(YAxisSign == null) return;

            XAxisSign.transform.localPosition = (transform.localScale.x + 0.35f) * Vector3.right;
            YAxisSign.transform.localPosition = (transform.localScale.y + 0.5f) * Vector3.up;
        }
    }
}