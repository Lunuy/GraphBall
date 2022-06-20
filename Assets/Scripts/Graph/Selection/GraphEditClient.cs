#nullable enable
using UnityEngine;
using System;
using Assets.Scripts.UI;

namespace Assets.Scripts.Graph.Selection
{
    public class GraphEditClient : MonoBehaviour
    {
        public delegate void ClickHandler(GraphEditClient graphEditClient);

        public event ClickHandler OnClick = delegate {  };

        public GraphEditManager? GraphEditManager;
        public EquationInputContext EquationInputContext = new EquationInputContext() { MaxCharacterCount = 30 };

        public void Start() {
            if(GraphEditManager == null) {
                throw new Exception("GraphEditManager is null");
            }

            GraphEditManager.AddClient(this);
        }

        public void Update() {

        }

        public void OnMouseDown() {
            OnClick(this);
        }
    }
}