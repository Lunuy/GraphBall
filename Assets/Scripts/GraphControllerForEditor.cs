#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof (GraphController))]
    public class GraphControllerForEditor : MonoBehaviour
    {
        public double MinX = 0;
        public double MaxX = 10;
        public double MinY = 0;
        public double Step = 0.01;

        private GraphController? _graphController;

        // Start is called before the first frame update
        void Start()
        {
            _graphController = GetComponent<GraphController>();
        }

        // Update is called once per frame
        void Update()
        {
            _graphController!.MinX = MinX;
            _graphController!.MaxX = MaxX;
            _graphController!.MinY = MinY;
            _graphController!.Step = Step;
        }
    }
}
